using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Dashboard;
using WPH.Models.ReceptionRoomBed;

namespace WPH.MvcMockingServices.Interface
{

    public interface IReceptionRoomBedMvcMockingService
    {
        OperationStatus RemoveReceptionRoomBed(Guid ReceptionRoomBedid);
        Guid AddNewReceptionRoomBed(ReceptionRoomBedViewModel ReceptionRoomBed);
        Guid UpdateReceptionRoomBed(ReceptionRoomBedViewModel Hosp);
        IEnumerable<ReceptionRoomBedViewModel> GetAllReceptionRoomBeds();
        ReceptionRoomBedViewModel GetReceptionRoomBed(Guid ReceptionRoomBedId);
        void GetModalsViewBags(dynamic viewBag);
        ReceptionRoomBedViewModel GetReceptionByRoomBedId(Guid RoomBedId);
        IEnumerable<ReceptionRoomBedViewModel> FilterReceptionByRoomAndBedAndPatientAndUser(Guid userId, Guid roomId, Guid roomBedId, Guid patientId, int periodId, DateTime DateFrom, DateTime DateTo);
        IEnumerable<ReceptionRoomBedViewModel> FilterReceptionByRoomAndBedAndPatient(Guid roomId, Guid roomBedId, Guid patientId, int periodId, DateTime DateFrom, DateTime DateTo);
        void CloseOldReceptionRoomBed(Guid receptionId);
        IEnumerable<HospitalDashboardViewModel> GetAllRoomsWithPatient(IEnumerable<Guid> clinicSectionId);
    }
}
