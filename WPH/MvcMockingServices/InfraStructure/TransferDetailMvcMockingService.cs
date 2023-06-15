using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.PurchaseInvoiceDetailSalePrice;
using WPH.Models.TransferDetail;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class TransferDetailMvcMockingService : ITransferDetailMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;
        public TransferDetailMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
        }

        public string RemoveTransferDetail(Guid transferDetailid)
        {
            try
            {
                var in_use = _unitOfWork.TransferDetails.CheckTransferDetailInUse(transferDetailid);
                if (in_use)
                    return "TransferInUse";

                TransferDetail transferDetail = _unitOfWork.TransferDetails.GetWithMasterByDetailId(transferDetailid);
                if (transferDetail.Master.ReceiverUserId != null)
                    return "TransferAccepted";

                TransferDetail Hos = _unitOfWork.TransferDetails.GetWithPricesById(transferDetailid);
                if (Hos.PurchaseInvoiceDetailId.HasValue)
                {
                    PurchaseInvoiceDetail pu = _unitOfWork.PurchaseInvoiceDetails.Get(Hos.PurchaseInvoiceDetailId.Value);
                    pu.RemainingNum += Hos.RemainingNum;
                }
                if (Hos.TransferDetailId.HasValue)
                {
                    TransferDetail pu = _unitOfWork.TransferDetails.Get(Hos.TransferDetailId.Value);
                    pu.RemainingNum += Hos.RemainingNum;
                }
                _unitOfWork.PurchaseInvoiceDetailSalePrice.RemoveRange(Hos.PurchaseInvoiceDetailSalePrices);
                _unitOfWork.TransferDetails.Remove(Hos);
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
            string controllerName = "/TransferDetail/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public string AddNewTransferDetailList(IEnumerable<TransferDetailViewModel> viewModels, Guid userId)
        {
            if (viewModels == null || !viewModels.Any())
                return "EmptyList";

            if (viewModels.Any(p => string.IsNullOrWhiteSpace(p.InvoiceType) || p.DetailId == Guid.Empty || p.MasterId == null))
                return "DataNotValid";

            var master_id = viewModels.FirstOrDefault().MasterId;
            if (viewModels.Any(p => p.MasterId != master_id))
                return "DataNotValid";

            var transfer = _unitOfWork.Transfers.Get(master_id.Value);

            if (transfer.ReceiverUserId != null)
                return "TransferAccepted";

            var Is_Parent = _unitOfWork.ClinicSections.CheckClinicSectionIsParent(transfer.SourceClinicSectionId.Value, transfer.DestinationClinicSectionId.Value);

            var create_date = DateTime.Now;

            var purchase_list = viewModels.Where(p => p.InvoiceType == "Purchase").Select(p => p.DetailId).ToList();
            List<PurchaseInvoiceDetail> purchases = _unitOfWork.PurchaseInvoiceDetails.GetWithPricesByMultipleIds(purchase_list).ToList();

            var transfer_list = viewModels.Where(p => p.InvoiceType == "Transfer").Select(p => p.DetailId).ToList();
            List<TransferDetail> transfers = _unitOfWork.TransferDetails.GetWithPricesByMultipleIds(transfer_list).ToList();

            List<TransferDetail> new_transfers = new List<TransferDetail>();

            new_transfers.AddRange(purchases.Select(p => new TransferDetail
            {
                MasterId = master_id,
                Consideration = "Multi Select Transfer",
                CreatedDate = create_date,
                CreatedUserId = userId,
                ExpireDate = p.ExpireDate,
                Num = p.RemainingNum,
                ProductId = p.ProductId,
                PurchaseInvoiceDetailId = p.Guid,
                RemainingNum = p.RemainingNum,
                DestinationProductId = (Is_Parent ? null : p.ProductId),
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                MiddleSellPrice = p.MiddleSellPrice,
                WholeSellPrice = p.WholeSellPrice,
                CurrencyId = p.CurrencyId,
                TransferDetailId = null,
                SourcePurchaseInvoiceDetailId = p.Guid,
                PurchaseInvoiceDetailSalePrices = p.PurchaseInvoiceDetailSalePrices.Select(x => new PurchaseInvoiceDetailSalePrice
                {
                    PurchaseInvoiceDetailId = null,
                    TypeId = x.TypeId,
                    CreateUserId = userId,
                    CreateDate = create_date,
                    CurrencyId = x.CurrencyId,
                    MoneyConvertId = x.MoneyConvertId
                }).ToList()
            }));

            new_transfers.AddRange(transfers.Select(p => new TransferDetail
            {
                MasterId = master_id,
                Consideration = "Multi Select Transfer",
                CreatedDate = create_date,
                CreatedUserId = userId,
                ExpireDate = p.ExpireDate,
                Num = p.RemainingNum,
                ProductId = p.ProductId,
                PurchaseInvoiceDetailId = null,
                RemainingNum = p.RemainingNum,
                DestinationProductId = (Is_Parent ? null : p.ProductId),
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                MiddleSellPrice = p.MiddleSellPrice,
                WholeSellPrice = p.WholeSellPrice,
                CurrencyId = p.CurrencyId,
                TransferDetailId = p.Guid,
                SourcePurchaseInvoiceDetailId = p.SourcePurchaseInvoiceDetailId,
                PurchaseInvoiceDetailSalePrices = p.PurchaseInvoiceDetailSalePrices.Select(x => new PurchaseInvoiceDetailSalePrice
                {
                    PurchaseInvoiceDetailId = null,
                    TypeId = x.TypeId,
                    CreateUserId = userId,
                    CreateDate = create_date,
                    CurrencyId = x.CurrencyId,
                    MoneyConvertId = x.MoneyConvertId
                }).ToList()
            }));

            _unitOfWork.TransferDetails.AddRange(new_transfers);
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

            _unitOfWork.Complete();
            return transfer.Guid.ToString();
        }

        public string AddNewTransferDetail(TransferDetailViewModel viewModel/*, Guid originalClinicSectionId*/)
        {
            if (string.IsNullOrWhiteSpace(viewModel.InvoiceType) || viewModel.DetailId == Guid.Empty || viewModel.Num.GetValueOrDefault(0) <= 0
                || viewModel.MasterId == null)
                return "DataNotValid";

            var transfer = _unitOfWork.Transfers.Get(viewModel.MasterId.Value);

            if (transfer.ReceiverUserId != null)
                return "TransferAccepted";

            viewModel.ClinicSectionId = transfer.SourceClinicSectionId;

            var Is_Parent = _unitOfWork.ClinicSections.CheckClinicSectionIsParent(transfer.SourceClinicSectionId.Value, transfer.DestinationClinicSectionId.Value);
            var create_date = DateTime.Now;

            TransferDetail new_transfer;
            if (viewModel.InvoiceType == "Purchase")
            {
                var detail = _unitOfWork.PurchaseInvoiceDetails.GetWithPricesById(viewModel.DetailId);

                if (viewModel.Num > detail.RemainingNum)
                    return "NotEnoughProductCount";

                new_transfer = new TransferDetail
                {
                    MasterId = viewModel.MasterId,
                    Consideration = viewModel.Consideration,
                    CreatedDate = create_date,
                    CreatedUserId = viewModel.CreatedUserId,
                    ExpireDate = detail.ExpireDate,
                    Num = viewModel.Num,
                    ProductId = detail.ProductId,
                    PurchaseInvoiceDetailId = detail.Guid,
                    RemainingNum = viewModel.Num,
                    DestinationProductId = (Is_Parent ? null : detail.ProductId),
                    PurchasePrice = detail.PurchasePrice,
                    SellingPrice = detail.SellingPrice,
                    MiddleSellPrice = detail.MiddleSellPrice,
                    WholeSellPrice = detail.WholeSellPrice,
                    CurrencyId = detail.CurrencyId,
                    TransferDetailId = null,
                    SourcePurchaseInvoiceDetailId = detail.Guid,
                    PurchaseInvoiceDetailSalePrices = detail.PurchaseInvoiceDetailSalePrices.Select(x => new PurchaseInvoiceDetailSalePrice
                    {
                        PurchaseInvoiceDetailId = null,
                        TypeId = x.TypeId,
                        CreateUserId = viewModel.CreatedUserId,
                        CreateDate = create_date,
                        CurrencyId = x.CurrencyId,
                        MoneyConvertId = x.MoneyConvertId
                    }).ToList()
                };

                detail.RemainingNum -= viewModel.Num;
                _unitOfWork.PurchaseInvoiceDetails.UpdateState(detail);
            }
            else
            {
                var detail = _unitOfWork.TransferDetails.GetWithPricesById(viewModel.DetailId);

                if (viewModel.Num > detail.RemainingNum)
                    return "NotEnoughProductCount";

                new_transfer = new TransferDetail
                {
                    MasterId = viewModel.MasterId,
                    Consideration = viewModel.Consideration,
                    CreatedDate = create_date,
                    CreatedUserId = viewModel.CreatedUserId,
                    ExpireDate = detail.ExpireDate,
                    Num = viewModel.Num,
                    ProductId = detail.ProductId,
                    PurchaseInvoiceDetailId = null,
                    RemainingNum = viewModel.Num,
                    DestinationProductId = (Is_Parent ? null : detail.ProductId),
                    PurchasePrice = detail.PurchasePrice,
                    SellingPrice = detail.SellingPrice,
                    MiddleSellPrice = detail.MiddleSellPrice,
                    WholeSellPrice = detail.WholeSellPrice,
                    CurrencyId = detail.CurrencyId,
                    TransferDetailId = detail.Guid,
                    SourcePurchaseInvoiceDetailId = detail.SourcePurchaseInvoiceDetailId,
                    PurchaseInvoiceDetailSalePrices = detail.PurchaseInvoiceDetailSalePrices.Select(x => new PurchaseInvoiceDetailSalePrice
                    {
                        PurchaseInvoiceDetailId = null,
                        TypeId = x.TypeId,
                        CreateUserId = viewModel.CreatedUserId,
                        CreateDate = create_date,
                        CurrencyId = x.CurrencyId,
                        MoneyConvertId = x.MoneyConvertId
                    }).ToList()
                };

                detail.RemainingNum -= viewModel.Num;
                _unitOfWork.TransferDetails.UpdateState(detail);
            }

            _unitOfWork.TransferDetails.Add(new_transfer);
            _unitOfWork.Complete();
            return new_transfer.Guid.ToString();

        }

        public string UpdateTransferDetail(TransferDetailViewModel newTransferDetail/*, Guid originalClinicSectionId*/)
        {
            if (newTransferDetail.Num.GetValueOrDefault(0) <= 0 || newTransferDetail.MasterId == null)
                return "DataNotValid";

            var in_use = _unitOfWork.TransferDetails.CheckTransferDetailInUse(newTransferDetail.Guid);
            if (in_use)
                return "TransferInUse";

            TransferDetail oldTransferDetail = _unitOfWork.TransferDetails.GetWithMasterByDetailId(newTransferDetail.Guid);
            if (oldTransferDetail.Master.ReceiverUserId != null)
                return "TransferAccepted";

            var diff = newTransferDetail.Num.Value - oldTransferDetail.Num.GetValueOrDefault(0);

            if (oldTransferDetail.PurchaseInvoiceDetailId.HasValue)
            {
                var detail = _unitOfWork.PurchaseInvoiceDetails.Get(oldTransferDetail.PurchaseInvoiceDetailId.Value);

                if (diff > detail.RemainingNum)
                    return "NotEnoughProductCount";

                detail.RemainingNum -= diff;

                _unitOfWork.PurchaseInvoiceDetails.UpdateState(detail);
            }
            else
            {
                var detail = _unitOfWork.TransferDetails.Get(oldTransferDetail.TransferDetailId.Value);

                if (diff > detail.RemainingNum)
                    return "NotEnoughProductCount";

                detail.RemainingNum -= diff;

                _unitOfWork.TransferDetails.UpdateState(detail);
            }

            oldTransferDetail.Num = oldTransferDetail.RemainingNum = newTransferDetail.Num;
            oldTransferDetail.ModifiedDate = DateTime.Now;
            oldTransferDetail.ModifiedUserId = newTransferDetail.ModifiedUserId;
            oldTransferDetail.Consideration = newTransferDetail.Consideration;

            _unitOfWork.TransferDetails.UpdateState(oldTransferDetail);
            _unitOfWork.Complete();

            return oldTransferDetail.Guid.ToString();
        }

        //public Transfer GetTransferDetailsBasedExpireDate(Product product, TransferDetailViewModel viewModel, Guid? exceptPIDId = null, Guid? exceptSTDId = null)
        //{
        //    Transfer result = new();

        //    var get = product.TransferDetailDestinationProducts.Select(s => new TransferDetail
        //    {
        //        Guid = s.Guid,
        //        PurchaseInvoiceDetailId = s.PurchaseInvoiceDetailId,
        //        ExpireDate = s.ExpireDate,
        //        Num = s.Num.GetValueOrDefault(0),
        //        SellingPrice = s.SellingPrice,
        //        PurchasePrice = s.PurchasePrice,
        //        TransferDetailId = s.TransferDetailId
        //    }).ToList();

        //    var send = product.TransferDetailProducts.GroupBy(g => new { g.TransferDetailId, g.PurchaseInvoiceDetailId, g.ExpireDate }).Select(s => new TransferDetail
        //    {
        //        PurchaseInvoiceDetailId = s.Key.PurchaseInvoiceDetailId,
        //        ExpireDate = s.Key.ExpireDate,
        //        Num = s.Sum(x => x.Num.GetValueOrDefault(0)),
        //        SellingPrice = s.FirstOrDefault().SellingPrice,
        //        PurchasePrice = s.FirstOrDefault().PurchasePrice,
        //        TransferDetailId = s.Key.TransferDetailId
        //    }).ToList();

        //    var buy = product.PurchaseInvoiceDetails.Select(s => new TransferDetail
        //    {
        //        Guid = s.Guid,
        //        PurchaseInvoiceDetailId = null,
        //        ExpireDate = s.ExpireDate,
        //        Num = s.Num.GetValueOrDefault(0) + s.FreeNum.GetValueOrDefault(0),
        //        SellingPrice = s.SellingPrice,
        //        PurchasePrice = s.PurchasePrice,
        //        TransferDetailId = null
        //    }).ToList();

        //    var total = buy.Sum(s => s.Num) - send.Sum(s => s.Num) + get.Sum(s => s.Num);

        //    if (viewModel.Num.GetValueOrDefault(0) > total)
        //    {
        //        result.Description = "NotEnoughProductCount";
        //        return result;
        //    }

        //    var get_send = (from g in get
        //                    join s in send
        //                    on g.Guid equals s.TransferDetailId into g_s
        //                    from j in g_s.DefaultIfEmpty()
        //                    select new TransferDetail
        //                    {
        //                        Guid = g.Guid,
        //                        PurchaseInvoiceDetailId = g.PurchaseInvoiceDetailId,
        //                        ExpireDate = g.ExpireDate,
        //                        Num = g.Num - (j?.Num ?? 0),
        //                        SellingPrice = g.SellingPrice,
        //                        PurchasePrice = g.PurchasePrice,
        //                        TransferDetailId = g.TransferDetailId
        //                    }).ToList();

        //    if (exceptPIDId != null)
        //        get_send = get_send.Where(p => p.Guid != exceptPIDId).ToList();

        //    var buy_send = (from b in buy
        //                    join s in send
        //                    on b.Guid equals s.PurchaseInvoiceDetailId into b_s
        //                    from j in b_s.DefaultIfEmpty()
        //                    select new TransferDetail
        //                    {
        //                        Guid = b.Guid,
        //                        PurchaseInvoiceDetailId = null,
        //                        ExpireDate = b.ExpireDate,
        //                        Num = b.Num - (j?.Num ?? 0),
        //                        SellingPrice = b.SellingPrice,
        //                        PurchasePrice = b.PurchasePrice,
        //                        TransferDetailId = null
        //                    }).ToList();

        //    if (exceptSTDId != null)
        //        buy_send = buy_send.Where(p => p.Guid != exceptSTDId).ToList();

        //    List<TransferDetail> all = new();

        //    all.AddRange(buy_send);
        //    all.AddRange(get_send);
        //    all = all.OrderBy(p => p.ExpireDate).ToList();

        //    //if (product.PurchaseInvoiceDetails != null && product.PurchaseInvoiceDetails.Any())
        //    //{
        //    //    var total = product.PurchaseInvoiceDetails.Sum(s => s.Num + s.FreeNum.GetValueOrDefault(0)) - send.Sum(s => s.Num) + get.Sum(s => s.Num);

        //    //    if (viewModel.Num.GetValueOrDefault(0) > total)
        //    //    {
        //    //        result.Description = "NotEnoughProductCount";
        //    //        return result;
        //    //    }

        //    //    var get_send = (from g in get
        //    //                    join s in send
        //    //                    on g.PurchaseInvoiceDetailId equals s.PurchaseInvoiceDetailId into g_s
        //    //                    from j in g_s.DefaultIfEmpty()
        //    //                    select new TransferDetail
        //    //                    {
        //    //                        PurchaseInvoiceDetailId = g.PurchaseInvoiceDetailId,
        //    //                        ExpireDate = g.ExpireDate,
        //    //                        Num = g.Num - (j?.Num ?? 0),
        //    //                        SellingPrice = g.SellingPrice,
        //    //                        PurchasePrice = g.PurchasePrice
        //    //                    }).ToList();

        //    //    all = (from pi in product.PurchaseInvoiceDetails
        //    //           join sg in get_send
        //    //           on pi.Guid equals sg.PurchaseInvoiceDetailId into pi_sg
        //    //           from j in pi_sg.DefaultIfEmpty()
        //    //           select new TransferDetail
        //    //           {
        //    //               PurchaseInvoiceDetailId = pi.Guid,
        //    //               ExpireDate = pi.ExpireDate,
        //    //               Num = pi.Num + pi.FreeNum.GetValueOrDefault(0) + (j?.Num ?? 0),
        //    //               SellingPrice = pi.SellingPrice,
        //    //               PurchasePrice = pi.PurchasePrice
        //    //           }).OrderBy(p => p.ExpireDate).ToList();

        //    //}
        //    //else
        //    //{
        //    //    var total = get.Sum(s => s.Num) - send.Sum(s => s.Num);

        //    //    if (viewModel.Num.GetValueOrDefault(0) > total)
        //    //    {
        //    //        result.Description = "NotEnoughProductCount";
        //    //        return result;
        //    //    }

        //    //    all = (from g in get
        //    //           join s in send
        //    //           on g.PurchaseInvoiceDetailId equals s.PurchaseInvoiceDetailId
        //    //           select new TransferDetail
        //    //           {
        //    //               PurchaseInvoiceDetailId = g.PurchaseInvoiceDetailId,
        //    //               ExpireDate = g.ExpireDate,
        //    //               Num = g.Num - s.Num,
        //    //               SellingPrice = g.SellingPrice,
        //    //               PurchasePrice = g.PurchasePrice
        //    //           }).OrderBy(p => p.ExpireDate).ToList();

        //    //}

        //    result.TransferDetails = new List<TransferDetail>();
        //    decimal num = viewModel.Num.GetValueOrDefault(0);
        //    decimal rem = num;
        //    foreach (var item in all)
        //    {
        //        if (!(item.Num.Value > 0))
        //            continue;

        //        rem -= item.Num.Value;
        //        if (rem > 0)
        //        {
        //            var transferDetail = new TransferDetail
        //            {
        //                MasterId = viewModel.MasterId,
        //                Consideration = viewModel.Consideration,
        //                ExpireDate = item.ExpireDate,
        //                Num = item.Num,
        //                ProductId = viewModel.ProductId,
        //                PurchaseInvoiceDetailId = (item.PurchaseInvoiceDetailId == null ? item.Guid : item.PurchaseInvoiceDetailId),
        //                DestinationProductId = (viewModel.Is_Parent ? null : viewModel.ProductId),
        //                PurchasePrice = item.PurchasePrice,
        //                SellingPrice = item.SellingPrice,
        //                CreatedDate = DateTime.Now,
        //                CreatedUserId = viewModel.CreatedUserId,
        //                TransferDetailId = (item.PurchaseInvoiceDetailId == null ? null : item.Guid)
        //            };

        //            result.TransferDetails.Add(transferDetail);
        //            num -= item.Num.Value;
        //        }
        //        else
        //        {
        //            var transferDetail = new TransferDetail
        //            {
        //                MasterId = viewModel.MasterId,
        //                Consideration = viewModel.Consideration,
        //                ExpireDate = item.ExpireDate,
        //                Num = num,
        //                ProductId = viewModel.ProductId,
        //                PurchaseInvoiceDetailId = (item.PurchaseInvoiceDetailId == null ? item.Guid : item.PurchaseInvoiceDetailId),
        //                DestinationProductId = (viewModel.Is_Parent ? null : viewModel.ProductId),
        //                PurchasePrice = item.PurchasePrice,
        //                SellingPrice = item.SellingPrice,
        //                CreatedDate = DateTime.Now,
        //                CreatedUserId = viewModel.CreatedUserId,
        //                TransferDetailId = (item.PurchaseInvoiceDetailId == null ? null : item.Guid)
        //            };

        //            result.TransferDetails.Add(transferDetail);
        //            break;
        //        }
        //    }


        //    return result;
        //}

        public IEnumerable<TransferDetailGridViewModel> GetAllTransferDetails(Guid transferId)
        {
            IEnumerable<TransferDetail> hosp = _unitOfWork.TransferDetails.GetAllWithChildByMasterId(transferId);
            List<TransferDetailGridViewModel> hospconvert = ConvertGridModels(hosp);
            Indexing<TransferDetailGridViewModel> indexing = new Indexing<TransferDetailGridViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<TransferDetailGridViewModel> GetUnreceivedTransferDetail(Guid transferId)
        {
            IEnumerable<TransferDetail> hosp = _unitOfWork.TransferDetails.GetUnreceivedTransferDetailByMasterId(transferId);
            List<TransferDetailGridViewModel> hospconvert = ConvertGridModels(hosp);
            Indexing<TransferDetailGridViewModel> indexing = new Indexing<TransferDetailGridViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public void ResetProductReceive(Guid transferId)
        {
            var producReceives = _unitOfWork.TransferDetails.Find(p => p.MasterId == transferId);

            foreach (var item in producReceives)
            {
                item.DestinationProductId = null;

                _unitOfWork.TransferDetails.UpdateState(item);
            }

            _unitOfWork.Complete();
        }

        public TransferDetailViewModel GetTransferDetailForUpdate(Guid transferDetailId)
        {
            try
            {
                TransferDetail TransferDetailgu = _unitOfWork.TransferDetails.GetTransferDetailForUpdate(transferDetailId);
                return ConvertUpdateModel(TransferDetailgu);
            }
            catch { return null; }
        }

        public string ConfirmAllProductRecive(Guid transferId, Guid userId)
        {
            var exists = _unitOfWork.TransferDetails.CheckConfirmAllProductRecive(transferId);
            if (exists)
                return "ProductNotConfirmed";

            var transfer = _unitOfWork.Transfers.Get(transferId);

            transfer.ReceiverUserId = userId;
            transfer.ReceiverDate = DateTime.Now;

            _unitOfWork.Transfers.UpdateState(transfer);
            _unitOfWork.Complete();
            return "";
        }

        public TransferDetailViewModel GetSourceProducName(Guid transferDateilId)
        {
            var product = _unitOfWork.TransferDetails.GetWithSourceProduct(transferDateilId);
            return ConvertModel(product);
        }

        public string AddProductReceive(TransferDetailViewModel viewModel, Guid clinicSectionId)
        {
            var transferDetail = _unitOfWork.TransferDetails.Get(viewModel.Guid);

            transferDetail.ModifiedUserId = viewModel.ModifiedUserId;
            transferDetail.ModifiedDate = DateTime.Now;

            if (viewModel.DestinationProductId == null)
            {
                var sourceProduct = _unitOfWork.Products.GetWithChild(transferDetail.ProductId.Value);

                Product destinationProduct = _unitOfWork.Products.GetProductByNameAndProducerAndType(clinicSectionId, sourceProduct.Name, sourceProduct.Producer.Name, sourceProduct.MaterialTypeId, sourceProduct.ProductType.Name);

                if (destinationProduct == null)
                {
                    destinationProduct = new Product
                    {
                        CreatedDate = DateTime.Now,
                        CreateUserId = viewModel.ModifiedUserId.Value,
                        Name = sourceProduct.Name,
                        MaterialTypeId = sourceProduct.MaterialTypeId,
                        Barcode = sourceProduct.Barcode,
                        Description = sourceProduct.Description,
                        Code = sourceProduct.Code,
                        ScientificName = sourceProduct.ScientificName,
                        ClinicSectionId = clinicSectionId
                    };

                    if (sourceProduct.ProducerId != null)
                    {
                        var destinationProducer = _unitOfWork.BaseInfos.GetSingle(p => p.ClinicSectionId == clinicSectionId && p.Name == sourceProduct.Producer.Name && p.TypeId == sourceProduct.Producer.TypeId);

                        destinationProduct.ProducerId = destinationProducer?.Guid;
                        if (destinationProducer == null)
                        {
                            destinationProduct.Producer = new BaseInfo
                            {
                                Name = sourceProduct.Producer.Name,
                                Priority = sourceProduct.Producer.Priority,
                                Description = sourceProduct.Producer.Description,
                                TypeId = sourceProduct.Producer.TypeId,
                                ClinicSectionId = clinicSectionId
                            };

                        }
                    }

                    if (sourceProduct.ProductTypeId != null)
                    {
                        var destinationProductType = _unitOfWork.BaseInfos.GetSingle(p => p.ClinicSectionId == clinicSectionId && p.Name == sourceProduct.ProductType.Name && p.TypeId == sourceProduct.ProductType.TypeId);

                        destinationProduct.ProductTypeId = destinationProductType?.Guid;
                        if (destinationProductType == null)
                        {
                            destinationProduct.ProductType = new BaseInfo
                            {
                                Name = sourceProduct.ProductType.Name,
                                Priority = sourceProduct.ProductType.Priority,
                                Description = sourceProduct.ProductType.Description,
                                TypeId = sourceProduct.ProductType.TypeId,
                                ClinicSectionId = clinicSectionId
                            };

                        }
                    }

                    if (sourceProduct.UnitId != null)
                    {
                        var destinationUnit = _unitOfWork.BaseInfos.GetSingle(p => p.ClinicSectionId == clinicSectionId && p.Name == sourceProduct.Unit.Name && p.TypeId == sourceProduct.Unit.TypeId);

                        destinationProduct.UnitId = destinationUnit?.Guid;
                        if (destinationUnit == null)
                        {
                            destinationProduct.Unit = new BaseInfo
                            {
                                Name = sourceProduct.Unit.Name,
                                Priority = sourceProduct.Unit.Priority,
                                Description = sourceProduct.Unit.Description,
                                TypeId = sourceProduct.Unit.TypeId,
                                ClinicSectionId = clinicSectionId
                            };

                        }
                    }

                    _unitOfWork.Products.Add(destinationProduct);

                    transferDetail.DestinationProduct = destinationProduct;
                }
                else
                {
                    transferDetail.DestinationProductId = destinationProduct.Guid;
                }
            }
            else
            {
                var product = _unitOfWork.Products.GetSingle(p => p.Guid == viewModel.DestinationProductId && p.ClinicSectionId == clinicSectionId);
                if (product == null)
                    return "InvalidProduct";

                transferDetail.DestinationProductId = viewModel.DestinationProductId;
            }

            _unitOfWork.TransferDetails.UpdateState(transferDetail);
            _unitOfWork.Complete();

            return transferDetail.Guid.ToString();
        }

        public ParentDetailSalePriceViewModel GetForNewSalePrice(Guid transferDetailId)
        {
            try
            {
                var detail = _unitOfWork.TransferDetails.GetForNewSalePrice(transferDetailId);
                return ConvertForSalePriceModel(detail);
            }
            catch (Exception) { return null; }
        }

        // Begin Convert 
        
        public ParentDetailSalePriceViewModel ConvertForSalePriceModel(TransferDetail detail)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TransferDetail, ParentDetailSalePriceViewModel>()
                .ForMember(a => a.TransferDetailId, b => b.MapFrom(c => c.Guid))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.PurchaseCurrency.Name))
                .ForMember(a => a.ProductName, b => b.MapFrom(c => $"{c.DestinationProduct.Name} | {c.DestinationProduct.ProductType.Name} | {c.DestinationProduct.Producer.Name}"))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<TransferDetail, ParentDetailSalePriceViewModel>(detail);
        }

        public TransferDetailViewModel ConvertModel(TransferDetail Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TransferDetail, TransferDetailViewModel>()
                //.ForMember(a => a.TransferDetailTypeName, b => b.MapFrom(c => c.TransferDetailType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<TransferDetail, TransferDetailViewModel>(Users);
        }

        public TransferDetailViewModel ConvertUpdateModel(TransferDetail Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TransferDetail, TransferDetailViewModel>()
                .ForMember(a => a.ProductName, b => b.MapFrom(c => $"{c.Product.Name} | {c.Product.ProductType.Name} | {c.Product.Producer.Name}"))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<TransferDetail, TransferDetailViewModel>(Users);
        }

        public List<TransferDetailGridViewModel> ConvertGridModels(IEnumerable<TransferDetail> transferDetails)
        {
            List<TransferDetailGridViewModel> transferDetailDtoList = new List<TransferDetailGridViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TransferDetail, TransferDetailGridViewModel>()
                .ForMember(a => a.ProductName, b => b.MapFrom(c => $"{c.Product.Name} | {c.Product.ProductType.Name} | {c.Product.Producer.Name}"))
                .ForMember(a => a.DestinationProductName, b => b.MapFrom(c => $"{c.DestinationProduct.Name} | {c.DestinationProduct.ProductType.Name} | {c.DestinationProduct.Producer.Name}"))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.PurchaseCurrency.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            transferDetailDtoList = mapper.Map<IEnumerable<TransferDetail>, List<TransferDetailGridViewModel>>(transferDetails);
            return transferDetailDtoList;
        }


        // End Convert
    }
}




