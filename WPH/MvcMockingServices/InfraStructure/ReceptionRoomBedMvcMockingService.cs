using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.FunctionModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Dashboard;
using WPH.Models.ReceptionRoomBed;
using WPH.MvcMockingServices.Interface;
using WPH.Views.Shared.PartialViews.AppWebForms.Home;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class ReceptionRoomBedMvcMockingService : IReceptionRoomBedMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReceptionRoomBedMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveReceptionRoomBed(Guid ReceptionRoomBedid)
        {
            try
            {
                ReceptionRoomBed Hos = _unitOfWork.ReceptionRoomBeds.Get(ReceptionRoomBedid);
                _unitOfWork.ReceptionRoomBeds.Remove(Hos);
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

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/ReceptionRoomBed/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }
        public Guid AddNewReceptionRoomBed(ReceptionRoomBedViewModel ReceptionRoomBed)
        {
            try
            {
                ReceptionRoomBed ReceptionRoomBed1 = Common.ConvertModels<ReceptionRoomBed, ReceptionRoomBedViewModel>.convertModels(ReceptionRoomBed);
                ReceptionRoomBed1.Guid = Guid.NewGuid();
                _unitOfWork.ReceptionRoomBeds.Add(ReceptionRoomBed1);
                _unitOfWork.Complete();
                return ReceptionRoomBed1.Guid;
            }
            catch (Exception ex) { throw ex; }
        }



        public Guid UpdateReceptionRoomBed(ReceptionRoomBedViewModel hosp)
        {
            try
            {
                ReceptionRoomBed ReceptionRoomBed2 = Common.ConvertModels<ReceptionRoomBed, ReceptionRoomBedViewModel>.convertModels(hosp);
                _unitOfWork.ReceptionRoomBeds.UpdateState(ReceptionRoomBed2);
                _unitOfWork.Complete();
                return ReceptionRoomBed2.Guid;
            }
            catch (Exception ex) { throw ex; }

        }

        public IEnumerable<ReceptionRoomBedViewModel> GetAllReceptionRoomBeds()
        {
            IEnumerable<ReceptionRoomBed> hosp = _unitOfWork.ReceptionRoomBeds.GetAllReceptionRoomBed();
            List<ReceptionRoomBedViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            Indexing<ReceptionRoomBedViewModel> indexing = new();
            return indexing.AddIndexing(hospconvert);
        }

        public ReceptionRoomBedViewModel GetReceptionRoomBed(Guid ReceptionRoomBedId)
        {
            try
            {
                ReceptionRoomBed ReceptionRoomBedgu = _unitOfWork.ReceptionRoomBeds.Get(ReceptionRoomBedId);
                return ConvertModel(ReceptionRoomBedgu);
            }
            catch { return null; }
        }

        public IEnumerable<ReceptionRoomBedViewModel> FilterReceptionByRoomAndBedAndPatientAndUser(Guid userId, Guid roomId, Guid roomBedId, Guid patientId, int periodId, DateTime DateFrom, DateTime DateTo)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                DateFrom = DateTime.Now;
                DateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
            }

            IEnumerable<ReceptionRoomBed> hosp = _unitOfWork.ReceptionRoomBeds.FilterReceptionByRoomAndBedAndPatientAndUser(userId, roomId, roomBedId, patientId, x => x.CreatedDate <= DateTo && x.CreatedDate >= DateFrom);
            List<ReceptionRoomBedViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            Indexing<ReceptionRoomBedViewModel> indexing = new();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReceptionRoomBedViewModel> FilterReceptionByRoomAndBedAndPatient(Guid roomId, Guid roomBedId, Guid patientId, int periodId, DateTime DateFrom, DateTime DateTo)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                DateFrom = DateTime.Now;
                DateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
            }

            IEnumerable<ReceptionRoomBed> hosp = _unitOfWork.ReceptionRoomBeds.FilterReceptionByRoomAndBedAndPatient(roomId, roomBedId, patientId, x => x.CreatedDate <= DateTo && x.CreatedDate >= DateFrom);
            List<ReceptionRoomBedViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            Indexing<ReceptionRoomBedViewModel> indexing = new();
            return indexing.AddIndexing(hospconvert);
        }

        public ReceptionRoomBedViewModel GetReceptionByRoomBedId(Guid RoomBedId)
        {
            try
            {
                ReceptionRoomBed ReceptionRoomBedgu = _unitOfWork.ReceptionRoomBeds.GetReceptionRMWithReceptionByRoomBedId(RoomBedId);
                return ConvertModel(ReceptionRoomBedgu);
            }
            catch { return null; }
        }

        public void CloseOldReceptionRoomBed(Guid receptionId)
        {
            var oldRoomBedReception = _unitOfWork.ReceptionRoomBeds.GetReceptionRMWithReceptionAndBedByReceptionId(receptionId);

            oldRoomBedReception.ExitDate = DateTime.Now;
            oldRoomBedReception.RoomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Dirty", "RoomStatus");

            _unitOfWork.ReceptionRoomBeds.UpdateState(oldRoomBedReception);
            _unitOfWork.RoomBeds.UpdateState(oldRoomBedReception.RoomBed);
            _unitOfWork.Complete();
        }

        public IEnumerable<HospitalDashboardViewModel> GetAllRoomsWithPatient(IEnumerable<Guid> clinicSectionId)
        {
            try
            {
                IEnumerable<HospitalDashboardModel> ReceptionRoomBedgu = _unitOfWork.ReceptionRoomBeds.GetAllRoomsWithPatient(clinicSectionId);
                IEnumerable<HospitalDashboardViewModel> all = ReceptionRoomBedgu.GroupBy(a => new { a.RoomName , a.BedName }).Select(b => new HospitalDashboardViewModel
                {
                    RoomName = b.Key.RoomName,
                    BedName = b.Key.BedName,
                    SingleRoomInfo = b.OrderByDescending(a => a.ReceptionDate).Select(c => new BedInfoViewModel
                    {
                        BedStatus = c.BedStatus,
                        Age = c.DateOfBirth.GetValueOrDefault().GetAge().ToString(),
                        PatientName = c.PatientName,
                        ReceptionId = c.ReceptionId.GetValueOrDefault(),
                        Doctor = c.Doctor,
                        ReceptionDate = c.ReceptionDate.GetValueOrDefault().Day + "/" + c.ReceptionDate.GetValueOrDefault().Month + "/" + c.ReceptionDate.GetValueOrDefault().Year,
                        Surgery = c.Surgery,
                        SurgeryDate = c.SurgeryDate.GetValueOrDefault().Day + "/" + c.SurgeryDate.GetValueOrDefault().Month + "/" + c.SurgeryDate.GetValueOrDefault().Year
                    }).FirstOrDefault()
                });

                IEnumerable<HospitalDashboardViewModel> all2 = all.GroupBy(a =>  a.RoomName).Select(b => new HospitalDashboardViewModel
                {
                    RoomName = b.Key,
                    RoomInfo = b.Select(c => new BedInfoViewModel
                    {
                        BedName = c.BedName,
                        BedStatus = c.SingleRoomInfo.BedStatus,
                        Age = c.SingleRoomInfo.Age,
                        PatientName = c.SingleRoomInfo.PatientName,
                        ReceptionId = c.SingleRoomInfo.ReceptionId,
                        Doctor = c.SingleRoomInfo.Doctor,
                        ReceptionDate = c.SingleRoomInfo.ReceptionDate,
                        Surgery = c.SingleRoomInfo.Surgery,
                        SurgeryDate = c.SingleRoomInfo.SurgeryDate
                    })
                });

                return all2;
            }
            catch(Exception e) { return null; }
        }

        // Begin Convert 
        public ReceptionRoomBedViewModel ConvertModel(ReceptionRoomBed reception)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionRoomBed, ReceptionRoomBedViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceptionRoomBed, ReceptionRoomBedViewModel>(reception);
        }
        public List<ReceptionRoomBedViewModel> ConvertModelsLists(IEnumerable<ReceptionRoomBed> receptionRoomBeds)
        {
            List<ReceptionRoomBedViewModel> receptionRoomBedDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionRoomBed, ReceptionRoomBedViewModel>()
                .ForMember(p => p.RoomName, r => r.MapFrom(s => s.RoomBed.Room.Name))
                .ForMember(p => p.RoomBedName, r => r.MapFrom(s => s.RoomBed.Name))
                .ForMember(p => p.PatientName, r => r.MapFrom(s => s.Reception.Patient.User.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            receptionRoomBedDtoList = mapper.Map<IEnumerable<ReceptionRoomBed>, List<ReceptionRoomBedViewModel>>(receptionRoomBeds);
            return receptionRoomBedDtoList;
        }

        


        // End Convert
    }
}
