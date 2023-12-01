using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aow3.VongQuayMayMans
{
    [Table("CacPhanThuongSpin", Schema = "spin")]
    public class CacPhanThuongSpin : Entity<int>
    {
        public int Id { get; set; }
        public string PhanThuong{ get; set; }
    }
}
