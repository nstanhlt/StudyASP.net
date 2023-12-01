using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aow.VongQuayMayMans
{
    [Table("ViewVongQuayMayMans", Schema = "dbo")]
    public class ViewVongQuayMayMan : Entity<string>
    {
        public override string Id
        {
            get { return UserName + "-" + ThoiGianQuay; }
            set { /* nothing */ }
        }
        public string TenKhachHang { get; set; }
        public string UserName { get; set; }
        public DateTime? ThoiGianCapNhatSpin { get; set; }
        public DateTime? ThoiGianTaoSpin { get; set; }
    
        public DateTime? ThoiGianQuay { get; set; }
        public int? Ngay { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }

        public string? PhanThuong { get; set; }

        public int TongSoLuotQuay { get; set; }
        public int SoLuotDaQuay { get; set; }
        public int SoLuotConLai { get; set; }

        public string? TTPD { get; set; }
        public string? TTCK { get; set; }
    }
}