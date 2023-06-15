using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.RoomItem;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class RoomItemMvcMockingService : IRoomItemMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomItemMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public OperationStatus RemoveRoomItem(Guid RoomItemid)
        {
            try
            {
                RoomItem Hos = _unitOfWork.RoomItems.Get(RoomItemid);
                _unitOfWork.RoomItems.Remove(Hos);
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
            string controllerName = "/RoomItem/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewRoomItem(RoomItemViewModel roomItem)
        {
            if (roomItem.RoomId == null || roomItem.RoomId == Guid.Empty || roomItem.ItemId == null || roomItem.ItemId == Guid.Empty)
                return "ERROR_Data";


            if (!string.IsNullOrWhiteSpace(roomItem.Code))
            {
                var dupCode = _unitOfWork.RoomItems.CheckDuplicateCode(roomItem.Code);
                if (dupCode)
                    return "CodeIsRepeated";
            }

            RoomItem roomItem1 = Common.ConvertModels<RoomItem, RoomItemViewModel>.convertModels(roomItem);
            
            _unitOfWork.RoomItems.Add(roomItem1);
            _unitOfWork.Complete();
            return roomItem1.Guid.ToString();

        }



        public string UpdateRoomItem(RoomItemViewModel viewModel)
        {
            if (viewModel.RoomId == null || viewModel.RoomId == Guid.Empty || viewModel.ItemId == null || viewModel.ItemId == Guid.Empty)
                return "ERROR_Data";


            if (!string.IsNullOrWhiteSpace(viewModel.Code))
            {
                var dupCode = _unitOfWork.RoomItems.CheckDuplicateCode(viewModel.Code, p => p.Guid != viewModel.Guid);
                if (dupCode)
                    return "CodeIsRepeated";
            }

            RoomItem roomItem2 = Common.ConvertModels<RoomItem, RoomItemViewModel>.convertModels(viewModel);
            _unitOfWork.RoomItems.UpdateState(roomItem2);
            _unitOfWork.Complete();
            return roomItem2.Guid.ToString();

        }

        public bool CheckRepeatedRoomItem(Guid? roomId, Guid? itemId, bool NewOrUpdate, Guid? oldRoomId = null, Guid? oldItemId = null)
        {
            RoomItem roomItem = null;
            if (NewOrUpdate)
            {
                roomItem = _unitOfWork.RoomItems.GetSingle(x => x.RoomId == roomId && x.ItemId == itemId);
            }
            else
            {
                roomItem = _unitOfWork.RoomItems.GetSingle(x => x.RoomId == roomId && x.ItemId == itemId && x.RoomId == oldRoomId && x.ItemId == oldItemId);
            }
            if (roomItem != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public IEnumerable<RoomItemViewModel> GetAllRoomItemsByRoomId(Guid RoomId)
        {
            IEnumerable<RoomItem> hosp = _unitOfWork.RoomItems.GetAllRoomItemByRoomId(RoomId);
            List<RoomItemViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            Indexing<RoomItemViewModel> indexing = new Indexing<RoomItemViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public RoomItemViewModel GetRoomItem(Guid RoomItemId)
        {
            RoomItem RoomItemgu = _unitOfWork.RoomItems.GetWithRoomAndStatus(RoomItemId);
            return ConvertModel(RoomItemgu);

        }

        // Begin Convert 
        public RoomItemViewModel ConvertModel(RoomItem roomItem)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomItem, RoomItemViewModel>()
                .ForMember(p => p.ItemTypeName, r => r.MapFrom(s => s.Item.ItemType.Name));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<RoomItem, RoomItemViewModel>(roomItem);
        }
        public List<RoomItemViewModel> ConvertModelsLists(IEnumerable<RoomItem> roomItems)
        {
            List<RoomItemViewModel> roomItemDtoList = new List<RoomItemViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomItem, RoomItemViewModel>()
                .ForMember(p => p.ItemTypeName, r => r.MapFrom(s => s.Item.ItemType.Name));
            });

            IMapper mapper = config.CreateMapper();
            roomItemDtoList = mapper.Map<IEnumerable<RoomItem>, List<RoomItemViewModel>>(roomItems);

            return roomItemDtoList;
        }
        // End Convert
    }
}
