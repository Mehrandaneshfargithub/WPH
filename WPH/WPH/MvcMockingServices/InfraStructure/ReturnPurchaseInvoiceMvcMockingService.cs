using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.ReturnPurchaseInvoice;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReturnPurchaseInvoiceMvcMockingService : IReturnPurchaseInvoiceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ReturnPurchaseInvoiceMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemoveReturnPurchaseInvoice(Guid returnPurchaseInvoiceid, Guid userId, string pass)
        {
            try
            {
                var check = _unitOfWork.Users.CheckUserByIdAndPass(userId, pass);
                if (!check)
                    return "WrongPass";

                var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(returnPurchaseInvoiceid);
                if (can_change)
                    return "InvoiceInUse";

                ReturnPurchaseInvoice Hos = _unitOfWork.ReturnPurchaseInvoices.Get(returnPurchaseInvoiceid);
                _unitOfWork.ReturnPurchaseInvoices.Remove(Hos);
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
            string controllerName = "/ReturnPurchaseInvoice/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewReturnPurchaseInvoice(ReturnPurchaseInvoiceViewModel viewModel)
        {
            if (viewModel.SupplierId == null)
                return "DataNotValid";

            ReturnPurchaseInvoice purchaseInvoice = ConvertModel(viewModel);

            var now = DateTime.Now;
            purchaseInvoice.CreateDate = now;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseReturnPurchaseInvoiceDate");
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

            while (true)
            {
                try
                {
                    purchaseInvoice.InvoiceNum = GetReturnPurchaseInvoiceNum(viewModel.ClinicSectionId.Value);

                    _unitOfWork.ReturnPurchaseInvoices.Add(purchaseInvoice);
                    _unitOfWork.Complete();

                    break;
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("") && !(e.InnerException?.Message ?? "UQ_ReturnPurchaseInvoice_InvoiceNum_ClinicSectionId").Contains("UQ_ReturnPurchaseInvoice_InvoiceNum_ClinicSectionId"))
                        throw e;
                }
            }

            return $"{purchaseInvoice.Guid}_{purchaseInvoice.InvoiceNum}";
        }



        public string UpdateReturnPurchaseInvoice(ReturnPurchaseInvoiceViewModel viewModel)
        {
            var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(viewModel.Guid);
            if (can_change)
                return "InvoiceInUse";

            if (viewModel.SupplierId == null)
                return "DataNotValid";

            ReturnPurchaseInvoice purchaseInvoice = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(viewModel.Guid);
            if (purchaseInvoice.OldFactor.GetValueOrDefault(false))
                return purchaseInvoice.TotalPrice;

            var exists = _unitOfWork.ReturnPurchaseInvoiceDetails.CheckDetailsExistByMasterId(viewModel.Guid);
            if (exists && purchaseInvoice.SupplierId != viewModel.SupplierId)
                return "CantChangeSupplier";

            purchaseInvoice.ModifiedDate = DateTime.Now;
            purchaseInvoice.ModifiedUserId = viewModel.ModifiedUserId;
            purchaseInvoice.Description = viewModel.Description;

            if (!purchaseInvoice.ReturnPurchaseInvoiceDetails.Any())
                purchaseInvoice.SupplierId = viewModel.SupplierId;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseReturnPurchaseInvoiceDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                purchaseInvoice.InvoiceDate = invoiceDate;
            }


            _unitOfWork.ReturnPurchaseInvoices.UpdateState(purchaseInvoice);
            //_unitOfWork.Complete();

            //_unitOfWork.ReturnPurchaseInvoices.Detach(purchaseInvoice);

            return UpdateTotalPrice(purchaseInvoice);
        }

        public string UpdateTotalPrice(ReturnPurchaseInvoice invoice)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            List<ReturnPurchaseInvoiceTotalPriceViewModel> prices = invoice.ReturnPurchaseInvoiceDetails.Select(p => new ReturnPurchaseInvoiceTotalPriceViewModel
            {
                Purchase = true,
                CurrencyName = p.CurrencyName,
                TotalDiscount = p.Discount.GetValueOrDefault(0),
                TotalPrice = p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0)
            }).ToList();

            prices.AddRange(invoice.ReturnPurchaseInvoiceDiscounts.Select(p => new ReturnPurchaseInvoiceTotalPriceViewModel
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

            invoice.ReturnPurchaseInvoiceDetails = null;
            invoice.ReturnPurchaseInvoiceDiscounts = null;
            _unitOfWork.ReturnPurchaseInvoices.UpdateState(invoice);
            _unitOfWork.Complete();

            return $"{total_price}#{total_discount}#{total_after_discount}";
        }

        public string GetReturnPurchaseInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                string purchaseInvoiceNum = _unitOfWork.ReturnPurchaseInvoices.GetLatestReturnPurchaseInvoiceNum(clinicSectionId);
                return NextReturnPurchaseInvoiceNum(purchaseInvoiceNum);
            }
            catch (Exception) { return "1"; }
        }

        public string NextReturnPurchaseInvoiceNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }

        public IEnumerable<ReturnPurchaseInvoiceViewModel> GetAllReturnPurchaseInvoices(Guid clinicSectionId, ReturnPurchaseInvoiceFilterViewModel filterViewModel)
        {
            if (filterViewModel.PeriodId != (int)Periods.FromDateToDate)
            {
                var DateFrom = DateTime.Now;
                var DateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, filterViewModel.PeriodId);

                filterViewModel.DateFrom = DateFrom;
                filterViewModel.DateTo = DateTo;
            }

            IEnumerable<ReturnPurchaseInvoice> hosp = _unitOfWork.ReturnPurchaseInvoices
                .GetAllReturnPurchaseInvoice(clinicSectionId, filterViewModel.DateFrom, filterViewModel.DateTo, p =>
                    (filterViewModel.Supplier == null || p.SupplierId == filterViewModel.Supplier) &&
                    (string.IsNullOrWhiteSpace(filterViewModel.InvoiceNum) || p.InvoiceNum.Contains(filterViewModel.InvoiceNum)));

            CultureInfo cultures = new CultureInfo("en-US");
            List<ReturnPurchaseInvoiceViewModel> hospconvert = hosp.Select(p => new ReturnPurchaseInvoiceViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                TotalPrice = p.TotalPrice,
                SupplierName = p.Supplier.User.Name,
                TotalDiscount = string.Join("_", p.ReturnPurchaseInvoiceDiscounts.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList()),
            }).ToList();
            Indexing<ReturnPurchaseInvoiceViewModel> indexing = new Indexing<ReturnPurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReturnPurchaseInvoiceTotalPriceViewModel> GetAllTotalPrice(Guid returnPurchaseInvoiceId)
        {
            var details = _unitOfWork.ReturnPurchaseInvoiceDetails.GetAllTotalPrice(returnPurchaseInvoiceId);

            var result = details.GroupBy(p => p.Currency.Name).Select(p => new ReturnPurchaseInvoiceTotalPriceViewModel
            {
                CurrencyName = p.Key,
                TotalDiscount = p.Sum(x => x.Discount.GetValueOrDefault(0)),
                TotalPrice = p.Sum(x => x.Num.GetValueOrDefault(0) * x.Price)
            }).ToList();
            Indexing<ReturnPurchaseInvoiceTotalPriceViewModel> indexing = new Indexing<ReturnPurchaseInvoiceTotalPriceViewModel>();
            return indexing.AddIndexing(result);
        }

        public ReturnPurchaseInvoiceViewModel GetReturnPurchaseInvoice(Guid returnPurchaseInvoiceId)
        {
            try
            {
                ReturnPurchaseInvoice invoice = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(returnPurchaseInvoiceId);
                var result = ConvertModel(invoice);
                result.TotalPrice = UpdateTotalPrice(invoice);

                return result;
            }
            catch { return null; }
        }

        public IEnumerable<ReturnPurchaseInvoiceViewModel> GetPayReturnPurchaseInvoice(Guid? payId)
        {
            var invoices = _unitOfWork.ReturnPurchaseInvoices.GetPayReturnPurchaseInvoice(payId);
            List<ReturnPurchaseInvoiceViewModel> hospconvert = ConvertCustomeModelsLists(invoices);
            Indexing<ReturnPurchaseInvoiceViewModel> indexing = new Indexing<ReturnPurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReturnPurchaseInvoiceViewModel> GetNotPayReturnPurchaseInvoice(Guid? supplierId)
        {
            var invoices = _unitOfWork.ReturnPurchaseInvoices.GetNotPayReturnPurchaseInvoice(supplierId);
            List<ReturnPurchaseInvoiceViewModel> hospconvert = ConvertCustomeModelsLists(invoices);
            Indexing<ReturnPurchaseInvoiceViewModel> indexing = new Indexing<ReturnPurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReturnPurchaseInvoiceViewModel> GetPartialPayReturnPurchaseInvoice(Guid? payId)
        {
            var invoices = _unitOfWork.ReturnPurchaseInvoices.GetPartialPayReturnPurchaseInvoice(payId);
            List<ReturnPurchaseInvoiceViewModel> hospconvert = ConvertCustomePartialModelsLists(invoices);
            Indexing<ReturnPurchaseInvoiceViewModel> indexing = new Indexing<ReturnPurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReturnPurchaseInvoiceViewModel> GetNotPartialPayReturnPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId)
        {
            var invoices = _unitOfWork.ReturnPurchaseInvoices.GetNotPartialPayReturnPurchaseInvoice(supplierId, currencyId, payId);
            List<ReturnPurchaseInvoiceViewModel> hospconvert = ConvertCustomePartialModelsLists(invoices);
            Indexing<ReturnPurchaseInvoiceViewModel> indexing = new Indexing<ReturnPurchaseInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public bool CheckReturnPurchaseInvoicePaid(Guid returnPurchaseInvoiceId)
        {
            var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(returnPurchaseInvoiceId);
            return can_change;
        }

        // Begin Convert 
        public ReturnPurchaseInvoice ConvertModel(ReturnPurchaseInvoiceViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseInvoiceViewModel, ReturnPurchaseInvoice>()
                .ForMember(a => a.ReturnPurchaseInvoiceDetails, b => b.Ignore());

                //.ForMember(a => a.ReturnPurchaseInvoiceTypeName, b => b.MapFrom(c => c.ReturnPurchaseInvoiceType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnPurchaseInvoiceViewModel, ReturnPurchaseInvoice>(Users);
        }

        public ReturnPurchaseInvoiceViewModel ConvertModel(ReturnPurchaseInvoice Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseInvoice, ReturnPurchaseInvoiceViewModel>()
                //.ForMember(a => a.ReturnPurchaseInvoiceDetails, b => b.Ignore())
                .ForMember(a => a.CanChange, b => b.MapFrom(c => !c.ReturnPurchaseInvoiceDetails.Any()))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnPurchaseInvoice, ReturnPurchaseInvoiceViewModel>(Users);
        }

        public List<ReturnPurchaseInvoiceViewModel> ConvertCustomePartialModelsLists(IEnumerable<PartialPayModel> purchaseInvoices)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            List<ReturnPurchaseInvoiceViewModel> purchaseInvoiceDtoList = new List<ReturnPurchaseInvoiceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PartialPayModel, ReturnPurchaseInvoiceViewModel>()
                .ForMember(a => a.InvoiceDateTxt, b => b.MapFrom(c => c.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)))
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDtoList = mapper.Map<IEnumerable<PartialPayModel>, List<ReturnPurchaseInvoiceViewModel>>(purchaseInvoices);
            return purchaseInvoiceDtoList;
        }

        public List<ReturnPurchaseInvoiceViewModel> ConvertCustomeModelsLists(IEnumerable<ReturnPurchaseInvoice> purchaseInvoices)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            List<ReturnPurchaseInvoiceViewModel> purchaseInvoiceDtoList = new List<ReturnPurchaseInvoiceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseInvoice, ReturnPurchaseInvoiceViewModel>()
                .ForMember(a => a.InvoiceDateTxt, b => b.MapFrom(c => c.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)))
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDtoList = mapper.Map<IEnumerable<ReturnPurchaseInvoice>, List<ReturnPurchaseInvoiceViewModel>>(purchaseInvoices);
            return purchaseInvoiceDtoList;
        }
        // End Convert
    }
}
