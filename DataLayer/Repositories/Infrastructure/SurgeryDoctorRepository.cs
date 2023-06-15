using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class SurgeryDoctorRepository : Repository<SurgeryDoctor>, ISurgeryDoctorRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SurgeryDoctorRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<SurgeryDoctor> GetDoctorsAndRoleBySurgeryId(Guid surgeryId)
        {
            return _context.SurgeryDoctors.AsNoTracking()
                .Where(p => p.SurgeryId == surgeryId)
                .Select(s => new SurgeryDoctor
                {
                    Guid = s.Guid,
                    DoctorId = s.DoctorId,
                    DoctorRoleId = s.DoctorRoleId,
                    DoctorRole = new BaseInfoGeneral
                    {
                        Name = s.DoctorRole.Name
                    }
                });
        }

        public SurgeryDoctor GetDoctorBySurgeryAndRoleId(Guid surgeryId, int? roleId)
        {
            return _context.SurgeryDoctors.AsNoTracking()
                .FirstOrDefault(p => p.SurgeryId == surgeryId && p.DoctorRoleId == roleId);
        }
    }
}
