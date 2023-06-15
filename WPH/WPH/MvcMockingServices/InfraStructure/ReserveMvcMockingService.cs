using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.Reserve;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class ReserveMvcMockingService : IReserveMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idiunit;

        public ReserveMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idiunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idiunit = idiunit;
        }

        public ReserveViewModel GetDate(DateTime date, Guid ClinicSectionId)
        {
            Reserve resDto = _unitOfWork.Reserves.GetDate(date, ClinicSectionId);
            return Common.ConvertModels<ReserveViewModel, Reserve>.convertModels(resDto);

        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Reserves/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.AccessLink = controllerName + "AccessModal?Id=";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public Guid UpdateReserve(ReserveViewModel reserve)
        {
            try
            {
                Reserve updatedReserve = Common.ConvertModels<Reserve, ReserveViewModel>.convertModels(reserve);
                Reserve oldReserve = _unitOfWork.Reserves.Get(updatedReserve.Guid);
                _unitOfWork.Reserves.Detach(oldReserve);
                _unitOfWork.Reserves.UpdateState(updatedReserve);
                _unitOfWork.Complete();
                return oldReserve.Guid;
            }
            catch (Exception ex) { throw ex; }
        }


        public Guid AddReserve(ReserveViewModel today)
        {
            try
            {
                Reserve reseDto = Common.ConvertModels<Reserve, ReserveViewModel>.convertModels(today);
                _unitOfWork.Reserves.Add(reseDto);
                _unitOfWork.Complete();
                return reseDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public ReserveViewModel CheckAndAddReserve(Guid clinicSectionId, DateTime? day)
        {
            DateTime Today;
            if (day == null)
                Today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            else
                Today = new DateTime(day.Value.Year, day.Value.Month, day.Value.Day, 0, 0, 0);

            ReserveViewModel todayExisit = _idiunit.reserve.GetDate(Today, clinicSectionId);

            if (todayExisit != null)
                return todayExisit;

            ReserveViewModel today = new ReserveViewModel
            {
                Guid = Guid.NewGuid(),
                Date = Today
            };
            ReserveViewModel pastWeek = _idiunit.reserve.GetDate(today.Date.AddDays(-7), clinicSectionId);
            if (pastWeek != null)
            {
                today.StartTime = pastWeek.StartTime;
                today.EndTime = pastWeek.EndTime;
                today.RoundTime = pastWeek.RoundTime;
                today.ClinicSectionId = clinicSectionId;
                _idiunit.reserve.AddReserve(today);
                return today;
            }

            ReserveViewModel yesterday = _idiunit.reserve.GetDate(today.Date.AddDays(-1), clinicSectionId);
            if (yesterday != null)
            {
                today.StartTime = yesterday.StartTime;
                today.EndTime = yesterday.EndTime;
                today.RoundTime = yesterday.RoundTime;
                today.ClinicSectionId = clinicSectionId;
                _idiunit.reserve.AddReserve(today);
                return today;
            }

            today.StartTime = new TimeSpan(14, 0, 0);
            today.EndTime = new TimeSpan(17, 0, 0);
            today.RoundTime = 15;
            today.ClinicSectionId = clinicSectionId;
            _idiunit.reserve.AddReserve(today);

            return today;
        }

        public async Task<string> CheckLastPatientVisit(Guid reserveDetailId, Guid clinicSectionId)
        {
            var sval = _idiunit.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "MaxDaysBetweenTwoVisit").FirstOrDefault();

            int day = Convert.ToInt32(sval?.SValue);
            if (day == 0)
                return "";

            var reserve = _unitOfWork.ReserveDetails.Get(reserveDetailId);
            if (reserve == null || reserve.LastVisit.GetValueOrDefault(false))
                return "";

            var last = _unitOfWork.ReserveDetails.GetLastReserveDetail(reserveDetailId, reserve.PatientId);
            if (last == null || last.LastVisit.GetValueOrDefault(false))
                return "";

            var diff = (reserve.ReserveDate.Value - last.ReserveDate.Value).TotalDays;
            if (diff <= day)
            {
                reserve.LastVisit = true;
                _unitOfWork.ReserveDetails.UpdateState(reserve);
                _unitOfWork.Complete();

                return "CheckPay";
            }

            return "";
        }

        public void RemoveReserveVisitPrice(Guid reserveDetailId)
        {
            var result = _unitOfWork.ReceptionServices.GetVisitPriceByReserveDetailId(reserveDetailId);

            _unitOfWork.ReceptionServiceReceiveds.RemoveRange(result.SelectMany(p => p.ReceptionServiceReceiveds));

            _unitOfWork.ReceptionServices.RemoveRange(result);
            _unitOfWork.Complete();
        }

        public static List<ReserveViewModel> ConvertModelsLists(IEnumerable<Reserve> meds)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reserve, ReserveViewModel>();
                cfg.CreateMap<ReserveDetail, ReserveDetailViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Reserve>, List<ReserveViewModel>>(meds);

        }
    }

}
