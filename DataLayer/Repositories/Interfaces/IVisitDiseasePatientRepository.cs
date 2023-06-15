using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IVisitDiseasePatientRepository : IRepository<VisitPatientDisease>
    {
        IEnumerable<VisitPatientDisease> GetAllVisit_Patient_Disease(Guid visitId);
        IEnumerable<VisitPatientDisease> GetAllVisitDiseaseWithJustDiseaseID(Guid visitId);
    }
}
