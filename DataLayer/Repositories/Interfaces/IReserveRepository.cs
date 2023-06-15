using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReserveRepository : IRepository<Reserve>
    {
        Reserve GetDate(DateTime date, Guid ClinicSectionId);
        IEnumerable<Reserve> GetClinicCalendarDaysByClinicSection(Guid clinicSectionId, DateTime fromDate, DateTime toDate);
        Task SaveClinicCalendar(List<Reserve> allReserve, Guid clinicSectionId, DateTime fromDate, DateTime toDate);
        IEnumerable<Reserve> GetClinicSectionCalendar(Guid clinicSectionId); 
    }
}
