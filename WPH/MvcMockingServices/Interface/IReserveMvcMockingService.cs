using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.Reserve;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReserveMvcMockingService
    {
        ReserveViewModel GetDate(DateTime date, Guid ClinicSectionId);
        void GetModalsViewBags(dynamic viewBag);
        Guid UpdateReserve(ReserveViewModel reserve);
        Guid AddReserve(ReserveViewModel today);
        Task<string> CheckLastPatientVisit(Guid reserveDetailId, Guid clinicSectionId);
        void RemoveReserveVisitPrice(Guid reserveDetailId);
        ReserveViewModel CheckAndAddReserve(Guid clinicSectionId, DateTime? day);
    }
}
