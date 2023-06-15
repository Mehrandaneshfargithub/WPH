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
    public class AmbulanceRepository : Repository<Ambulance>, IAmbulanceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AmbulanceRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<Ambulance> GetAllAmbulance()
        {
            return Context.Ambulances.Include(a => a.Hospital).AsNoTracking();
        }

        public Ambulance GetAmbulance(Guid ambulanceId)
        {
            return Context.Ambulances.Include(a=>a.Hospital).AsNoTracking().SingleOrDefault(x=>x.Guid == ambulanceId);
        }
    }
}
