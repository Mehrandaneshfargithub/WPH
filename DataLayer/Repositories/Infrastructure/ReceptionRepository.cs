using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{

    public class ReceptionRepository : Repository<Reception>, IReceptionRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<Reception> GetAllReception()
        {
            return Context.Receptions.AsNoTracking();
        }

        public IEnumerable<Reception> GetAllReceptionsByClinicSection(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid receptionId, Expression<Func<Reception, bool>> predicate = null)
        {
            IQueryable<Reception> result = Context.Receptions.AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                 .Include(p => p.PaymentStatus)
                 .Where(p => p.ReceptionTypeId == null)
                 .Select(x => new Reception
                 {

                     ClinicSectionId = x.ClinicSectionId,
                     Guid = x.Guid,
                     ReceptionDate = x.ReceptionDate,
                     ReceptionNum = x.ReceptionNum,
                     Discharge = x.Discharge,
                     Patient = new Patient
                     {
                         DateOfBirth = x.Patient.DateOfBirth,
                         User = new User
                         {
                             Name = x.Patient.User.Name,
                             GenderId = x.Patient.User.GenderId,
                             PhoneNumber = x.Patient.User.PhoneNumber,
                             Gender = x.Patient.User.Gender
                         }
                     },
                     ReceptionRoomBeds = x.ReceptionRoomBeds,
                     PaymentStatus = x.PaymentStatus,
                 }).Where(x => x.ReceptionDate <= dateTo && x.ReceptionDate > dateFrom && x.ClinicSectionId == clinicSectionId);

            if (predicate != null)
            {
                result = result.Where(predicate);
            }

            return result;
        }

        public List<Reception> GetAllReceptionsForSelectRoomBed(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid receptionId, Expression<Func<Reception, bool>> predicate = null)
        {
            IQueryable<Reception> result = Context.Receptions.AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                 .Include(p => p.ReceptionRoomBeds).ThenInclude(p => p.RoomBed).ThenInclude(p => p.Room)
                 .Include(p => p.PaymentStatus);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(x => new Reception
            {
                //CreatedDate = x.CreatedDate,
                //Discount = x.Discount,
                ClinicSectionId = x.ClinicSectionId,
                Guid = x.Guid,
                ReceptionDate = x.ReceptionDate,
                ReceptionNum = x.ReceptionNum,
                Patient = new Patient
                {
                    DateOfBirth = x.Patient.DateOfBirth,
                    User = new User
                    {
                        Name = x.Patient.User.Name,
                        GenderId = x.Patient.User.GenderId,
                        PhoneNumber = x.Patient.User.PhoneNumber,
                        Gender = x.Patient.User.Gender
                    }
                },
                ReceptionRoomBeds = x.ReceptionRoomBeds,
                PaymentStatus = x.PaymentStatus,
            }).Where(x => x.ReceptionDate <= dateTo && x.ReceptionDate > dateFrom && x.ClinicSectionId == clinicSectionId && x.Guid != receptionId).ToList();
        }


        public IEnumerable<Reception> GetAllAllHospitalPatientReport(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<Reception, bool>> predicate = null)
        {

            IQueryable<Reception> result = Context.Receptions.AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User)
                 .Include(p => p.ReceptionServices).ThenInclude(p => p.Service).ThenInclude(p => p.Type)
                 .Include(p => p.ReceptionRoomBeds).ThenInclude(p => p.RoomBed).ThenInclude(p => p.Room)
                 .Include(p => p.Surgeries).ThenInclude(p => p.SurgeryDoctors).ThenInclude(p => p.DoctorRole)
                 .Include(p => p.Surgeries).ThenInclude(p => p.SurgeryDoctors).ThenInclude(p => p.Doctor).ThenInclude(p => p.User)
                 .Where(x => x.ReceptionTypeId == null && x.ReceptionDate <= dateTo && x.ReceptionDate >= dateFrom && x.ClinicSectionId == clinicSectionId);


            if (predicate != null)
            {
                result = result.Where(predicate);
            }

            return result.Select(x => new Reception
            {
                ReceptionDate = x.ReceptionDate,
                ExitDate = x.ExitDate,
                Discharge = x.Discharge,
                Guid = x.Guid,
                Patient = new Patient
                {
                    DateOfBirth = x.Patient.DateOfBirth,
                    User = new User
                    {
                        Name = x.Patient.User.Name
                    }
                },
                ReceptionServices = (ICollection<ReceptionService>)x.ReceptionServices.Where(w => w.Service.Type.Name == "Operation").Select(r => new ReceptionService
                {
                    Service = new Service
                    {
                        Name = r.Service.Name,
                        Type = new BaseInfoGeneral
                        {
                            Name = r.Service.Type.Name
                        }
                    }
                }),
                Surgeries = (ICollection<Surgery>)x.Surgeries.Select(s => new Surgery
                {
                    SurgeryDate = s.SurgeryDate,
                    SurgeryDoctors = (ICollection<SurgeryDoctor>)s.SurgeryDoctors.Where(w => w.DoctorRole.Name == "Surgery1").Select(d => new SurgeryDoctor
                    {
                        Doctor = new Doctor
                        {
                            User = new User
                            {
                                Name = d.Doctor.User.Name
                            }
                        },
                        DoctorRole = new BaseInfoGeneral
                        {
                            Name = d.DoctorRole.Name
                        }
                    })
                }),
                ReceptionRoomBeds = (ICollection<ReceptionRoomBed>)x.ReceptionRoomBeds.Where(p => p.ExitDate == null).Select(r => new ReceptionRoomBed
                {
                    RoomBed = new RoomBed
                    {
                        Name = r.RoomBed.Name,
                        Room = new Room
                        {
                            Name = r.RoomBed.Room.Name
                        }
                    },

                })/*.OrderBy(o => o.CreatedDate)*/
            }).OrderByDescending(x => x.ReceptionDate);

        }


        public IEnumerable<Reception> GetAllAllHospitalPatientReportStimul(Guid clinicSectionId, Expression<Func<Reception, bool>> predicate = null)
        {

            IQueryable<Reception> result = Context.Receptions.AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User)
                 .Include(p => p.ReceptionServices).ThenInclude(p => p.Service).ThenInclude(p => p.Type)
                 .Include(p => p.ReceptionRoomBeds).ThenInclude(p => p.RoomBed).ThenInclude(p => p.Room)
                 .Include(p => p.Surgeries).ThenInclude(p => p.SurgeryDoctors).ThenInclude(p => p.DoctorRole)
                 .Include(p => p.Surgeries).ThenInclude(p => p.SurgeryDoctors).ThenInclude(p => p.Doctor).ThenInclude(p => p.User)
                 .Where(x => x.ClinicSectionId == clinicSectionId);


            if (predicate != null)
            {
                result = result.Where(predicate);
            }

            return result.Select(x => new Reception
            {
                ReceptionDate = x.ReceptionDate,
                ExitDate = x.ExitDate,
                Discharge = x.Discharge,
                Guid = x.Guid,
                Patient = new Patient
                {
                    DateOfBirth = x.Patient.DateOfBirth,
                    User = new User
                    {
                        Name = x.Patient.User.Name
                    }
                },
                ReceptionServices = (ICollection<ReceptionService>)x.ReceptionServices.Where(w => w.Service.Type.Name == "Operation").Select(r => new ReceptionService
                {
                    Service = new Service
                    {
                        Name = r.Service.Name,
                        Type = new BaseInfoGeneral
                        {
                            Name = r.Service.Type.Name
                        }
                    }
                }),
                Surgeries = (ICollection<Surgery>)x.Surgeries.Select(s => new Surgery
                {
                    SurgeryDate = s.SurgeryDate,
                    SurgeryDoctors = (ICollection<SurgeryDoctor>)s.SurgeryDoctors.Where(w => w.DoctorRole.Name == "Surgery1").Select(d => new SurgeryDoctor
                    {
                        Doctor = new Doctor
                        {
                            User = new User
                            {
                                Name = d.Doctor.User.Name
                            }
                        },
                        DoctorRole = new BaseInfoGeneral
                        {
                            Name = d.DoctorRole.Name
                        }
                    })
                }),
                ReceptionRoomBeds = (ICollection<ReceptionRoomBed>)x.ReceptionRoomBeds.Select(r => new ReceptionRoomBed
                {
                    RoomBed = new RoomBed
                    {
                        Name = r.RoomBed.Name,
                        Room = new Room
                        {
                            Name = r.RoomBed.Room.Name
                        }
                    },

                })/*.OrderBy(o => o.CreatedDate)*/
            }).OrderByDescending(x => x.ReceptionDate);

        }


        public IEnumerable<ReceptionForCashReport> GetAllReceptionsByClinicSectionForCashReport(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, string status)
        {
            string sqlFormattedDateFrom = dateFrom.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string sqlFormattedDateTo = dateTo.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string status_condition = "";
            if (status != "All")
                status_condition = $" WHERE status.Name = '{status}' ";

            //string qury = "SELECT Reception.GUID, Reception.ReceptionDate, Reception.ReceptionInvoiceNum," +
            //     " DoctorUsers.Name as DoctorName, DoctorUsers.GUID as DoctorId, PatientUsers.Name as PatientName, Service.Name as ServiceName" +
            //     ", Service.Price as ServicePrice, Surgery.Id as SurgeryId," +
            //     "ISNULL(HumanResourceSalary.Salary, 0) as HumanResourceSalary, HumanResourceSalaryT.Name as HumanResourceSalaryType" +
            //     ", ISNULL(HumanResourceSalary.HumanResourceId, NULL) as HumanResourceId, DoctorType.Name as DoctorType" +
            //     ", '' as DoctorSalary, '' as AnethName, '' as AnethSalary FROM Reception " +
            //     "left outer JOIN ReceptionService AS ReceptionService ON Reception.GUID = ReceptionService.ReceptionId left outer JOIN " +
            //     "Service AS Service ON ReceptionService.ServiceId = Service.GUID left outer JOIN " +
            //     "BaseInfoGeneral AS ReceptionServiceStatus ON ReceptionService.StatusId = ReceptionServiceStatus.Id left outer JOIN " +
            //     "BaseInfoGeneral AS BaseInfoGeneral ON Service.TypeId = BaseInfoGeneral.Id left outer JOIN " +
            //     "Surgery AS Surgery ON Reception.GUID = Surgery.ReceptionId left outer JOIN " +
            //     "SurgeryDoctor AS SurgeryDoctor ON Surgery.GUID = SurgeryDoctor.SurgeryId left outer JOIN " +
            //     "HumanResourceSalary AS HumanResourceSalary ON Surgery.GUID = HumanResourceSalary.SurgeryId left outer JOIN " +
            //     "BaseInfoGeneral AS HumanResourceSalaryT ON HumanResourceSalary.CadreTypeId = HumanResourceSalaryT.Id left outer JOIN " +
            //     "[dbo].[User] AS DoctorUsers ON SurgeryDoctor.DoctorId = DoctorUsers.GUID left outer JOIN " +
            //     "BaseInfoGeneral AS DoctorType ON SurgeryDoctor.DoctorRoleId = DoctorType.Id left outer JOIN " +
            //     "[dbo].[User] AS PatientUsers ON PatientUsers.GUID = Reception.PatientId" +
            //     $" WHERE Reception.ReceptionDate <= '{sqlFormattedDateTo}' AND Reception.ReceptionDate > '{sqlFormattedDateFrom}' AND Reception.ClinicSectionId = '{clinicSectionId}' AND Surgery.Id IS NOT NULL AND ReceptionServiceStatus.Name = '{status}'";

            string qury =
                        " SELECT * FROM( " +
                        " SELECT sg.CreatedDate, sg.ClinicSectionId, re.GUID ReceptionId,sg.Explanation Description , u.Name VName,'Patient' VType,0 price,NULL StatusName, re.ReceptionDate,re.ReceptionInvoiceNum,sg.Id SurgeryId FROM dbo.Surgery sg " +
                        " LEFT JOIN dbo.Reception re ON re.GUID = sg.ReceptionId " +
                        " LEFT JOIN dbo.[User] u ON u.GUID = re.PatientId " +
                        " UNION " +
                        " SELECT sg.CreatedDate, sg.ClinicSectionId, rs.ReceptionId,sg.Explanation Description,s.Name VName, CASE WHEN type.Name='Operation' THEN 'Operation' ELSE 'Service' END VType, ISNULL(rs.Number, 1)*rs.Price - ISNULL(rs.Discount, 0) price,status.Name StatusName, NULL ReceptionDate,NULL ReceptionInvoiceNum, NULL SurgeryId FROM dbo.ReceptionService rs " +
                        " LEFT JOIN dbo.Service s ON s.GUID = rs.ServiceId " +
                        " LEFT JOIN dbo.Surgery sg ON sg.ReceptionId = rs.ReceptionId " +
                        " LEFT JOIN dbo.BaseInfoGeneral status ON status.Id = rs.StatusId " +
                        " LEFT JOIN dbo.BaseInfoGeneral type ON type.Id = s.TypeId " +
                        $" {status_condition} " +
                        " UNION " +
                        " SELECT sg.CreatedDate, sg.ClinicSectionId, sg.ReceptionId,'' Description,u.Name VName, ISNULL(DoctorRole.Name, cadre.Name) VType,hm.Salary price, NULL StatusName ,NULL ReceptionDate, NULL ReceptionInvoiceNum,NULL SurgeryId  FROM dbo.Surgery sg " +
                        " LEFT JOIN dbo.HumanResourceSalary hm ON hm.SurgeryId = sg.GUID " +
                        " LEFT JOIN dbo.BaseInfoGeneral cadre ON cadre.Id = hm.CadreTypeId " +
                        " LEFT JOIN dbo.SurgeryDoctor sd ON sd.SurgeryId = sg.GUID AND sd.DoctorId = hm.HumanResourceId " +
                        " LEFT JOIN dbo.[User] u ON u.GUID = sd.DoctorId " +
                        " LEFT JOIN dbo.BaseInfoGeneral DoctorRole ON DoctorRole.Id = sd.DoctorRoleId ) res " +
                        $" WHERE res.CreatedDate <= '{sqlFormattedDateTo}' AND res.CreatedDate >= '{sqlFormattedDateFrom}' AND ClinicSectionId = '{clinicSectionId}' ";

            IEnumerable<ReceptionForCashReport> we;
            try
            {
                we = Context.Set<ReceptionForCashReport>().FromSqlRaw(qury);
            }
            catch (Exception) { return null; }


            return we;


        }

        public List<Reception> GetReceptionsByClinicSectionForCash(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid receptionId, Expression<Func<Reception, bool>> predicate = null)
        {
            IQueryable<Reception> result = Context.Receptions.AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                 .Include(p => p.ReceptionRoomBeds).ThenInclude(p => p.RoomBed.Room)
                 .Include(p => p.ReceptionServices).ThenInclude(p => p.Service)
                 .Include(p => p.ReceptionServices).ThenInclude(p => p.ReceptionServiceReceiveds)
                 .Include(p => p.Purpose)
                 .Include(p => p.PaymentStatus)
                 .Include(p => p.ReceptionInsurances).ThenInclude(x => x.ReceptionInsuranceReceiveds);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(x => new Reception
            {
                //CreatedDate = x.CreatedDate,
                //Discount = x.Discount, 
                ClinicSectionId = x.ClinicSectionId,
                Guid = x.Guid,
                ReceptionDate = x.ReceptionDate,
                ReceptionNum = x.ReceptionNum,
                Patient = new Patient
                {
                    DateOfBirth = x.Patient.DateOfBirth,
                    User = new User
                    {
                        Name = x.Patient.User.Name,
                        GenderId = x.Patient.User.GenderId,
                        PhoneNumber = x.Patient.User.PhoneNumber,
                        Gender = x.Patient.User.Gender
                    }
                },
                ReceptionRoomBeds = x.ReceptionRoomBeds,
                ReceptionServices = x.ReceptionServices,
                PaymentStatus = x.PaymentStatus,
                Purpose = new BaseInfoGeneral
                {
                    Name = x.Purpose.Name
                },
                ReceptionInsurances = x.ReceptionInsurances.Select(a => new ReceptionInsurance
                {
                    ReceptionInsuranceReceiveds = a.ReceptionInsuranceReceiveds.Select(b => new ReceptionInsuranceReceived
                    {
                        Amount = b.Amount,
                        AmountStatus = b.AmountStatus
                    }).ToList()
                }).ToList()

            }).Where(x => x.ReceptionDate <= dateTo && x.ReceptionDate > dateFrom && x.ClinicSectionId == clinicSectionId && x.ReceptionServices.Count > 0).ToList();
        }

        public Reception GetReception(Guid receptionId)
        {
            return Context.Receptions
                .AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                 .Include(x => x.Patient).ThenInclude(x => x.Address)
                 .Include(x => x.Purpose)
                 .Include(x => x.ClearanceType)
                 .Select(x => new Reception
                 {

                     Guid = x.Guid,
                     ReceptionDate = x.ReceptionDate,
                     ReceptionNum = x.ReceptionNum,
                     ClearanceType = x.ClearanceType,
                     Patient = new Patient
                     {
                         DateOfBirth = x.Patient.DateOfBirth,
                         AddressId = x.Patient.AddressId,
                         Address = x.Patient.Address,
                         MotherName = x.Patient.MotherName,
                         IdentityNumber = x.Patient.IdentityNumber,
                         User = new User
                         {
                             Name = x.Patient.User.Name,
                             GenderId = x.Patient.User.GenderId,
                             PhoneNumber = x.Patient.User.PhoneNumber,
                             Gender = x.Patient.User.Gender
                         }
                     },
                     PurposeId = x.PurposeId,
                     Purpose = x.Purpose,
                     ClinicSectionId = x.ClinicSectionId,
                     Description = x.Description,
                     DiscountCurrencyId = x.DiscountCurrencyId,
                     EntranceDate = x.EntranceDate,
                     ExitDate = x.ExitDate,
                     PatientAttendanceName = x.PatientAttendanceName,
                     PatientId = x.PatientId,
                     ChiefComplaint = x.ChiefComplaint,
                     Examination = x.Examination,
                     ClearanceTypeId = x.ClearanceTypeId

                 }).SingleOrDefault(x => x.Guid == receptionId);
        }

        public Reception GetReceptionWithServices(Guid receptionId)
        {
            return Context.Receptions.AsNoTracking()
                .Include(p => p.ReceptionServices)
                .SingleOrDefault(x => x.Guid == receptionId);
        }

        public IEnumerable<User> GetReceptionPatient()
        {
            return Context.Receptions.AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User)
                 .Select(p => p.Patient.User).Distinct();
        }

        public IEnumerable<PatientImage> RemoveReception(Guid receptionid)
        {
            _context.Emergencies.RemoveRange(_context.Emergencies.Where(x => x.Guid == receptionid));
            var su = _context.Surgeries.SingleOrDefault(x => x.ReceptionId == receptionid);
            if (su != null)
            {
                _context.SurgeryDoctors.RemoveRange(_context.SurgeryDoctors.Where(x => x.SurgeryId == su.Guid));
                //_context..RemoveRange(_context.SurgeryDoctors.Where(x => x.SurgeryId == su.Guid));
            }

            _context.Emergencies.RemoveRange(_context.Emergencies.Where(x => x.Guid == receptionid));
            _context.Surgeries.RemoveRange(_context.Surgeries.Where(x => x.ReceptionId == receptionid));
            _context.ReceptionAmbulances.RemoveRange(_context.ReceptionAmbulances.Where(x => x.ReceptionId == receptionid));
            _context.ReceptionClinicSections.RemoveRange(_context.ReceptionClinicSections.Where(x => x.ReceptionId == receptionid));
            _context.ReceptionRoomBeds.RemoveRange(_context.ReceptionRoomBeds.Where(x => x.ReceptionId == receptionid));
            var rs = _context.ReceptionServices.AsNoTracking().Include(p => p.ReceptionServiceReceiveds).Where(x => x.ReceptionId == receptionid);

            _context.ReceptionInsurances.RemoveRange(_context.ReceptionInsurances.Where(a => a.ReceptionId == receptionid).Include(b => b.ReceptionInsuranceReceiveds));
            _context.ReceptionServiceReceiveds.RemoveRange(rs.SelectMany(p => p.ReceptionServiceReceiveds));

            _context.ReceptionServices.RemoveRange(_context.ReceptionServices.Where(x => x.ReceptionId == receptionid));
            _context.Receptions.Remove(_context.Receptions.SingleOrDefault(x => x.Guid == receptionid));

            var images = _context.PatientImages.Where(p => p.ReceptionId == receptionid);
            _context.RemoveRange(images);
            return images;
        }

        public Guid UpdateReception(Reception reception)
        {
            //Context.Entry(reception).State = EntityState.Modified;
            //Context.Attach<Reception>(reception);
            //Context.Attach(reception);
            Context.Entry(reception).Property(x => x.PatientAttendanceName).IsModified = true;
            Context.Entry(reception.Patient).Property(x => x.AddressId).IsModified = true;
            Context.Entry(reception.Patient).Property(x => x.DateOfBirth).IsModified = true;
            Context.Entry(reception.Patient).Property(x => x.MotherName).IsModified = true;
            Context.Entry(reception.Patient).Property(x => x.IdentityNumber).IsModified = true;
            Context.Entry(reception.Patient.User).Property(x => x.PhoneNumber).IsModified = true;
            Context.Entry(reception.Patient.User).Property(x => x.GenderId).IsModified = true;
            if (reception.Surgeries.Count != 0)
            {
                Surgery sur = Context.Surgeries.FirstOrDefault(x => x.ReceptionId == reception.Guid);
                sur.SurgeryDate = reception.Surgeries.FirstOrDefault().SurgeryDate;
                reception.Surgeries.FirstOrDefault().Guid = sur.Guid;
            }

            //Context.Entry(sur).Property(x => x.SurgeryDate).IsModified = true;
            //reception.Surgeries = null;
            _context.SaveChanges();
            return reception.Guid;
        }

        public void UpdateReceptionCleareance(Reception reception)
        {
            try
            {
                Context.Attach(reception);
                Context.Entry(reception).Property(x => x.ClearanceTypeId).IsModified = true;
                Context.Entry(reception).Property(x => x.ExitDate).IsModified = true;
                //Context.Entry(reception).Property(x => x.ChiefComplaint).IsModified = true;
                //Context.Entry(reception).Property(x => x.Examination).IsModified = true;

                _context.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        public void UpdateReceptionChiefComplaint(Reception reception)
        {
            try
            {
                Context.Attach(reception);
                //Context.Entry(reception).Property(x => x.ClearanceTypeId).IsModified = true;
                //Context.Entry(reception).Property(x => x.ExitDate).IsModified = true;
                Context.Entry(reception).Property(x => x.ChiefComplaint).IsModified = true;
                Context.Entry(reception).Property(x => x.Examination).IsModified = true;

                _context.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        public bool GetReceptionDischargeStatus(Guid receptionid)
        {
            return Context.Receptions.AsNoTracking().SingleOrDefault(p => p.Guid == receptionid).Discharge ?? false;
        }

        public Reception GetReceptionWithServiceByReserveDetailId(Guid reserveDetailId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(p => p.ReceptionServices).ThenInclude(p => p.ReceptionServiceReceiveds)
                .Where(x => x.ReserveDetailId == reserveDetailId)
                .Select(p => new Reception
                {
                    Guid = p.Guid,
                    ReceptionServices = (ICollection<ReceptionService>)p.ReceptionServices.Select(x => new ReceptionService
                    {
                        Number = x.Number,
                        Price = x.Price,
                        Discount = x.Discount,
                        ReceptionServiceReceiveds = (ICollection<ReceptionServiceReceived>)x.ReceptionServiceReceiveds.Select(s => new ReceptionServiceReceived
                        {
                            Amount = s.Amount,
                            AmountStatus = s.AmountStatus
                        })
                    })
                })
                .FirstOrDefault();
        }

        public IEnumerable<Reception> GetAllReceptionForOneDayBasedOnDoctorIdJustStatusAndVisitNum(Guid doctorId, DateTime date)
        {
            var aa =  _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail)
                .Include(x => x.Status)
                .Where(x => x.ReceptionDate != null && x.ReceptionDate.Value.Date == date && x.ReserveDetail.DoctorId == doctorId)
                .Select(x => new Reception
                {
                    VisitNum = x.VisitNum,
                    Status = new BaseInfoGeneral
                    {
                        Name = x.Status.Name
                    }
                });

            return aa;
        }


        public Reception GetTodayReceptionThatMustVisitingByDoctorId(Guid doctorId, DateTime today, bool lastVisit, int visitNum = 0)
        {
            IQueryable<Reception> result = _context.Receptions.AsNoTracking()
                .Include(x => x.ReserveDetail).ThenInclude(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.Status)
                .Where(x => x.ReserveDetail.DoctorId == doctorId && x.ReceptionDate != null && x.ReceptionDate.Value.Date == today)
                .Select(x => new Reception
                {
                    Guid = x.Guid,
                    VisitNum = x.VisitNum,
                    ReceptionDate = x.ReceptionDate,
                    ReceptionNum = x.ReceptionNum,
                    ReserveDetailId = x.ReserveDetailId,
                    ServerVisitNum = x.ServerVisitNum,
                    AnalysisServerVisitNum = x.AnalysisServerVisitNum,
                    Description = x.Description,
                    ReserveDetail = (x.ReserveDetail == null) ? null : new ReserveDetail
                    {
                        PatientId = x.ReserveDetail.PatientId,
                        ReserveStartTime = x.ReserveDetail.ReserveStartTime,
                        Patient = new Patient
                        {
                            Guid = x.ReserveDetail.Patient.Guid,
                            DateOfBirth = x.ReserveDetail.Patient.DateOfBirth,
                            FileNum = x.ReserveDetail.Patient.FileNum,
                            FormNumber = x.ReserveDetail.Patient.FormNumber,
                            User = new User
                            {
                                Name = x.ReserveDetail.Patient.User.Name,
                                GenderId = x.ReserveDetail.Patient.User.GenderId
                            }
                        }
                    },
                    Status = new BaseInfoGeneral { Name = x.Status.Name }
                });

            if (lastVisit)
            {
                return result.OrderByDescending(x => x.VisitNum).FirstOrDefault();
            }

            if (visitNum == 0)
            {
                return result.OrderBy(x => x.VisitNum).FirstOrDefault(x => x.Status.Name == "Visiting");
            }

            return result.FirstOrDefault(x => x.VisitNum == visitNum);

        }

        public Reception GetMedicineReceptionForServer(Guid receptionId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(p => p.PrescriptionDetails).ThenInclude(p => p.Medicine).ThenInclude(p => p.MedicineForm)
                .Include(p => p.ReserveDetail).ThenInclude(p => p.Patient).ThenInclude(p => p.User)
                .Include(p => p.ReserveDetail).ThenInclude(p => p.Doctor).ThenInclude(p => p.User)
                .Include(p => p.ClinicSection)
                .Where(p => p.Guid == receptionId)
                .Select(s => new Reception
                {
                    ServerVisitNum = s.ServerVisitNum,
                    PrescriptionDetails = (ICollection<PrescriptionDetail>)s.PrescriptionDetails.Select(p => new PrescriptionDetail
                    {
                        Num = p.Num,
                        ConsumptionInstruction = p.ConsumptionInstruction,
                        Explanation = p.Explanation,
                        Medicine = new Medicine
                        {
                            ScientificName = p.Medicine.ScientificName,
                            JoineryName = p.Medicine.JoineryName,
                            MedicineForm = new BaseInfo
                            {
                                Name = p.Medicine.MedicineForm.Name
                            }
                        }
                    }),
                    ClinicSection = new ClinicSection
                    {
                        Name = s.ClinicSection.Name
                    },
                    ReserveDetail = (s.ReserveDetail == null)? null : new ReserveDetail
                    {
                        Doctor = new Doctor
                        {
                            User = new User
                            {
                                Name = s.ReserveDetail.Doctor.User.Name
                            }
                        },
                        Patient = new Patient
                        {
                            User = new User
                            {
                                Name = s.ReserveDetail.Patient.User.Name,
                                PhoneNumber = s.ReserveDetail.Patient.User.PhoneNumber,
                            }
                        }
                    },
                    Patient = (s.Patient == null) ? null : new Patient
                    {
                        User = new User
                        {
                            Name = s.Patient.User.Name,
                            PhoneNumber = s.Patient.User.PhoneNumber,
                        }
                    }
                }).SingleOrDefault();
        }

        public Reception GetAnalysisReceptionForServer(Guid receptionId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(p => p.PrescriptionTestDetails).ThenInclude(p => p.Test)
                .Include(p => p.ReserveDetail).ThenInclude(p => p.Patient).ThenInclude(p => p.User)
                .Include(p => p.ReserveDetail).ThenInclude(p => p.Doctor).ThenInclude(p => p.User)
                .Include(p => p.ClinicSection)
                .Where(p => p.Guid == receptionId)
                .Select(s => new Reception
                {
                    AnalysisServerVisitNum = s.AnalysisServerVisitNum,
                    PrescriptionTestDetails = (ICollection<PrescriptionTestDetail>)s.PrescriptionTestDetails.Select(p => new PrescriptionTestDetail
                    {
                        AnalysisName = p.AnalysisName,
                        Explanation = p.Explanation,
                        Test = new BaseInfo
                        {
                            Name = p.Test.Name
                        }
                    }),
                    ClinicSection = new ClinicSection
                    {
                        Name = s.ClinicSection.Name
                    },
                    ReserveDetail = new ReserveDetail
                    {
                        Doctor = new Doctor
                        {
                            User = new User
                            {
                                Name = s.ReserveDetail.Doctor.User.Name
                            }
                        },
                        Patient = new Patient
                        {
                            User = new User
                            {
                                Name = s.ReserveDetail.Patient.User.Name,
                                PhoneNumber = s.ReserveDetail.Patient.User.PhoneNumber,
                            }
                        }
                    }
                }).SingleOrDefault();
        }

        public Reception GetReceptionWithPatient(Guid receptionId)
        {
            return _context.Receptions.AsNoTracking()
                .Include(p => p.Patient).ThenInclude(p => p.User)
                .Include(p => p.Status)
                .Where(p => p.Guid == receptionId)
                .Select(p => new Reception
                {
                    ReceptionNum = p.ReceptionNum,
                    Status = new BaseInfoGeneral
                    {
                        Name = p.Status.Name
                    },
                    Patient = new Patient
                    {
                        User = new User
                        {
                            Name = p.Patient.User.Name
                        }
                    }

                }).SingleOrDefault();
        }

        public string GetServerVisitNum(Guid receptionId)
        {
            return _context.Receptions.FirstOrDefault(a => a.Guid == receptionId).ServerVisitNum.GetValueOrDefault().ToString();
        }

        public IEnumerable<DateTime> GetReceptionCount(Guid userId)
        {

            string re = @$"SELECT ReceptionDate ExpireDate,'' ProductId FROM dbo.Reception WHERE ClinicSectionId IN (SELECT ClinicSectionId FROM dbo.ClinicSection_User WHERE UserId = '{userId}')";

            var result= _context.Set<ExpiredProductModel>().FromSqlRaw(re);

            return result.Select(a => a.ExpireDate??DateTime.Now);

        }
    }
}
