using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.Pay;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class PayMvcMockingService : IPayMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public PayMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemovePay(Guid Payid, Guid userId, string pass)
        {
            try
            {
                var check = _unitOfWork.Users.CheckUserByIdAndPass(userId, pass);
                if (!check)
                    return "WrongPass";

                Pay Hos = _unitOfWork.Pays.GetPayWithPurchaseInvocie(Payid);
                _unitOfWork.PayAmounts.RemoveRange(Hos.PayAmounts);
                _unitOfWork.PurchaseInvoicePays.RemoveRange(Hos.PurchaseInvoicePays);
                _unitOfWork.ReturnPurchaseInvoicePays.RemoveRange(Hos.ReturnPurchaseInvoicePays);
                _unitOfWork.Pays.Remove(Hos);
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
            string controllerName = "/Pay/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string UpdatePartialPay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            if (!invoiceIds.Any())
                return "DataNotValid";

            var repeated_pay = _unitOfWork.PurchaseInvoicePays.CheckRepeatedPay(invoiceIds, viewModel.Guid);
            if (repeated_pay)
                return "RepeatedPay";

            //var currency = _unitOfWork.BaseInfoGenerals.GetSingle(p => p.Id == viewModel.BaseCurrencyId);

            //var invoices = _unitOfWork.PurchaseInvoices.GetForPay(invoiceIds, currency?.Name);


            //if (amount < viewModel.Discount)
            //    return "DiscountIsGreaterThanTheAmount";

            //if (viewModel.BaseCurrencyId == viewModel.CurrencyId)
            //    viewModel.BaseAmount = viewModel.DestAmount = 1;

            Pay pay = _unitOfWork.Pays.GetPayWithPurchaseInvocie(viewModel.Guid);

            pay.SupplierId = viewModel.SupplierId;
            //pay.Amount = amount;
            //pay.Discount = viewModel.Discount;
            pay.Description = viewModel.Description;
            //pay.CurrencyId = viewModel.CurrencyId;
            //pay.BaseCurrencyId = viewModel.BaseCurrencyId;
            pay.ModifiedUserId = viewModel.ModifiedUserId;
            pay.ModifiedDate = viewModel.ModifiedDate;
            //pay.BaseAmount = viewModel.BaseAmount;
            //pay.DestAmount = viewModel.DestAmount;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUsePayDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.PayDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime payDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                pay.PayDate = payDate;
            }

            //var purchase_pays = invoices.Select(p => new PurchaseInvoicePay
            //{
            //    PayId = viewModel.Guid,
            //    InvoiceId = p.Guid,
            //    FullPay = true
            //}).ToList();

            _unitOfWork.PurchaseInvoicePays.RemoveRange(pay.PurchaseInvoicePays);
            _unitOfWork.Pays.UpdateState(pay);
            //_unitOfWork.PurchaseInvoicePays.AddRange(purchase_pays);
            _unitOfWork.Complete();
            return pay.Guid.ToString();
        }

        public string AddNewPartialPay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            if (!invoiceIds.Any())
                return "DataNotValid";

            //var currency = _unitOfWork.BaseInfoGenerals.GetSingle(p => p.Id == viewModel.BaseCurrencyId);
            //var invoices = _unitOfWork.PurchaseInvoices.GetForPartialPay(invoiceIds, currency?.Name, viewModel.BaseCurrencyId, null);

            //var new_pay = invoices.Where(p => !p.PurchaseInvoicePays.Any()).Any();

            //var list_withOutNull = invoices.Where(p => p.PurchaseInvoicePays.Any())
            //    .Select(x => string.Join(",", x.PurchaseInvoicePays.Select(s => s.PayId)));

            //var equ = list_withOutNull.All(p => p == list_withOutNull.FirstOrDefault());
            //if (!equ)
            //    return "ErrorInSelectInvoice";

            //var pay_ids = invoices.Where(p => p.PurchaseInvoicePays.Count > 0).Any();
            //if (new_pay && pay_ids)
            //    return "ErrorInSelectInvoice";


            //if (viewModel.Amount < viewModel.Discount)
            //    return "DiscountIsGreaterThanTheAmount";

            //var paid_amount = invoices.SelectMany(p => p.PurchaseInvoicePays).Sum(p => p.Pay.Amount.GetValueOrDefault(0) * p.Pay.DestAmount.GetValueOrDefault(1) / p.Pay.BaseAmount.GetValueOrDefault(1));

            //var diff = amount - (paid_amount + (viewModel.Amount * viewModel.DestAmount / viewModel.BaseAmount));
            //if (diff < 0)
            //    return "AmountOverFlow";

            //if (viewModel.BaseCurrencyId == viewModel.CurrencyId)
            //    viewModel.BaseAmount = viewModel.DestAmount = 1;

            Pay pay = Common.ConvertModels<Pay, PayViewModel>.convertModels(viewModel);

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUsePayDate");
            if (!access)
            {
                pay.PayDate = DateTime.Now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.PayDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime payDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                pay.PayDate = payDate;
            }

            //pay.Amount = viewModel.Amount;

            //pay.PurchaseInvoicePays = invoices.Select(p => new PurchaseInvoicePay
            //{
            //    InvoiceId = p.Guid,
            //    FullPay = diff == 0
            //}).ToList();

            _unitOfWork.Pays.Add(pay);
            _unitOfWork.Complete();
            return pay.Guid.ToString();
        }

        public string UpdateInvoicePay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            if ((!invoiceIds.Any() && !returnIds.Any()) || viewModel.PayAmounts.Any(p => string.IsNullOrWhiteSpace(p.BaseCurrencyName)) ||
                viewModel.PayAmounts.Any(p => p.BaseAmount.GetValueOrDefault(0) <= 0 || p.DestAmount.GetValueOrDefault(0) <= 0 || p.Discount.GetValueOrDefault(0) < 0))
                return "DataNotValid";

            var repeated_pay = _unitOfWork.PurchaseInvoicePays.CheckRepeatedPay(invoiceIds, viewModel.Guid);
            if (repeated_pay)
                return "RepeatedPay";

            var repeated_return = _unitOfWork.ReturnPurchaseInvoicePays.CheckRepeatedPay(returnIds, viewModel.Guid);
            if (repeated_return)
                return "RepeatedPay";

            var invoices = _unitOfWork.PurchaseInvoices.GetForPay(invoiceIds);
            var returns = _unitOfWork.ReturnPurchaseInvoices.GetForPay(returnIds);

            var currencies = _unitOfWork.BaseInfoGenerals.GetByTypeAndNames("Currency", viewModel.PayAmounts.Select(p => p.BaseCurrencyName));

            var payAmount_viewModel = from p in viewModel.PayAmounts
                                      join c in currencies
                                      on p.BaseCurrencyName equals c.Name into p_c
                                      from j in p_c.DefaultIfEmpty()
                                      select new PayAmountViewModel
                                      {
                                          BaseAmount = p.BaseAmount,
                                          DestAmount = p.DestAmount,
                                          BaseCurrencyId = j?.Id,
                                          BaseCurrencyName = j?.Name,
                                          CurrencyId = p.CurrencyId,
                                          Discount = p.Discount
                                      };

            var ex = payAmount_viewModel.Any(p => p.BaseCurrencyId == null);
            if (ex)
                return "DataNotValid";

            var amounts = invoices.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() })
                .GroupBy(p => p.CurrencyName).Select(p => new { CurrencyName = p.Key, Amount = p.Sum(s => s.Amount) }).ToList();

            amounts.AddRange(returns.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() })
                .GroupBy(p => p.CurrencyName).Select(p => new { CurrencyName = p.Key, Amount = -p.Sum(s => s.Amount) }));

            var pay_amounts = from r in payAmount_viewModel
                              join a in amounts
                              on r.BaseCurrencyName equals a.CurrencyName into r_a
                              from j in r_a.DefaultIfEmpty()
                              select new PayAmountViewModel
                              {
                                  Amount = j?.Amount,
                                  BaseAmount = r.BaseCurrencyId == r.CurrencyId ? 1 : r.BaseAmount.GetValueOrDefault(1),
                                  DestAmount = r.BaseCurrencyId == r.CurrencyId ? 1 : r.DestAmount.GetValueOrDefault(1),
                                  BaseCurrencyId = r.BaseCurrencyId,
                                  BaseCurrencyName = r.BaseCurrencyName,
                                  CurrencyId = r.CurrencyId,
                                  Discount = j?.Amount < 0 ? -r.Discount.GetValueOrDefault(0) : r.Discount.GetValueOrDefault(0)
                              };


            var pay_result = pay_amounts.GroupBy(p => p.CurrencyId).Select(p => new PayAmountViewModel
            {
                CurrencyId = p.Key,
                Amount = p.Sum(s => s.Amount.GetValueOrDefault(0) * s.DestAmount / s.BaseAmount),
                BaseAmount = p.FirstOrDefault().BaseAmount,
                DestAmount = p.FirstOrDefault().DestAmount,
                BaseCurrencyId = p.FirstOrDefault().BaseCurrencyId,
                BaseCurrencyName = p.FirstOrDefault().BaseCurrencyName,
                Discount = p.Sum(s => s.Discount.GetValueOrDefault(0) * s.DestAmount / s.BaseAmount)
            });

            //var neg = pay_result.Where(p => p.Amount < 0).ToList();
            //var neg_currency = neg.Select(p => p.BaseCurrencyName);
            //var conflict = amounts.Any(p => neg_currency.Any() && !neg_currency.Contains(p.CurrencyName));
            //if (conflict)
            //    return "CurrencyConflict";

            //if (neg.Any())
            //    return "NegativeAmount";

            var discount = pay_result.Any(p => Math.Abs((p.Amount).GetValueOrDefault(0)) < Math.Abs(p.Discount.GetValueOrDefault(0)));
            if (discount)
                return "DiscountIsGreaterThanTheAmount";

            Pay pay = _unitOfWork.Pays.GetPayWithPurchaseInvocie(viewModel.Guid);

            pay.SupplierId = viewModel.SupplierId;
            pay.Description = viewModel.Description;
            pay.ModifiedUserId = viewModel.ModifiedUserId;
            pay.ModifiedDate = viewModel.ModifiedDate;
            pay.MainInvoiceNum = viewModel.MainInvoiceNum;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUsePayDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.PayDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime payDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                pay.PayDate = payDate;
            }

            var payAmounts = pay_amounts.GroupBy(p => p.BaseCurrencyId).Select(p => new PayAmount
            {
                PayId = viewModel.Guid,
                BaseCurrencyId = p.Key,
                Amount = p.Sum(s => s.Amount.GetValueOrDefault(0)),
                Discount = p.FirstOrDefault().Discount,
                BaseAmount = p.FirstOrDefault().BaseAmount,
                DestAmount = p.FirstOrDefault().DestAmount,
                CurrencyId = p.FirstOrDefault().CurrencyId,
            }).ToList();

            var purchaseInvoicePays = invoices.Select(p => new PurchaseInvoicePay
            {
                PayId = viewModel.Guid,
                InvoiceId = p.Guid,
                FullPay = true
            }).ToList();

            var returnPurchaseInvoicePays = returns.Select(p => new ReturnPurchaseInvoicePay
            {
                PayId = viewModel.Guid,
                InvoiceId = p.Guid,
                FullPay = true
            }).ToList();

            _unitOfWork.ReturnPurchaseInvoicePays.RemoveRange(pay.ReturnPurchaseInvoicePays);
            _unitOfWork.PurchaseInvoicePays.RemoveRange(pay.PurchaseInvoicePays);
            _unitOfWork.PayAmounts.RemoveRange(pay.PayAmounts);
            _unitOfWork.Pays.UpdateState(pay);
            _unitOfWork.PayAmounts.AddRange(payAmounts);
            _unitOfWork.PurchaseInvoicePays.AddRange(purchaseInvoicePays);
            _unitOfWork.ReturnPurchaseInvoicePays.AddRange(returnPurchaseInvoicePays);
            _unitOfWork.Complete();
            return pay.Guid.ToString();
        }

        public string AddNewInvoicePay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            if ((!invoiceIds.Any() && !returnIds.Any()) || viewModel.PayAmounts.Any(p => string.IsNullOrWhiteSpace(p.BaseCurrencyName)) ||
                viewModel.PayAmounts.Any(p => p.BaseAmount.GetValueOrDefault(0) <= 0 || p.DestAmount.GetValueOrDefault(0) <= 0 || p.Discount.GetValueOrDefault(0) < 0))
                return "DataNotValid";

            var repeated_pay = _unitOfWork.PurchaseInvoicePays.CheckRepeatedPay(invoiceIds, null);
            if (repeated_pay)
                return "RepeatedPay";

            var repeated_return = _unitOfWork.ReturnPurchaseInvoicePays.CheckRepeatedPay(returnIds, null);
            if (repeated_return)
                return "RepeatedPay";

            var invoices = _unitOfWork.PurchaseInvoices.GetForPay(invoiceIds);
            var returns = _unitOfWork.ReturnPurchaseInvoices.GetForPay(returnIds);

            var currencies = _unitOfWork.BaseInfoGenerals.GetByTypeAndNames("Currency", viewModel.PayAmounts.Select(p => p.BaseCurrencyName));

            var payAmount_viewModel = from p in viewModel.PayAmounts
                                      join c in currencies
                                      on p.BaseCurrencyName equals c.Name into p_c
                                      from j in p_c.DefaultIfEmpty()
                                      select new PayAmountViewModel
                                      {
                                          BaseAmount = p.BaseAmount,
                                          DestAmount = p.DestAmount,
                                          BaseCurrencyId = j?.Id,
                                          BaseCurrencyName = j?.Name,
                                          CurrencyId = p.CurrencyId,
                                          Discount = p.Discount
                                      };

            var ex = payAmount_viewModel.Any(p => p.BaseCurrencyId == null);
            if (ex)
                return "DataNotValid";

            var amounts = invoices.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() })
                .GroupBy(p => p.CurrencyName).Select(p => new { CurrencyName = p.Key, Amount = p.Sum(s => s.Amount) }).ToList();

            amounts.AddRange(returns.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() })
                .GroupBy(p => p.CurrencyName).Select(p => new { CurrencyName = p.Key, Amount = -p.Sum(s => s.Amount) }));

            var pay_amounts = from r in payAmount_viewModel
                              join a in amounts
                              on r.BaseCurrencyName equals a.CurrencyName into r_a
                              from j in r_a.DefaultIfEmpty()
                              select new PayAmountViewModel
                              {
                                  Amount = j?.Amount,
                                  BaseAmount = r.BaseCurrencyId == r.CurrencyId ? 1 : r.BaseAmount.GetValueOrDefault(1),
                                  DestAmount = r.BaseCurrencyId == r.CurrencyId ? 1 : r.DestAmount.GetValueOrDefault(1),
                                  BaseCurrencyId = r.BaseCurrencyId,
                                  BaseCurrencyName = r.BaseCurrencyName,
                                  CurrencyId = r.CurrencyId,
                                  Discount = j?.Amount < 0 ? -r.Discount.GetValueOrDefault(0) : r.Discount.GetValueOrDefault(0)
                              };


            var pay_result = pay_amounts.GroupBy(p => p.CurrencyId).Select(p => new PayAmountViewModel
            {
                CurrencyId = p.Key,
                Amount = p.Sum(s => s.Amount.GetValueOrDefault(0) * s.DestAmount / s.BaseAmount),
                BaseAmount = p.FirstOrDefault().BaseAmount,
                DestAmount = p.FirstOrDefault().DestAmount,
                BaseCurrencyId = p.FirstOrDefault().BaseCurrencyId,
                BaseCurrencyName = p.FirstOrDefault().BaseCurrencyName,
                Discount = p.Sum(s => s.Discount.GetValueOrDefault(0) * s.DestAmount / s.BaseAmount)
            });

            //var neg = pay_result.Where(p => p.Amount < 0).ToList();
            //var neg_currency = neg.Select(p => p.BaseCurrencyName);
            //var conflict = amounts.Any(p => neg_currency.Any() && !neg_currency.Contains(p.CurrencyName));
            //if (conflict)
            //    return "CurrencyConflict";

            //if (neg.Any())
            //    return "NegativeAmount";

            var discount = pay_result.Any(p => Math.Abs((p.Amount).GetValueOrDefault(0)) < Math.Abs(p.Discount.GetValueOrDefault(0)));
            if (discount)
                return "DiscountIsGreaterThanTheAmount";

            Pay pay = ConvertModel(viewModel);

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUsePayDate");
            if (!access)
            {
                pay.PayDate = DateTime.Now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.PayDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime payDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                pay.PayDate = payDate;
            }

            pay.PayAmounts = pay_amounts.GroupBy(p => p.BaseCurrencyId).Select(p => new PayAmount
            {
                BaseCurrencyId = p.Key,
                Amount = p.Sum(s => s.Amount.GetValueOrDefault(0)),
                Discount = p.FirstOrDefault().Discount,
                BaseAmount = p.FirstOrDefault().BaseAmount,
                DestAmount = p.FirstOrDefault().DestAmount,
                CurrencyId = p.FirstOrDefault().CurrencyId,
            }).ToList();

            pay.PurchaseInvoicePays = invoices.Select(p => new PurchaseInvoicePay
            {
                InvoiceId = p.Guid,
                FullPay = true
            }).ToList();

            pay.ReturnPurchaseInvoicePays = returns.Select(p => new ReturnPurchaseInvoicePay
            {
                InvoiceId = p.Guid,
                FullPay = true
            }).ToList();

            while (true)
            {
                try
                {
                    pay.InvoiceNum = GetPayInvoiceNum(viewModel.ClinicSectionId.Value);


                    _unitOfWork.Pays.Add(pay);
                    _unitOfWork.Complete();

                    break;
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("") && !(e.InnerException?.Message ?? "UQ_Pay_InvoiceNum_ClinicSectionId").Contains("UQ_Pay_InvoiceNum_ClinicSectionId"))
                        throw e;
                }
            }

            return pay.Guid.ToString();
        }

        public string AddNewPay(PayViewModel viewModel)
        {
            if (!viewModel.PayAmounts.Any())
                return "DataNotValid";

            var pay_amount = viewModel.PayAmounts.FirstOrDefault();
            if (pay_amount.Amount.GetValueOrDefault(0) <= 0 || pay_amount.BaseAmount.GetValueOrDefault(0) <= 0 || pay_amount.DestAmount.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            if (pay_amount.Amount < pay_amount.Discount)
                return "DiscountIsGreaterThanTheAmount";

            if (pay_amount.BaseCurrencyId == pay_amount.CurrencyId)
                pay_amount.BaseAmount = pay_amount.DestAmount = 1;

            Pay pay = ConvertModel(viewModel);

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUsePayDate");
            if (!access)
            {
                pay.PayDate = DateTime.Now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.PayDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime payDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                pay.PayDate = payDate;
            }

            var payAmount = new PayAmount
            {
                Amount = pay_amount.Amount,
                Discount = pay_amount.Discount,
                BaseAmount = pay_amount.BaseAmount,
                DestAmount = pay_amount.DestAmount,
                BaseCurrencyId = pay_amount.BaseCurrencyId,
                CurrencyId = pay_amount.CurrencyId,
            };

            pay.PayAmounts = new List<PayAmount>
            {
                payAmount
            };

            while (true)
            {
                try
                {
                    pay.InvoiceNum = GetPayInvoiceNum(viewModel.ClinicSectionId.Value);


                    _unitOfWork.Pays.Add(pay);
                    _unitOfWork.Complete();

                    break;
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("") && !(e.InnerException?.Message ?? "UQ_Pay_InvoiceNum_ClinicSectionId").Contains("UQ_Pay_InvoiceNum_ClinicSectionId"))
                        throw e;
                }
            }

            return pay.Guid.ToString();
        }

        public string UpdatePay(PayViewModel viewModel)
        {
            if (!viewModel.PayAmounts.Any())
                return "DataNotValid";

            var pay_amount = viewModel.PayAmounts.FirstOrDefault();
            if (pay_amount.Amount.GetValueOrDefault(0) <= 0 || pay_amount.BaseAmount.GetValueOrDefault(0) <= 0 || pay_amount.DestAmount.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            if (pay_amount.Amount < pay_amount.Discount)
                return "DiscountIsGreaterThanTheAmount";

            if (pay_amount.BaseCurrencyId == pay_amount.CurrencyId)
                pay_amount.BaseAmount = pay_amount.DestAmount = 1;

            var pay = _unitOfWork.Pays.GetWithPayAmount(viewModel.Guid);
            pay.ModifiedDate = viewModel.ModifiedDate;
            pay.ModifiedUserId = viewModel.ModifiedUserId;
            pay.Description = viewModel.Description;
            pay.MainInvoiceNum = viewModel.MainInvoiceNum;
            pay.SupplierId = viewModel.SupplierId;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUsePayDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.PayDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime payDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                pay.PayDate = payDate;
            }

            var total = pay.PayAmounts.FirstOrDefault();
            total.CurrencyId = pay_amount.CurrencyId;
            total.BaseCurrencyId = pay_amount.BaseCurrencyId;
            total.Amount = pay_amount.Amount;
            total.Discount = pay_amount.Discount;
            total.BaseAmount = pay_amount.BaseAmount;
            total.DestAmount = pay_amount.DestAmount;

            _unitOfWork.Pays.UpdateState(pay);
            _unitOfWork.PayAmounts.UpdateState(total);
            _unitOfWork.Complete();
            return pay.Guid.ToString();
        }

        public PayViewModel GetPay(Guid payId)
        {
            Pay pay = _unitOfWork.Pays.GetWithSupplier(payId);
            var result = ConvertTotalModel(pay);

            var payType = _unitOfWork.Pays.CheckPayStatus(payId);

            if (string.IsNullOrWhiteSpace(payType))
                result.PayType = "Total";
            else if (payType.Contains("0"))
                result.PayType = "Partial";
            else
                result.PayType = "Factor";

            return result;
        }

        public IEnumerable<PayViewModel> GetPartialPayHistory(IEnumerable<string> payIds)
        {
            var list = payIds.Where(p => !string.IsNullOrWhiteSpace(p));

            if (!list.Any())
                return new List<PayViewModel>();

            var guids = list.SelectMany(p => p.Split(" "));

            var result = _unitOfWork.Pays.GetPartialPayHistory(guids);
            CultureInfo cultures = new CultureInfo("en-US");
            var res = result.Select(p => new PayViewModel
            {
                PayDateTxt = p.PayDate.Value.ToString("dd/MM/yyyy", cultures),
                //Amount = p.Amount * p.DestAmount / p.BaseAmount,
                //Discount = p.Discount.GetValueOrDefault(0),
                //Description = p.Description,
                //PayType = p.Currency.Name
            });

            Indexing<PayViewModel> indexing = new Indexing<PayViewModel>();
            return indexing.AddIndexing(res.ToList());
        }


        public string GetPayInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                string purchaseInvoiceNum = _unitOfWork.Pays.GetLatestPayInvoiceNum(clinicSectionId);
                return NextPayInvoiceNum(purchaseInvoiceNum);
            }
            catch (Exception) { return "1"; }
        }

        public string NextPayInvoiceNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }


        // Begin Convert 

        public Pay ConvertModel(PayViewModel pay)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PayViewModel, Pay>()
                .ForMember(a => a.PayAmounts, b => b.Ignore())
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PayViewModel, Pay>(pay);
        }

        public PayViewModel ConvertTotalModel(Pay pay)
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<PayAmount, PayAmountViewModel>()
                .ForMember(a => a.BaseCurrencyName, b => b.MapFrom(c => c.BaseCurrency.Name))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                .ForMember(a => a.Discount, b => b.MapFrom(c => Math.Abs(c.Discount.GetValueOrDefault(0))))
                .ForMember(a => a.Amount, b => b.MapFrom(c => c.Amount.GetValueOrDefault(0)))
                .ForMember(a => a.BaseAmount, b => b.MapFrom(c => c.BaseAmount.GetValueOrDefault(1)))
                .ForMember(a => a.DestAmount, b => b.MapFrom(c => c.DestAmount.GetValueOrDefault(1)))
                ;
                cfg.CreateMap<Pay, PayViewModel>()
                .ForMember(a => a.SupplierName, b => b.MapFrom(c => c.Supplier.User.Name))
                //.ForMember(a => a.PayAmounts, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Pay, PayViewModel>(pay);
        }



        // End Convert
    }
}
