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
    public class HospitalRepository : Repository<Hospital>, IHospitalRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public HospitalRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<Hospital> GetAllHospital()
        {
            return _context.Hospitals.AsNoTracking();
        }

        public Hospital GetHospitalByName(string name)
        {
            return _context.Hospitals.AsNoTracking()
                .FirstOrDefault(p => p.Name == name);
        }
    }
}
