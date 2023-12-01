using Abp.Domain.Repositories;
using Abp.UI;
using Aow.VongQuayMayMans;
using aow3;
using aow3.Forums.DTO;
using aow3.Models.Forum;
using AowGold.VongQuayMayMans.DTO;
using NAWASCO.ERP.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aow3.Forums
{
    public class BaiDangForumAppServices : aow3AppServiceBase, IBaiDangForumAppServices
    {
        #region Khai bao Repository
        private readonly IRepository<BaiDangForum> _baiDangForumRepository;


        public BaiDangForumAppServices(
            IRepository<BaiDangForum> baiDangForumRepository
            )
        {
            _baiDangForumRepository = baiDangForumRepository;
        }
        #endregion

        public object GetAll(InputGetAllDto input)
        {
            var tasks = _baiDangForumRepository.GetAll();
            if (input.q != null)
            {
                {
                    tasks = tasks.Where(p =>
                    p.Content.ToLower().Contains(input.q.ToLower())
                );
                }
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
                                tasks = tasks.Where(p => p.Content.ToLower().Contains(q.ToLower()));
                                break;
                            case "ttpd":
                                var ttpd = (string)filter.Value;
                                if (ttpd != "")
                                {
                                    if (eq == "neq")
                                    {
                                        tasks = tasks.Where(p => p.TTPD != ttpd.ToUpper());
                                    }
                                    else
                                        tasks = tasks.Where(p => p.TTPD == ttpd.ToUpper());
                                }
                                break;

                        }
                }
            }
            tasks = tasks.OrderByDescending(d => d.CreationTime);
            var totalCount = tasks.Count();
            if (input.start.HasValue)
                tasks = tasks.Skip(input.start.Value);
            if (input.limit.HasValue)
                tasks = tasks.Take(input.limit.Value);
            var list = tasks.ToList();
            return new PagedResultTotalDto<object>(totalCount, list);
        }
        public bool CreateBaiDangForum(InputCreateBaiDangForum input)
        {
            BaiDangForum task = new BaiDangForum()
            {
                Content = input.Content,
                TieuDe = input.TieuDe,
                ChuDe = input.ChuDe,
                TTPD = input.TTPD
            };
            _baiDangForumRepository.Insert(task);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }
        public bool DeleteBaiDangForum(int idBaiDang)
        {
            //tasks = tasks.Where(d => d.CreatorUserId == AbpSession.UserId.Value

            var task = _baiDangForumRepository.FirstOrDefault(x => x.Id == idBaiDang);
            if (task == null)
            {
                throw new UserFriendlyException("Không tìm thấy bài đăng");
            }
            if (task.CreatorUserId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException("Bạn Không phải người tạo bài viết");
            }
            _baiDangForumRepository.Delete(task);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }
        public bool UpdateBaiDangForum(int idBaiDang, string content)
        {
            var task = _baiDangForumRepository.FirstOrDefault(x => x.Id == idBaiDang);
            if (task == null)
            {
                throw new UserFriendlyException("Không tìm thấy bài đăng");
            }
            //if (task.CreatorUserId != AbpSession.UserId.Value)
            //{
            //    throw new UserFriendlyException("Bạn Không phải người tạo bài viết");
            //}
            task.Content = content;
            task.TTPD = "TT_P";
            _baiDangForumRepository.Update(task);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }
        public bool PheDuyetBaiDangForum(int idBaiDang)
        {
            var task = _baiDangForumRepository.FirstOrDefault(x => x.Id == idBaiDang);
            if (task == null)
            {
                throw new UserFriendlyException("Không tìm thấy bài đăng");
            }
            //if (task.CreatorUserId != 1)
            //{
            //    throw new UserFriendlyException("Bạn không đủ quyền");
            //}
            task.TTPD = "TT_A";
            _baiDangForumRepository.Update(task);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }



    }
}
