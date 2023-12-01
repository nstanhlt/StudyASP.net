
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aow.Facebook.Comments
{
    [Table("Comments", Schema = "fb")]
    public class Comments : FullAuditedEntity<int>
    {
        public int Id { get; set; }
        public string IdComment { get; set; }
        public DateTime created_time { get; set; }
        public string message { get; set; }
        public string name { get; set; }
        public string idCreator { get; set; }
        [ForeignKey("IdPost")]
        public string IdPost { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public bool? isSticker { get; set; }
    }
}

