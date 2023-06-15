using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.ReserveDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface ITotalReservesMvcMockingService
    {
        List<ReserveDetailViewModel> GetAllReserveForSpecificDateBasedOnUserAccess(List<Guid> doctors, int periodId, DateTime dateFrom, DateTime dateTo);
        List<ReserveDetailViewModel> GetAllReserveByDoctorIdForSpecificDate(Guid doctorId, int periodId, DateTime dateFrom, DateTime dateTo);
        Task<string> ConvertToVisit(Guid reserveDetailId, Guid clinicSectionId, Guid userId);
    }
}
