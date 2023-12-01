using AowGold.VongQuayMayMans.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace AowGold.VongQuayMayMans.DTO
{
    public class ViewVongQuayMayManOutput
    {
        public string UserName { get; set; }
        public string TenKhachHang { get; set; }
        public int idKhachCayGold { get; set; }

        public DateTime LastModificationTime { get; set; }
        public DateTime ThoiGianTaoUser { get; set; }
        public List<string>? PhanThuongs { get; set; }
        public DateTime ThoiGianQuay { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int TongSoLuotQuay { get; set; }
        public int SoLuotDaQuay { get; set; }
        public int SoLuotConLai { get; set; }

        public string? TTPD { get; set; }
        public string? TTCK { get; set; }

    }
}

