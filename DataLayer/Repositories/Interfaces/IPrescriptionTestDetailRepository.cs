using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPrescriptionTestDetailRepository : IRepository<PrescriptionTestDetail>
    {
        IEnumerable<PrescriptionTestDetail> GetAllPrescriptionTestDetail(Guid visitId);
    }
}
