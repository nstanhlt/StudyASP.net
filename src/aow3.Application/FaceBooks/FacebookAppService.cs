using Abp.Domain.Repositories;
using Abp.UI;
using Aow.Facebook;
using Aow.Facebook.Attachments;
using Aow.Facebook.Comments;
using aow3;
using AowGold.FaceBooks.Dto;
using NAWASCO.ERP.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static AowGold.Users.Dto.FacebookApiCommentsPageResponseDTO;
using static AowGold.Users.Dto.PostAndCommentsPageFBDto;

namespace AowGold.FaceBooks
{
    public class FacebookAppService : aow3AppServiceBase, IFacebookService
    {
        private readonly IRepository<PostFaceBook> _postFaceBookRepository;
        private readonly IRepository<Attachments> _attachmentsRepository;
        private readonly IRepository<Comments> _commentsRepository;
        private readonly IRepository<ViewPostFanpageFaceBook> _viewPostFanpageFaceBookRepository;

        public FacebookAppService(
            IRepository<PostFaceBook> postFaceBookRepository,
            IRepository<Attachments> attachmentsRepository,
            IRepository<Comments> commentsRepository,
            IRepository<ViewPostFanpageFaceBook> viewPostFanpageFaceBookRepository
            )
        {
            _postFaceBookRepository = postFaceBookRepository;
            _attachmentsRepository = attachmentsRepository;
            _commentsRepository = commentsRepository;
            _viewPostFanpageFaceBookRepository = viewPostFanpageFaceBookRepository;

        }
        #region cookie, token
        static readonly string accessToken = "EAABsbCS1iHgBAGyWXSyZCAFKRs1g4pzZAZAsYlYZCQ7VoJm9xGnlwAUXZB9rSUip3pdFZAEZBPxjkoQDNLp8fjLSEFmtvZCubePc1nUzkFrhDs7m8UpTyp6N3mu8U2VhqSHQPmpdya7DdWQkWvcqnK3Mn0D5ioEOplml0DZAnlho8YwZDZD";
        static readonly string Cookies = "fr=00YPj81IgvoBpPdYC.AWWFndGJqq7ZBN1P6Hxx_k96H44.BkpT3F.kP.AAA.0.0.BkpT3F.AWXpJ49V1pc; sb=S5vXY1N-mzfYq_kgnUp0yhuG; datr=S5vXY0DvAj55o4kt39NJAPaC; c_user=100059067742398; xs=15:9JIjYHcdREdrzQ:2:1685246701:-1:8594::AcWokQGfJ8uVh422x86GJSbrNoQfa9L2RomLkCCzgbk";
        static readonly string accessToken2TA = "EAAGNO4a7r2wBAFKH6ipMOlMvk1WxIyP7czsJ0MuVKyVu2EvBZCBmq70ZBWGSOoSjZBFGISpFFU7Mpw0xYYI19mmOzntVJcDZBMnclDB9FigoBggeFF0ZBwXIebGxcFalFl7NVU5cYozu2gZBbz6DULMy8etTCWZCY19VFKoIAMqfogyqDPcNd0p";
        static readonly string Cookies2TA = "sb=TJCYYY7AOviBCFh5PIGnxxqx; datr=UpCYYVYt-e0s83X1eAgZaddz; c_user=100007488934074; dpr=1; xs=36%3AJfM5kWqgq5f_ag%3A2%3A1678861813%3A-1%3A6238%3A%3AAcXZ-Lcg59Sk6hx8XFYN8dVB8w6iUSakm3XIph_tcSME; fr=023CKrB7gQUsptxYx.AWUsoGos9AihtETc37cUsXp61QE.Bkinmr.qg.AAA.0.0.BkioQP.AWUMqL3h6tw; usida=eyJ2ZXIiOjEsImlkIjoiQXJ3OXlxNTRodXdzbyIsInRpbWUiOjE2ODY3OTkzNzd9; wd=1920x511";
        static HttpClientHandler CookieToContainer(string URL, string Cookies)
        {
            HttpClientHandler HttpClientHandler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            foreach (var item in Cookies.Split(";"))
            {
                if (item != "")
                {
                    string key = item.Split('=')[0];
                    string value = item.Split('=')[1];
                    HttpClientHandler.CookieContainer.Add(new Uri(URL), new Cookie(key.Trim(), value.Trim()));
                }
            }
            return HttpClientHandler;
        }
        #endregion

