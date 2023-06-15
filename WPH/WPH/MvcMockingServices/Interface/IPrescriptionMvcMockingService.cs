using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.PrescriptionTest;
using WPH.Models.CustomDataModels.PrescrptionDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPrescriptionMvcMockingService
    {
        Guid AddPrescriptionDetail(PrescriptionDetailViewModel prescription, string status);
        void AddPrescriptionDetailRange(List<PrescriptionDetailViewModel> prescription);
        Guid UpdatePrescriptionDetail(PrescriptionDetailViewModel prescription);
        List<PrescriptionDetailViewModel> GetAllPrescriptionDetai(Guid VisitId);
        PrescriptionDetailViewModel GetLastMedicinePrescription(Guid MedicineId);
        void RemovePrescriptionDetail(Guid preGuid);
        PrescriptionDetailViewModel GetPrescriptionDetailById(Guid prescriptionId);
        List<PrescriptionTestDetailViewModel> GetAllPrescriptonTests(Guid visitId);
        Guid AddPrescriptionTest(PrescriptionTestDetailViewModel viewModel);
        PrescriptionTestDetailViewModel GetPrescriptionTestById(Guid prescriptionId);
        Guid UpdatePrescriptionTest(PrescriptionTestDetailViewModel prescription);
        void RemovePrescriptionTest(Guid prescriptionId);
        bool VisitHasPrescription(Guid Guid);
        void UpdateMedicineInPrescription(PrescriptionDetailViewModel medicine);
        List<PrescriptionTestDetailViewModel> GetAllVisitPrescriptionOtherAnalysis(Guid id);
    }
}
