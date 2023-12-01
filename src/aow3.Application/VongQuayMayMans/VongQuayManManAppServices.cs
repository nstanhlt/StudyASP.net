using Abp.Domain.Repositories;
using Abp.UI;
using Aow.VongQuayMayMans;
using aow3;
using aow3.VongQuayMayMans;
using aow3.VongQuayMayMans.DTO;
using AowGold.VongQuayMayMans.DTO;
using Microsoft.AspNetCore.Mvc;
using NAWASCO.ERP.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AowGold.VongQuayMayMans
{
    public class VongQuayManManAppServices : aow3AppServiceBase, IVongQuayManManAppServices
    {
        #region Khai bao Repository
        private readonly IRepository<VongQuayMayMan> _vongQuayMayManRepository;
        private readonly IRepository<ViewVongQuayMayMan, string> _viewVongQuayMayManRepository;
        private readonly IRepository<PhanThuongVongQuay> _phanThuongVongQuayRepository;
        private readonly IRepository<CacPhanThuongSpin> _cacPhanThuongSpinRepository;

        public VongQuayManManAppServices(
            IRepository<VongQuayMayMan> vongQuayMayManRepository,
            IRepository<ViewVongQuayMayMan, string> viewVongQuayMayManRepository,
            IRepository<PhanThuongVongQuay> phanThuongVongQuayRepository,
            IRepository<CacPhanThuongSpin> cacPhanThuongSpinRepository
            )
        {
            _vongQuayMayManRepository = vongQuayMayManRepository;
            _viewVongQuayMayManRepository = viewVongQuayMayManRepository;
            _phanThuongVongQuayRepository = phanThuongVongQuayRepository;
            _cacPhanThuongSpinRepository = cacPhanThuongSpinRepository;
        }
        #endregion
        public VongQuayMayMan Get()
        {
            //CheckGetPermission();
            var task = _vongQuayMayManRepository.FirstOrDefault(p => p.UserId == AbpSession.UserId.Value);
            return task;
        }


        public PagedResultTotalDto<ViewVongQuayMayMan> GetAllViewVongQuayMayMan(InputGetAllDto input)
        {
            var tasks = _viewVongQuayMayManRepository.GetAll();
            if (input.q != null)
            {
                {
                    tasks = tasks.Where(p =>
                    p.TenKhachHang.ToLower().Contains(input.q.ToLower())
                    || p.PhanThuong.ToLower().Contains(input.q.ToLower())
                );
                }
            }

            if (input.filter != null)
            {
                var filters = JsonConvert.DeserializeObject<List<FilterDto>>(input.filter);

                foreach (var filter in filters)
                {
                    var eq = filter.Operator;

                    if (filter.Value != null)

                        switch (filter.Property.ToLower().Trim())
                        {
                            case "q":
                                var q = ((string)filter.Value).Trim();
                                tasks = tasks.Where(p => p.TenKhachHang.ToLower().Contains(q.ToLower()) || p.PhanThuong.ToLower().Contains(q.ToLower()));
                                break;
                            case "username":
                                var username = (string)filter.Value;
                                if (username != "")
                                {
                                    if (eq == "neq")
                                    {
                                        tasks = tasks.Where(p => p.UserName != username);
                                    }
                                    else
                                        tasks = tasks.Where(p => p.UserName == username);
                                }
                                break;
                            case "ttpd":
                                var ttpd = (string)filter.Value;
                                if (ttpd != "")
                                {
                                    if (eq == "neq")
                                    {
                                        tasks = tasks.Where(p => p.TTPD != ttpd.ToUpper());
                                    }
                                    else
                                        tasks = tasks.Where(p => p.TTPD == ttpd.ToUpper());
                                }
                                break;
                            case "phanthuong":
                                var phanthuong = (string)filter.Value;
                                if (phanthuong != "")
                                {
                                    if (eq == "neq")
                                    {
                                        tasks = tasks.Where(p => p.PhanThuong != phanthuong.ToUpper());
                                    }
                                    else
                                        tasks = tasks.Where(p => p.PhanThuong.Contains(phanthuong));
                                }
                                break;

                        }
                }
            }
            var totalCount = tasks.Count();
            if (input.start.HasValue)
                tasks = tasks.Skip(input.start.Value);
            if (input.limit.HasValue)
                tasks = tasks.Take(input.limit.Value);
            // tasks = tasks.OrderByDescending(d => d.Nam).ThenByDescending(d => d.Thang).ThenByDescending(d => d.Ngay);
            var list = tasks.ToList();
            return new PagedResultTotalDto<ViewVongQuayMayMan>(totalCount, list);
        }

        public bool AddUserQuayVongQuayMayMan(AddUserQuayVongQuayMayManInput input)
        {
            var tasks = _vongQuayMayManRepository.GetAll().FirstOrDefault(u => u.UserName == input.UserName);
            if (tasks != null)
            {
                tasks.TongSoLuotQuay = tasks.TongSoLuotQuay + input.SoLuotThem;
                tasks.SoLuotConLai = tasks.SoLuotConLai + input.SoLuotThem;
                _vongQuayMayManRepository.Update(tasks);
                CurrentUnitOfWork.SaveChanges();
            }
            else
            {
                //người mới
                VongQuayMayMan userQuayVongQuayMayMan = new VongQuayMayMan()
                {
                    UserId = AbpSession.UserId.Value,
                    Created_time = DateTime.Now,
                    UserName = input.UserName,
                    Ngay = DateTime.Now.Day,
                    Thang = DateTime.Now.Month,
                    Nam = DateTime.Now.Year,
                    TongSoLuotQuay = input.SoLuotThem,
                    SoLuotConLai = input.SoLuotThem,
                    SoLuotDaQuay = 0,
                    TTPD = "TT_A"
                };
                _vongQuayMayManRepository.Insert(userQuayVongQuayMayMan);
                CurrentUnitOfWork.SaveChanges();
            }
            return true;
        }

        public bool UpdateSauKhiQuay(UpdateSauKhQuayInputDto input)
        {
            var tasks = _vongQuayMayManRepository.GetAll().FirstOrDefault(u => u.UserId == input.UserId);
            if (tasks == null)
            {
                throw new UserFriendlyException("Không tìm thấy người dùng");
            }
            if (tasks.SoLuotConLai == 0)
            {
                throw new UserFriendlyException("Đã hết lượt quay");
            }
            if (tasks.TongSoLuotQuay < tasks.SoLuotDaQuay)
            {
                throw new UserFriendlyException("Đã có lỗi server");
            }
            //luot quay
            tasks.SoLuotDaQuay = tasks.SoLuotDaQuay + 1;
            tasks.SoLuotConLai = tasks.TongSoLuotQuay - tasks.SoLuotDaQuay;
            _vongQuayMayManRepository.Update(tasks);
            CurrentUnitOfWork.SaveChanges();
            //them vào phần thưởng
            PhanThuongVongQuay newPhanThuong = new PhanThuongVongQuay()
            {
                UserName = tasks.UserName,
                ThoiGianQuay = DateTime.Now,
                Ngay = DateTime.Now.Day,
                Thang = DateTime.Now.Month,
                Nam = DateTime.Now.Year,
                PhanThuong = input.PhanThuong
            };
            _phanThuongVongQuayRepository.Insert(newPhanThuong);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }
        public List<CacPhanThuongSpin> GetAllPhanThuuongVaoSpin()
        {
            return _cacPhanThuongSpinRepository.GetAll().ToList();
        }
        public bool AddPhanThuongVaoSpin(string phanThuong)
        {
            _cacPhanThuongSpinRepository.Insert(new CacPhanThuongSpin() { PhanThuong = phanThuong });
            return true;
        }
        public bool DeletePhanThuongVaoSpin(int id)
        {
            var task = _cacPhanThuongSpinRepository.FirstOrDefault(x => x.Id == id);
            _cacPhanThuongSpinRepository.Delete(task);
            return true;
        }
    }
}
