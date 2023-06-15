using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class AnalysisResultMaster
    {
        public AnalysisResultMaster()
        {
            AnalysisResults = new HashSet<AnalysisResult>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Description { get; set; }
        public int? PrintedNum { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid ReceptionId { get; set; }
        public int? ServerNumber { get; set; }
        public DateTime? UploadDate { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual ICollection<AnalysisResult> AnalysisResults { get; set; }
    }
}
