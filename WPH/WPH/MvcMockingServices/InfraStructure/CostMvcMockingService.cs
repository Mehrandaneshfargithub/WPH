using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Cost;
using WPH.Models.CustomDataModels.CostReport;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class CostMvcMockingService : ICostMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public CostMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public IEnumerable<CostViewModel> GetAllCosts(Guid clinicSectionId, Guid costtypeId, int periodId, DateTime DateFrom, DateTime DateTo)
        {
            List<Cost> costDtos;
            if (periodId == (int)Periods.FromDateToDate)
            {
                costDtos = _unitOfWork.Costs.GetAllCosts(clinicSectionId, DateFrom, DateTo, costtypeId).ToList();
            }
            else
            {
                DateTime dateFrom = DateTime.Now;
                DateTime dateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref dateFrom, ref dateTo, periodId);
                costDtos = _unitOfWork.Costs.GetAllCosts(clinicSectionId, dateFrom, dateTo, costtypeId).ToList();
            }

            List<CostViewModel> costs = ConvertModelsLists(costDtos).ToList();
            Indexing<CostViewModel> indexing = new Indexing<CostViewModel>();
            return indexing.AddIndexing(costs);
        }

        public IEnumerable<CostViewModel> GetAllPurchasInvoiceCosts(Guid purchaseInvoiceId)
        {
            var costDtos = _unitOfWork.Costs.GetAllPurchasInvoiceCosts(purchaseInvoiceId).ToList();

            List<CostViewModel> costs = ConvertPurchaseModelsLists(costDtos).ToList();
            Indexing<CostViewModel> indexing = new Indexing<CostViewModel>();
            return indexing.AddIndexing(costs);
        }

        public IEnumerable<CostViewModel> GetAllSaleInvoiceCosts(Guid saleInvoiceId)
        {
            var costDtos = _unitOfWork.Costs.GetAllSaleInvoiceCosts(saleInvoiceId);

            List<CostViewModel> costs = ConvertPurchaseModelsLists(costDtos);
            Indexing<CostViewModel> indexing = new Indexing<CostViewModel>();
            return indexing.AddIndexing(costs);
        }



        public CostReportViewModel GetAllCostsByDateRange(List<Guid> clinicSectionIds, Guid? costTypeId, DateTime DateFrom, DateTime DateTo, bool Detail)
        {
            List<Cost> costDtos = new List<Cost>();

            costDtos = _unitOfWork.Costs.GetAllCostsForReport(clinicSectionIds, DateFrom, DateTo);

            CostReportViewModel costs = new CostReportViewModel
            {
                AllCost = new List<CostReportViewModel>()
            };

            if (Detail)
            {
                foreach (var time in costDtos)
                {

                    var amount = time.Price;

                    if (amount != 0)
                    {
                        costs.AllCost.Add(new CostReportViewModel { Amount = amount.GetValueOrDefault(0).ToString("N0"), Date = time.CostDate.GetValueOrDefault().Date, ClinicSectionName = time.ClinicSection.Name, CostTypeName = time.CostType.Name });
                    }
                }

                costs.AllClinicSectionTypeCostTotal = new List<CostReportViewModel>();
                List<string> allSections = costDtos.Select(x => x.ClinicSection.Name).Distinct().ToList();
                List<string> allTypes = costDtos.Select(x => x.CostType.Name).Distinct().ToList();

                foreach (var section in allSections)
                {
                    foreach (var type in allTypes)
                    {
                        var amount = costDtos.Where(x => x.ClinicSection.Name == section && x.CostType.Name == type).Sum(a => a.Price);

                        if (amount != 0)
                        {
                            costs.AllClinicSectionTypeCostTotal.Add(new CostReportViewModel { Amount = amount.GetValueOrDefault(0).ToString("N0"), ClinicSectionName = section, CostTypeName = type });
                        }
                    }

                }

                costs.AllSectionsTotal = new List<CostReportViewModel>();

                foreach (var section in allSections)
                {
                    costs.AllSectionsTotal.Add(new CostReportViewModel { Amount = costDtos.Where(x => x.ClinicSection.Name == section).Sum(a => a.Price).GetValueOrDefault().ToString("N0"), ClinicSectionName = section });
                }

                costs.AllTypeTotal = new List<CostReportViewModel>();

                foreach (var type in allTypes)
                {
                    costs.AllTypeTotal.Add(new CostReportViewModel { Amount = costDtos.Where(x => x.CostType.Name == type).Sum(a => a.Price).GetValueOrDefault().ToString("N0"), CostTypeName = type });
                }
            }
            else
            {
                IEnumerable<DateTime> allTime = costDtos.Select(x => x.CostDate.GetValueOrDefault().Date).Distinct();
                List<string> allSections = costDtos.Select(x => x.ClinicSection.Name).Distinct().ToList();
                List<string> allTypes = costDtos.Select(x => x.CostType.Name).Distinct().ToList();

                foreach (var section in allSections)
                {
                    foreach (var type in allTypes)
                    {
                        foreach (var time in allTime)
                        {
                            var f = time.Date;
                            var amount = costDtos.Where(x => x.CostDate.GetValueOrDefault().Date == time.Date && x.ClinicSection.Name == section && x.CostType.Name == type).Sum(a => a.Price);

                            if (amount != 0)
                            {
                                costs.AllCost.Add(new CostReportViewModel { Amount = amount.GetValueOrDefault(0).ToString("N0"), Date = time.Date, ClinicSectionName = section, CostTypeName = type });
                            }
                        }
                    }

                }

                costs.AllClinicSectionTypeCostTotal = new List<CostReportViewModel>();

                foreach (var section in allSections)
                {
                    foreach (var type in allTypes)
                    {
                        var amount = costDtos.Where(x => x.ClinicSection.Name == section && x.CostType.Name == type).Sum(a => a.Price);

                        if (amount != 0)
                        {
                            costs.AllClinicSectionTypeCostTotal.Add(new CostReportViewModel { Amount = amount.GetValueOrDefault(0).ToString("N0"), ClinicSectionName = section, CostTypeName = type });
                        }
                    }

                }

                costs.AllSectionsTotal = new List<CostReportViewModel>();

                foreach (var section in allSections)
                {
                    costs.AllSectionsTotal.Add(new CostReportViewModel { Amount = costDtos.Where(x => x.ClinicSection.Name == section).Sum(a => a.Price).GetValueOrDefault().ToString("N0"), ClinicSectionName = section });
                }

                costs.AllTypeTotal = new List<CostReportViewModel>();

                foreach (var type in allTypes)
                {
                    costs.AllTypeTotal.Add(new CostReportViewModel { Amount = costDtos.Where(x => x.CostType.Name == type).Sum(a => a.Price).GetValueOrDefault().ToString("N0"), CostTypeName = type });
                }

            }
            costs.Total = costDtos.Sum(x => x.Price).GetValueOrDefault().ToString("N0");

            return costs;

        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Cost/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewCost(CostViewModel cost)
        {
            if (string.IsNullOrWhiteSpace(cost.CostTypeName) || cost.Price.GetValueOrDefault(0) <= 0 ||
                cost.ClinicSectionId == null || cost.ClinicSectionId == Guid.Empty)
                return "DataNotValid";

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(cost.CostTypeName, "CostType", cost.OriginalClinicSectionId.Value);
            cost.CostTypeId = typeId;
            Cost costt = Common.ConvertModels<Cost, CostViewModel>.convertModels(cost);

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = cost.CostTypeName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("CostType"),
                    ClinicSectionId = cost.OriginalClinicSectionId
                };

                costt.CostType = baseInfo;
            }


            _unitOfWork.Costs.Add(costt);
            _unitOfWork.Complete();
            return costt.Guid.ToString();
        }

        public string AddPurchaseInvoiceCost(CostViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Price = decimal.Parse(viewModel.PriceTxt ?? "0", cultures);

            if (viewModel.Price.GetValueOrDefault(0) <= 0 || viewModel.PurchaseInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(viewModel.PurchaseInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType("PurchaseInvoiceCost", "CostType", null);
            viewModel.CostTypeId = typeId;
            Cost cost = Common.ConvertModels<Cost, CostViewModel>.convertModels(viewModel);

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = "PurchaseInvoiceCost",
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("CostType"),
                    ClinicSectionId = null
                };

                cost.CostType = baseInfo;
            }

            _unitOfWork.Costs.Add(cost);
            //_unitOfWork.Complete();

            var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(viewModel.PurchaseInvoiceId.Value);
            cost.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(cost.CurrencyId.Value);
            currencyExist.Costs.Add(cost);

            return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
        }

        public string AddSaleInvoiceCost(CostViewModel viewModel)
        {
            if (viewModel.Price.GetValueOrDefault(0) <= 0 || viewModel.SaleInvoiceId == null)
                return "DataNotValid";

            //var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoiceInUse(viewModel.PurchaseInvoiceId.Value);
            //if (can_change)
            //    return "InvoiceInUse";

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.CostTypeName, "CostType", viewModel.ClinicSectionId);
            viewModel.CostTypeId = typeId;
            Cost cost = Common.ConvertModels<Cost, CostViewModel>.convertModels(viewModel);

            //cost.InvoiceNum +=  "InvoiceNum: " + viewModel.InvoiceNum.ToString();

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = viewModel.CostTypeName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("CostType"),
                    ClinicSectionId = viewModel.ClinicSectionId
                };

                cost.CostType = baseInfo;
            }

            _unitOfWork.Costs.Add(cost);
            _unitOfWork.Complete();
            return cost.Guid.ToString();
        }

        public string UpdateCost(CostViewModel cost)
        {
            if (string.IsNullOrWhiteSpace(cost.CostTypeName) || cost.Price.GetValueOrDefault(0) <= 0 ||
                cost.ClinicSectionId == null || cost.ClinicSectionId == Guid.Empty)
                return "DataNotValid";

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(cost.CostTypeName, "CostType", cost.ClinicSectionId.Value);
            cost.CostTypeId = typeId;
            Cost costt = Common.ConvertModels<Cost, CostViewModel>.convertModels(cost);

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = cost.CostTypeName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("CostType"),
                    ClinicSectionId = cost.ClinicSectionId
                };

                _unitOfWork.BaseInfos.Add(baseInfo);

                costt.CostType = baseInfo;
            }


            _unitOfWork.Costs.UpdateState(costt);
            _unitOfWork.Complete();
            return costt.Guid.ToString();
        }

        public string UpdatePurchaseInvoiceCost(CostViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Price = decimal.Parse(viewModel.PriceTxt ?? "0", cultures);

            if (viewModel.Price.GetValueOrDefault(0) <= 0 || viewModel.PurchaseInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(viewModel.PurchaseInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var cost = _unitOfWork.Costs.Get(viewModel.Guid);
            cost.Price = viewModel.Price;
            cost.CurrencyId = viewModel.CurrencyId;
            cost.Explanation = viewModel.Explanation;

            _unitOfWork.Costs.UpdateState(cost);

            var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(viewModel.PurchaseInvoiceId.Value);
            currencyExist.Costs = currencyExist.Costs.Where(p => p.Guid != viewModel.Guid).ToList();
            cost.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(cost.CurrencyId.Value);
            currencyExist.Costs.Add(cost);

            return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
        }

        public string UpdateSaleInvoiceCost(CostViewModel viewModel)
        {
            if (viewModel.Price.GetValueOrDefault(0) <= 0 || viewModel.SaleInvoiceId == null)
                return "DataNotValid";

            //var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoiceInUse(viewModel.PurchaseInvoiceId.Value);
            //if (can_change)
            //    return "InvoiceInUse";

            var cost = _unitOfWork.Costs.Get(viewModel.Guid);
            cost.Price = viewModel.Price;
            cost.CurrencyId = viewModel.CurrencyId;
            cost.Explanation = viewModel.Explanation;

            _unitOfWork.Costs.UpdateState(cost);
            _unitOfWork.Complete();
            return cost.Guid.ToString();
        }

        public string PurchasInvoiceCostRemove(Guid costId)
        {
            try
            {
                Cost cost = _unitOfWork.Costs.Get(costId);

                if (cost.PurchaseInvoiceId != null)
                {
                    var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(cost.PurchaseInvoiceId.Value);
                    if (can_change)
                        return "InvoiceInUse";
                }

                _unitOfWork.Costs.Remove(cost);

                var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(cost.PurchaseInvoiceId.Value);
                currencyExist.Costs = currencyExist.Costs.Where(p => p.Guid != costId).ToList();

                return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
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
        
        public string RemoveCost(Guid costId)
        {
            try
            {
                Cost cost = _unitOfWork.Costs.Get(costId);

                if (cost.PurchaseInvoiceId != null)
                {
                    var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(cost.PurchaseInvoiceId.Value);
                    if (can_change)
                        return "InvoiceInUse";
                }

                _unitOfWork.Costs.Remove(cost);
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

        public CostViewModel GetWithType(Guid costId)
        {
            try
            {
                Cost costDto = _unitOfWork.Costs.GetWithType(costId);
                return ConvertModels(costDto);
            }
            catch { return null; }
        }

        public CostViewModel GetCost(Guid costId)
        {
            try
            {
                Cost costDto = _unitOfWork.Costs.Get(costId);
                return ConvertPurchaseModels(costDto);
            }
            catch { return null; }
        }


        public CostViewModel ConvertPurchaseModels(Cost cost)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            CostViewModel costView = new CostViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cost, CostViewModel>()
                .ForMember(a => a.PriceTxt, b => b.MapFrom(c => c.Price.GetValueOrDefault(0).ToString("0.##", cultures)))
                ;
            });
            IMapper mapper = config.CreateMapper();
            costView = mapper.Map<Cost, CostViewModel>(cost);
            return costView;
        }

        public CostViewModel ConvertModels(Cost cost)
        {
            CostViewModel costView = new CostViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<Cost, CostViewModel>()
                .ForMember(a => a.CostTypeName, b => b.MapFrom(c => c.CostType.Name))
                ;
            });
            IMapper mapper = config.CreateMapper();
            costView = mapper.Map<Cost, CostViewModel>(cost);
            return costView;
        }

        public static List<CostViewModel> ConvertModelsLists(IEnumerable<Cost> costs)
        {
            List<CostViewModel> CostViewModelList = new List<CostViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<Cost, CostViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                .ForMember(a => a.Explanation, b => b.MapFrom(c => $"{c.Explanation} -- { c.PurchaseInvoice.InvoiceNum ?? c.SaleInvoice.InvoiceNum ?? ""}"));

            });
            IMapper mapper = config.CreateMapper();
            CostViewModelList = mapper.Map<IEnumerable<Cost>, List<CostViewModel>>(costs);
            return CostViewModelList;
        }

        public static List<CostViewModel> ConvertPurchaseModelsLists(IEnumerable<Cost> costs)
        {
            List<CostViewModel> CostViewModelList = new List<CostViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<Cost, CostViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                ;

            });
            IMapper mapper = config.CreateMapper();
            CostViewModelList = mapper.Map<IEnumerable<Cost>, List<CostViewModel>>(costs);
            return CostViewModelList;
        }
    }
}
