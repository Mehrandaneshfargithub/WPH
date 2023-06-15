using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReserveDetailRepository : Repository<ReserveDetail>, IReserveDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReserveDetailRepository(WASContext context)
            : base(context)
        {
        }


        public IEnumerable<ReserveDetail> GetAllReserveDetail(Guid ClinicSectionId)
        {
            return _context.ReserveDetails
                .AsNoTracking()
                .Include(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.Status)
                .Where(x => x.Master.ClinicSectionId == ClinicSectionId);
        }


        public ReserveDetail GetReserveDetail(Guid resAllD)
        {
            return _context.ReserveDetails
                .Include(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.Patient).ThenInclude(x => x.Address)
                .Include(x => x.Patient).ThenInclude(x => x.FatherJob)
                .Include(x => x.Patient).ThenInclude(x => x.MotherJob)
                .Include(x => x.Status)
                .FirstOrDefault(x => x.Guid == resAllD);
        }


        public ReserveDetail GetReserveDetailDoctorId(Guid reserveDetailId)
        {
            return _context.ReserveDetails
                .AsNoTracking()
                .SingleOrDefault(x => x.Guid == reserveDetailId);
        }


        public IEnumerable<ReserveDetail> GetAllReserveDetailsforEvent(Guid ClinicSectionId)
        {
            return _context.ReserveDetails
                .AsNoTracking()
                .Where(x => x.Master.ClinicSectionId == ClinicSectionId)
                .Include(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.Status);
        }

        public IEnumerable<ReserveDetail> GetAllReservesBetweenTwoDateBasedOnUserAccess(List<Guid> doctors, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return _context.ReserveDetails
                    .AsNoTracking()
                    .Where(x => x.DoctorId != null && doctors.Contains(x.DoctorId.Value) && x.ReserveDate >= fromDate && x.ReserveDate <= toDate)
                    .Include(x => x.Patient.User)
                    .Include(x => x.Status);

            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<ReserveDetail> GetAllReservesBetweenTwoDateByDocotrId(Guid doctorId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return _context.ReserveDetails
                    .AsNoTracking()
                    .Where(x => x.DoctorId == doctorId && x.ReserveDate >= fromDate && x.ReserveDate <= toDate)
                    .Include(x => x.Patient.User)
                    .Include(x => x.Status);

            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<FN_GetAllEventsForCalendar_Result> GetAllReservesBetweenTwoDateForCalendar(Guid originalClinicSectionId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, Guid doctorId)
        {
            try
            {
                return _context.FN_GetAllEventsForCalendar(originalClinicSectionId, clinicSectionId, fromDate, toDate, doctorId);
            }
            catch (Exception e) { throw e; }
        }

        public DateTime? GetLastPatientVisitDate(Guid patientId, bool recieved)
        {
            try
            {
                if (recieved)
                {
                    return _context.ReserveDetails.AsNoTracking().Where(x => x.PatientId == patientId).OrderByDescending(a => a.Id).ToList().ElementAt(1).ReserveDate;
                }
                else
                {
                    return _context.ReserveDetails.AsNoTracking().Where(x => x.PatientId == patientId).OrderByDescending(a => a.Id).FirstOrDefault().ReserveDate;
                }

            }
            catch { return null; }
        }

        public void UpdateReserveDetailTime(Guid reserveid, string start, string end)
        {
            try
            {
                ReserveDetail re = new ReserveDetail() { Guid = reserveid, ReserveStartTime = start, ReserveEndTime = end };
                _context.ReserveDetails.Attach(re);
                _context.Entry(re).Property(x => x.ReserveStartTime).IsModified = true;
                _context.Entry(re).Property(x => x.ReserveEndTime).IsModified = true;

                _context.SaveChanges();
            }
            catch (Exception e) { throw e; }


        }

        public void AddNewReserveDetail(ReserveDetail res, bool newPatient)
        {
            try
            {
                if (!newPatient)
                {

                    Context.Patients.Attach(res.Patient);
                    Context.Users.Attach(res.Patient.User);
                    Context.Entry(res.Patient).Property(x => x.AddressId).IsModified = true;
                    Context.Entry(res.Patient).Property(x => x.DateOfBirth).IsModified = true;
                    Context.Entry(res.Patient).Property(x => x.FatherJobId).IsModified = true;
                    Context.Entry(res.Patient).Property(x => x.MotherJobId).IsModified = true;

                    Context.Entry(res.Patient.User).Property(x => x.GenderId).IsModified = true;
                    Context.Entry(res.Patient.User).Property(x => x.PhoneNumber).IsModified = true;
                }



                Context.ReserveDetails.Add(res);
                Context.SaveChanges();

            }
            catch (Exception e) { throw e; }
        }

        public void UpdateReserveDetail(ReserveDetail res, bool newPatient)
        {
            try
            {
                Context.Entry(res).State = EntityState.Detached;
                if (!newPatient)
                {

                    Context.ReserveDetails.Attach(res);
                    Context.Patients.Attach(res.Patient);
                    Context.Users.Attach(res.Patient.User);
                    Context.Entry(res.Patient).Property(x => x.AddressId).IsModified = true;
                    Context.Entry(res.Patient).Property(x => x.DateOfBirth).IsModified = true;
                    Context.Entry(res.Patient).Property(x => x.FatherJobId).IsModified = true;
                    Context.Entry(res.Patient).Property(x => x.MotherJobId).IsModified = true;

                    Context.Entry(res.Patient.User).Property(x => x.GenderId).IsModified = true;
                    Context.Entry(res.Patient.User).Property(x => x.PhoneNumber).IsModified = true;
                }
                else
                {
                    Context.Entry(res.Patient.User).State = EntityState.Added;
                    Context.Entry(res.Patient).State = EntityState.Added;
                }

                Context.Entry(res).State = EntityState.Modified;
                Context.Entry(res).Property(x => x.ReserveDate).IsModified = false;


                var reception = _context.Receptions.AsNoTracking().FirstOrDefault(p => p.ReserveDetailId == res.Guid);
                if (reception != null)
                {
                    reception.PatientId = res.PatientId;
                    _context.Entry(reception).State = EntityState.Modified;
                }

                Context.SaveChanges();

            }
            catch (Exception e) { throw e; }
        }

        public Guid GetPatientIdFromReserveDetailId(Guid reserveDetailId)
        {
            return _context.ReserveDetails.AsNoTracking().SingleOrDefault(x => x.Guid == reserveDetailId).PatientId ?? Guid.Empty;
        }

        public string RemoveReserveDetail(Guid reserveDetailId)
        {
            Guid visitId = Guid.Empty;
            try
            {
                var visit = _context.Receptions.Include(p => p.Status).SingleOrDefault(x => x.ReserveDetailId == reserveDetailId);
                //if (visit?.Status.Name == "Visited")
                //    return "Visited";

                visitId = visit.Guid;
            }
            catch { };

            if (visitId != Guid.Empty)
            {
                _context.PrescriptionDetails.RemoveRange(_context.PrescriptionDetails.Where(x => x.ReceptionId == visitId));
                _context.PrescriptionTestDetails.RemoveRange(_context.PrescriptionTestDetails.Where(x => x.ReceptionId == visitId));
                _context.PatientVariablesValues.RemoveRange(_context.PatientVariablesValues.Where(x => x.ReceptionId == visitId));
                _context.VisitPatientDiseases.RemoveRange(_context.VisitPatientDiseases.Where(x => x.ReceptionId == visitId));
                _context.PatientImages.RemoveRange(_context.PatientImages.Where(x => x.ReceptionId == visitId));
                _context.ReceptionServices.RemoveRange(_context.ReceptionServices.Where(x => x.ReceptionId == visitId));


                if (_context.Receptions.Local.Any(x => x.Guid == visitId))
                {
                    _context.Entry(_context.Receptions.Local.SingleOrDefault(x => x.Guid == visitId)).State = EntityState.Deleted;
                }
                else
                {
                    Reception visit = new Reception() { Guid = visitId };
                    _context.Receptions.Attach(visit);
                    _context.Entry(visit).State = EntityState.Deleted;
                }

            }

            if (_context.ReserveDetails.Local.Any(x => x.Guid == reserveDetailId))
            {
                _context.Entry(_context.ReserveDetails.Local.SingleOrDefault(x => x.Guid == reserveDetailId)).State = EntityState.Deleted;
                Context.SaveChanges();
            }
            else
            {
                ReserveDetail res = new ReserveDetail() { Guid = reserveDetailId };
                _context.ReserveDetails.Attach(res);
                _context.Entry(res).State = EntityState.Deleted;
                Context.SaveChanges();
            }

            return "";
        }

        public void UpdateReserveDetailStatus(ReserveDetail res)
        {
            if (_context.ReserveDetails.Local.Any(x => x.Guid == res.Guid))
            {
                _context.Entry(_context.ReserveDetails.Local.SingleOrDefault(x => x.Guid == res.Guid)).Property(x => x.StatusId).IsModified = true;
                Context.SaveChanges();
            }
            else
            {

                _context.ReserveDetails.Attach(res);
                _context.Entry(res).Property(x => x.StatusId).IsModified = true;
                _context.SaveChanges();
            }
            _context.Entry(res).State = EntityState.Detached;
        }

        public IEnumerable<ReserveDetail> GetAllNotVisitedPatients(Guid clinicSectionId)
        {
            try
            {
                DateTime today = DateTime.Now.Date;
                return _context.ReserveDetails.Include(x => x.Master).AsNoTracking().Where(x => x.Master.ClinicSectionId == clinicSectionId && x.ReserveDate >= today && x.StatusId == 8)
                    .Select(a => new ReserveDetail
                    {
                        ReserveDate = a.ReserveDate
                    });
            }
            catch (Exception e) { throw e; }
        }

        public ReserveDetail GetWithReception(Guid reserveDetailId)
        {
            return _context.ReserveDetails.AsNoTracking()
                .Include(p => p.Receptions)
                .SingleOrDefault(p => p.Guid == reserveDetailId);
        }

        public ReserveDetail GetLastReserveDetail(Guid reserveDetailId, Guid? patientId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(p => p.ReserveDetail)
                .Where(p => p.ReserveDetailId != reserveDetailId && p.PatientId == patientId)
                .OrderByDescending(p => p.ReceptionDate)
                .Select(p => p.ReserveDetail)
                .FirstOrDefault();
        }

        public ReserveDetail GetNoTracking(Guid reserveDetailId)
        {
            return _context.ReserveDetails.AsNoTracking()
                .SingleOrDefault(p => p.Guid == reserveDetailId);
        }
    }
}
