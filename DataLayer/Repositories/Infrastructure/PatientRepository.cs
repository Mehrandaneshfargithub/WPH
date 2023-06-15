using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        protected readonly WASContext _context;

        public PatientRepository(WASContext context) : base(context)
        {
            _context = context;
        }

        //public WASContext _context
        //{
        //    get { return Context as WASContext; }
        //}
        //public PatientRepository(WASContext context)
        //    : base(context)
        //{
        //}


        public IEnumerable<Patient> GetAllPatientS(bool forGrid, Guid? clinicSectionId)
        {
            IQueryable<Patient> result = _context.Patients
                .AsNoTracking()
                .Include(x => x.User).ThenInclude(x => x.Gender)
                .Include(x => x.Address);

            if (clinicSectionId != null)
            {
                result = result.Where(x => x.User.ClinicSectionId == clinicSectionId);
            }
            if (!forGrid)
            {

                result = result.Take(20);
            }

            return result;

        }

        public IEnumerable<Patient> GetAllPatientForFilter(Guid clinicSectionId)
        {
            return _context.Patients
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Address)
                .Include(x => x.MotherJob)
                .Include(x => x.FatherJob)
                .Where(x => x.User.ClinicSectionId == clinicSectionId)
                .Select(p => new Patient
                {
                    Guid = p.Guid,
                    DateOfBirth = p.DateOfBirth,
                    MotherName = p.MotherName,
                    FileNum = p.FileNum,
                    FormNumber = p.FormNumber,
                    Address = (p.Address == null) ? null : new BaseInfo
                    {
                        Id = p.Address.Id,
                        Name = p.Address.Name
                    },
                    FatherJob = (p.FatherJob == null) ? null : new BaseInfo
                    {
                        Id = p.FatherJob.Id,
                        Name = p.FatherJob.Name
                    },
                    MotherJob = (p.MotherJob == null) ? null : new BaseInfo
                    {
                        Id = p.MotherJob.Id,
                        Name = p.MotherJob.Name
                    },
                    User = new User
                    {
                        Name = p.User.Name,
                        GenderId = p.User.GenderId,
                        PhoneNumber = p.User.PhoneNumber,
                        Email = p.User.Email,

                    }
                });

        }

        public IEnumerable<Patient> GetPatientJustNameAndGuid(Guid clinicSectionId)
        {
            return _context.Patients
                .AsNoTracking()
                .Where(x => x.User.ClinicSectionId == clinicSectionId)
                .Include(x => x.User).Select(x => new Patient
                {
                    Guid = x.Guid,
                    User = new User
                    {
                        Name = x.User.Name,
                        ClinicSectionId = x.User.ClinicSectionId
                    }
                });
        }

        public string GetLatestPatientFileNum(Guid clinicSectionId, Guid clinicId)
        {
            return _context.FN_LatestFileNum(clinicSectionId, clinicId).FirstOrDefault().CODE.ToString();

        }
        public Patient GetPatient(Guid patientId)
        {
            return _context.Patients
                .AsNoTracking()
                .Include(x => x.User).ThenInclude(x => x.Gender)
                .Include(x => x.User).ThenInclude(x => x.UserType)
                .SingleOrDefault(x => x.Guid == patientId);
        }

        public Patient GetPatientIdAndNameFromReserveDetailId(Guid reserveDetailId)
        {
            try
            {
                return _context.ReserveDetails
                     .AsNoTracking()
                 .Where(x => x.Guid == reserveDetailId)
                 .Include(x => x.Patient).ThenInclude(x => x.User)
                 .Select(x => new Patient
                 {
                     Guid = x.Patient.Guid,
                     User = new User
                     {
                         Name = x.Patient.User.Name
                     }
                 }).FirstOrDefault();
            }
            catch { return new Patient(); }
        }

        public Patient GetPatientByName(string name, Guid ClinicSectionId)
        {
            return _context.Patients
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefault(x => x.User.Name == name && x.User.ClinicSectionId == ClinicSectionId);
        }

        public IEnumerable<Patient> GetAllClinicPatients(Guid clinicSectionId)
        {
            try
            {
                //return _context.Patients
                //.AsNoTracking()
                //.Include(x => x.User).ThenInclude(a=>a.ClinicSection).ThenInclude(a => a.ClinicSectionType)
                //.Where(a => a.User.ClinicSection.SectionType.Name.ToLower() == "lab");

                return _context.ReceptionClinicSections
                .AsNoTracking()
                .Include(x => x.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User).ThenInclude(a => a.ClinicSection).ThenInclude(a => a.ClinicSectionType)
                .Where(a => a.DestinationReception.ClinicSection.SectionType.Name.ToLower() == "lab").Select(c => c.Reception.Patient);
            }
            catch { return null; }
        }

        
    }
}