//public IEnumerable<TransferDetail> TransferProduct(TransferDetailViewModel viewModel)
//{
//    var AllUsableProductList = _idunit.product.GetAllUsableProductList(viewModel.ProductId.Value, viewModel.ClinicSectionId.Value);

//    List<TransferDetail> all = new();

//    var totalNum = viewModel.Num;

//    if (totalNum > AllUsableProductList.Sum(a => a.RemainingNum))
//    {
//        Exception e = new Exception("NotEnoughProductCount");
//        throw e;
//    }


//    foreach (var usableProduct in AllUsableProductList)
//    {
//        if (totalNum > 0)
//        {
//            if (totalNum >= usableProduct.RemainingNum)
//            {
//                all.Add(new TransferDetail
//                {
//                    MasterId = viewModel.MasterId,
//                    Consideration = viewModel.Consideration,
//                    CreatedDate = DateTime.Now,
//                    CreatedUserId = viewModel.CreatedUserId,
//                    ExpireDate = usableProduct.ExpireDate,
//                    Num = usableProduct.RemainingNum,
//                    ProductId = viewModel.ProductId,
//                    PurchaseInvoiceDetailId = (usableProduct.PurchaseInvoiceDetailId.HasValue) ? usableProduct.PurchaseInvoiceDetailId : null,
//                    RemainingNum = usableProduct.RemainingNum,
//                    DestinationProductId = (viewModel.Is_Parent ? null : viewModel.ProductId),
//                    PurchasePrice = usableProduct.PurchasePrice,
//                    SellingPrice = usableProduct.SellingPrice,
//                    TransferDetailId = (usableProduct.TransferDetailId.HasValue) ? usableProduct.TransferDetailId : null,
//                    SourcePurchaseInvoiceDetailId = usableProduct.SourcePurchaseInvoiceDetailId
//                });

