using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Supplier;
using WPH.Models.SupplierAccount;
using WPH.MvcMockingServices.Interface;
using static Common.Enums;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class SupplierMvcMockingService : ISupplierMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SupplierMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public OperationStatus RemoveSupplier(Guid Supplierid)
        {
            try
            {
                Supplier Hos = _unitOfWork.Suppliers.Get(Supplierid);
                _unitOfWork.Suppliers.Remove(Hos);
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
            string controllerName = "/Supplier/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewSupplier(SupplierViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Name))
                return "DataNotValid";

            if (!string.IsNullOrWhiteSpace(viewModel.PhoneNumber) && viewModel.PhoneNumber.Length < 8)
                return "WrongMobile";

            var exist_user = _unitOfWork.Suppliers.CheckSupplierExistBaseOnName(viewModel.ClinicSectionId.Value, viewModel.Name);
            if (exist_user)
                return "ValueIsRepeated";

            Supplier supplier = Common.ConvertModels<Supplier, SupplierViewModel>.convertModels(viewModel);

            supplier.User = new User
            {
                ClinicSectionId = supplier.ClinicSectionId,
                Name = viewModel.Name,
                UserName = "sup",
                Pass1 = "123",
                PhoneNumber = viewModel.PhoneNumber,
            };

            if (!string.IsNullOrWhiteSpace(viewModel.CityName))
            {
                var city = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CityName, "City", viewModel.ClinicSectionId.Value);
                supplier.CityId = city?.BaseInfos?.FirstOrDefault()?.Guid;
                if (supplier.CityId == null)
                {
                    supplier.City = new BaseInfo
                    {
                        Name = viewModel.CityName,
                        ClinicSectionId = viewModel.ClinicSectionId.Value,
                        TypeId = city.Guid
                    };

                    _unitOfWork.BaseInfos.Add(supplier.City);
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.CountryName))
            {
                var country = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CountryName, "Country", viewModel.ClinicSectionId.Value);
                supplier.CountryId = country?.BaseInfos?.FirstOrDefault()?.Guid;
                if (supplier.CountryId == null)
                {
                    supplier.Country = new BaseInfo
                    {
                        Name = viewModel.CountryName,
                        ClinicSectionId = viewModel.ClinicSectionId,
                        TypeId = country.Guid
                    };

                    _unitOfWork.BaseInfos.Add(supplier.Country);
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.SupplierTypeName))
            {
                var supplierType = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.SupplierTypeName, "SupplierType", viewModel.ClinicSectionId.Value);
                supplier.SupplierTypeId = supplierType?.BaseInfos?.FirstOrDefault()?.Guid;
                if (supplier.SupplierTypeId == null)
                {
                    supplier.SupplierType = new BaseInfo
                    {
                        Name = viewModel.SupplierTypeName,
                        ClinicSectionId = viewModel.ClinicSectionId,
                        TypeId = supplierType.Guid
                    };

                    _unitOfWork.BaseInfos.Add(supplier.SupplierType);
                }
            }

            _unitOfWork.Suppliers.Add(supplier);
            _unitOfWork.Complete();
            return supplier.Guid.ToString();
        }


        public string UpdateSupplier(SupplierViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Name))
                return "DataNotValid";

            if (!string.IsNullOrWhiteSpace(viewModel.PhoneNumber) && viewModel.PhoneNumber.Length < 8)
                return "WrongMobile";

            var exist_user = _unitOfWork.Suppliers.CheckSupplierExistBaseOnName(viewModel.ClinicSectionId.Value, viewModel.Name, p => p.Guid != viewModel.Guid);
            if (exist_user)
                return "ValueIsRepeated";

            Supplier supplier = Common.ConvertModels<Supplier, SupplierViewModel>.convertModels(viewModel);

            var user = _unitOfWork.Users.Get(viewModel.Guid);
            user.Name = viewModel.Name;
            user.PhoneNumber = viewModel.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(viewModel.CityName))
            {
                var city = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CityName, "City", viewModel.ClinicSectionId.Value);
                supplier.CityId = city?.BaseInfos?.FirstOrDefault()?.Guid;
                if (supplier.CityId == null)
                {
                    supplier.City = new BaseInfo
                    {
                        Name = viewModel.CityName,
                        ClinicSectionId = viewModel.ClinicSectionId.Value,
                        TypeId = city.Guid
                    };

                    _unitOfWork.BaseInfos.Add(supplier.City);
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.CountryName))
            {
                var country = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.CountryName, "Country", viewModel.ClinicSectionId.Value);
                supplier.CountryId = country?.BaseInfos?.FirstOrDefault()?.Guid;
                if (supplier.CountryId == null)
                {
                    supplier.Country = new BaseInfo
                    {
                        Name = viewModel.CountryName,
                        ClinicSectionId = viewModel.ClinicSectionId,
                        TypeId = country.Guid
                    };

                    _unitOfWork.BaseInfos.Add(supplier.Country);
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.SupplierTypeName))
            {
                var supplierType = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.SupplierTypeName, "SupplierType", viewModel.ClinicSectionId.Value);
                supplier.SupplierTypeId = supplierType?.BaseInfos?.FirstOrDefault()?.Guid;
                if (supplier.SupplierTypeId == null)
                {
                    supplier.SupplierType = new BaseInfo
                    {
                        Name = viewModel.SupplierTypeName,
                        ClinicSectionId = viewModel.ClinicSectionId,
                        TypeId = supplierType.Guid
                    };

                    _unitOfWork.BaseInfos.Add(supplier.SupplierType);
                }
            }

            _unitOfWork.Suppliers.UpdateState(supplier);
            _unitOfWork.Users.UpdateState(user);
            _unitOfWork.Complete();
            return supplier.Guid.ToString();
        }

        public IEnumerable<SupplierViewModel> GetAllSuppliers(Guid clinicSectionId)
        {
            IEnumerable<Supplier> hosp = _unitOfWork.Suppliers.GetAllSupplier(clinicSectionId);
            List<SupplierViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            Indexing<SupplierViewModel> indexing = new Indexing<SupplierViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<SupplierViewModel> GetAllSuppliersName(Guid clinicSectionId)
        {
            IEnumerable<Supplier> hosp = _unitOfWork.Suppliers.GetAllSupplierName(clinicSectionId);
            List<SupplierViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            return hospconvert;
        }

        public IEnumerable<SupplierViewModel> GetAllSuppliersByClinicSectionId(Guid clinicSectionId)
        {
            var clinicSection = _unitOfWork.ClinicSections.Get(clinicSectionId);

            if (clinicSection.ClinicSectionTypeId != null)
                clinicSectionId = clinicSection.ParentId.Value;

            IEnumerable<Supplier> hosp = _unitOfWork.Suppliers.GetAllSupplierName(clinicSectionId);
            List<SupplierViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            return hospconvert;
        }

        public IEnumerable<SupplierAccountViewModel> GetAllSupplierAccount(SupplierAccountFilterViewModel viewModel)
        {

            if (viewModel.SupplierId == null)
                return new List<SupplierAccountViewModel>();

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

            var filter = (SupplierFilter)viewModel.SupplierFilter;
            var total = _unitOfWork.Suppliers.GetAllSupplierAccount(viewModel.SupplierId.Value, viewModel.CurrencyId, filter, viewModel.DateFrom, viewModel.DateTo);

            CultureInfo cultures = new CultureInfo("en-US");
            var result = total.GroupBy(g => g.Guid).Select(p => new SupplierAccountViewModel
            {
                Guid = p.Key,
                RecordType = p.FirstOrDefault()?.RecordType,
                Date = p.FirstOrDefault()?.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                Invoices = string.Join(",", p.Select(s => s.PayInvoiceNum).Where(w => !string.IsNullOrWhiteSpace(w))),
                MainInvoices = string.Join(",", p.Select(s => s.MainInvoiceNum).Where(w => !string.IsNullOrWhiteSpace(w))),
                ReturnInvoices = string.Join(",", p.Select(s => s.RetunInvoiceNum).Where(w => !string.IsNullOrWhiteSpace(w))),
                InvoiceNum = p.FirstOrDefault()?.InvoiceNum,
                Description = p.FirstOrDefault()?.Description,
                PayAmount = p.FirstOrDefault()?.PayAmount,
                GetAmount = p.FirstOrDefault()?.GetAmount,
                PayStatus = p.FirstOrDefault()?.PayStatus,
                MainInvoicePay = p.FirstOrDefault()?.MainInvoicePay
            }).ToList();

            Indexing<SupplierAccountViewModel> indexing = new Indexing<SupplierAccountViewModel>();
            return indexing.AddIndexing(result);

        }

        public SupplierViewModel GetSupplier(Guid SupplierId)
        {
            try
            {
                Supplier Suppliergu = _unitOfWork.Suppliers.GetSupplier(SupplierId);
                return ConvertModel(Suppliergu);
            }
            catch { return null; }
        }

        public SupplierViewModel GetSupplierName(Guid supplierId)
        {
            Supplier Suppliergu = _unitOfWork.Suppliers.GetSupplierName(supplierId);
            return ConvertModel(Suppliergu);

        }

        public SupplierAccountReportResultViewModel GetSupplierAccountReport(SupplierAccountReportFilterViewModel viewModel, IStringLocalizer<SharedResource> _localizer)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            var purchase_txt = _localizer["Purchase"].Value;
            var return_txt = _localizer["Return"].Value;
            string rem = "";
            var result = new SupplierAccountReportResultViewModel();

            bool? paid_filter = null, purchase_filter = null;
            switch ((PayReportFilter)viewModel.FilterId)
            {
                case PayReportFilter.All:
                    {
                        paid_filter = null;
                        purchase_filter = null;
                    }
                    break;
                case PayReportFilter.UnpaidInvoice:
                    {
                        paid_filter = false;
                        purchase_filter = null;
                    }
                    break;
                case PayReportFilter.UnpaidInvoice_Purchase:
                    {
                        paid_filter = false;
                        purchase_filter = true;
                    }
                    break;
                case PayReportFilter.UnpaidInvoice_ReturnPurchase:
                    {
                        paid_filter = false;
                        purchase_filter = false;
                    }
                    break;
                case PayReportFilter.PaidInvoice:
                    {
                        paid_filter = true;
                        purchase_filter = null;
                    }
                    break;
                case PayReportFilter.PaidInvoice_Purchase:
                    {
                        paid_filter = true;
                        purchase_filter = true;
                    }
                    break;
                case PayReportFilter.PaidInvoice_ReturnPurchase:
                    {
                        paid_filter = true;
                        purchase_filter = false;
                    }
                    break;
            }

            Supplier report;

            if (viewModel.Detail)
            {
                report = _unitOfWork.Suppliers.GetSupplierAccountDetailReport(viewModel.SupplierId, paid_filter, purchase_filter, viewModel.CurrencyName ?? "", viewModel.CurrencyId, viewModel.FromDate, viewModel.ToDate);

                if (!purchase_filter.GetValueOrDefault(true))
                {
                    report.PurchaseInvoices = new List<PurchaseInvoice>();
                }

                if (purchase_filter.GetValueOrDefault(false))
                {
                    report.ReturnPurchaseInvoices = new List<ReturnPurchaseInvoice>();
                }

                List<string> dd = new List<string> { "0" };

                report.PurchaseInvoices = report.PurchaseInvoices.Select(x =>
                  {
                      x.TotalDiscount = string.Join("_", !x.PurchaseInvoiceDiscounts.Any() ? dd :
                          x.PurchaseInvoiceDiscounts.GroupBy(g => g.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}"));
                      return x;
                  }).ToList();

                report.ReturnPurchaseInvoices = report.ReturnPurchaseInvoices.Select(x =>
                {
                    x.TotalDiscount = string.Join("_", !x.ReturnPurchaseInvoiceDiscounts.Any() ? dd :
                        x.ReturnPurchaseInvoiceDiscounts.GroupBy(g => g.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}"));
                    return x;
                }).ToList();

                var purchase = report.PurchaseInvoices.SelectMany(p => p.PurchaseInvoiceDetails, (p, s) => new
                {
                    p.Guid,
                    TempDate = p.InvoiceDate.Value,
                    p.InvoiceNum,
                    p.MainInvoiceNum,
                    TotalPrice = SplitCurrency(p.TotalPrice, viewModel.CurrencyName),
                    p.TotalDiscount,
                    EnInvoiceType = "Purchase",
                    InvoiceType = purchase_txt,
                    ProductName = s.Product.Name,
                    ProductType = s.Product.ProductType.Name,
                    ProducerName = s.Product.Producer.Name,
                    ExpiryDate = s.ExpireDate.Value,
                    Num = s.Num.GetValueOrDefault(0),
                    FreeNum = s.FreeNum.GetValueOrDefault(0),
                    s.PurchasePrice,
                    Currency = s.CurrencyName,
                    Discount = s.Discount.GetValueOrDefault(0),
                }).ToList();

                purchase.AddRange(report.ReturnPurchaseInvoices.SelectMany(p => p.ReturnPurchaseInvoiceDetails, (p, s) => new
                {
                    p.Guid,
                    TempDate = p.InvoiceDate.Value,
                    p.InvoiceNum,
                    MainInvoiceNum = "",
                    TotalPrice = SplitCurrency(p.TotalPrice, viewModel.CurrencyName),
                    p.TotalDiscount,
                    EnInvoiceType = "Return",
                    InvoiceType = return_txt,
                    ProductName = s.TransferDetail == null ? s.PurchaseInvoiceDetail.Product.Name : s.TransferDetail.Product.Name,
                    ProductType = s.TransferDetail == null ? s.PurchaseInvoiceDetail.Product.ProductType.Name : s.TransferDetail.Product.ProductType.Name,
                    ProducerName = s.TransferDetail == null ? s.PurchaseInvoiceDetail.Product.Producer.Name : s.TransferDetail.Product.Producer.Name,
                    ExpiryDate = s.TransferDetail == null ? s.PurchaseInvoiceDetail.ExpireDate.Value : s.TransferDetail.ExpireDate.Value,
                    Num = s.Num.GetValueOrDefault(0),
                    FreeNum = s.FreeNum.GetValueOrDefault(0),
                    PurchasePrice = s.Price,
                    Currency = s.CurrencyName,
                    Discount = s.Discount.GetValueOrDefault(0),
                }));

                result.AllDetailPurchase = purchase.OrderBy(p => p.TempDate).Select(p => new SupplierAccountReportDetailViewModel
                {
                    Guid = p.Guid.ToString(),
                    Date = p.TempDate.ToString("dd/MM/yyyy", cultures),
                    InvoiceNum = p.InvoiceNum,
                    MainInvoiceNum = p.MainInvoiceNum,
                    EnInvoiceType = p.EnInvoiceType,
                    InvoiceType = p.InvoiceType,
                    ProductName = p.ProductName,
                    ProductType = p.ProductType,
                    ProducerName = p.ProducerName,
                    ExpiryDate = p.ExpiryDate.ToString("dd/MM/yyyy", cultures),
                    TempNum = p.Num,
                    TempFreeNum = p.FreeNum,
                    TempPurchasePrice = p.PurchasePrice.GetValueOrDefault(0),
                    TempDiscount = p.Discount,
                    TotalDiscount = p.TotalDiscount.Replace("_", "<br>"),
                    TotalPrice = p.TotalPrice.Replace("_", "<br>"),
                    CurrencyName = p.Currency
                });


                var total_purchase = report.PurchaseInvoices.SelectMany(p => p.PurchaseInvoiceDetails).Select(p => new
                {
                    p.CurrencyName,
                    TotalDiscount = p.Discount.GetValueOrDefault(0),
                    TotalPrice = p.Num.GetValueOrDefault(0) * p.PurchasePrice.GetValueOrDefault(0)
                }).ToList();

                total_purchase.AddRange(report.PurchaseInvoices.SelectMany(p => p.PurchaseInvoiceDiscounts).Select(p => new
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

                var returns = report.ReturnPurchaseInvoices.SelectMany(p => p.ReturnPurchaseInvoiceDetails).Select(p => new
                {
                    p.CurrencyName,
                    TotalDiscount = p.Discount.GetValueOrDefault(0),
                    TotalPrice = p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0)
                }).ToList();

                returns.AddRange(report.ReturnPurchaseInvoices.SelectMany(p => p.ReturnPurchaseInvoiceDiscounts).Select(p => new
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
                report = _unitOfWork.Suppliers.GetSupplierAccountReport(viewModel.SupplierId, paid_filter, purchase_filter, viewModel.CurrencyName ?? "", viewModel.FromDate, viewModel.ToDate);

                if (!purchase_filter.GetValueOrDefault(true))
                {
                    report.PurchaseInvoices = new List<PurchaseInvoice>();
                }

                if (purchase_filter.GetValueOrDefault(false))
                {
                    report.ReturnPurchaseInvoices = new List<ReturnPurchaseInvoice>();
                }

                var purchase = report.PurchaseInvoices.Select(p => new
                {
                    TempDate = p.InvoiceDate.Value,
                    p.InvoiceNum,
                    p.MainInvoiceNum,
                    EnInvoiceType = "Purchase",
                    InvoiceType = purchase_txt,
                    TempTotalPrice = SplitCurrency(p.TotalPrice, viewModel.CurrencyName),
                    TempDiscount = string.Join("_", p.PurchaseInvoiceDiscounts.GroupBy(p => p.CurrencyName).Select(x => $"{x.Key} {x.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}")),
                    TotalAfterDiscount = SplitCurrency(p.TotalPrice, viewModel.CurrencyName).Replace("_", "<br>")
                }).ToList();

                purchase.AddRange(report.ReturnPurchaseInvoices.Select(p => new
                {
                    TempDate = p.InvoiceDate.Value,
                    p.InvoiceNum,
                    MainInvoiceNum = "",
                    EnInvoiceType = "Return",
                    InvoiceType = return_txt,
                    TempTotalPrice = SplitCurrency(p.TotalPrice, viewModel.CurrencyName),
                    TempDiscount = string.Join("_", p.ReturnPurchaseInvoiceDiscounts.GroupBy(p => p.CurrencyName).Select(x => $"{x.Key} {x.Sum(s => s.Amount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}")),
                    TotalAfterDiscount = SplitCurrency(p.TotalPrice, viewModel.CurrencyName).Replace("_", "<br>")
                }));

                result.AllPurchase = purchase.OrderBy(p => p.TempDate).Select(p => new SupplierAccountReportViewModel
                {
                    Date = p.TempDate.ToString("dd/MM/yyyy", cultures),
                    InvoiceNum = p.InvoiceNum,
                    MainInvoiceNum = p.MainInvoiceNum,
                    EnInvoiceType = p.EnInvoiceType,
                    InvoiceType = p.InvoiceType,
                    TotalPrice = AddDiscount(p.TempDiscount, p.TempTotalPrice, viewModel.CurrencyName, cultures),
                    Discount = string.IsNullOrWhiteSpace(p.TempDiscount) ? "0" : p.TempDiscount.Replace("_", "<br>"),
                    TotalAfterDiscount = p.TotalAfterDiscount,
                    TempRem = rem = CalculateTotal(rem, p.TempTotalPrice, p.EnInvoiceType, viewModel.CurrencyName, cultures)
                }).ToList();

            }

            result.SupplierName = report.User.Name;
            var Total = new SupplierAccountReportTotalViewModel
            {
                ShowContent = _localizer["Total"],
                ShowValue = rem.Replace("_", "<br>")
            };

            var purchaseTotal = report.PurchaseInvoices.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() }).Where(p => p.CurrencyName.Contains(viewModel.CurrencyName ?? ""))
                .GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount).ToString("#,0.##", cultures)}");

            var PurchaseTotal = new SupplierAccountReportTotalViewModel
            {
                ShowContent = _localizer["TotalPurchase"],
                ShowValue = string.Join("<br>", purchaseTotal),
            };

            var returnTotal = report.ReturnPurchaseInvoices.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() }).Where(p => p.CurrencyName.Contains(viewModel.CurrencyName ?? ""))
                .GroupBy(p => p.CurrencyName).Select(p => $"{p.Key} {p.Sum(s => s.Amount).ToString("#,0.##", cultures)}");

            var ReturnTotal = new SupplierAccountReportTotalViewModel
            {
                ShowContent = _localizer["TotalReturn"],
                ShowValue = string.Join("<br>", returnTotal)
            };

            result.Results = new List<SupplierAccountReportTotalViewModel>
            {
                PurchaseTotal,
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

            if (type == "Purchase")
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
        public SupplierViewModel ConvertModel(Supplier Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Supplier, SupplierViewModel>()
                .ForMember(a => a.User, b => b.Ignore())
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber));
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Supplier, SupplierViewModel>(Users);
        }
        public List<SupplierViewModel> ConvertModelsLists(IEnumerable<Supplier> Suppliers)
        {
            List<SupplierViewModel> SupplierDtoList = new List<SupplierViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Supplier, SupplierViewModel>()
                .ForMember(a => a.User, b => b.Ignore())
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber));
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            SupplierDtoList = mapper.Map<IEnumerable<Supplier>, List<SupplierViewModel>>(Suppliers);
            return SupplierDtoList;
        }


        // End Convert
    }
}
