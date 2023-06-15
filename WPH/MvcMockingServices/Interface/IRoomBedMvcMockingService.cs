using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.RoomBed;

namespace WPH.MvcMockingServices.Interface
{
    public interface IRoomBedMvcMockingService
    {
        OperationStatus RemoveRoomBed(Guid RoomBedid);
        string AddNewRoomBed(RoomBedViewModel RoomBed);
        string UpdateRoomBed(RoomBedViewModel Hosp);
        string UpdateRoomBedWithReceptionRoomBed(RoomBedViewModel Hosp);
        bool CheckRepeatedRoomBedName(string name, bool NewOrUpdate, string oldName = "");
        IEnumerable<RoomBedViewModel> GetAllRoomBedsWithChildByRoomId(Guid RoomId);
        IEnumerable<RoomBedViewModel> GetEmptyBedByClinicSectionId(Guid ClinicSectionId);
        RoomBedViewModel GetRoomBed(Guid RoomBedId);
        void GetModalsViewBags(dynamic viewBag);
        RoomBedViewModel GetRoomBedWithPatient(Guid RoomBedId);
        void SetPatientRoomBed(PatientRoomBedViewModel viewModel);
        IEnumerable<RoomBedViewModel> GetAllRoomBedsByRoomId(Guid RoomId);
        string GetReceptionRoomBedName(Guid receptionId);
        RoomBedViewModel GetReceptionRoomBedId(Guid receptionId);
        void UpdateRoomStatus(Guid roomId);
    }
}
