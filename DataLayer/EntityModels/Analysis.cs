using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Analysis
    {
        public Analysis()
        {
            AnalysisAnalysisItems = new HashSet<AnalysisAnalysisItem>();
            AnalysisResults = new HashSet<AnalysisResult>();
            GroupAnalysisAnalyses = new HashSet<GroupAnalysisAnalysis>();
            PatientReceptionAnalyses = new HashSet<PatientReceptionAnalysis>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string Code { get; set; }
        public string Abbreviation { get; set; }
        public int? Priority { get; set; }
        public bool? IsButton { get; set; }
        public virtual User CreateUser { get; set; }
        public virtual BaseInfoGeneral DiscountCurrency { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual ICollection<AnalysisAnalysisItem> AnalysisAnalysisItems { get; set; }
        public virtual ICollection<AnalysisResult> AnalysisResults { get; set; }
        public virtual ICollection<GroupAnalysisAnalysis> GroupAnalysisAnalyses { get; set; }
        public virtual ICollection<PatientReceptionAnalysis> PatientReceptionAnalyses { get; set; }
    }
}
