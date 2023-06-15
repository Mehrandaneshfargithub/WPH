using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Room;


namespace WPH.MvcMockingServices.Interface
{
    public interface IRoomMvcMockingService
    {
        IEnumerable<RoomViewModel> GetAllRoomsWithChildForUserBySectionId(Guid userId, Guid sectionId);
        IEnumerable<RoomViewModel> GetAllRoomsWithChildBySectionId(Guid sectionId);
        OperationStatus RemoveRoom(Guid Roomid);
        string AddNewRoom(RoomViewModel Room);
        string UpdateRoom(RoomViewModel Room);
        RoomViewModel GetRoom(Guid Room);
        RoomViewModel GetRoomWithSection(Guid RoomId);
        bool CheckRepeatedRoomName(string name,bool NewOrUpdate, string oldName = "");
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<RoomViewModel> GetAllRoomsForUserBySectionId(Guid userId, Guid sectionId);
        IEnumerable<RoomViewModel> GetAllRoomsBySectionId(Guid sectionId);
    }
}
