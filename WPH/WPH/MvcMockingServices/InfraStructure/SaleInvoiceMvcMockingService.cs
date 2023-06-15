using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.SaleInvoice;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class SaleInvoiceMvcMockingService : ISaleInvoiceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SaleInvoiceMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public string RemoveSaleInvoice(Guid SaleInvoiceid, Guid userId, string pass)
        {
            try
            {
                var check = _unitOfWork.Users.CheckUserByIdAndPass(userId, pass);
                if (!check)
                    return "WrongPass";

                var can_change = _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(SaleInvoiceid);
                if (can_change)
                    return "InvoiceInUse"; 

                //SaleInvoice Hos = _unitOfWork.SaleInvoices.Get(SaleInvoiceid);
                _unitOfWork.SaleInvoices.RemoveSaleInvoice(SaleInvoiceid);
                //_unitOfWork.Complete();
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

        public bool CheckSaleInvoiceRecieve(Guid id)
        {
            return _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(id);
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/SaleInvoice/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewSaleInvoice(SaleInvoiceViewModel viewModel)
        {
            //if (string.IsNullOrWhiteSpace(viewModel.MainInvoiceNum) || viewModel.SupplierId == null)
            //    return "DataNotValid";

            if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                return "DateNotValid";

            SaleInvoice saleInvoice = Common.ConvertModels<SaleInvoice, SaleInvoiceViewModel>.convertModels(viewModel);

            saleInvoice.CreatedDate = DateTime.Now;
            saleInvoice.InvoiceDate = invoiceDate;
            saleInvoice.InvoiceNum = GetSaleInvoiceNum(viewModel.ClinicSectionId.Value);
            saleInvoice.OldFactor = false;

            //if (saleInvoice.Discount == null)
            //    saleInvoice.Discount = 0;

            _unitOfWork.SaleInvoices.Add(saleInvoice);
            _unitOfWork.Complete();
            return saleInvoice.Guid.ToString();
        }

        public string UpdateSaleInvoice(SaleInvoiceViewModel viewModel)
        {
            //if (string.IsNullOrWhiteSpace(viewModel.MainInvoiceNum) || viewModel.SupplierId == null)
            //    return "DataNotValid";

            var can_change = _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(viewModel.Guid);
            if (can_change)
                return "InvoiceInUse";

            if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                return "DateNotValid";

            SaleInvoice saleInvoice = _unitOfWork.SaleInvoices.Get(viewModel.Guid);

            saleInvoice.ModifiedDate = DateTime.Now;
            saleInvoice.ModifiedUserId = viewModel.ModifiedUserId;
            //saleInvoice.InvoiceDate = invoiceDate;
            //saleInvoice.MainInvoiceNum = viewModel.MainInvoiceNum;
            //saleInvoice.SupplierId = viewModel.SupplierId;
            //saleInvoice.CurrencyId = viewModel.CurrencyId;
            //saleInvoice.Discount = viewModel.Discount;
            //saleInvoice.Description = viewModel.Description;

            //if (saleInvoice.Discount == null)
            //    saleInvoice.Discount = 0;

            _unitOfWork.SaleInvoices.UpdateState(saleInvoice);
            _unitOfWork.Complete();
            return saleInvoice.Guid.ToString();
        }

        public string GetSaleInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                string saleInvoiceNum = _unitOfWork.SaleInvoices.GetLatestSaleInvoiceNum(clinicSectionId);
                return NextSaleInvoiceNum(saleInvoiceNum);
            }
            catch (Exception ex) { return "1"; }
        }

        public string NextSaleInvoiceNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }

        public IEnumerable<SaleInvoiceViewModel> GetAllSaleInvoices(Guid clinicSectionId, SaleInvoiceFilterViewModel filterViewModel, IStringLocalizer<SharedResource> _localizer)
        {

            if (filterViewModel.PeriodId != (int)Periods.FromDateToDate)
            {
                var DateFrom = DateTime.Now;
                var DateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, filterViewModel.PeriodId);

                filterViewModel.DateFrom = DateFrom;
                filterViewModel.DateTo = DateTo;
            }

            IEnumerable<SaleInvoice> hosp = _unitOfWork.SaleInvoices
                .GetAllSaleInvoice(clinicSectionId, filterViewModel.DateFrom, filterViewModel.DateTo,
                filterViewModel.Customer, filterViewModel.InvoiceNum, filterViewModel.ProductId);


            //IEnumerable<SaleInvoice> hosp = _unitOfWork.SaleInvoices.GetAllSaleInvoice(clinicSectionId);
            List<SaleInvoiceViewModel> hospconvert = ConvertModelsLists(hosp, _localizer);
            Indexing<SaleInvoiceViewModel> indexing = new Indexing<SaleInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public SaleInvoiceViewModel GetSaleInvoiceByReceiveId(Guid receiveId)
        {
            try
            {
                SaleInvoice SaleInvoicegu = _unitOfWork.SaleInvoices.GetByReceiveId(receiveId);
                return ConvertModel(SaleInvoicegu);
            }
            catch { return null; }
        }
        
        public SaleInvoiceViewModel GetSaleInvoice(Guid SaleInvoiceId)
        {
            try
            {
                SaleInvoice SaleInvoicegu = _unitOfWork.SaleInvoices.GetWithType(SaleInvoiceId);
                return ConvertModel(SaleInvoicegu);
            }
            catch { return null; }
        }

        public string UpdateTotalPrice(IEnumerable<SaleInvoiceDetail> saleInvoice, SaleInvoiceDiscount saleInvoiceDiscount, string type, string currencyname)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var invoice = _unitOfWork.SaleInvoices.GetForUpdateTotalPrice(saleInvoice == null? saleInvoiceDiscount.SaleInvoiceId ?? Guid.Empty : saleInvoice.FirstOrDefault().SaleInvoiceId?? Guid.Empty);
            if(saleInvoice != null)
            {
                if(string.Compare(type, "add", true) == 0)
                {
                    foreach (var sale in saleInvoice)
                    {
                        invoice.SaleInvoiceDetails.Add(sale);
                    }
                }
                else if(string.Compare(type, "update", true) == 0)
                {
                    //invoice.SaleInvoiceDetails.ToList().RemoveAll(invoice.SaleInvoiceDetails.Where(a=> saleInvoice.Select(a=>a.Guid).Contains(a.Guid)));
                    foreach(var sale in saleInvoice)
                    {
                        invoice.SaleInvoiceDetails.Remove(invoice.SaleInvoiceDetails.FirstOrDefault(a => a.Guid == sale.Guid));
                        invoice.SaleInvoiceDetails.Add(sale);
                    }
                    
                }
                else
                {
                    foreach (var sale in saleInvoice)
                    {
                        invoice.SaleInvoiceDetails.Remove(invoice.SaleInvoiceDetails.FirstOrDefault(a => a.Guid == sale.Guid));
                    }
                    //invoice.SaleInvoiceDetails.Remove(invoice.SaleInvoiceDetails.FirstOrDefault(a => a.Guid == saleInvoice.Guid));
                    //invoice.SaleInvoiceDetails.ToList().RemoveAll(a => saleInvoice.Select(a => a.Guid).Contains(a.Guid));
                }
            }
            

            List<SaleInvoiceTotalPriceViewModel> prices = invoice.SaleInvoiceDetails.Select(p => new SaleInvoiceTotalPriceViewModel
            {
                CurrencyName = (p.Currency == null)? currencyname : p.Currency.Name,
                TotalDiscount = p.Discount.GetValueOrDefault(0),
                TotalPrice = p.Num.GetValueOrDefault(0) * p.SalePrice.GetValueOrDefault(0)
            }).ToList();

            if (saleInvoiceDiscount != null)
            {
                if (string.Compare(type, "add", true) == 0)
                {
                    invoice.SaleInvoiceDiscounts.Add(saleInvoiceDiscount);
                }
                else if (string.Compare(type, "update", true) == 0)
                {
                    invoice.SaleInvoiceDiscounts.Remove(invoice.SaleInvoiceDiscounts.FirstOrDefault(a => a.Guid == saleInvoiceDiscount.Guid));
                    invoice.SaleInvoiceDiscounts.Add(saleInvoiceDiscount);
                }
                else
                {
                    invoice.SaleInvoiceDiscounts.Remove(invoice.SaleInvoiceDiscounts.FirstOrDefault(a => a.Guid == saleInvoiceDiscount.Guid));
                }
            }
                
            prices.AddRange(invoice.SaleInvoiceDiscounts.Select(p => new SaleInvoiceTotalPriceViewModel
            {
                CurrencyName = (p.Currency == null) ? currencyname : p.Currency.Name,
                TotalDiscount = p.Amount.GetValueOrDefault(0),
                TotalPrice = 0
            }).ToList());

            var result = prices.GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.PriceAfterDiscount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}");

            invoice.TotalPrice = string.Join("_", result);
            _unitOfWork.SaleInvoices.UpdateState(invoice);
            _unitOfWork.Complete();

            return invoice.TotalPrice;
        }

        // Begin Convert 

        
        public IEnumerable<SaleInvoiceViewModel> GetReceiveSaleInvoice(Guid? ReceiveId)
        {
            if (ReceiveId == null || ReceiveId == Guid.Empty)
                return new List<SaleInvoiceViewModel>();

            var invoices = _unitOfWork.SaleInvoices.GetReceiveSaleInvoice(ReceiveId);
            List<SaleInvoiceViewModel> hospconvert = ConvertCustomeModelsLists(invoices);
            Indexing<SaleInvoiceViewModel> indexing = new Indexing<SaleInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<SaleInvoiceViewModel> GetNotReceiveSaleInvoice(Guid? customerId)
        {
            if (customerId == null || customerId == Guid.Empty)
                return new List<SaleInvoiceViewModel>();

            var invoices = _unitOfWork.SaleInvoices.GetNotReceiveSaleInvoice(customerId);
            List<SaleInvoiceViewModel> hospconvert = ConvertCustomeModelsLists(invoices);
            Indexing<SaleInvoiceViewModel> indexing = new Indexing<SaleInvoiceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public string GetTotalPrice(Guid saleInvoiceId)
        {
            return _unitOfWork.SaleInvoices.GetSingle(a=>a.Guid == saleInvoiceId).TotalPrice;
        }

        public IEnumerable<PieChartViewModel> GetAllIncomes(Guid clinicSectionId)
        {


            var allIncome = _unitOfWork.SaleInvoices.GetAllIncomes(clinicSectionId);


            var income = allIncome.GroupBy(a => new { a.Date, a.CurrencyName }).Select(a=>new IncomeModel 
            {
                Date = a.Key.Date,
                CurrencyName = a.Key.CurrencyName,
                SalePrice = a.Sum(a=>a.SalePrice)
            }).OrderByDescending(a=>a.Date);



            //PieChartViewModel pie = new PieChartViewModel
            //{
            //    Labels = result.Select(a => a.Label).ToArray(),
            //    Value = result.Select(a => Convert.ToInt32(a.Value ?? 0)).ToArray()
            //};

            return null;
        }

        public List<IncomeViewModel> GetAllStoreInCome(Guid clinicSectionId, string type)
        {
            List<IncomeModel> lastResult = _unitOfWork.SaleInvoices.GetAllStoreInCome(clinicSectionId).GroupBy(a=>new { a.CurrencyName, a.Date }).Select(a=>new IncomeModel 
            {
                CurrencyName = a.Key.CurrencyName,
                Date = a.Key.Date,
                SalePrice = a.Sum(a=>a.SalePrice)
            }).ToList();

            List<IncomeModel> all = new();

            IEnumerable<DateTime> allDate = lastResult.Select(a => a.Date).Distinct();

            IEnumerable<string> allCurrency = lastResult.Select(a => a.CurrencyName).Distinct();


            foreach(var date in allDate)
            {
                foreach(var cur in allCurrency)
                {

                    var iqd = lastResult.FirstOrDefault(a => a.CurrencyName == cur && a.Date == date);

                    if (iqd == null)
                        all.Add(new IncomeModel
                        {
                            CurrencyName = cur,
                            Date = date,
                            SalePrice = 0
                        });
                    else
                    {
                        all.Add(iqd);
                    }
                }
            }


            IEnumerable<IncomeModel> result;

            List<IncomeViewModel> income = new();

            List<PieChartModel> pi = new List<PieChartModel>();

            if (string.Compare(type, "day", true) == 0)
            {

                result = all.Where(a => a.Date >= DateTime.Now.AddDays(-7).Date);


                foreach(var cu in allCurrency)
                {
                    income.Add(new IncomeViewModel
                    {
                        CurrencyName = cu,
                        Date = result.Where(a=>a.CurrencyName == cu).Select(a=>a.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)).ToArray(),
                        Value = result.Where(a => a.CurrencyName == cu).Select(a=>a.SalePrice).ToArray()
                    });
                }

            }
            else if (string.Compare(type, "month", true) == 0)
            {

                result = all.Where(a => a.Date >= new DateTime(DateTime.Now.Year, 1, 1).Date);

                List<string> date = new();
                List<decimal> val = new();
                foreach (var cu in allCurrency)
                {

                    date = new();
                    val = new();
                    for (int i = 1; i < 13; i++)
                    {

                        DateTime thismonth = new DateTime(DateTime.Now.Year, i, 1);

                        string mo = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(thismonth.Month);

                        date.Add(mo);

                        var sale = result.Where(a => a.Date.Month == i && a.CurrencyName == cu);

                        if(sale.Count() == 0)
                            val.Add(0);
                        else
                            val.Add(sale.Sum(a => a.SalePrice));



                    }

                    income.Add(new IncomeViewModel
                    {
                        CurrencyName = cu,
                        Date = date.ToArray(),
                        Value = val.ToArray()
                    });


                }

            }
            else
            {

                int firstyear = all.Min(a => a.Date.Year);
                int currentyear = DateTime.Now.Year;
                
                List<string> date = new();
                List<decimal> val = new();
                foreach (var cu in allCurrency)
                {
                    date = new();
                    val = new();
                    for (int i = firstyear; i <= currentyear; i++)
                    {
                        
                        date.Add(firstyear.ToString());

                        var sale = all.Where(a => a.Date.Year == i && a.CurrencyName == cu);

                        if (sale.Count() == 0)
                            val.Add(0);
                        else
                            val.Add(sale.Sum(a => a.SalePrice));

                    }

                    income.Add(new IncomeViewModel
                    {
                        CurrencyName = cu,
                        Date = date.ToArray(),
                        Value = val.ToArray()
                    });

                }

                    
            }


            return income;
        }



        public SaleInvoiceViewModel ConvertModel(SaleInvoice Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaleInvoice, SaleInvoiceViewModel>()
                //.ForMember(a => a.SaleInvoiceTypeName, b => b.MapFrom(c => c.SaleInvoiceType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<SaleInvoice, SaleInvoiceViewModel>(Users);
        }
        public List<SaleInvoiceViewModel> ConvertModelsLists(IEnumerable<SaleInvoice> saleInvoices, IStringLocalizer<SharedResource> _localizer)
        {
            List<SaleInvoiceViewModel> saleInvoiceDtoList = new List<SaleInvoiceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaleInvoice, SaleInvoiceViewModel>()
                .ForMember(a => a.CustomerName, b => b.MapFrom(c => c.Customer.User.Name))
                .ForMember(a => a.SalePriceTypeName, b => b.MapFrom(c => c.SalePriceType.Name))
                .ForMember(a => a.Status, b => b.MapFrom(c => c.Status ? _localizer["Paid"] : _localizer["UnPaid"])  )
                .ForMember(a => a.Customer, b => b.Ignore())
                .ForMember(a => a.SalePriceType, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            saleInvoiceDtoList = mapper.Map<IEnumerable<SaleInvoice>, List<SaleInvoiceViewModel>>(saleInvoices);
            return saleInvoiceDtoList;
        }

        public List<SaleInvoiceViewModel> ConvertCustomePartialModelsLists(IEnumerable<PartialPayModel> purchaseInvoices)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            List<SaleInvoiceViewModel> purchaseInvoiceDtoList = new List<SaleInvoiceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PartialPayModel, SaleInvoiceViewModel>()
                .ForMember(a => a.InvoiceDateTxt, b => b.MapFrom(c => c.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)))
                .ForMember(a => a.SaleInvoiceDetails, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDtoList = mapper.Map<IEnumerable<PartialPayModel>, List<SaleInvoiceViewModel>>(purchaseInvoices);
            return purchaseInvoiceDtoList;
        }

        public List<SaleInvoiceViewModel> ConvertCustomeModelsLists(IEnumerable<SaleInvoice> purchaseInvoices)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            List<SaleInvoiceViewModel> purchaseInvoiceDtoList = new List<SaleInvoiceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaleInvoice, SaleInvoiceViewModel>()
                .ForMember(a => a.InvoiceDateTxt, b => b.MapFrom(c => c.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)))
                .ForMember(a => a.SaleInvoiceDetails, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDtoList = mapper.Map<IEnumerable<SaleInvoice>, List<SaleInvoiceViewModel>>(purchaseInvoices);
            return purchaseInvoiceDtoList;
        }

        





        // End Convert
    }
}
