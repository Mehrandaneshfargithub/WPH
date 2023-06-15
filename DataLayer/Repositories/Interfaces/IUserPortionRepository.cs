using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IUserPortionRepository : IRepository<UserPortion>
    {
        
        IEnumerable<UserPortion> GetAllUserPortions(Guid clinicSectionId);
        IEnumerable<UserPortion> GetAllUserPortionsBySpecification(Guid clinicSectionId, bool specification, Guid ReceptionId);
        IEnumerable<UserPortionReport> GetAllUserPortionForReport(Guid userId, DateTime fromDate, DateTime toDate, bool detail);
        IEnumerable<UserPortionReport> GetPortionReport(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, string status, Guid doctorId);
    }
}
