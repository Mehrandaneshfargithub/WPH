using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.ReturnSaleInvoiceDetail;
using WPH.Models.SaleInvoiceDetail;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class SaleInvoiceDetailMvcMockingService : ISaleInvoiceDetailMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _diunit;
        public SaleInvoiceDetailMvcMockingService(IUnitOfWork unitOfWork, IDIUnit dunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _diunit = dunit;
        }

        public string RemoveSaleInvoiceDetail(IEnumerable<Guid> saleInvoiceDetailid)
        {
            try
            {
                IEnumerable<SaleInvoiceDetail> HosList = _unitOfWork.SaleInvoiceDetails.Find(a=> saleInvoiceDetailid.Contains(a.Guid));

                foreach(var Hos in HosList)
                {
                    var can_change = _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(Hos.SaleInvoiceId.Value);
                    if (can_change)
                        return "InvoiceInUse";

                    var currencyExist = _unitOfWork.SaleInvoices.CheckForCurrency(Hos.SaleInvoiceId.Value, Hos.CurrencyId);
                    if (currencyExist == null)
                        return "DataNotValid";

                    //var total_purchase = currencyExist.SaleInvoiceDetails.Where(p => p.Guid != saleInvoiceDetailid).Sum(p => p.Num.GetValueOrDefault(0) * p.SalePrice.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
                    //var total_discount = currencyExist.SaleInvoiceDiscounts.Sum(p => p.Amount.GetValueOrDefault(0));
                    //var after_discount = total_purchase - total_discount;
                    //if (after_discount < 0)
                    //    return "DiscountIsGreaterThanTheAmount";

                    var total = Hos.Num.GetValueOrDefault(0) + Hos.FreeNum.GetValueOrDefault(0);
                    if (Hos.PurchaseInvoiceDetailId != null)
                    {
                        var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(Hos.PurchaseInvoiceDetailId ?? Guid.Empty);

                        purchase.RemainingNum += total;
                        _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                    }
                    else
                    {
                        var transfer = _unitOfWork.TransferDetails.Get(Hos.TransferDetailId ?? Guid.Empty);

                        transfer.RemainingNum += total;
                        _unitOfWork.TransferDetails.UpdateState(transfer);
                    }
                }

                

                _unitOfWork.SaleInvoiceDetails.RemoveRange(HosList);
                //_unitOfWork.Complete();
                _diunit.saleInvoice.UpdateTotalPrice(HosList, null, "remove","");
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
            string controllerName = "/SaleInvoiceDetail/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewSaleInvoiceDetail(SaleInvoiceDetailViewModel viewModel)
        {
            //if (viewModel.ProductId == null || viewModel.Num.GetValueOrDefault(0) <= 0 ||
            //    viewModel.SalePrice.GetValueOrDefault(0) <= 0 || viewModel.SellingPrice.GetValueOrDefault(0) <= 0)
            //    return "DataNotValid";

            //if (!DateTime.TryParseExact(viewModel.ExpireDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime expireDate))
            //    return "DateNotValid";

            var can_change = _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(viewModel.SaleInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            SaleInvoiceDetail saleInvoiceDetail = Common.ConvertModels<SaleInvoiceDetail, SaleInvoiceDetailViewModel>.convertModels(viewModel);
            saleInvoiceDetail.Num = viewModel.Num.GetValueOrDefault(0);
            saleInvoiceDetail.FreeNum = viewModel.FreeNum.GetValueOrDefault(0);

            if ((saleInvoiceDetail.Num * saleInvoiceDetail.SalePrice) - saleInvoiceDetail.Discount < 0)
                return "DiscountIsGreaterThanTheAmount";

            var total = saleInvoiceDetail.Num + saleInvoiceDetail.FreeNum;
            if (total <= 0)
                return "DataNotValid";
            saleInvoiceDetail.RemainingNum = total;
            List<SaleInvoiceDetail> allSale = new List<SaleInvoiceDetail>();

            List<SaleInvoiceDetail> all = new List<SaleInvoiceDetail>();

            if (viewModel.CurrentStock || !viewModel.NearestExpire)
            {

                if (saleInvoiceDetail.PurchaseInvoiceDetailId != null)
                {
                    var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(saleInvoiceDetail.PurchaseInvoiceDetailId ?? Guid.Empty);
                    if (total > purchase.RemainingNum)
                    {
                        return "stocknotenough";
                    }
                    else
                    {
                        purchase.RemainingNum -= total;
                        _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                    }

                }
                else
                {
                    var transfer = _unitOfWork.TransferDetails.Get(saleInvoiceDetail.TransferDetailId ?? Guid.Empty);
                    if (total > transfer.RemainingNum)
                    {
                        return "stocknotenough";
                    }
                    else
                    {
                        transfer.RemainingNum -= total;
                        _unitOfWork.TransferDetails.UpdateState(transfer);
                    }
                }
                _unitOfWork.SaleInvoiceDetails.Add(saleInvoiceDetail);
                all.Add(saleInvoiceDetail);
            }
            else
            {

                var allPurchase = _unitOfWork.PurchaseInvoiceDetails.GetAllProductExpireList(viewModel.ProductId ?? Guid.Empty).OrderBy(a=>a.ExpireDate);

                //List<SaleInvoiceDetail> allSale = new List<SaleInvoiceDetail>();

                List<PurchaseOrTransferProductDetail> allupdates = new List<PurchaseOrTransferProductDetail>();

                var totalNum = viewModel.Num;
                var totalFreeNum = viewModel.FreeNum;

                foreach (var purchase in allPurchase)
                {
                    if (totalNum + totalFreeNum <= 0)
                        break;
                    if(totalNum > purchase.Stock)
                    {

                        if (purchase.SaleType == "PurchaseInvoiceDetail")
                        {
                            allSale.Add(new SaleInvoiceDetail
                            {
                                Consideration = viewModel.Consideration,
                                CurrencyId = viewModel.CurrencyId,
                                Discount = viewModel.Discount,
                                FreeNum = 0,
                                MoneyConvertId = viewModel.MoneyConvertId,
                                Num = purchase.Stock,
                                ProductId = viewModel.ProductId,
                                PurchaseInvoiceDetailId = purchase.Guid,
                                RemainingNum = purchase.Stock,
                                SaleInvoiceId = viewModel.SaleInvoiceId,
                                SalePrice = viewModel.SalePrice,
                                CreatedDate = viewModel.CreatedDate,
                                CreatedUserId = viewModel.CreatedUserId,
                                ModifiedDate = viewModel.ModifiedDate,
                                ModifiedUserId = viewModel.ModifiedUserId
                            });

                            allupdates.Add(new PurchaseOrTransferProductDetail
                            {
                                Guid = purchase.Guid,
                                Stock = 0,
                                SaleType = "PurchaseInvoiceDetail"
                            });
                        }
                        else
                        {
                            allSale.Add(new SaleInvoiceDetail
                            {
                                Consideration = viewModel.Consideration,
                                CurrencyId = viewModel.CurrencyId,
                                Discount = viewModel.Discount,
                                FreeNum = 0,
                                MoneyConvertId = viewModel.MoneyConvertId,
                                Num = purchase.Stock,
                                ProductId = viewModel.ProductId,
                                TransferDetailId = purchase.Guid,
                                RemainingNum = purchase.Stock,
                                SaleInvoiceId = viewModel.SaleInvoiceId,
                                SalePrice = viewModel.SalePrice,
                                CreatedDate = viewModel.CreatedDate,
                                CreatedUserId = viewModel.CreatedUserId,
                                ModifiedDate = viewModel.ModifiedDate,
                                ModifiedUserId = viewModel.ModifiedUserId
                            });

                            allupdates.Add(new PurchaseOrTransferProductDetail
                            {
                                Guid = purchase.Guid,
                                Stock = 0,
                                SaleType = "TransferDetail"
                            });
                        }

                        totalNum -= purchase.Stock;

                    }
                    else
                    {
                        if (purchase.SaleType == "PurchaseInvoiceDetail")
                        {

                            SaleInvoiceDetail newSale = new SaleInvoiceDetail() 
                            {
                                Consideration = viewModel.Consideration,
                                CurrencyId = viewModel.CurrencyId,
                                Discount = viewModel.Discount,
                                FreeNum = 0,
                                MoneyConvertId = viewModel.MoneyConvertId,
                                Num = totalNum,
                                ProductId = viewModel.ProductId,
                                PurchaseInvoiceDetailId = purchase.Guid,
                                RemainingNum = totalNum,
                                SaleInvoiceId = viewModel.SaleInvoiceId,
                                SalePrice = viewModel.SalePrice,
                                CreatedDate = viewModel.CreatedDate,
                                CreatedUserId = viewModel.CreatedUserId,
                                ModifiedDate = viewModel.ModifiedDate,
                                ModifiedUserId = viewModel.ModifiedUserId
                            };

                            
                            purchase.Stock -= totalNum.GetValueOrDefault();
                            totalNum = 0;

                            if (totalFreeNum > purchase.Stock)
                            {
                                newSale.FreeNum = purchase.Stock;
                                newSale.RemainingNum += purchase.Stock;
                                totalFreeNum -= purchase.Stock;
                                purchase.Stock = 0;
                            }
                            else
                            {
                                newSale.FreeNum = totalFreeNum;
                                newSale.RemainingNum += totalFreeNum;
                                purchase.Stock -= totalFreeNum.GetValueOrDefault();
                                totalFreeNum = 0;
                            }

                            allSale.Add(newSale);

                            allupdates.Add(new PurchaseOrTransferProductDetail
                            {
                                Guid = purchase.Guid,
                                Stock = purchase.Stock,
                                SaleType = "PurchaseInvoiceDetail"
                            });
                        }
                        else
                        {

                            SaleInvoiceDetail newSale = new SaleInvoiceDetail()
                            {
                                Consideration = viewModel.Consideration,
                                CurrencyId = viewModel.CurrencyId,
                                Discount = viewModel.Discount,
                                FreeNum = 0,
                                MoneyConvertId = viewModel.MoneyConvertId,
                                Num = totalNum,
                                ProductId = viewModel.ProductId,
                                TransferDetailId = purchase.Guid,
                                RemainingNum = totalNum + viewModel.FreeNum,
                                SaleInvoiceId = viewModel.SaleInvoiceId,
                                SalePrice = viewModel.SalePrice,
                                CreatedDate = viewModel.CreatedDate,
                                CreatedUserId = viewModel.CreatedUserId,
                                ModifiedDate = viewModel.ModifiedDate,
                                ModifiedUserId = viewModel.ModifiedUserId
                            };

                            purchase.Stock -= totalNum.GetValueOrDefault();

                            totalNum = 0;

                            if (totalFreeNum > purchase.Stock)
                            {
                                newSale.FreeNum = purchase.Stock;
                                newSale.RemainingNum += purchase.Stock;
                                totalFreeNum -= purchase.Stock;
                                purchase.Stock = 0;
                            }
                            else
                            {
                                newSale.FreeNum = totalFreeNum;
                                newSale.RemainingNum += totalFreeNum;
                                purchase.Stock -= totalFreeNum.GetValueOrDefault();
                                totalFreeNum = 0;
                            }

                            allSale.Add(newSale);

                            allupdates.Add(new PurchaseOrTransferProductDetail
                            {
                                Guid = purchase.Guid,
                                Stock = purchase.Stock,
                                SaleType = "TransferDetail"
                            });
                        }
                    }
                    
                }

                _unitOfWork.SaleInvoiceDetails.AddRange(allSale);
                all.AddRange(allSale);
                _unitOfWork.SaleInvoiceDetails.UpdatePurchaseAndTransferStock(allupdates);
            }

            

            //saleInvoiceDetail.CreatedDate = DateTime.Now;
            //saleInvoiceDetail.ExpireDate = expireDate;

            

            
            
            //_unitOfWork.Complete();
            return _diunit.saleInvoice.UpdateTotalPrice(all, null,"add", viewModel.CurrencyName);
            //return saleInvoiceDetail.Guid.ToString();
        }


        public string UpdateSaleInvoiceDetail(SaleInvoiceDetailViewModel viewModel)
        {
            //if (viewModel.ProductId == null || viewModel.Num.GetValueOrDefault(0) <= 0 ||
            //    viewModel.SalePrice.GetValueOrDefault(0) <= 0 || viewModel.SellingPrice.GetValueOrDefault(0) <= 0)
            //    return "DataNotValid";

            //if (!DateTime.TryParseExact(viewModel.ExpireDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime expireDate))
            //    return "DateNotValid";

            var can_change = _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(viewModel.SaleInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            SaleInvoiceDetail saleInvoiceDetail = Common.ConvertModels<SaleInvoiceDetail, SaleInvoiceDetailViewModel>.convertModels(viewModel);
            saleInvoiceDetail.Num = viewModel.Num.GetValueOrDefault(0);
            saleInvoiceDetail.FreeNum = viewModel.FreeNum.GetValueOrDefault(0);

            if ((saleInvoiceDetail.Num * saleInvoiceDetail.SalePrice) - saleInvoiceDetail.Discount < 0)
                return "DiscountIsGreaterThanTheAmount";

            var total = saleInvoiceDetail.Num + saleInvoiceDetail.FreeNum;
            if (total <= 0)
                return "DataNotValid";

            //saleInvoiceDetail.ModifiedDate = DateTime.Now;
            //saleInvoiceDetail.ExpireDate = expireDate;
            var oldSale = _unitOfWork.SaleInvoiceDetails.Get(viewModel.Guid);
            if (viewModel.ChangeNum)
            {
                
                var old_total = oldSale.Num + oldSale.FreeNum;
                var ret = old_total - oldSale.RemainingNum.GetValueOrDefault(0);
                if (total < ret)
                    return "Stock not enough";
                saleInvoiceDetail.RemainingNum = total - ret;

                var new_total = total - old_total;

                if (saleInvoiceDetail.PurchaseInvoiceDetailId != null)
                {
                    var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(saleInvoiceDetail.PurchaseInvoiceDetailId ?? Guid.Empty);
                    if (new_total > purchase.RemainingNum)
                    {
                        return "Stock not enough";
                    }
                    else
                    {
                        purchase.RemainingNum -= new_total;
                        //_unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                    }
                }
                else
                {
                    var transfer = _unitOfWork.TransferDetails.Get(saleInvoiceDetail.TransferDetailId ?? Guid.Empty);
                    if (new_total > transfer.RemainingNum)
                    {
                        return "Stock not enough";
                    }
                    else
                    {
                        transfer.RemainingNum -= new_total;
                        //_unitOfWork.TransferDetails.UpdateState(purchase);
                    }
                }

                
            }
            else
            {
                saleInvoiceDetail.RemainingNum = oldSale.RemainingNum;
            }


            saleInvoiceDetail.CreatedDate = oldSale.CreatedDate;
            saleInvoiceDetail.CreatedUserId = oldSale.CreatedUserId;

            _unitOfWork.SaleInvoiceDetails.Detach(oldSale);
            //oldSale = saleInvoiceDetail;
            _unitOfWork.SaleInvoiceDetails.UpdateState(saleInvoiceDetail);
            //_unitOfWork.Complete();
            List<SaleInvoiceDetail> all = new List<SaleInvoiceDetail>();
            all.Add(saleInvoiceDetail);
            return _diunit.saleInvoice.UpdateTotalPrice(all, null,"update", viewModel.CurrencyName);
            //return saleInvoiceDetail.Guid.ToString();
        }


        public IEnumerable<SaleInvoiceDetailViewModel> GetAllSaleInvoiceDetails(Guid saleInvoiceId)
        {
            try
            {
                IEnumerable<SaleInvoiceDetail> hosp = _unitOfWork.SaleInvoiceDetails.GetAllSaleInvoiceDetailByMasterId(saleInvoiceId).GroupBy(a => new { a.ProductId, a.CurrencyId}).Select(a =>
                {
                    a.FirstOrDefault().ChildrenGuids = string.Join(',', a.Select(s => s.Guid));
                    a.FirstOrDefault().ChildrenCount = a.Count();
                    a.FirstOrDefault().Num = a.Sum(s => s.Num.GetValueOrDefault(0));
                    a.FirstOrDefault().FreeNum = a.Sum(s => s.FreeNum.GetValueOrDefault(0));
                    a.FirstOrDefault().Discount = a.Sum(s => s.Discount.GetValueOrDefault(0));
                    return a.FirstOrDefault();

                });
                List<SaleInvoiceDetailViewModel> hospconvert = ConvertModelsLists(hosp);
                
                Indexing<SaleInvoiceDetailViewModel> indexing = new Indexing<SaleInvoiceDetailViewModel>();
                return indexing.AddIndexing(hospconvert);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public IEnumerable<SaleInvoiceDetailViewModel> GetAllDetail(IEnumerable<Guid> saleInvoiceDetailIds)
        {
            try
            {
                IEnumerable<SaleInvoiceDetail> hosp = _unitOfWork.SaleInvoiceDetails.GetAllDetail(saleInvoiceDetailIds);
                List<SaleInvoiceDetailViewModel> hospconvert = ConvertModelsLists(hosp);

                Indexing<SaleInvoiceDetailViewModel> indexing = new Indexing<SaleInvoiceDetailViewModel>();
                return indexing.AddIndexing(hospconvert);
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public SaleInvoiceDetailViewModel GetSaleInvoiceDetail(Guid saleInvoiceDetailId)
        {
            try
            {
                SaleInvoiceDetail SaleInvoiceDetailgu = _unitOfWork.SaleInvoiceDetails.Get(saleInvoiceDetailId);
                return ConvertModel(SaleInvoiceDetailgu);
            }
            catch { return null; }
        }

        public IEnumerable<ReturnSaleInvoiceDetailSelectViewModel> GetDetailsForReturn(Guid masterId, Guid productId, Guid clinicSectionId, bool like)
        {
            var result = _unitOfWork.SaleInvoiceDetails.GetDetailsForReturn(productId, masterId, clinicSectionId, like);
            List<ReturnSaleInvoiceDetailSelectViewModel> hospconvert = ConvertSelectModelsLists(result);
            Indexing<ReturnSaleInvoiceDetailSelectViewModel> indexing = new Indexing<ReturnSaleInvoiceDetailSelectViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        // Begin Convert 
        public SaleInvoiceDetailViewModel ConvertModel(SaleInvoiceDetail Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaleInvoiceDetail, SaleInvoiceDetailViewModel>()
                //.ForMember(a => a.SaleInvoiceDetailTypeName, b => b.MapFrom(c => c.SaleInvoiceDetailType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<SaleInvoiceDetail, SaleInvoiceDetailViewModel>(Users);
        }

        public List<SaleInvoiceDetailViewModel> ConvertModelsLists(IEnumerable<SaleInvoiceDetail> saleInvoiceDetails)
        {
            List<SaleInvoiceDetailViewModel> saleInvoiceDetailDtoList = new List<SaleInvoiceDetailViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaleInvoiceDetail, SaleInvoiceDetailViewModel>()

               .ForMember(a => a.ProductName, b => b.MapFrom(c => c.Product.Name))
               .ForMember(a => a.PurchasePrice, b => b.MapFrom(c => c.PurchaseInvoiceDetail != null ? (c.PurchaseInvoiceDetail.PurchasePrice.GetValueOrDefault(0).ToString("#,###.##") + " " + c.PurchaseInvoiceDetail.Currency.Name) : (c.TransferDetail.PurchasePrice.GetValueOrDefault(0).ToString("#,###.##") + " " + c.TransferDetail.PurchaseCurrency.Name)))
               .ForMember(a => a.PurchasePriceNumeric, b => b.MapFrom(c => c.PurchaseInvoiceDetail != null ? (c.PurchaseInvoiceDetail.PurchasePrice.GetValueOrDefault(0)) : (c.TransferDetail.PurchasePrice.GetValueOrDefault(0))))
               .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
               .ForMember(a => a.CurrencyNameId, b => b.MapFrom(c => c.CurrencyId))
               .ForMember(a => a.FirstCurrencyId, b => b.MapFrom(c => c.CurrencyId))
               .ForMember(a => a.OldSalePrice, b => b.MapFrom(c => c.SalePrice.Value.ToString("#,###.##") + c.Currency.Name))
               .ForMember(a => a.FirstPrice, b => b.MapFrom(c => c.SalePrice))
               //.ForMember(a => a.OldSalePrice, b => b.MapFrom(c => c.PurchaseInvoiceDetail != null ?( (c.SaleInvoice.SalePriceType.Name.ToLower() == "retailprice")? c.PurchaseInvoiceDetail.SellingPrice : ((c.SaleInvoice.SalePriceType.Name.ToLower() == "middelprice")? c.PurchaseInvoiceDetail.MiddleSellPrice : c.PurchaseInvoiceDetail.WholeSellPrice) ):((c.SaleInvoice.SalePriceType.Name.ToLower() == "retailprice") ? c.TransferDetail.SellingPrice : ((c.SaleInvoice.SalePriceType.Name.ToLower() == "middelprice") ? c.TransferDetail.MiddleSellPrice : c.TransferDetail.WholeSellPrice))))
               .ForMember(a => a.ExpireDate, b => b.MapFrom(c => c.PurchaseInvoiceDetail != null ? (c.PurchaseInvoiceDetail.ExpireDate.GetValueOrDefault().ToString("dd/MM/yyyy", new CultureInfo("en-US"))) : (c.TransferDetail.ExpireDate.GetValueOrDefault().ToString("dd/MM/yyyy", new CultureInfo("en-US")))))
               .ForMember(a => a.InvoiceType, b => b.MapFrom(c => c.TransferDetailId != null ? "TransferDetail" : "PurchaseInvoiceDetail"))
               .ForMember(a => a.MoneyConvertTxt, b => b.MapFrom(c => (c.MoneyConvert != null)? c.MoneyConvert.BaseAmount.GetValueOrDefault().ToString("N0") + " " + c.MoneyConvert.BaseCurrency.Name + " = " + c.MoneyConvert.DestAmount.GetValueOrDefault().ToString("N0") + " " + c.MoneyConvert.DestCurrency.Name : "1 " + c.Currency.Name + " = 1 " + c.Currency.Name))
               .ForMember(a => a.MoneyConvertTxtId, b => b.MapFrom(c => c.MoneyConvertId))
               .ForMember(a => a.FirstMoneyConvertId, b => b.MapFrom(c => c.MoneyConvertId))
               .ForMember(a => a.RemainingNum, b => b.MapFrom(c => c.PurchaseInvoiceDetail != null ? (c.PurchaseInvoiceDetail.RemainingNum) : (c.TransferDetail.RemainingNum)))
               //.ForMember(a => a.Profit, b => b.MapFrom(c => (c.MoneyConvert == null) ? ((c.SalePrice - c.PurchaseInvoiceDetail.PurchasePrice) / c.PurchaseInvoiceDetail.PurchasePrice) * 100 : ((c.Currency.Name == c.MoneyConvert.BaseCurrency.Name)?((c.SalePrice - (c.PurchaseInvoiceDetail.PurchasePrice * (c.MoneyConvert.BaseAmount/ c.MoneyConvert.DestAmount))) / (c.PurchaseInvoiceDetail.PurchasePrice * (c.MoneyConvert.BaseAmount / c.MoneyConvert.DestAmount))) * 100 : ((c.SalePrice - (c.PurchaseInvoiceDetail.PurchasePrice / (c.MoneyConvert.BaseAmount / c.MoneyConvert.DestAmount))) / (c.PurchaseInvoiceDetail.PurchasePrice / (c.MoneyConvert.BaseAmount / c.MoneyConvert.DestAmount))) * 100)))
               .ForMember(a => a.Currency, b => b.Ignore())
               .ForMember(a => a.Product, b => b.Ignore())
               .ForMember(a => a.PurchaseInvoiceDetail, b => b.Ignore())
               .ForMember(a => a.TransferDetail, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            saleInvoiceDetailDtoList = mapper.Map<IEnumerable<SaleInvoiceDetail>, List<SaleInvoiceDetailViewModel>>(saleInvoiceDetails);
            return saleInvoiceDetailDtoList;
        }

        public List<ReturnSaleInvoiceDetailSelectViewModel> ConvertSelectModelsLists(IEnumerable<SaleInvoiceDetail> purchaseInvoiceDetails)
        {
            List<ReturnSaleInvoiceDetailSelectViewModel> purchaseInvoiceDetailDtoList = new List<ReturnSaleInvoiceDetailSelectViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaleInvoiceDetail, ReturnSaleInvoiceDetailSelectViewModel>()
                //.ForMember(a => a.SalePrice, b => b.MapFrom(c => c.SalePrice))
                .ForMember(a => a.InvoiceNum, b => b.MapFrom(c => c.SaleInvoice.InvoiceNum))
                .ForMember(a => a.ExpireDate, b => b.MapFrom(c => c.PurchaseInvoiceDetail.ExpireDate ?? c.TransferDetail.ExpireDate))
                .ForMember(a => a.InvoiceDate, b => b.MapFrom(c => c.SaleInvoice.InvoiceDate))
                .ForMember(a => a.Currency, b => b.MapFrom(c => c.Currency.Name))
                //.ForMember(a => a.SellingCurrencyName, b => b.MapFrom(c => c.SellingCurrency.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDetailDtoList = mapper.Map<IEnumerable<SaleInvoiceDetail>, List<ReturnSaleInvoiceDetailSelectViewModel>>(purchaseInvoiceDetails);
            return purchaseInvoiceDetailDtoList;
        }

        
        // End Convert
    }
}