        public object GetAllFeedGroupsSaveDB(string groupId = "389090464593160", int limit = 5)
        {
            string URL = $"https://graph.facebook.com/v17.0/{groupId}/feed";
            string urlParameters = $"?fields=id,created_time,message,attachments" + $",link,coments&access_token={accessToken}&limit={limit.ToString()}";

            HttpClient client = new HttpClient(CookieToContainer(URL, Cookies));
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                var resultApi = JsonConvert.DeserializeObject<FacebookApiGetPostPage>(dataObjects);
                List<DataClass> dataFB = resultApi.data;
                var tasks = _postFaceBookRepository.GetAll();
                foreach (DataClass item in dataFB)
                {
                    var post = tasks.FirstOrDefault(p => p.Id == item.id);
                    if (post == null)
                    {
                        //thêm bảng post
                        PostFaceBook newPost = new PostFaceBook()
                        {
                            Id = item.id,
                            Message = item.message,
                            Created_time = item.created_time,
                            Ngay = item.created_time.Day,
                            Thang = item.created_time.Month,
                            Nam = item.created_time.Year,
                            permalink_url = item.link ?? item.permalink_url,
                            idAttachment = item.attachments != null ? item.attachments.data[0].target.id : null,
                        };
                        _postFaceBookRepository.Insert(newPost);
                        CurrentUnitOfWork.SaveChanges();


                        //Chia nhỏ các đính kèm
                        var subattachments = item.attachments.data[0].subattachments;
                        if (subattachments != null)
                        {
                            foreach (DataAchmentsClass subattachment in item.attachments.data[0].subattachments.data)
                            {
                                Attachments newAttachments = new Attachments()
                                {
                                    IdPost = item.id,
                                    IdAttachments = subattachment.target.id,
                                    Description = item.message,
                                    Type = subattachment.type,
                                    Url = subattachment.target.url,
                                    Src = subattachment.media.image.src,
                                    Ngay = item.created_time.Day,
                                    Thang = item.created_time.Month,
                                    Nam = item.created_time.Year
                                };
                                _attachmentsRepository.Insert(newAttachments);
                                CurrentUnitOfWork.SaveChanges();
                            }
                        }
                        else
                        {
                            //lưu đính kèm
                            Attachments newAttachments = new Attachments()
                            {
                                IdPost = item.id,
                                IdAttachments = item.attachments.data[0].target.id,
                                Description = item.message,
                                Type = item.attachments.data[0].type,
                                Url = item.attachments.data[0].url,
                                Src = item.attachments.data[0].media.image.src,
                                Ngay = item.created_time.Day,
                                Thang = item.created_time.Month,
                                Nam = item.created_time.Year
                            };
                            _attachmentsRepository.Insert(newAttachments);
                            CurrentUnitOfWork.SaveChanges();
                        }
                    }
                }
                client.Dispose();
                return resultApi;
            }
            else
            {
                //thử api và cookie2
                try
                {
                    urlParameters = $"?fields=id,created_time,message,attachments" + $",link,coments&access_token={accessToken2TA}&limit={limit.ToString()}";
                    HttpClient client2 = new HttpClient(CookieToContainer(URL, Cookies2TA));
                    client2.BaseAddress = new Uri(URL);
                    client2.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response2 = client2.GetAsync(urlParameters).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        var dataObjects = response2.Content.ReadAsStringAsync().Result;
                        var resultApi = JsonConvert.DeserializeObject<FacebookApiGetPostPage>(dataObjects);
                        List<DataClass> feedGroup = resultApi.data;
                        var tasks = _postFaceBookRepository.GetAll();
                        foreach (DataClass item in feedGroup)
                        {
                            DateTime created_time = (DateTime)item.created_time;
                            var post = tasks.FirstOrDefault(p => p.Id == item.id);
                            if (post == null)
                            {
                                //thêm bảng post
                                PostFaceBook newPost = new PostFaceBook()
                                {
                                    Id = item.id,
                                    Message = item.message,
                                    Created_time = created_time,
                                    Ngay = created_time.Day,
                                    Thang = created_time.Month,
                                    Nam = created_time.Year,
                                    permalink_url = item.link,
                                    idAttachment = item.attachments != null ? item.attachments.data[0].target.id : null,
                                };
                                _postFaceBookRepository.Insert(newPost);
                                CurrentUnitOfWork.SaveChanges();

                                //Chia nhỏ các đính kèm
                                if (item.attachments.data[0].subattachments != null)
                                {
                                    foreach (DataAchmentsClass subattachment in item.attachments.data[0].subattachments.data)
                                    {
                                        Attachments newAttachments = new Attachments()
                                        {
                                            IdPost = item.id,
                                            IdAttachments = subattachment.target.id,
                                            Description = item.message,
                                            Type = subattachment.type,
                                            Url = subattachment.target.url,
                                            Src = subattachment.media.image.src,
                                            Ngay = created_time.Day,
                                            Thang = created_time.Month,
                                            Nam = created_time.Year
                                        };
                                        _attachmentsRepository.Insert(newAttachments);
                                        CurrentUnitOfWork.SaveChanges();
                                    }
                                }
                                else
                                {
                                    //lưu đính kèm
                                    Attachments newAttachments = new Attachments()
                                    {
                                        IdPost = item.id,
                                        IdAttachments = item.attachments.data[0].target.id,
                                        Description = item.message,
                                        Type = item.attachments.data[0].type,
                                        Url = item.attachments.data[0].url,
                                        Src = item.attachments.data[0].media.image.src,
                                        Ngay = created_time.Day,
                                        Thang = created_time.Month,
                                        Nam = created_time.Year
                                    };
                                    _attachmentsRepository.Insert(newAttachments);
                                    CurrentUnitOfWork.SaveChanges();
                                }
                            }
                        }
                        client.Dispose();
                        return resultApi;
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response2.StatusCode, response2.Headers.ToString());
                        throw new UserFriendlyException("Lỗi Khi Call API get" + response2.Headers.ToString());
                    }
                }
                catch
                {

                }
                throw new UserFriendlyException("Lỗi Khi Call API get" + response.Headers.ToString());
            }
        }
        public object GetFeedFanPageSaveDB(string pageId = "588089934686794", int limit = 5)
        {
            string URL = $"https://graph.facebook.com/v17.0/{pageId}/feed";
            string urlParameters = $"?fields=id,created_time,attachments,permalink_url,message&access_token={accessToken}&limit={limit.ToString()}";

            HttpClient client = new HttpClient(CookieToContainer(URL, Cookies));
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                //lưu vào cơ sở dữ liệu
                var tasks = _postFaceBookRepository.GetAll();
                FacebookApiGetPostPage resultApi = JsonConvert.DeserializeObject<FacebookApiGetPostPage>(dataObjects);
                List<DataClass> feedPage = resultApi.data;
                foreach (DataClass item in feedPage)
                {
                    DateTime created_time = (DateTime)item.created_time;
                    var post = tasks.FirstOrDefault(p => p.Id == item.id);
                    if (post == null)
                    {
                        //thêm bảng post
                        PostFaceBook newPost = new PostFaceBook()
                        {
                            Id = item.id,
                            Message = item.message,
                            Created_time = created_time,
                            Ngay = created_time.Day,
                            Thang = created_time.Month,
                            Nam = created_time.Year,
                            permalink_url = item.link ?? item.permalink_url,
                            idAttachment = item.attachments != null ? item.attachments.data[0].target.id : null,
                        };
                        _postFaceBookRepository.Insert(newPost);
                        CurrentUnitOfWork.SaveChanges();

                        //Chia nhỏ các đính kèm
                        if (item.attachments.data[0].subattachments != null)
                        {
                            foreach (DataAchmentsClass subattachment in item.attachments.data[0].subattachments.data)
                            {
                                Attachments newAttachments = new Attachments()
                                {
                                    IdPost = item.id,
                                    IdAttachments = subattachment.target.id,
                                    Description = item.message,
                                    Type = subattachment.type,
                                    Url = subattachment.target.url,
                                    Src = subattachment.media.image.src,
                                    Ngay = created_time.Day,
                                    Thang = created_time.Month,
                                    Nam = created_time.Year
                                };
                                _attachmentsRepository.Insert(newAttachments);
                                CurrentUnitOfWork.SaveChanges();
                            }
                        }
                        else
                        {
                            //lưu đính kèm
                            Attachments newAttachments = new Attachments()
                            {
                                IdPost = item.id,
                                IdAttachments = item.attachments.data[0].target.id,
                                Description = item.message,
                                Type = item.attachments.data[0].type,
                                Url = item.attachments.data[0].url,
                                Src = item.attachments.data[0].media.image.src,
                                Ngay = created_time.Day,
                                Thang = created_time.Month,
                                Nam = created_time.Year
                            };
                            _attachmentsRepository.Insert(newAttachments);
                            CurrentUnitOfWork.SaveChanges();
                        }
                    }
                }
                client.Dispose();
                return resultApi;
            }
            else
            {
                try
                {
                    urlParameters = $"?fields=id,created_time,attachments,permalink_url,message&access_token={accessToken}&limit={limit.ToString()}";
                    HttpClient client2 = new HttpClient(CookieToContainer(URL, Cookies2TA));
                    client2.BaseAddress = new Uri(URL);
                    client2.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response2 = client.GetAsync(urlParameters).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var dataObjects = response2.Content.ReadAsStringAsync().Result;
                        var tasks = _postFaceBookRepository.GetAll();
                        FacebookApiGetPostPage resultApi = JsonConvert.DeserializeObject<FacebookApiGetPostPage>(dataObjects);
                        List<DataClass> feedPage = resultApi.data;
                        foreach (DataClass item in feedPage)
                        {
                            DateTime created_time = (DateTime)item.created_time;
                            var post = tasks.FirstOrDefault(p => p.Id == item.id);
                            if (post == null)
                            {
                                //thêm bảng post
                                PostFaceBook newPost = new PostFaceBook()
                                {
                                    Id = item.id,
                                    Message = item.message,
                                    Created_time = created_time,
                                    Ngay = created_time.Day,
                                    Thang = created_time.Month,
                                    Nam = created_time.Year,
                                    permalink_url = item.link ?? item.permalink_url,
                                    idAttachment = item.attachments != null ? item.attachments.data[0].target.id : null,
                                };
                                _postFaceBookRepository.Insert(newPost);
                                CurrentUnitOfWork.SaveChanges();

                                //Chia nhỏ các đính kèm
                                if (item.attachments.data[0].subattachments != null)
                                {
                                    foreach (DataAchmentsClass subattachment in item.attachments.data[0].subattachments.data)
                                    {
                                        Attachments newAttachments = new Attachments()
                                        {
                                            IdPost = item.id,
                                            IdAttachments = subattachment.target.id,
                                            Description = item.message,
                                            Type = subattachment.type,
                                            Url = subattachment.target.url,
                                            Src = subattachment.media.image.src,
                                            Ngay = created_time.Day,
                                            Thang = created_time.Month,
                                            Nam = created_time.Year
                                        };
                                        _attachmentsRepository.Insert(newAttachments);
                                        CurrentUnitOfWork.SaveChanges();
                                    }
                                }
                                else
                                {
                                    //lưu đính kèm
                                    Attachments newAttachments = new Attachments()
                                    {
                                        IdPost = item.id,
                                        IdAttachments = item.attachments.data[0].target.id,
                                        Description = item.message,
                                        Type = item.attachments.data[0].type,
                                        Url = item.attachments.data[0].url,
                                        Src = item.attachments.data[0].media.image.src,
                                        Ngay = created_time.Day,
                                        Thang = created_time.Month,
                                        Nam = created_time.Year
                                    };
                                    _attachmentsRepository.Insert(newAttachments);
                                    CurrentUnitOfWork.SaveChanges();
                                }
                            }
                        }
                        client.Dispose();
                        return resultApi;
                    }
                    else
                    {
                        throw new UserFriendlyException("Lỗi Khi Call API get:" + response2.StatusCode, response2.ReasonPhrase);
                    }
                }
                catch
                {

                }
                throw new UserFriendlyException("Lỗi Khi Call API get:" + response.StatusCode, response.Headers.ToString());
            }
        }
        public object GetPostFanPageSaveDB(string pageId = "588089934686794", int limit = 100)
        {
            string URL = $"https://graph.facebook.com/v17.0/{pageId}/posts";
            string urlParameters = $"?fields=full_picture,message,created_time,likes,attachments,permalink_url,comments&access_token={accessToken}&limit={limit.ToString()}";

            HttpClient client = new HttpClient(CookieToContainer(URL, Cookies));
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                var tasks = _postFaceBookRepository.GetAll();
                var returnValue = JsonConvert.DeserializeObject(dataObjects);

                PostAndCommentsFanPageFBDto resultApi = JsonConvert.DeserializeObject<PostAndCommentsFanPageFBDto>(dataObjects);
                List<DataClassPostPage> postPages = resultApi.data;

                foreach (DataClassPostPage postPage in postPages)
                {
                    var post = tasks.FirstOrDefault(p => p.Id == postPage.id);
                    if (post == null)
                    {
                        //thêm bảng post
                        PostFaceBook newPost = new PostFaceBook()
                        {
                            Id = postPage.id,
                            Message = postPage.message,
                            Created_time = postPage.created_time,
                            permalink_url = postPage.link ?? postPage.permalink_url,
                            idAttachment = postPage.attachments != null ? postPage.attachments.data[0].target.id : null,
                            //TypeName = postPage.__type__.name,
                            Ngay = postPage.created_time.Day,
                            Thang = postPage.created_time.Month,
                            Nam = postPage.created_time.Year,
                        };

                        _postFaceBookRepository.Insert(newPost);
                        CurrentUnitOfWork.SaveChanges();

                        //Chia nhỏ các đính kèm
                        if (postPage.attachments.data[0].subattachments != null)
                        {
                            foreach (DataAchmentsPostPage subattachment in postPage.attachments.data[0].subattachments.data)
                            {
                                Attachments newAttachments = new Attachments()
                                {
                                    IdPost = postPage.id,
                                    IdAttachments = subattachment.target.id,
                                    Description = postPage.message,
                                    Type = subattachment.type,
                                    Url = subattachment.target.url,
                                    Src = subattachment.media.image.src,
                                    Ngay = postPage.created_time.Day,
                                    Thang = postPage.created_time.Month,
                                    Nam = postPage.created_time.Year,
                                };
                                _attachmentsRepository.Insert(newAttachments);
                                CurrentUnitOfWork.SaveChanges();
                            }
                        }
                        else
                        {
                            //lưu đính kèm
                            Attachments newAttachments = new Attachments()
                            {
                                IdPost = postPage.id,
                                IdAttachments = postPage.attachments.data[0].target.id,
                                Description = postPage.message,
                                Type = postPage.attachments.data[0].type,
                                Url = postPage.attachments.data[0].url,
                                Src = postPage.attachments.data[0].media.image.src,
                            };
                            _attachmentsRepository.Insert(newAttachments);
                            CurrentUnitOfWork.SaveChanges();
                        }

                        if (postPage.comments != null)
                        {
                            foreach (DataCommentClass comment in postPage.comments.data)
                            {
                                Comments newComment = new Comments()
                                {
                                    IdComment = comment.id,
                                    created_time = comment.created_time,
                                    message = comment.message,
                                    name = comment.from.name,
                                    idCreator = comment.from.id,
                                    IdPost = postPage.id,
                                    Ngay = postPage.created_time.Day,
                                    Thang = postPage.created_time.Month,
                                    Nam = postPage.created_time.Year,
                                };
                                _commentsRepository.Insert(newComment);
                                CurrentUnitOfWork.SaveChanges();
                            }
                        }
                    }
                }
                client.Dispose();
                return returnValue;
            }
            else
            {
                try
                {
                    urlParameters = $"?fields=full_picture,message,created_time,likes,attachments,permalink_url,comments&access_token&access_token={accessToken2TA}&limit={limit.ToString()}";
                    HttpClient client2 = new HttpClient(CookieToContainer(URL, Cookies2TA));
                    client2.BaseAddress = new Uri(URL);
                    client2.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response2 = client.GetAsync(urlParameters).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var dataObjects = response2.Content.ReadAsStringAsync().Result;
                        var tasks = _postFaceBookRepository.GetAll();
                        FacebookApiGetPostPage resultApi = JsonConvert.DeserializeObject<FacebookApiGetPostPage>(dataObjects);
                        List<DataClass> feedPage = resultApi.data;
                        foreach (DataClass item in feedPage)
                        {
                            DateTime created_time = (DateTime)item.created_time;
                            var post = tasks.FirstOrDefault(p => p.Id == item.id);
                            if (post == null)
                            {
                                //thêm bảng post
                                PostFaceBook newPost = new PostFaceBook()
                                {
                                    Id = item.id,
                                    Message = item.message,
                                    Created_time = created_time,
                                    Ngay = created_time.Day,
                                    Thang = created_time.Month,
                                    Nam = created_time.Year,
                                    permalink_url = item.link ?? item.permalink_url,
                                    idAttachment = item.attachments != null ? item.attachments.data[0].target.id : null,
                                };
                                _postFaceBookRepository.Insert(newPost);
                                CurrentUnitOfWork.SaveChanges();

                                //Chia nhỏ các đính kèm
                                if (item.attachments.data[0].subattachments != null)
                                {
                                    foreach (DataAchmentsClass subattachment in item.attachments.data[0].subattachments.data)
                                    {
                                        Attachments newAttachments = new Attachments()
                                        {
                                            IdPost = item.id,
                                            IdAttachments = subattachment.target.id,
                                            Description = item.message,
                                            Type = subattachment.type,
                                            Url = subattachment.target.url,
                                            Src = subattachment.media.image.src,
                                            Ngay = created_time.Day,
                                            Thang = created_time.Month,
                                            Nam = created_time.Year
                                        };
                                        _attachmentsRepository.Insert(newAttachments);
                                        CurrentUnitOfWork.SaveChanges();
                                    }
                                }
                                else
                                {
                                    //lưu đính kèm
                                    Attachments newAttachments = new Attachments()
                                    {
                                        IdPost = item.id,
                                        IdAttachments = item.attachments.data[0].target.id,
                                        Description = item.message,
                                        Type = item.attachments.data[0].type,
                                        Url = item.attachments.data[0].url,
                                        Src = item.attachments.data[0].media.image.src,
                                        Ngay = created_time.Day,
                                        Thang = created_time.Month,
                                        Nam = created_time.Year
                                    };
                                    _attachmentsRepository.Insert(newAttachments);
                                    CurrentUnitOfWork.SaveChanges();
                                }
                            }
                        }
                        client.Dispose();
                        return resultApi;
                    }
                    else
                    {
                        throw new UserFriendlyException("Lỗi Khi Call API get:" + response2.StatusCode, response2.ReasonPhrase);
                    }
                }
                catch
                {

                }
                throw new UserFriendlyException("Lỗi Khi Call API get:" + response.StatusCode, response.Headers.ToString());
            }
        }

        public PagedResultTotalDto<ViewPostPageOutput> GetAllPostPage(InputGetAllDto input)
        {
            var posts = _viewPostFanpageFaceBookRepository.GetAll();
            if (input.filter != null)
            {
                var filters = JsonConvert.DeserializeObject<List<FilterDto>>(input.filter);
            }

            if (input.q != null)
            {
                posts = posts.Where(p =>
                      p.Id.ToLower().Contains(input.q.ToLower())
                    || p.Message.ToLower().Contains(input.q.ToLower())
                    || p.Type.ToLower().Contains(input.q.ToLower())

                );
            }

            if (input.filter != null)
            {
                var filters = JsonConvert.DeserializeObject<List<FilterDto>>(input.filter);

                foreach (var filter in filters)
                {
                    var eq = filter.Operator;

                    if (filter.Value != null)

                        switch (filter.Property.ToLower().Trim())
                        {

                            case "q":
                                var q = ((string)filter.Value).Trim();
                                posts = posts.Where(p =>
                                 p.Message.ToLower().Contains(input.q.ToLower())
                                 || p.Type.ToLower().Contains(input.q.ToLower())
                                  );

                                break;
                            case "type":
                                var type = (string)filter.Value;
                                if (type != "")
                                {
                                    if (eq == "neq")
                                    {
                                        posts = posts.Where(p => p.Type != type.ToUpper());
                                    }
                                    else if (eq == "eq")
                                    {
                                        posts = posts.Where(p => p.Type == type.ToUpper());
                                    }

                                }
                                break;
                        }
                }
            }


            var totalCount = posts.Count();
            if (input.start.HasValue)
                posts = posts.Skip(input.start.Value);
            if (input.limit.HasValue)
                posts = posts.Take(input.limit.Value);
            posts = posts.OrderByDescending(d => d.Created_time);
            var list = posts.ToList();
            return new PagedResultTotalDto<ViewPostPageOutput>(totalCount, (IReadOnlyList<ViewPostPageOutput>)list);
        }
        public PagedResultTotalDto<Comments> GetComment(string postId)
        {
            var tasks = _commentsRepository.GetAll().Where(c => c.IdPost == postId);

            var totalCount = tasks.Count();

            tasks = tasks.OrderByDescending(d => d.created_time);
            var list = tasks.ToList();
            return new PagedResultTotalDto<Comments>(totalCount, list);
        }
        public bool PostComment(string postId, string message)
        {
            string URL = $"https://graph.facebook.com/v17.0/{postId}/comments";

            var formData = new FormUrlEncodedContent(new[]
         {
                new KeyValuePair<string, string>("message", message)
            });


            HttpClient client = new HttpClient(CookieToContainer(URL, Cookies));
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsync(URL, formData).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new UserFriendlyException("Lỗi Khi Call API" + response.Headers.ToString());
            }
        }

        public List<dynamic> DeletePost(string pageId = "100064185061762", int limit = 100)
        {
            List<dynamic> listPost = new List<dynamic>();
            string accessToken = "EAAGNO4a7r2wBO404mGXFQi9PWHtmx6zk3st9SjkZB7pfm8gVifYBJywPiXPM0TJL07ngObVz2fEuavWfRztvMMrxmBM138S4ZAUdzrvgZBVoKi0CWMcb0Ct4J7bSbP5zX66xVgbOOB6AnmIy3HzDICkZBuBwtv2EWZAVQjJjT4rZC23UggKp9nXu9RpQZDZD";
            string Cookies = "sb=TJCYYY7AOviBCFh5PIGnxxqx; datr=UpCYYVYt-e0s83X1eAgZaddz; c_user=100007488934074; fbl_cs=AhCjiAcVFr8ySxfuR9a%2B1WwxGFFnY0pHRGVaaG09bTZ2TjJ3WXBsZElUZQ; fbl_ci=231743586477872; vpd=v1%3B740x360x4; fbl_st=100629066%3BT%3A28159716; wl_cbv=v2%3Bclient_version%3A2289%3Btimestamp%3A1689582995; cppo=1; m_page_voice=100064185061762; i_user=100064185061762; xs=36%3AJfM5kWqgq5f_ag%3A2%3A1678861813%3A-1%3A6238%3A%3AAcXuKuHZ84x5fWDiYHo2lIP_-tqQ6B3H8FaNlxnxHG6X; fr=0vPUgumH9ECqFF1fD.AWUS_z3RsiIx5pRa39Tca-D_ZKE.BkxIa2.qg.AAA.0.0.BkxIwH.AWVh2H3VFdo; wd=1920x261; usida=eyJ2ZXIiOjEsImlkIjoiQXJ5amNtOWU0ZG15OCIsInRpbWUiOjE2OTA2MDM4NTV9";
            string URL = $"https://graph.facebook.com/v13.0/{pageId}/feed";
            string urlParameters = $"?fields=id,created_time,message&limit={limit}&access_token={accessToken}";

            HttpClient client = new HttpClient(CookieToContainer(URL, Cookies));
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            var dataObjects = response.Content.ReadAsStringAsync().Result;
            dynamic returnValue = JsonConvert.DeserializeObject(dataObjects);
            var listId = returnValue.data;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string accessToken1 = "EAAGNO4a7r2wBOwtElVhG9Lxv2yr2YcaKxvI6vichz68yPsETpbFgmEZCjKrq3YyezFsN7gAbqYFmyDCNHtL5ykf4Hd93mWW1MTS9invS6rtSDXW9ObTsDRAyfW7NSf35c0VPIaozmkVmI1UYYsQCHAika55v8AAcCrKUBcZC2W3hvwTc86PeNGPYsWldXZCawZDZD";
                    foreach (var item in listId)
                    {
                        string id = item.id;
                        if (id == "1231014803667139_993081544996594" || id == "1231014803667139_663102162505947")
                        {

                        }
                        else
                        {
                            string apiUrl = $"/v13.0/{id}?access_token={accessToken1}";
                            string uri = "https://graph.facebook.com";
                            HttpClient clientDelete = new HttpClient(CookieToContainer(uri, Cookies));
                            clientDelete.BaseAddress = new Uri(uri);
                            HttpResponseMessage responseDelete = clientDelete.DeleteAsync(apiUrl).Result;
                            // Xử lý dữ liệu từ phản hồi ở đây...
                            Console.WriteLine(responseDelete);
                            Console.WriteLine("Bắt đầu xóa :" + item.message);
                            int delayMilliseconds = new Random().Next(300, 1000);
                            // Create a Task representing the delay
                            Task delayTask = Task.Delay(delayMilliseconds);
                            // Block the execution until the delay is completed
                            delayTask.Wait();
                            listPost.Add(item);
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);
                }

                return listPost;
            }
            return listPost;
        }
    }
}
