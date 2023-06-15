using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class MedicineRepository : Repository<Medicine>, IMedicineRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public MedicineRepository(WASContext context)
            : base(context)
        {
        }

        public void UpdateMedicine(Medicine entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.Entry(entity).Property(x => x.Priority).IsModified = false;
            Context.SaveChanges();
        }

        public void UpdatePriority(Medicine currentMedicine)
        {
            Context.Entry(currentMedicine).State = EntityState.Modified;
            Context.Entry(currentMedicine).Property(x => x.Priority).IsModified = true;
            Context.SaveChanges();
        }

        public IEnumerable<MedicineForVisit> GetAllMedicinesForDisease(Guid clinicSectionId, Guid diseaseId, bool all)
        {
            if (all)
                return Context.Medicines.AsNoTracking()
                    .Include(x => x.MedicineDiseases)
                    .Where(x => x.ClinicSectionId == clinicSectionId && !x.MedicineDiseases.Where(y => y.DiseaseId == diseaseId).Select(y => y.MedicineId).Contains(x.Guid)).OrderBy(x => x.Priority).Select(x => new MedicineForVisit { Guid = x.Guid, JoineryName = x.JoineryName });
            else
                return Context.Medicines.AsNoTracking()
                    .Include(x => x.MedicineDiseases)
                    .Where(x => x.ClinicSectionId == clinicSectionId && x.MedicineDiseases.Any(y => y.DiseaseId == diseaseId)).OrderBy(x => x.Priority).Select(x => new MedicineForVisit { Guid = x.Guid, JoineryName = x.JoineryName });
        }


        public IEnumerable<Medicine> GetAllMedicines(Guid clinicSectionId)
        {

            return Context.Medicines.AsNoTracking()
            .Where(x => x.ClinicSectionId == clinicSectionId)
            .Include(x => x.MedicineForm)
            .Include(x => x.Producer)
            .OrderBy(x => x.Priority);
        }

        public IEnumerable<Medicine> GetAllMedicineForListBox(Guid clinicSectionId, Guid patientId)
        {
            return Context.Medicines.AsNoTracking()
            .Where(x => x.ClinicSectionId == clinicSectionId &&
                  !_context.PatientMedicineRecords.AsNoTracking().Where(p => p.PatientId == patientId).Select(p => p.MedicineId).Contains(x.Guid))
            .Select(p => new Medicine
            {
                Guid = p.Guid,
                JoineryName = p.JoineryName,
                Priority = p.Priority
            })
            .OrderBy(x => x.Priority)
            ;
        }

        public IEnumerable<MedicineForVisit> GetAllMedicinesForVisitPrescription(Guid clinicSectionId)
        {
            return Context.Medicines.AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId)
                .Select(x => new MedicineForVisit { Guid = x.Guid, JoineryName = x.JoineryName });
        }

        public List<PrescriptionDetail> GetMedicineReport(Guid clinicSectionId, DateTime fromDate, DateTime toDate, Guid medicineId, Guid producerId)
        {
            IQueryable<PrescriptionDetail> result = Context.PrescriptionDetails.AsNoTracking()
                .Include(x => x.Medicine)
                .ThenInclude(x => x.Producer)
                .Where(x => x.ClinicSectionId == clinicSectionId && x.Reception.ReceptionDate != null && x.Reception.ReceptionDate.Value.Date <= toDate && x.Reception.ReceptionDate.Value.Date >= fromDate)
                .Select(a => new PrescriptionDetail
                {
                    Medicine = new Medicine
                    {
                        Guid = a.Medicine.Guid,
                        JoineryName = a.Medicine.JoineryName,
                        ProducerId = a.Medicine.ProducerId,
                        Producer = new BaseInfo
                        {
                            Name = a.Medicine.Producer.Name,
                        }
                    },
                    Num = a.Num
                });

            return result.Where(p => (medicineId == Guid.Empty || p.MedicineId == medicineId)
                           && (producerId == Guid.Empty || p.Medicine.ProducerId == producerId)).ToList();

        }

        public void UpdateMedicineNum(Guid PreId, Guid MedicineId, string num, string status)
        {
            Medicine oldMed = Context.Medicines.SingleOrDefault(x => x.Guid == MedicineId);
            if (status == "Dicrease")
            {
                oldMed.MedNum -= Convert.ToInt32(num);
            }
            else if (status == "Increase")
            {
                oldMed.MedNum += Convert.ToInt32(num);
            }
            else
            {
                PrescriptionDetail pre = Context.PrescriptionDetails.SingleOrDefault(x => x.Guid == PreId);
                oldMed.MedNum += Convert.ToInt32(pre.Num);
                oldMed.MedNum -= Convert.ToInt32(num);
            }
        }

        public IEnumerable<Medicine> GetAllExpiredMedicines(Guid clinicSectionId)
        {
            return Context.Medicines.Where(x => x.ClinicSectionId == clinicSectionId && x.MedNum <= 0).Select(a => new Medicine
            {
                JoineryName = a.JoineryName
            });
        }

        public IEnumerable<PieChartModel> GetMostUsedMedicine(Guid clinicSectionId)
        {
            return Context.PrescriptionDetails.Include(a => a.Medicine).Where(a => a.ClinicSectionId == clinicSectionId).GroupBy(a => a.Medicine.JoineryName).Select(a => new PieChartModel
            {
                Label = a.Key,
                Value = a.Sum(a=>Convert.ToInt32(a.Num))
            });
        }
    }
}
