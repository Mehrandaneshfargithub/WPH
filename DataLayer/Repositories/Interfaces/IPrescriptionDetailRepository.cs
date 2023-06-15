using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPrescriptionDetailRepository : IRepository<PrescriptionDetail>
    {
        IEnumerable<PrescriptionDetail> GetAllPrescriptionDetail(Guid VisitId);
        bool VisitHasPrescription(Guid Guid);
        void UpdateMedicineInPrescription(PrescriptionDetail preDto);
    }
}
