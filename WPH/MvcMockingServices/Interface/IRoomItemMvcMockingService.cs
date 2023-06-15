using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.RoomItem;

namespace WPH.MvcMockingServices.Interface
{
    public interface IRoomItemMvcMockingService
    {
        OperationStatus RemoveRoomItem(Guid RoomItemid);
        string AddNewRoomItem(RoomItemViewModel RoomItem);
        string UpdateRoomItem(RoomItemViewModel Hosp);
        bool CheckRepeatedRoomItem(Guid? roomId, Guid? itemId, bool NewOrUpdate, Guid? oldRoomId = null, Guid? oldItemId = null);
        IEnumerable<RoomItemViewModel> GetAllRoomItemsByRoomId(Guid RoomId);
        RoomItemViewModel GetRoomItem(Guid RoomItemId);
        void GetModalsViewBags(dynamic viewBag);
    }
}
