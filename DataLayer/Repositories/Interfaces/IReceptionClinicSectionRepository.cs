using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    
    public interface IReceptionClinicSectionRepository : IRepository<ReceptionClinicSection>
    {
        IEnumerable<ReceptionClinicSection> GetAllReceptionClinicSection();
        IEnumerable<ReceptionClinicSection> GetAllReceptionClinicSectionByReceptionId(Guid ReceptionId);
        IEnumerable<ReceptionClinicSection> GetAllReceptionClinicSectionByClinicSectionId(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid receptionId, Expression<Func<ReceptionClinicSection, bool>> predicate = null);
        ReceptionClinicSection GetReceptionClinicSectionByDestinationReceptionId(Guid DestinationReceptionId);
        IEnumerable<ReceptionClinicSection> GetPatientToAnotherSectionReport(DateTime dateFrom, DateTime dateTo, Guid clinicSectionId);
        IEnumerable<Patient> GetAllReceptionClinicSectionPatients();
    }
}
