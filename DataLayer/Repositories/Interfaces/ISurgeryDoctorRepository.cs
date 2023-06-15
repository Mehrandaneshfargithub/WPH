using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISurgeryDoctorRepository : IRepository<SurgeryDoctor>
    {
        IEnumerable<SurgeryDoctor> GetDoctorsAndRoleBySurgeryId(Guid surgeryId);
        SurgeryDoctor GetDoctorBySurgeryAndRoleId(Guid surgeryId,int? roleId);
    }
}
