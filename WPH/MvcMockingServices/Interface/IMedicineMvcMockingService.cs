using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.Medicine;
using WPH.Models.MedicineReport;

namespace WPH.MvcMockingServices.Interface
{
    public interface IMedicineMvcMockingService
    {
        IEnumerable<MedicineViewModel> GetAllMedicines(Guid clinicSectionId);
        IEnumerable<MedicineForVisitViewModel> GetAllMedicinesForVisitPrescription(Guid clinicSectionId);
        IEnumerable<MedicineForVisitViewModel> GetAllMedicinesForDisease(Guid clinicSectionId, Guid diseaseId, bool all);
        void GetModalsViewBags(dynamic viewBag);
        Guid AddNewMedicine(MedicineViewModel newMedicine, Guid clinicSectionId);
        Guid UpdateMedicine(MedicineViewModel med);
        void SwapPriority(Guid medicineId, Guid clinicSectionId, string type);
        PieChartViewModel GetMostUsedMedicine(Guid clinicSectionId);
        OperationStatus RemoveMedicine(Guid medicineId);
        MedicineViewModel GetMedicine(Guid medicineId);
        bool CheckRepeatedMedicineName(string name, Guid clinicSectionId, bool NewOrUpdate, string oldName = "");
        List<MedicineReportViewModel> GetMedicineReport(Guid clinicSectionId, DateTime fromDate, DateTime toDate, Guid medicineId, Guid producerId);
        void UpdateMedicineNum(Guid PreId, Guid MedicineId, string num, string v);
        IEnumerable<MedicineViewModel> GetAllExpiredMedicines(Guid clinicSectionId);
        IEnumerable<MedicineViewModel> GetAllMedicineForListBox(Guid clinicSectionId, Guid patientId);
    }
}
