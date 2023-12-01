using System;
using System.Collections.Generic;
using System.Text;

namespace AowGold.FaceBooks.Dto
{
    public class ViewPostPageOutput
    {
        public string Id { get; set; }
        public DateTime Created_time { get; set; }
        public int? Ngay { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }
        public string Message { get; set; }
        public string? permalink_url { get; set; }
        public string idAttachment { get; set; }
        public string? Url { get; set; }
        public string Type { get; set; }
        public List<string> Srcs{ get; set; }
        public List<CommentsPostClass> comments { get; set; }
    }

    
 

    public class CommentsPostClass
    {
        public DateTime thoiGianComment { get; set; }
        public string nguoiCmt { get; set; }
        public string noiDungCmt { get; set; }
    }
}
