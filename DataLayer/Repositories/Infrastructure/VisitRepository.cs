using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    class VisitRepository : Repository<Reception>, IVisitRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public VisitRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Reception> GetAllVisitByClinicSection(Guid clinicSection)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.Status)
                .Include(p => p.ReceptionType).ThenInclude(p => p.Type)
                .Where(x => x.ReceptionType.Type.Ename == "ReceptionType" && x.ReceptionType.Name == "VisitReception" && x.ClinicSectionId == clinicSection);
        }

        public int GetLastClinicSectionVisitNum(Guid clinicSectonId, DateTime date)
        {
            return (_context.Receptions.AsNoTracking()
                .Include(p => p.ReceptionType).ThenInclude(p => p.Type)
                .Where(p => p.ReceptionType.Type.Ename == "ReceptionType" && p.ReceptionType.Name == "VisitReception" && p.ClinicSectionId == clinicSectonId && p.ReceptionDate != null && p.ReceptionDate.Value.Date == date.Date)
                .OrderByDescending(o => o.VisitNum).FirstOrDefault()?.VisitNum ?? 0) + 1;
        }


        public IEnumerable<Reception> GetAllVisitForOneDayBasedOnDoctorId(Guid doctorId, DateTime? Date)
        {
            return _context.Receptions
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.FatherJob)
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.MotherJob)
                .Include(x => x.Status)
                .Where(x => x.ReceptionDate != null && x.ReceptionDate.Value.Date == Date && x.ReserveDetail.DoctorId == doctorId);
        }


        public IEnumerable<Reception> GetAllVisitForOneDayBasedOnDoctorIdJustStatusAndVisitNum(Guid doctorId, DateTime date)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail)
                .Include(x => x.Status)
                .Where(x => x.ReceptionDate != null && x.ReceptionDate.Value == date && x.ReserveDetail.DoctorId == doctorId)
                .Select(x => new Reception
                {
                    VisitNum = x.VisitNum,
                    Status = new BaseInfoGeneral
                    {
                        Name = x.Status.Name
                    }
                });
        }

        public IEnumerable<Reception> GetAllPatientVisitByClinicSection(Guid patientId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User)
                .Where(x => x.ReserveDetail.PatientId == patientId);
        }

        public Reception GetVisitDetailBasedOnId(Guid visitId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User)
                .Where(x => x.Guid == visitId)
                .Select(x => new Reception
                {
                    Guid = x.Guid,
                    ReserveDetailId = x.ReserveDetailId,
                    ClinicSectionId = x.ClinicSectionId,
                    ReceptionDate = x.ReceptionDate,
                    ReceptionNum = x.ReceptionNum,
                    ReserveDetail = new ReserveDetail
                    {
                        PatientId = x.ReserveDetail.PatientId,
                        Patient = new Patient
                        {
                            User = new User
                            {
                                Name = x.ReserveDetail.Patient.User.Name
                            }
                        }
                    }
                })
                .SingleOrDefault();
        }

        public Reception GetVisitBasedOnReserveDetailId(Guid ReserveDetailId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User)

                .Where(x => x.ReserveDetailId == ReserveDetailId)
                .Select(x => new Reception
                {
                    Guid = x.Guid,
                    ReserveDetailId = x.ReserveDetailId,
                    ClinicSectionId = x.ClinicSectionId,
                    ReceptionDate = x.ReceptionDate,
                    ReceptionNum = x.ReceptionNum,
                    ReserveDetail = new ReserveDetail
                    {
                        PatientId = x.ReserveDetail.PatientId,
                        Patient = new Patient
                        {
                            User = new User
                            {
                                Name = x.ReserveDetail.Patient.User.Name
                            }
                        }
                    }
                }).FirstOrDefault();
        }

        public IEnumerable<Reception> GetAllVisitForSpecificDateByDoctorId(Guid doctorId, DateTime dateFrom, DateTime dateTo)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                .Include(x => x.Status)
                .Where(x => x.ReserveDetail != null && x.ReserveDetail.DoctorId != null && x.ReserveDetail.DoctorId == doctorId && x.ReceptionDate != null && x.ReceptionDate.Value >= dateFrom && x.ReceptionDate.Value <= dateTo);
        }

        public IEnumerable<Reception> GetAllVisitForSpecificDateBasedOnUserAccess(List<Guid> doctors, DateTime dateFrom, DateTime dateTo)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                .Include(x => x.Status)
                .Where(x => x.ReserveDetail != null && x.ReserveDetail.DoctorId != null && doctors.Contains(x.ReserveDetail.DoctorId.Value) && x.ReceptionDate != null && x.ReceptionDate.Value >= dateFrom && x.ReceptionDate.Value <= dateTo);
        }

        public int GetAllTodayVisitsCountBasedOnClinicSection(Guid clinicSectionId, DateTime today)
        {
            return _context.Receptions.AsNoTracking()
                .Include(p => p.ReceptionType).ThenInclude(p => p.Type)
                .Count(x => x.ReceptionType.Type.Ename == "ReceptionType" && x.ReceptionType.Name == "VisitReception" && x.ClinicSectionId == clinicSectionId && x.ReceptionDate.Value.Date == today);
        }

        public async Task UpdateReceptionNums(Guid doctorId)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("DSP_RefreshReceptionNums {0}", doctorId);
            }
            catch (Exception e) { throw e; }
        }

        public void AddNewVisit(Reception vis, List<PatientVariablesValue> addValues, List<PatientVariablesValue> updatedValues)
        {
            try
            {

                ReserveDetail re = new ReserveDetail() { Guid = vis.ReserveDetailId ?? Guid.Empty, StatusId = vis.StatusId };
                if (_context.ReserveDetails.Local.Any(x => x.Guid == vis.ReserveDetailId))
                {
                    _context.ReserveDetails.Local.SingleOrDefault(x => x.Guid == vis.ReserveDetailId).StatusId = vis.StatusId;
                    _context.Entry(_context.ReserveDetails.Local.SingleOrDefault(x => x.Guid == vis.ReserveDetailId)).Property(x => x.StatusId).IsModified = true;
                    //Context.Entry(re).Property(x => x.StatusId).IsModified = true;
                }
                else
                {
                    Context.ReserveDetails.Attach(re);
                    Context.Entry(re).Property(x => x.StatusId).IsModified = true;
                }


                if (addValues.Count != 0)
                {
                    Context.PatientVariablesValues.AddRange(addValues);
                }
                if (updatedValues.Count != 0)
                {
                    foreach (var v in updatedValues)
                    {
                        Context.Entry(v).State = EntityState.Modified;
                    }
                }

                Context.Receptions.Add(vis);
            }
            catch (Exception e) { throw e; }
        }

        public void UpdateVisit(Reception vis, List<PatientVariablesValue> add, List<PatientVariablesValue> update)
        {
            try
            {

                if (add.Count != 0)
                {
                    Context.PatientVariablesValues.AddRange(add);
                }
                if (update.Count != 0)
                {
                    foreach (var v in update)
                    {
                        Context.PatientVariablesValues.Attach(v);
                        Context.Entry(v).Property(x => x.Value).IsModified = true;
                    }
                }

                ReserveDetail re = _context.ReserveDetails.AsNoTracking().SingleOrDefault(a => a.Guid == vis.ReserveDetailId)  /*new ReserveDetail() { Guid = vis.ReserveDetailId ?? Guid.Empty, StatusId = vis.StatusId }*/;
                re.StatusId = vis.StatusId;
                Context.Entry(re).State = EntityState.Modified;

                if (_context.Receptions.Local.Any(x => x.Guid == vis.Guid))
                {
                    _context.Entry(_context.Receptions.Local.SingleOrDefault(x => x.Guid == vis.Guid)).State = EntityState.Detached;
                    Context.Entry(vis).State = EntityState.Modified;
                }
                else
                {
                    _context.Receptions.Attach(vis);
                    Context.Entry(vis).State = EntityState.Detached;
                    Context.Entry(vis).State = EntityState.Modified;
                }


                Context.SaveChanges();

            }
            catch (Exception e) { throw e; }
        }

        public void UpdateVisitStatus(Reception vis)
        {

            if (_context.Receptions.Local.Any(x => x.Guid == vis.Guid))
            {
                _context.Entry(_context.Receptions.Local.SingleOrDefault(x => x.Guid == vis.Guid)).Property(x => x.StatusId).IsModified = true;
                _context.Entry(_context.Receptions.Local.SingleOrDefault(x => x.Guid == vis.Guid)).Property(x => x.ReceptionDate).IsModified = true;
                //Context.SaveChanges();
            }
            else
            {
                _context.Receptions.Attach(vis);
                _context.Entry(vis).Property(x => x.StatusId).IsModified = true;
                _context.Entry(vis).Property(x => x.ReceptionDate).IsModified = true;
                //_context.SaveChanges();
            }

        }

        public void RemoveVisit(Guid id)
        {
            _context.PrescriptionDetails.RemoveRange(_context.PrescriptionDetails.Where(x => x.ReceptionId == id));
            _context.PrescriptionTestDetails.RemoveRange(_context.PrescriptionTestDetails.Where(x => x.ReceptionId == id));
            _context.PatientVariablesValues.RemoveRange(_context.PatientVariablesValues.Where(x => x.ReceptionId == id));
            _context.VisitPatientDiseases.RemoveRange(_context.VisitPatientDiseases.Where(x => x.ReceptionId == id));
            _context.PatientImages.RemoveRange(_context.PatientImages.Where(x => x.ReceptionId == id));
            ReserveDetail re = new ReserveDetail
            {
                Guid = _context.Receptions.SingleOrDefault(x => x.Guid == id).ReserveDetailId ?? Guid.Empty,
                StatusId = _context.BaseInfoGenerals.SingleOrDefault(x => x.Name == "NotVisited").Id
            };
            if (_context.Receptions.Local.Any(x => x.Guid == id))
            {
                //Visit visit = new Visit() { Guid = id };

                _context.Entry(_context.Receptions.Local.SingleOrDefault(x => x.Guid == id)).State = EntityState.Deleted;

            }
            else
            {
                Reception visit = new Reception() { Guid = id };
                _context.Receptions.Attach(visit);
                _context.Entry(visit).State = EntityState.Deleted;

            }
            if (_context.ReserveDetails.Local.Any(x => x.Guid == re.Guid))
            {
                //Visit visit = new Visit() { Guid = id };

                _context.Entry(_context.ReserveDetails.Local.SingleOrDefault(x => x.Guid == re.Guid)).Property(x => x.StatusId).IsModified = true;

            }
            else
            {

                _context.ReserveDetails.Attach(re);
                _context.Entry(re).Property(x => x.StatusId).IsModified = true;

            }
            _context.SaveChanges();

        }

        public Reception GetVisitById(Guid visitId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User).ThenInclude(p => p.Gender)
                .Include(x => x.Patient).ThenInclude(x => x.User).ThenInclude(p => p.Gender)
                .SingleOrDefault(x => x.Guid == visitId);
        }

        public Reception GetVisitForReportById(Guid visitId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User)
                .Include(p => p.ReserveDetail).ThenInclude(p => p.Doctor).ThenInclude(p => p.User)
                .Select(p => new Reception
                {
                    Guid = p.Guid,
                    ReceptionNum = p.ReceptionNum,
                    ReceptionDate = p.ReceptionDate,
                    ReserveDetail = new ReserveDetail
                    {
                        Patient = new Patient
                        {
                            DateOfBirth = p.ReserveDetail.Patient.DateOfBirth,
                            User = new User
                            {
                                Name = p.ReserveDetail.Patient.User.Name
                            }
                        },
                        Doctor = new Doctor
                        {
                            LogoAddress = p.ReserveDetail.Doctor.LogoAddress,
                            Explanation = p.ReserveDetail.Doctor.Explanation,
                            User = new User
                            {
                                Name = p.ReserveDetail.Doctor.User.Name
                            }
                        }
                    }
                })
                .SingleOrDefault(x => x.Guid == visitId);
        }

        public Reception GetVisitWithReserveDetailId(Guid visitId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(p => p.ReserveDetail)
                .SingleOrDefault(p => p.Guid == visitId);
        }

        public Guid? CheckVisitExistByReserveDetailId(Guid reserveDetailId)
        {
            return _context.Receptions.AsNoTracking()
                .Where(p => p.ReserveDetailId == reserveDetailId)
                .FirstOrDefault()?
                .Guid;
        }
    }
}
