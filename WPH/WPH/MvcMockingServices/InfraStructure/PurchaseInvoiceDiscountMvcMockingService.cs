using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.PurchaseInvoiceDiscount;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class PurchaseInvoiceDiscountMvcMockingService : IPurchaseInvoiceDiscountMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public PurchaseInvoiceDiscountMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public IEnumerable<PurchaseInvoiceDiscountViewModel> GetAllPurchaseInvoiceDiscounts(Guid purchaseInvoiceId)
        {
            var purchaseInvoiceDiscountDtos = _unitOfWork.PurchaseInvoiceDiscounts.GetAllPurchaseInvoiceDiscounts(purchaseInvoiceId).ToList();

            List<PurchaseInvoiceDiscountViewModel> purchaseInvoiceDiscounts = ConvertModelsLists(purchaseInvoiceDiscountDtos).ToList();
            Indexing<PurchaseInvoiceDiscountViewModel> indexing = new Indexing<PurchaseInvoiceDiscountViewModel>();
            return indexing.AddIndexing(purchaseInvoiceDiscounts);
        }

        public string AddNewPurchaseInvoiceDiscount(PurchaseInvoiceDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.PurchaseInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(viewModel.PurchaseInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(viewModel.PurchaseInvoiceId.Value);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.PurchaseInvoiceDetails.Any(p => p.CurrencyId == viewModel.CurrencyId))
                return "CantAddWithThisCurrency";

            var total_purchase = currencyExist.PurchaseInvoiceDetails.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Num.GetValueOrDefault(0) * p.PurchasePrice.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.PurchaseInvoiceDiscounts.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_purchase - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            PurchaseInvoiceDiscount purchaseInvoiceDiscountt = Common.ConvertModels<PurchaseInvoiceDiscount, PurchaseInvoiceDiscountViewModel>.convertModels(viewModel);

            _unitOfWork.PurchaseInvoiceDiscounts.Add(purchaseInvoiceDiscountt);
            //_unitOfWork.Complete();
            purchaseInvoiceDiscountt.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(purchaseInvoiceDiscountt.CurrencyId.Value);
            currencyExist.PurchaseInvoiceDiscounts.Add(purchaseInvoiceDiscountt);

            return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
        }

        public string UpdatePurchaseInvoiceDiscount(PurchaseInvoiceDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.PurchaseInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(viewModel.PurchaseInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(viewModel.PurchaseInvoiceId.Value);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.PurchaseInvoiceDetails.Any(p => p.CurrencyId == viewModel.CurrencyId))
                return "CantAddWithThisCurrency";

            var total_purchase = currencyExist.PurchaseInvoiceDetails.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Num.GetValueOrDefault(0) * p.PurchasePrice.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.PurchaseInvoiceDiscounts.Where(p => p.CurrencyId == viewModel.CurrencyId && p.Guid != viewModel.Guid).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_purchase - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            var purchaseInvoiceDiscountt = _unitOfWork.PurchaseInvoiceDiscounts.Get(viewModel.Guid);

            purchaseInvoiceDiscountt.Amount = viewModel.Amount;
            purchaseInvoiceDiscountt.CurrencyId = viewModel.CurrencyId;
            purchaseInvoiceDiscountt.Description = viewModel.Description;
            purchaseInvoiceDiscountt.ModifiedDate = viewModel.ModifiedDate;
            purchaseInvoiceDiscountt.ModifiedUserId = viewModel.ModifiedUserId;

            _unitOfWork.PurchaseInvoiceDiscounts.UpdateState(purchaseInvoiceDiscountt);
            //_unitOfWork.Complete();
            currencyExist.PurchaseInvoiceDiscounts = currencyExist.PurchaseInvoiceDiscounts.Where(p => p.Guid != viewModel.Guid).ToList();
            purchaseInvoiceDiscountt.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(purchaseInvoiceDiscountt.CurrencyId.Value);
            currencyExist.PurchaseInvoiceDiscounts.Add(purchaseInvoiceDiscountt);

            return _idunit.purchaseInvoice.UpdateTotalPrice(currencyExist);
        }

        public string RemovePurchaseInvoiceDiscount(Guid purchaseInvoiceDiscountId)
        {
            try
            {
                PurchaseInvoiceDiscount purchaseInvoiceDiscount = _unitOfWork.PurchaseInvoiceDiscounts.Get(purchaseInvoiceDiscountId);

                var can_change = _unitOfWork.PurchaseInvoicePays.CheckPurchaseInvoicePaid(purchaseInvoiceDiscount.PurchaseInvoiceId.Value);
                if (can_change)
                    return "InvoiceInUse";

                _unitOfWork.PurchaseInvoiceDiscounts.Remove(purchaseInvoiceDiscount);
                //_unitOfWork.Complete();

                var currencyExist = _unitOfWork.PurchaseInvoices.GetForUpdateTotalPrice(purchaseInvoiceDiscount.PurchaseInvoiceId.Value);
                currencyExist.PurchaseInvoiceDiscounts = currencyExist.PurchaseInvoiceDiscounts.Where(p => p.Guid != purchaseInvoiceDiscountId).ToList();

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

        public PurchaseInvoiceDiscountViewModel GetPurchaseInvoiceDiscount(Guid purchaseInvoiceDiscountId)
        {
            try
            {
                PurchaseInvoiceDiscount purchaseInvoiceDiscountDto = _unitOfWork.PurchaseInvoiceDiscounts.Get(purchaseInvoiceDiscountId);
                return ConvertModels(purchaseInvoiceDiscountDto);
            }
            catch { return null; }
        }

        public static PurchaseInvoiceDiscountViewModel ConvertModels(PurchaseInvoiceDiscount purchaseInvoiceDiscount)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDiscount, PurchaseInvoiceDiscountViewModel>()
                .ForMember(a => a.AmountTxt, b => b.MapFrom(c => c.Amount.GetValueOrDefault(0).ToString("0.##", cultures)))
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<PurchaseInvoiceDiscount, PurchaseInvoiceDiscountViewModel>(purchaseInvoiceDiscount);

        }

        public static List<PurchaseInvoiceDiscountViewModel> ConvertModelsLists(IEnumerable<PurchaseInvoiceDiscount> purchaseInvoiceDiscounts)
        {
            List<PurchaseInvoiceDiscountViewModel> PurchaseInvoiceDiscountViewModelList = new List<PurchaseInvoiceDiscountViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseInvoiceDiscount, PurchaseInvoiceDiscountViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                ;

            });
            IMapper mapper = config.CreateMapper();
            PurchaseInvoiceDiscountViewModelList = mapper.Map<IEnumerable<PurchaseInvoiceDiscount>, List<PurchaseInvoiceDiscountViewModel>>(purchaseInvoiceDiscounts);
            return PurchaseInvoiceDiscountViewModelList;
        }
    }
}
