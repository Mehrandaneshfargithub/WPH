using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class PrescriptionDetailRepository : Repository<PrescriptionDetail>, IPrescriptionDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PrescriptionDetailRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PrescriptionDetail> GetAllPrescriptionDetail(Guid VisitId)
        {
            return _context.PrescriptionDetails.AsNoTracking()
                .Where(x => x.ReceptionId == VisitId)
                .Include(x => x.Medicine)
                .Include(x => x.ModifiedUser)
                .Select(x => new PrescriptionDetail
                {
                    ConsumptionInstruction = x.ConsumptionInstruction,
                    Explanation = x.Explanation,
                    Id = x.Id,
                    Guid = x.Guid,
                    Num = x.Num,
                    MedicineId = x.MedicineId,
                    ReceptionId = x.ReceptionId,
                    ClinicSectionId = x.ClinicSectionId,
                    Medicine = new Medicine
                    {
                        JoineryName = x.Medicine.JoineryName
                    },
                    ModifiedUser = new User
                    {
                        Name = x.ModifiedUser.Name
                    }
                });
        }

        public bool VisitHasPrescription(Guid Guid)
        {
            bool exist = false;
            if (_context.PrescriptionDetails.Any(x => x.ReceptionId == Guid) || _context.PrescriptionTestDetails.Any(x => x.ReceptionId == Guid))
                exist = true;
            return exist;
        }

        public void UpdateMedicineInPrescription(PrescriptionDetail preDto)
        {
            try
            {
                PrescriptionDetail pre = _context.PrescriptionDetails.SingleOrDefault(x => x.Guid == preDto.Guid);
                pre.Num = preDto.Num;
                pre.ConsumptionInstruction = preDto.ConsumptionInstruction;
                pre.Explanation = preDto.Explanation;
                _context.SaveChanges();
            }
            catch (Exception e) { throw e; }

        }
    }
}
