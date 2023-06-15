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
    public class ReceptionInsuranceReceivedRepository : Repository<ReceptionInsuranceReceived>, IReceptionInsuranceReceivedRepository
    { 
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionInsuranceReceivedRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReceptionInsuranceReceived> GetAllReceptionInsuranceReceived(Guid receptionId)
        {
            //return _context.ReceptionInsuranceReceiveds.Include(x => x.ReceptionInsuranceReceiveds)
            //    .Where(a=>a.ReceptionId == receptionId)
            //    .Select(x=>x.ReceptionInsuranceReceiveds).ToList();
            return _context.ReceptionInsuranceReceiveds.Include(x => x.ReceptionInsurance)
                .Where(a=>a.ReceptionInsurance.ReceptionId == receptionId);
        }
    }
}
