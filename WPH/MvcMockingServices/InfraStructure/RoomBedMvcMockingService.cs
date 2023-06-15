using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ReceptionRoomBed;
using WPH.Models.RoomBed;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class RoomBedMvcMockingService : IRoomBedMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomBedMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveRoomBed(Guid RoomBedid)
        {
            try
            {
                RoomBed Hos = _unitOfWork.RoomBeds.Get(RoomBedid);
                _unitOfWork.RoomBeds.Remove(Hos);
                _unitOfWork.Complete();

                UpdateRoomStatus(Hos.RoomId.Value);
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
            string controllerName = "/RoomBed/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewRoomBed(RoomBedViewModel roomBed)
        {
            if (roomBed.RoomId == null || roomBed.RoomId == Guid.Empty || roomBed.BedStatusId == null || string.IsNullOrWhiteSpace(roomBed.Name))
                return "ERROR_Data";

            int? remind = _unitOfWork.Rooms.GetRoomCapacityRemined(roomBed.RoomId);
            if (!(remind.GetValueOrDefault(0) > 0))
                return "FullCapacity";

            if (!string.IsNullOrWhiteSpace(roomBed.Code))
            {
                var dupCode = _unitOfWork.RoomBeds.CheckDuplicateCode(roomBed.Code);
                if (dupCode)
                    return "CodeIsRepeated";
            }

            RoomBed roomBed1 = ConvertModel(roomBed);

            _unitOfWork.RoomBeds.Add(roomBed1);
            _unitOfWork.Complete();

            UpdateRoomStatus(roomBed.RoomId.Value);
            return roomBed1.Guid.ToString();
        }



        public string UpdateRoomBed(RoomBedViewModel viewModel)
        {
            if (viewModel.RoomId == null || viewModel.RoomId == Guid.Empty || viewModel.BedStatusId == null || string.IsNullOrWhiteSpace(viewModel.Name))
                return "ERROR_Data";


            if (!string.IsNullOrWhiteSpace(viewModel.Code))
            {
                var dupCode = _unitOfWork.RoomBeds.CheckDuplicateCode(viewModel.Code, p => p.Guid != viewModel.Guid);
                if (dupCode)
                    return "CodeIsRepeated";
            }

            RoomBed roomBed2 = ConvertModel(viewModel);
            _unitOfWork.RoomBeds.UpdateState(roomBed2);
            _unitOfWork.Complete();

            UpdateRoomStatus(roomBed2.RoomId.Value);

            return roomBed2.Guid.ToString();
        }


        public string UpdateRoomBedWithReceptionRoomBed(RoomBedViewModel viewModel)
        {
            if (viewModel.RoomId == null || viewModel.RoomId == Guid.Empty || viewModel.BedStatusId == null || string.IsNullOrWhiteSpace(viewModel.Name))
                return "ERROR_Data";

            if (!string.IsNullOrWhiteSpace(viewModel.Code))
            {
                var dupCode = _unitOfWork.RoomBeds.CheckDuplicateCode(viewModel.Code, p => p.Guid != viewModel.Guid);
                if (dupCode)
                    return "CodeIsRepeated";
            }

            var receptionRoomBed = _unitOfWork.ReceptionRoomBeds.GetReceptionRMWithReceptionByRoomBedId(viewModel.Guid);
            if (receptionRoomBed == null)
                throw new Exception();

            RoomBed roomBed2 = ConvertModel(viewModel);
            _unitOfWork.RoomBeds.UpdateState(roomBed2);

            receptionRoomBed.ExitDate = DateTime.Now;
            _unitOfWork.ReceptionRoomBeds.UpdateState(receptionRoomBed);

            _unitOfWork.Complete();
            UpdateRoomStatus(roomBed2.RoomId.Value);

            return roomBed2.Guid.ToString();
        }

        public bool CheckRepeatedRoomBedName(string name, bool NewOrUpdate, string oldName = "")
        {
            RoomBed roomBed = null;
            if (NewOrUpdate)
            {
                roomBed = _unitOfWork.RoomBeds.GetSingle(x => x.Name.Trim() == name.Trim());
            }
            else
            {
                roomBed = _unitOfWork.RoomBeds.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName);
            }
            if (roomBed != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<RoomBedViewModel> GetAllRoomBedsWithChildByRoomId(Guid RoomId)
        {
            IEnumerable<RoomBed> hosp = _unitOfWork.RoomBeds.GetAllRoomBedWithChildByRoomId(RoomId);
            List<RoomBedViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<RoomBedViewModel> indexing = new();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<RoomBedViewModel> GetAllRoomBedsByRoomId(Guid RoomId)
        {
            IEnumerable<RoomBed> hosp = _unitOfWork.RoomBeds.GetAllRoomBedByRoomId(RoomId);
            List<RoomBedViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<RoomBedViewModel> indexing = new();
            return indexing.AddIndexing(hospconvert);
        }

        public RoomBedViewModel GetRoomBed(Guid RoomBedId)
        {
            RoomBed RoomBedgu = _unitOfWork.RoomBeds.GetWithRoomAndStatus(RoomBedId);
            return ConvertModel(RoomBedgu);
        }

        public RoomBedViewModel GetRoomBedWithPatient(Guid RoomBedId)
        {
            RoomBed roomBed = _unitOfWork.RoomBeds.GetRoomBedWithPatientById(RoomBedId);
            var viewModel = ConvertModel(roomBed);

            viewModel.PatientName = roomBed.ReceptionRoomBeds?.LastOrDefault()?.Reception?.Patient?.User?.Name ?? string.Empty;
            return viewModel;
        }

        public IEnumerable<RoomBedViewModel> GetEmptyBedByClinicSectionId(Guid ClinicSectionId)
        {
            IEnumerable<RoomBed> hosp = _unitOfWork.RoomBeds.GetEmptyBedByClinicSectionId(ClinicSectionId);
            List<RoomBedViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<RoomBedViewModel> indexing = new();
            return indexing.AddIndexing(hospconvert);
        }

        public void SetPatientRoomBed(PatientRoomBedViewModel viewModel)
        {
            var roomBed = _unitOfWork.RoomBeds.Get(viewModel.SetRoomBedValue);
            roomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Full", "RoomStatus");

            if (viewModel.OldReceptionValue != Guid.Empty && viewModel.OldRoomBedValue != Guid.Empty)
            {
                var oldRoomBedReception = _unitOfWork.ReceptionRoomBeds.GetReceptionRMWithReceptionByReceptionId(viewModel.OldReceptionValue);
                oldRoomBedReception.ExitDate = DateTime.Now;
                _unitOfWork.ReceptionRoomBeds.UpdateState(oldRoomBedReception);

                ReceptionRoomBed receptionRoomBed = new()
                {
                    Guid = Guid.NewGuid(),
                    ReceptionId = viewModel.SetReceptionValue,
                    EntranceDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = viewModel.UserId,
                    ModifiedUserId = viewModel.UserId,
                    RoomBedId = viewModel.SetRoomBedValue
                };
                _unitOfWork.ReceptionRoomBeds.Add(receptionRoomBed);

                var oldPatientRoomBedReception = _unitOfWork.ReceptionRoomBeds.GetReceptionRMWithReceptionByRoomBedId(viewModel.OldRoomBedValue);
                oldPatientRoomBedReception.ExitDate = DateTime.Now;
                _unitOfWork.ReceptionRoomBeds.UpdateState(oldPatientRoomBedReception);

                ReceptionRoomBed patientReceptionRoomBed = new()
                {
                    Guid = Guid.NewGuid(),
                    ReceptionId = viewModel.OldReceptionValue,
                    EntranceDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = viewModel.UserId,
                    ModifiedUserId = viewModel.UserId,
                    RoomBedId = viewModel.OldRoomBedValue
                };
                _unitOfWork.ReceptionRoomBeds.Add(patientReceptionRoomBed);

                var oldRoomBed = _unitOfWork.RoomBeds.Get(viewModel.OldRoomBedValue);
                oldRoomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Full", "RoomStatus");
                _unitOfWork.RoomBeds.UpdateState(oldRoomBed);
            }
            else if (viewModel.OldReceptionValue != Guid.Empty && viewModel.OldRoomBedValue == Guid.Empty)
            {
                var oldRoomBedReception = _unitOfWork.ReceptionRoomBeds.GetReceptionRMWithReceptionByReceptionId(viewModel.OldReceptionValue);
                oldRoomBedReception.ExitDate = DateTime.Now;
                _unitOfWork.ReceptionRoomBeds.UpdateState(oldRoomBedReception);

                ReceptionRoomBed receptionRoomBed = new()
                {
                    Guid = Guid.NewGuid(),
                    ReceptionId = viewModel.SetReceptionValue,
                    EntranceDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = viewModel.UserId,
                    ModifiedUserId = viewModel.UserId,
                    RoomBedId = viewModel.SetRoomBedValue
                };
                _unitOfWork.ReceptionRoomBeds.Add(receptionRoomBed);

            }
            else if (viewModel.OldReceptionValue == Guid.Empty && viewModel.OldRoomBedValue != Guid.Empty)
            {
                var oldPatientRoomBedReception = _unitOfWork.ReceptionRoomBeds.GetReceptionRMWithReceptionByRoomBedId(viewModel.OldRoomBedValue);
                oldPatientRoomBedReception.ExitDate = DateTime.Now;
                _unitOfWork.ReceptionRoomBeds.UpdateState(oldPatientRoomBedReception);

                ReceptionRoomBed receptionRoomBed = new()
                {
                    Guid = Guid.NewGuid(),
                    ReceptionId = viewModel.SetReceptionValue,
                    EntranceDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = viewModel.UserId,
                    ModifiedUserId = viewModel.UserId,
                    RoomBedId = viewModel.SetRoomBedValue
                };
                _unitOfWork.ReceptionRoomBeds.Add(receptionRoomBed);


                var oldRoomBed = _unitOfWork.RoomBeds.Get(viewModel.OldRoomBedValue);
                oldRoomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Dirty", "RoomStatus");
                _unitOfWork.RoomBeds.UpdateState(oldRoomBed);
            }
            else
            {
                ReceptionRoomBed receptionRoomBed = new()
                {
                    Guid = Guid.NewGuid(),
                    ReceptionId = viewModel.SetReceptionValue,
                    EntranceDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = viewModel.UserId,
                    ModifiedUserId = viewModel.UserId,
                    RoomBedId = viewModel.SetRoomBedValue
                };


                _unitOfWork.ReceptionRoomBeds.Add(receptionRoomBed);

            }
            _unitOfWork.RoomBeds.UpdateState(roomBed);
            _unitOfWork.Complete();

            UpdateRoomStatus(roomBed.RoomId.Value);
        }

        public void UpdateRoomStatus(Guid roomId)
        {
            var full = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Full", "RoomStatus");
            var clean = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Clean", "RoomStatus");
            var room = _unitOfWork.Rooms.GetRoomWithBeds(roomId);

            var status = room.RoomBeds.All(p => p.StatusId == full);
            room.StatusId = status ? full : clean;
            _unitOfWork.Rooms.UpdateState(room);
            _unitOfWork.Complete();
        }

        public string GetReceptionRoomBedName(Guid receptionId)
        {
            ReceptionRoomBed RRB = _unitOfWork.ReceptionRoomBeds.GetReceptionRoomBedName(receptionId);
            string rb = $"{RRB?.RoomBed?.Room?.Name ?? ""} | {RRB?.RoomBed?.Name ?? ""}";
            return rb;
        }

        public RoomBedViewModel GetReceptionRoomBedId(Guid receptionId)
        {
            ReceptionRoomBed RRB = _unitOfWork.ReceptionRoomBeds.GetReceptionRoomBedName(receptionId);
            return new RoomBedViewModel
            {
                Guid = RRB?.RoomBedId.GetValueOrDefault(Guid.Empty) ?? Guid.Empty,
                Name = RRB?.RoomBed?.Name ?? "",
                RoomName = RRB?.RoomBed?.Room?.Name ?? "",
            };
        }

        // Begin Convert 
        public List<RoomBedViewModel> ConvertModelsLists(IEnumerable<RoomBed> roomBeds)
        {

            List<RoomBedViewModel> roomBedDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomBed, RoomBedViewModel>()
                .ForMember(p => p.BedStatus, r => r.MapFrom(s => s.Status.Name))
                .ForMember(p => p.BedStatusId, r => r.MapFrom(s => s.StatusId))
                .ForMember(a => a.PatientName, b => b.MapFrom(c => !(c.ReceptionRoomBeds.LastOrDefault(p => p.ExitDate == null).Reception.Discharge ?? false) ? (c.ReceptionRoomBeds.LastOrDefault(p => p.ExitDate == null).Reception.Patient.User.Name ?? string.Empty) : string.Empty))
                .ForMember(a => a.ReceptionId, b => b.MapFrom(c => !(c.ReceptionRoomBeds.LastOrDefault(p => p.ExitDate == null).Reception.Discharge ?? false) ? (c.ReceptionRoomBeds.LastOrDefault(p => p.ExitDate == null).Reception.Guid) : Guid.Empty))
                ;
            });

            IMapper mapper = config.CreateMapper();
            roomBedDtoList = mapper.Map<IEnumerable<RoomBed>, List<RoomBedViewModel>>(roomBeds);
            return roomBedDtoList;
        }

        public RoomBedViewModel ConvertModel(RoomBed Bed)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomBed, RoomBedViewModel>()
                .ForMember(p => p.BedStatus, r => r.MapFrom(s => s.Status.Name))
                .ForMember(p => p.BedStatusId, r => r.MapFrom(s => s.StatusId));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<RoomBed, RoomBedViewModel>(Bed);
        }
        public RoomBed ConvertModel(RoomBedViewModel Model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomBedViewModel, RoomBed>()
                .ForMember(p => p.StatusId, r => r.MapFrom(s => s.BedStatusId));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<RoomBedViewModel, RoomBed>(Model);
        }


        // End Convert
    }
}
