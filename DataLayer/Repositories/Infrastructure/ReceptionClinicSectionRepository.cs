using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReceptionClinicSectionRepository : Repository<ReceptionClinicSection>, IReceptionClinicSectionRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionClinicSectionRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<ReceptionClinicSection> GetAllReceptionClinicSection()
        {
            return Context.ReceptionClinicSections.AsNoTracking();
        }

        public IEnumerable<ReceptionClinicSection> GetAllReceptionClinicSectionByReceptionId(Guid ReceptionId)
        {
            return Context.ReceptionClinicSections.Include(x => x.ClinicSection).AsNoTracking()
                .Where(x => x.ReceptionId == ReceptionId)
                .Select(x => new ReceptionClinicSection
                {
                    ClinicSection = new ClinicSection
                    {
                        Name = x.ClinicSection.Name
                    },
                    ClinicSectionId = x.ClinicSectionId,
                    Description = x.Description,
                    Guid = x.Guid,
                    ReceptionId = x.ReceptionId
                });
        }

        public IEnumerable<ReceptionClinicSection> GetAllReceptionClinicSectionByClinicSectionId(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid receptionId, Expression<Func<ReceptionClinicSection, bool>> predicate = null)
        {
            IQueryable<ReceptionClinicSection> result = Context.ReceptionClinicSections.Include(x => x.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User).AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId && x.DestinationReceptionId == null && x.CreatedDate <= dateTo && x.CreatedDate >= dateFrom)
                .Select(x => new ReceptionClinicSection
                {
                    ClinicSectionId = x.ClinicSectionId,
                    Description = x.Description,
                    Guid = x.Guid,
                    ReceptionId = x.ReceptionId,
                    Reception = new Reception
                    {
                        PatientId = x.Reception.PatientId,
                        ReceptionDoctors = (ICollection<ReceptionDoctor>)x.Reception.ReceptionDoctors.Select(a => new ReceptionDoctor
                        {
                            DoctorId = a.DoctorId
                        }),
                        Patient = new Patient
                        {
                            User = new User
                            {
                                Name = x.Reception.Patient.User.Name
                            }
                        }
                    }
                })
                ;

            if (predicate != null)
            {
                result = result.Where(predicate);
            }

            return result.ToList();
        }


        public IEnumerable<ReceptionClinicSection> GetPatientToAnotherSectionReport(DateTime dateFrom, DateTime dateTo, Guid clinicSectionId)
        {
            return Context.ReceptionClinicSections.AsNoTracking()
                .Include(x => x.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User)
                .Include(x => x.DestinationReception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.Analysis)
                .Include(x => x.DestinationReception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.AnalysisItem)
                .Include(x => x.DestinationReception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis)
                .Where(x => (clinicSectionId == Guid.Empty || x.ClinicSectionId == clinicSectionId) && x.DestinationReceptionId != null && x.DestinationReception.ReceptionDate <= dateTo && x.DestinationReception.ReceptionDate >= dateFrom)
                .Select(x => new ReceptionClinicSection
                {
                    ClinicSection = new ClinicSection
                    {
                        Name = x.ClinicSection.Name
                    },
                    Reception = new Reception
                    {
                        Patient = new Patient
                        {
                            User = new User
                            {
                                Name = x.Reception.Patient.User.Name
                            }
                        }
                    },
                    DestinationReception = new Reception
                    {
                        ReceptionDate = x.DestinationReception.ReceptionDate,
                        PatientReceptionAnalyses = (ICollection<PatientReceptionAnalysis>)x.DestinationReception.PatientReceptionAnalyses.Select(p => new PatientReceptionAnalysis
                        {
                            Amount = p.Amount,
                            Discount = p.Discount,
                            Analysis = new Analysis
                            {
                                Name = p.Analysis.Name,
                            },
                            AnalysisItem = new AnalysisItem
                            {
                                Name = p.AnalysisItem.Name,
                            },
                            GroupAnalysis = new GroupAnalysis
                            {
                                Name = p.GroupAnalysis.Name,
                            },
                        })
                    }
                }).ToList()
                ;
        }


        public ReceptionClinicSection GetReceptionClinicSectionByDestinationReceptionId(Guid DestinationReceptionId)
        {
            return Context.ReceptionClinicSections.AsNoTracking().SingleOrDefault(x => x.DestinationReceptionId == DestinationReceptionId);

        }

        public IEnumerable<Patient> GetAllReceptionClinicSectionPatients()
        {
            return Context.ReceptionClinicSections.AsNoTracking()
                .Include(a => a.DestinationReception)
                .ThenInclude(a => a.Patient)
                .ThenInclude(a => a.User)
                .Include(a => a.ClinicSection)
                .Where(a => a.DestinationReceptionId != null)
                .Select(a => new Patient
                {
                    User = new User
                    {
                        Name = a.DestinationReception == null ? "" : a.DestinationReception.Patient.User.Name + " | " + a.ClinicSection.Name,
                    },
                    Guid = a.DestinationReception.Patient.Guid

                });
        }
    }
}
