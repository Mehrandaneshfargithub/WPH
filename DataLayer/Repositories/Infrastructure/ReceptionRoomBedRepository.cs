using DataLayer.EntityModels;
using DataLayer.FunctionModels;
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

    public class ReceptionRoomBedRepository : Repository<ReceptionRoomBed>, IReceptionRoomBedRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionRoomBedRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReceptionRoomBed> GetAllReceptionRoomBed()
        {
            return Context.ReceptionRoomBeds.AsNoTracking();
        }

        public ReceptionRoomBed GetReceptionRMWithReceptionByRoomBedId(Guid roomBedId)
        {
            return Context.ReceptionRoomBeds.AsNoTracking().
                Include(p => p.Reception).
                Where(p => p.RoomBedId.Equals(roomBedId)).
                OrderBy(p => p.Id).
                LastOrDefault(p => p.ExitDate == null);
        }

        public ReceptionRoomBed GetReceptionRMWithReceptionByReceptionId(Guid receptionId)
        {
            return Context.ReceptionRoomBeds.AsNoTracking().
                Include(p => p.Reception).
                Where(p => p.ReceptionId.Equals(receptionId)).
                OrderBy(p => p.Id).
                LastOrDefault(p => p.ExitDate == null);
        }

        public ReceptionRoomBed GetReceptionRMWithReceptionAndBedByReceptionId(Guid receptionId)
        {
            return Context.ReceptionRoomBeds.AsNoTracking().
                Include(p => p.Reception).
                Include(p => p.RoomBed).
                Where(p => p.ReceptionId.Equals(receptionId)).
                OrderBy(p => p.Id).
                LastOrDefault(p => p.ExitDate == null);
        }

        public ReceptionRoomBed GetReceptionRoomBedName(Guid receptionId)
        {
            return Context.ReceptionRoomBeds.AsNoTracking().
                Include(p => p.RoomBed).ThenInclude(a => a.Room).
                Where(p => p.ReceptionId.Equals(receptionId)).
                OrderBy(p => p.Id).
                LastOrDefault(p => p.ExitDate == null);
        }

        public IEnumerable<ReceptionRoomBed> FilterReceptionByRoomAndBedAndPatient(Guid roomId, Guid roomBedId, Guid patientId, Expression<Func<ReceptionRoomBed, bool>> predicate = null)
        {
            IQueryable<ReceptionRoomBed> result = Context.ReceptionRoomBeds.AsNoTracking().
                Include(p => p.RoomBed.Room).
                Include(p => p.Reception.Patient.User);

            if (roomId != Guid.Empty)
                result = result.Where(p => p.RoomBed.Room.Guid.Equals(roomId));

            if (roomBedId != Guid.Empty)
                result = result.Where(p => p.RoomBed.Guid.Equals(roomBedId));

            if (patientId != Guid.Empty)
                result = result.Where(p => p.Reception.Patient.User.Guid.Equals(patientId));

            if (predicate != null)
                result = result.Where(predicate);

            return result.OrderByDescending(p => p.EntranceDate);
        }

        public IEnumerable<ReceptionRoomBed> FilterReceptionByRoomAndBedAndPatientAndUser(Guid userId, Guid roomId, Guid roomBedId, Guid patientId, Expression<Func<ReceptionRoomBed, bool>> predicate = null)
        {
            IQueryable<ReceptionRoomBed> result = Context.ClinicSectionUsers.AsNoTracking()
                .Include(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType)
                .Include(x => x.ClinicSection.Receptions).ThenInclude(p => p.ReceptionRoomBeds).ThenInclude(p => p.RoomBed.Room)
                .Include(x => x.ClinicSection.Receptions).ThenInclude(p => p.ReceptionRoomBeds).ThenInclude(p => p.Reception.Patient.User)
                .Where(x => x.UserId == userId && x.ClinicSection.ClinicSectionType.Name == "NotLab").
                SelectMany(p => p.ClinicSection.Receptions.SelectMany(x => x.ReceptionRoomBeds));

            if (roomId != Guid.Empty)
                result = result.Where(p => p.RoomBed.Room.Guid.Equals(roomId));

            if (roomBedId != Guid.Empty)
                result = result.Where(p => p.RoomBed.Guid.Equals(roomBedId));

            if (patientId != Guid.Empty)
                result = result.Where(p => p.Reception.Patient.User.Guid.Equals(patientId));

            if (predicate != null)
                result = result.Where(predicate);

            return result.OrderByDescending(p => p.EntranceDate);
        }

        public IEnumerable<ReceptionRoomBed> GetReceptionRoomBedForDischarge(Guid receptionId)
        {
            return Context.ReceptionRoomBeds.AsNoTracking()
                .Include(p => p.RoomBed)
                .Where(p => p.ReceptionId == receptionId && p.ExitDate == null);
        }

        public IEnumerable<HospitalDashboardModel> GetAllRoomsWithPatient(IEnumerable<Guid> clinicSectionId)
        {

            string sections = "";

            foreach(var cl in clinicSectionId)
            {
                sections += cl.ToString() + ",";
            }
            string sections2 = sections.Remove(sections.Length-1);

            string qury = "SELECT DISTINCT reception.ClinicSectionId, roomebed.Name BedName,bedstatus.Name BedStatus, patient.DateOfBirth DateOfBirth, patientuser.Name PatientName,reception.GUID ReceptionId," +
                            "room.Name RoomName, reception.ReceptionDate ReceptionDate, sservice.Name Surgery, surgery.SurgeryDate SurgeryDate, doctor.Name Doctor FROM "+
                            "dbo.RoomBed AS roomebed LEFT OUTER JOIN dbo.BaseInfoGeneral AS bedstatus ON bedstatus.Id = roomebed.StatusId "+
                            "LEFT OUTER JOIN dbo.Room AS room ON room.GUID = roomebed.RoomId "+
                            "LEFT OUTER JOIN dbo.ReceptionRoomBed AS receptionroombed ON receptionroombed.RoomBedId = roomebed.GUID "+
                            "LEFT OUTER JOIN dbo.Reception AS reception ON reception.GUID = receptionroombed.ReceptionId "+
                            "LEFT OUTER JOIN dbo.Patient AS patient ON reception.PatientId = patient.GUID "+
                            "LEFT OUTER JOIN dbo.[User] AS patientuser ON patient.GUID = patientuser.GUID "+
                            "LEFT OUTER JOIN dbo.Surgery AS surgery ON surgery.ReceptionId = reception.GUID "+
                            "LEFT OUTER JOIN dbo.SurgeryDoctor AS surgerydoctor ON surgerydoctor.SurgeryId = surgery.GUID "+
                            "LEFT OUTER JOIN dbo.BaseInfoGeneral AS doctorrole ON surgerydoctor.DoctorRoleId = doctorrole.Id "+
                            "LEFT OUTER JOIN dbo.[User] AS doctor ON surgerydoctor.DoctorId = doctor.GUID "+
                            "LEFT OUTER JOIN dbo.ReceptionService AS receptionservice ON receptionservice.ReceptionId = reception.GUID "+
                            "LEFT OUTER JOIN dbo.[Service] AS sservice ON sservice.GUID = receptionservice.ServiceId "+
                            "LEFT OUTER JOIN dbo.BaseInfoGeneral AS[type] ON[type].Id = sservice.TypeId "+
                            $"WHERE(reception.ClinicSectionId IN('{sections2}') AND(reception.Discharge = 0 OR reception.Discharge IS NULL) " +
                            "AND[type].Name = 'Operation' AND doctorrole.Name = 'Surgery1') OR bedstatus.Name <> 'Full'";

            IEnumerable<HospitalDashboardModel> all = Context.Set<HospitalDashboardModel>().FromSqlRaw(qury);

            return all;

            //return Context.ReceptionRoomBeds.AsNoTracking()
            //    .Include(p => p.RoomBed).ThenInclude(a=>a.Status)
            //    .Include(p => p.RoomBed).ThenInclude(a=>a.Room)
            //    .Include(a=>a.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User)
            //    .Include(a=>a.Reception).ThenInclude(a => a.Surgeries).ThenInclude(a => a.SurgeryDoctors).ThenInclude(a => a.Doctor).ThenInclude(a => a.User)
            //    .Include(a=>a.Reception).ThenInclude(a => a.ReceptionServices).ThenInclude(a=>a.Service).ThenInclude(a => a.Type)
            //    .Where(x=>clinicSectionId.Contains(x.Reception.ClinicSectionId.Value) && (x.Reception.Discharge == false || x.Reception.Discharge == null))
            //    .Select(x=> new HospitalDashboardModel 
            //    {
            //        ClinicSectionId = x.Reception.ClinicSectionId,
            //        BedName = x.RoomBed.Name,
            //        BedStatus = x.RoomBed.Status.Name,
            //        DateOfBirth=x.Reception.Patient.DateOfBirth,
            //        PatientName= x.Reception.Patient.User.Name,
            //        ReceptionId = x.ReceptionId,
            //        RoomName = x.RoomBed.Room.Name,
            //        ReceptionDate = x.Reception.ReceptionDate.Value.Day + "/" + x.Reception.ReceptionDate.Value.Month + "/" + x.Reception.ReceptionDate.Value.Year,
            //        Surgery = x.Reception.ReceptionServices.FirstOrDefault(x=>x.Service.Type.Name == "Operation").Service.Name,
            //        SurgeryDate = x.Reception.Surgeries.FirstOrDefault().SurgeryDate.Value.Day + "/" + x.Reception.Surgeries.FirstOrDefault().SurgeryDate.Value.Month + "/" + x.Reception.Surgeries.FirstOrDefault().SurgeryDate.Value.Year,
            //        Doctor = x.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.FirstOrDefault().Doctor.User.Name
            //    });
        }
    }
}
