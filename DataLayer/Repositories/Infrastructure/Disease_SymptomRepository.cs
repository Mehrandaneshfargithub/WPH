using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class Disease_SymptomRepository : Repository<DiseaseSymptom>, IDisease_SymptomRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public Disease_SymptomRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<DiseaseSymptom> GetAllDisease_Symptoms(Guid DiseaseId)
        {

            return Context.DiseaseSymptoms.AsNoTracking().Where(x=>x.DiseaseId == DiseaseId);
        }

        public void AddAllSymptomForDisease(IEnumerable<DiseaseSymptom> disease_Symptom)
        {
            Guid diseaseId = disease_Symptom.FirstOrDefault().DiseaseId;
            Context.DiseaseSymptoms.RemoveRange(Context.DiseaseSymptoms.Where(x => x.DiseaseId == diseaseId));
            Context.DiseaseSymptoms.AddRange(disease_Symptom);
            Context.SaveChanges();
        }
    }
    
}
