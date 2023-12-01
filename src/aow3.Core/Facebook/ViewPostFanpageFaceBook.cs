using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aow.Facebook
{
    [Table("ViewPostFanpageFaceBook", Schema = "dbo")]
    public class ViewPostFanpageFaceBook : Entity<int>
    {
        public string Id { get; set; }
        public DateTime Created_time { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public string Message { get; set; }
        public string permalink_url { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Src { get; set; }
    }
}
