using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using WPH.Helper;
using WPH.Models.Receive;
using WPH.Models.Customer;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReceiveMvcMockingService : IReceiveMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ReceiveMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemoveReceive(Guid Receiveid, Guid userId, string pass)
        {
            try
            {
                var check = _unitOfWork.Users.CheckUserByIdAndPass(userId, pass);
                if (!check)
                    return "WrongPass";

                Receive Hos = _unitOfWork.Receives.GetReceiveWithSaleInvocie(Receiveid);
                _unitOfWork.ReceiveAmounts.RemoveRange(Hos.ReceiveAmounts);
                _unitOfWork.SaleInvoiceReceives.RemoveRange(Hos.SaleInvoiceReceives);
                _unitOfWork.ReturnSaleInvoiceReceives.RemoveRange(Hos.ReturnSaleInvoiceReceives);
                _unitOfWork.Receives.Remove(Hos);
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

        public string RemoveReceiveAmount(Guid id)
        {
            try
            {
                ReceiveAmount Hos = _unitOfWork.ReceiveAmounts.Get(id);
                _unitOfWork.ReceiveAmounts.Remove(Hos);
                _unitOfWork.Receives.Remove(_unitOfWork.Receives.Get(Hos.ReceiveId));
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
            string controllerName = "/Receive/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string UpdatePartialReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            if (!invoiceIds.Any())
                return "DataNotValid";

            var repeated_receive = _unitOfWork.SaleInvoiceReceives.CheckRepeatedReceive(invoiceIds, viewModel.Guid);
            if (repeated_receive)
                return "RepeatedReceive";

            //var currency = _unitOfWork.BaseInfoGenerals.GetSingle(p => p.Id == viewModel.BaseCurrencyId);

            //var invoices = _unitOfWork.SaleInvoices.GetForReceive(invoiceIds, currency?.Name);

            //if (amount < viewModel.Discount)
            //    return "DiscountIsGreaterThanTheAmount";

            //if (viewModel.BaseCurrencyId == viewModel.CurrencyId)
            //    viewModel.BaseAmount = viewModel.DestAmount = 1;

            Receive receive = _unitOfWork.Receives.GetReceiveWithSaleInvocie(viewModel.Guid);

            receive.CustomerId = viewModel.CustomerId;
            //receive.Amount = amount;
            //receive.Discount = viewModel.Discount;
            receive.Description = viewModel.Description;
            //receive.CurrencyId = viewModel.CurrencyId;
            //receive.BaseCurrencyId = viewModel.BaseCurrencyId;
            receive.ModifiedUserId = viewModel.ModifiedUserId;
            receive.ModifiedDate = viewModel.ModifiedDate;
            //receive.BaseAmount = viewModel.BaseAmount;
            //receive.DestAmount = viewModel.DestAmount;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseRecevieDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.ReceiveDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime receiveDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                receive.ReceiveDate = receiveDate;
            }


            //var sale_receives = invoices.Select(p => new SaleInvoiceReceive
            //{
            //    ReceiveId = viewModel.Guid,
            //    InvoiceId = p.Guid,
            //    FullPay = true
            //}).ToList();

            _unitOfWork.SaleInvoiceReceives.RemoveRange(receive.SaleInvoiceReceives);
            _unitOfWork.Receives.UpdateState(receive);
            //_unitOfWork.SaleInvoiceReceives.AddRange(sale_receives);
            _unitOfWork.Complete();
            return receive.Guid.ToString();
        }

        public string AddNewPartialReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            if (!invoiceIds.Any())
                return "DataNotValid";

            //var currency = _unitOfWork.BaseInfoGenerals.GetSingle(p => p.Id == viewModel.BaseCurrencyId);
            //var invoices = _unitOfWork.SaleInvoices.GetForPartialReceive(invoiceIds, currency?.Name, viewModel.BaseCurrencyId, null);

            //var new_receive = invoices.Where(p => !p.SaleInvoiceReceives.Any()).Any();

            //var list_withOutNull = invoices.Where(p => p.SaleInvoiceReceives.Any())
            //    .Select(x => string.Join(",", x.SaleInvoiceReceives.Select(s => s.ReceiveId)));

            //var equ = list_withOutNull.All(p => p == list_withOutNull.FirstOrDefault());
            //if (!equ)
            //    return "ErrorInSelectInvoice";

            //var receive_ids = invoices.Where(p => p.SaleInvoiceReceives.Count > 0).Any();
            //if (new_receive && receive_ids)
            //    return "ErrorInSelectInvoice";

            //if (viewModel.Amount < viewModel.Discount)
            //    return "DiscountIsGreaterThanTheAmount";

            //var paid_amount = invoices.SelectMany(p => p.SaleInvoiceReceives).Sum(p => p.Receive.Amount.GetValueOrDefault(0) * p.Receive.DestAmount.GetValueOrDefault(1) / p.Receive.BaseAmount.GetValueOrDefault(1));

            //var diff = amount - (paid_amount + (viewModel.Amount * viewModel.DestAmount / viewModel.BaseAmount));
            //if (diff < 0)
            //    return "AmountOverFlow";

            //if (viewModel.BaseCurrencyId == viewModel.CurrencyId)
            //    viewModel.BaseAmount = viewModel.DestAmount = 1;

            Receive receive = Common.ConvertModels<Receive, ReceiveViewModel>.convertModels(viewModel);

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseRecevieDate");
            if (!access)
            {
                receive.ReceiveDate = DateTime.Now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.ReceiveDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime receiveDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                receive.ReceiveDate = receiveDate;
            }

            //receive.Amount = viewModel.Amount;

            //receive.SaleInvoiceReceives = invoices.Select(p => new SaleInvoiceReceive
            //{
            //    InvoiceId = p.Guid,
            //    FullPay = diff == 0
            //}).ToList();

            _unitOfWork.Receives.Add(receive);
            _unitOfWork.Complete();
            return receive.Guid.ToString();
        }

        public string UpdateInvoiceReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            if ((!invoiceIds.Any() && !returnIds.Any()) || viewModel.ReceiveAmounts.Any(p => string.IsNullOrWhiteSpace(p.BaseCurrencyName)) ||
                viewModel.ReceiveAmounts.Any(p => p.BaseAmount.GetValueOrDefault(0) <= 0 || p.DestAmount.GetValueOrDefault(0) <= 0 || p.Discount.GetValueOrDefault(0) < 0))
                return "DataNotValid";

            var repeated_receive = _unitOfWork.SaleInvoiceReceives.CheckRepeatedReceive(invoiceIds, viewModel.Guid);
            if (repeated_receive)
                return "RepeatedReceive";

            var repeated_return = _unitOfWork.ReturnSaleInvoiceReceives.CheckRepeatedReceive(returnIds, viewModel.Guid);
            if (repeated_return)
                return "RepeatedReceive";

            var invoices = _unitOfWork.SaleInvoices.GetForReceive(invoiceIds);
            var returns = _unitOfWork.ReturnSaleInvoices.GetForReceive(returnIds);

            var currencies = _unitOfWork.BaseInfoGenerals.GetByTypeAndNames("Currency", viewModel.ReceiveAmounts.Select(p => p.BaseCurrencyName));

            var receiveAmount_viewModel = from p in viewModel.ReceiveAmounts
                                          join c in currencies
                                          on p.BaseCurrencyName equals c.Name into p_c
                                          from j in p_c.DefaultIfEmpty()
                                          select new ReceiveAmountViewModel
                                          {
                                              BaseAmount = p.BaseAmount,
                                              DestAmount = p.DestAmount,
                                              BaseCurrencyId = j?.Id,
                                              BaseCurrencyName = j?.Name,
                                              CurrencyId = p.CurrencyId,
                                              Discount = p.Discount
                                          };

            var ex = receiveAmount_viewModel.Any(p => p.BaseCurrencyId == null);
            if (ex)
                return "DataNotValid";

            var amounts = invoices.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() })
                .GroupBy(p => p.CurrencyName).Select(p => new { CurrencyName = p.Key, Amount = p.Sum(s => s.Amount) }).ToList();

            amounts.AddRange(returns.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() })
                .GroupBy(p => p.CurrencyName).Select(p => new { CurrencyName = p.Key, Amount = -p.Sum(s => s.Amount) }));

            var receive_amounts = from r in receiveAmount_viewModel
                                  join a in amounts
                                  on r.BaseCurrencyName equals a.CurrencyName into r_a
                                  from j in r_a.DefaultIfEmpty()
                                  select new ReceiveAmountViewModel
                                  {
                                      Amount = j?.Amount,
                                      BaseAmount = r.BaseCurrencyId == r.CurrencyId ? 1 : r.BaseAmount.GetValueOrDefault(1),
                                      DestAmount = r.BaseCurrencyId == r.CurrencyId ? 1 : r.DestAmount.GetValueOrDefault(1),
                                      BaseCurrencyId = r.BaseCurrencyId,
                                      BaseCurrencyName = r.BaseCurrencyName,
                                      CurrencyId = r.CurrencyId,
                                      Discount = j?.Amount < 0 ? -r.Discount.GetValueOrDefault(0) : r.Discount.GetValueOrDefault(0)
                                  };


            var receive_result = receive_amounts.GroupBy(p => p.CurrencyId).Select(p => new ReceiveAmountViewModel
            {
                CurrencyId = p.Key,
                Amount = p.Sum(s => s.Amount.GetValueOrDefault(0) * s.DestAmount / s.BaseAmount),
                BaseAmount = p.FirstOrDefault().BaseAmount,
                DestAmount = p.FirstOrDefault().DestAmount,
                BaseCurrencyId = p.FirstOrDefault().BaseCurrencyId,
                BaseCurrencyName = p.FirstOrDefault().BaseCurrencyName,
                Discount = p.Sum(s => s.Discount.GetValueOrDefault(0) * s.DestAmount / s.BaseAmount),
            });

            //var neg = receive_result.Where(p => p.Amount < 0).ToList();
            //var neg_currency = neg.Select(p => p.BaseCurrencyName);
            //var conflict = amounts.Any(p => neg_currency.Any() && !neg_currency.Contains(p.CurrencyName));
            //if (conflict)
            //    return "CurrencyConflict";

            //if (neg.Any())
            //    return "NegativeAmount";

            var discount = receive_result.Any(p => Math.Abs((p.Amount).GetValueOrDefault(0)) < Math.Abs(p.Discount.GetValueOrDefault(0)));
            if (discount)
                return "DiscountIsGreaterThanTheAmount";

            Receive receive = _unitOfWork.Receives.GetReceiveWithSaleInvocie(viewModel.Guid);

            receive.CustomerId = viewModel.CustomerId;
            receive.Description = viewModel.Description;
            receive.ModifiedUserId = viewModel.ModifiedUserId;
            receive.ModifiedDate = viewModel.ModifiedDate;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseRecevieDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.ReceiveDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime receiveDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                receive.ReceiveDate = receiveDate;
            }


            var receiveAmounts = receive_amounts.GroupBy(p => p.BaseCurrencyId).Select(p => new ReceiveAmount
            {
                ReceiveId = viewModel.Guid,
                BaseCurrencyId = p.Key,
                Amount = p.Sum(s => s.Amount.GetValueOrDefault(0)),
                Discount = p.FirstOrDefault().Discount,
                BaseAmount = p.FirstOrDefault().BaseAmount,
                DestAmount = p.FirstOrDefault().DestAmount,
                CurrencyId = p.FirstOrDefault().CurrencyId,
            }).ToList();

            var saleInvoiceReceives = invoices.Select(p => new SaleInvoiceReceive
            {
                ReceiveId = viewModel.Guid,
                InvoiceId = p.Guid,
                FullPay = true
            }).ToList();

            var returnSaleInvoiceReceives = returns.Select(p => new ReturnSaleInvoiceReceive
            {
                ReceiveId = viewModel.Guid,
                InvoiceId = p.Guid,
                FullPay = true
            }).ToList();

            _unitOfWork.ReturnSaleInvoiceReceives.RemoveRange(receive.ReturnSaleInvoiceReceives);
            _unitOfWork.SaleInvoiceReceives.RemoveRange(receive.SaleInvoiceReceives);
            _unitOfWork.ReceiveAmounts.RemoveRange(receive.ReceiveAmounts);
            _unitOfWork.Receives.UpdateState(receive);
            _unitOfWork.ReceiveAmounts.AddRange(receiveAmounts);
            _unitOfWork.SaleInvoiceReceives.AddRange(saleInvoiceReceives);
            _unitOfWork.ReturnSaleInvoiceReceives.AddRange(returnSaleInvoiceReceives);
            _unitOfWork.Complete();
            return receive.Guid.ToString();
        }

        public string AddNewInvoiceReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            if ((!invoiceIds.Any() && !returnIds.Any()) || viewModel.ReceiveAmounts.Any(p => string.IsNullOrWhiteSpace(p.BaseCurrencyName)) ||
                viewModel.ReceiveAmounts.Any(p => p.BaseAmount.GetValueOrDefault(0) <= 0 || p.DestAmount.GetValueOrDefault(0) <= 0 || p.Discount.GetValueOrDefault(0) < 0))
                return "DataNotValid";

            var repeated_receive = _unitOfWork.SaleInvoiceReceives.CheckRepeatedReceive(invoiceIds, null);
            if (repeated_receive)
                return "RepeatedReceive";

            var repeated_return = _unitOfWork.ReturnSaleInvoiceReceives.CheckRepeatedReceive(returnIds, null);
            if (repeated_return)
                return "RepeatedReceive";

            var invoices = _unitOfWork.SaleInvoices.GetForReceive(invoiceIds);
            var returns = _unitOfWork.ReturnSaleInvoices.GetForReceive(returnIds);

            var currencies = _unitOfWork.BaseInfoGenerals.GetByTypeAndNames("Currency", viewModel.ReceiveAmounts.Select(p => p.BaseCurrencyName));

            var receiveAmount_viewModel = from p in viewModel.ReceiveAmounts
                                          join c in currencies
                                          on p.BaseCurrencyName equals c.Name into p_c
                                          from j in p_c.DefaultIfEmpty()
                                          select new ReceiveAmountViewModel
                                          {
                                              BaseAmount = p.BaseAmount,
                                              DestAmount = p.DestAmount,
                                              BaseCurrencyId = j?.Id,
                                              BaseCurrencyName = j?.Name,
                                              CurrencyId = p.CurrencyId,
                                              Discount = p.Discount
                                          };

            var ex = receiveAmount_viewModel.Any(p => p.BaseCurrencyId == null);
            if (ex)
                return "DataNotValid";

            var amounts = invoices.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() })
                .GroupBy(p => p.CurrencyName).Select(p => new { CurrencyName = p.Key, Amount = p.Sum(s => s.Amount) }).ToList();

            amounts.AddRange(returns.Where(p => !string.IsNullOrWhiteSpace(p.TotalPrice)).SelectMany(p => p.TotalPrice.Split("_"))
                .Select(p => p.Split(" ")).Select(p => new { CurrencyName = p[0], Amount = p[1].GetDecimalNumber() })
                .GroupBy(p => p.CurrencyName).Select(p => new { CurrencyName = p.Key, Amount = -p.Sum(s => s.Amount) }));

            var receive_amounts = from r in receiveAmount_viewModel
                                  join a in amounts
                                  on r.BaseCurrencyName equals a.CurrencyName into r_a
                                  from j in r_a.DefaultIfEmpty()
                                  select new ReceiveAmountViewModel
                                  {
                                      Amount = j?.Amount,
                                      BaseAmount = r.BaseCurrencyId == r.CurrencyId ? 1 : r.BaseAmount.GetValueOrDefault(1),
                                      DestAmount = r.BaseCurrencyId == r.CurrencyId ? 1 : r.DestAmount.GetValueOrDefault(1),
                                      BaseCurrencyId = r.BaseCurrencyId,
                                      BaseCurrencyName = r.BaseCurrencyName,
                                      CurrencyId = r.CurrencyId,
                                      Discount = j?.Amount < 0 ? -r.Discount.GetValueOrDefault(0) : r.Discount.GetValueOrDefault(0)
                                  };


            var receive_result = receive_amounts.GroupBy(p => p.CurrencyId).Select(p => new ReceiveAmountViewModel
            {
                CurrencyId = p.Key,
                Amount = p.Sum(s => s.Amount.GetValueOrDefault(0) * s.DestAmount / s.BaseAmount),
                BaseAmount = p.FirstOrDefault().BaseAmount,
                DestAmount = p.FirstOrDefault().DestAmount,
                BaseCurrencyId = p.FirstOrDefault().BaseCurrencyId,
                BaseCurrencyName = p.FirstOrDefault().BaseCurrencyName,
                Discount = p.Sum(s => s.Discount.GetValueOrDefault(0) * s.DestAmount / s.BaseAmount),
            });

            //var neg = receive_result.Where(p => p.Amount < 0).ToList();
            //var neg_currency = neg.Select(p => p.BaseCurrencyName);
            //var conflict = amounts.Any(p => neg_currency.Any() && !neg_currency.Contains(p.CurrencyName));
            //if (conflict)
            //    return "CurrencyConflict";

            //if (neg.Any())
            //    return "NegativeAmount";

            var discount = receive_result.Any(p => Math.Abs((p.Amount).GetValueOrDefault(0)) < Math.Abs(p.Discount.GetValueOrDefault(0)));
            if (discount)
                return "DiscountIsGreaterThanTheAmount";

            Receive receive = ConvertModel(viewModel);

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseRecevieDate");
            if (!access)
            {
                receive.ReceiveDate = DateTime.Now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.ReceiveDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime receiveDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                receive.ReceiveDate = receiveDate;
            }


            receive.ReceiveAmounts = receive_amounts.GroupBy(p => p.BaseCurrencyId).Select(p => new ReceiveAmount
            {
                BaseCurrencyId = p.Key,
                Amount = p.Sum(s => s.Amount.GetValueOrDefault(0)),
                Discount = p.FirstOrDefault().Discount,
                BaseAmount = p.FirstOrDefault().BaseAmount,
                DestAmount = p.FirstOrDefault().DestAmount,
                CurrencyId = p.FirstOrDefault().CurrencyId,
            }).ToList();

            receive.SaleInvoiceReceives = invoices.Select(p => new SaleInvoiceReceive
            {
                InvoiceId = p.Guid,
                FullPay = true
            }).ToList();

            receive.ReturnSaleInvoiceReceives = returns.Select(p => new ReturnSaleInvoiceReceive
            {
                InvoiceId = p.Guid,
                FullPay = true
            }).ToList();

            while (true)
            {
                try
                {
                    receive.InvoiceNum = GetReceiveInvoiceNum(viewModel.ClinicSectionId.Value);


                    _unitOfWork.Receives.Add(receive);
                    _unitOfWork.Complete();

                    break;
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("") && !(e.InnerException?.Message ?? "UQ_Receive_InvoiceNum_ClinicSectionId").Contains("UQ_Receive_InvoiceNum_ClinicSectionId"))
                        throw e;
                }
            }

            return receive.Guid.ToString();
        }

        public string AddNewReceive(ReceiveViewModel viewModel)
        {
            if (!viewModel.ReceiveAmounts.Any())
                return "DataNotValid";

            List<ReceiveAmount> recAmo = new List<ReceiveAmount>();

            foreach (var receiveamount in viewModel.ReceiveAmounts)
            {
                //var receive_amount = viewModel.ReceiveAmounts.FirstOrDefault();
                if (receiveamount.Amount.GetValueOrDefault(0) <= 0 || receiveamount.BaseAmount.GetValueOrDefault(0) <= 0 || receiveamount.DestAmount.GetValueOrDefault(0) <= 0)
                    return "DataNotValid";

                if (receiveamount.Amount < receiveamount.Discount)
                    return "DiscountIsGreaterThanTheAmount";

                if (receiveamount.BaseCurrencyId == receiveamount.CurrencyId)
                    receiveamount.BaseAmount = receiveamount.DestAmount = 1;

                recAmo.Add(new ReceiveAmount
                {
                    Amount = receiveamount.Amount,
                    Discount = receiveamount.Discount,
                    BaseAmount = receiveamount.BaseAmount,
                    DestAmount = receiveamount.DestAmount,
                    BaseCurrencyId = receiveamount.BaseCurrencyId,
                    CurrencyId = receiveamount.CurrencyId,
                });
            }

            Receive receive = ConvertModel(viewModel);

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseRecevieDate");
            if (!access)
            {
                receive.ReceiveDate = DateTime.Now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.ReceiveDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime receiveDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                receive.ReceiveDate = receiveDate;
            }

            receive.ReceiveAmounts = recAmo;

            while (true)
            {
                try
                {
                    receive.InvoiceNum = GetReceiveInvoiceNum(viewModel.ClinicSectionId.Value);


                    _unitOfWork.Receives.Add(receive);
                    _unitOfWork.Complete();

                    break;
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("") && !(e.InnerException?.Message ?? "UQ_Receive_InvoiceNum_ClinicSectionId").Contains("UQ_Receive_InvoiceNum_ClinicSectionId"))
                        throw e;
                }
            }

            return receive.Guid.ToString();
        }

        public string UpdateReceive(ReceiveViewModel viewModel)
        {
            if (!viewModel.ReceiveAmounts.Any())
                return "DataNotValid";

            var receive_amount = viewModel.ReceiveAmounts.FirstOrDefault();
            if (receive_amount.Amount.GetValueOrDefault(0) <= 0 || receive_amount.BaseAmount.GetValueOrDefault(0) <= 0 || receive_amount.DestAmount.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            if (receive_amount.Amount < receive_amount.Discount)
                return "DiscountIsGreaterThanTheAmount";

            if (receive_amount.BaseCurrencyId == receive_amount.CurrencyId)
                receive_amount.BaseAmount = receive_amount.DestAmount = 1;

            var receive = _unitOfWork.Receives.GetWithReceiveAmount(viewModel.Guid);
            receive.ModifiedDate = viewModel.ModifiedDate;
            receive.ModifiedUserId = viewModel.ModifiedUserId;
            receive.Description = viewModel.Description;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseRecevieDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.ReceiveDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime receiveDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                receive.ReceiveDate = receiveDate;
            }


            var total = receive.ReceiveAmounts.FirstOrDefault();
            total.CurrencyId = receive_amount.CurrencyId;
            total.BaseCurrencyId = receive_amount.BaseCurrencyId;
            total.Amount = receive_amount.Amount;
            total.Discount = receive_amount.Discount;
            total.BaseAmount = receive_amount.BaseAmount;
            total.DestAmount = receive_amount.DestAmount;

            _unitOfWork.Receives.UpdateState(receive);
            _unitOfWork.ReceiveAmounts.UpdateState(total);
            _unitOfWork.Complete();
            return receive.Guid.ToString();
        }

        public ReceiveViewModel GetReceive(Guid receiveId)
        {
            Receive receive = _unitOfWork.Receives.GetWithCustomer(receiveId);
            var result = ConvertTotalModel(receive);

            var receiveType = _unitOfWork.Receives.CheckReceiveStatus(receiveId);

            if (string.IsNullOrWhiteSpace(receiveType))
                result.ReceiveType = "Total";
            else if (receiveType.Contains("0"))
                result.ReceiveType = "Partial";
            else
                result.ReceiveType = "Factor";

            return result;
        }

        public decimal GetSaleInvoiceReceives(Guid saleInvoiceId, int currencyId)
        {
            try
            {
                return _unitOfWork.Receives.GetSaleInvoiceReceives(saleInvoiceId, currencyId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<ReceiveViewModel> GetPartialReceiveHistory(IEnumerable<string> receiveIds)
        {
            var list = receiveIds.Where(p => !string.IsNullOrWhiteSpace(p));

            if (!list.Any())
                return new List<ReceiveViewModel>();

            var guids = list.SelectMany(p => p.Split(" "));

            var result = _unitOfWork.Receives.GetPartialReceiveHistory(guids);
            CultureInfo cultures = new CultureInfo("en-US");
            var res = result.Select(p => new ReceiveViewModel
            {
                ReceiveDateTxt = p.ReceiveDate.Value.ToString("dd/MM/yyyy", cultures),
                //Amount = p.Amount * p.DestAmount / p.BaseAmount,
                //Discount = p.Discount.GetValueOrDefault(0),
                //Description = p.Description,
                //ReceiveType = p.Currency.Name
            });

            Indexing<ReceiveViewModel> indexing = new Indexing<ReceiveViewModel>();
            return indexing.AddIndexing(res.ToList());
        }

        public IEnumerable<ReceiveAmountViewModel> GetAllReceiveAmount(Guid saleInvoiceId, int currencyId)
        {
            IEnumerable<ReceiveAmount> result = _unitOfWork.ReceiveAmounts.GetAllReceiveAmount(saleInvoiceId, currencyId);
            var all = ConvertReceiveAmountModelList(result);
            Indexing<ReceiveAmountViewModel> indexing = new Indexing<ReceiveAmountViewModel>();
            return indexing.AddIndexing(all);


        }

        public string GetReceiveInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                string purchaseInvoiceNum = _unitOfWork.Receives.GetLatestReceiveInvoiceNum(clinicSectionId);
                return NextReceiveInvoiceNum(purchaseInvoiceNum);
            }
            catch (Exception) { return "1"; }
        }

        public string NextReceiveInvoiceNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }

        // Begin Convert 

        public Receive ConvertModel(ReceiveViewModel receive)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiveViewModel, Receive>()
                .ForMember(a => a.ReceiveAmounts, b => b.Ignore())
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceiveViewModel, Receive>(receive);
        }

        public ReceiveViewModel ConvertTotalModel(Receive receive)
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<ReceiveAmount, ReceiveAmountViewModel>()
                .ForMember(a => a.BaseCurrencyName, b => b.MapFrom(c => c.BaseCurrency.Name))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                .ForMember(a => a.Discount, b => b.MapFrom(c => Math.Abs(c.Discount.GetValueOrDefault(0))))
                .ForMember(a => a.Amount, b => b.MapFrom(c => c.Amount.GetValueOrDefault(0)))
                .ForMember(a => a.BaseAmount, b => b.MapFrom(c => c.BaseAmount.GetValueOrDefault(1)))
                .ForMember(a => a.DestAmount, b => b.MapFrom(c => c.DestAmount.GetValueOrDefault(1)))
                ;
                cfg.CreateMap<Receive, ReceiveViewModel>()
                .ForMember(a => a.CustomerName, b => b.MapFrom(c => c.Customer.User.Name))
                //.ForMember(a => a.ReceiveAmounts, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Receive, ReceiveViewModel>(receive);
        }

        public List<ReceiveAmountViewModel> ConvertReceiveAmountModelList(IEnumerable<ReceiveAmount> receive)
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<ReceiveAmount, ReceiveAmountViewModel>()
                .ForMember(a => a.ReceiveDate, b => b.MapFrom(c => c.Receive.ReceiveDate))
                .ForMember(a => a.AmountTxt, b => b.MapFrom(c => (c.CurrencyId == c.BaseCurrencyId) ? c.Amount.Value.ToString("#.##") : c.Amount.Value.ToString("#.##") + "(" + (c.Amount * c.DestAmount / c.BaseAmount).Value.ToString("#.##") + " " + c.BaseCurrency.Name + ")"))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<ReceiveAmount>, List<ReceiveAmountViewModel>>(receive);
        }









        // End Convert
    }
}
