using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.Damage;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class DamageMvcMockingService : IDamageMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public DamageMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }
        public string RemoveDamage(Guid Damageid, Guid userId, string pass)
        {
            try
            {
                var check = _unitOfWork.Users.CheckUserByIdAndPass(userId, pass);
                if (!check)
                    return "WrongPass";

                Damage Hos = _unitOfWork.Damages.Get(Damageid);
                _unitOfWork.Damages.Remove(Hos);
                _unitOfWork.Complete();
                return OperationStatus.SUCCESSFUL.ToString();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity.ToString();
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong.ToString();
                }
            }
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Damage/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewDamage(DamageViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.CostTypeTxt) || string.IsNullOrWhiteSpace(viewModel.ReasonTxt))
                return "DataNotValid";

            Damage damage = ConvertModel(viewModel);

            var now = DateTime.Now;
            damage.CreateDate = now;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseDamageDate");
            if (!access)
            {
                damage.InvoiceDate = now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                damage.InvoiceDate = invoiceDate;
            }

            var cost_type = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CostTypeTxt, "CostType", viewModel.ClinicSectionId.Value);
            damage.CostTypeId = cost_type?.BaseInfos?.FirstOrDefault()?.Guid;
            if (damage.CostTypeId == null)
            {
                damage.CostType = new BaseInfo
                {
                    Name = viewModel.ReasonTxt,
                    ClinicSectionId = viewModel.ClinicSectionId,
                    TypeId = cost_type.Guid
                };

                _unitOfWork.BaseInfos.Add(damage.CostType);
            }

            var reason_type = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.ReasonTxt, "Reason", viewModel.ClinicSectionId.Value);
            damage.ReasonId = reason_type?.BaseInfos?.FirstOrDefault()?.Guid;
            if (damage.ReasonId == null)
            {
                damage.Reason = new BaseInfo
                {
                    Name = viewModel.ReasonTxt,
                    ClinicSectionId = viewModel.ClinicSectionId,
                    TypeId = reason_type.Guid
                };

                _unitOfWork.BaseInfos.Add(damage.Reason);
            }


            while (true)
            {
                try
                {
                    damage.InvoiceNum = GetDamageNum(viewModel.ClinicSectionId.Value);

                    _unitOfWork.Damages.Add(damage);
                    _unitOfWork.Complete();

                    break;
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("") && !(e.InnerException?.Message ?? "UQ_Damage_InvoiceNum_ClinicSectionId").Contains("UQ_Damage_InvoiceNum_ClinicSectionId"))
                        throw e;
                }
            }

            return $"{damage.Guid}_{damage.InvoiceNum}";
        }



        public string UpdateDamage(DamageViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.CostTypeTxt) || string.IsNullOrWhiteSpace(viewModel.ReasonTxt))
                return "DataNotValid";

            Damage damage = _unitOfWork.Damages.GetForUpdateTotalPrice(viewModel.Guid);

            damage.ModifiedDate = DateTime.Now;
            damage.ModifiedUserId = viewModel.ModifiedUserId;
            damage.Description = viewModel.Description;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseDamageDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now)
                //    return "DateNotValid";

                damage.InvoiceDate = invoiceDate;
            }

            var cost_type = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CostTypeTxt, "CostType", viewModel.ClinicSectionId.Value);
            damage.CostTypeId = cost_type?.BaseInfos?.FirstOrDefault()?.Guid;
            if (damage.CostTypeId == null)
            {
                damage.CostType = new BaseInfo
                {
                    Name = viewModel.ReasonTxt,
                    ClinicSectionId = viewModel.ClinicSectionId,
                    TypeId = cost_type.Guid
                };

                _unitOfWork.BaseInfos.Add(damage.CostType);
            }

            var reason_type = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.ReasonTxt, "Reason", viewModel.ClinicSectionId.Value);
            damage.ReasonId = reason_type?.BaseInfos?.FirstOrDefault()?.Guid;
            if (damage.ReasonId == null)
            {
                damage.Reason = new BaseInfo
                {
                    Name = viewModel.ReasonTxt,
                    ClinicSectionId = viewModel.ClinicSectionId,
                    TypeId = reason_type.Guid
                };

                _unitOfWork.BaseInfos.Add(damage.Reason);
            }

            _unitOfWork.Damages.UpdateState(damage);
            //_unitOfWork.Complete();

            //_unitOfWork.Damages.Detach(damage);

            return UpdateTotalPrice(damage);
        }

        public string UpdateTotalPrice(Damage invoice)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            List<DamageTotalPriceViewModel> prices = invoice.DamageDetails.Select(p => new DamageTotalPriceViewModel
            {
                Purchase = true,
                CurrencyName = p.CurrencyName,
                TotalDiscount = p.Discount.GetValueOrDefault(0),
                TotalPrice = p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0)
            }).ToList();

            prices.AddRange(invoice.DamageDiscounts.Select(p => new DamageTotalPriceViewModel
            {
                Purchase = false,
                CurrencyName = p.CurrencyName,
                TotalDiscount = p.Amount.GetValueOrDefault(0),
                TotalPrice = 0
            }).ToList());

            var total_price = string.Join("_", prices.Where(p => p.Purchase).GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.PriceAfterDiscount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList());

            var total_discount = string.Join("_", prices.Where(p => !p.Purchase).GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.TotalDiscount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList());

            var total_after_discount = string.Join("_", prices.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.PriceAfterDiscount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList());

            invoice.TotalPrice = total_after_discount;

            invoice.DamageDetails = null;
            invoice.DamageDiscounts = null;
            _unitOfWork.Damages.UpdateState(invoice);
            _unitOfWork.Complete();

            return $"{total_price}#{total_discount}#{total_after_discount}";
        }

        public string GetDamageNum(Guid clinicSectionId)
        {
            try
            {
                string damageNum = _unitOfWork.Damages.GetLatestDamageNum(clinicSectionId);
                return NextDamageNum(damageNum);
            }
            catch (Exception) { return "1"; }
        }

        public string NextDamageNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }

        public IEnumerable<DamageViewModel> GetAllDamages(Guid clinicSectionId, DamageFilterViewModel filterViewModel)
        {
            if (filterViewModel.PeriodId != (int)Periods.FromDateToDate)
            {
                var DateFrom = DateTime.Now;
                var DateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, filterViewModel.PeriodId);

                filterViewModel.DateFrom = DateFrom;
                filterViewModel.DateTo = DateTo;
            }

            IEnumerable<Damage> hosp = _unitOfWork.Damages.GetAllDamage(clinicSectionId, filterViewModel.DateFrom, filterViewModel.DateTo);

            CultureInfo cultures = new CultureInfo("en-US");
            List<DamageViewModel> hospconvert = hosp.Select(p => new DamageViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                TotalPrice = p.TotalPrice,
                CostTypeTxt = p.CostType.Name,
                ReasonTxt = p.Reason.Name,
                TotalDiscount = string.Join("_", p.DamageDiscounts.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList()),
            }).ToList();
            Indexing<DamageViewModel> indexing = new Indexing<DamageViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<DamageTotalPriceViewModel> GetAllTotalPrice(Guid damageId)
        {
            var details = _unitOfWork.DamageDetails.GetAllTotalPrice(damageId);

            var result = details.GroupBy(p => p.Currency.Name).Select(p => new DamageTotalPriceViewModel
            {
                CurrencyName = p.Key,
                TotalDiscount = p.Sum(x => x.Discount.GetValueOrDefault(0)),
                TotalPrice = p.Sum(x => x.Num.GetValueOrDefault(0) * x.Price)
            }).ToList();
            Indexing<DamageTotalPriceViewModel> indexing = new Indexing<DamageTotalPriceViewModel>();
            return indexing.AddIndexing(result);
        }

        public DamageViewModel GetDamage(Guid DamageId)
        {
            try
            {
                Damage damage = _unitOfWork.Damages.GetDamage(DamageId);
                var result = ConvertModel(damage);
                result.TotalPrice = UpdateTotalPrice(damage);

                return result;
            }
            catch { return null; }
        }

        // Begin Convert 
        public Damage ConvertModel(DamageViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DamageViewModel, Damage>().ForMember(a => a.DamageDetails, b => b.Ignore());

                //.ForMember(a => a.DamageTypeName, b => b.MapFrom(c => c.DamageType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<DamageViewModel, Damage>(Users);
        }

        public DamageViewModel ConvertModel(Damage Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Damage, DamageViewModel>()
                .ForMember(a => a.CostTypeTxt, b => b.MapFrom(c => c.CostType.Name))
                .ForMember(a => a.ReasonTxt, b => b.MapFrom(c => c.Reason.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Damage, DamageViewModel>(Users);
        }
    }
}
