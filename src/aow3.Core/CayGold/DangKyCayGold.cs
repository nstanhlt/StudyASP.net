using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace aow3.CayGold
{
    [Table("DangKyCayGolds", Schema = "g")]
    public class DangKyCayGold : FullAuditedEntity<int>
    {
        public int Id { get; set; }
        public string TaiKhoanGoogle { get; set; }
        public string MatKhau { get; set; }
        public string Ma10So { get; set; }
        public string SoTienCayGold { get; set; }
        public string ResourceID { get; set; }
        public string? TTPD { get; set; }
        public string? soDienThoai { get; set; }
        public string? FB { get; set; }
        public string? Zalo { get; set; }
    }
}
