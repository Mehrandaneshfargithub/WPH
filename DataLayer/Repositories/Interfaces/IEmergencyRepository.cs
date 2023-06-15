using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{

    public interface IEmergencyRepository : IRepository<Emergency>
    {
        IEnumerable<Emergency> GetAllEmergency();
        List<Reception> GetAllReceptionsByClinicSection(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo);
        Reception GetEmergency(Guid emergencyId);
        IEnumerable<PatientImage> RemoveEmergency(Guid emergencyid);
    }
}
