using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.AnalysisResult;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;

namespace WPH.Models.CustomDataModels.AnalysisResultMaster
{
    public class AnalysisResultMasterViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Description { get; set; }
        public string LastInvoiceNum { get; set; }
        public string FirstInvoiceNum { get; set; }
        public int? PrintedNum { get; set; }
        public string Ready { get; set; }
        public string AllAnalysisName { get; set; }
        public string ClinicSectionTypeName { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid ReceptionId { get; set; }
        public decimal? PatientReceptionRemained { get; set; }
        public int? ServerNumber { get; set; }
        public DateTime? UploadDate { get; set; }

        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
        public virtual List<AnalysisResultViewModel> AnalysisResults { get; set; }
        public virtual ReceptionViewModel PatientReception { get; set; }

    }
}