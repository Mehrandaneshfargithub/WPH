using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class ClinicSectionChoosenValueRepository : Repository<ClinicSectionChoosenValue>, IClinicSectionChoosenValueRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ClinicSectionChoosenValueRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ClinicSectionChoosenValue> GetAllClinicSectionChoosenValue(Guid clinicSectionId)
        {
            return _context.ClinicSectionChoosenValues.AsNoTracking()
                .Include(x => x.VariableStatus)
                .Include(x => x.VariableDisplay)
                .Include(x => x.PatientVariable)
                .Where(x => x.ClinicSectionId == clinicSectionId);
                
        }

        public IEnumerable<ClinicSectionChoosenValue> GetAllNumericClinicSectionChoosenValues(Guid clinicSectionId)
        {
            return _context.ClinicSectionChoosenValues.AsNoTracking()
                .Include(x => x.VariableStatus)
                .Include(x => x.VariableDisplay)
                .Include(x => x.PatientVariable)
                .Where(x => x.ClinicSectionId == clinicSectionId /*&& x.PatientVariable.VariableType == "Int" || x.PatientVariable.VariableType == "Decimal"*/)
                ;

        }

        public IEnumerable<ClinicSectionChoosenValue> GetAllFillBySecretaryClinicSectionChoosenValues(Guid clinicSectionId)
        {
            return _context.ClinicSectionChoosenValues.AsNoTracking()
                .Include(x => x.VariableStatus)
                .Include(x => x.VariableDisplay)
                .Include(x => x.PatientVariable)
                .Where(x => x.ClinicSectionId == clinicSectionId && x.FillBySecretary == true)
                ;
        }
    }
}
