using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;
using WPH.Models.SurgeryDoctor;
using WPH.Models.SurgeryService;

namespace WPH.Models.Surgery
{
    public class SurgeryViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? SurgeryRoomId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string SurgeryDetail { get; set; }
        public string Explanation { get; set; }
        public string PostOperativeTreatment { get; set; }
        public Guid? AnesthesiologistId { get; set; }
        public string AnesthesiologistName { get; set; }
        public Guid? PediatricianId { get; set; }
        public string PediatricianName { get; set; }
        public Guid? SurgeryOneId { get; set; }
        public string SurgeryOneName { get; set; }
        public Guid? SurgeryTwoId { get; set; }
        public string SurgeryTwoName { get; set; }
        public int? AnesthesiologistionTypeId { get; set; }
        public string SideEffects { get; set; }
        public string AnesthesiologistionMedicine { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public int? ClassificationId { get; set; }
        public Guid OperationId { get; set; }
        public string OperationName { get; set; }

        public virtual DoctorViewModel Anesthesiologist { get; set; }
        public virtual DoctorViewModel SurgeryOne { get; set; }
        public virtual DoctorViewModel DispatcherDoctor { get; set; }
        public virtual BaseInfoGeneralViewModel AnesthesiologistionType { get; set; }
        public virtual BaseInfoGeneralViewModel Classification { get; set; }
        
        public virtual UserInformationViewModel ModifiedUser { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
        
        public virtual ICollection<SurgeryDoctorViewModel> SurgeryDoctors { get; set; }
        
    }
}
