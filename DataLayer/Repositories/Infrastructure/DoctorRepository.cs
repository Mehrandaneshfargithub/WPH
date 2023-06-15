using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public DoctorRepository(WASContext context)
            : base(context)
        {
        }

        public Doctor CheckRepeatedDoctorNameAndPhone(Guid clinicSectionId, string name, string phoneNumber, string nameHolder = "", string phoneNumberHolder = "")
        {
            return _context.Doctors.AsNoTracking().Include(x => x.User)
                .Where(x => x.ClinicSectionId == clinicSectionId && x.User.Name == name && x.User.PhoneNumber == phoneNumber && x.User.Name != nameHolder && x.User.PhoneNumber != phoneNumberHolder).FirstOrDefault();
        }

        public IEnumerable<Doctor> GetAllDoctors(bool forGrid, Guid? clinicSectionId)
        {
            IQueryable<Doctor> allDoctor = _context.Doctors.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Speciality);
            if (clinicSectionId != null)
            {
                allDoctor = allDoctor.Where(x => x.ClinicSectionId == clinicSectionId);
            }
            if (!forGrid)
            {

                allDoctor = allDoctor.Take(20);
            }


            return allDoctor;

        }

        public IEnumerable<Doctor> GetAllDoctorsForFilter(Guid clinicSectionId)
        {
            return _context.Doctors.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Speciality)
                .Where(x => x.ClinicSectionId == clinicSectionId).Select(p => new Doctor
                {
                    Guid = p.Guid,
                    User = new User
                    {
                        Name = p.User.Name
                    },
                    Speciality = (p.Speciality == null) ? null : new BaseInfo
                    {
                        Name = p.Speciality.Name
                    }
                });

        }

        public Doctor CheckRepeatedDoctorNameAndSpeciallity(Guid clinicSectionId, string name, Guid? specialityId, string nameHolder = "", Guid? specialityIdHolder = null)
        {
            return _context.Doctors.AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.ClinicSectionId == clinicSectionId && x.User.Name.Trim() == name.Trim()/* && x.SpecialityId == specialityId*/ && x.User.Name.Trim() != nameHolder.Trim()/* && x.SpecialityId != specialityIdHolder*/).FirstOrDefault();

        }

        public Doctor GetDoctorById(Guid doctorId)
        {
            return _context.Doctors.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Speciality)
                .Select(p => new Doctor
                {
                    Guid = p.Guid,
                    SpecialityId = p.SpecialityId,
                    Explanation = p.Explanation,
                    MedicalSystemCode = p.MedicalSystemCode,
                    User = new User
                    {
                        Name = p.User.Name,
                        UserName = p.User.Name,
                        PhoneNumber = p.User.PhoneNumber
                    }
                })
                .SingleOrDefault(x => x.Guid == doctorId);
        }

        public Doctor GetDoctorByName(Guid clinicSectionId, string name)
        {
            return _context.Doctors.AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.ClinicSectionId == clinicSectionId && x.User.Name == name)
                .FirstOrDefault();
        }

        public IEnumerable<Doctor> GetDoctorsBasedOnUserSection(List<Guid> sections)
        {
            return _context.ClinicSectionUsers.AsNoTracking()
               .Include(p => p.User).ThenInclude(p => p.Doctor)
               .Include(p => p.User).ThenInclude(p => p.UserType)
               .Where(p => p.User.Doctor != null && p.User.Active.Value && p.User.Active != null && p.User.UserType.Name == "Doctor" && sections.Contains(p.ClinicSectionId))
               .Select(x => new Doctor
               {
                   Guid = x.User.Guid,
                   User = new User
                   {
                       Name = x.User.Name
                   }
               }).Distinct();
        }
    }
}
