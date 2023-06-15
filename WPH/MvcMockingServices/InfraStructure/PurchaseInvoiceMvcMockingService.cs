using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.PurchaseInvoice;
using WPH.Models.PurchaseInvoiceDetail;
using WPH.Models.PurchaseInvoiceDiscount;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class PurchaseInvoiceMvcMockingService : IPurchaseInvoiceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public PurchaseInvoiceMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemovePurchaseInvoice(Guid purchaseInvoiceId, Guid userId, string pass)
        {
            try
            {
                var check = _unitOfWork.Users.CheckUserByIdAndPass(userId, pass);
                if (!check)
                    return "WrongPass";

                var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(purchaseInvoiceId);
                if (can_change)
                    return "InvoiceInUse";

                PurchaseInvoice Hos = _unitOfWork.PurchaseInvoices.Get(purchaseInvoiceId);
                _unitOfWork.PurchaseInvoices.Remove(Hos);
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
            string controllerName = "/PurchaseInvoice/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewPurchaseInvoice(PurchaseInvoiceViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.MainInvoiceNum) || viewModel.SupplierId == null)
                return "DataNotValid";

            //var exsit = _unitOfWork.PurchaseInvoices.CheckRepeatedInvoiceNum(viewModel.SupplierId.Value, viewModel.MainInvoiceNum);
            //if (exsit)
            //    return "RepeatedMainInvoiceNum";

            PurchaseInvoice purchaseInvoice = ConvertModel(viewModel);

            var now = DateTime.Now;
            purchaseInvoice.CreateDate = now;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUsePurchaseInvoiceDate");
            if (!access)
            {
                purchaseInvoice.InvoiceDate = now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                purchaseInvoice.InvoiceDate = invoiceDate;
            }

            purchaseInvoice.TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(viewModel.PurchaseType, "MaterialType");
            while (true)
            {
                try
                {
                    purchaseInvoice.InvoiceNum = GetPurchaseInvoiceNum(viewModel.ClinicSectionId.Value);

                    _unitOfWork.PurchaseInvoices.Add(purchaseInvoice);
                    _unitOfWork.Complete();

                    break;
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("UQ_PurchaseInvoice_InvoiceNum_ClinicSectionId") && !(e.InnerException?.Message ?? "").Contains("UQ_PurchaseInvoice_InvoiceNum_ClinicSectionId"))
                        throw e;
                }
            }

            return $"{purchaseInvoice.Guid}_{purchaseInvoice.InvoiceNum}";
        }


        public string UpdatePurchaseInvoice(PurchaseInvoiceViewModel viewModel)
        {
            var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(viewModel.Guid);
            if (can_change)
                return "InvoiceInUse";

            if (string.IsNullOrWhiteSpace(viewModel.MainInvoiceNum) || viewModel.SupplierId == null)
                return "DataNotValid";

            //var exsit = _unitOfWork.PurchaseInvoices.CheckRepeatedInvoiceNum(viewModel.SupplierId.Value, viewModel.MainInvoiceNum, p => p.Guid != viewModel.Guid);
            //if (exsit)
            //    return "RepeatedMainInvoiceNum";

            PurchaseInvoice purchaseInvoice = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(viewModel.Guid);
            if (purchaseInvoice.OldFactor.GetValueOrDefault(false))
                return purchaseInvoice.TotalPrice;

            //var exists = _unitOfWork.ReturnPurchaseInvoiceDetails.CheckDetailsExistByPurchaseInvoiceDetailIds(viewModel.Guid);
            //if (exists && purchaseInvoice.SupplierId != viewModel.SupplierId)
            //    return "Purchase";

            purchaseInvoice.ModifiedDate = DateTime.Now;
            purchaseInvoice.ModifiedUserId = viewModel.ModifiedUserId;
            purchaseInvoice.MainInvoiceNum = viewModel.MainInvoiceNum;
            purchaseInvoice.Description = viewModel.Description;
            purchaseInvoice.TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(viewModel.PurchaseType, "MaterialType");

            if (!purchaseInvoice.PurchaseInvoiceDetails.Any())
                purchaseInvoice.SupplierId = viewModel.SupplierId;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUsePurchaseInvoiceDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                purchaseInvoice.InvoiceDate = invoiceDate;
            }

            _unitOfWork.PurchaseInvoices.UpdateState(purchaseInvoice);
            //_unitOfWork.Complete();

            //_unitOfWork.PurchaseInvoices.Detach(purchaseInvoice);

            return UpdateTotalPrice(purchaseInvoice);
        }

        public string UpdateTotalPrice(PurchaseInvoice invoice)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            List<PurchaseInvoiceTotalPriceViewModel> prices = invoice.PurchaseInvoiceDetails.Select(p => new PurchaseInvoiceTotalPriceViewModel
            {
                Purchase = true,
                CurrencyName = p.CurrencyName,
                TotalDiscount = p.Discount.GetValueOrDefault(0),
                TotalPrice = p.Num.GetValueOrDefault(0) * p.PurchasePrice.GetValueOrDefault(0)
            }).ToList();

            prices.AddRange(invoice.PurchaseInvoiceDiscounts.Select(p => new PurchaseInvoiceTotalPriceViewModel
            {
                Purchase = false,
                CurrencyName = p.CurrencyName,
                TotalDiscount = p.Amount.GetValueOrDefault(0),
                TotalPrice = 0
            }).ToList());

            var total_price = string.Join("_", prices.Where(p => p.Purchase).GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.PriceAfterDiscount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList());

            var total_discount = string.Join("_", prices.Where(p => !p.Purchase).GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.TotalDiscount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList());

            var total_after_discount = string.Join("_", prices.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.PriceAfterDiscount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList());

            var total_cost = string.Join("_", invoice.Costs.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Price.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList());

            invoice.TotalPrice = total_after_discount;

            invoice.PurchaseInvoiceDetails = null;
            invoice.PurchaseInvoiceDiscounts = null;
            invoice.Costs = null;
            _unitOfWork.PurchaseInvoices.UpdateState(invoice);
            _unitOfWork.Complete();

            return $"{total_price}#{total_discount}#{total_after_discount}#{total_cost}";
        }

        public string GetPurchaseInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                string purchaseInvoiceNum = _unitOfWork.PurchaseInvoices.GetLatestPurchaseInvoiceNum(clinicSectionId);
                return NextPurchaseInvoiceNum(purchaseInvoiceNum);
            }
            catch (Exception) { return "1"; }
        }

        public string NextPurchaseInvoiceNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }

        public IEnumerable<PurchaseInvoiceViewModel> GetAllPurchaseInvoices(Guid clinicSectionId, PurchaseInvoiceFilterViewModel filterViewModel, IStringLocalizer<SharedResource> _localizer)
        {
            IEnumerable<PurchaseInvoice> hosp;

            if (!string.IsNullOrWhiteSpace(filterViewModel.InvoiceNum))
            {
                hosp = _unitOfWork.PurchaseInvoices.GetAllPurchaseInvoiceByInvoiceNum(clinicSectionId, filterViewModel.InvoiceNum);
            }
            else
            {
                if (filterViewModel.PeriodId != (int)Periods.FromDateToDate)
                {
                    var DateFrom = DateTime.Now;
                    var DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, filterViewModel.PeriodId);

                    filterViewModel.DateFrom = DateFrom;
                    filterViewModel.DateTo = DateTo;
                }

                int? type = null;
                if (!string.IsNullOrWhiteSpace(filterViewModel.PurchaseType))
                {
                    //type = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Material", "MaterialType");
                    type = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(filterViewModel.PurchaseType, "MaterialType");
                }

                hosp = _unitOfWork.PurchaseInvoices
                   .GetAllPurchaseInvoice(clinicSectionId, filterViewModel.DateFrom, filterViewModel.DateTo, p =>
                       (filterViewModel.Supplier == null || p.SupplierId == filterViewModel.Supplier) &&
                       (type == null || p.TypeId == type) &&
                       (filterViewModel.ProductId == null || p.PurchaseInvoiceDetails.Any(x => x.ProductId == filterViewModel.ProductId)) &&
                       (string.IsNullOrWhiteSpace(filterViewModel.MainInvoiceNum) || p.MainInvoiceNum.Contains(filterViewModel.MainInvoiceNum)));

            }

            CultureInfo cultures = new CultureInfo("en-US");
            List<PurchaseInvoiceViewModel> hospconvert = hosp.Select(p => new PurchaseInvoiceViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                MainInvoiceNum = p.MainInvoiceNum,
                TotalPrice = p.TotalPrice,
                SupplierName = p.Supplier.User.Name,
                Status = p.Status ? _localizer["Paid"] : _localizer["UnPaid"],
                TotalDiscount = string.Join("_", p.PurchaseInvoiceDiscounts.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList()),
                TotalCost = string.Join("_", p.Costs.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Price.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList())
            }).ToList();

            Indexing<PurchaseInvoiceViewModel> indexing = new Indexing<PurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<PurchaseInvoiceTotalPriceViewModel> GetAllTotalPrice(Guid purchaseInvoiceId)
        {
            var details = _unitOfWork.PurchaseInvoiceDetails.GetAllTotalPrice(purchaseInvoiceId);

            var result = details.GroupBy(p => p.Currency.Name).Select(p => new PurchaseInvoiceTotalPriceViewModel
            {
                CurrencyName = p.Key,
                TotalDiscount = p.Sum(x => x.Discount.GetValueOrDefault(0)),
                TotalPrice = p.Sum(x => x.Num.GetValueOrDefault(0) * x.PurchasePrice)
            }).ToList();
            Indexing<PurchaseInvoiceTotalPriceViewModel> indexing = new Indexing<PurchaseInvoiceTotalPriceViewModel>();
            return indexing.AddIndexing(result);
        }

        public PurchaseInvoiceViewModel GetPurchaseInvoice(Guid PurchaseInvoiceId)
        {
            try
            {
                PurchaseInvoice invoice = _unitOfWork.PurchaseInvoices.GetPurchaseInvoice(PurchaseInvoiceId);
                var result = ConvertModel(invoice);
                result.TotalPrice = UpdateTotalPrice(invoice);

                return result;
            }
            catch { return null; }
        }

        public IEnumerable<PurchaseInvoiceViewModel> GetPayPurchaseInvoice(Guid? payId)
        {
            if (payId == null || payId == Guid.Empty)
                return new List<PurchaseInvoiceViewModel>();

            var invoices = _unitOfWork.PurchaseInvoices.GetPayPurchaseInvoice(payId);
            List<PurchaseInvoiceViewModel> hospconvert = ConvertCustomeModelsLists(invoices);
            Indexing<PurchaseInvoiceViewModel> indexing = new Indexing<PurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<PurchaseInvoiceViewModel> GetNotPayPurchaseInvoice(Guid? supplierId)
        {
            if (supplierId == null || supplierId == Guid.Empty)
                return new List<PurchaseInvoiceViewModel>();

            var invoices = _unitOfWork.PurchaseInvoices.GetNotPayPurchaseInvoice(supplierId);
            List<PurchaseInvoiceViewModel> hospconvert = ConvertCustomeModelsLists(invoices);
            Indexing<PurchaseInvoiceViewModel> indexing = new Indexing<PurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<PurchaseInvoiceViewModel> GetPartialPayPurchaseInvoice(Guid? payId, int? currencyId)
        {
            var invoices = _unitOfWork.PurchaseInvoices.GetPartialPayPurchaseInvoice(payId, currencyId);
            List<PurchaseInvoiceViewModel> hospconvert = ConvertCustomePartialModelsLists(invoices);
            Indexing<PurchaseInvoiceViewModel> indexing = new Indexing<PurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<PurchaseInvoiceViewModel> GetNotPartialPayPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId)
        {
            var invoices = _unitOfWork.PurchaseInvoices.GetNotPartialPayPurchaseInvoice(supplierId, currencyId, payId);
            List<PurchaseInvoiceViewModel> hospconvert = ConvertCustomePartialModelsLists(invoices);
            Indexing<PurchaseInvoiceViewModel> indexing = new Indexing<PurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public PrintPurchaseReportViewModel GetPurchaseForReport(Guid purchaseInvoiceId, IStringLocalizer<SharedResource> _localizer)
        {
            var purchase = _unitOfWork.PurchaseInvoices.GetPurchaseForReport(purchaseInvoiceId);

            CultureInfo cultures = new CultureInfo("en-US");
            var result = new PrintPurchaseReportViewModel
            {
                Supplier = purchase.Supplier.User.Name,
                InvoiceNum = purchase.InvoiceNum,
                MainInvoiceNum = purchase.MainInvoiceNum,
                InvoiceDate = purchase.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures),
                Description = purchase.Description,
                Totals = new List<TotalDiscountViewModel>
                {
                    new TotalDiscountViewModel
                    {
                        ShowName = _localizer["Total"],
                        ShowValue = string.Join("<br>", purchase.PurchaseInvoiceDetails.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Num.GetValueOrDefault(0) * s.PurchasePrice.GetValueOrDefault(0) - s.Discount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}").OrderBy(p => p).ToList())
                    },
                    new TotalDiscountViewModel
                    {
                        ShowName = _localizer["Discount"],
                        ShowValue = string.Join("<br>", purchase.PurchaseInvoiceDiscounts.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}").OrderBy(p => p).ToList())
                    },
                    new TotalDiscountViewModel
                    {
                        ShowName = _localizer["TotalAfterDiscount"],
                        ShowValue = purchase.TotalPrice.Replace("_","<br>")
                    }
                },
                Details = purchase.PurchaseInvoiceDetails.Select(p => new PrintPurchaseDetailReportViewModel
                {
                    ProductName = p.Product.Name,
                    Producer = p.Product.Producer.Name,
                    ProductType = p.Product.ProductType.Name,
                    ExpireDate = p.ExpireDate.Value.ToString("dd/MM/yyyy", cultures),
                    Num = p.Num.GetValueOrDefault(0).ToString("#,0.##", cultures),
                    FreeNum = p.FreeNum.GetValueOrDefault(0).ToString("#,0.##", cultures),
                    CurrencyName = p.CurrencyName,
                    TempPurchasePrice = p.PurchasePrice.GetValueOrDefault(0),
                    TempDiscount = p.Discount.GetValueOrDefault(0),
                    TempTotal = p.Num.GetValueOrDefault(0) * p.PurchasePrice.GetValueOrDefault(0)
                })
            };

            return result;
        }

        public bool CheckPurchaseInvoicePaid(Guid purchaseInvoiceId)
        {
            var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(purchaseInvoiceId);
            return can_change;
        }

        // Begin Convert 
        public PurchaseInvoice ConvertModel(PurchaseInvoiceViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceViewModel, PurchaseInvoice>().ForMember(a => a.PurchaseInvoiceDetails, b => b.Ignore());

                //.ForMember(a => a.PurchaseInvoiceTypeName, b => b.MapFrom(c => c.PurchaseInvoiceType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PurchaseInvoiceViewModel, PurchaseInvoice>(Users);
        }

        public PurchaseInvoiceViewModel ConvertModel(PurchaseInvoice Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoice, PurchaseInvoiceViewModel>()
                .ForMember(a => a.PurchaseInvoiceDetails, b => b.Ignore())
                .ForMember(a => a.CanChange, b => b.MapFrom(c => !c.PurchaseInvoiceDetails.Any()))
                .ForMember(a => a.PurchaseType, b => b.MapFrom(c => c.Type.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PurchaseInvoice, PurchaseInvoiceViewModel>(Users);
        }

        public List<PurchaseInvoiceViewModel> ConvertCustomePartialModelsLists(IEnumerable<PartialPayModel> purchaseInvoices)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            List<PurchaseInvoiceViewModel> purchaseInvoiceDtoList = new List<PurchaseInvoiceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PartialPayModel, PurchaseInvoiceViewModel>()
                .ForMember(a => a.InvoiceDateTxt, b => b.MapFrom(c => c.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)))
                .ForMember(a => a.PurchaseInvoiceDetails, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDtoList = mapper.Map<IEnumerable<PartialPayModel>, List<PurchaseInvoiceViewModel>>(purchaseInvoices);
            return purchaseInvoiceDtoList;
        }

        public List<PurchaseInvoiceViewModel> ConvertCustomeModelsLists(IEnumerable<PurchaseInvoice> purchaseInvoices)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            List<PurchaseInvoiceViewModel> purchaseInvoiceDtoList = new List<PurchaseInvoiceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoice, PurchaseInvoiceViewModel>()
                .ForMember(a => a.InvoiceDateTxt, b => b.MapFrom(c => c.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)))
                .ForMember(a => a.PurchaseInvoiceDetails, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDtoList = mapper.Map<IEnumerable<PurchaseInvoice>, List<PurchaseInvoiceViewModel>>(purchaseInvoices);
            return purchaseInvoiceDtoList;
        }
        // End Convert
    }
}
