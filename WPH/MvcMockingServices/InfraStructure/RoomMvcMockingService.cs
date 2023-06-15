using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.Room;
using WPH.MvcMockingServices.Interface;


namespace WPH.MvcMockingServices.InfraStructure
{
    public class RoomMvcMockingService : IRoomMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public IEnumerable<RoomViewModel> GetAllRoomsWithChildForUserBySectionId(Guid userId, Guid sectionId)
        {
            IEnumerable<Room> RoomDb = _unitOfWork.Rooms.GetAllRoomsWithChildByUserId(userId, (sectionId != Guid.Empty) ? (p => p.ClinicSectionId.Equals(sectionId)) : null);
            List<RoomViewModel> Roomvi = ConvertModelsLists(RoomDb);
            Indexing<RoomViewModel> indexing = new Indexing<RoomViewModel>();
            return indexing.AddIndexing(Roomvi);
        }

        public IEnumerable<RoomViewModel> GetAllRoomsWithChildBySectionId(Guid sectionId)
        {
            IEnumerable<Room> RoomDb = _unitOfWork.Rooms.GetAllRoomsWithChild((sectionId != Guid.Empty) ? (p => p.ClinicSectionId.Equals(sectionId)) : null);
            List<RoomViewModel> Roomvi = ConvertModelsLists(RoomDb);
            Indexing<RoomViewModel> indexing = new Indexing<RoomViewModel>();
            return indexing.AddIndexing(Roomvi);
        }

        public IEnumerable<RoomViewModel> GetAllRoomsForUserBySectionId(Guid userId, Guid sectionId)
        {
            IEnumerable<Room> RoomDb = _unitOfWork.Rooms.GetAllRoomsByUserId(userId, (sectionId != Guid.Empty) ? (p => p.ClinicSectionId.Equals(sectionId)) : null);
            List<RoomViewModel> Roomvi = ConvertModelsLists(RoomDb);
            Indexing<RoomViewModel> indexing = new Indexing<RoomViewModel>();
            return indexing.AddIndexing(Roomvi);
        }

        public IEnumerable<RoomViewModel> GetAllRoomsBySectionId(Guid sectionId)
        {
            IEnumerable<Room> RoomDb = _unitOfWork.Rooms.GetAllRooms((sectionId != Guid.Empty) ? (p => p.ClinicSectionId.Equals(sectionId)) : null);
            List<RoomViewModel> Roomvi = ConvertModelsLists(RoomDb);
            Indexing<RoomViewModel> indexing = new Indexing<RoomViewModel>();
            return indexing.AddIndexing(Roomvi);
        }

        public OperationStatus RemoveRoom(Guid Roomid)
        {
            try
            {
                Room Rom = _unitOfWork.Rooms.Get(Roomid);
                _unitOfWork.Rooms.Remove(Rom);
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

        public string AddNewRoom(RoomViewModel room)
        {
            if (string.IsNullOrWhiteSpace(room.Name) || room.RoomClinicSectionId == null || room.RoomClinicSectionId == Guid.Empty ||
                room.TypeId == null || room.RoomStatusId == null || room.MaxCapacity.GetValueOrDefault(0) < 0)
                return "ERROR_Data";

            Room roomDb = ConvertModel(room);

            _unitOfWork.Rooms.Add(roomDb);
            _unitOfWork.Complete();
            return roomDb.Guid.ToString();
        }

        public string UpdateRoom(RoomViewModel room)
        {
            if (string.IsNullOrWhiteSpace(room.Name) || room.RoomClinicSectionId == null || room.RoomClinicSectionId == Guid.Empty ||
                room.TypeId == null || room.RoomStatusId == null || room.MaxCapacity == null)
                return "ERROR_Data";

            var beds = _unitOfWork.RoomBeds.GetBedCountByRoomId(room.Guid);
            if (beds > room.MaxCapacity)
                return "ERROR_ChangeRoomCapacity";

            Room roomdb = ConvertModel(room);
            _unitOfWork.Rooms.UpdateState(roomdb);
            _unitOfWork.Complete();
            return roomdb.Guid.ToString();

        }

        

        public RoomViewModel GetRoom(Guid RoomId)
        {
            Room RoomDb = _unitOfWork.Rooms.Get(RoomId);
            return ConvertModel(RoomDb);

        }

        public RoomViewModel GetRoomWithSection(Guid RoomId)
        {
            Room room = _unitOfWork.Rooms.GetRoomWithSection(RoomId);
            return ConvertModel(room);
        }

        public bool CheckRepeatedRoomName(string name, bool NewOrUpdate, string oldName = "")
        {
            Room room = null;
            if (NewOrUpdate)
            {
                room = _unitOfWork.Rooms.GetSingle(x => x.Name.Trim() == name.Trim());
            }
            else
            {
                room = _unitOfWork.Rooms.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName);
            }
            if (room != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Room/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public List<RoomViewModel> ConvertModelsLists(IEnumerable<Room> Rooms)
        {
            List<RoomViewModel> RoomList = new List<RoomViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Room, RoomViewModel>()
                .ForMember(a => a.SectionName, b => b.MapFrom(c => c.ClinicSection.Name))
                .ForMember(a => a.RoomStatus, b => b.MapFrom(c => c.Status.Name))
                .ForMember(a => a.RoomType, b => b.MapFrom(c => c.Type.Name))
                .ForMember(a => a.RoomClinicSectionId, b => b.MapFrom(c => c.ClinicSectionId))
                .ForMember(a => a.RoomStatusId, b => b.MapFrom(c => c.StatusId))
                .ForMember(a => a.EmptyBed, b => b.MapFrom(c => c.RoomBeds.Where(p => p.Status.Name == "Clean" || p.Status.Name == "Dirty").Count()))
                ;
            });

            IMapper mapper = config.CreateMapper();
            RoomList = mapper.Map<IEnumerable<Room>, List<RoomViewModel>>(Rooms);
            return RoomList;
        }

        public RoomViewModel ConvertModel(Room Rooms)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Room, RoomViewModel>()
                .ForMember(a => a.RoomClinicSectionId, b => b.MapFrom(c => c.ClinicSectionId))
                .ForMember(a => a.SectionName, b => b.MapFrom(c => c.ClinicSection.Name))
                .ForMember(a => a.RoomStatusId, b => b.MapFrom(c => c.StatusId));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Room, RoomViewModel>(Rooms);
        }

        public Room ConvertModel(RoomViewModel Model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomViewModel, Room>()
                .ForMember(a => a.ClinicSectionId, b => b.MapFrom(c => c.RoomClinicSectionId))
                .ForMember(a => a.StatusId, b => b.MapFrom(c => c.RoomStatusId));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<RoomViewModel, Room>(Model);
        }

        
    }
}
