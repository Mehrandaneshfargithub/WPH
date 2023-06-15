using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PatientImage
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? PatientId { get; set; }
        public string ImageAddress { get; set; }
        public string FileName { get; set; }
        public DateTime? ImageDateTime { get; set; }
        public Guid? ReceptionId { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string ThumbNailAddress { get; set; }
        public Guid? VisitId { get; set; }

        public virtual BaseInfoGeneral AttachmentType { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Reception Reception { get; set; }
    }
}
