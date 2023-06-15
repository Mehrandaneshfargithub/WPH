using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class PatientDiseaseRecordRepository : Repository<PatientDiseaseRecord>, IPatientDiseaseRecordRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PatientDiseaseRecordRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PatientDiseaseRecord> GetAllPatientDiseaseRecord(Guid patientId)
        {
            return _context.PatientDiseaseRecords.AsNoTracking()
                .Include(x => x.Disease)
                .Where(x => x.Patientid == patientId)
                .Select(p => new PatientDiseaseRecord
                {
                    DiseaseId = p.DiseaseId,
                    Disease = new Disease
                    {
                        Name = p.Disease.Name
                    }
                });
        }

        public IEnumerable<PatientDiseaseRecord> GetAllPatientDiseaseRecordByType(Guid PatientId, string DiseaseType)
        {
            return _context.PatientDiseaseRecords.AsNoTracking()
                .Include(x => x.Disease)
                .Where(x => x.Patientid == PatientId && x.Disease.DiseaseType.Name == DiseaseType);
        }

        public IEnumerable<PatientDiseaseRecord> RemoveDiseasesFromPatient(IEnumerable<Guid> diseaseIds, Guid PatientId)
        {
            return _context.PatientDiseaseRecords.AsNoTracking()
                .Where(p => p.Patientid == PatientId && p.DiseaseId != null && diseaseIds.Contains(p.DiseaseId.Value));
        }
    }
}
