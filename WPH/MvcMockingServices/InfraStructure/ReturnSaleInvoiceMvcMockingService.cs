using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.ReturnSaleInvoice;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReturnSaleInvoiceMvcMockingService : IReturnSaleInvoiceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ReturnSaleInvoiceMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemoveReturnSaleInvoice(Guid returnSaleInvoiceid, Guid userId, string pass)
        {
            try
            {
                var check = _unitOfWork.Users.CheckUserByIdAndPass(userId, pass);
                if (!check)
                    return "WrongPass";

                var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(returnSaleInvoiceid);
                if (can_change)
                    return "InvoiceInUse";

                ReturnSaleInvoice Hos = _unitOfWork.ReturnSaleInvoices.Get(returnSaleInvoiceid);
                _unitOfWork.ReturnSaleInvoices.Remove(Hos);
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
            string controllerName = "/ReturnSaleInvoice/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewReturnSaleInvoice(ReturnSaleInvoiceViewModel viewModel)
        {
            if (viewModel.CustomerId == null)
                return "DataNotValid";

            ReturnSaleInvoice purchaseInvoice = ConvertModel(viewModel);

            var now = DateTime.Now;
            purchaseInvoice.CreateDate = now;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseReturnSaleInvoiceDate");
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
                    purchaseInvoice.InvoiceNum = GetReturnSaleInvoiceNum(viewModel.ClinicSectionId.Value);

                    _unitOfWork.ReturnSaleInvoices.Add(purchaseInvoice);
                    _unitOfWork.Complete();

                    break;
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("") && !(e.InnerException?.Message ?? "UQ_ReturnSaleInvoice_InvoiceNum_ClinicSectionId").Contains("UQ_ReturnSaleInvoice_InvoiceNum_ClinicSectionId"))
                        throw e;
                }
            }

            return $"{purchaseInvoice.Guid}_{purchaseInvoice.InvoiceNum}";
        }



        public string UpdateReturnSaleInvoice(ReturnSaleInvoiceViewModel viewModel)
        {
            var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(viewModel.Guid);
            if (can_change)
                return "InvoiceInUse";

            if (viewModel.CustomerId == null)
                return "DataNotValid";

            ReturnSaleInvoice purchaseInvoice = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(viewModel.Guid);
            if (purchaseInvoice.OldFactor.GetValueOrDefault(false))
                return purchaseInvoice.TotalPrice;

            var exists = _unitOfWork.ReturnSaleInvoiceDetails.CheckDetailsExistByMasterId(viewModel.Guid);
            if (exists && purchaseInvoice.CustomerId != viewModel.CustomerId)
                return "CantChangeCustomer";

            purchaseInvoice.ModifiedDate = DateTime.Now;
            purchaseInvoice.ModifiedUserId = viewModel.ModifiedUserId;
            purchaseInvoice.Description = viewModel.Description;

            if (!purchaseInvoice.ReturnSaleInvoiceDetails.Any())
                purchaseInvoice.CustomerId = viewModel.CustomerId;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseReturnSaleInvoiceDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                purchaseInvoice.InvoiceDate = invoiceDate;
            }

            _unitOfWork.ReturnSaleInvoices.UpdateState(purchaseInvoice);
            //_unitOfWork.Complete();

            //_unitOfWork.ReturnSaleInvoices.Detach(purchaseInvoice);

            return UpdateTotalPrice(purchaseInvoice);
        }

        public string UpdateTotalPrice(ReturnSaleInvoice invoice)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            List<ReturnSaleInvoiceTotalPriceViewModel> prices = invoice.ReturnSaleInvoiceDetails.Select(p => new ReturnSaleInvoiceTotalPriceViewModel
            {
                Purchase = true,
                CurrencyName = p.CurrencyName,
                TotalDiscount = p.Discount.GetValueOrDefault(0),
                TotalPrice = p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0)
            }).ToList();

            prices.AddRange(invoice.ReturnSaleInvoiceDiscounts.Select(p => new ReturnSaleInvoiceTotalPriceViewModel
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

            invoice.ReturnSaleInvoiceDetails = null;
            invoice.ReturnSaleInvoiceDiscounts = null;
            _unitOfWork.ReturnSaleInvoices.UpdateState(invoice);
            _unitOfWork.Complete();

            return $"{total_price}#{total_discount}#{total_after_discount}";
        }

        public string GetReturnSaleInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                string purchaseInvoiceNum = _unitOfWork.ReturnSaleInvoices.GetLatestReturnSaleInvoiceNum(clinicSectionId);
                return NextReturnSaleInvoiceNum(purchaseInvoiceNum);
            }
            catch (Exception) { return "1"; }
        }

        public string NextReturnSaleInvoiceNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }

        public IEnumerable<ReturnSaleInvoiceViewModel> GetAllReturnSaleInvoices(Guid clinicSectionId, ReturnSaleInvoiceFilterViewModel filterViewModel)
        {
            if (filterViewModel.PeriodId != (int)Periods.FromDateToDate)
            {
                var DateFrom = DateTime.Now;
                var DateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, filterViewModel.PeriodId);

                filterViewModel.DateFrom = DateFrom;
                filterViewModel.DateTo = DateTo;
            }

            IEnumerable<ReturnSaleInvoice> hosp = _unitOfWork.ReturnSaleInvoices
                .GetAllReturnSaleInvoice(clinicSectionId, filterViewModel.DateFrom, filterViewModel.DateTo, p =>
                    (filterViewModel.Customer == null || p.CustomerId == filterViewModel.Customer) &&
                    (string.IsNullOrWhiteSpace(filterViewModel.InvoiceNum) || p.InvoiceNum.Contains(filterViewModel.InvoiceNum)));

            CultureInfo cultures = new CultureInfo("en-US");
            List<ReturnSaleInvoiceViewModel> hospconvert = hosp.Select(p => new ReturnSaleInvoiceViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                TotalPrice = p.TotalPrice,
                CustomerName = p.Customer.User.Name,
                TotalDiscount = string.Join("_", p.ReturnSaleInvoiceDiscounts.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}").OrderBy(p => p).ToList()),
            }).ToList();

            Indexing<ReturnSaleInvoiceViewModel> indexing = new Indexing<ReturnSaleInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReturnSaleInvoiceTotalPriceViewModel> GetAllTotalPrice(Guid returnSaleInvoiceId)
        {
            var details = _unitOfWork.ReturnSaleInvoiceDetails.GetAllTotalPrice(returnSaleInvoiceId);

            var result = details.GroupBy(p => p.Currency.Name).Select(p => new ReturnSaleInvoiceTotalPriceViewModel
            {
                CurrencyName = p.Key,
                TotalDiscount = p.Sum(x => x.Discount.GetValueOrDefault(0)),
                TotalPrice = p.Sum(x => x.Num.GetValueOrDefault(0) * x.Price)
            }).ToList();
            Indexing<ReturnSaleInvoiceTotalPriceViewModel> indexing = new Indexing<ReturnSaleInvoiceTotalPriceViewModel>();
            return indexing.AddIndexing(result);
        }

        public ReturnSaleInvoiceViewModel GetReturnSaleInvoice(Guid returnSaleInvoiceId)
        {
            try
            {
                ReturnSaleInvoice invoice = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(returnSaleInvoiceId);
                var result = ConvertModel(invoice);
                result.TotalPrice = UpdateTotalPrice(invoice);

                return result;
            }
            catch { return null; }
        }

        public IEnumerable<ReturnSaleInvoiceViewModel> GetReceiveReturnSaleInvoice(Guid? payId)
        {
            var invoices = _unitOfWork.ReturnSaleInvoices.GetReceiveReturnSaleInvoice(payId);
            List<ReturnSaleInvoiceViewModel> hospconvert = ConvertCustomeModelsLists(invoices);
            Indexing<ReturnSaleInvoiceViewModel> indexing = new Indexing<ReturnSaleInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReturnSaleInvoiceViewModel> GetNotReceiveReturnSaleInvoice(Guid? customerId)
        {
            var invoices = _unitOfWork.ReturnSaleInvoices.GetNotReceiveReturnSaleInvoice(customerId);
            List<ReturnSaleInvoiceViewModel> hospconvert = ConvertCustomeModelsLists(invoices);
            Indexing<ReturnSaleInvoiceViewModel> indexing = new Indexing<ReturnSaleInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        //public IEnumerable<ReturnSaleInvoiceViewModel> GetPartialReceiveReturnSaleInvoice(Guid? payId)
        //{
        //    var invoices = _unitOfWork.ReturnSaleInvoices.GetPartialReceiveReturnSaleInvoice(payId);
        //    List<ReturnSaleInvoiceViewModel> hospconvert = ConvertCustomePartialModelsLists(invoices);
        //    Indexing<ReturnSaleInvoiceViewModel> indexing = new Indexing<ReturnSaleInvoiceViewModel>();
        //    return indexing.AddIndexing(hospconvert);
        //}

        //public IEnumerable<ReturnSaleInvoiceViewModel> GetNotPartialReceiveReturnSaleInvoice(Guid? customerId, int? currencyId, Guid? payId)
        //{
        //    var invoices = _unitOfWork.ReturnSaleInvoices.GetNotPartialReceiveReturnSaleInvoice(customerId, currencyId, payId);
        //    List<ReturnSaleInvoiceViewModel> hospconvert = ConvertCustomePartialModelsLists(invoices);
        //    Indexing<ReturnSaleInvoiceViewModel> indexing = new Indexing<ReturnSaleInvoiceViewModel>();
        //    return indexing.AddIndexing(hospconvert);
        //}

        public bool CheckReturnSaleInvoiceReceived(Guid returnSaleInvoiceId)
        {
            var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(returnSaleInvoiceId);
            return can_change;
        }

        // Begin Convert 
        public ReturnSaleInvoice ConvertModel(ReturnSaleInvoiceViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnSaleInvoiceViewModel, ReturnSaleInvoice>()
                .ForMember(a => a.ReturnSaleInvoiceDetails, b => b.Ignore());

                //.ForMember(a => a.ReturnSaleInvoiceTypeName, b => b.MapFrom(c => c.ReturnSaleInvoiceType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnSaleInvoiceViewModel, ReturnSaleInvoice>(Users);
        }

        public ReturnSaleInvoiceViewModel ConvertModel(ReturnSaleInvoice Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnSaleInvoice, ReturnSaleInvoiceViewModel>()
                //.ForMember(a => a.ReturnSaleInvoiceDetails, b => b.Ignore())
                .ForMember(a => a.CanChange, b => b.MapFrom(c => !c.ReturnSaleInvoiceDetails.Any()))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnSaleInvoice, ReturnSaleInvoiceViewModel>(Users);
        }

        //public List<ReturnSaleInvoiceViewModel> ConvertCustomePartialModelsLists(IEnumerable<PartialReceiveModel> purchaseInvoices)
        //{
        //      CultureInfo cultures = new CultureInfo("en-US");
        //    List<ReturnSaleInvoiceViewModel> purchaseInvoiceDtoList = new List<ReturnSaleInvoiceViewModel>();
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<PartialReceiveModel, ReturnSaleInvoiceViewModel>()
        //        .ForMember(a => a.InvoiceDateTxt, b => b.MapFrom(c => c.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)))
        //        ;
        //    });

        //    IMapper mapper = config.CreateMapper();
        //    purchaseInvoiceDtoList = mapper.Map<IEnumerable<PartialReceiveModel>, List<ReturnSaleInvoiceViewModel>>(purchaseInvoices);
        //    return purchaseInvoiceDtoList;
        //}

        public List<ReturnSaleInvoiceViewModel> ConvertCustomeModelsLists(IEnumerable<ReturnSaleInvoice> purchaseInvoices)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            List<ReturnSaleInvoiceViewModel> purchaseInvoiceDtoList = new List<ReturnSaleInvoiceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnSaleInvoice, ReturnSaleInvoiceViewModel>()
                .ForMember(a => a.InvoiceDateTxt, b => b.MapFrom(c => c.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)))
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDtoList = mapper.Map<IEnumerable<ReturnSaleInvoice>, List<ReturnSaleInvoiceViewModel>>(purchaseInvoices);
            return purchaseInvoiceDtoList;
        }
        // End Convert
    }
}
