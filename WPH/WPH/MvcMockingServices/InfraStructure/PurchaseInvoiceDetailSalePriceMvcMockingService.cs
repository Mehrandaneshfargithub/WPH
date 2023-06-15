using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.PurchaseInvoiceDetailSalePrice;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class PurchaseInvoiceDetailSalePriceMvcMockingService : IPurchaseInvoiceDetailSalePriceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public PurchaseInvoiceDetailSalePriceMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public OperationStatus RemovePurchaseInvoiceDetailSalePrice(Guid PurchaseInvoiceDetailSalePriceid)
        {
            try
            {
                PurchaseInvoiceDetailSalePrice Hos = _unitOfWork.PurchaseInvoiceDetailSalePrice.Get(PurchaseInvoiceDetailSalePriceid);
                _unitOfWork.PurchaseInvoiceDetailSalePrice.Remove(Hos);
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

        public void GetTransferModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/TransferDetailSalePrice/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/PurchaseInvoiceDetailSalePrice/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string UpdateMainSalePrice(Guid detailId, int typeId, string priceTxt, Guid userId)
        {
            if (typeId < 1 || typeId > 3)
                return "DataNotValid";

            if (!decimal.TryParse(priceTxt, out decimal price))
                return "DataNotValid";

            var detail = _unitOfWork.TransferDetails.Get(detailId);
            if (typeId == 1)
            {
                if (price == 0)
                    return "DataNotValid";

                detail.SellingPrice = price;
            }
            else if (typeId == 2)
            {
                var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseMiddleSellPrice");
                if (!access)
                    return "AccessDenied";

                detail.MiddleSellPrice = price;
            }
            else if (typeId == 3)
            {
                var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseWholeSellPrice");
                if (!access)
                    return "AccessDenied";

                detail.WholeSellPrice = price;
            }


            detail.ModifiedUserId = userId;
            detail.ModifiedDate = DateTime.Now;

            _unitOfWork.TransferDetails.UpdateState(detail);
            _unitOfWork.Complete();

            return detailId.ToString();
        }

        public string AddNewTransferDetailSalePrice(PurchaseInvoiceDetailSalePriceViewModel viewModel)
        {
            if (viewModel.TransferDetailId == null || viewModel.CurrencyId == null || viewModel.TypeId == null)
                return "DataNotValid";

            var duplicate = _unitOfWork.PurchaseInvoiceDetailSalePrice.CheckTransferCurrencyExist(viewModel.TransferDetailId, viewModel.CurrencyId, viewModel.TypeId);
            if (duplicate)
                return "RepeatedCurrency";

            var purchase = _unitOfWork.TransferDetails.GetForSalePrice(viewModel.TransferDetailId.Value);

            if (purchase == null || purchase.CurrencyId == viewModel.CurrencyId)
                return "DataNotValid";

            PurchaseInvoiceDetailSalePrice salePrice = Common.ConvertModels<PurchaseInvoiceDetailSalePrice, PurchaseInvoiceDetailSalePriceViewModel>.convertModels(viewModel);

            if (salePrice.MoneyConvertId == null || salePrice.MoneyConvertId == Guid.Empty)
            {
                if (viewModel.BaseAmount.GetValueOrDefault(0) <= 0 || viewModel.DestAmount.GetValueOrDefault(0) <= 0)
                    return "DataNotValid";

                if (viewModel.BaseAmount == viewModel.DestAmount)
                    return "ThereIsNoMoneyConvert";

                var rel = _unitOfWork.MoneyConvert.GetMoneyConvertIdBaseCurrenciesAndAmounts(viewModel.ClinicSectionId, purchase.CurrencyId.Value, viewModel.CurrencyId.Value, viewModel.BaseAmount, viewModel.DestAmount);
                salePrice.MoneyConvertId = rel;

                if (rel == null)
                {
                    var money_convert = new MoneyConvert
                    {
                        Guid = Guid.NewGuid(),
                        ClinicSectionId = viewModel.ClinicSectionId,
                        BaseCurrencyId = purchase.CurrencyId.Value,
                        DestCurrencyId = viewModel.CurrencyId.Value,
                        BaseAmount = viewModel.BaseAmount,
                        DestAmount = viewModel.DestAmount,
                        IsMain = false,
                        Date = DateTime.Now,
                    };

                    _unitOfWork.MoneyConvert.Add(money_convert);
                    salePrice.MoneyConvertId = money_convert.Guid;
                }
            }
            salePrice.CreateUserId = viewModel.UserId;
            salePrice.CreateDate = DateTime.Now;

            _unitOfWork.PurchaseInvoiceDetailSalePrice.Add(salePrice);
            _unitOfWork.Complete();
            return salePrice.Guid.ToString();
        }

        public string AddNewPurchaseInvoiceDetailSalePrice(PurchaseInvoiceDetailSalePriceViewModel viewModel)
        {
            if (viewModel.PurchaseInvoiceDetailId == null || viewModel.CurrencyId == null || viewModel.TypeId == null)
                return "DataNotValid";

            var duplicate = _unitOfWork.PurchaseInvoiceDetailSalePrice.CheckCurrencyExist(viewModel.PurchaseInvoiceDetailId, viewModel.CurrencyId, viewModel.TypeId);
            if (duplicate)
                return "RepeatedCurrency";

            var purchase = _unitOfWork.PurchaseInvoiceDetails.GetForSalePrice(viewModel.PurchaseInvoiceDetailId.Value);

            if (purchase == null || purchase.CurrencyId == viewModel.CurrencyId)
                return "DataNotValid";

            //var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoiceInUse(purchase.MasterId.Value);
            //if (can_change)
            //    return "InvoiceInUse";

            PurchaseInvoiceDetailSalePrice salePrice = Common.ConvertModels<PurchaseInvoiceDetailSalePrice, PurchaseInvoiceDetailSalePriceViewModel>.convertModels(viewModel);

            if (salePrice.MoneyConvertId == null || salePrice.MoneyConvertId == Guid.Empty)
            {
                if (viewModel.BaseAmount.GetValueOrDefault(0) <= 0 || viewModel.DestAmount.GetValueOrDefault(0) <= 0)
                    return "DataNotValid";

                if (viewModel.BaseAmount == viewModel.DestAmount)
                    return "ThereIsNoMoneyConvert";

                var rel = _unitOfWork.MoneyConvert.GetMoneyConvertIdBaseCurrenciesAndAmounts(viewModel.ClinicSectionId, purchase.CurrencyId.Value, viewModel.CurrencyId.Value, viewModel.BaseAmount, viewModel.DestAmount);
                salePrice.MoneyConvertId = rel;

                if (rel == null)
                {
                    var money_convert = new MoneyConvert
                    {
                        Guid = Guid.NewGuid(),
                        ClinicSectionId = viewModel.ClinicSectionId,
                        BaseCurrencyId = purchase.CurrencyId.Value,
                        DestCurrencyId = viewModel.CurrencyId.Value,
                        BaseAmount = viewModel.BaseAmount,
                        DestAmount = viewModel.DestAmount,
                        IsMain = false,
                        Date = DateTime.Now,
                    };

                    _unitOfWork.MoneyConvert.Add(money_convert);
                    salePrice.MoneyConvertId = money_convert.Guid;
                }
            }
            salePrice.CreateUserId = viewModel.UserId;
            salePrice.CreateDate = DateTime.Now;

            _unitOfWork.PurchaseInvoiceDetailSalePrice.Add(salePrice);
            _unitOfWork.Complete();
            return salePrice.Guid.ToString();
        }


        public string UpdateTransferDetailSalePrice(PurchaseInvoiceDetailSalePriceViewModel viewModel)
        {
            if (viewModel.TransferDetailId == null || viewModel.CurrencyId == null)
                return "DataNotValid";

            var duplicate = _unitOfWork.PurchaseInvoiceDetailSalePrice.CheckTransferCurrencyExist(viewModel.TransferDetailId, viewModel.CurrencyId, viewModel.TypeId, p => p.Guid != viewModel.Guid);
            if (duplicate)
                return "RepeatedCurrency";

            var purchase = _unitOfWork.TransferDetails.GetForSalePrice(viewModel.TransferDetailId.Value);

            if (purchase == null || purchase.CurrencyId == viewModel.CurrencyId)
                return "DataNotValid";

            var salePrice = _unitOfWork.PurchaseInvoiceDetailSalePrice.Get(viewModel.Guid);

            salePrice.MoneyConvertId = viewModel.MoneyConvertId;
            salePrice.CurrencyId = viewModel.CurrencyId;
            salePrice.TypeId = viewModel.TypeId;
            salePrice.ModifiedUserId = viewModel.UserId;
            salePrice.ModifiedDate = DateTime.Now;

            if (salePrice.MoneyConvertId == null || salePrice.MoneyConvertId == Guid.Empty)
            {
                if (viewModel.BaseAmount.GetValueOrDefault(0) <= 0 || viewModel.DestAmount.GetValueOrDefault(0) <= 0)
                    return "DataNotValid";

                if (viewModel.BaseAmount == viewModel.DestAmount)
                    return "ThereIsNoMoneyConvert";

                var rel = _unitOfWork.MoneyConvert.GetMoneyConvertIdBaseCurrenciesAndAmounts(viewModel.ClinicSectionId, purchase.CurrencyId.Value, viewModel.CurrencyId.Value, viewModel.BaseAmount, viewModel.DestAmount);
                salePrice.MoneyConvertId = rel;

                if (rel == null)
                {
                    var money_convert = new MoneyConvert
                    {
                        Guid = Guid.NewGuid(),
                        ClinicSectionId = viewModel.ClinicSectionId,
                        BaseCurrencyId = purchase.CurrencyId.Value,
                        DestCurrencyId = viewModel.CurrencyId.Value,
                        BaseAmount = viewModel.BaseAmount,
                        DestAmount = viewModel.DestAmount,
                        IsMain = false,
                        Date = DateTime.Now,
                    };

                    _unitOfWork.MoneyConvert.Add(money_convert);
                    salePrice.MoneyConvertId = money_convert.Guid;
                }
            }

            _unitOfWork.PurchaseInvoiceDetailSalePrice.UpdateState(salePrice);
            _unitOfWork.Complete();
            return salePrice.Guid.ToString();
        }

        public string UpdatePurchaseInvoiceDetailSalePrice(PurchaseInvoiceDetailSalePriceViewModel viewModel)
        {
            if (viewModel.PurchaseInvoiceDetailId == null || viewModel.CurrencyId == null)
                return "DataNotValid";

            var duplicate = _unitOfWork.PurchaseInvoiceDetailSalePrice.CheckCurrencyExist(viewModel.PurchaseInvoiceDetailId, viewModel.CurrencyId, viewModel.TypeId, p => p.Guid != viewModel.Guid);
            if (duplicate)
                return "RepeatedCurrency";

            var purchase = _unitOfWork.PurchaseInvoiceDetails.GetForSalePrice(viewModel.PurchaseInvoiceDetailId.Value);

            if (purchase == null || purchase.CurrencyId == viewModel.CurrencyId)
                return "DataNotValid";

            //var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoiceInUse(purchase.MasterId.Value);
            //if (can_change)
            //    return "InvoiceInUse";

            var salePrice = _unitOfWork.PurchaseInvoiceDetailSalePrice.Get(viewModel.Guid);

            salePrice.MoneyConvertId = viewModel.MoneyConvertId;
            salePrice.CurrencyId = viewModel.CurrencyId;
            salePrice.TypeId = viewModel.TypeId;
            salePrice.ModifiedUserId = viewModel.UserId;
            salePrice.ModifiedDate = DateTime.Now;

            if (salePrice.MoneyConvertId == null || salePrice.MoneyConvertId == Guid.Empty)
            {
                if (viewModel.BaseAmount.GetValueOrDefault(0) <= 0 || viewModel.DestAmount.GetValueOrDefault(0) <= 0)
                    return "DataNotValid";

                if (viewModel.BaseAmount == viewModel.DestAmount)
                    return "ThereIsNoMoneyConvert";

                var rel = _unitOfWork.MoneyConvert.GetMoneyConvertIdBaseCurrenciesAndAmounts(viewModel.ClinicSectionId, purchase.CurrencyId.Value, viewModel.CurrencyId.Value, viewModel.BaseAmount, viewModel.DestAmount);
                salePrice.MoneyConvertId = rel;

                if (rel == null)
                {
                    var money_convert = new MoneyConvert
                    {
                        Guid = Guid.NewGuid(),
                        ClinicSectionId = viewModel.ClinicSectionId,
                        BaseCurrencyId = purchase.CurrencyId.Value,
                        DestCurrencyId = viewModel.CurrencyId.Value,
                        BaseAmount = viewModel.BaseAmount,
                        DestAmount = viewModel.DestAmount,
                        IsMain = false,
                        Date = DateTime.Now,
                    };

                    _unitOfWork.MoneyConvert.Add(money_convert);
                    salePrice.MoneyConvertId = money_convert.Guid;
                }
            }

            _unitOfWork.PurchaseInvoiceDetailSalePrice.UpdateState(salePrice);
            _unitOfWork.Complete();
            return salePrice.Guid.ToString();
        }


        public IEnumerable<PurchaseInvoiceDetailSalePriceViewModel> GetAllTransferDetailSalePrices(Guid transferDetailId, IStringLocalizer<SharedResource> localizer)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var access = _idunit.subSystem.GetUserSubSystemAccess("CanUseWholeSellPrice", "CanUseMiddleSellPrice");

            var prices = _unitOfWork.TransferDetails.GetPricesByDetailId(transferDetailId);

            var result = new List<PurchaseInvoiceDetailSalePriceViewModel>
            {
                new PurchaseInvoiceDetailSalePriceViewModel
                {
                    Guid = transferDetailId,
                    CurrencyName = prices.CurrencyName,
                    TypeId = 1,
                    TypeName = localizer["RetailPrice"],
                    MoneyConvertName = "",
                    AmountTxt = prices.SellingPrice.GetValueOrDefault(0).ToString("#,0.##", cultures)
                }
            };

            if (access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice"))
            {
                result.Add(new PurchaseInvoiceDetailSalePriceViewModel
                {
                    Guid = transferDetailId,
                    CurrencyName = prices.CurrencyName,
                    TypeId = 2,
                    TypeName = localizer["MiddelPrice"],
                    MoneyConvertName = "",
                    AmountTxt = prices.MiddleSellPrice.GetValueOrDefault(0).ToString("#,0.##", cultures)
                });
            }

            if (access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice"))
            {
                result.Add(new PurchaseInvoiceDetailSalePriceViewModel
                {
                    Guid = transferDetailId,
                    CurrencyName = prices.CurrencyName,
                    TypeId = 3,
                    TypeName = localizer["WholePrice"],
                    MoneyConvertName = "",
                    AmountTxt = prices.WholeSellPrice.GetValueOrDefault(0).ToString("#,0.##", cultures)
                });
            }

            var list = _unitOfWork.PurchaseInvoiceDetailSalePrice.GetAllTransferDetailSalePrice(transferDetailId);
            result.AddRange(list.Select(p => new PurchaseInvoiceDetailSalePriceViewModel
            {
                Guid = p.Guid,
                CurrencyName = p.Currency.Name,
                TypeId = 0,
                TypeName = localizer[p.Type.Name],
                MoneyConvertName = $"{p.MoneyConvert.BaseAmount.GetValueOrDefault(0).ToString("#,0.##", cultures)} {p.MoneyConvert.BaseCurrency.Name} = {p.MoneyConvert.DestAmount.GetValueOrDefault(0).ToString("#,0.##", cultures)} {p.MoneyConvert.DestCurrency.Name}",
                AmountTxt = CalculatePrice(cultures, p.TransferDetail.PurchaseCurrency.Name, p.MoneyConvert.BaseCurrency.Name, p.MoneyConvert.DestCurrency.Name,
                                          p.Type.Name == "WholePrice" ? p.TransferDetail.WholeSellPrice : p.Type.Name == "MiddelPrice" ? p.TransferDetail.MiddleSellPrice : p.TransferDetail.SellingPrice,
                                          p.MoneyConvert.BaseAmount, p.MoneyConvert.DestAmount)
            }).ToList());

            Indexing<PurchaseInvoiceDetailSalePriceViewModel> indexing = new Indexing<PurchaseInvoiceDetailSalePriceViewModel>();
            return indexing.AddIndexing(result);
        }


        public IEnumerable<PurchaseInvoiceDetailSalePriceViewModel> GetAllPurchaseInvoiceDetailSalePrices(Guid purchaseInvoiceDetailId, IStringLocalizer<SharedResource> localizer)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var list = _unitOfWork.PurchaseInvoiceDetailSalePrice.GetAllPurchaseInvoiceDetailSalePrice(purchaseInvoiceDetailId);
            var result = list.Select(p => new PurchaseInvoiceDetailSalePriceViewModel
            {
                Guid = p.Guid,
                CurrencyName = p.Currency.Name,
                TypeName = localizer[p.Type.Name],
                MoneyConvertName = $"{p.MoneyConvert.BaseAmount.GetValueOrDefault(0).ToString("#,0.##", cultures)} {p.MoneyConvert.BaseCurrency.Name} = {p.MoneyConvert.DestAmount.GetValueOrDefault(0).ToString("#,0.##", cultures)} {p.MoneyConvert.DestCurrency.Name}",
                AmountTxt = CalculatePrice(cultures, p.PurchaseInvoiceDetail.Currency.Name, p.MoneyConvert.BaseCurrency.Name, p.MoneyConvert.DestCurrency.Name,
                                            p.Type.Name == "WholePrice" ? p.PurchaseInvoiceDetail.WholeSellPrice : p.Type.Name == "MiddelPrice" ? p.PurchaseInvoiceDetail.MiddleSellPrice : p.PurchaseInvoiceDetail.SellingPrice,
                                            p.MoneyConvert.BaseAmount, p.MoneyConvert.DestAmount)
            }).ToList();

            Indexing<PurchaseInvoiceDetailSalePriceViewModel> indexing = new Indexing<PurchaseInvoiceDetailSalePriceViewModel>();
            return indexing.AddIndexing(result);
        }

        private static string CalculatePrice(CultureInfo cultures, string currency, string baseCurrency, string destcurrency, decimal? amount, decimal? baseAmount, decimal? destAmount)
        {
            if (currency == baseCurrency)
            {
                return (amount.GetValueOrDefault(0) * destAmount.GetValueOrDefault(0) / baseAmount.GetValueOrDefault(1)).ToString("#,0.##", cultures);
            }
            else if (currency == destcurrency)
            {
                return (amount.GetValueOrDefault(0) * baseAmount.GetValueOrDefault(0) / destAmount.GetValueOrDefault(1)).ToString("#,0.##", cultures);
            }
            else
            {
                return "0";
            }
            return "";
        }

        public PurchaseInvoiceDetailSalePriceViewModel GetTransferParentCurrency(Guid transferDetailId)
        {
            try
            {
                CultureInfo cultures = new CultureInfo("en-US");

                var purchase = _unitOfWork.TransferDetails.GetParentCurrency(transferDetailId);
                return new PurchaseInvoiceDetailSalePriceViewModel
                {
                    TransferDetailId = transferDetailId,
                    BaseCurrencyId = purchase.CurrencyId,
                    CurrencyName = purchase.CurrencyName,
                    SellingPrice = purchase.SellingPrice.GetValueOrDefault(0).ToString("0.##", cultures),
                    MiddleSellPrice = purchase.MiddleSellPrice.GetValueOrDefault(0).ToString("0.##", cultures),
                    WholeSellPrice = purchase.WholeSellPrice.GetValueOrDefault(0).ToString("0.##", cultures),
                    BaseAmountTxt = "1",
                    DestAmountTxt = "1",
                };
            }
            catch { return null; }
        }

        public PurchaseInvoiceDetailSalePriceViewModel GetParentCurrency(Guid purchaseInvoiceDetailId)
        {
            try
            {
                CultureInfo cultures = new CultureInfo("en-US");

                var purchase = _unitOfWork.PurchaseInvoiceDetails.GetParentCurrency(purchaseInvoiceDetailId);
                return new PurchaseInvoiceDetailSalePriceViewModel
                {
                    PurchaseInvoiceDetailId = purchaseInvoiceDetailId,
                    BaseCurrencyId = purchase.CurrencyId,
                    CurrencyName = purchase.CurrencyName,
                    SellingPrice = purchase.SellingPrice.GetValueOrDefault(0).ToString("0.##", cultures),
                    MiddleSellPrice = purchase.MiddleSellPrice.GetValueOrDefault(0).ToString("0.##", cultures),
                    WholeSellPrice = purchase.WholeSellPrice.GetValueOrDefault(0).ToString("0.##", cultures),
                    BaseAmountTxt = "1",
                    DestAmountTxt = "1",
                };
            }
            catch { return null; }
        }

        public PurchaseInvoiceDetailSalePriceViewModel GetTransferDetailSalePrice(Guid purchaseInvoiceDetailSalePriceId)
        {
            try
            {
                PurchaseInvoiceDetailSalePrice salePrice = _unitOfWork.PurchaseInvoiceDetailSalePrice.GetForTransferEdit(purchaseInvoiceDetailSalePriceId);
                return ConvertTransferModel(salePrice);
            }
            catch { return null; }
        }

        public PurchaseInvoiceDetailSalePriceViewModel GetPurchaseInvoiceDetailSalePrice(Guid purchaseInvoiceDetailSalePriceId)
        {
            try
            {
                PurchaseInvoiceDetailSalePrice salePrice = _unitOfWork.PurchaseInvoiceDetailSalePrice.GetForEdit(purchaseInvoiceDetailSalePriceId);
                return ConvertModel(salePrice);
            }
            catch { return null; }
        }

        // Begin Convert 
        public PurchaseInvoiceDetailSalePriceViewModel ConvertTransferModel(PurchaseInvoiceDetailSalePrice Users)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDetailSalePrice, PurchaseInvoiceDetailSalePriceViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.TransferDetail.PurchaseCurrency.Name))
                .ForMember(a => a.BaseCurrencyId, b => b.MapFrom(c => c.TransferDetail.CurrencyId))
                .ForMember(a => a.BaseAmountTxt, b => b.MapFrom(c => c.MoneyConvert.BaseAmount.GetValueOrDefault(1).ToString("0.##", cultures)))
                .ForMember(a => a.DestAmountTxt, b => b.MapFrom(c => c.MoneyConvert.DestAmount.GetValueOrDefault(1).ToString("0.##", cultures)))
                .ForMember(a => a.SellingPrice, b => b.MapFrom(c => c.TransferDetail.SellingPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.MiddleSellPrice, b => b.MapFrom(c => c.TransferDetail.MiddleSellPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.WholeSellPrice, b => b.MapFrom(c => c.TransferDetail.WholeSellPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                //.ForMember(a => a.MoneyConvertName, b => b.MapFrom(c => $"{c.MoneyConvert.BaseAmount.GetValueOrDefault(0).ToString("#,#.##", cultures)} {c.MoneyConvert.BaseCurrency.Name} = {c.MoneyConvert.DestAmount.GetValueOrDefault(0).ToString("#,#.##", cultures)} {c.MoneyConvert.DestCurrency.Name}"))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PurchaseInvoiceDetailSalePrice, PurchaseInvoiceDetailSalePriceViewModel>(Users);
        }

        public PurchaseInvoiceDetailSalePriceViewModel ConvertModel(PurchaseInvoiceDetailSalePrice Users)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDetailSalePrice, PurchaseInvoiceDetailSalePriceViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.PurchaseInvoiceDetail.Currency.Name))
                .ForMember(a => a.BaseCurrencyId, b => b.MapFrom(c => c.PurchaseInvoiceDetail.CurrencyId))
                .ForMember(a => a.BaseAmountTxt, b => b.MapFrom(c => c.MoneyConvert.BaseAmount.GetValueOrDefault(1).ToString("0.##", cultures)))
                .ForMember(a => a.DestAmountTxt, b => b.MapFrom(c => c.MoneyConvert.DestAmount.GetValueOrDefault(1).ToString("0.##", cultures)))
                .ForMember(a => a.SellingPrice, b => b.MapFrom(c => c.PurchaseInvoiceDetail.SellingPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.MiddleSellPrice, b => b.MapFrom(c => c.PurchaseInvoiceDetail.MiddleSellPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.WholeSellPrice, b => b.MapFrom(c => c.PurchaseInvoiceDetail.WholeSellPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                //.ForMember(a => a.MoneyConvertName, b => b.MapFrom(c => $"{c.MoneyConvert.BaseAmount.GetValueOrDefault(0).ToString("#,#.##", cultures)} {c.MoneyConvert.BaseCurrency.Name} = {c.MoneyConvert.DestAmount.GetValueOrDefault(0).ToString("#,#.##", cultures)} {c.MoneyConvert.DestCurrency.Name}"))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PurchaseInvoiceDetailSalePrice, PurchaseInvoiceDetailSalePriceViewModel>(Users);
        }

        public List<PurchaseInvoiceDetailSalePriceViewModel> ConvertModelsLists(IEnumerable<PurchaseInvoiceDetailSalePrice> items)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            List<PurchaseInvoiceDetailSalePriceViewModel> itemDtoList = new List<PurchaseInvoiceDetailSalePriceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDetailSalePrice, PurchaseInvoiceDetailSalePriceViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                .ForMember(a => a.TypeName, b => b.MapFrom(c => c.Type.Name))
                .ForMember(a => a.MoneyConvertName, b => b.MapFrom(c => $"{c.MoneyConvert.BaseAmount.GetValueOrDefault(0).ToString("#,0.##", cultures)} {c.MoneyConvert.BaseCurrency.Name} = {c.MoneyConvert.DestAmount.GetValueOrDefault(0).ToString("#,0.##", cultures)} {c.MoneyConvert.DestCurrency.Name}"))
                ;
            });

            IMapper mapper = config.CreateMapper();
            itemDtoList = mapper.Map<IEnumerable<PurchaseInvoiceDetailSalePrice>, List<PurchaseInvoiceDetailSalePriceViewModel>>(items);
            return itemDtoList;
        }
        // End Convert
    }
}
