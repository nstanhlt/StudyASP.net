using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aow3.Models.Forum
{
    [Table("BaiDangForum", Schema = "fr")]
    public class BaiDangForum : FullAuditedEntity<int>
    {
        public string Content { get; set; }
        public string TieuDe { get; set; }
        public string ChuDe { get; set; }
        public string TTPD { get; set; }
    }
}

