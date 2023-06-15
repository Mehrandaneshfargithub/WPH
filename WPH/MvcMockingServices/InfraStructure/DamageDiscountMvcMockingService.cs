using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.DamageDiscount;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class DamageDiscountMvcMockingService : IDamageDiscountMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public DamageDiscountMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public IEnumerable<DamageDiscountViewModel> GetAllDamageDiscounts(Guid damageId)
        {
            var damageDiscountDtos = _unitOfWork.DamageDiscounts.GetAllDamageDiscounts(damageId).ToList();

            List<DamageDiscountViewModel> damageDiscounts = ConvertModelsLists(damageDiscountDtos).ToList();
            Indexing<DamageDiscountViewModel> indexing = new Indexing<DamageDiscountViewModel>();
            return indexing.AddIndexing(damageDiscounts);
        }

        public string AddNewDamageDiscount(DamageDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.DamageId == null)
                return "DataNotValid";

            var currencyExist = _unitOfWork.Damages.GetForUpdateTotalPrice(viewModel.DamageId.Value);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.DamageDetails.Any(p => p.CurrencyId == viewModel.CurrencyId))
                return "CantAddWithThisCurrency";

            var total_purchase = currencyExist.DamageDetails.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.DamageDiscounts.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_purchase - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            DamageDiscount damageDiscountt = Common.ConvertModels<DamageDiscount, DamageDiscountViewModel>.convertModels(viewModel);
            damageDiscountt.CreateDate = DateTime.Now;

            _unitOfWork.DamageDiscounts.Add(damageDiscountt);
            //_unitOfWork.Complete();
            damageDiscountt.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(damageDiscountt.CurrencyId.Value);
            currencyExist.DamageDiscounts.Add(damageDiscountt);

            return _idunit.damage.UpdateTotalPrice(currencyExist);
        }

        public string UpdateDamageDiscount(DamageDiscountViewModel viewModel)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Amount = decimal.Parse(viewModel.AmountTxt ?? "0", cultures);

            if (viewModel.Amount.GetValueOrDefault(0) <= 0 || viewModel.DamageId == null)
                return "DataNotValid";

            var currencyExist = _unitOfWork.Damages.GetForUpdateTotalPrice(viewModel.DamageId.Value);

            if (currencyExist == null)
                return "DataNotValid";

            if (!currencyExist.DamageDetails.Any(p => p.CurrencyId == viewModel.CurrencyId))
                return "CantAddWithThisCurrency";

            var total_purchase = currencyExist.DamageDetails.Where(p => p.CurrencyId == viewModel.CurrencyId).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
            var total_discount = currencyExist.DamageDiscounts.Where(p => p.CurrencyId == viewModel.CurrencyId && p.Guid != viewModel.Guid).Sum(p => p.Amount.GetValueOrDefault(0));
            var after_discount = total_purchase - total_discount;
            if (after_discount < viewModel.Amount)
                return "DiscountIsGreaterThanTheAmount";

            var damageDiscountt = _unitOfWork.DamageDiscounts.Get(viewModel.Guid);

            damageDiscountt.Amount = viewModel.Amount;
            damageDiscountt.CurrencyId = viewModel.CurrencyId;
            damageDiscountt.Description = viewModel.Description;
            damageDiscountt.ModifiedDate = DateTime.Now;
            damageDiscountt.ModifiedUserId = viewModel.ModifiedUserId;

            _unitOfWork.DamageDiscounts.UpdateState(damageDiscountt);
            //_unitOfWork.Complete();
            currencyExist.DamageDiscounts = currencyExist.DamageDiscounts.Where(p => p.Guid != viewModel.Guid).ToList();
            damageDiscountt.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(damageDiscountt.CurrencyId.Value);
            currencyExist.DamageDiscounts.Add(damageDiscountt);

            return _idunit.damage.UpdateTotalPrice(currencyExist);
        }

        public string RemoveDamageDiscount(Guid damageDiscountId)
        {
            try
            {
                DamageDiscount damageDiscount = _unitOfWork.DamageDiscounts.Get(damageDiscountId);

                _unitOfWork.DamageDiscounts.Remove(damageDiscount);
                //_unitOfWork.Complete();

                var currencyExist = _unitOfWork.Damages.GetForUpdateTotalPrice(damageDiscount.DamageId.Value);
                currencyExist.DamageDiscounts = currencyExist.DamageDiscounts.Where(p => p.Guid != damageDiscountId).ToList();

                return _idunit.damage.UpdateTotalPrice(currencyExist);
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

        public DamageDiscountViewModel GetDamageDiscount(Guid damageDiscountId)
        {
            try
            {
                DamageDiscount damageDiscountDto = _unitOfWork.DamageDiscounts.Get(damageDiscountId);
                return ConvertModels(damageDiscountDto);
            }
            catch { return null; }
        }

        public static DamageDiscountViewModel ConvertModels(DamageDiscount damageDiscount)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DamageDiscount, DamageDiscountViewModel>()
                .ForMember(a => a.AmountTxt, b => b.MapFrom(c => c.Amount.GetValueOrDefault(0).ToString("0.##", cultures)))
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<DamageDiscount, DamageDiscountViewModel>(damageDiscount);

        }

        public static List<DamageDiscountViewModel> ConvertModelsLists(IEnumerable<DamageDiscount> damageDiscounts)
        {
            List<DamageDiscountViewModel> DamageDiscountViewModelList = new List<DamageDiscountViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DamageDiscount, DamageDiscountViewModel>()
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                ;

            });
            IMapper mapper = config.CreateMapper();
            DamageDiscountViewModelList = mapper.Map<IEnumerable<DamageDiscount>, List<DamageDiscountViewModel>>(damageDiscounts);
            return DamageDiscountViewModelList;
        }
    }
}
