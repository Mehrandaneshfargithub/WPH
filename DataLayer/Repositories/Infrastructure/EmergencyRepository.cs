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

    public class EmergencyRepository : Repository<Emergency>, IEmergencyRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public EmergencyRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<Emergency> GetAllEmergency()
        {
            return Context.Emergencies.AsNoTracking();
        }

        public List<Reception> GetAllReceptionsByClinicSection(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo)
        {
            return Context.Emergencies
                .AsNoTracking()
                 .Include(x => x.Reception).ThenInclude(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                 .Select(x => (x.Reception==null) ? null : new Reception
                 {
                     CreatedDate = x.Reception.CreatedDate,
                     Discount = x.Reception.Discount,
                     Guid = x.Guid,
                     ReceptionDate = x.Reception.ReceptionDate,
                     ReceptionNum = x.Reception.ReceptionNum,
                     Patient = (x.Reception.Patient == null) ? null : new Patient
                     {
                         DateOfBirth = x.Reception.Patient.DateOfBirth,
                         User = new User
                         {
                             Name = x.Reception.Patient.User.Name,
                             GenderId = x.Reception.Patient.User.GenderId,
                             PhoneNumber = x.Reception.Patient.User.PhoneNumber,
                             Gender = x.Reception.Patient.User.Gender
                         }
                     },


                 }).Where(x => x.CreatedDate <= dateTo && x.CreatedDate > dateFrom).ToList();
        }

        public Reception GetEmergency(Guid emergencyId)
        {
            return Context.Receptions
                .AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                 .Include(x => x.Patient).ThenInclude(x => x.Address)
                 .Include(x => x.ReceptionAmbulances)
                 .Include(x => x.Surgeries).ThenInclude(x => x.SurgeryDoctors)
                 .Include(x => x.Emergency)
                 .Select(x => new Reception
                 {
                    CreatedDate = x.CreatedDate,
                    Discount = x.Discount,
                    Guid = x.Guid,
                    ReceptionDate = x.ReceptionDate,
                    ReceptionNum = x.ReceptionNum,
                    PurposeId = x.PurposeId,
                    Patient = new Patient
                    {
                        DateOfBirth = x.Patient.DateOfBirth,
                        MotherName = x.Patient.MotherName,
                        Address = x.Patient.Address,
                        User = new User
                        {
                            Name = x.Patient.User.Name,
                            GenderId = x.Patient.User.GenderId,
                            PhoneNumber = x.Patient.User.PhoneNumber,
                            Gender = x.Patient.User.Gender
                        }
                    },
                    CreatedUserId = x.CreatedUserId,
                    Description = x.Description,
                    DiscountCurrencyId = x.DiscountCurrencyId,
                    Emergency = x.Emergency,
                    //{
                    //    ArrivalId = x.Emergency.ArrivalId,
                    //    CriticallyId = x.Emergency.CriticallyId,
                    //},
                    EntranceDate = x.EntranceDate,
                    ExitDate = x.ExitDate,
                    PatientAttendanceName = x.PatientAttendanceName,
                    PatientId = x.PatientId,
                    PoliceReport = x.PoliceReport,
                    Surgeries = (x.Surgeries == null) ? null : x.Surgeries.Select(a => new Surgery
                    {
                        Guid = a.Guid,
                        ReceptionId = a.ReceptionId,
                        SurgeryDate = a.SurgeryDate,
                        SurgeryDoctors = (a.SurgeryDoctors == null) ? null : a.SurgeryDoctors
                    }).ToList(),
                     ReceptionAmbulances = (x.ReceptionAmbulances == null) ? null : x.ReceptionAmbulances.Select(a => new ReceptionAmbulance
                     {
                         AmbulanceId = a.AmbulanceId,
                         Cost = a.Cost,
                         CostCurrencyId = a.CostCurrencyId,
                         Explanation = a.Explanation,
                         PatientHealthId = a.PatientHealthId,
                         FromHospitalId = a.FromHospitalId,
                         ReceptionId = a.ReceptionId,
                         ToHospitalId = a.ToHospitalId
                     }).ToList(),
                 }).SingleOrDefault(x=>x.Guid == emergencyId);
        }

        public IEnumerable<PatientImage> RemoveEmergency(Guid emergencyid)
        {
            _context.Emergencies.RemoveRange(_context.Emergencies.Where(x => x.Guid == emergencyid));
            var su = _context.Surgeries.SingleOrDefault(x => x.ReceptionId == emergencyid);
            if (su != null)
            {
                _context.SurgeryDoctors.RemoveRange(_context.SurgeryDoctors.Where(x => x.SurgeryId == su.Guid));
                //_context..RemoveRange(_context.SurgeryDoctors.Where(x => x.SurgeryId == su.Guid));
            }

            _context.Emergencies.RemoveRange(_context.Emergencies.Where(x => x.Guid == emergencyid));
            _context.Surgeries.RemoveRange(_context.Surgeries.Where(x => x.Guid == emergencyid));
            _context.ReceptionAmbulances.RemoveRange(_context.ReceptionAmbulances.Where(x => x.ReceptionId == emergencyid));
            _context.ReceptionClinicSections.RemoveRange(_context.ReceptionClinicSections.Where(x => x.ReceptionId == emergencyid));
            _context.ReceptionRoomBeds.RemoveRange(_context.ReceptionRoomBeds.Where(x => x.ReceptionId == emergencyid));
            var rs = _context.ReceptionServices.SingleOrDefault(x=>x.ReceptionId == emergencyid);
            if (rs != null)
            {
                _context.ReceptionServiceReceiveds.RemoveRange(_context.ReceptionServiceReceiveds.Where(x => x.ReceptionServiceId == rs.Guid));
            }
            _context.ReceptionServices.RemoveRange(_context.ReceptionServices.Where(x => x.ReceptionId == emergencyid));
            _context.Receptions.Remove(_context.Receptions.SingleOrDefault(x => x.Guid == emergencyid));

            var images = _context.PatientImages.Where(p => p.ReceptionId == emergencyid);
            _context.RemoveRange(images);
            return images;
        }
    }
}
