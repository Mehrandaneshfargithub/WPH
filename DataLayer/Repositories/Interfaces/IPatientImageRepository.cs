using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPatientImageRepository : IRepository<PatientImage>
    {
        IEnumerable<PatientImage> GetAttachmentsByPatientAndTypeId(Guid patientId, int? typeId);
        IEnumerable<PatientImage> GetAttachmentsByReceptionAndTypeId(Guid receptionId, int? typeId);
        IEnumerable<PatientImage> GetAttachmentsByReceptionId(Guid receptionId);
    }
}
