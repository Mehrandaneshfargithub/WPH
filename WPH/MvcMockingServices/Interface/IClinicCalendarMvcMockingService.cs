using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.Reserve;

namespace WPH.MvcMockingServices.Interface
{
    public interface IClinicCalendarMvcMockingService
    {
        List<ReserveViewModel> GetClinicCalendarDaysByClinicSection(Guid clinicSectionId, int periodId, DateTime fromDate, DateTime toDate);
        Task SaveClinicCalendar(List<ReserveViewModel> reserves, Guid clinicSectionId, int periodId, DateTime fromDate, DateTime toDate);
    }
}
