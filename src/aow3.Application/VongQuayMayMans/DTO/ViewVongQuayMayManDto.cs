using System;
using System.Collections.Generic;
using System.Text;

namespace AowGold.VongQuayMayMans.DTO
{
    public class ViewVongQuayMayManDto
    {
        public long Id { get; set; }
        public DateTime? Created_time { get; set; }
        public string TenKhachHang { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int TongSoLuotQuay { get; set; }
        public int SoLuotDaQuay { get; set; }
        public int SoLuotConLai { get; set; }
        public int PhanThuong { get; set; }
        public string TTPD { get; set; }
        public string? TTCK { get; set; }
    }
}
