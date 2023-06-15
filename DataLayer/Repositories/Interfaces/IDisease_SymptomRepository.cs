using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IDisease_SymptomRepository:IRepository<DiseaseSymptom>
    {
        IEnumerable<DiseaseSymptom> GetAllDisease_Symptoms(Guid DiseaseId);
        void AddAllSymptomForDisease(IEnumerable<DiseaseSymptom> disease_Symptom);
    }
}
