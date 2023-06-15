using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class GroupAnalysis
    {
        public GroupAnalysis()
        {
            AnalysisResults = new HashSet<AnalysisResult>();
            GroupAnalysisAnalyses = new HashSet<GroupAnalysisAnalysis>();
            GroupAnalysisItems = new HashSet<GroupAnalysisItem>();
            PatientReceptionAnalyses = new HashSet<PatientReceptionAnalysis>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public int? Priority { get; set; }
        public string Code { get; set; }
        public string Abbreviation { get; set; }
        public bool? IsButton { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral DiscountCurrency { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual ICollection<AnalysisResult> AnalysisResults { get; set; }
        public virtual ICollection<GroupAnalysisAnalysis> GroupAnalysisAnalyses { get; set; }
        public virtual ICollection<GroupAnalysisItem> GroupAnalysisItems { get; set; }
        public virtual ICollection<PatientReceptionAnalysis> PatientReceptionAnalyses { get; set; }
    }
}
