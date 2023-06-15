using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices.Interface;
using static Common.Enums;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class BaseInfoMvcMockingService : IBaseInfoMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseInfoMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public IEnumerable<BaseInfoGeneralViewModel> GetAllBaseInfoGenerals(string baseinfoType)
        {
            int baseInfoGeneralTypeId = _unitOfWork.BaseInfoGenerals.GetBaseInfoGeneralType(x => x.Ename == baseinfoType).Id;
            IEnumerable<BaseInfoGeneral> baseinfos = _unitOfWork.BaseInfoGenerals.Find(x => x.TypeId == baseInfoGeneralTypeId).OrderBy(a => a.Priority);
            return Common.ConvertModels<BaseInfoGeneralViewModel, BaseInfoGeneral>.convertModelsLists(baseinfos);
        }

        public IEnumerable<BaseInfoGeneralViewModel> GetAllBaseInfoGeneralsExcept(string baseinfoType, string exceptName)
        {
            int baseInfoGeneralTypeId = _unitOfWork.BaseInfoGenerals.GetBaseInfoGeneralType(x => x.Ename == baseinfoType).Id;
            IEnumerable<BaseInfoGeneral> baseinfos = _unitOfWork.BaseInfoGenerals.Find(x => x.TypeId == baseInfoGeneralTypeId && x.Name != exceptName);
            return Common.ConvertModels<BaseInfoGeneralViewModel, BaseInfoGeneral>.convertModelsLists(baseinfos);
        }

        public IEnumerable<BaseInfoViewModel> GetAllBaseInfos(string baseinfoType, Guid clinicSectionId)
        {
            Guid baseInfoTypeId = _unitOfWork.BaseInfos.GetBaseInfoType(x => x.Ename == baseinfoType).Guid;
            IEnumerable<BaseInfo> baseinfos = _unitOfWork.BaseInfos.Find(x => x.TypeId == baseInfoTypeId && x.ClinicSectionId == clinicSectionId);
            List<BaseInfoViewModel> baseInfoViews = Common.ConvertModels<BaseInfoViewModel, BaseInfo>.convertModelsLists(baseinfos);
            Indexing<BaseInfoViewModel> indexing = new Indexing<BaseInfoViewModel>();
            return indexing.AddIndexing(baseInfoViews);
        }

        public IEnumerable<BaseInfoViewModel> GetAllBaseInfos(string baseinfoType)
        {
            Guid baseInfoTypeId = _unitOfWork.BaseInfos.GetBaseInfoType(x => x.Ename == baseinfoType).Guid;
            IEnumerable<BaseInfo> baseinfos = _unitOfWork.BaseInfos.Find(x => x.TypeId == baseInfoTypeId);
            List<BaseInfoViewModel> baseInfoViews = Common.ConvertModels<BaseInfoViewModel, BaseInfo>.convertModelsLists(baseinfos);
            Indexing<BaseInfoViewModel> indexing = new Indexing<BaseInfoViewModel>();
            return indexing.AddIndexing(baseInfoViews);
        }

        public int GetBaseInfoGeneralByName(string baseinfoGeneralName)
        {
            try
            {
                return _unitOfWork.BaseInfoGenerals.GetSingle(x => x.Name == baseinfoGeneralName).Id;

            }
            catch { return 0; }

        }


        public IEnumerable<BaseInfoGeneralViewModel> GetCustomInvoiceDetailTypes(string baseinfoType)
        {
            int baseInfoGeneralTypeId = _unitOfWork.BaseInfoGenerals.GetBaseInfoGeneralType(x => x.Ename == baseinfoType).Id;
            IEnumerable<BaseInfoGeneral> baseinfos = _unitOfWork.BaseInfoGenerals.Find(x => x.TypeId == baseInfoGeneralTypeId && x.Name.ToLower() != "add" && x.Name.ToLower() != "use");
            return Common.ConvertModels<BaseInfoGeneralViewModel, BaseInfoGeneral>.convertModelsLists(baseinfos);
        }

        public Guid GetBaseInfoTypeGuidByName(string baseinfoName)
        {
            try
            {
                return _unitOfWork.BaseInfos.GetBaseInfoType(x => x.Ename == baseinfoName).Guid;

            }
            catch { return Guid.Empty; }

        }

        public IEnumerable<BaseInfoViewModel> GetAllBaseInfos(Guid baseinfoTypeId, Guid clinicSectionId)
        {
            IEnumerable<BaseInfo> baseinfos = _unitOfWork.BaseInfos.Find(x => x.TypeId == baseinfoTypeId && x.ClinicSectionId == clinicSectionId);
            List<BaseInfoViewModel> baseInfoViews = Common.ConvertModels<BaseInfoViewModel, BaseInfo>.convertModelsLists(baseinfos);
            Indexing<BaseInfoViewModel> indexing = new Indexing<BaseInfoViewModel>();
            return indexing.AddIndexing(baseInfoViews);
        }

        public IEnumerable<BaseInfoTypeViewModel> GetAllBaseInfoTypesForSpecificSection(int sectionTypeId, IStringLocalizer<SharedResource> _localizer)
        {
            IEnumerable<BaseInfoType> baseinfoTypes = _unitOfWork.BaseInfos.GetAllBaseInfoTypesForSpecificSection(sectionTypeId);
            List<BaseInfoType> translatedBaseInfo = new List<BaseInfoType>();
            foreach (BaseInfoType baseInf in baseinfoTypes)
            {
                baseInf.Fname = _localizer[baseInf.Ename];
                translatedBaseInfo.Add(baseInf);
            }
            List<BaseInfoTypeViewModel> baseInfoTypeViews = Common.ConvertModels<BaseInfoTypeViewModel, BaseInfoType>.convertModelsLists(translatedBaseInfo);
            return baseInfoTypeViews;
        }

        public IEnumerable<BaseInfoTypeViewModel> GetAllBaseInfoTypes()
        {
            IEnumerable<BaseInfoType> baseinfoTypes = _unitOfWork.BaseInfos.GetAllBaseInfoTypes();
            //List<BaseInfoType> translatedBaseInfo = new List<BaseInfoType>();
            //foreach (BaseInfoType baseInf in baseinfoTypes)
            //{
            //    baseInf.Fname = langExps.SingleOrDefault(x => x.Expression == baseInf.Ename).ExpressionEquivalent;
            //    translatedBaseInfo.Add(baseInf);
            //}
            List<BaseInfoTypeViewModel> baseInfoTypeViews = Common.ConvertModels<BaseInfoTypeViewModel, BaseInfoType>.convertModelsLists(baseinfoTypes);
            return baseInfoTypeViews;
        }

        public Guid GetBaseInfoTypeIdByName(string BaseInfoTypeName)
        {
            return _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName(BaseInfoTypeName);
        }

        public IEnumerable<BaseInfoTypeViewModel> GetAllBaseInfoType()
        {
            IEnumerable<BaseInfoType> baseinfoTypes = _unitOfWork.BaseInfos.GetAllBaseInfoTypes();
            return Common.ConvertModels<BaseInfoTypeViewModel, BaseInfoType>.convertModelsLists(baseinfoTypes);
        }



        public void AddBaseInfoTypeOfBaseInfo(Guid baseInfoId, Guid baseInfoTypeId)
        {
            BaseInfo baseInfo = _unitOfWork.BaseInfos.Get(baseInfoId);
            _unitOfWork.BaseInfos.Detach(baseInfo);
            _unitOfWork.Complete();
            baseInfo.TypeId = baseInfoTypeId;
            _unitOfWork.BaseInfos.UpdateState(baseInfo);
            _unitOfWork.Complete();
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/BaseInfo/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public Guid AddNewBaseInfo(BaseInfoViewModel newBaseInfo, Guid clinicSectionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newBaseInfo.Name))
                    return Guid.Empty;

                newBaseInfo.ClinicSectionId = clinicSectionId;
                BaseInfo baseInfo = Common.ConvertModels<BaseInfo, BaseInfoViewModel>.convertModels(newBaseInfo);
                baseInfo.Guid = Guid.NewGuid();
                _unitOfWork.BaseInfos.Add(baseInfo);
                _unitOfWork.Complete();
                return baseInfo.Guid;
            }
            catch (Exception ex) { throw ex; }
        }
        public Guid UpdateBaseInfo(BaseInfoViewModel baseInfo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseInfo.Name))
                    return Guid.Empty;

                BaseInfo baseInfoDto = Common.ConvertModels<BaseInfo, BaseInfoViewModel>.convertModels(baseInfo);
                _unitOfWork.BaseInfos.UpdateState(baseInfoDto);
                _unitOfWork.Complete();
                return baseInfoDto.Guid;
            }
            catch (Exception ex) { throw ex; }

        }

        public OperationStatus RemoveBaseInfo(Guid baseInfoId)
        {
            try
            {
                BaseInfo baseInfo = _unitOfWork.BaseInfos.Get(baseInfoId);
                _unitOfWork.BaseInfos.Remove(baseInfo);
                _unitOfWork.Complete();
                return OperationStatus.SUCCESSFUL;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity;
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong;
                }
            }

        }

        public BaseInfoViewModel GetBaseInfo(Guid baseInfoId)
        {
            try
            {
                BaseInfo baseInfo = _unitOfWork.BaseInfos.Get(baseInfoId);
                BaseInfoViewModel med = Common.ConvertModels<BaseInfoViewModel, BaseInfo>.convertModels(baseInfo);
                return med;
            }
            catch { return null; }
        }

        public BaseInfoGeneralViewModel GetBaseInfoGeneral(int baseInfoId)
        {
            try
            {
                BaseInfoGeneral baseInfo = _unitOfWork.BaseInfoGenerals.GetSingle(a => a.Id == baseInfoId);
                BaseInfoGeneralViewModel med = Common.ConvertModels<BaseInfoGeneralViewModel, BaseInfoGeneral>.convertModels(baseInfo);
                return med;
            }
            catch { return null; }
        }

        public int? GetIdByNameAndType(string name, string type)
        {
            return _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(name, type);
        }

        public bool CheckRepeatedInfoBaseName(string name, Guid clinicSectionId, bool NewOrUpdate, Guid baseInfoTypeId, string oldName = "")
        {

            try
            {
                BaseInfo baseInfo = null;
                if (NewOrUpdate)
                {
                    baseInfo = _unitOfWork.BaseInfos.Find(x => x.Name.Trim() == name.Trim() && x.ClinicSectionId == clinicSectionId && x.TypeId == baseInfoTypeId).FirstOrDefault();
                }
                else
                {
                    baseInfo = _unitOfWork.BaseInfos.Find(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName && x.ClinicSectionId == clinicSectionId && x.TypeId == baseInfoTypeId).FirstOrDefault();
                }
                if (baseInfo != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<PeriodsViewModel> GetAllPeriods(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)Periods.Day,
                Name = _localizer[Periods.Day.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)Periods.Week,
                Name = _localizer[Periods.Week.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)Periods.Month,
                Name = _localizer[Periods.Month.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)Periods.Year,
                Name = _localizer[Periods.Year.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)Periods.Allready,
                Name = _localizer[Periods.Allready.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)Periods.FromDateToDate,
                Name = _localizer["From"] + " " + _localizer["Date"] + " " + _localizer["To"] + " " + _localizer["Date"]
            };
            periods.Add(period);
            return periods;
        }


        public IEnumerable<PeriodsViewModel> GetAllClearanceType(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)DischargeType.NotDischarge,
                //period.Name = _localizer[ClearanceType.NotDischarge.ToString()];
                Name = DischargeType.NotDischarge.ToString()
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)DischargeType.Discharge,
                //period.Name = _localizer[ClearanceType.Discharge.ToString()];
                Name = DischargeType.Discharge.ToString()
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)DischargeType.All,
                //period.Name = _localizer[ClearanceType.All.ToString()];
                Name = DischargeType.All.ToString()
            };
            periods.Add(period);


            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllPaymentStatus(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)PaymentStatus.Unpaid,
                Name = PaymentStatus.Unpaid.ToString()
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PaymentStatus.Paid,
                Name = PaymentStatus.Paid.ToString()
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PaymentStatus.All,
                Name = PaymentStatus.All.ToString()
            };
            periods.Add(period);


            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllReturnSaleFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)SaleFilter.Customer,
                Name = _localizer[SaleFilter.Customer.ToString()]
            };
            periods.Add(period);

            //period = new PeriodsViewModel
            //{
            //    Id = (int)SaleFilter.Currency,
            //    Name = _localizer[SaleFilter.Currency.ToString()]
            //};
            //periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)SaleFilter.InvoiceNum,
                Name = _localizer[SaleFilter.InvoiceNum.ToString()]
            };
            periods.Add(period);

            //period = new PeriodsViewModel
            //{
            //    Id = (int)SaleFilter.MainInvoiceNum,
            //    Name = _localizer[SaleFilter.MainInvoiceNum.ToString()]
            //};
            //periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllReturnPurchaseFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)PurchaseFilter.Supplier,
                Name = _localizer[PurchaseFilter.Supplier.ToString()]
            };
            periods.Add(period);

            //period = new PeriodsViewModel
            //{
            //    Id = (int)PurchaseFilter.Currency,
            //    Name = _localizer[PurchaseFilter.Currency.ToString()]
            //};
            //periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PurchaseFilter.InvoiceNum,
                Name = _localizer[PurchaseFilter.InvoiceNum.ToString()]
            };
            periods.Add(period);

            //period = new PeriodsViewModel
            //{
            //    Id = (int)PurchaseFilter.MainInvoiceNum,
            //    Name = _localizer[PurchaseFilter.MainInvoiceNum.ToString()]
            //};
            //periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllPurchaseFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)PurchaseFilter.Supplier,
                Name = _localizer[PurchaseFilter.Supplier.ToString()]
            };
            periods.Add(period);

            //period = new PeriodsViewModel
            //{
            //    Id = (int)PurchaseFilter.Currency,
            //    Name = _localizer[PurchaseFilter.Currency.ToString()]
            //};
            //periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PurchaseFilter.InvoiceNum,
                Name = _localizer[PurchaseFilter.InvoiceNum.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PurchaseFilter.MainInvoiceNum,
                Name = _localizer[PurchaseFilter.MainInvoiceNum.ToString()]
            };
            periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllSaleFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();


            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)SaleFilter.Customer,
                Name = _localizer[SaleFilter.Customer.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)SaleFilter.InvoiceNum,
                Name = _localizer[SaleFilter.InvoiceNum.ToString()]
            };
            periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllTransferYearFilter(IStringLocalizer<SharedResource> _localizer, string year)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();

            int year_now = DateTime.Now.Year;
            if (int.TryParse(year, out int res) && res <= year_now && res >= 2020)
            {
                for (int i = year_now; i >= res; i--)
                {
                    PeriodsViewModel period = new PeriodsViewModel
                    {
                        Id = i,
                        Name = i.ToString()
                    };
                    periods.Add(period);
                }
            }
            else
            {
                PeriodsViewModel period = new PeriodsViewModel
                {
                    Id = year_now,
                    Name = year_now.ToString()
                };
                periods.Add(period);
            }

            PeriodsViewModel last_period = new PeriodsViewModel
            {
                Id = 0,
                Name = _localizer["From"] + " " + _localizer["Date"] + " " + _localizer["To"] + " " + _localizer["Date"]
            };
            periods.Add(last_period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetMedicineProductFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)ProductFilter.Barcode,
                Name = _localizer[ProductFilter.Barcode.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ProductFilter.OrderPointRange,
                Name = _localizer[ProductFilter.OrderPointRange.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ProductFilter.Supplier,
                Name = _localizer[ProductFilter.Supplier.ToString()]
            };
            periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllTransferFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)TransferFilter.Type,
                Name = _localizer[TransferFilter.Type.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)TransferFilter.ClinicSection,
                Name = _localizer[TransferFilter.ClinicSection.ToString()]
            };
            periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllReceiveReportFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)ReceiveReportFilter.All,
                Name = _localizer[ReceiveReportFilter.All.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ReceiveReportFilter.UnpaidInvoice,
                Name = _localizer[ReceiveReportFilter.UnpaidInvoice.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ReceiveReportFilter.UnpaidInvoice_Sale,
                Name = _localizer[ReceiveReportFilter.UnpaidInvoice_Sale.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ReceiveReportFilter.UnpaidInvoice_ReturnSale,
                Name = _localizer[ReceiveReportFilter.UnpaidInvoice_ReturnSale.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ReceiveReportFilter.PaidInvoice,
                Name = _localizer[ReceiveReportFilter.PaidInvoice.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ReceiveReportFilter.PaidInvoice_Sale,
                Name = _localizer[ReceiveReportFilter.PaidInvoice_Sale.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ReceiveReportFilter.PaidInvoice_ReturnSale,
                Name = _localizer[ReceiveReportFilter.PaidInvoice_ReturnSale.ToString()]
            };
            periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllPayReportFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)PayReportFilter.All,
                Name = _localizer[PayReportFilter.All.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PayReportFilter.UnpaidInvoice,
                Name = _localizer[PayReportFilter.UnpaidInvoice.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PayReportFilter.UnpaidInvoice_Purchase,
                Name = _localizer[PayReportFilter.UnpaidInvoice_Purchase.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PayReportFilter.UnpaidInvoice_ReturnPurchase,
                Name = _localizer[PayReportFilter.UnpaidInvoice_ReturnPurchase.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PayReportFilter.PaidInvoice,
                Name = _localizer[PayReportFilter.PaidInvoice.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PayReportFilter.PaidInvoice_Purchase,
                Name = _localizer[PayReportFilter.PaidInvoice_Purchase.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)PayReportFilter.PaidInvoice_ReturnPurchase,
                Name = _localizer[PayReportFilter.PaidInvoice_ReturnPurchase.ToString()]
            };
            periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllSupplierFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)SupplierFilter.All,
                Name = _localizer[SupplierFilter.All.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)SupplierFilter.CashPayment,
                Name = _localizer[SupplierFilter.CashPayment.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)SupplierFilter.GetCash,
                Name = _localizer[SupplierFilter.GetCash.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)SupplierFilter.Invoice,
                Name = _localizer[SupplierFilter.Invoice.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)SupplierFilter.NotReceivedInvoices,
                Name = _localizer[SupplierFilter.NotReceivedInvoices.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)SupplierFilter.PartialReceivedInvoices,
                Name = _localizer[SupplierFilter.PartialReceivedInvoices.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)SupplierFilter.ReceivedInvoices,
                Name = _localizer[SupplierFilter.ReceivedInvoices.ToString()]
            };
            periods.Add(period);

            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllMonthPeriods(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)MonthPeriods.OneMonth,
                Name = _localizer[MonthPeriods.OneMonth.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)MonthPeriods.TwoMonth,
                Name = _localizer[MonthPeriods.TwoMonth.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)MonthPeriods.ThreeMonth,
                Name = _localizer[MonthPeriods.ThreeMonth.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)MonthPeriods.Allready,
                Name = _localizer[MonthPeriods.Allready.ToString()]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)MonthPeriods.FromDateToDate,
                Name = _localizer["From"] + " " + _localizer["Date"] + " " + _localizer["To"] + " " + _localizer["Date"]
            };
            periods.Add(period);
            period = new PeriodsViewModel
            {
                Id = (int)MonthPeriods.SeeAllHuman,
                Name = _localizer["SeeAllHuman"]
            };
            periods.Add(period);
            return periods;
        }

        public IEnumerable<PeriodsViewModel> GetAllProductReportFilter(IStringLocalizer<SharedResource> _localizer)
        {
            List<PeriodsViewModel> periods = new List<PeriodsViewModel>();
            PeriodsViewModel period = new PeriodsViewModel
            {
                Id = (int)ProductReportFilter.Cardex,
                Name = _localizer[ProductReportFilter.Cardex.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ProductReportFilter.Purchase,
                Name = _localizer[ProductReportFilter.Purchase.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ProductReportFilter.Sale,
                Name = _localizer[ProductReportFilter.Sale.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ProductReportFilter.ReturnPurchase,
                Name = _localizer[ProductReportFilter.ReturnPurchase.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ProductReportFilter.ReturnSale,
                Name = _localizer[ProductReportFilter.ReturnSale.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ProductReportFilter.TransferProduct,
                Name = _localizer[ProductReportFilter.TransferProduct.ToString()]
            };
            periods.Add(period);

            period = new PeriodsViewModel
            {
                Id = (int)ProductReportFilter.ReceiveProduct,
                Name = _localizer[ProductReportFilter.ReceiveProduct.ToString()]
            };
            periods.Add(period);

            return periods;
        }

        public BaseInfoViewModel GetBaseInfoByName(string baseInfo, Guid ClinicSectionId)
        {
            BaseInfo baseInfos = _unitOfWork.BaseInfos.GetSingle(x => x.ClinicSectionId == ClinicSectionId && x.Name == baseInfo);
            return Common.ConvertModels<BaseInfoViewModel, BaseInfo>.convertModels(baseInfos);
        }


    }
}
