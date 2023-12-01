using Abp.Domain.Repositories;
using aow3.GOLD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aow3.Gold
{
    public class GoldAppservices : aow3AppServiceBase, IGoldAppservices
    {
        private readonly IRepository<GiaGold> _giaGoldRepository;

        public GoldAppservices(IRepository<GiaGold> giaGoldRepository)
        {
            _giaGoldRepository = giaGoldRepository;
        }
        public List<GiaGold> GetAll()
        {
            return _giaGoldRepository.GetAll().ToList();
        }
        public bool Add(int gia)
        {
            _giaGoldRepository.Insert(new GiaGold() { Gia = gia });
            return true;
        }
        public bool Delete(int id)
        {
            var task = _giaGoldRepository.FirstOrDefault(x => x.Id == id);
            _giaGoldRepository.Delete(task);
            return true;
        }
    }
}
