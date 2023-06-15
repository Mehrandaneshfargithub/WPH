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
    public class PatientVariableRepository : Repository<PatientVariable>, IPatientVariableRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PatientVariableRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PatientVariable> GetAllPatientVariables(Guid doctorId)
        {
            return _context.PatientVariables
                .Include(a => a.VariableType)
                .Include(a => a.VariableDisplay)
                .Include(a => a.VariableStatus)
                .Where(a => a.DoctorId == doctorId).Select(c => new PatientVariable
            {
                Abbreviation = c.Abbreviation,
                Description = c.Description,
                VariableName = c.VariableName,
                VariableType = new BaseInfoGeneral
                {
                    Name = c.VariableType.Name
                },
                VariableStatus = new BaseInfoGeneral
                {
                    Name = c.VariableStatus.Name
                },
                    VariableDisplay = new BaseInfoGeneral
                    {
                        Name = c.VariableDisplay.Name
                    },
                VariableUnit = c.VariableUnit
            });
        }

        public IEnumerable<PatientVariable> GetAllVariablesForPatient(Guid doctorId, Guid patientId, Guid receptionId, string DisplayType)
        {
            IQueryable<PatientVariable> result = _context.PatientVariables
                .Include(a => a.VariableType)
                .Include(a => a.VariableDisplay)
                .Include(a => a.VariableStatus)
                .Include(a => a.PatientVariablesValues)
                .Where(a => a.DoctorId == doctorId ).Select(c => new PatientVariable
                {
                    Id = c.Id,
                    Abbreviation = c.Abbreviation,
                    VariableName = c.VariableName,
                    VariableType = new BaseInfoGeneral
                    {
                        Name = c.VariableType.Name
                    },
                    VariableStatus = new BaseInfoGeneral
                    {
                        Name = c.VariableStatus.Name
                    },
                    VariableDisplay = new BaseInfoGeneral
                    {
                        Name = c.VariableDisplay.Name
                    },
                    VariableUnit = c.VariableUnit,
                    PatientVariablesValues = c.PatientVariablesValues.Where(b=>b.PatientId == patientId && b.ReceptionId == null || b.ReceptionId == receptionId).Select(b=>new PatientVariablesValue
                    {
                        Guid = b.Guid,
                        PatientId = b.PatientId,
                        ReceptionId = b.ReceptionId,
                        Value = b.Value
                    }).ToList()
                });

            if (string.IsNullOrWhiteSpace(DisplayType))
                return result;

            return result.Where(a => a.VariableDisplay.Name == DisplayType);


        }
    }
}
