using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class PatientVariablesValueRepository : Repository<PatientVariablesValue>, IPatientVariablesValueRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PatientVariablesValueRepository(WASContext context)
            : base(context)
        {
        }

        public List<PatientVariablesValue> GetAllPatientVariablesValueBasedOnVisitId(Guid visitId)
        {
            return _context.PatientVariablesValues
                .AsNoTracking()
                .Include(x=>x.PatientVariable).Where(x => x.ReceptionId == visitId).ToList();
        }

        public void UpdatePatientVariablesValueBasedOnGuid(Guid Guid, string value)
        {
            try
            {
                PatientVariablesValue pv = new PatientVariablesValue() { Guid = Guid, Value = value };
                _context.PatientVariablesValues.Attach(pv);
                _context.Entry(pv).Property(x => x.Value).IsModified = true;
                _context.SaveChanges();
            }
            catch(Exception e) { throw e; }
            
        }

        public List<PatientVariablesValue> GetAllPatientSpeceficVariable(Guid receptionId, string variableName)
        {
            return _context.PatientVariablesValues.AsNoTracking()
                .Include(x => x.PatientVariable).Where(x => x.ReceptionId == receptionId && x.PatientVariable.VariableName.Trim() == variableName.Trim()).ToList();
        }

        public IEnumerable<PatientVariablesValue> GetAllReceptionVariable(Guid receptionId)
        {
            return _context.PatientVariablesValues.AsNoTracking()
                .Include(x => x.PatientVariable).Where(x => x.ReceptionId == receptionId).OrderByDescending(x=>x.Id);
        }
    }
}
