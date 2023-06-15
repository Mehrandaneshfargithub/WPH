using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.ReturnPurchaseInvoiceDetail;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReturnPurchaseInvoiceDetailMvcMockingService : IReturnPurchaseInvoiceDetailMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ReturnPurchaseInvoiceDetailMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemoveReturnPurchaseInvoiceDetail(Guid purchaseInvoiceDetailid)
        {
            try
            {
                ReturnPurchaseInvoiceDetail Hos = _unitOfWork.ReturnPurchaseInvoiceDetails.Get(purchaseInvoiceDetailid);

                var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(Hos.MasterId.Value);
                if (can_change)
                    return "InvoiceInUse";

                var currencyExist = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(Hos.MasterId.Value);
                if (currencyExist == null)
                    return "DataNotValid";

                var total_purchase = currencyExist.ReturnPurchaseInvoiceDetails.Where(p => p.CurrencyId == Hos.CurrencyId && p.Guid != purchaseInvoiceDetailid).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
                var total_discount = currencyExist.ReturnPurchaseInvoiceDiscounts.Where(x => x.CurrencyId == Hos.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
                var after_discount = total_purchase - total_discount;
                if (after_discount < 0)
                    return "DiscountIsGreaterThanTheAmount";

                _unitOfWork.ReturnPurchaseInvoiceDetails.Remove(Hos);
                var total = Hos.Num.GetValueOrDefault(0) + Hos.FreeNum.GetValueOrDefault(0);
                if (Hos.TransferDetailId != null)
                {
                    var transfer = _unitOfWork.TransferDetails.Get(Hos.TransferDetailId.Value);

                    transfer.RemainingNum += total;
                    _unitOfWork.TransferDetails.UpdateState(transfer);
                }
                else
                {
                    var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(Hos.PurchaseInvoiceDetailId.Value);

                    purchase.RemainingNum += total;
                    _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                }

                //_unitOfWork.Complete();
                //_unitOfWork.ReturnPurchaseInvoiceDetails.Detach(Hos);

                currencyExist.ReturnPurchaseInvoiceDetails = currencyExist.ReturnPurchaseInvoiceDetails.Where(p => p.Guid != purchaseInvoiceDetailid).ToList();

                return _idunit.returnPurchaseInvoice.UpdateTotalPrice(currencyExist);
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
            string controllerName = "/ReturnPurchaseInvoiceDetail/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewReturnPurchaseInvoiceDetailList(IEnumerable<ReturnPurchaseInvoiceDetailViewModel> viewModels, Guid clinicSectionId, Guid userId)
        {
            if (viewModels == null || !viewModels.Any())
                return "EmptyList";

            if (viewModels.Any(p => string.IsNullOrWhiteSpace(p.ReasonTxt)))
                return "EmptyReason";

            if (viewModels.Any(p => p.PurchaseInvoiceDetailId == null || p.MasterId == null))
                return "DataNotValid";

            var master_id = viewModels.FirstOrDefault().MasterId;
            if (viewModels.Any(p => p.MasterId != master_id))
                return "DataNotValid";

            var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(master_id.Value);
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
            var purchase_list = viewModels.Where(p => p.TransferDetailId == null).Select(p => p.PurchaseInvoiceDetailId.Value).ToList();
            List<PurchaseInvoiceDetail> purchases = _unitOfWork.PurchaseInvoiceDetails.GetByMultipleIds(purchase_list).ToList();

            var transfer_list = viewModels.Where(p => p.TransferDetailId != null).Select(p => p.TransferDetailId.Value).ToList();
            List<TransferDetail> transfers = _unitOfWork.TransferDetails.GetWithPurchaseInvoiceDetail(transfer_list).ToList();

            List<ReturnPurchaseInvoiceDetail> returns = new List<ReturnPurchaseInvoiceDetail>();
            returns.AddRange(purchases.Select(p => new ReturnPurchaseInvoiceDetail
            {
                MasterId = master_id,
                PurchaseInvoiceDetailId = p.Guid,
                TransferDetailId = null,
                Num = p.RemainingNum,
                FreeNum = 0,
                Discount = 0,
                Price = p.PurchasePrice,
                CurrencyId = p.CurrencyId,
                ReasonId = reason,
                CreateDate = create_date,
                CreatedUserId = userId,
            }));

            returns.AddRange(transfers.Select(p => new ReturnPurchaseInvoiceDetail
            {
                MasterId = master_id,
                PurchaseInvoiceDetailId = p.SourcePurchaseInvoiceDetailId,
                TransferDetailId = p.Guid,
                Num = p.RemainingNum,
                FreeNum = 0,
                Discount = 0,
                Price = p.PurchasePrice,
                CurrencyId = p.SourcePurchaseInvoiceDetail.CurrencyId,
                ReasonId = reason,
                CreateDate = create_date,
                CreatedUserId = userId,
            }));

            _unitOfWork.ReturnPurchaseInvoiceDetails.AddRange(returns);
            foreach (var item in purchases)
            {
                item.RemainingNum = 0;
                _unitOfWork.PurchaseInvoiceDetails.UpdateState(item);
            }

            foreach (var item in transfers)
            {
                item.RemainingNum = 0;
                _unitOfWork.TransferDetails.UpdateState(item);
            }

            //_unitOfWork.Complete();

            var currencies = _unitOfWork.BaseInfoGenerals.GetAllNamesByType("Currency");

            var currencyExist = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(master_id.Value);
            foreach (var item in returns)
            {
                item.CurrencyName = currencies.FirstOrDefault(p => p.Id == item.CurrencyId.Value).Name;

                currencyExist.ReturnPurchaseInvoiceDetails.Add(item);
            }

            return _idunit.returnPurchaseInvoice.UpdateTotalPrice(currencyExist);
        }

        public string AddNewReturnPurchaseInvoiceDetail(ReturnPurchaseInvoiceDetailViewModel viewModel, Guid clinicSectionId)
        {
            var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(viewModel.MasterId.Value);
            if (can_change)
                return "InvoiceInUse";

            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Num = decimal.Parse(viewModel.NumTxt ?? "0", cultures);
            viewModel.FreeNum = decimal.Parse(viewModel.FreeNumTxt ?? "0", cultures);
            viewModel.Discount = decimal.Parse(viewModel.DiscountTxt ?? "0", cultures);
            viewModel.TotalPrice = decimal.Parse(viewModel.TotalPriceTxt ?? "0", cultures);

            if (viewModel.MasterId == null || viewModel.PurchaseInvoiceDetailId == null || viewModel.Num.GetValueOrDefault(0) <= 0 ||
                string.IsNullOrWhiteSpace(viewModel.ReasonTxt) || viewModel.TotalPrice.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            ReturnPurchaseInvoiceDetail purchaseInvoiceDetail = ConvertModelWithOutGuid(viewModel);
            purchaseInvoiceDetail.CreateDate = viewModel.CreateDate ?? DateTime.Now;
            purchaseInvoiceDetail.Price = viewModel.TotalPrice / viewModel.Num;

            if ((purchaseInvoiceDetail.Num * purchaseInvoiceDetail.Price) - purchaseInvoiceDetail.Discount.GetValueOrDefault(0) < 0)
                return "DiscountIsGreaterThanTheAmount";

            var total_num = viewModel.Num.GetValueOrDefault(0) + viewModel.FreeNum.GetValueOrDefault(0);
            if (viewModel.TransferDetailId != null)
            {
                var transfer = _unitOfWork.TransferDetails.Get(viewModel.TransferDetailId.Value);
                if (total_num > transfer.RemainingNum)
                    return "NotEnoughProductCount";

                transfer.RemainingNum -= total_num;
                _unitOfWork.TransferDetails.UpdateState(transfer);
                purchaseInvoiceDetail.CurrencyId = transfer.CurrencyId;
            }
            else
            {
                var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(viewModel.PurchaseInvoiceDetailId.Value);
                if (total_num > purchase.RemainingNum)
                    return "NotEnoughProductCount";

                purchase.RemainingNum -= total_num;
                _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                purchaseInvoiceDetail.CurrencyId = purchase.CurrencyId;
            }

            var reason = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.ReasonTxt, "Reason", clinicSectionId);
            purchaseInvoiceDetail.ReasonId = reason?.BaseInfos?.FirstOrDefault()?.Guid;

            if (purchaseInvoiceDetail.ReasonId == null)
            {
                purchaseInvoiceDetail.Reason = new BaseInfo
                {
                    Name = viewModel.ReasonTxt,
                    ClinicSectionId = clinicSectionId,
                    TypeId = reason.Guid
                };

                _unitOfWork.BaseInfos.Add(purchaseInvoiceDetail.Reason);
            }

            _unitOfWork.ReturnPurchaseInvoiceDetails.Add(purchaseInvoiceDetail);
            //_unitOfWork.Complete();

            //_unitOfWork.ReturnPurchaseInvoiceDetails.Detach(purchaseInvoiceDetail);

            var currencyExist = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(viewModel.MasterId.Value);
            purchaseInvoiceDetail.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(purchaseInvoiceDetail.CurrencyId.Value);
            currencyExist.ReturnPurchaseInvoiceDetails.Add(purchaseInvoiceDetail);

            return _idunit.returnPurchaseInvoice.UpdateTotalPrice(currencyExist);
        }


        public string UpdateReturnPurchaseInvoiceDetail(ReturnPurchaseInvoiceDetailViewModel viewModel, Guid clinicSectionId)
        {
            var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(viewModel.MasterId.Value);
            if (can_change)
                return "InvoiceInUse";

            ReturnPurchaseInvoiceDetail purchaseInvoiceDetailOld = _unitOfWork.ReturnPurchaseInvoiceDetails.Get(viewModel.Guid);

            if (purchaseInvoiceDetailOld.PurchaseInvoiceDetailId != viewModel.PurchaseInvoiceDetailId || purchaseInvoiceDetailOld.TransferDetailId != viewModel.TransferDetailId)
            {
                var old_total = purchaseInvoiceDetailOld.Num.GetValueOrDefault(0) + purchaseInvoiceDetailOld.FreeNum.GetValueOrDefault(0);

                if (purchaseInvoiceDetailOld.TransferDetailId != null)
                {
                    var transfer = _unitOfWork.TransferDetails.Get(purchaseInvoiceDetailOld.TransferDetailId.Value);
                    transfer.RemainingNum += old_total;
                    _unitOfWork.TransferDetails.UpdateState(transfer);
                }
                else
                {
                    var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(purchaseInvoiceDetailOld.PurchaseInvoiceDetailId.Value);
                    purchase.RemainingNum += old_total;
                    _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                }

                _unitOfWork.ReturnPurchaseInvoiceDetails.Remove(purchaseInvoiceDetailOld);

                viewModel.CreateDate = purchaseInvoiceDetailOld.CreateDate;
                viewModel.CreatedUserId = purchaseInvoiceDetailOld.CreatedUserId;
                viewModel.ModifiedDate = DateTime.Now;
                return AddNewReturnPurchaseInvoiceDetail(viewModel, clinicSectionId);
            }

            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Num = decimal.Parse(viewModel.NumTxt ?? "0", cultures);
            viewModel.FreeNum = decimal.Parse(viewModel.FreeNumTxt ?? "0", cultures);
            viewModel.Discount = decimal.Parse(viewModel.DiscountTxt ?? "0", cultures);
            viewModel.TotalPrice = decimal.Parse(viewModel.TotalPriceTxt ?? "0", cultures);

            if (viewModel.MasterId == null || viewModel.PurchaseInvoiceDetailId == null || viewModel.Num.GetValueOrDefault(0) <= 0 ||
                string.IsNullOrWhiteSpace(viewModel.ReasonTxt) || viewModel.TotalPrice.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            var new_num = viewModel.Num.GetValueOrDefault(0) + viewModel.FreeNum.GetValueOrDefault(0);
            var current_num = purchaseInvoiceDetailOld.Num.GetValueOrDefault(0) + purchaseInvoiceDetailOld.FreeNum.GetValueOrDefault(0);
            var diff = new_num - current_num;

            if (viewModel.TransferDetailId != null)
            {
                var transfer = _unitOfWork.TransferDetails.Get(viewModel.TransferDetailId.Value);
                if (diff > transfer.RemainingNum)
                    return "NotEnoughProductCount";

                transfer.RemainingNum -= diff;
                _unitOfWork.TransferDetails.UpdateState(transfer);
                viewModel.CurrencyId = transfer.CurrencyId;
            }
            else
            {
                var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(viewModel.PurchaseInvoiceDetailId.Value);
                if (diff > purchase.RemainingNum)
                    return "NotEnoughProductCount";

                purchase.RemainingNum -= diff;
                _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                viewModel.CurrencyId = purchase.CurrencyId;
            }

            var reason = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.ReasonTxt, "Reason", clinicSectionId);
            purchaseInvoiceDetailOld.ReasonId = reason?.BaseInfos?.FirstOrDefault()?.Guid;

            if (purchaseInvoiceDetailOld.ReasonId == null)
            {
                purchaseInvoiceDetailOld.Reason = new BaseInfo
                {
                    Name = viewModel.ReasonTxt,
                    ClinicSectionId = clinicSectionId,
                    TypeId = reason.Guid
                };

                _unitOfWork.BaseInfos.Add(purchaseInvoiceDetailOld.Reason);
            }

            purchaseInvoiceDetailOld.ModifiedUserId = viewModel.ModifiedUserId;
            purchaseInvoiceDetailOld.ModifiedDate = DateTime.Now;
            purchaseInvoiceDetailOld.Discount = viewModel.Discount;
            purchaseInvoiceDetailOld.FreeNum = viewModel.FreeNum;
            purchaseInvoiceDetailOld.Num = viewModel.Num;
            purchaseInvoiceDetailOld.CurrencyId = viewModel.CurrencyId;
            purchaseInvoiceDetailOld.Price = viewModel.TotalPrice / viewModel.Num;

            if ((purchaseInvoiceDetailOld.Num * purchaseInvoiceDetailOld.Price) - purchaseInvoiceDetailOld.Discount.GetValueOrDefault(0) < 0)
                return "DiscountIsGreaterThanTheAmount";

            _unitOfWork.ReturnPurchaseInvoiceDetails.UpdateState(purchaseInvoiceDetailOld);
            //_unitOfWork.Complete();

            //_unitOfWork.ReturnPurchaseInvoiceDetails.Detach(purchaseInvoiceDetailOld);

            var currencyExist = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(purchaseInvoiceDetailOld.MasterId.Value);
            currencyExist.ReturnPurchaseInvoiceDetails = currencyExist.ReturnPurchaseInvoiceDetails.Where(p => p.Guid != purchaseInvoiceDetailOld.Guid).ToList();
            purchaseInvoiceDetailOld.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(purchaseInvoiceDetailOld.CurrencyId.Value);
            currencyExist.ReturnPurchaseInvoiceDetails.Add(purchaseInvoiceDetailOld);

            return _idunit.returnPurchaseInvoice.UpdateTotalPrice(currencyExist);
        }


        public IEnumerable<ReturnPurchaseInvoiceDetailViewModel> GetAllReturnPurchaseInvoiceDetails(Guid returnPurchaseInvoiceId)
        {
            IEnumerable<ReturnPurchaseInvoiceDetail> hosp = _unitOfWork.ReturnPurchaseInvoiceDetails.GetAllReturnPurchaseInvoiceDetailByMasterId(returnPurchaseInvoiceId);
            List<ReturnPurchaseInvoiceDetailViewModel> hospconvert = ConvertModelsLists(hosp);

            var result = hospconvert.GroupBy(p => new { p.ProductName, p.CurrencyName, p.Price }).Select(x => new ReturnPurchaseInvoiceDetailViewModel
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
            Indexing<ReturnPurchaseInvoiceDetailViewModel> indexing = new Indexing<ReturnPurchaseInvoiceDetailViewModel>();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<SubReturnPurchaseInvoiceDetailViewModel> GetAllReturnPurchaseInvoiceDetailChildren(string children)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var ids = children.Split(",").ToList();
            IEnumerable<ReturnPurchaseInvoiceDetail> hosp = _unitOfWork.ReturnPurchaseInvoiceDetails.GetAllReturnPurchaseInvoiceDetailByIds(ids.ConvertAll(Guid.Parse));
            List<ReturnPurchaseInvoiceDetailViewModel> hospconvert = ConvertModelsLists(hosp);

            Indexing<ReturnPurchaseInvoiceDetailViewModel> indexing = new Indexing<ReturnPurchaseInvoiceDetailViewModel>();
            return indexing.AddIndexing(hospconvert).Select(p => new SubReturnPurchaseInvoiceDetailViewModel
            {
                SubIndex = p.Index,
                SubGuid = p.Guid,
                SubProductName = p.ProductName,
                SubNum = p.Num.GetValueOrDefault(0).ToString("#,0.##", cultures),
                SubFreeNum = p.FreeNum.GetValueOrDefault(0).ToString("#,0.##", cultures),
                SubPrice = p.Price.GetValueOrDefault(0).ToString("#,0.##", cultures),
                SubDiscount = $"{p.CurrencyName} {p.Discount.GetValueOrDefault(0).ToString("#,0.##", cultures)}",
                SubCurrencyName = p.CurrencyName,
                SubTotalPrice = p.TotalPrice.GetValueOrDefault(0).ToString("#,0.##", cultures),
                SubTotal = $"{p.CurrencyName} {(p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0)).ToString("#,0.##", cultures)}",
                SubTotalAfterDiscount = $"{p.CurrencyName} {(p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0)).ToString("#,0.##", cultures)}",
                SubReasonTxt = p.ReasonTxt
            });
        }

        public ReturnPurchaseInvoiceDetailViewModel GetReturnPurchaseInvoiceDetailForEdit(Guid purchaseInvoiceDetailId)
        {
            try
            {
                ReturnPurchaseInvoiceDetail ReturnPurchaseInvoiceDetailgu = _unitOfWork.ReturnPurchaseInvoiceDetails.GetReturnPurchaseInvoiceDetailForEdit(purchaseInvoiceDetailId);
                return ConvertModel(ReturnPurchaseInvoiceDetailgu);
            }
            catch (Exception) { return null; }
        }


        // Begin Convert 
        public ReturnPurchaseInvoiceDetail ConvertModelWithOutGuid(ReturnPurchaseInvoiceDetailViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseInvoiceDetailViewModel, ReturnPurchaseInvoiceDetail>()
                .ForMember(a => a.Guid, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnPurchaseInvoiceDetailViewModel, ReturnPurchaseInvoiceDetail>(Users);
        }

        public ReturnPurchaseInvoiceDetailViewModel ConvertModel(ReturnPurchaseInvoiceDetail Users)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseInvoiceDetail, ReturnPurchaseInvoiceDetailViewModel>()
                .ForMember(a => a.ProductId, b => b.MapFrom(c => c.PurchaseInvoiceDetail.ProductId))
                .ForMember(a => a.ReasonTxt, b => b.MapFrom(c => c.Reason.Name))
                .ForMember(a => a.NumTxt, b => b.MapFrom(c => c.Num.GetValueOrDefault(1).ToString("0.##", cultures)))
                .ForMember(a => a.FreeNumTxt, b => b.MapFrom(c => c.FreeNum.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.PriceTxt, b => b.MapFrom(c => c.Price.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.DiscountTxt, b => b.MapFrom(c => c.Discount.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.PurchaseInvoiceDetailTxt, b => b.MapFrom(c => $"{c.PurchaseInvoiceDetail.Master.InvoiceNum} | {c.PurchaseInvoiceDetail.Master.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)} | {c.PurchaseInvoiceDetail.ExpireDate.Value.ToString("dd/MM/yyyy", cultures)} "))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnPurchaseInvoiceDetail, ReturnPurchaseInvoiceDetailViewModel>(Users);
        }

        public List<ReturnPurchaseInvoiceDetailViewModel> ConvertModelsLists(IEnumerable<ReturnPurchaseInvoiceDetail> purchaseInvoiceDetails)
        {
            List<ReturnPurchaseInvoiceDetailViewModel> purchaseInvoiceDetailDtoList = new List<ReturnPurchaseInvoiceDetailViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseInvoiceDetail, ReturnPurchaseInvoiceDetailViewModel>()
                .ForMember(a => a.ProductName, b => b.MapFrom(c => $"{c.PurchaseInvoiceDetail.Product.Name} | {c.PurchaseInvoiceDetail.Product.ProductType.Name} | {c.PurchaseInvoiceDetail.Product.Producer.Name}"))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                .ForMember(a => a.ReasonTxt, b => b.MapFrom(c => c.Reason.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDetailDtoList = mapper.Map<IEnumerable<ReturnPurchaseInvoiceDetail>, List<ReturnPurchaseInvoiceDetailViewModel>>(purchaseInvoiceDetails);
            return purchaseInvoiceDetailDtoList;
        }

        // End Convert
    }
}
