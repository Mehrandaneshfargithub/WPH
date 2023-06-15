using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReceptionInsuranceRepository : Repository<ReceptionInsurance>, IReceptionInsuranceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionInsuranceRepository(WASContext context)
            : base(context)
        {
        }

        public ReceptionInsurance GetReceptionInsuranceWithReceiveds(Guid receptionId)
        {
            return _context.ReceptionInsurances
                .Include(c => c.ReceptionInsuranceReceiveds)
                .Where(a => a.ReceptionId == receptionId).Select(c => new ReceptionInsurance
                {
                    CreatedDate = c.CreatedDate,
                    ReceptionInsuranceReceiveds = c.ReceptionInsuranceReceiveds,
                    Reception = new Reception
                    {
                        Patient = new Patient
                        {
                            User = new User
                            {
                                Name = c.Reception.Patient.User.Name,
                            },
                            DateOfBirth = c.Reception.Patient.DateOfBirth

                        },
                        ReceptionDate = c.Reception.ReceptionDate,
                        ReceptionNum = c.Reception.ReceptionNum

                    }
                }).FirstOrDefault();
                
        }

    }
}
