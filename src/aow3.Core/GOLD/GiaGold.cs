

using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace aow3.GOLD
{
    [Table("GiaGolds", Schema = "gold")]
    public class GiaGold : Entity<int>
    {
        public int Id { get; set; }
        public int Gia{ get; set; }
    }
}
