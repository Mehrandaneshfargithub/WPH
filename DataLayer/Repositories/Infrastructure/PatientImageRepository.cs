using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class PatientImageRepository : Repository<PatientImage>, IPatientImageRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PatientImageRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PatientImage> GetAttachmentsByPatientAndTypeId(Guid patientId, int? typeId)
        {
            return _context.PatientImages.AsNoTracking()
                .Where(x => x.PatientId == patientId && x.AttachmentTypeId == typeId);
        }

        public IEnumerable<PatientImage> GetAttachmentsByReceptionAndTypeId(Guid receptionId, int? typeId)
        {
            return _context.PatientImages.AsNoTracking()
                .Where(x => x.ReceptionId == receptionId && x.AttachmentTypeId == typeId);
        }

        public IEnumerable<PatientImage> GetAttachmentsByReceptionId(Guid receptionId)
        {
            return _context.PatientImages.AsNoTracking()
                .Where(x => x.ReceptionId == receptionId);
        }
    }
}
