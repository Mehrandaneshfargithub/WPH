using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.MoneyConvert;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class MoneyConvertMvcMockingService : IMoneyConvertMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MoneyConvertMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/MoneyConvert/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public IEnumerable<MoneyConvertViewModel> GetAllMoneyConvertByDate(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                DateFrom = DateTime.Now;
                DateTo = DateTime.Now;

                CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
            }

            IEnumerable<MoneyConvert> moneyConvertDtos = _unitOfWork.MoneyConvert.GetAllMoneyConvertByDate(clinicSectionId, DateFrom, DateTo).OrderByDescending(x => x.Id);

            List<MoneyConvertViewModel> costs = ConvertModelList(moneyConvertDtos);
            Indexing<MoneyConvertViewModel> indexing = new Indexing<MoneyConvertViewModel>();
            return indexing.AddIndexing(costs);
        }

        public decimal? GetMoneyConvertAmountByBaseCurrencyIdAndDestCurrencyId(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId)
        {
            return _unitOfWork.MoneyConvert.GetMoneyConvertAmountByBaseCurrencyIdAndDestCurrencyId(clinicSectionId, baseCurrencyId, destCurrencyId);
        }

        public MoneyConvertViewModel GetMoneyConvertBaseCurrencyName(Guid clinicSectionId, string baseCurrencyName, string destCurrencyName)
        {
            var baseCurrencyId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(baseCurrencyName, "Currency");
            var destCurrencyId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(destCurrencyName, "Currency");


            return GetMoneyConvertBaseCurrencies(clinicSectionId, baseCurrencyId.GetValueOrDefault(0), destCurrencyId.GetValueOrDefault(0));
        }

        public MoneyConvertViewModel GetMoneyConvertBaseCurrencies(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId)
        {
            if (baseCurrencyId == destCurrencyId)
                return new MoneyConvertViewModel
                {
                    Amount = 1,
                    BaseAmount = 1,
                    DestAmount = 1,
                    BaseCurrencyName = "",
                    DestCurrencyName = ""
                };

            var res = _unitOfWork.MoneyConvert.GetMoneyConvertBaseCurrencies(clinicSectionId, baseCurrencyId, destCurrencyId);
            if (res == null)
                return null;

            return ConvertModel(res);
        }

        public IEnumerable<MoneyConvertViewModel> GetLatestMoneyConverts(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId, Guid? moneyConvertId)
        {
            if (baseCurrencyId == destCurrencyId)
                return new List<MoneyConvertViewModel>
                {
                    new MoneyConvertViewModel
                    {
                        Amount = 1,
                        BaseAmount = 1,
                        DestAmount = 1,
                        BaseCurrencyName = "",
                        DestCurrencyName = ""
                    }
                };

            var res = _unitOfWork.MoneyConvert.GetLatestMoneyConverts(clinicSectionId, baseCurrencyId, destCurrencyId).ToList();

            if (moneyConvertId != null)
            {
                var money = _unitOfWork.MoneyConvert.GetMoneyConverts(moneyConvertId.Value);
                if (money != null)
                {
                    res = res.Where(p => p.Guid != moneyConvertId).ToList();

                    res.Add(money);
                }
            }

            return ConvertList(res);
        }

        public IEnumerable<MoneyConvertViewModel> GetLatestMoneyConvertsWithIsMain(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId, Guid? moneyConvertId)
        {
            if (baseCurrencyId == destCurrencyId)
                return new List<MoneyConvertViewModel>
                {
                    new MoneyConvertViewModel
                    {
                        Amount = 1,
                        BaseAmount = 1,
                        DestAmount = 1,
                        BaseCurrencyName = "",
                        DestCurrencyName = ""
                    }
                };

            var res = _unitOfWork.MoneyConvert.GetLatestMoneyConvertsWithIsMain(clinicSectionId, baseCurrencyId, destCurrencyId).ToList();

            if (moneyConvertId != null)
            {
                var money = _unitOfWork.MoneyConvert.GetMoneyConverts(moneyConvertId.Value);
                if (money != null)
                {
                    res = res.Where(p => p.Guid != moneyConvertId).ToList();

                    res.Add(money);
                }
            }

            return ConvertList(res);
        }


        public string AddNewMoneyConvert(MoneyConvertViewModel viewModel)
        {
            if (viewModel.BaseAmount.GetValueOrDefault(0) == 0 || viewModel.DestAmount.GetValueOrDefault(0) == 0 ||
                viewModel.BaseCurrencyId == 0 || viewModel.DestCurrencyId == 0 || viewModel.BaseCurrencyId == viewModel.DestCurrencyId)
                return "DataNotValid";

            //if (!DateTime.TryParseExact(viewModel.DateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime date))
            //    return "DateNotValid";

            MoneyConvert dto = Common.ConvertModels<MoneyConvert, MoneyConvertViewModel>.convertModels(viewModel);
            dto.Date = DateTime.Now;
            dto.IsMain = true;

            _unitOfWork.MoneyConvert.Add(dto);
            _unitOfWork.Complete();
            return dto.Guid.ToString();

        }

        public MoneyConvertViewModel GetMoneyConvertById(Guid id)
        {
            try
            {
                MoneyConvert dto = _unitOfWork.MoneyConvert.Get(id);
                return Common.ConvertModels<MoneyConvertViewModel, MoneyConvert>.convertModels(dto);

            }
            catch (Exception ex) { throw ex; }
        }

        public string UpdateMoneyConvert(MoneyConvertViewModel viewModel)
        {
            if (viewModel.BaseAmount.GetValueOrDefault(0) == 0 || viewModel.DestAmount.GetValueOrDefault(0) == 0 ||
                viewModel.BaseCurrencyId == 0 || viewModel.DestCurrencyId == 0 || viewModel.BaseCurrencyId == viewModel.DestCurrencyId)
                return "DataNotValid";

            //if (!DateTime.TryParseExact(viewModel.DateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime date))
            //    return "DateNotValid";

            MoneyConvert moneyConvert = _unitOfWork.MoneyConvert.Get(viewModel.Guid);
            moneyConvert.BaseAmount = viewModel.BaseAmount;
            moneyConvert.DestAmount = viewModel.DestAmount;
            moneyConvert.BaseCurrencyId = viewModel.BaseCurrencyId;
            moneyConvert.DestCurrencyId = viewModel.DestCurrencyId;
            //moneyConvert.Date = date;

            _unitOfWork.MoneyConvert.UpdateState(moneyConvert);
            _unitOfWork.Complete();
            return moneyConvert.Guid.ToString();

        }

        public OperationStatus RemoveMoneyConvert(Guid MoneyConvertId)
        {
            try
            {
                _unitOfWork.MoneyConvert.RemoveMoneyConvert(MoneyConvertId);
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

        public List<MoneyConvertViewModel> ConvertModelList(IEnumerable<MoneyConvert> moneyConvert)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MoneyConvert, MoneyConvertViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<MoneyConvert>, List<MoneyConvertViewModel>>(moneyConvert);
        }

        public List<MoneyConvertViewModel> ConvertList(IEnumerable<MoneyConvert> moneyConvert)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MoneyConvert, MoneyConvertViewModel>()
                .ForMember(a => a.BaseCurrencyName, b => b.MapFrom(c => c.BaseCurrency.Name))
                .ForMember(a => a.DestCurrencyName, b => b.MapFrom(c => c.DestCurrency.Name))
                .ForMember(a => a.BaseCurrency, b => b.Ignore())
                .ForMember(a => a.DestCurrency, b => b.Ignore())
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<MoneyConvert>, List<MoneyConvertViewModel>>(moneyConvert);
        }

        public MoneyConvertViewModel ConvertModel(MoneyConvert moneyConvert)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MoneyConvert, MoneyConvertViewModel>()
                .ForMember(a => a.BaseCurrencyName, b => b.MapFrom(c => c.BaseCurrency.Name))
                .ForMember(a => a.DestCurrencyName, b => b.MapFrom(c => c.DestCurrency.Name))
                .ForMember(a => a.BaseCurrency, b => b.Ignore())
                .ForMember(a => a.DestCurrency, b => b.Ignore())
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<MoneyConvert, MoneyConvertViewModel>(moneyConvert);
        }

    }

}
