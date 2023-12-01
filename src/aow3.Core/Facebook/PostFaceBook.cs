using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aow.Facebook
{
    [Table("PostFaceBooks", Schema = "fb")]
    public class PostFaceBook : FullAuditedEntity<int>
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime Created_time { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public string? TypeName { get; set; }
        public string permalink_url { get; set; }
        [ForeignKey("idAttachment")]
        public string idAttachment { get; set; }
    }
}
