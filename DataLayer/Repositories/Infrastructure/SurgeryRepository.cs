using DataLayer.EntityModels;
using DataLayer.FunctionModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    class SurgeryRepository : Repository<Surgery>, ISurgeryRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SurgeryRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<Surgery> GetAllSurgery()
        {
            return Context.Surgeries.AsNoTracking();
        }


        public Surgery GetSurgery(Guid SurgeryId)
        {
            return Context.Surgeries
                .AsNoTracking()
                 .Include(x => x.Reception).ThenInclude(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                 .Include(x => x.SurgeryDoctors).ThenInclude(x => x.DoctorRole)
                 .Include(x => x.Reception).ThenInclude(x => x.ReceptionServices).ThenInclude(x => x.Service).ThenInclude(x => x.Type)
                 .Select(x => new Surgery
                 {
                     CreatedDate = x.CreatedDate,
                     Guid = x.Guid,
                     ClinicSectionId = x.ClinicSectionId,
                     AnesthesiologistionMedicine = x.AnesthesiologistionMedicine,
                     AnesthesiologistionTypeId = x.AnesthesiologistionTypeId,
                     ClassificationId = x.ClassificationId,
                     CreatedUserId = x.CreatedUserId,
                     ExitDate = x.ExitDate,
                     Explanation = x.Explanation,
                     PostOperativeTreatment = x.PostOperativeTreatment,
                     ReceptionId = x.ReceptionId,
                     SideEffects = x.SideEffects,
                     StartDate = x.StartDate,
                     SurgeryDetail = x.SurgeryDetail,
                     SurgeryRoomId = x.SurgeryRoomId,
                     Reception = new Reception
                     {
                         ReceptionDate = x.Reception.ReceptionDate,
                         ReceptionNum = x.Reception.ReceptionNum,
                         Patient = new Patient
                         {
                             DateOfBirth = x.Reception.Patient.DateOfBirth,
                             MotherName = x.Reception.Patient.MotherName,
                             Address = x.Reception.Patient.Address,
                             User = new User
                             {
                                 Name = x.Reception.Patient.User.Name,
                                 GenderId = x.Reception.Patient.User.GenderId,
                                 PhoneNumber = x.Reception.Patient.User.PhoneNumber,
                                 Gender = x.Reception.Patient.User.Gender
                             }
                         },
                         ReceptionServices = (ICollection<ReceptionService>)x.Reception.ReceptionServices.Where(p => p.Service.Type.Name == "Operation").Select(s => new ReceptionService
                         {
                             Service = new Service
                             {
                                 Guid = s.Service.Guid,
                                 //Name = s.Service.Name
                             }
                         })
                     },
                     SurgeryDoctors = x.SurgeryDoctors.Select(a => new SurgeryDoctor
                     {
                         DoctorId = a.DoctorId,
                         Doctor = new Doctor
                         {
                             User = new User
                             {
                                 Name = a.Doctor.User.Name
                             }
                         },
                         DoctorRole = new BaseInfoGeneral
                         {
                             Name = a.DoctorRole.Name
                         }
                     }).ToList()

                 }).SingleOrDefault(x => x.Guid == SurgeryId);
        }

        public void RemoveSurgery(Guid Surgeryid)
        {

        }

        public IEnumerable<SurgeryGrid> GetAllSurgeryByClinicSectionId(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid? doctorId, Guid? operationId)
        {

            string sqlFormattedDateFrom = dateFrom.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string sqlFormattedDateTo = dateTo.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var DoctorId = "NULL";
            var OperationId = "NULL";

            if (doctorId != null)
                DoctorId = $"'{doctorId}'";

            if (operationId != null)
                OperationId = $"'{operationId}'";

            string qury = "SELECT surery.GUID Guid,surery.SurgeryDate SurgeryDate, reception.ReceptionDate ReceptionDate,reception.ReceptionNum ReceptionNum" +
                ",patient.DateOfBirth DateOfBirth, patientusers.Name PatientName,patientusers.PhoneNumber ,gender.Name Gender,servic.Name OperationName, doctors.Name SurgeryOneName FROM " +
                "dbo.Surgery AS surery LEFT OUTER JOIN " +
                "dbo.Reception reception ON reception.GUID = surery.ReceptionId LEFT OUTER JOIN " +
                "dbo.Patient patient ON patient.GUID = reception.PatientId LEFT OUTER JOIN " +
                "dbo.[User] patientusers ON patientusers.GUID = patient.GUID LEFT OUTER JOIN " +
                "dbo.ReceptionService receptionService ON receptionService.ReceptionId = reception.GUID LEFT OUTER JOIN " +
                "dbo.Service servic ON servic.GUID = receptionService.ServiceId LEFT OUTER JOIN " +
                "dbo.SurgeryDoctor surgeryDoctor ON surgeryDoctor.SurgeryId = surery.GUID LEFT OUTER JOIN " +
                "dbo.BaseInfoGeneral operation ON operation.Id = servic.TypeId LEFT OUTER JOIN " +
                "dbo.BaseInfoGeneral gender ON gender.Id = patientusers.GenderId LEFT OUTER JOIN " +
                "dbo.[User] doctors ON doctors.GUID = surgeryDoctor.DoctorId LEFT OUTER JOIN " +
                "dbo.BaseInfoGeneral doctorRole ON doctorRole.Id = surgeryDoctor.DoctorRoleId " +
                $"WHERE surery.ClinicSectionId = '{clinicSectionId}' AND surery.SurgeryDate <= '{sqlFormattedDateTo}' AND surery.SurgeryDate >= '{sqlFormattedDateFrom}' AND operation.Name = 'Operation' AND doctorRole.Name = 'Surgery1' " +
                "AND ISNULL(SurgeryDoctor.DoctorId, '00000000-0000-0000-0000-000000000000') = " +
                $"CASE ISNULL({DoctorId}, '00000000-0000-0000-0000-000000000000') WHEN '00000000-0000-0000-0000-000000000000'" +
                $" THEN ISNULL(SurgeryDoctor.DoctorId, '00000000-0000-0000-0000-000000000000') ELSE {DoctorId} END " +
                "AND ISNULL(ReceptionService.ServiceId, '00000000-0000-0000-0000-000000000000') = " +
                $"CASE ISNULL({OperationId}, '00000000-0000-0000-0000-000000000000') WHEN '00000000-0000-0000-0000-000000000000' " +
                $"THEN ISNULL(ReceptionService.ServiceId, '00000000-0000-0000-0000-000000000000') ELSE {OperationId} END ORDER BY surery.SurgeryDate DESC";


            IEnumerable<SurgeryGrid> we;
            try
            {
                we = Context.Set<SurgeryGrid>().FromSqlRaw(qury);
            }
            catch (Exception e) { return null; }


            //IQueryable<Surgery> surgeries = Context.Surgeries
            //    .AsNoTracking()
            //     .Include(x => x.Reception).ThenInclude(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
            //     .Include(x => x.Reception).ThenInclude(x => x.ReceptionServices).ThenInclude(x => x.Service).ThenInclude(x => x.Type)
            //     .Include(x => x.SurgeryDoctors).ThenInclude(x => x.DoctorRole)
            //     .Include(x => x.SurgeryDoctors).ThenInclude(x => x.Doctor).ThenInclude(x => x.User)
            //     .Select(x => new Surgery
            //     {
            //         CreatedDate = x.CreatedDate,
            //         Guid = x.Guid,
            //         ClinicSectionId = x.ClinicSectionId,
            //         SurgeryDate = x.SurgeryDate,
            //         Reception = (x.Reception == null) ? null : new Reception
            //         {
            //             ReceptionDate = x.Reception.ReceptionDate,
            //             ReceptionNum = x.Reception.ReceptionNum,
            //             Patient = new Patient
            //             {
            //                 DateOfBirth = x.Reception.Patient.DateOfBirth,
            //                 User = new User
            //                 {
            //                     Name = x.Reception.Patient.User.Name,
            //                     GenderId = x.Reception.Patient.User.GenderId,
            //                     PhoneNumber = x.Reception.Patient.User.PhoneNumber,
            //                     Gender = x.Reception.Patient.User.Gender
            //                 }
            //             },
            //             ReceptionServices = x.Reception.ReceptionServices.Where(p => p.Service.Type.Name == "Operation").Select(s => new ReceptionService
            //             {
            //                 ServiceId = s.ServiceId,
            //                 Service = new Service
            //                 {
            //                     Name = s.Service.Name
            //                 }
            //             }).Where(p => (operationId == null ? true : p.ServiceId == operationId)).ToList()
            //         },
            //         SurgeryDoctors = (ICollection<SurgeryDoctor>)x.SurgeryDoctors.Where(a => (doctorId == null)?true:a.DoctorId == doctorId)

            //     }).Where(x => x.ClinicSectionId == clinicSectionId && x.SurgeryDate <= dateTo && x.SurgeryDate >= dateFrom).OrderByDescending(x => x.SurgeryDate);

            //if (doctorId != null)
            //{
            //    surgeries = surgeries.Where(a => a.SurgeryDoctors.Any(b => b.DoctorId == doctorId));
            //    //surgeries = surgeries.Where(a => a.SurgeryDoctors.FirstOrDefault().DoctorId == doctorId);
            //}

            //if (operationId != null)
            //{
            //    surgeries = surgeries.Where(a => a.Reception.ReceptionServices.Any(x => x.Service.Guid == operationId));
            //}

            return we;

        }

        public void UpdateSurgery(Surgery surgery2)
        {
            try
            {
                _context.SurgeryDoctors.RemoveRange(_context.SurgeryDoctors.Where(x => x.SurgeryId == surgery2.Guid));
                _context.SurgeryDoctors.AddRange(surgery2.SurgeryDoctors);
                _context.Entry(surgery2).State = EntityState.Modified;
                _context.Entry(surgery2).Property(a => a.CreatedDate).IsModified = false;
                _context.Entry(surgery2).Property(a => a.SurgeryDate).IsModified = false;
                //_context.Update(surgery2);
            }
            catch (Exception w) { throw w; }
        }



        public Surgery GetSurgeryReportForPrint(Guid surgeryId)
        {
            return Context.Surgeries
                .AsNoTracking()
                 .Include(x => x.Reception).ThenInclude(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
                 .Include(x => x.SurgeryDoctors).ThenInclude(x => x.DoctorRole)
                 .Include(x => x.SurgeryDoctors).ThenInclude(x => x.Doctor)
                 .Select(x => new Surgery
                 {

                     CreatedDate = x.CreatedDate,
                     Guid = x.Guid,
                     CreatedUserId = x.CreatedUserId,
                     ModifiedUser = (x.ModifiedUser == null) ? null : new User
                     {
                         Name = x.ModifiedUser.Name
                     },
                     Reception = new Reception
                     {
                         ReceptionDate = x.Reception.ReceptionDate,
                         ReceptionNum = x.Reception.ReceptionNum,
                         Patient = new Patient
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
                     },
                     SurgeryDoctors = x.SurgeryDoctors.Select(a => new SurgeryDoctor
                     {
                         DoctorId = a.DoctorId,
                         DoctorRole = new BaseInfoGeneral
                         {
                             Name = a.DoctorRole.Name
                         },
                         Doctor = new Doctor
                         {
                             User = new User
                             {
                                 Name = a.Doctor.User.Name,
                             }
                         }
                     }).ToList()

                 }).SingleOrDefault(x => x.Guid == surgeryId);
        }

        public Surgery GetSurgeryByReceptionId(Guid receptionId)
        {
            return Context.Surgeries
                .AsNoTracking()
                 .Include(x => x.SurgeryDoctors).ThenInclude(x => x.DoctorRole)
                 .Include(x => x.SurgeryDoctors).ThenInclude(x => x.Doctor).ThenInclude(x => x.User)
                 .Include(x => x.AnesthesiologistionType)
                 .Select(x => new Surgery
                 {
                     SurgeryDate = x.SurgeryDate,
                     Guid = x.Guid,
                     ReceptionId = x.ReceptionId,
                     AnesthesiologistionTypeId = x.AnesthesiologistionTypeId,
                     AnesthesiologistionMedicine = x.AnesthesiologistionMedicine,
                     ClassificationId = x.ClassificationId,
                     Classification = x.Classification,
                     PostOperativeTreatment = x.PostOperativeTreatment,
                     SurgeryDetail = x.SurgeryDetail,
                     AnesthesiologistionType = x.AnesthesiologistionType,
                     SurgeryDoctors = x.SurgeryDoctors.Select(a => new SurgeryDoctor
                     {
                         SurgeryId = a.SurgeryId,
                         DoctorId = a.DoctorId,
                         DoctorRole = a.DoctorRole,
                         Doctor = a.Doctor

                     }).ToList()

                 }).FirstOrDefault(x => x.ReceptionId == receptionId);
        }

        public Surgery GetSimpleSurgeryByReceptionId(Guid receptionId)
        {
            return Context.Surgeries.AsNoTracking()
                .FirstOrDefault(x => x.ReceptionId == receptionId);
        }

        public object GetNearestOperations(Guid userId)
        {

            int operation = Context.BaseInfoGenerals.FirstOrDefault(a => a.Name == "Operation").Id;

            return Context.Surgeries.AsNoTracking()
                .Include(a=>a.Reception).ThenInclude(a=>a.Patient).ThenInclude(a => a.User)
                .Include(a=>a.Reception).ThenInclude(a=>a.ReceptionDoctors).ThenInclude(a => a.Doctor).ThenInclude(a => a.User)
                .Include(a=>a.Reception).ThenInclude(a=>a.ReceptionServices).ThenInclude(a => a.Service)
                .Where(x => x.SurgeryDate >= DateTime.Now 
                && Context.ClinicSectionUsers.Where(a => a.UserId == userId).Select(a => a.ClinicSectionId).Contains(x.Reception.ClinicSectionId.Value)
                && x.Reception.ReceptionServices.Where(a=>a.Service.TypeId == operation).Any())
                .OrderByDescending(a=>a.SurgeryDate).Take(5).Select(a=>new 
                {
                    PatientName = a.Reception.Patient.User.Name + " / " + a.Reception.ReceptionServices.FirstOrDefault(x=>x.Service.TypeId == operation).Service.Name
                });
        }
    }
}
