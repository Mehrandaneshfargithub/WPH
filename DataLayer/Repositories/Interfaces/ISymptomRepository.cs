using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISymptomRepository : IRepository<Symptom>
    {
        IEnumerable<Symptom> GetAllSymptomsForDisease(Guid clinicSectionId, Guid diseaseId, bool all);
        void RemoveSymptom(Guid symptomId);
        IEnumerable<Symptom> GetAllSymptomJustNameAndGuid(Guid clinicSectionId);
    }
}
