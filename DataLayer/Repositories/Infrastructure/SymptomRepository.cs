using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class SymptomRepository : Repository<Symptom>, ISymptomRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }

        public SymptomRepository(WASContext context)
                   : base(context)
        {
        }

        public IEnumerable<Symptom> GetAllSymptomsForDisease(Guid clinicSectionId, Guid diseaseId, bool all)
        {
            if (all)
                return Context.Symptoms.AsNoTracking()
                    .Include(x => x.DiseaseSymptoms)
                    .Where(x => x.ClinicSectionId == clinicSectionId && !x.DiseaseSymptoms.Where(y => y.DiseaseId == diseaseId).Select(y => y.SymptomId).Contains(x.Guid));
            else
                return Context.Symptoms.AsNoTracking()
                    .Include(x => x.DiseaseSymptoms)
                    .Where(x => x.ClinicSectionId == clinicSectionId && x.DiseaseSymptoms.Any(y => y.DiseaseId == diseaseId));
        }

        public void RemoveSymptom(Guid symptomId)
        {
            Symptom se = new Symptom() { Guid = symptomId };
            Context.Entry(se).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public IEnumerable<Symptom> GetAllSymptomJustNameAndGuid(Guid clinicSectionId)
        {
            return Context.Symptoms.AsNoTracking().Where(x => x.ClinicSectionId == clinicSectionId).Select(x=>new Symptom { Guid = x.Guid, Name = x.Name });
        }
    }
}
