using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IMedicineDiseaseRepository : IRepository<MedicineDisease>
    {
        IEnumerable<MedicineDisease> GetAllMedicine_Diseases(Guid diseaseId);
        void AddAllMedicinesForDisease(IEnumerable<MedicineDisease> disease_Symptom);
    }
}
