using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.ReturnSaleInvoiceDiscount;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReturnSaleInvoiceDiscountMvcMockingService : IReturnSaleInvoiceDiscountMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ReturnSaleInvoiceDiscountMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public IEnumerable<ReturnSaleInvoiceDiscountViewModel> GetAllReturnSaleInvoiceDiscounts(Guid saleInvoiceId)
        {
            var saleInvoiceDiscountDtos = _unitOfWork.ReturnSaleInvoiceDiscounts.GetAllReturnSaleInvoiceDiscounts(saleInvoiceId).ToList();

            List<ReturnSaleInvoiceDiscountViewModel> saleInvoiceDiscounts = ConvertModelsLists(saleInvoiceDiscountDtos).ToList();
            Indexing<ReturnSaleInvoiceDiscountViewModel> indexing = new Indexing<ReturnSaleInvoiceDiscountViewModel>();
            return indexing.AddIndexing(saleInvoiceDiscounts);
        }

        public string AddNewReturnSaleInvoiceDiscount(ReturnSaleInvoiceDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.ReturnSaleInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(viewModel.ReturnSaleInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var currencyExist = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(viewModel.ReturnSaleInvoiceId.Value);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.ReturnSaleInvoiceDetails.Any(p => p.CurrencyId == viewModel.CurrencyId))
                return "CantAddWithThisCurrency";

            var total_sale = currencyExist.ReturnSaleInvoiceDetails.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.ReturnSaleInvoiceDiscounts.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_sale - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            ReturnSaleInvoiceDiscount saleInvoiceDiscountt = Common.ConvertModels<ReturnSaleInvoiceDiscount, ReturnSaleInvoiceDiscountViewModel>.convertModels(viewModel);

            _unitOfWork.ReturnSaleInvoiceDiscounts.Add(saleInvoiceDiscountt);
            //_unitOfWork.Complete();
            saleInvoiceDiscountt.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(saleInvoiceDiscountt.CurrencyId.Value);
            currencyExist.ReturnSaleInvoiceDiscounts.Add(saleInvoiceDiscountt);

            return _idunit.returnSaleInvoice.UpdateTotalPrice(currencyExist);
        }

        public string UpdateReturnSaleInvoiceDiscount(ReturnSaleInvoiceDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.ReturnSaleInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(viewModel.ReturnSaleInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var currencyExist = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(viewModel.ReturnSaleInvoiceId.Value);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.ReturnSaleInvoiceDetails.Any(p => p.CurrencyId == viewModel.CurrencyId))
                return "CantAddWithThisCurrency";

            var total_sale = currencyExist.ReturnSaleInvoiceDetails.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.ReturnSaleInvoiceDiscounts.Where(p => p.CurrencyId == viewModel.CurrencyId && p.Guid != viewModel.Guid).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_sale - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            var saleInvoiceDiscountt = _unitOfWork.ReturnSaleInvoiceDiscounts.Get(viewModel.Guid);

            saleInvoiceDiscountt.Amount = viewModel.Amount;
            saleInvoiceDiscountt.CurrencyId = viewModel.CurrencyId;
            saleInvoiceDiscountt.Description = viewModel.Description;
            saleInvoiceDiscountt.ModifiedDate = viewModel.ModifiedDate;
            saleInvoiceDiscountt.ModifiedUserId = viewModel.ModifiedUserId;

            _unitOfWork.ReturnSaleInvoiceDiscounts.UpdateState(saleInvoiceDiscountt);
            //_unitOfWork.Complete();
            currencyExist.ReturnSaleInvoiceDiscounts = currencyExist.ReturnSaleInvoiceDiscounts.Where(p => p.Guid != viewModel.Guid).ToList();
            saleInvoiceDiscountt.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(saleInvoiceDiscountt.CurrencyId.Value);
            currencyExist.ReturnSaleInvoiceDiscounts.Add(saleInvoiceDiscountt);

            return _idunit.returnSaleInvoice.UpdateTotalPrice(currencyExist);
        }

        public string RemoveReturnSaleInvoiceDiscount(Guid saleInvoiceDiscountId)
        {
            try
            {
                ReturnSaleInvoiceDiscount saleInvoiceDiscount = _unitOfWork.ReturnSaleInvoiceDiscounts.Get(saleInvoiceDiscountId);

                var can_change = _unitOfWork.ReturnSaleInvoiceReceives.CheckReturnSaleInvoiceReceived(saleInvoiceDiscount.ReturnSaleInvoiceId.Value);
                if (can_change)
                    return "InvoiceInUse";

                _unitOfWork.ReturnSaleInvoiceDiscounts.Remove(saleInvoiceDiscount);
                //_unitOfWork.Complete();

                var currencyExist = _unitOfWork.ReturnSaleInvoices.GetForUpdateTotalPrice(saleInvoiceDiscount.ReturnSaleInvoiceId.Value);
                currencyExist.ReturnSaleInvoiceDiscounts = currencyExist.ReturnSaleInvoiceDiscounts.Where(p => p.Guid != saleInvoiceDiscountId).ToList();

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

        public ReturnSaleInvoiceDiscountViewModel GetReturnSaleInvoiceDiscount(Guid saleInvoiceDiscountId)
        {
            try
            {
                ReturnSaleInvoiceDiscount saleInvoiceDiscountDto = _unitOfWork.ReturnSaleInvoiceDiscounts.Get(saleInvoiceDiscountId);
                return ConvertModels(saleInvoiceDiscountDto);
            }
            catch { return null; }
        }

        public static ReturnSaleInvoiceDiscountViewModel ConvertModels(ReturnSaleInvoiceDiscount saleInvoiceDiscount)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnSaleInvoiceDiscount, ReturnSaleInvoiceDiscountViewModel>()
                .ForMember(a => a.AmountTxt, b => b.MapFrom(c => c.Amount.GetValueOrDefault(0).ToString("0.##", cultures)))
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReturnSaleInvoiceDiscount, ReturnSaleInvoiceDiscountViewModel>(saleInvoiceDiscount);
            
        }
        public static List<ReturnSaleInvoiceDiscountViewModel> ConvertModelsLists(IEnumerable<ReturnSaleInvoiceDiscount> saleInvoiceDiscounts)
        {
            List<ReturnSaleInvoiceDiscountViewModel> ReturnSaleInvoiceDiscountViewModelList = new List<ReturnSaleInvoiceDiscountViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnSaleInvoiceDiscount, ReturnSaleInvoiceDiscountViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                ;

            });
            IMapper mapper = config.CreateMapper();
            ReturnSaleInvoiceDiscountViewModelList = mapper.Map<IEnumerable<ReturnSaleInvoiceDiscount>, List<ReturnSaleInvoiceDiscountViewModel>>(saleInvoiceDiscounts);
            return ReturnSaleInvoiceDiscountViewModelList;
        }
    }
}
