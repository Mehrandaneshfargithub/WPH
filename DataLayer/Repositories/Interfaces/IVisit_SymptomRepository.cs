using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IVisit_SymptomRepository : IRepository<VisitSymptom>
    {
        IEnumerable<VisitSymptom> GetAllVisit_Symptom(Guid VisitId);
        IEnumerable<VisitSymptom> GetAllVisitSymptomWithJustSymptomID(Guid visitId);
    }
}
