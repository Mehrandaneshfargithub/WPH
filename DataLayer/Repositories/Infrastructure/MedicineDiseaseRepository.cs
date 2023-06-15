using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class MedicineDiseaseRepository : Repository<MedicineDisease>, IMedicineDiseaseRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public MedicineDiseaseRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<MedicineDisease> GetAllMedicine_Diseases(Guid diseaseId)
        {

            return Context.MedicineDiseases.AsNoTracking().Where(x=>x.DiseaseId == diseaseId).Include(x => x.Disease).Include(x => x.Medicine);
        }

        public void AddAllMedicinesForDisease(IEnumerable<MedicineDisease> disease_Symptom)
        {
            Guid diseaseId = disease_Symptom.FirstOrDefault().DiseaseId;
            Context.MedicineDiseases.RemoveRange(Context.MedicineDiseases.Where(x => x.DiseaseId == diseaseId));
            Context.MedicineDiseases.AddRange(disease_Symptom);
            Context.SaveChanges();
        }
    }
}
