using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class PrescriptionTestDetailRepository : Repository<PrescriptionTestDetail>, IPrescriptionTestDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PrescriptionTestDetailRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PrescriptionTestDetail> GetAllPrescriptionTestDetail(Guid visitId)
        {
            return Context.PrescriptionTestDetails.AsNoTracking()
                .Where(x => x.ReceptionId == visitId)
                .Include(x => x.Test)
                .Include(x => x.ModifiedUser)
                .Select(x => new PrescriptionTestDetail
                {
                    Explanation = x.Explanation,
                    Guid = x.Guid,
                    Id = x.Id,
                    TestId = x.TestId,
                    ReceptionId = x.ReceptionId,
                    ClinicSectionId = x.ClinicSectionId,
                    AnalysisName = x.AnalysisName,
                    Test = x.Test,
                    ModifiedUser = new User
                    {
                        Name = x.ModifiedUser.Name
                    }
                    
                });
        }
    }
}
