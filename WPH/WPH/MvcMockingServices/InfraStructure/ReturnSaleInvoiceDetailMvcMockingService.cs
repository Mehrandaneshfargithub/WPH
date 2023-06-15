using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.MoneyConvert;
using WPH.Models.ReturnSaleInvoiceDetail;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReturnSaleInvoiceDetailMvcMockingService : IReturnSaleInvoiceDetailMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ReturnSaleInvoiceDetailMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemoveReturnSaleInvoiceDetail(Guid returnSaleInvoiceDetailid)
        {
            try
            {
                ReturnSaleInvoiceDetail Hos = _unitOfWork.ReturnSaleInvoiceDetails.GetWithSaleInvoice(returnSaleInvoiceDetailid);

                var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(Hos.MasterId.Value);
                if (can_change)
                    return "InvoiceInUse";

                var currencyExist = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(Hos.MasterId.Value);
                if (currencyExist == null)
                    return "DataNotValid";

                var total_sale = currencyExist.ReturnSaleInvoiceDetails.Where(p => p.CurrencyId == Hos.CurrencyId && p.Guid != returnSaleInvoiceDetailid).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
                var total_discount = currencyExist.ReturnSaleInvoiceDiscounts.Where(x => x.CurrencyId == Hos.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
                var after_discount = total_sale - total_discount;
                if (after_discount < 0)
                    return "DiscountIsGreaterThanTheAmount";

                _unitOfWork.ReturnSaleInvoiceDetails.Remove(Hos);
                var total = Hos.Num.GetValueOrDefault(0) + Hos.FreeNum.GetValueOrDefault(0);
                if (Hos.SaleInvoiceDetail.TransferDetail != null)
                {
                    var transfer = Hos.SaleInvoiceDetail.TransferDetail;

                    transfer.RemainingNum -= total;
                    _unitOfWork.TransferDetails.UpdateState(transfer);
                }
                else
                {
                    var purchase = Hos.SaleInvoiceDetail.PurchaseInvoiceDetail;

                    purchase.RemainingNum -= total;
                    _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                }

                var sale = Hos.SaleInvoiceDetail;
                sale.RemainingNum += total;

                _unitOfWork.SaleInvoiceDetails.UpdateState(sale);

                //_unitOfWork.Complete();
                //_unitOfWork.ReturnSaleInvoiceDetails.Detach(Hos);

                currencyExist.ReturnSaleInvoiceDetails = currencyExist.ReturnSaleInvoiceDetails.Where(p => p.Guid != returnSaleInvoiceDetailid).ToList();

                return _idunit.returnSaleInvoice.UpdateTotalPrice(currencyExist);
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
            string controllerName = "/ReturnSaleInvoiceDetail/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewReturnSaleInvoiceDetailList(IEnumerable<ReturnSaleInvoiceDetailViewModel> viewModels, Guid clinicSectionId, Guid userId)
        {
            if (viewModels == null || !viewModels.Any())
                return "EmptyList";

            if (viewModels.Any(p => string.IsNullOrWhiteSpace(p.ReasonTxt)))
                return "EmptyReason";

            if (viewModels.Any(p => p.SaleInvoiceDetailId == null || p.MasterId == null))
                return "DataNotValid";

            var master_id = viewModels.FirstOrDefault().MasterId;
            if (viewModels.Any(p => p.MasterId != master_id))
                return "DataNotValid";

            var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(master_id.Value);
            if (can_change)
                return "InvoiceInUse";

            string txt = viewModels.First().ReasonTxt;
            var create_date = DateTime.Now;

            var reason_type = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(txt, "Reason", clinicSectionId);
            var reason = reason_type?.BaseInfos?.FirstOrDefault()?.Guid;

            if (reason == null)
            {
                reason = Guid.NewGuid();
                var new_reason = new BaseInfo
                {
                    Guid = reason.Value,
                    Name = txt,
                    ClinicSectionId = clinicSectionId,
                    TypeId = reason_type.Guid
                };

                _unitOfWork.BaseInfos.Add(new_reason);
            }

            var sale_list = viewModels.Select(p => p.SaleInvoiceDetailId.Value).ToList();
            List<SaleInvoiceDetail> sales = _unitOfWork.SaleInvoiceDetails.GetByMultipleIds(sale_list).ToList();

            List<ReturnSaleInvoiceDetail> returns = new List<ReturnSaleInvoiceDetail>();
            returns.AddRange(sales.Select(p => new ReturnSaleInvoiceDetail
            {
                MasterId = master_id,
                SaleInvoiceDetailId = p.Guid,
                Num = p.RemainingNum,
                FreeNum = 0,
                Discount = 0,
                Price = p.SalePrice,
                CurrencyId = p.CurrencyId,
                ReasonId = reason,
                CreateDate = create_date,
                CreatedUserId = userId,
            }));

            _unitOfWork.ReturnSaleInvoiceDetails.AddRange(returns);
            foreach (var item in sales)
            {
                if (item.PurchaseInvoiceDetail == null)
                {
                    _unitOfWork.TransferDetails.IncreaseUpdateWithLocal(item.TransferDetail, item.RemainingNum.GetValueOrDefault(0));
                }
                else
                {
                    _unitOfWork.PurchaseInvoiceDetails.IncreaseUpdateWithLocal(item.PurchaseInvoiceDetail, item.RemainingNum.GetValueOrDefault(0));
                }

                item.RemainingNum = 0;
                _unitOfWork.SaleInvoiceDetails.UpdateState(item);
            }

            //_unitOfWork.Complete();

            var currencies = _unitOfWork.BaseInfoGenerals.GetAllNamesByType("Currency");

            var currencyExist = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(master_id.Value);
            foreach (var item in returns)
            {
                item.CurrencyName = currencies.FirstOrDefault(p => p.Id == item.CurrencyId.Value).Name;

                currencyExist.ReturnSaleInvoiceDetails.Add(item);
            }

            return _idunit.returnSaleInvoice.UpdateTotalPrice(currencyExist);
        }

        public string AddNewReturnSaleInvoiceDetail(ReturnSaleInvoiceDetailViewModel viewModel, Guid clinicSectionId)
        {
            var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(viewModel.MasterId.Value);
            if (can_change)
                return "InvoiceInUse";

            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Num = decimal.Parse(viewModel.NumTxt ?? "0", cultures);
            viewModel.FreeNum = decimal.Parse(viewModel.FreeNumTxt ?? "0", cultures);
            viewModel.Discount = decimal.Parse(viewModel.DiscountTxt ?? "0", cultures);
            viewModel.TotalPrice = decimal.Parse(viewModel.TotalPriceTxt ?? "0", cultures);

            if (viewModel.MasterId == null || viewModel.SaleInvoiceDetailId == null || viewModel.Num.GetValueOrDefault(0) <= 0 ||
                string.IsNullOrWhiteSpace(viewModel.ReasonTxt) || viewModel.TotalPrice.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            ReturnSaleInvoiceDetail returnSaleInvoiceDetail = ConvertModelWithOutGuid(viewModel);
            returnSaleInvoiceDetail.CreateDate = viewModel.CreateDate ?? DateTime.Now;
            returnSaleInvoiceDetail.Price = viewModel.TotalPrice / viewModel.Num;

            if ((returnSaleInvoiceDetail.Num * returnSaleInvoiceDetail.Price) - returnSaleInvoiceDetail.Discount.GetValueOrDefault(0) < 0)
                return "DiscountIsGreaterThanTheAmount";

            var saleInvoice = _unitOfWork.SaleInvoiceDetails.GetWithPurchaseAndTransfer(viewModel.SaleInvoiceDetailId.Value);
            var total_num = viewModel.Num.GetValueOrDefault(0) + viewModel.FreeNum.GetValueOrDefault(0);
            if (total_num > saleInvoice.RemainingNum)
                return "NotEnoughProductCount";

            returnSaleInvoiceDetail.CurrencyId = saleInvoice.CurrencyId;
            saleInvoice.RemainingNum -= total_num;
            _unitOfWork.SaleInvoiceDetails.UpdateState(saleInvoice);

            if (saleInvoice.TransferDetail != null)
            {
                var transfer = saleInvoice.TransferDetail;

                transfer.RemainingNum += total_num;
                _unitOfWork.TransferDetails.UpdateState(transfer);
            }
            else
            {
                var purchase = saleInvoice.PurchaseInvoiceDetail;

                purchase.RemainingNum += total_num;
                _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
            }

            var reason = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.ReasonTxt, "Reason", clinicSectionId);
            returnSaleInvoiceDetail.ReasonId = reason?.BaseInfos?.FirstOrDefault()?.Guid;

            if (returnSaleInvoiceDetail.ReasonId == null)
            {
                returnSaleInvoiceDetail.Reason = new BaseInfo
                {
                    Name = viewModel.ReasonTxt,
                    ClinicSectionId = clinicSectionId,
                    TypeId = reason.Guid
                };

                _unitOfWork.BaseInfos.Add(returnSaleInvoiceDetail.Reason);
            }

            _unitOfWork.ReturnSaleInvoiceDetails.Add(returnSaleInvoiceDetail);
            //_unitOfWork.Complete();

            //_unitOfWork.ReturnSaleInvoiceDetails.Detach(returnSaleInvoiceDetail);

            var currencyExist = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(viewModel.MasterId.Value);
            returnSaleInvoiceDetail.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(returnSaleInvoiceDetail.CurrencyId.Value);
            currencyExist.ReturnSaleInvoiceDetails.Add(returnSaleInvoiceDetail);

            return _idunit.returnSaleInvoice.UpdateTotalPrice(currencyExist);
        }


        public string UpdateReturnSaleInvoiceDetail(ReturnSaleInvoiceDetailViewModel viewModel, Guid clinicSectionId)
        {
            var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(viewModel.MasterId.Value);
            if (can_change)
                return "InvoiceInUse";

            ReturnSaleInvoiceDetail oldReturn = _unitOfWork.ReturnSaleInvoiceDetails.GetWithPurchaseAndTransfer(viewModel.Guid);

            if (oldReturn.SaleInvoiceDetailId != viewModel.SaleInvoiceDetailId)
            {
                var old_total = oldReturn.Num.GetValueOrDefault(0) + oldReturn.FreeNum.GetValueOrDefault(0);

                if (oldReturn.SaleInvoiceDetail.TransferDetail != null)
                {
                    var transfer = oldReturn.SaleInvoiceDetail.TransferDetail;
                    transfer.RemainingNum += old_total;
                    _unitOfWork.TransferDetails.UpdateState(transfer);
                }
                else
                {
                    var purchase = oldReturn.SaleInvoiceDetail.PurchaseInvoiceDetail;
                    purchase.RemainingNum += old_total;
                    _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                }

                _unitOfWork.ReturnSaleInvoiceDetails.Remove(oldReturn);

                viewModel.CreateDate = oldReturn.CreateDate;
                viewModel.CreatedUserId = oldReturn.CreatedUserId;
                viewModel.ModifiedDate = DateTime.Now;
                return AddNewReturnSaleInvoiceDetail(viewModel, clinicSectionId);
            }

            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Num = decimal.Parse(viewModel.NumTxt ?? "0", cultures);
            viewModel.FreeNum = decimal.Parse(viewModel.FreeNumTxt ?? "0", cultures);
            viewModel.Discount = decimal.Parse(viewModel.DiscountTxt ?? "0", cultures);
            viewModel.TotalPrice = decimal.Parse(viewModel.TotalPriceTxt ?? "0", cultures);

            if (viewModel.MasterId == null || viewModel.SaleInvoiceDetailId == null || viewModel.Num.GetValueOrDefault(0) <= 0 ||
                string.IsNullOrWhiteSpace(viewModel.ReasonTxt) || viewModel.TotalPrice.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            var new_num = viewModel.Num.GetValueOrDefault(0) + viewModel.FreeNum.GetValueOrDefault(0);
            var current_num = oldReturn.Num.GetValueOrDefault(0) + oldReturn.FreeNum.GetValueOrDefault(0);
            var diff = new_num - current_num;

            var saleInvoice = oldReturn.SaleInvoiceDetail;
            if (diff > saleInvoice.RemainingNum)
                return "NotEnoughProductCount";

            saleInvoice.RemainingNum -= diff;
            _unitOfWork.SaleInvoiceDetails.UpdateState(saleInvoice);

            if (saleInvoice.TransferDetail != null)
            {
                var transfer = saleInvoice.TransferDetail;

                transfer.RemainingNum += diff;
                _unitOfWork.TransferDetails.UpdateState(transfer);
            }
            else
            {
                var purchase = saleInvoice.PurchaseInvoiceDetail;

                purchase.RemainingNum += diff;
                _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
            }

            var reason = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.ReasonTxt, "Reason", clinicSectionId);
            oldReturn.ReasonId = reason?.BaseInfos?.FirstOrDefault()?.Guid;

            if (oldReturn.ReasonId == null)
            {
                oldReturn.Reason = new BaseInfo
                {
                    Name = viewModel.ReasonTxt,
                    ClinicSectionId = clinicSectionId,
                    TypeId = reason.Guid
                };

                _unitOfWork.BaseInfos.Add(oldReturn.Reason);
            }

            oldReturn.ModifiedUserId = viewModel.ModifiedUserId;
            oldReturn.ModifiedDate = DateTime.Now;
            oldReturn.Discount = viewModel.Discount;
            oldReturn.FreeNum = viewModel.FreeNum;
            oldReturn.Num = viewModel.Num;
            oldReturn.CurrencyId = saleInvoice.CurrencyId;
            oldReturn.Price = viewModel.TotalPrice / viewModel.Num;

            if ((oldReturn.Num * oldReturn.Price) - oldReturn.Discount.GetValueOrDefault(0) < 0)
                return "DiscountIsGreaterThanTheAmount";

            _unitOfWork.ReturnSaleInvoiceDetails.UpdateState(oldReturn);
            //_unitOfWork.Complete();

            //_unitOfWork.ReturnSaleInvoiceDetails.Detach(oldReturn);

            var currencyExist = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(oldReturn.MasterId.Value);
            currencyExist.ReturnSaleInvoiceDetails = currencyExist.ReturnSaleInvoiceDetails.Where(p => p.Guid != oldReturn.Guid).ToList();
            oldReturn.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(oldReturn.CurrencyId.Value);
            currencyExist.ReturnSaleInvoiceDetails.Add(oldReturn);

            return _idunit.returnSaleInvoice.UpdateTotalPrice(currencyExist);
        }


        public IEnumerable<ReturnSaleInvoiceDetailViewModel> GetAllReturnSaleInvoiceDetails(Guid returnSaleInvoiceId)
        {
            IEnumerable<ReturnSaleInvoiceDetail> hosp = _unitOfWork.ReturnSaleInvoiceDetails.GetAllReturnSaleInvoiceDetailByMasterId(returnSaleInvoiceId);
            List<ReturnSaleInvoiceDetailViewModel> hospconvert = ConvertModelsLists(hosp);

            var result = hospconvert.GroupBy(p => new { p.ProductName, p.CurrencyName, p.Price }).Select(x => new ReturnSaleInvoiceDetailViewModel
            {
                ChildrenCount = x.Count(),
                ChildrenGuids = string.Join(",", x.Select(s => s.Guid)),
                Guid = x.FirstOrDefault().Guid,
                ProductName = x.Key.ProductName,
                Num = x.Sum(s => s.Num.GetValueOrDefault(0)),
                FreeNum = x.Sum(s => s.FreeNum.GetValueOrDefault(0)),
                Discount = x.Sum(s => s.Discount.GetValueOrDefault(0)),
                Price = x.Key.Price,
                CurrencyName = x.Key.CurrencyName,
                ReasonTxt = x.FirstOrDefault().ReasonTxt
            }).ToList();
            Indexing<ReturnSaleInvoiceDetailViewModel> indexing = new Indexing<ReturnSaleInvoiceDetailViewModel>();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<SubReturnSaleInvoiceDetailViewModel> GetAllReturnSaleInvoiceDetailChildren(string children)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var ids = children.Split(",").ToList();
            IEnumerable<ReturnSaleInvoiceDetail> hosp = _unitOfWork.ReturnSaleInvoiceDetails.GetAllReturnSaleInvoiceDetailByIds(ids.ConvertAll(Guid.Parse));
            List<ReturnSaleInvoiceDetailViewModel> hospconvert = ConvertModelsLists(hosp);

            Indexing<ReturnSaleInvoiceDetailViewModel> indexing = new Indexing<ReturnSaleInvoiceDetailViewModel>();
            return indexing.AddIndexing(hospconvert).Select(p => new SubReturnSaleInvoiceDetailViewModel
            {
                SubIndex = p.Index,
                SubGuid = p.Guid,
                SubProductName = p.ProductName,
                SubNum = p.Num.GetValueOrDefault(0).ToString("#,0.##", cultures),
                SubFreeNum = p.FreeNum.GetValueOrDefault(0).ToString("#,0.##", cultures),
                SubPrice = p.Price.GetValueOrDefault(0).ToString("#,0.##", cultures),
                SubDiscount = $"{p.CurrencyName} { p.Discount.GetValueOrDefault(0).ToString("#,0.##", cultures)}",
                SubCurrencyName = p.CurrencyName,
                SubTotalPrice = p.TotalPrice.GetValueOrDefault(0).ToString("#,0.##", cultures),
                SubTotal = $"{p.CurrencyName} {(p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0)).ToString("#,0.##", cultures)}",
                SubTotalAfterDiscount = $"{p.CurrencyName} {(p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}",
                SubReasonTxt = p.ReasonTxt
            });
        }

        public ReturnSaleInvoiceDetailViewModel GetReturnSaleInvoiceDetailForEdit(Guid saleInvoiceDetailId)
        {
            try
            {
                ReturnSaleInvoiceDetail ReturnSaleInvoiceDetailgu = _unitOfWork.ReturnSaleInvoiceDetails.GetReturnSaleInvoiceDetailForEdit(saleInvoiceDetailId);
                return ConvertModel(ReturnSaleInvoiceDetailgu);
            }
            catch (Exception) { return null; }
        }


        public PieChartViewModel GetMostReturnedProducts(Guid clinicSectionId)
        {
            try
            {
                var result = _unitOfWork.ReturnSaleInvoiceDetails.GetMostReturnedProducts(clinicSectionId);
                PieChartViewModel pie = new PieChartViewModel
                {
                    Labels = result.Select(a => a.Label).ToArray(),
                    Value = result.Select(a => Convert.ToInt32(a.Value ?? 0)).ToArray()
                };

                return pie;
            }
            catch (Exception e) { throw e; }
        }


        // Begin Convert 
        public ReturnSaleInvoiceDetail ConvertModelWithOutGuid(ReturnSaleInvoiceDetailViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnSaleInvoiceDetailViewModel, ReturnSaleInvoiceDetail>()
                .ForMember(a => a.Guid, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnSaleInvoiceDetailViewModel, ReturnSaleInvoiceDetail>(Users);
        }

        public ReturnSaleInvoiceDetailViewModel ConvertModel(ReturnSaleInvoiceDetail Users)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnSaleInvoiceDetail, ReturnSaleInvoiceDetailViewModel>()
                .ForMember(a => a.ProductId, b => b.MapFrom(c => c.SaleInvoiceDetail.ProductId))
                .ForMember(a => a.ReasonTxt, b => b.MapFrom(c => c.Reason.Name))
                .ForMember(a => a.NumTxt, b => b.MapFrom(c => c.Num.GetValueOrDefault(1).ToString("0.##", cultures)))
                .ForMember(a => a.FreeNumTxt, b => b.MapFrom(c => c.FreeNum.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.PriceTxt, b => b.MapFrom(c => c.Price.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.DiscountTxt, b => b.MapFrom(c => c.Discount.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.SaleInvoiceDetailTxt, b => b.MapFrom(c => $"{c.SaleInvoiceDetail.SaleInvoice.InvoiceNum} | {c.SaleInvoiceDetail.SaleInvoice.InvoiceDate.Value:dd/MM/yyyy}"))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnSaleInvoiceDetail, ReturnSaleInvoiceDetailViewModel>(Users);
        }

        public List<ReturnSaleInvoiceDetailViewModel> ConvertModelsLists(IEnumerable<ReturnSaleInvoiceDetail> saleInvoiceDetails)
        {
            List<ReturnSaleInvoiceDetailViewModel> saleInvoiceDetailDtoList = new List<ReturnSaleInvoiceDetailViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnSaleInvoiceDetail, ReturnSaleInvoiceDetailViewModel>()
                .ForMember(a => a.ProductName, b => b.MapFrom(c => $"{c.SaleInvoiceDetail.Product.Name} | {c.SaleInvoiceDetail.Product.ProductType.Name} | {c.SaleInvoiceDetail.Product.Producer.Name}"))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                .ForMember(a => a.ReasonTxt, b => b.MapFrom(c => c.Reason.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            saleInvoiceDetailDtoList = mapper.Map<IEnumerable<ReturnSaleInvoiceDetail>, List<ReturnSaleInvoiceDetailViewModel>>(saleInvoiceDetails);
            return saleInvoiceDetailDtoList;
        }



        // End Convert
    }
}
