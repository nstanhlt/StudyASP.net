using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Aow.VongQuayMayMans
{
    [Table("PhanThuongVongQuay", Schema = "spin")]
    public class PhanThuongVongQuay : FullAuditedEntity<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime ThoiGianQuay { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public string PhanThuong { get; set; }
    }
}
