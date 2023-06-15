using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Ambulance;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Hospital;
using WPH.Models.Customer;
using WPH.MvcMockingServices.Interface;
using WPH.Models.CustomerAccount;
using System.Globalization;
using static Common.Enums;
using Microsoft.Extensions.Localization;
using WPH.Models.SupplierAccount;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class CustomerMvcMockingService : ICustomerMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public OperationStatus RemoveCustomer(Guid Customerid)
        {
            try
            {
                Customer Hos = _unitOfWork.Customers.Get(Customerid);
                _unitOfWork.Customers.Remove(Hos);
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

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Customer/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewCustomer(CustomerViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Name))
                return "DataNotValid";

            if (!string.IsNullOrWhiteSpace(viewModel.PhoneNumber) && viewModel.PhoneNumber.Length < 8)
                return "WrongMobile";

            var exist_user = _unitOfWork.Customers.CheckCustomerExistBaseOnName(viewModel.ClinicSectionId.Value, viewModel.Name);
            if (exist_user)
                return "ValueIsRepeated";

            Customer customer = Common.ConvertModels<Customer, CustomerViewModel>.convertModels(viewModel);
            customer.CreateDate = DateTime.Now;

            customer.User = new User
            {
                ClinicSectionId = customer.ClinicSectionId,
                Name = viewModel.Name,
                UserName = "cus",
                Pass1 = "123",
                PhoneNumber = viewModel.PhoneNumber,
            };

            if (!string.IsNullOrWhiteSpace(viewModel.CityName))
            {

                var city = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CityName, "City", viewModel.ClinicSectionId.Value);
                customer.CityId = city?.BaseInfos?.FirstOrDefault()?.Guid;
                if (customer.CityId == null)
                {
                    customer.City = new BaseInfo
                    {
                        Name = viewModel.CityName,
                        ClinicSectionId = viewModel.ClinicSectionId.Value,
                        TypeId = city.Guid
                    };

                    _unitOfWork.BaseInfos.Add(customer.City);
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.CustomerTypeName))
            {
                var customerType = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CustomerTypeName, "CustomerType", viewModel.ClinicSectionId.Value);
                customer.CustomerTypeId = customerType?.BaseInfos?.FirstOrDefault()?.Guid;
                if (customer.CustomerTypeId == null)
                {
                    customer.CustomerType = new BaseInfo
                    {
                        Name = viewModel.CustomerTypeName,
                        ClinicSectionId = viewModel.ClinicSectionId,
                        TypeId = customerType.Guid
                    };

                    _unitOfWork.BaseInfos.Add(customer.CustomerType);
                }
            }

            _unitOfWork.Customers.Add(customer);
            _unitOfWork.Complete();
            return customer.Guid.ToString();
        }


        public string UpdateCustomer(CustomerViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Name))
                return "DataNotValid";

            if (!string.IsNullOrWhiteSpace(viewModel.PhoneNumber) && viewModel.PhoneNumber.Length < 8)
                return "WrongMobile";

            var exist_user = _unitOfWork.Customers.CheckCustomerExistBaseOnName(viewModel.ClinicSectionId.Value, viewModel.Name, p => p.Guid != viewModel.Guid);
            if (exist_user)
                return "ValueIsRepeated";

            Customer customer = _unitOfWork.Customers.GetCustomerWithUser(viewModel.Guid);
            customer.ModifiedDate = DateTime.Now;
            customer.Address = viewModel.Address;
            customer.Description = viewModel.Description;

            customer.User.Name = viewModel.Name;
            customer.User.PhoneNumber = viewModel.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(viewModel.CityName))
            {
                var city = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CityName, "City", viewModel.ClinicSectionId.Value);
                customer.CityId = city?.BaseInfos?.FirstOrDefault()?.Guid;
                if (customer.CityId == null)
                {
                    customer.City = new BaseInfo
                    {
                        Name = viewModel.CityName,
                        ClinicSectionId = viewModel.ClinicSectionId.Value,
                        TypeId = city.Guid
                    };

                    _unitOfWork.BaseInfos.Add(customer.City);
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.CustomerTypeName))
            {
                var customerType = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CustomerTypeName, "CustomerType", viewModel.ClinicSectionId.Value);
                customer.CustomerTypeId = customerType?.BaseInfos?.FirstOrDefault()?.Guid;
                if (customer.CustomerTypeId == null)
                {
                    customer.CustomerType = new BaseInfo
                    {
                        Name = viewModel.CustomerTypeName,
                        ClinicSectionId = viewModel.ClinicSectionId,
                        TypeId = customerType.Guid
                    };

                    _unitOfWork.BaseInfos.Add(customer.CustomerType);
                }
            }

            _unitOfWork.Customers.UpdateState(customer);
            _unitOfWork.Users.UpdateState(customer.User);
            _unitOfWork.Complete();
            return customer.Guid.ToString();
        }

        public IEnumerable<CustomerViewModel> GetAllCustomers(Guid clinicSectionId)
        {
            IEnumerable<Customer> hosp = _unitOfWork.Customers.GetAllCustomer(clinicSectionId);
            List<CustomerViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            Indexing<CustomerViewModel> indexing = new Indexing<CustomerViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<CustomerViewModel> GetAllCustomersName(Guid clinicSectionId)
        {
            IEnumerable<Customer> hosp = _unitOfWork.Customers.GetAllCustomerName(clinicSectionId);
            List<CustomerViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            return hospconvert;
        }

        public IEnumerable<CustomerAccountViewModel> GetAllCustomerAccount(CustomerAccountFilterViewModel viewModel)
        {
            try
            {
                if (viewModel.CustomerId == null)
                    return new List<CustomerAccountViewModel>();

                if (viewModel.Year == 0)
                {
                    viewModel.DateFrom = viewModel.DateTo = DateTime.Now;

                    if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateFrom))
                        viewModel.DateFrom = DateFrom;

                    if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateTo))
                        viewModel.DateTo = DateTo;
                }
                else
                {
                    viewModel.DateFrom = new DateTime(viewModel.Year, 1, 1, 0, 0, 0);
                    viewModel.DateTo = new DateTime(viewModel.Year, 12, 31, 23, 59, 59);
                }

                var filter = (SupplierFilter)viewModel.CustomerFilter;
                var total = _unitOfWork.Customers.GetAllCustomerAccount(viewModel.CustomerId.Value, viewModel.CurrencyId, filter, viewModel.DateFrom, viewModel.DateTo);

                CultureInfo cultures = new CultureInfo("en-US");
                var result = total.GroupBy(g => g.Guid).Select(p => new CustomerAccountViewModel
                {
                    Guid = p.Key,
                    RecordType = p.FirstOrDefault()?.RecordType,
                    Date = p.FirstOrDefault()?.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                    Invoices = string.Join(",", p.Select(s => s.ReceiveInvoiceNum).Where(w => !string.IsNullOrWhiteSpace(w))),
                    ReturnInvoices = string.Join(",", p.Select(s => s.RetunInvoiceNum).Where(w => !string.IsNullOrWhiteSpace(w))),
                    InvoiceNum = p.FirstOrDefault()?.InvoiceNum,
                    Description = p.FirstOrDefault()?.Description,
                    ReceiveAmount = p.FirstOrDefault()?.ReceiveAmount,
                    GetAmount = p.FirstOrDefault()?.GetAmount,
                    ReceiveStatus = p.FirstOrDefault()?.ReceiveStatus
                }).ToList();

                Indexing<CustomerAccountViewModel> indexing = new Indexing<CustomerAccountViewModel>();
                return indexing.AddIndexing(result);
            }
            catch (Exception e) { throw; }

        }

        public CustomerViewModel GetCustomer(Guid CustomerId)
        {
            try
            {
                Customer Customergu = _unitOfWork.Customers.GetCustomer(CustomerId);
                return ConvertModel(Customergu);
            }
            catch { return null; }
        }

        public CustomerViewModel GetCustomerName(Guid supplierId)
        {
            Customer Suppliergu = _unitOfWork.Customers.GetCustomerName(supplierId);
            return ConvertModel(Suppliergu);

        }



        public CustomerAccountReportResultViewModel GetCustomerAccountReport(CustomerAccountReportFilterViewModel viewModel, IStringLocalizer<SharedResource> _localizer)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            var purchase_txt = _localizer["Sale"].Value;
            var return_txt = _localizer["Return"].Value;
            string rem = "";
            var result = new CustomerAccountReportResultViewModel();

            bool? receive_filter = null, purchase_filter = null;
            switch ((ReceiveReportFilter)viewModel.FilterId)
            {
                case ReceiveReportFilter.All:
                    {
                        receive_filter = null;
                        purchase_filter = null;
                    }
                    break;
                case ReceiveReportFilter.UnpaidInvoice:
                    {
                        receive_filter = false;
                        purchase_filter = null;
                    }
                    break;
                case ReceiveReportFilter.UnpaidInvoice_Sale:
                    {
                        receive_filter = false;
                        purchase_filter = true;
                    }
                    break;
                case ReceiveReportFilter.UnpaidInvoice_ReturnSale:
                    {
                        receive_filter = false;
                        purchase_filter = false;
                    }
                    break;
                case ReceiveReportFilter.PaidInvoice:
                    {
                        receive_filter = true;
                        purchase_filter = null;
                    }
                    break;
                case ReceiveReportFilter.PaidInvoice_Sale:
                    {
                        receive_filter = true;
                        purchase_filter = true;
                    }
                    break;
                case ReceiveReportFilter.PaidInvoice_ReturnSale:
                    {
                        receive_filter = true;
                        purchase_filter = false;
                    }
                    break;
            }

            Customer report;

            if (viewModel.Detail)
            {
                report = _unitOfWork.Customers.GetCustomerAccountDetailReport(viewModel.CustomerId, receive_filter, purchase_filter, viewModel.CurrencyName ?? "", viewModel.CurrencyId, viewModel.FromDate, viewModel.ToDate);

                if (!purchase_filter.GetValueOrDefault(true))
                {
                    report.SaleInvoices = new List<SaleInvoice>();
                }

                if (purchase_filter.GetValueOrDefault(false))
                {
                    report.ReturnSaleInvoices = new List<ReturnSaleInvoice>();
                }

                List<string> dd = new List<string> { "0" };

                report.SaleInvoices = report.SaleInvoices.Select(x =>
                {
                    x.TotalDiscount = string.Join("_", !x.SaleInvoiceDiscounts.Any() ? dd :
                        x.SaleInvoiceDiscounts.GroupBy(g => g.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}"));
                    return x;
                }).ToList();

                report.ReturnSaleInvoices = report.ReturnSaleInvoices.Select(x =>
                {
                    x.TotalDiscount = string.Join("_", !x.ReturnSaleInvoiceDiscounts.Any() ? dd :
                        x.ReturnSaleInvoiceDiscounts.GroupBy(g => g.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}"));
                    return x;
                }).ToList();

                var purchase = report.SaleInvoices.SelectMany(p => p.SaleInvoiceDetails, (p, s) => new
                {
                    p.Guid,
                    TempDate = p.InvoiceDate.Value,
                    p.InvoiceNum,
                    p.Description,
                    TotalPrice = SplitCurrency(p.TotalPrice, viewModel.CurrencyName),
                    p.TotalDiscount,
                    EnInvoiceType = "Sale",
                    InvoiceType = purchase_txt,
                    ProductName = s.Product.Name,
                    ProductType = s.Product.ProductType.Name,
                    ProducerName = s.Product.Producer.Name,
                    ExpiryDate = s.TransferDetail == null ? s.PurchaseInvoiceDetail.ExpireDate.Value : s.TransferDetail.ExpireDate.Value,
                    Num = s.Num.GetValueOrDefault(0),
                    FreeNum = s.FreeNum.GetValueOrDefault(0),
                    s.SalePrice,
                    Currency = s.CurrencyName,
                    Discount = s.Discount.GetValueOrDefault(0),
                }).ToList();

                purchase.AddRange(report.ReturnSaleInvoices.SelectMany(p => p.ReturnSaleInvoiceDetails, (p, s) => new
                {
                    p.Guid,
                    TempDate = p.InvoiceDate.Value,
                    p.InvoiceNum,
                    p.Description,
                    TotalPrice = SplitCurrency(p.TotalPrice, viewModel.CurrencyName),
                    p.TotalDiscount,
                    EnInvoiceType = "Return",
                    InvoiceType = return_txt,
                    ProductName =  s.SaleInvoiceDetail.PurchaseInvoiceDetail.Product.Name ,
                    ProductType =  s.SaleInvoiceDetail.PurchaseInvoiceDetail.Product.ProductType.Name ,
                    ProducerName =  s.SaleInvoiceDetail.PurchaseInvoiceDetail.Product.Producer.Name ,
                    ExpiryDate = s.SaleInvoiceDetail.TransferDetail == null ? s.SaleInvoiceDetail.PurchaseInvoiceDetail.ExpireDate.Value : s.SaleInvoiceDetail.TransferDetail.ExpireDate.Value,
                    Num = s.Num.GetValueOrDefault(0),
                    FreeNum = s.FreeNum.GetValueOrDefault(0),
                    SalePrice = s.Price,
                    Currency = s.CurrencyName,
                    Discount = s.Discount.GetValueOrDefault(0),
                }));

                result.AllDetailSale = purchase.OrderBy(p => p.TempDate).Select(p => new CustomerAccountReportDetailViewModel
                {
                    Guid = p.Guid.ToString(),
                    Date = p.TempDate.ToString("dd/MM/yyyy", cultures),
                    InvoiceNum = p.InvoiceNum,
                    Description = p.Description,
                    EnInvoiceType = p.EnInvoiceType,
                    InvoiceType = p.InvoiceType,
                    ProductName = p.ProductName,
                    ProductType = p.ProductType,
                    ProducerName = p.ProducerName,
                    ExpiryDate = p.ExpiryDate.ToString("dd/MM/yyyy", cultures),
                    TempNum = p.Num,
                    TempFreeNum = p.FreeNum,
                    TempSalePrice = p.SalePrice.GetValueOrDefault(0),
                    TempDiscount = p.Discount,
                    TotalDiscount = p.TotalDiscount.Replace("_", "<br>"),
                    TotalPrice = p.TotalPrice.Replace("_", "<br>"),
                    CurrencyName = p.Currency
                });


                var total_purchase = report.SaleInvoices.SelectMany(p => p.SaleInvoiceDetails).Select(p => new
                {
                    p.CurrencyName,
                    TotalDiscount = p.Discount.GetValueOrDefault(0),
                    TotalPrice = p.Num.GetValueOrDefault(0) * p.SalePrice.GetValueOrDefault(0)
                }).ToList();

                total_purchase.AddRange(report.SaleInvoices.SelectMany(p => p.SaleInvoiceDiscounts).Select(p => new
                {
                    p.CurrencyName,
                    TotalDiscount = p.Amount.GetValueOrDefault(0),
                    TotalPrice = (decimal)0
                }).ToList());

                var total_result = total_purchase.GroupBy(p => p.CurrencyName).Select(p => new
                {
                    CurrencyName = p.Key,
                    PriceAfterDiscount = p.Sum(s => s.TotalPrice - s.TotalDiscount)
                }).ToList();

                var returns = report.ReturnSaleInvoices.SelectMany(p => p.ReturnSaleInvoiceDetails).Select(p => new
                {
                    p.CurrencyName,
                    TotalDiscount = p.Discount.GetValueOrDefault(0),
                    TotalPrice = p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0)
                }).ToList();

                returns.AddRange(report.ReturnSaleInvoices.SelectMany(p => p.ReturnSaleInvoiceDiscounts).Select(p => new
                {
                    p.CurrencyName,
                    TotalDiscount = p.Amount.GetValueOrDefault(0),
                    TotalPrice = (decimal)0
                }).ToList());

                var total_return = returns.GroupBy(p => p.CurrencyName).Select(p => new
                {
                    CurrencyName = p.Key,
                    PriceAfterDiscount = p.Sum(s => s.TotalPrice - s.TotalDiscount)
                }).ToList();

                total_result.AddRange(total_return.Select(p => new
                {
                    p.CurrencyName,
                    PriceAfterDiscount = -p.PriceAfterDiscount
                }).ToList());

                rem = string.Join("_", total_result.GroupBy(p => p.CurrencyName)
                    .Select(p => $"{p.Key} {p.Sum(s => s.PriceAfterDiscount).ToString("#,0.##", cultures)}"));

            }
            else
            {
                report = _unitOfWork.Customers.GetCustomerAccountReport(viewModel.CustomerId, receive_filter, purchase_filter, viewModel.CurrencyName ?? "", viewModel.FromDate, viewModel.ToDate);

                if (!purchase_filter.GetValueOrDefault(true))
                {
                    report.SaleInvoices = new List<SaleInvoice>();
                }

                if (purchase_filter.GetValueOrDefault(false))
                {
                    report.ReturnSaleInvoices = new List<ReturnSaleInvoice>();
                }

                var purchase = report.SaleInvoices.Select(p => new
                {
                    TempDate = p.InvoiceDate.Value,
                    p.InvoiceNum,
                    p.Description,
                    EnInvoiceType = "Sale",
                    InvoiceType = purchase_txt,
                    TempTotalPrice = SplitCurrency(p.TotalPrice, viewModel.CurrencyName),
                    TempDiscount = string.Join("_", p.SaleInvoiceDiscounts.GroupBy(p => p.CurrencyName).Select(x => $"{x.Key} {x.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}")),
                    TotalAfterDiscount = SplitCurrency(p.TotalPrice, viewModel.CurrencyName).Replace("_", "<br>")
                }).ToList();

                purchase.AddRange(report.ReturnSaleInvoices.Select(p => new
                {
                    TempDate = p.InvoiceDate.Value,
                    p.InvoiceNum,
                    p.Description,
                    EnInvoiceType = "Return",
                    InvoiceType = return_txt,
                    TempTotalPrice = SplitCurrency(p.TotalPrice, viewModel.CurrencyName),
                    TempDiscount = string.Join("_", p.ReturnSaleInvoiceDiscounts.GroupBy(p => p.CurrencyName).Select(x => $"{x.Key} {x.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}")),
                    TotalAfterDiscount = SplitCurrency(p.TotalPrice, viewModel.CurrencyName).Replace("_", "<br>")
                }));

                result.AllSale = purchase.OrderBy(p => p.TempDate).Select(p => new CustomerAccountReportViewModel
                {
                    Date = p.TempDate.ToString("dd/MM/yyyy", cultures),
                    InvoiceNum = p.InvoiceNum,
                    Description = p.Description,
                    EnInvoiceType = p.EnInvoiceType,
                    InvoiceType = p.InvoiceType,
                    TotalPrice = AddDiscount(p.TempDiscount, p.TempTotalPrice, viewModel.CurrencyName, cultures),
                    Discount = string.IsNullOrWhiteSpace(p.TempDiscount) ? "0" : p.TempDiscount.Replace("_", "<br>"),
                    TotalAfterDiscount = p.TotalAfterDiscount,
                    TempRem = rem = CalculateTotal(rem, p.TempTotalPrice, p.EnInvoiceType, viewModel.CurrencyName, cultures)
                }).ToList();

            }

            result.CustomerName = report.User.Name;
            var Total = new SupplierAccountReportTotalViewModel
            {
                ShowContent = _localizer["Total"],
                ShowValue = rem.Replace("_", "<br>")
            };

            var purchaseTotal = report.SaleInvoices.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() }).Where(p => p.CurrencyName.Contains(viewModel.CurrencyName ?? ""))
                .GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount).ToString("#,0.##", cultures)}");

            var SaleTotal = new SupplierAccountReportTotalViewModel
            {
                ShowContent = _localizer["TotalSale"],
                ShowValue = string.Join("<br>", purchaseTotal),
            };

            var returnTotal = report.ReturnSaleInvoices.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() }).Where(p => p.CurrencyName.Contains(viewModel.CurrencyName ?? ""))
                .GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount).ToString("#,0.##", cultures)}");

            var ReturnTotal = new SupplierAccountReportTotalViewModel
            {
                ShowContent = _localizer["TotalReturn"],
                ShowValue = string.Join("<br>", returnTotal)
            };

            result.Results = new List<SupplierAccountReportTotalViewModel>
            {
                SaleTotal,
                ReturnTotal,
                Total
            };

            return result;
        }

        private static string SplitCurrency(string amount, string currencyName)
        {
            if (string.IsNullOrWhiteSpace(currencyName))
                return amount;

            string res = "";

            var total_money = amount.Split("_");
            foreach (var item in total_money)
            {
                if (item.Contains(currencyName))
                {
                    res = item;
                    break;
                }
            }
            return res;
        }

        private static string AddDiscount(string discount, string total, string currencyName, CultureInfo cultures)
        {
            if (string.IsNullOrWhiteSpace(discount))
                return total.Replace("_", "<br>");

            var total_dis = discount.Split("_").OrderBy(p => p).ToList();
            var amount = total.Split("_").OrderBy(p => p).ToList();
            string result = "";

            for (int i = 0; i < amount.Count; i++)
            {
                var money = amount[i].Split(" ");
                var add_dis = total_dis.FirstOrDefault(p => p.Contains(money[0]));

                if (string.IsNullOrWhiteSpace(add_dis))
                {

                    result += $"{money[0]} {money[1]}_";
                }
                else
                {
                    var sum = money[1].GetDecimalNumber();
                    var dis = add_dis.GetDecimalNumber();

                    sum += dis;

                    result += $"{money[0]} {sum.ToString("#,0.##", cultures)}_";
                }
            }

            if (result.EndsWith("_"))
                result = result.Remove(result.Length - 1);

            return SplitCurrency(result, currencyName).Replace("_", "<br>");
        }

        private static string CalculateTotal(string last, string total, string type, string currencyName, CultureInfo cultures)
        {
            //if (string.IsNullOrWhiteSpace(last))
            //    return total;

            var amount = total.Split("_");
            string CurrentRemain = "";

            if (type == "Sale")
            {

                for (int i = 0; i < amount.Length; i++)
                {
                    var money = amount[i].Split(" ");

                    if (money.Length > 1)
                    {

                        if (last.Contains(money[0]))
                        {
                            var last_res = last.Split("_");

                            for (int j = 0; j < last_res.Length; j++)
                            {
                                if (last_res[j].Contains(money[0]))
                                {
                                    var last_money = last_res[j].Split(" ");

                                    var sum = money[1].GetDecimalNumber();
                                    var last_sum = last_money[1].GetDecimalNumber();

                                    sum += last_sum;

                                    CurrentRemain += $"{money[0]} {sum.ToString("#,0.##", cultures)}_";
                                }
                            }

                        }
                        else
                        {

                            CurrentRemain += $"{money[0]} {money[1]}_";
                        }

                    }
                }

            }
            else
            {

                for (int i = 0; i < amount.Length; i++)
                {
                    var money = amount[i].Split(" ");

                    if (money.Length > 1)
                    {

                        if (last.Contains(money[0]))
                        {
                            var last_res = last.Split("_");

                            for (int j = 0; j < last_res.Length; j++)
                            {
                                if (last_res[j].Contains(money[0]))
                                {
                                    var last_money = last_res[j].Split(" ");

                                    var sum = money[1].GetDecimalNumber();
                                    var last_sum = last_money[1].GetDecimalNumber();

                                    sum = -sum;

                                    sum += last_sum;

                                    CurrentRemain += $"{money[0]} {sum.ToString("#,0.##", cultures)}_";
                                }
                            }

                        }
                        else
                        {

                            CurrentRemain += $"{money[0]} -{money[1]}_";
                        }

                    }
                }


            }


            var last_rem = last.Split("_");
            var rem = CurrentRemain;
            for (var i = 0; i < last_rem.Length; i++)
            {
                var money = last_rem[i].Split(" ");
                if (!rem.Contains(money[0]) && money.Length > 1)
                {
                    CurrentRemain += $"{money[0]} {money[1]}_";
                }
            }

            if (CurrentRemain.EndsWith("_"))
                CurrentRemain = CurrentRemain.Remove(CurrentRemain.Length - 1);

            if (string.IsNullOrWhiteSpace(currencyName))
                return CurrentRemain;

            return SplitCurrency(CurrentRemain, currencyName);
        }


        // Begin Convert 
        public CustomerViewModel ConvertModel(Customer Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerViewModel>()
                .ForMember(a => a.CityName, b => b.MapFrom(c => c.City.Name))
                .ForMember(a => a.CustomerTypeName, b => b.MapFrom(c => c.CustomerType.Name))
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber));
                //cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Customer, CustomerViewModel>(Users);
        }

        public List<CustomerViewModel> ConvertModelsLists(IEnumerable<Customer> Customers)
        {
            List<CustomerViewModel> CustomerDtoList = new List<CustomerViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerViewModel>()
                .ForMember(a => a.CityName, b => b.MapFrom(c => c.City.Name))
                .ForMember(a => a.CustomerTypeName, b => b.MapFrom(c => c.CustomerType.Name))
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber));
                //cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            CustomerDtoList = mapper.Map<IEnumerable<Customer>, List<CustomerViewModel>>(Customers);
            return CustomerDtoList;
        }


        // End Convert
    }
}
