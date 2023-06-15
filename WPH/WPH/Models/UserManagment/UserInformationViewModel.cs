using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;

namespace WPH.Models.CustomDataModels.UserManagment
{
    public class UserInformationViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public string UserName { get; set; }
        public string UserNameHolder { get; set; }
        public int Index { get; set; }



        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Pass3 { get; set; }
        public string Pass4 { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberHolder { get; set; }

        public int? UserTypeId { get; set; }
        public int? GenderId { get; set; }

        public string UserTypeName { get; set; }
        public string GenderName { get; set; }
        public bool? Active { get; set; }
        public int? AccessTypeId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string MultiSelectGuids { get; set; }
        public bool? IsUser { get; set; }

        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public string AccessTypeName { get; set; }
        public List<ClinicSectionViewModel> ClinicSections { get; set; }
        public virtual Guid[] ClinicSectionGuids { get; set; }
        public virtual ICollection<ClinicSectionUserViewModel> ClinicSectionUsers { get; set; }
    }
}