//                if (usableProduct.TransferDetailId.HasValue)
//                {
//                    TransferDetail td = _unitOfWork.TransferDetails.Get(usableProduct.TransferDetailId.Value);
//                    td.RemainingNum = 0;
//                }
//                else
//                {
//                    PurchaseInvoiceDetail pid = _unitOfWork.PurchaseInvoiceDetails.Get(usableProduct.PurchaseInvoiceDetailId.Value);
//                    pid.RemainingNum = 0;
//                }
//                totalNum -= usableProduct.RemainingNum;

//            }
//            else
//            {
//                all.Add(new TransferDetail
//                {
//                    MasterId = viewModel.MasterId,
//                    Consideration = viewModel.Consideration,
//                    CreatedDate = DateTime.Now,
//                    CreatedUserId = viewModel.CreatedUserId,
//                    ExpireDate = usableProduct.ExpireDate,
//                    Num = totalNum,
//                    ProductId = viewModel.ProductId,
//                    PurchaseInvoiceDetailId = (usableProduct.PurchaseInvoiceDetailId.HasValue) ? usableProduct.PurchaseInvoiceDetailId : null,
//                    RemainingNum = totalNum,
//                    DestinationProductId = (viewModel.Is_Parent ? null : viewModel.ProductId),
//                    PurchasePrice = usableProduct.PurchasePrice,
//                    SellingPrice = usableProduct.SellingPrice,
//                    TransferDetailId = (usableProduct.TransferDetailId.HasValue) ? usableProduct.TransferDetailId : null,
//                    SourcePurchaseInvoiceDetailId = usableProduct.SourcePurchaseInvoiceDetailId
//                });

//                if (usableProduct.TransferDetailId.HasValue)
//                {
//                    TransferDetail td = _unitOfWork.TransferDetails.Get(usableProduct.TransferDetailId.Value);
//                    td.RemainingNum -= totalNum;
//                }
//                else
//                {
//                    PurchaseInvoiceDetail pid = _unitOfWork.PurchaseInvoiceDetails.Get(usableProduct.PurchaseInvoiceDetailId.Value);
//                    pid.RemainingNum -= totalNum;
//                }

//                totalNum = 0;
//            }
//        }

//    }

//    return all;
//}
