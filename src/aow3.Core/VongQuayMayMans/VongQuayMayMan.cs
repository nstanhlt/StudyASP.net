using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aow.VongQuayMayMans
{
    [Table("VongQuayMayMans", Schema = "spin")]
    public class VongQuayMayMan : FullAuditedEntity<int>
    {
        public int Id { get; set; }
        public DateTime Created_time { get; set; }
        public  string UserName { get; set; }
        public long UserId { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int TongSoLuotQuay { get; set; }
        public int SoLuotDaQuay { get; set; }
        public int SoLuotConLai { get; set; }
        public string? PhanThuong { get; set; }
        public string TTPD { get; set; }
        public string? TTCK { get; set; }
    }
}

