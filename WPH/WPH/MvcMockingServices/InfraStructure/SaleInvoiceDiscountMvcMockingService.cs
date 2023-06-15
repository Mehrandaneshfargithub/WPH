using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.SaleInvoiceDiscount;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class SaleInvoiceDiscountMvcMockingService : ISaleInvoiceDiscountMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public SaleInvoiceDiscountMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public IEnumerable<SaleInvoiceDiscountViewModel> GetAllSaleInvoiceDiscounts(Guid SaleInvoiceId)
        {
            var SaleInvoiceDiscountDtos = _unitOfWork.SaleInvoiceDiscounts.GetAllSaleInvoiceDiscounts(SaleInvoiceId).ToList();

            List<SaleInvoiceDiscountViewModel> SaleInvoiceDiscounts = ConvertModelsLists(SaleInvoiceDiscountDtos).ToList();
            Indexing<SaleInvoiceDiscountViewModel> indexing = new Indexing<SaleInvoiceDiscountViewModel>();
            return indexing.AddIndexing(SaleInvoiceDiscounts);
        }

        public string AddNewSaleInvoiceDiscount(SaleInvoiceDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.SaleInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(viewModel.SaleInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var currencyExist = _unitOfWork.SaleInvoices.CheckForCurrency(viewModel.SaleInvoiceId.Value, viewModel.CurrencyId);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.SaleInvoiceDetails.Any())
                return "CantAddWithThisCurrency";

            var total_Sale = currencyExist.SaleInvoiceDetails.Sum(p => p.Num.GetValueOrDefault(0) * p.SalePrice.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.SaleInvoiceDiscounts.Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_Sale - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            SaleInvoiceDiscount SaleInvoiceDiscountt = Common.ConvertModels<SaleInvoiceDiscount, SaleInvoiceDiscountViewModel>.convertModels(viewModel);

            _unitOfWork.SaleInvoiceDiscounts.Add(SaleInvoiceDiscountt);
            //_unitOfWork.Complete();
            var CurrencyName = _unitOfWork.BaseInfoGenerals.GetSingle(a => a.Id == viewModel.CurrencyId).Name;
            return _idunit.saleInvoice.UpdateTotalPrice(null, SaleInvoiceDiscountt, "add", CurrencyName);
        }

        public string UpdateSaleInvoiceDiscount(SaleInvoiceDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) < 0 || viewModel.SaleInvoiceId == null)
                return "DataNotValid";

            var can_change = _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(viewModel.SaleInvoiceId.Value);
            if (can_change)
                return "InvoiceInUse";

            var currencyExist = _unitOfWork.SaleInvoices.CheckForCurrency(viewModel.SaleInvoiceId.Value, viewModel.CurrencyId);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.SaleInvoiceDetails.Any())
                return "CantAddWithThisCurrency";

            var total_Sale = currencyExist.SaleInvoiceDetails.Sum(p => p.Num.GetValueOrDefault(0) * p.SalePrice.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.SaleInvoiceDiscounts.Where(p => p.Guid != viewModel.Guid).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_Sale - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            var SaleInvoiceDiscountt = _unitOfWork.SaleInvoiceDiscounts.Get(viewModel.Guid);

            SaleInvoiceDiscountt.Amount = viewModel.Amount;
            SaleInvoiceDiscountt.CurrencyId = viewModel.CurrencyId;
            SaleInvoiceDiscountt.Description = viewModel.Description;
            SaleInvoiceDiscountt.ModifiedDate = viewModel.ModifiedDate;
            SaleInvoiceDiscountt.ModifiedUserId = viewModel.ModifiedUserId;

            _unitOfWork.SaleInvoiceDiscounts.UpdateState(SaleInvoiceDiscountt);
            //_unitOfWork.Complete();
            var CurrencyName = _unitOfWork.BaseInfoGenerals.GetSingle(a => a.Id == viewModel.CurrencyId).Name;
            return _idunit.saleInvoice.UpdateTotalPrice(null, SaleInvoiceDiscountt, "update", CurrencyName);
        }

        public string RemoveSaleInvoiceDiscount(Guid SaleInvoiceDiscountId)
        {
            try
            {
                SaleInvoiceDiscount SaleInvoiceDiscount = _unitOfWork.SaleInvoiceDiscounts.Get(SaleInvoiceDiscountId);

                var can_change = _unitOfWork.SaleInvoiceReceives.CheckSaleInvoiceInUse(SaleInvoiceDiscount.SaleInvoiceId.Value);
                if (can_change)
                    return "InvoiceInUse";

                _unitOfWork.SaleInvoiceDiscounts.Remove(SaleInvoiceDiscount);
                //_unitOfWork.Complete();

                return _idunit.saleInvoice.UpdateTotalPrice(null, SaleInvoiceDiscount, "remove", "");
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

        public SaleInvoiceDiscountViewModel GetSaleInvoiceDiscountByCurrencyId(Guid saleInvoiceId, int currencyId)
        {
            SaleInvoiceDiscount sale = _unitOfWork.SaleInvoiceDiscounts.GetSaleInvoiceDiscountByCurrencyId(saleInvoiceId, currencyId);
            return Common.ConvertModels<SaleInvoiceDiscountViewModel, SaleInvoiceDiscount>.convertModels(sale);
        }

        public SaleInvoiceDiscountViewModel GetSaleInvoiceDiscount(Guid saleInvoiceDiscountId)
        {
            try
            {
                SaleInvoiceDiscount saleInvoiceDiscountDto = _unitOfWork.SaleInvoiceDiscounts.Get(saleInvoiceDiscountId);
                return ConvertModelsLists(saleInvoiceDiscountDto);
            }
            catch { return null; }
        }

        public static SaleInvoiceDiscountViewModel ConvertModelsLists(SaleInvoiceDiscount saleInvoiceDiscount)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaleInvoiceDiscount, SaleInvoiceDiscountViewModel>()
                .ForMember(a => a.AmountTxt, b => b.MapFrom(c => c.Amount.GetValueOrDefault(0).ToString("0.##", cultures)))
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<SaleInvoiceDiscount, SaleInvoiceDiscountViewModel>(saleInvoiceDiscount);

        }

        public static List<SaleInvoiceDiscountViewModel> ConvertModelsLists(IEnumerable<SaleInvoiceDiscount> SaleInvoiceDiscounts)
        {
            List<SaleInvoiceDiscountViewModel> SaleInvoiceDiscountViewModelList = new List<SaleInvoiceDiscountViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaleInvoiceDiscount, SaleInvoiceDiscountViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                ;

            });
            IMapper mapper = config.CreateMapper();
            SaleInvoiceDiscountViewModelList = mapper.Map<IEnumerable<SaleInvoiceDiscount>, List<SaleInvoiceDiscountViewModel>>(SaleInvoiceDiscounts);
            return SaleInvoiceDiscountViewModelList;
        }

        
    }
}
