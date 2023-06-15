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

    public class ReceptionAmbulanceRepository : Repository<ReceptionAmbulance>, IReceptionAmbulanceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionAmbulanceRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<ReceptionAmbulance> GetAllReceptionAmbulance()
        {
            return Context.ReceptionAmbulances.AsNoTracking();
        }

        public ReceptionAmbulance GetReceptionAmbulanceWithHospital(Guid receptionId)
        {
            return Context.ReceptionAmbulances.AsNoTracking()
                .Include(p => p.FromHospital)
                .Include(p => p.ToHospital)
                .SingleOrDefault(p => p.ReceptionId == receptionId);
        }
    }
}
