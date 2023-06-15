using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.Disease;

namespace WPH.MvcMockingServices.Interface
{
    public interface IDiseaseMvcMockingService
    {
        IEnumerable<DiseaseViewModel> GetAllDiseases(Guid clinicSectionId);
        OperationStatus RemoveDisease(Guid Diseaseid);
        string AddNewDisease(DiseaseViewModel Disease);
        string UpdateDisease(DiseaseViewModel dese);
        DiseaseViewModel GetDisease(Guid DiseaseId);
        IEnumerable<Medicine_DiseaseViewModel> GetAllMedicinesForDisease(Guid DiseaseId);
        IEnumerable<DiseaseViewModel> GetAllDiseasesJustNameAndGuid(Guid clinicSectionId);
        void AddAllSymptomForDisease(string itemList, Guid diseaseId);
        void AddAllMedicineForDisease(string itemList, Guid diseaseId);
        bool CheckRepeatedDiseaseName(string name, Guid clinicSectionId, bool NewOrUpdate, string oldName = "");
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<DiseaseViewModel> GetAllDiseaseForListBox(Guid clinicSectionId, Guid patientId);
        PieChartViewModel GetMostUsedDisease(Guid userId);
    }
}
