using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class PatientReceptionRepository : Repository<PatientReception>, IPatientReceptionRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }

        public PatientReceptionRepository(WASContext context)
                   : base(context)
        {
        }

        public Guid AddNewPatientReception(Reception patientReception, bool newDoctor)
        {
            try
            {
                if (patientReception.HospitalReception ?? false)
                {

                    patientReception.Patient = null;
                    if (patientReception.ReceptionDoctors.Count != 0)
                        patientReception.ReceptionDoctors.FirstOrDefault().Doctor = null;
                }

                Context.Receptions.Add(patientReception);
                Context.SaveChanges();
                return patientReception.Guid;
            }
            catch (Exception e) { throw e; }
        }

        public Guid UpdatePatientReception(Reception patientReception, bool newPatient, bool newDoctor)
        {
            try
            {
                Guid analysisMasterId = Guid.Empty;
                try
                {
                    analysisMasterId = Context.PatientReceptionAnalyses.Where(x => x.ReceptionId == patientReception.Guid).FirstOrDefault().Guid;
                }
                catch
                {
                    analysisMasterId = Guid.Empty;
                }

                

                if (analysisMasterId != Guid.Empty)
                {
                    Context.AnalysisResults.RemoveRange(Context.AnalysisResults.Where(x => x.AnalysisResultMasterId == analysisMasterId));
                }

                try
                {
                    if (patientReception.HospitalReception ?? false)
                    {

                        patientReception.Patient = null;
                        if (patientReception.ReceptionDoctors.Count != 0)
                            patientReception.ReceptionDoctors.FirstOrDefault().Doctor = null;
                    }
                    else
                    {
                        if (newPatient)
                        {
                            _context.Patients.Add(patientReception.Patient);
                        }
                    }
                }
                catch { }
                Context.PatientReceptionAnalyses.RemoveRange(Context.PatientReceptionAnalyses.Where(x => x.ReceptionId == patientReception.Guid));
                Context.PatientReceptionAnalyses.AddRange(patientReception.PatientReceptionAnalyses);

                Context.ReceptionDoctors.RemoveRange(Context.ReceptionDoctors.Where(x => x.ReceptionId == patientReception.Guid));
                Context.ReceptionDoctors.AddRange(patientReception.ReceptionDoctors);
                Context.Receptions.Attach(patientReception);
                Context.Entry(patientReception).State = EntityState.Modified;
                Context.Entry(patientReception).Property(x => x.ReceptionDate).IsModified = false;
                Context.Entry(patientReception).Property(x => x.CreatedDate).IsModified = false;
                Context.SaveChanges();
                return patientReception.Guid;
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<PatientImage> RemovePatientReceptionWithReceives(Guid patientReceptionId)
        {
            Reception pr = _context.Receptions.AsNoTracking()
                .Include(p => p.PatientReceptionAnalyses)
                .Include(p => p.ReceptionServices).ThenInclude(p => p.ReceptionServiceReceiveds)
                .Include(p => p.ReceptionDoctors)
                .Include(p => p.ReceptionClinicSections)
                .Include(p => p.AnalysisResultMasters).ThenInclude(p => p.AnalysisResults)
                .Include(p => p.PatientImages)
                .SingleOrDefault(p => p.Guid == patientReceptionId);

            Context.PatientReceptionAnalyses.RemoveRange(pr.PatientReceptionAnalyses);
            Context.ReceptionServices.RemoveRange(pr.ReceptionServices);
            Context.ReceptionServiceReceiveds.RemoveRange(pr.ReceptionServices.SelectMany(p => p.ReceptionServiceReceiveds));
            Context.ReceptionDoctors.RemoveRange(pr.ReceptionDoctors);
            Context.ReceptionClinicSections.RemoveRange(pr.ReceptionClinicSections);
            Context.AnalysisResultMasters.RemoveRange(pr.AnalysisResultMasters);

            Context.AnalysisResults.RemoveRange(pr.AnalysisResultMasters.SelectMany(p => p.AnalysisResults));


            Context.Receptions.Attach(pr);
            Context.Entry(pr).State = EntityState.Deleted;
            var images = pr.PatientImages;
            return images;
        }


        public Reception GetPatientReceptionByIdWithDoctor(Guid patientReceptionId)
        {
            return Context.Receptions
                .AsNoTracking()
                .Include(x => x.ReceptionDoctors).ThenInclude(x => x.Doctor).ThenInclude(x => x.User)
                .Include(x => x.Patient).ThenInclude(x => x.User)
                .Select(x => new Reception
                {
                    ClinicSectionId = x.ClinicSectionId,
                    CreatedDate = x.CreatedDate,
                    CreatedUserId = x.CreatedUserId,
                    Description = x.Description,
                    Discount = x.Discount,
                    ReceptionDoctors = (ICollection<ReceptionDoctor>)x.ReceptionDoctors.Select(a => new ReceptionDoctor
                    {
                        Doctor = new Doctor
                        {
                            SpecialityId = a.Doctor.SpecialityId,
                            User = new User
                            {
                                Name = a.Doctor.User.Name
                            }
                        }
                    }),
                    Guid = x.Guid,
                    ReceptionDate = x.ReceptionDate,
                    ReceptionNum = x.ReceptionNum,
                    Patient = x.Patient,
                    PatientId = x.PatientId,
                    HospitalReception = x.HospitalReception


                }).SingleOrDefault(x => x.Guid == patientReceptionId);
        }

        public Reception GetPatientReceptionById(Guid patientReceptionId)
        {
            return Context.Receptions
                .AsNoTracking()
                .Include(x => x.Patient).ThenInclude(x => x.User)
                .Select(x => new Reception
                {
                    ClinicSectionId = x.ClinicSectionId,
                    CreatedDate = x.CreatedDate,
                    CreatedUserId = x.CreatedUserId,
                    Description = x.Description,
                    Discount = x.Discount,
                    Guid = x.Guid,
                    ReceptionDate = x.ReceptionDate,
                    ReceptionNum = x.ReceptionNum,
                    Patient = x.Patient,
                    PatientId = x.PatientId,
                    HospitalReception = x.HospitalReception
                }).SingleOrDefault(x => x.Guid == patientReceptionId);
        }


        public string GetLatestReceptionInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                return Context.FN_LatestReceptionInvoiceNum(clinicSectionId).FirstOrDefault().CODE;
            }
            catch (Exception e) { return "1"; }

        }

        public IEnumerable<Reception> GetAllPatientReceptionInvoiceNums(Guid clinicSectionId)
        {
            return Context.Receptions
                .AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId)
                .Select(a => new Reception
                {
                    Guid = a.Guid,
                    ReceptionNum = a.ReceptionNum

                });
        }

        public IEnumerable<Reception> GetAllPatientReceptionPatients(Guid clinicSectionId)
        {
            return Context.Receptions
                .AsNoTracking()
                 .Include(x => x.Patient).ThenInclude(x => x.User)
                 .Where(x => x.ClinicSectionId == clinicSectionId)
                 .Select(a => new Reception
                 {
                     Guid = a.Guid,
                     Patient = new Patient
                     {
                         User = new User
                         {
                             Name = a.Patient.User.Name,
                             PhoneNumber = a.Patient.User.PhoneNumber
                         }

                     }

                 });
        }

        public Reception GetPatientReceptionByIdForAnalysisResult(Guid patientReceptionId)
        {
            return Context.Receptions
                .AsNoTracking()
                .Include(x => x.ReceptionDoctors).ThenInclude(x => x.Doctor).ThenInclude(x => x.User)
                .Include(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType)
                .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Select(x => new Reception
                {
                    //Barcode = x.Barcode,
                    //BaseCurrencyId = x.BaseCurrencyId,
                    ClinicSectionId = x.ClinicSectionId,
                    CreatedDate = x.CreatedDate,
                    CreatedUserId = x.CreatedUserId,
                    Description = x.Description,
                    ClinicSection = new ClinicSection
                    {
                        ClinicSectionType = new BaseInfoGeneral
                        {
                            Name = x.ClinicSection.ClinicSectionType.Name,
                        }
                    },
                    //Discount = x.Discount,
                    ReceptionDoctors = x.ReceptionDoctors.Select(a => new ReceptionDoctor
                    {
                        Doctor = a.Doctor == null ? null : new Doctor
                        {
                            User = new User
                            {
                                Name = a.Doctor.User.Name
                            }
                        },
                    }).ToList(),

                    //DoctorId = x.DoctorId,
                    Guid = x.Guid,
                    ReceptionDate = x.ReceptionDate,
                    ReceptionNum = x.ReceptionNum,
                    Patient = new Patient
                    {
                        DateOfBirth = x.Patient.DateOfBirth,
                        User = new User
                        {
                            Name = x.Patient.User.Name,
                            GenderId = x.Patient.User.GenderId,
                            PhoneNumber = x.Patient.User.PhoneNumber,
                            Gender = x.Patient.User.Gender
                        }
                    },
                    PatientId = x.PatientId,
                    PatientReceptionAnalyses = x.PatientReceptionAnalyses.Select(a => new PatientReceptionAnalysis
                    {
                        Analysis = a.Analysis == null ? null : new Analysis
                        {
                            Name = a.Analysis.Name,
                            AnalysisAnalysisItems = a.Analysis.AnalysisAnalysisItems.Select(b => new AnalysisAnalysisItem
                            {
                                AnalysisId = b.AnalysisId,
                                AnalysisItem = new AnalysisItem
                                {
                                    Guid = b.AnalysisItem.Guid,
                                    ValueType = b.AnalysisItem.ValueType,
                                    Name = b.AnalysisItem.Name,
                                    AnalysisItemMinMaxValues = b.AnalysisItem.AnalysisItemMinMaxValues,
                                    AnalysisItemValuesRanges = b.AnalysisItem.AnalysisItemValuesRanges,
                                    NormalValues = b.AnalysisItem.NormalValues
                                }
                            }).ToList()
                        },
                        AnalysisItem = a.AnalysisItem == null ? null : new AnalysisItem
                        {
                            AnalysisItemMinMaxValues = a.AnalysisItem.AnalysisItemMinMaxValues,
                            AnalysisItemValuesRanges = a.AnalysisItem.AnalysisItemValuesRanges,
                            Name = a.AnalysisItem.Name,
                            Guid = a.AnalysisItem.Guid,
                            ValueType = a.AnalysisItem.ValueType,
                            NormalValues = a.AnalysisItem.NormalValues,

                        },
                        GroupAnalysis = a.GroupAnalysis == null ? null : new GroupAnalysis
                        {
                            Guid = a.GroupAnalysis.Guid,
                            Name = a.GroupAnalysis.Name,
                            GroupAnalysisItems = a.GroupAnalysis.GroupAnalysisItems.Select(b => new GroupAnalysisItem
                            {
                                Guid = b.Guid,
                                AnalysisItem = b.AnalysisItem == null ? null : new AnalysisItem
                                {
                                    Guid = b.AnalysisItem.Guid,
                                    AnalysisItemMinMaxValues = b.AnalysisItem.AnalysisItemMinMaxValues,
                                    AnalysisItemValuesRanges = b.AnalysisItem.AnalysisItemValuesRanges,
                                    Name = b.AnalysisItem.Name,
                                    NormalValues = b.AnalysisItem.NormalValues,
                                    ValueType = b.AnalysisItem.ValueType,

                                },
                                GroupAnalysisId = b.GroupAnalysisId,
                                AnalysisItemId = b.AnalysisItemId
                            }).ToList(),
                            GroupAnalysisAnalyses = a.GroupAnalysis.GroupAnalysisAnalyses.Select(b => new GroupAnalysisAnalysis
                            {
                                AnalysisId = b.AnalysisId,
                                GroupAnalysisId = b.GroupAnalysisId,
                                Guid = b.Guid,
                                Analysis = b.Analysis == null ? null : new Analysis
                                {
                                    Name = b.Analysis.Name,
                                    AnalysisAnalysisItems = b.Analysis.AnalysisAnalysisItems.Select(c => new AnalysisAnalysisItem
                                    {
                                        AnalysisId = c.AnalysisId,
                                        AnalysisItem = c.AnalysisItem == null ? null : new AnalysisItem
                                        {
                                            Guid = c.AnalysisItem.Guid,
                                            ValueType = c.AnalysisItem.ValueType,
                                            Name = c.AnalysisItem.Name,
                                            AnalysisItemMinMaxValues = c.AnalysisItem.AnalysisItemMinMaxValues,
                                            AnalysisItemValuesRanges = c.AnalysisItem.AnalysisItemValuesRanges,
                                            NormalValues = c.AnalysisItem.NormalValues
                                        }
                                    }).ToList()
                                },
                            }).ToList()
                        }

                    }).ToList()



                }).SingleOrDefault(x => x.Guid == patientReceptionId);
        }

        public Reception GetPatientReceptionByIdForReport(Guid patienReceptionId)
        {
            return Context.Receptions
                .AsNoTracking()
                 .Include(x => x.ReceptionDoctors).ThenInclude(x => x.Doctor).ThenInclude(x => x.User)
                 .Include(x => x.ReceptionDoctors).ThenInclude(x => x.DoctorRole)
                 .Include(x => x.Patient).ThenInclude(x => x.User)
                 .Include(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType)
                 //.Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.AmountCurrency)
                 .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.Analysis)
                 .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.AnalysisItem)
                 .Include(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis)
                 //.Include(x => x.PatientReceptionReceiveds)
                 .Where(x => x.Guid == patienReceptionId)
                 .Select(a => new Reception
                 {
                     ClinicSection = new ClinicSection
                     {
                         Name = a.ClinicSection.Name,
                         ClinicSectionType = new BaseInfoGeneral
                         {
                             Name = a.ClinicSection.ClinicSectionType.Name,
                         }
                     },
                     Patient = new Patient
                     {
                         DateOfBirth = a.Patient.DateOfBirth,
                         User = new User
                         {
                             Name = a.Patient.User.Name,
                             PhoneNumber = a.Patient.User.PhoneNumber
                         }

                     },
                     ReceptionDoctors = a.ReceptionDoctors.Where(x => x.DoctorRole.Name == "DispatcherDoctor").Select(a => new ReceptionDoctor
                     {
                         Doctor = a.Doctor == null ? null : new Doctor
                         {
                             User = new User
                             {
                                 Name = a.Doctor.User.Name
                             }
                         },
                     }).ToList(),
                     ReceptionDate = a.ReceptionDate,
                     ReceptionNum = a.ReceptionNum,
                     Discount = a.Discount,

                     PatientReceptionAnalyses = a.PatientReceptionAnalyses.Select(b => new PatientReceptionAnalysis
                     {
                         Amount =  b.Amount,
                         AmountCurrencyId = b.AmountCurrencyId,
                         Discount =  b.Discount,
                         Analysis = b.Analysis,

                         AnalysisItem = b.AnalysisItem,

                         GroupAnalysis = b.GroupAnalysis,

                     }).ToList()

                 }).FirstOrDefault();
        }

    }
}
