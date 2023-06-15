using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class PatientMedicineRecordRepository : Repository<PatientMedicineRecord>, IPatientMedicineRecordRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PatientMedicineRecordRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PatientMedicineRecord> GetAllPatientMedicineRecord(Guid patientId)
        {
            return _context.PatientMedicineRecords.AsNoTracking()
                .Include(x => x.Medicine)
                .Where(x => x.PatientId == patientId)
                .Select(p => new PatientMedicineRecord
                {
                    MedicineId = p.MedicineId,
                    Medicine = new Medicine
                    {
                        JoineryName = p.Medicine.JoineryName
                    }
                })
                ;
        }

        public IEnumerable<PatientMedicineRecord> GetAllPatientMedicineRecordGrid(Guid patientId)
        {
            return _context.PatientMedicineRecords
                .AsNoTracking()
                .Include(x => x.Medicine)
                .Include(x => x.Patient)
                .Where(x => x.PatientId == patientId);
        }

        public IEnumerable<PatientMedicineRecord> RemoveMedicineFromPatient(IEnumerable<Guid> medicineIds, Guid patientId)
        {
            return _context.PatientMedicineRecords.AsNoTracking()
                .Where(p => p.PatientId == patientId != null && medicineIds.Contains(p.MedicineId));
        }
    }
}
