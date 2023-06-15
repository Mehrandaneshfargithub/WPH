using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class Visit_SymptomRepository : Repository<VisitSymptom>, IVisit_SymptomRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public Visit_SymptomRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<VisitSymptom> GetAllVisit_Symptom(Guid visitId)
        {

            return Context.VisitSymptoms.AsNoTracking().Include(x => x.Symptom).Where(x => x.ReceptionId == visitId);
        }

        public IEnumerable<VisitSymptom> GetAllVisitSymptomWithJustSymptomID(Guid visitId)
        {
            return Context.VisitSymptoms.AsNoTracking().Where(x => x.ReceptionId == visitId).Select(x => new VisitSymptom { SymptomId = x.SymptomId });
        }
    }
}
