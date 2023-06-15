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
    public class ReceptionDoctorRepository : Repository<ReceptionDoctor>, IReceptionDoctorRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionDoctorRepository(WASContext context)
            : base(context)
        {
        }


        public IEnumerable<ReceptionDoctor> GetReceptionDoctor(Guid receptionId)
        {
            return _context.ReceptionDoctors.AsNoTracking()
                .Include(a => a.DoctorRole)
                .Include(a => a.Doctor).ThenInclude(a => a.User)
                .Where(p => p.ReceptionId == receptionId)
                .Select(s => new ReceptionDoctor
                {
                    DoctorId = s.DoctorId,
                    DoctorRole = new BaseInfoGeneral
                    {
                        Name = s.DoctorRole.Name
                    },
                    Doctor = new Doctor
                    {
                        SpecialityId = s.Doctor.SpecialityId,
                        User = new User
                        {
                            Name = s.Doctor.User.Name
                        }
                    }
                });

        }
    }
}
