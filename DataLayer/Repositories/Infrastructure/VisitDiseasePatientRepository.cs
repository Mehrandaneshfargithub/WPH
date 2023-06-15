using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class VisitDiseasePatientRepository : Repository<VisitPatientDisease>, IVisitDiseasePatientRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public VisitDiseasePatientRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<VisitPatientDisease> GetAllVisit_Patient_Disease(Guid VisitId)
        {

            return Context.VisitPatientDiseases.AsNoTracking().Include(x => x.Disease).Where(x => x.ReceptionId == VisitId);
        }

        public IEnumerable<VisitPatientDisease> GetAllVisitDiseaseWithJustDiseaseID(Guid visitId)
        {
            return Context.VisitPatientDiseases.AsNoTracking().Where(x => x.ReceptionId == visitId).Select(x => new VisitPatientDisease { DiseaseId = x.DiseaseId });
        }
    }
}
