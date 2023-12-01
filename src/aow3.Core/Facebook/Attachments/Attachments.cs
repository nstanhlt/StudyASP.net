using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aow.Facebook.Attachments
{
    [Table("Attachments", Schema = "fb")]
    public class Attachments : FullAuditedEntity<int>
    {
        public int Id { get; set; }
        public string IdPost { get; set; }
        public string IdAttachments { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Src { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
    }
}
