using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PatientReceptionAnalysis
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid ReceptionId { get; set; }
        public decimal? Amount { get; set; }
        public int? AmountCurrencyId { get; set; }
        public decimal? Discount { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public Guid? GroupAnalysisId { get; set; }

        public virtual BaseInfoGeneral AmountCurrency { get; set; }
        public virtual Analysis Analysis { get; set; }
        public virtual AnalysisItem AnalysisItem { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual GroupAnalysis GroupAnalysis { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Reception Reception { get; set; }
    }
}
