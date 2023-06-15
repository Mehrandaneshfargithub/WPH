using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories
{
    public interface IDiseaseRepository : IRepository<Disease>
    {
        IEnumerable<Disease> GetAllDiseases(Guid clinicSectionId);
        IEnumerable<Disease> GetAllDiseasesJustNameAndGuid(Guid clinicSectionId);
        IEnumerable<Disease> GetAllDiseaseForListBox(Guid clinicSectionId, Guid patientId);
        IEnumerable<PieChartModel> GetMostUsedDisease(Guid userId);
    }
}