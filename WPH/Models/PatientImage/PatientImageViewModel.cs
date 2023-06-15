using System;

namespace WPH.Models.PatientImage
{
    public class PatientImageViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? PatientId { get; set; }
        public string ImageAddress { get; set; }
        public string FileName { get; set; }
        public Guid? VisitId { get; set; }
        public DateTime? ImageDateTime { get; set; }
        public Guid? ReceptionId { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string ThumbNailAddress { get; set; }
    }
}