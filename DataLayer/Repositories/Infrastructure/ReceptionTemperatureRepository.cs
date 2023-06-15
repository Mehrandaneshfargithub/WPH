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
    public class ReceptionTemperatureRepository : Repository<ReceptionTemperature>, IReceptionTemperatureRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionTemperatureRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReceptionTemperature> GetAllReceptionTemperatures(Guid ReceptionId)
        {
            return Context.ReceptionTemperatures.AsNoTracking()
                .Where(x => x.ReceptionId == ReceptionId);

        }

    }
}
