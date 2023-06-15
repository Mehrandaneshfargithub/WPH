using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class ClinicRepository : Repository<Clinic>, IClinicRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ClinicRepository(WASContext context)
            : base(context)
        {
        } 

        public Clinic GetFirst()
        {
            return _context.Clinics.AsNoTracking()
                .FirstOrDefault();
        }
    }
}
