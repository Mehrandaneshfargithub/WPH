using DataLayer.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class DiseaseRepository : Repository<Disease>, IDiseaseRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public DiseaseRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Disease> GetAllDiseaseForListBox(Guid clinicSectionId, Guid patientId)
        {
            return Context.Diseases.AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId &&
                     !_context.PatientDiseaseRecords.AsNoTracking().Where(p=>p.Patientid==patientId).Select(p=>p.DiseaseId).Contains(x.Guid))
                .Select(p => new Disease
                {
                    Guid = p.Guid,
                    Name = p.Name
                })
                ;
        }

        public IEnumerable<Disease> GetAllDiseases(Guid clinicSectionId)
        {
            return Context.Diseases.AsNoTracking()
                .Include(x => x.DiseaseType)
                .Where(x => x.ClinicSectionId == clinicSectionId)
                ;
        }

        public IEnumerable<Disease> GetAllDiseasesJustNameAndGuid(Guid clinicSectionId)
        {
            return Context.Diseases.AsNoTracking().Where(x => x.ClinicSectionId == clinicSectionId).Select(x => new Disease { Guid = x.Guid, Name = x.Name });
        }

        public IEnumerable<PieChartModel> GetMostUsedDisease(Guid userId)
        {
            return Context.VisitPatientDiseases
                .Include(a => a.Disease)
                .Include(a => a.Reception)
                .Where(a => Context.ClinicSectionUsers.Where(a=>a.UserId == userId).Select(a=>a.ClinicSectionId).Contains(a.Reception.ClinicSectionId.Value))
                .GroupBy(a => a.Disease.Name).Select(a => new PieChartModel
                {
                    Label = a.Key,
                    Value = a.Count()
                });
        }
    }
}
