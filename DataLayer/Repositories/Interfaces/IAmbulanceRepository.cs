using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IAmbulanceRepository : IRepository<Ambulance>
    {
        IEnumerable<Ambulance> GetAllAmbulance();
        Ambulance GetAmbulance(Guid ambulanceId);
    }
}
