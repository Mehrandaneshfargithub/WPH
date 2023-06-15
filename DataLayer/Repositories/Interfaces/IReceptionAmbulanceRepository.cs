using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReceptionAmbulanceRepository : IRepository<ReceptionAmbulance>
    {
        IEnumerable<ReceptionAmbulance> GetAllReceptionAmbulance();
        ReceptionAmbulance GetReceptionAmbulanceWithHospital(Guid receptionId);
    }
}
