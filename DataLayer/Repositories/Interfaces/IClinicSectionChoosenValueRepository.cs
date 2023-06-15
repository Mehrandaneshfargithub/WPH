using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IClinicSectionChoosenValueRepository : IRepository<ClinicSectionChoosenValue>
    {
        IEnumerable<ClinicSectionChoosenValue> GetAllClinicSectionChoosenValue(Guid clinicSectionId);
        IEnumerable<ClinicSectionChoosenValue> GetAllNumericClinicSectionChoosenValues(Guid clinicSectionId);
        IEnumerable<ClinicSectionChoosenValue> GetAllFillBySecretaryClinicSectionChoosenValues(Guid clinicSectionId);
    }
}
