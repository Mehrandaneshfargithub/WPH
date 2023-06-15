using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class TotalReservesMvcMockingService : ITotalReservesMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idiunit;

        public TotalReservesMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idiunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idiunit = idiunit;
        }

        public List<ReserveDetailViewModel> GetAllReserveByDoctorIdForSpecificDate(Guid doctorId, int periodId, DateTime dateFrom, DateTime dateTo)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                dateFrom = DateTime.Now;
                dateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref dateFrom, ref dateTo, periodId);
            }
            List<ReserveDetail> visitDtos = _unitOfWork.ReserveDetails.GetAllReservesBetweenTwoDateByDocotrId(doctorId, dateFrom, dateTo).ToList();

            List<ReserveDetailViewModel> visits = ConvertModelsLists(visitDtos).ToList();
            Indexing<ReserveDetailViewModel> indexing = new Indexing<ReserveDetailViewModel>();
            return indexing.AddIndexing(visits);
        }

        public List<ReserveDetailViewModel> GetAllReserveForSpecificDateBasedOnUserAccess(List<Guid> doctors, int periodId, DateTime dateFrom, DateTime dateTo)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                dateFrom = DateTime.Now;
                dateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref dateFrom, ref dateTo, periodId);
            }
            List<ReserveDetail> visitDtos = _unitOfWork.ReserveDetails.GetAllReservesBetweenTwoDateBasedOnUserAccess(doctors, dateFrom, dateTo).ToList();

            List<ReserveDetailViewModel> visits = ConvertModelsLists(visitDtos).ToList();
            Indexing<ReserveDetailViewModel> indexing = new Indexing<ReserveDetailViewModel>();
            return indexing.AddIndexing(visits);
        }

        public async Task<string> ConvertToVisit(Guid reserveDetailId, Guid clinicSectionId, Guid userId)
        {
            var reserve = _unitOfWork.ReserveDetails.GetWithReception(reserveDetailId);

            var visitedId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Visited", "VisitStatus");
            var reception_exist = reserve.Receptions != null && !reserve.Receptions.Any();
            if (reserve.StatusId == visitedId && reception_exist)
                return "Visited";

            if (reception_exist)
            {
                var serviceId = _unitOfWork.Services.GetServiceByName(null, "DoctorVisit")?.Guid;
                var sval = _idiunit.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "VisitPrice").FirstOrDefault();
                var today = DateTime.Now;

                Reception reception = new Reception
                {
                    Guid = Guid.NewGuid(),
                    ReceptionNum = _idiunit.patientReception.GetLatestReceptionInvoiceNum(clinicSectionId),
                    ReceptionDate = today,
                    ClinicSectionId = clinicSectionId,
                    ReceptionTypeId = _idiunit.baseInfo.GetIdByNameAndType("VisitReception", "ReceptionType"),
                    PatientId = reserve.PatientId,
                    Discount = 0,
                    CreatedUserId = userId,
                    CreatedDate = today,
                    ReserveDetailId = reserve.Guid,
                    StatusId = visitedId,
                };

                ReceptionService service = new()
                {
                    ServiceId = serviceId,
                    ReceptionId = reception.Guid,
                    Number = 1,
                    Price = decimal.Parse(sval?.SValue ?? "0"),
                    StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus"),
                    CreatedDate = today,
                    ServiceDate = today,
                    CreatedUserId = userId
                };

                if (serviceId == null || serviceId == Guid.Empty)
                {
                    service.Service = new Service
                    {
                        Name = "DoctorVisit",
                        Price = 1,
                        TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Other", "ServiceType")

                    };
                }

                _unitOfWork.Receptions.Add(reception);
                _unitOfWork.ReceptionServices.Add(service);
            }
            else
            {
                var reception = reserve.Receptions.First();
                reception.StatusId = visitedId;

                _unitOfWork.Receptions.UpdateState(reception);
            }

            reserve.StatusId = visitedId;
            _unitOfWork.ReserveDetails.UpdateState(reserve);

            _unitOfWork.Complete();
            return "";
        }

        public static List<ReserveDetailViewModel> ConvertModelsLists(IEnumerable<ReserveDetail> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReserveDetail, ReserveDetailViewModel>()
                .ForMember(a => a.Reserve, b => b.Ignore())
                .ForMember(a => a.Visits, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                .ForMember(a => a.BaseInfoGeneral, b => b.Ignore())
                .ForMember(a => a.BloodType, b => b.Ignore())
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();

            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<ReserveDetail>, List<ReserveDetailViewModel>>(ress);

        }
    }


}
