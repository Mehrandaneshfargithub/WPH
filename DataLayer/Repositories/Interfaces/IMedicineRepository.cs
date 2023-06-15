using System;
using System.Collections.Generic;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IMedicineRepository : IRepository<Medicine>
    {
        void UpdateMedicine(Medicine entity);
        void UpdatePriority(Medicine currentMedicine);
        IEnumerable<MedicineForVisit> GetAllMedicinesForDisease(Guid clinicSectionId, Guid diseaseId, bool all);
        IEnumerable<Medicine> GetAllMedicines(Guid clinicSectionId);
        IEnumerable<MedicineForVisit> GetAllMedicinesForVisitPrescription(Guid clinicSectionId);
        List<PrescriptionDetail> GetMedicineReport(Guid clinicSectionId, DateTime fromDate, DateTime toDate, Guid medicineId, Guid producerId);
        void UpdateMedicineNum(Guid PreId, Guid MedicineId, string num, string status);
        IEnumerable<Medicine> GetAllExpiredMedicines(Guid clinicSectionId);
        IEnumerable<Medicine> GetAllMedicineForListBox(Guid clinicSectionId, Guid patientId);
        IEnumerable<PieChartModel> GetMostUsedMedicine(Guid clinicSectionId);
    }
}
