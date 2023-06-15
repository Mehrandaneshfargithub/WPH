using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.PurchaseInvoiceDetail;
using WPH.Models.PurchaseInvoiceDetailSalePrice;
using WPH.Models.ReturnPurchaseInvoiceDetail;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class PurchaseInvoiceDetailMvcMockingService : IPurchaseInvoiceDetailMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public PurchaseInvoiceDetailMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemovePurchaseInvoiceDetail(Guid purchaseInvoiceDetailid)
        {
            try
            {
                PurchaseInvoiceDetail Hos = _unitOfWork.PurchaseInvoiceDetails.Get(purchaseInvoiceDetailid);

                var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(Hos.MasterId.Value);
                if (can_change)
                    return "InvoiceInUse";

                var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(Hos.MasterId.Value);
                if (currencyExist == null)
                    return "DataNotValid";

                var total_purchase = currencyExist.PurchaseInvoiceDetails.Where(p => p.CurrencyId == Hos.CurrencyId && p.Guid != purchaseInvoiceDetailid).Sum(p => p.Num.GetValueOrDefault(0) * p.PurchasePrice.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
                var total_discount = currencyExist.PurchaseInvoiceDiscounts.Where(x => x.CurrencyId == Hos.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
                var after_discount = total_purchase - total_discount;
                if (after_discount < 0)
                    return "DiscountIsGreaterThanTheAmount";

                _unitOfWork.PurchaseInvoiceDetails.Remove(Hos);
                //_unitOfWork.Complete();
                //_unitOfWork.PurchaseInvoiceDetails.Detach(Hos);

                currencyExist.PurchaseInvoiceDetails = currencyExist.PurchaseInvoiceDetails.Where(p => p.Guid != purchaseInvoiceDetailid).ToList();

                return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
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
            string controllerName = "/PurchaseInvoiceDetail/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewPurchaseInvoiceDetail(PurchaseInvoiceDetailViewModel viewModel, Guid clinicSectionId)
        {
            if (viewModel.MasterId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(viewModel.MasterId.Value);
            if (can_change)
                return "InvoiceInUse";

            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Discount = decimal.Parse(viewModel.DiscountTxt ?? "0", cultures);
            viewModel.Num = decimal.Parse(viewModel.NumTxt ?? "0", cultures);
            viewModel.PurchasePrice = decimal.Parse(viewModel.PurchasePriceTxt ?? "0", cultures);
            viewModel.SellingPrice = decimal.Parse(viewModel.SellingPriceTxt ?? "0", cultures);
            viewModel.FreeNum = decimal.Parse(viewModel.FreeNumTxt ?? "0", cultures);
            viewModel.WholeSellPrice = decimal.Parse(viewModel.WholeSellPriceTxt ?? "0", cultures);
            viewModel.MiddleSellPrice = decimal.Parse(viewModel.MiddleSellPriceTxt ?? "0", cultures);
            viewModel.WholePurchasePrice = decimal.Parse(viewModel.WholePurchasePriceTxt ?? "0", cultures);

            if (viewModel.ProductId == null || (viewModel.Num.GetValueOrDefault(0) <= 0 && viewModel.FreeNum.GetValueOrDefault(0) <= 0))
                return "DataNotValid";

            PurchaseInvoiceDetail purchaseInvoiceDetail = Common.ConvertModels<PurchaseInvoiceDetail, PurchaseInvoiceDetailViewModel>.convertModels(viewModel);

            var now = DateTime.Now;
            if (viewModel.PurchaseType == "Medicine")
            {

                if (viewModel.Num.GetValueOrDefault(0) != 0 && (viewModel.WholePurchasePrice.GetValueOrDefault(0) <= 0 || viewModel.SellingPrice.GetValueOrDefault(0) <= 0))
                    return "DataNotValid";

                if (viewModel.Num.GetValueOrDefault(0) == 0 && viewModel.SellingPrice.GetValueOrDefault(0) <= 0)
                    return "DataNotValid";

                if (!DateTime.TryParseExact(viewModel.ExpireDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime expireDate))
                    return "DateNotValid";


                if (expireDate.Date <= now.Date)
                    return "DateNotValid";

                purchaseInvoiceDetail.ExpireDate = expireDate;
            }

            purchaseInvoiceDetail.CreateDate = now;
            purchaseInvoiceDetail.RemainingNum = purchaseInvoiceDetail.Num + purchaseInvoiceDetail.FreeNum;
            purchaseInvoiceDetail.PurchasePrice = viewModel.Num.GetValueOrDefault(0) == 0 ? 0 : (viewModel.WholePurchasePrice / viewModel.Num);

            if ((purchaseInvoiceDetail.Num * purchaseInvoiceDetail.PurchasePrice) - purchaseInvoiceDetail.Discount < 0)
                return "DiscountIsGreaterThanTheAmount";

            _unitOfWork.PurchaseInvoiceDetails.Add(purchaseInvoiceDetail);
            //_unitOfWork.Complete();

            //_unitOfWork.PurchaseInvoiceDetails.Detach(purchaseInvoiceDetail);

            var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(purchaseInvoiceDetail.MasterId.Value);
            purchaseInvoiceDetail.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(purchaseInvoiceDetail.CurrencyId.Value);
            currencyExist.PurchaseInvoiceDetails.Add(purchaseInvoiceDetail);

            return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
        }


        public string UpdatePurchaseInvoiceDetail(PurchaseInvoiceDetailViewModel viewModel, Guid clinicSectionId)
        {
            if (viewModel.MasterId == null)
                return "DataNotValid";

            //var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoiceInUse(viewModel.MasterId.Value);
            //if (can_change)
            //    return "InvoiceInUse";

            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Discount = decimal.Parse(viewModel.DiscountTxt ?? "0", cultures);
            viewModel.Num = decimal.Parse(viewModel.NumTxt ?? "0", cultures);
            viewModel.PurchasePrice = decimal.Parse(viewModel.PurchasePriceTxt ?? "0", cultures);
            viewModel.SellingPrice = decimal.Parse(viewModel.SellingPriceTxt ?? "0", cultures);
            viewModel.FreeNum = decimal.Parse(viewModel.FreeNumTxt ?? "0", cultures);
            viewModel.WholeSellPrice = decimal.Parse(viewModel.WholeSellPriceTxt ?? "0", cultures);
            viewModel.MiddleSellPrice = decimal.Parse(viewModel.MiddleSellPriceTxt ?? "0", cultures);
            viewModel.WholePurchasePrice = decimal.Parse(viewModel.WholePurchasePriceTxt ?? "0", cultures);


            if (viewModel.ProductId == null || (viewModel.Num.GetValueOrDefault(0) <= 0 && viewModel.FreeNum.GetValueOrDefault(0) <= 0))
                return "DataNotValid";

            var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(viewModel.MasterId.Value);
            if (can_change)
                return UpdateAfterPayInvoice(viewModel);

            PurchaseInvoiceDetail purchaseInvoiceDetailOld = _unitOfWork.PurchaseInvoiceDetails.Get(viewModel.Guid);

            if (viewModel.PurchaseType == "Medicine")
            {

                if (!DateTime.TryParseExact(viewModel.ExpireDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime expireDate))
                    return "DateNotValid";

                if (expireDate.Date < purchaseInvoiceDetailOld.ExpireDate.Value.Date)
                    return "DateNotValid";

                viewModel.ExpireDate = expireDate;
                purchaseInvoiceDetailOld.ExpireDate = expireDate;

                var conflict = _unitOfWork.PurchaseInvoiceDetailSalePrice.CheckConflictCurrency(viewModel.Guid, viewModel.CurrencyId);
                if (conflict)
                    return "ConflictCurrency";

                if (viewModel.Num.GetValueOrDefault(0) != 0 && (viewModel.WholePurchasePrice.GetValueOrDefault(0) <= 0 || viewModel.SellingPrice.GetValueOrDefault(0) <= 0))
                    return "DataNotValid";

                if (viewModel.Num.GetValueOrDefault(0) == 0 && viewModel.SellingPrice.GetValueOrDefault(0) <= 0)
                    return "DataNotValid";

            }

            if (viewModel.Num < purchaseInvoiceDetailOld.Num)
            {
                var minus = (purchaseInvoiceDetailOld.Num + purchaseInvoiceDetailOld.FreeNum) - (viewModel.Num + viewModel.FreeNum);
                if (purchaseInvoiceDetailOld.RemainingNum - minus < 0)
                {
                    return $"YouHaveJust {purchaseInvoiceDetailOld.RemainingNum.GetValueOrDefault(0):N0}";
                }
                purchaseInvoiceDetailOld.RemainingNum -= minus;
            }
            else
            {
                var minus = (viewModel.Num + viewModel.FreeNum) - (purchaseInvoiceDetailOld.Num + purchaseInvoiceDetailOld.FreeNum);
                purchaseInvoiceDetailOld.RemainingNum += minus;
            }

            purchaseInvoiceDetailOld.ModifiedUserId = viewModel.ModifiedUserId;
            purchaseInvoiceDetailOld.ModifiedDate = DateTime.Now;
            purchaseInvoiceDetailOld.Consideration = viewModel.Consideration;
            purchaseInvoiceDetailOld.Discount = viewModel.Discount;
            purchaseInvoiceDetailOld.FreeNum = viewModel.FreeNum;
            purchaseInvoiceDetailOld.Num = viewModel.Num;
            purchaseInvoiceDetailOld.PurchasePrice = viewModel.Num.GetValueOrDefault(0) == 0 ? 0 : (viewModel.WholePurchasePrice / viewModel.Num);
            purchaseInvoiceDetailOld.SellingPrice = viewModel.SellingPrice;
            purchaseInvoiceDetailOld.MiddleSellPrice = viewModel.MiddleSellPrice;
            purchaseInvoiceDetailOld.WholeSellPrice = viewModel.WholeSellPrice;
            purchaseInvoiceDetailOld.CurrencyId = viewModel.CurrencyId;

            if ((purchaseInvoiceDetailOld.Num * purchaseInvoiceDetailOld.PurchasePrice) - purchaseInvoiceDetailOld.Discount < 0)
                return "DiscountIsGreaterThanTheAmount";

            var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(purchaseInvoiceDetailOld.MasterId.Value);
            if (currencyExist == null)
                return "DataNotValid";

            var total_purchase = currencyExist.PurchaseInvoiceDetails.Where(p => p.CurrencyId == purchaseInvoiceDetailOld.CurrencyId && p.Guid != purchaseInvoiceDetailOld.Guid).Sum(p => p.Num.GetValueOrDefault(0) * p.PurchasePrice.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.PurchaseInvoiceDiscounts.Where(p => p.CurrencyId == purchaseInvoiceDetailOld.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_purchase - total_discount + (purchaseInvoiceDetailOld.Num * purchaseInvoiceDetailOld.PurchasePrice - purchaseInvoiceDetailOld.Discount.GetValueOrDefault(0));
            if (after_discount < 0)
                return "DiscountIsGreaterThanTheAmount";


            _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchaseInvoiceDetailOld);
            //_unitOfWork.Complete();

            //_unitOfWork.PurchaseInvoiceDetails.Detach(purchaseInvoiceDetailOld);
            currencyExist.PurchaseInvoiceDetails = currencyExist.PurchaseInvoiceDetails.Where(p => p.Guid != purchaseInvoiceDetailOld.Guid).ToList();
            purchaseInvoiceDetailOld.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(purchaseInvoiceDetailOld.CurrencyId.Value);
            currencyExist.PurchaseInvoiceDetails.Add(purchaseInvoiceDetailOld);

            return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
        }

        private string UpdateAfterPayInvoice(PurchaseInvoiceDetailViewModel viewModel)
        {
            PurchaseInvoiceDetail purchaseInvoiceDetailOld = _unitOfWork.PurchaseInvoiceDetails.Get(viewModel.Guid);
            if (viewModel.PurchaseType == "Medicine")
            {
                if (!DateTime.TryParseExact(viewModel.ExpireDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime expireDate))
                    return "DateNotValid";

                if (expireDate.Date < purchaseInvoiceDetailOld.ExpireDate.Value.Date)
                    return "DateNotValid";

                viewModel.ExpireDate = expireDate;
                purchaseInvoiceDetailOld.ExpireDate = expireDate;
            }

            purchaseInvoiceDetailOld.ModifiedUserId = viewModel.ModifiedUserId;
            purchaseInvoiceDetailOld.ModifiedDate = DateTime.Now;

            purchaseInvoiceDetailOld.SellingPrice = viewModel.SellingPrice;
            purchaseInvoiceDetailOld.MiddleSellPrice = viewModel.MiddleSellPrice;
            purchaseInvoiceDetailOld.WholeSellPrice = viewModel.WholeSellPrice;

            var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(purchaseInvoiceDetailOld.MasterId.Value);
            if (currencyExist == null)
                return "DataNotValid";

            _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchaseInvoiceDetailOld);

            return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
        }


        public IEnumerable<PurchaseInvoiceDetailViewModel> GetAllPurchaseInvoiceDetails(Guid purchaseInvoiceId)
        {
            IEnumerable<PurchaseInvoiceDetail> hosp = _unitOfWork.PurchaseInvoiceDetails.GetAllPurchaseInvoiceDetailByMasterId(purchaseInvoiceId);
            List<PurchaseInvoiceDetailViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<PurchaseInvoiceDetailViewModel> indexing = new Indexing<PurchaseInvoiceDetailViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<PurchaseInvoiceDetailHistoryViewModel> GetPurchaseHistory(Guid clinicSectionId, Guid productId)
        {
            var result = _unitOfWork.PurchaseInvoiceDetails.GetPurchaseHistoryByProductId(clinicSectionId, productId);
            List<PurchaseInvoiceDetailHistoryViewModel> hospconvert = ConvertHistoryModelsLists(result);
            Indexing<PurchaseInvoiceDetailHistoryViewModel> indexing = new Indexing<PurchaseInvoiceDetailHistoryViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReturnPurchaseInvoiceDetailSelectViewModel> GetDetailsForReturn(Guid masterId, Guid productId, Guid clinicSectionId, bool like)
        {
            var result = _unitOfWork.PurchaseInvoiceDetails.GetDetailsForReturn(productId, masterId, clinicSectionId, like);
            List<ReturnPurchaseInvoiceDetailSelectViewModel> hospconvert = ConvertSelectModelsLists(result);
            Indexing<ReturnPurchaseInvoiceDetailSelectViewModel> indexing = new Indexing<ReturnPurchaseInvoiceDetailSelectViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public PurchaseInvoiceDetailViewModel GetPurchaseInvoiceDetailForEdit(Guid purchaseInvoiceDetailId)
        {
            try
            {
                PurchaseInvoiceDetail PurchaseInvoiceDetailgu = _unitOfWork.PurchaseInvoiceDetails.GetPurchaseInvoiceDetailForEdit(purchaseInvoiceDetailId);
                return ConvertEditModel(PurchaseInvoiceDetailgu);
            }
            catch (Exception) { return null; }
        }

        public ParentDetailSalePriceViewModel GetForNewSalePrice(Guid purchaseInvoiceDetailId)
        {
            try
            {
                PurchaseInvoiceDetail detail = _unitOfWork.PurchaseInvoiceDetails.GetForNewSalePrice(purchaseInvoiceDetailId);
                return ConvertModel(detail);
            }
            catch (Exception) { return null; }
        }

        public Guid? GetPurchaseInvoiceDetailSalePrice(Guid purchaseInvoiceDetailId, int currencyId, string priceType, string saleType)
        {

            if (string.Compare(saleType, "PurchaseInvoiceDetail", true) == 0)
                return _unitOfWork.PurchaseInvoiceDetailSalePrice.GetSingle(a => a.PurchaseInvoiceDetailId == purchaseInvoiceDetailId && a.CurrencyId == currencyId && a.Type.Name == priceType)?.MoneyConvertId.Value;
            else
                return _unitOfWork.PurchaseInvoiceDetailSalePrice.GetSingle(a => a.TransferDetailId == purchaseInvoiceDetailId && a.CurrencyId == currencyId && a.Type.Name == priceType)?.MoneyConvertId.Value;
        }




        // Begin Convert 
        public ParentDetailSalePriceViewModel ConvertModel(PurchaseInvoiceDetail detail)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDetail, ParentDetailSalePriceViewModel>()
                .ForMember(a => a.PurchaseInvoiceDetailId, b => b.MapFrom(c => c.Guid))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                .ForMember(a => a.ProductName, b => b.MapFrom(c => $"{c.Product.Name} | {c.Product.ProductType.Name} | {c.Product.Producer.Name}"))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PurchaseInvoiceDetail, ParentDetailSalePriceViewModel>(detail);
        }

        public PurchaseInvoiceDetailViewModel ConvertEditModel(PurchaseInvoiceDetail Users)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDetail, PurchaseInvoiceDetailViewModel>()
                .ForMember(a => a.DiscountTxt, b => b.MapFrom(c => c.Discount.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.NumTxt, b => b.MapFrom(c => c.Num.GetValueOrDefault(1).ToString("0.##", cultures)))
                .ForMember(a => a.PurchasePriceTxt, b => b.MapFrom(c => c.PurchasePrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.SellingPriceTxt, b => b.MapFrom(c => c.SellingPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.FreeNumTxt, b => b.MapFrom(c => c.FreeNum.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.WholeSellPriceTxt, b => b.MapFrom(c => c.WholeSellPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.MiddleSellPriceTxt, b => b.MapFrom(c => c.MiddleSellPrice.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.WholePurchasePriceTxt, b => b.MapFrom(c => (c.Num.GetValueOrDefault(0) * c.PurchasePrice.GetValueOrDefault(0)).ToString("0.##", cultures)))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PurchaseInvoiceDetail, PurchaseInvoiceDetailViewModel>(Users);
        }

        public List<PurchaseInvoiceDetailViewModel> ConvertModelsLists(IEnumerable<PurchaseInvoiceDetail> purchaseInvoiceDetails)
        {
            List<PurchaseInvoiceDetailViewModel> purchaseInvoiceDetailDtoList = new List<PurchaseInvoiceDetailViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDetail, PurchaseInvoiceDetailViewModel>()
               .ForMember(a => a.ProductName, b => b.MapFrom(c => $"{c.Product.Name} | {c.Product.ProductType.Name} | {c.Product.Producer.Name}"))
               .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDetailDtoList = mapper.Map<IEnumerable<PurchaseInvoiceDetail>, List<PurchaseInvoiceDetailViewModel>>(purchaseInvoiceDetails);
            return purchaseInvoiceDetailDtoList;
        }

        public List<PurchaseInvoiceDetailHistoryViewModel> ConvertHistoryModelsLists(IEnumerable<PurchaseInvoiceDetail> purchaseInvoiceDetails)
        {
            List<PurchaseInvoiceDetailHistoryViewModel> purchaseInvoiceDetailDtoList = new List<PurchaseInvoiceDetailHistoryViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDetail, PurchaseInvoiceDetailHistoryViewModel>()
               .ForMember(a => a.Supplier, b => b.MapFrom(c => c.Master.Supplier.User.Name))
               .ForMember(a => a.InvoiceNum, b => b.MapFrom(c => c.Master.InvoiceNum))
               .ForMember(a => a.MainInvoiceNum, b => b.MapFrom(c => c.Master.MainInvoiceNum))
               .ForMember(a => a.Date, b => b.MapFrom(c => c.Master.InvoiceDate))
               .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDetailDtoList = mapper.Map<IEnumerable<PurchaseInvoiceDetail>, List<PurchaseInvoiceDetailHistoryViewModel>>(purchaseInvoiceDetails);
            return purchaseInvoiceDetailDtoList;
        }

        public List<ReturnPurchaseInvoiceDetailSelectViewModel> ConvertSelectModelsLists(IEnumerable<ReturnPurchaseDetailModel> purchaseInvoiceDetails)
        {
            List<ReturnPurchaseInvoiceDetailSelectViewModel> purchaseInvoiceDetailDtoList = new List<ReturnPurchaseInvoiceDetailSelectViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseDetailModel, ReturnPurchaseInvoiceDetailSelectViewModel>()
                //.ForMember(a => a.Supplier, b => b.MapFrom(c => c.Master.Supplier.User.Name))
                //.ForMember(a => a.InvoiceNum, b => b.MapFrom(c => c.Master.InvoiceNum))
                //.ForMember(a => a.MainInvoiceNum, b => b.MapFrom(c => c.Master.MainInvoiceNum))
                //.ForMember(a => a.Date, b => b.MapFrom(c => c.Master.InvoiceDate))
                //.ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                //.ForMember(a => a.SellingCurrencyName, b => b.MapFrom(c => c.SellingCurrency.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            purchaseInvoiceDetailDtoList = mapper.Map<IEnumerable<ReturnPurchaseDetailModel>, List<ReturnPurchaseInvoiceDetailSelectViewModel>>(purchaseInvoiceDetails);
            return purchaseInvoiceDetailDtoList;
        }


        // End Convert
    }
}
