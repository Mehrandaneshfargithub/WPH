using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class AnalysisItem
    {
        public AnalysisItem()
        {
            AnalysisAnalysisItems = new HashSet<AnalysisAnalysisItem>();
            AnalysisItemMinMaxValues = new HashSet<AnalysisItemMinMaxValue>();
            AnalysisItemValuesRanges = new HashSet<AnalysisItemValuesRange>();
            AnalysisResults = new HashSet<AnalysisResult>();
            GroupAnalysisItems = new HashSet<GroupAnalysisItem>();
            PatientReceptionAnalyses = new HashSet<PatientReceptionAnalysis>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string NormalValues { get; set; }
        public int? Priority { get; set; }
        public decimal? Amount { get; set; }
        public int? AmountCurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Code { get; set; }
        public string Abbreviation { get; set; }
        public int? ValueTypeId { get; set; }
        public Guid? UnitId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public bool? IsButton { get; set; }
        public bool? ShowChart { get; set; }

        public virtual BaseInfoGeneral AmountCurrency { get; set; }
        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual BaseInfo Unit { get; set; }
        public virtual BaseInfoGeneral ValueType { get; set; }
        public virtual ICollection<AnalysisAnalysisItem> AnalysisAnalysisItems { get; set; }
        public virtual ICollection<AnalysisItemMinMaxValue> AnalysisItemMinMaxValues { get; set; }
        public virtual ICollection<AnalysisItemValuesRange> AnalysisItemValuesRanges { get; set; }
        public virtual ICollection<AnalysisResult> AnalysisResults { get; set; }
        public virtual ICollection<GroupAnalysisItem> GroupAnalysisItems { get; set; }
        public virtual ICollection<PatientReceptionAnalysis> PatientReceptionAnalyses { get; set; }
    }
}
