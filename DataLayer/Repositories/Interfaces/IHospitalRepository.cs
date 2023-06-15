using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IHospitalRepository : IRepository<Hospital>
    {
        IEnumerable<Hospital> GetAllHospital();
        Hospital GetHospitalByName(string name);
    }
}
