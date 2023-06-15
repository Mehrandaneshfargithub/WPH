using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ReturnPurchaseInvoiceDiscount;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReturnPurchaseInvoiceDiscountMvcMockingService : IReturnPurchaseInvoiceDiscountMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ReturnPurchaseInvoiceDiscountMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public IEnumerable<ReturnPurchaseInvoiceDiscountViewModel> GetAllReturnPurchaseInvoiceDiscounts(Guid purchaseInvoiceId)
        {
            var purchaseInvoiceDiscountDtos = _unitOfWork.ReturnPurchaseInvoiceDiscounts.GetAllReturnPurchaseInvoiceDiscounts(purchaseInvoiceId).ToList();

            List<ReturnPurchaseInvoiceDiscountViewModel> purchaseInvoiceDiscounts = ConvertModelsLists(purchaseInvoiceDiscountDtos).ToList();
            Indexing<ReturnPurchaseInvoiceDiscountViewModel> indexing = new Indexing<ReturnPurchaseInvoiceDiscountViewModel>();
            return indexing.AddIndexing(purchaseInvoiceDiscounts);
        }

        public string AddNewReturnPurchaseInvoiceDiscount(ReturnPurchaseInvoiceDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.ReturnPurchaseInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(viewModel.ReturnPurchaseInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var currencyExist = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(viewModel.ReturnPurchaseInvoiceId.Value);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.ReturnPurchaseInvoiceDetails.Any(p => p.CurrencyId == viewModel.CurrencyId))
                return "CantAddWithThisCurrency";

            var total_purchase = currencyExist.ReturnPurchaseInvoiceDetails.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.ReturnPurchaseInvoiceDiscounts.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_purchase - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            ReturnPurchaseInvoiceDiscount purchaseInvoiceDiscountt = Common.ConvertModels<ReturnPurchaseInvoiceDiscount, ReturnPurchaseInvoiceDiscountViewModel>.convertModels(viewModel);

            _unitOfWork.ReturnPurchaseInvoiceDiscounts.Add(purchaseInvoiceDiscountt);
            //_unitOfWork.Complete();
            purchaseInvoiceDiscountt.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(purchaseInvoiceDiscountt.CurrencyId.Value);
            currencyExist.ReturnPurchaseInvoiceDiscounts.Add(purchaseInvoiceDiscountt);

            return _idunit.returnPurchaseInvoice.UpdateTotalPrice(currencyExist);
        }

        public string UpdateReturnPurchaseInvoiceDiscount(ReturnPurchaseInvoiceDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.ReturnPurchaseInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(viewModel.ReturnPurchaseInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var currencyExist = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(viewModel.ReturnPurchaseInvoiceId.Value);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.ReturnPurchaseInvoiceDetails.Any(p => p.CurrencyId == viewModel.CurrencyId))
                return "CantAddWithThisCurrency";

            var total_purchase = currencyExist.ReturnPurchaseInvoiceDetails.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.ReturnPurchaseInvoiceDiscounts.Where(p => p.CurrencyId == viewModel.CurrencyId && p.Guid != viewModel.Guid).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_purchase - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            var purchaseInvoiceDiscountt = _unitOfWork.ReturnPurchaseInvoiceDiscounts.Get(viewModel.Guid);

            purchaseInvoiceDiscountt.Amount = viewModel.Amount;
            purchaseInvoiceDiscountt.CurrencyId = viewModel.CurrencyId;
            purchaseInvoiceDiscountt.Description = viewModel.Description;
            purchaseInvoiceDiscountt.ModifiedDate = viewModel.ModifiedDate;
            purchaseInvoiceDiscountt.ModifiedUserId = viewModel.ModifiedUserId;

            _unitOfWork.ReturnPurchaseInvoiceDiscounts.UpdateState(purchaseInvoiceDiscountt);
            //_unitOfWork.Complete();
            currencyExist.ReturnPurchaseInvoiceDiscounts = currencyExist.ReturnPurchaseInvoiceDiscounts.Where(p => p.Guid != viewModel.Guid).ToList();
            purchaseInvoiceDiscountt.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(purchaseInvoiceDiscountt.CurrencyId.Value);
            currencyExist.ReturnPurchaseInvoiceDiscounts.Add(purchaseInvoiceDiscountt);

            return _idunit.returnPurchaseInvoice.UpdateTotalPrice(currencyExist);
        }

        public string RemoveReturnPurchaseInvoiceDiscount(Guid purchaseInvoiceDiscountId)
        {
            try
            {
                ReturnPurchaseInvoiceDiscount purchaseInvoiceDiscount = _unitOfWork.ReturnPurchaseInvoiceDiscounts.Get(purchaseInvoiceDiscountId);

                var can_change = _unitOfWork.ReturnPurchaseInvoicePays.CheckReturnPurchaseInvoicePaid(purchaseInvoiceDiscount.ReturnPurchaseInvoiceId.Value);
                if (can_change)
                    return "InvoiceInUse";

                _unitOfWork.ReturnPurchaseInvoiceDiscounts.Remove(purchaseInvoiceDiscount);
                //_unitOfWork.Complete();

                var currencyExist = _unitOfWork.ReturnPurchaseInvoices.GetForUpdateTotalPrice(purchaseInvoiceDiscount.ReturnPurchaseInvoiceId.Value);
                currencyExist.ReturnPurchaseInvoiceDiscounts = currencyExist.ReturnPurchaseInvoiceDiscounts.Where(p => p.Guid != purchaseInvoiceDiscountId).ToList();

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

        public ReturnPurchaseInvoiceDiscountViewModel GetReturnPurchaseInvoiceDiscount(Guid purchaseInvoiceDiscountId)
        {
            try
            {
                ReturnPurchaseInvoiceDiscount purchaseInvoiceDiscountDto = _unitOfWork.ReturnPurchaseInvoiceDiscounts.Get(purchaseInvoiceDiscountId);
                return ConvertModels(purchaseInvoiceDiscountDto);
            }
            catch { return null; }
        }

        public static ReturnPurchaseInvoiceDiscountViewModel ConvertModels(ReturnPurchaseInvoiceDiscount purchaseInvoiceDiscount)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseInvoiceDiscount, ReturnPurchaseInvoiceDiscountViewModel>()
                .ForMember(a => a.AmountTxt, b => b.MapFrom(c => c.Amount.GetValueOrDefault(0).ToString("0.##", cultures)))
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnPurchaseInvoiceDiscount, ReturnPurchaseInvoiceDiscountViewModel>(purchaseInvoiceDiscount);
            
        }
        public static List<ReturnPurchaseInvoiceDiscountViewModel> ConvertModelsLists(IEnumerable<ReturnPurchaseInvoiceDiscount> purchaseInvoiceDiscounts)
        {
            List<ReturnPurchaseInvoiceDiscountViewModel> ReturnPurchaseInvoiceDiscountViewModelList = new List<ReturnPurchaseInvoiceDiscountViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnPurchaseInvoiceDiscount, ReturnPurchaseInvoiceDiscountViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                ;

            });
            IMapper mapper = config.CreateMapper();
            ReturnPurchaseInvoiceDiscountViewModelList = mapper.Map<IEnumerable<ReturnPurchaseInvoiceDiscount>, List<ReturnPurchaseInvoiceDiscountViewModel>>(purchaseInvoiceDiscounts);
            return ReturnPurchaseInvoiceDiscountViewModelList;
        }
    }
}
