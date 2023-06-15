using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class AnalysisResultMasterRepository : Repository<AnalysisResultMaster>, IAnalysisResultMasterRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }

        public AnalysisResultMasterRepository(WASContext context)
                   : base(context)
        {
        }


        public IEnumerable<AnalysisResultMaster> GetAllAnalysisResultMaster(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo)
        {
            return Context.AnalysisResultMasters.AsNoTracking()
            .Include(x => x.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User)
            .Where(x => x.CreatedDate >= dateFrom && x.CreatedDate < dateTo && x.Reception.ClinicSectionId == clinicSectionId)
            .Select(x => new AnalysisResultMaster
            {
                Guid = x.Guid,
                CreatedDate = x.CreatedDate,
                PrintedNum = x.PrintedNum,
                InvoiceDate = x.InvoiceDate,
                ReceptionId = x.ReceptionId,
                AnalysisResults = x.AnalysisResults.Select(a => new AnalysisResult
                {

                }).ToList(),
                Reception = new Reception
                {

                    Discount = x.Reception.Discount,
                    Patient = new Patient
                    {
                        DateOfBirth = x.Reception.Patient.DateOfBirth,
                        User = new User
                        {
                            Name = x.Reception.Patient.User.Name,
                            PhoneNumber = x.Reception.Patient.User.PhoneNumber,
                            Gender = x.Reception.Patient.User.Gender
                        }
                    },
                    ReceptionDate = x.Reception.ReceptionDate,
                    ReceptionNum = x.Reception.ReceptionNum,

                },



            });
        }


        public IEnumerable<AnalysisResultMasterGrid> GetAllAnalysisResultMasterByUserId(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo)
        {

            string sqlFormattedDateFrom = dateFrom.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string sqlFormattedDateTo = dateTo.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //HumanResourceSalary.Salary as HumanResourceSalary,
            string qury = "SELECT AnalysisResultMaster.GUID, AnalysisResultMaster.ReceptionId, AnalysisResultMaster.InvoiceDate, AnalysisResultMaster.PrintedNum,AnalysisResultMaster.ServerNumber," +
                "AnalysisResultMaster.UploadDate ,PatientUsers.Name as PatientName, Gender.Name as Gender, PatientUsers.PhoneNumber as PhoneNumber," +
                "Reception.ReceptionNum as ReceptionNum, Patient.DateOfBirth as DateOfBirth, ClinicSectionType.Name as ClinicSectionTypeName" +
                " FROM AnalysisResultMaster left outer JOIN Reception AS Reception ON AnalysisResultMaster.ReceptionId = Reception.GUID" +
                " left outer JOIN Patient AS Patient ON Reception.PatientId = Patient.GUID" +
                " left outer JOIN[dbo].[User] AS PatientUsers ON Patient.GUID = PatientUsers.GUID" +
                " left outer JOIN BaseInfoGeneral AS Gender ON PatientUsers.GenderId = Gender.Id" +
                " left outer JOIN ClinicSection AS ClinicSection ON Reception.ClinicSectionId = ClinicSection.GUID" +
                " left outer JOIN BaseInfoGeneral AS ClinicSectionType ON ClinicSection.ClinicSectionTypeId = ClinicSectionType.Id " +
                 $" WHERE Reception.ReceptionDate <= '{sqlFormattedDateTo}'  AND Reception.ReceptionDate > '{sqlFormattedDateFrom}' AND Reception.ClinicSectionId = '{clinicSectionId}'";

            IEnumerable<AnalysisResultMasterGrid> we;
            try
            {
                we = Context.Set<AnalysisResultMasterGrid>().FromSqlRaw(qury);
            }
            catch (Exception e) { return null; }


            return we.OrderByDescending(a => a.InvoiceDate);


            //return Context.AnalysisResultMasters.AsNoTracking()
            //.Include(x => x.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User).ThenInclude(a => a.Gender)
            //.Include(x => x.Reception).ThenInclude(a => a.ClinicSection).ThenInclude(a => a.ClinicSectionType)
            //.Select(analysisResultMaster =>  new AnalysisResultMaster
            //{
            //    Guid = analysisResultMaster.Guid,
            //    PrintedNum = analysisResultMaster.PrintedNum,
            //    InvoiceDate = analysisResultMaster.InvoiceDate,
            //    ReceptionId = analysisResultMaster.ReceptionId,
            //    Reception = (analysisResultMaster.Reception == null)? null : new Reception
            //    {
            //        ClinicSectionId = analysisResultMaster.Reception.ClinicSectionId,
            //        ClinicSection = new ClinicSection
            //        {
            //            ClinicSectionType = new BaseInfoGeneral
            //            {
            //                Name = analysisResultMaster.Reception.ClinicSection.ClinicSectionType.Name
            //            }
            //        },
            //        Patient = new Patient
            //        {
            //            DateOfBirth = analysisResultMaster.Reception.Patient.DateOfBirth,
            //            User = new User
            //            {
            //                Name = analysisResultMaster.Reception.Patient.User.Name,
            //                PhoneNumber = analysisResultMaster.Reception.Patient.User.PhoneNumber,
            //                Gender = analysisResultMaster.Reception.Patient.User.Gender
            //            }
            //        },
            //        ReceptionDate = analysisResultMaster.Reception.ReceptionDate,
            //        ReceptionNum = analysisResultMaster.Reception.ReceptionNum,
            //    },

            //}).Where(x => x.InvoiceDate >= dateFrom && x.InvoiceDate < dateTo && x.Reception.ClinicSectionId == clinicSectionId).OrderByDescending(a=>a.InvoiceDate);


        }


        public IEnumerable<AnalysisResultMasterGrid> GetAnalysisResultByPatientId(Guid patientId)
        {


            string qury = "SELECT AnalysisResultMaster.GUID, AnalysisResultMaster.ReceptionId, AnalysisResultMaster.InvoiceDate, AnalysisResultMaster.PrintedNum," +
                "PatientUsers.Name as PatientName, Gender.Name as Gender, PatientUsers.PhoneNumber as PhoneNumber," +
                "Reception.ReceptionNum as ReceptionNum, Patient.DateOfBirth as DateOfBirth, ClinicSectionType.Name as ClinicSectionTypeName,AnalysisResultMaster.ServerNumber,AnalysisResultMaster.UploadDate" +
                " FROM AnalysisResultMaster left outer JOIN Reception AS Reception ON AnalysisResultMaster.ReceptionId = Reception.GUID" +
                " left outer JOIN Patient AS Patient ON Reception.PatientId = Patient.GUID" +
                " left outer JOIN[dbo].[User] AS PatientUsers ON Patient.GUID = PatientUsers.GUID" +
                " left outer JOIN BaseInfoGeneral AS Gender ON PatientUsers.GenderId = Gender.Id" +
                " left outer JOIN ClinicSection AS ClinicSection ON Reception.ClinicSectionId = ClinicSection.GUID" +
                " left outer JOIN BaseInfoGeneral AS ClinicSectionType ON ClinicSection.ClinicSectionTypeId = ClinicSectionType.Id " +
                 $" WHERE Reception.PatientId = '{patientId}'";

            IEnumerable<AnalysisResultMasterGrid> we;
            try
            {
                we = Context.Set<AnalysisResultMasterGrid>().FromSqlRaw(qury);
            }
            catch (Exception e) { return null; }


            return we.OrderByDescending(a => a.InvoiceDate);


            //return Context.AnalysisResultMasters.AsNoTracking()
            //.Include(x => x.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User).ThenInclude(a => a.Gender)
            //.Include(x => x.Reception).ThenInclude(a => a.ClinicSection).ThenInclude(a => a.ClinicSectionType)
            //.Select(analysisResultMaster => new AnalysisResultMaster
            //{
            //    Guid = analysisResultMaster.Guid,
            //    PrintedNum = analysisResultMaster.PrintedNum,
            //    InvoiceDate = analysisResultMaster.InvoiceDate,
            //    ReceptionId = analysisResultMaster.ReceptionId,
            //    Reception = (analysisResultMaster.Reception == null) ? null : new Reception
            //    {
            //        PatientId = analysisResultMaster.Reception.PatientId,
            //        ClinicSectionId = analysisResultMaster.Reception.ClinicSectionId,
            //        ClinicSection = new ClinicSection
            //        {
            //            ClinicSectionType = new BaseInfoGeneral
            //            {
            //                Name = analysisResultMaster.Reception.ClinicSection.ClinicSectionType.Name
            //            }
            //        },
            //        Patient = new Patient
            //        {
            //            DateOfBirth = analysisResultMaster.Reception.Patient.DateOfBirth,
            //            User = new User
            //            {
            //                Name = analysisResultMaster.Reception.Patient.User.Name,
            //                PhoneNumber = analysisResultMaster.Reception.Patient.User.PhoneNumber,
            //                Gender = analysisResultMaster.Reception.Patient.User.Gender
            //            }
            //        },
            //        ReceptionDate = analysisResultMaster.Reception.ReceptionDate,
            //        ReceptionNum = analysisResultMaster.Reception.ReceptionNum,
            //    },

            //}).Where(x => x.Reception.PatientId == patientId).OrderByDescending(a => a.InvoiceDate);
        }

        public AnalysisResultMaster GetAnalysisResultMasterByIdForAnalysisResult(Guid AnalysisResultMasterId)
        {
            return Context.AnalysisResultMasters.AsNoTracking()
                .Include(a=>a.AnalysisResults)
                .Include(x => x.Reception).ThenInclude(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType)
                .Include(x => x.Reception).ThenInclude(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses)
                .Select(x => new AnalysisResultMaster
                {
                    Guid = x.Guid,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    InvoiceDate = x.InvoiceDate,
                    ReceptionId = x.ReceptionId,
                    PrintedNum = x.PrintedNum,
                    Reception = new Reception
                    {

                        ClinicSectionId = x.Reception.ClinicSectionId,
                        CreatedDate = x.CreatedDate,
                        CreatedUserId = x.CreatedUserId,
                        Description = x.Description,
                        Discount = x.Reception.Discount,
                        ClinicSection = x.Reception.ClinicSection,
                        Guid = x.Guid,
                        ReceptionDate = x.InvoiceDate,
                        ReceptionNum = x.Reception.ReceptionNum,
                        
                        Patient = new Patient
                        {
                            DateOfBirth = x.Reception.Patient.DateOfBirth,
                            User = new User
                            {
                                Name = x.Reception.Patient.User.Name,
                                GenderId = x.Reception.Patient.User.GenderId,
                                PhoneNumber = x.Reception.Patient.User.PhoneNumber,
                                Gender = x.Reception.Patient.User.Gender
                            }
                        },
                        PatientId = x.Reception.PatientId,
                        PatientReceptionAnalyses = x.Reception.PatientReceptionAnalyses.Select(a=>new PatientReceptionAnalysis
                        {
                            AnalysisId = a.AnalysisId,
                            AnalysisItemId = a.AnalysisItemId,
                            GroupAnalysisId = a.GroupAnalysisId
                        }).ToList()
                    },
                    AnalysisResults = x.AnalysisResults
                    
                }).SingleOrDefault(x => x.Guid == AnalysisResultMasterId);

        }


        public AnalysisResultMaster GetAnalysisResultMasterByInvoiceNum(Guid clinicSectionId, string invoiceNum)
        {
            return Context.AnalysisResultMasters.AsNoTracking()
               .Include(x => x.AnalysisResults)
                .Include(x => x.Reception).ThenInclude(x => x.Patient).ThenInclude(x => x.User)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.Reception).ThenInclude(x => x.PatientReceptionAnalyses).ThenInclude(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
               .Select(x => new AnalysisResultMaster
               {
                   Guid = x.Guid,
                   CreatedDate = x.CreatedDate,
                   Description = x.Description,
                   InvoiceDate = x.InvoiceDate,
                   AnalysisResults = x.AnalysisResults,
                   ReceptionId = x.ReceptionId,
                   PrintedNum = x.PrintedNum,

                   Reception = new Reception
                   {

                       ClinicSectionId = x.Reception.ClinicSectionId,
                       CreatedDate = x.CreatedDate,
                       CreatedUserId = x.CreatedUserId,
                       Description = x.Description,
                       Discount = x.Reception.Discount,

                       Guid = x.Guid,
                       ReceptionDate = x.InvoiceDate,
                       ReceptionNum = x.Reception.ReceptionNum,
                       Patient = new Patient
                       {
                           DateOfBirth = x.Reception.Patient.DateOfBirth,
                           User = new User
                           {
                               Name = x.Reception.Patient.User.Name,
                               GenderId = x.Reception.Patient.User.GenderId,
                               PhoneNumber = x.Reception.Patient.User.PhoneNumber,
                               Gender = x.Reception.Patient.User.Gender
                           }
                       },
                       PatientId = x.Reception.PatientId ?? Guid.Empty,
                       PatientReceptionAnalyses = x.Reception.PatientReceptionAnalyses.Select(a => new PatientReceptionAnalysis
                       {
                           Amount = a.Amount,
                           AmountCurrencyId = a.AmountCurrencyId,
                           Discount = a.Discount,
                           Analysis = a.Analysis == null ? null : new Analysis
                           {
                               Guid = a.Analysis.Guid,
                               Name = a.Analysis.Name,
                               AnalysisAnalysisItems = a.Analysis.AnalysisAnalysisItems.Select(b => new AnalysisAnalysisItem
                               {
                                   AnalysisId = b.AnalysisId,
                                   AnalysisItem = b.AnalysisItem == null ? null : new AnalysisItem
                                   {
                                       Guid = b.AnalysisItem.Guid,
                                       ValueType = b.AnalysisItem.ValueType,
                                       Name = b.AnalysisItem.Name,
                                       AnalysisItemMinMaxValues = b.AnalysisItem.AnalysisItemMinMaxValues,
                                       AnalysisItemValuesRanges = b.AnalysisItem.AnalysisItemValuesRanges,
                                       NormalValues = b.AnalysisItem.NormalValues,
                                       Unit = b.AnalysisItem.Unit == null ? null : new BaseInfo
                                       {
                                           Name = b.AnalysisItem.Unit.Name
                                       }
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
                               Unit = a.AnalysisItem.Unit == null ? null : new BaseInfo
                               {
                                   Name = a.AnalysisItem.Unit.Name
                               }

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
                                       Unit = b.AnalysisItem.Unit == null ? null : new BaseInfo
                                       {
                                           Name = b.AnalysisItem.Unit.Name
                                       }
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
                                               NormalValues = c.AnalysisItem.NormalValues,
                                               Unit = c.AnalysisItem.Unit == null ? null : new BaseInfo
                                               {
                                                   Name = c.AnalysisItem.Unit.Name
                                               }
                                           }
                                       }).ToList()
                                   },
                               }).ToList()
                           }

                       }).ToList(),
                       //PatientReceptionReceiveds = x.Reception.PatientReceptionReceiveds.Select(a => new PatientReceptionReceived
                       //{
                       //    AmountCurrencyId = a.AmountCurrencyId,

                       //}).ToList(),
                   }
               }).SingleOrDefault(x => x.Reception.ClinicSectionId == clinicSectionId && x.Reception.ReceptionNum == invoiceNum);
        }


        public void UpdateAnalysisResultMaster(AnalysisResultMaster updatedAnalysisResultMaster)
        {
            try
            {

                Context.AnalysisResults.RemoveRange(Context.AnalysisResults.Where(x => x.AnalysisResultMasterId == updatedAnalysisResultMaster.Guid));
                Context.AnalysisResults.AddRange(updatedAnalysisResultMaster.AnalysisResults);
                Context.AnalysisResultMasters.Attach(updatedAnalysisResultMaster);
                Context.Entry(updatedAnalysisResultMaster).Property(x => x.Description).IsModified = true;
                Context.Entry(updatedAnalysisResultMaster).Property(x => x.ModifiedDate).IsModified = true;
                Context.Entry(updatedAnalysisResultMaster).Property(x => x.ModifiedUserId).IsModified = true;
                Context.Entry(updatedAnalysisResultMaster).Property(x => x.PrintedNum).IsModified = true;
                Context.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }


        public AnalysisResultMaster GetAnalysisResultMasterForAnalysisResultReport(Guid analysisResultMasterId)
        {


            return Context.AnalysisResultMasters.AsNoTracking()
            .Include(x => x.Reception).ThenInclude(x => x.ReceptionDoctors).ThenInclude(x => x.Doctor).ThenInclude(x => x.User)
            .Include(x => x.Reception).ThenInclude(x => x.Patient).ThenInclude(x => x.User).ThenInclude(x => x.Gender)
            .Select(x => new AnalysisResultMaster
            {
                Guid = x.Guid,
                ModifiedDate = x.ModifiedDate,
                Description = x.Description,
                ReceptionId = x.ReceptionId,
                Reception = new Reception
                {
                    Patient = new Patient
                    {
                        DateOfBirth = x.Reception.Patient.DateOfBirth,
                        User = new User
                        {
                            Name = x.Reception.Patient.User.Name,
                            PhoneNumber = x.Reception.Patient.User.PhoneNumber,
                            Gender = x.Reception.Patient.User.Gender
                        }
                    },
                    PatientId = x.Reception.PatientId,
                    ReceptionDate = x.Reception.ReceptionDate,
                    
                    ReceptionDoctors = x.Reception.ReceptionDoctors.Select(a => new ReceptionDoctor
                    {

                        Doctor = a.Doctor == null ? null : new Doctor
                        {
                            User = new User
                            {
                                Name = a.Doctor.User.Name
                            }
                        }

                    }).ToList(),

                },

            }).SingleOrDefault(x => x.Guid == analysisResultMasterId);

        }

        public void IncreasePrintNumber(Guid analysisResultMasterId)
        {
            try
            {
                AnalysisResultMaster old = Context.AnalysisResultMasters.SingleOrDefault(x => x.Guid == analysisResultMasterId);
                if (old.PrintedNum == null)
                    old.PrintedNum = 1;
                else
                    old.PrintedNum++;
                Context.Entry(old).Property(x => x.PrintedNum).IsModified = true;
                Context.SaveChanges();
            }
            catch (Exception e) { throw e; }

        }


        public IEnumerable<FN_GetPastAnalysisResult_Result> GetPastAnalysisResults(Guid analysisResultMasterId, Guid patientId)
        {
            try
            {
                return Context.FN_GetPastAnalysisResult(patientId, analysisResultMasterId).ToList();
            }
            catch (Exception e) { throw e; }

        }

        public AnalysisResultMaster GetWithPatientAndItem(Guid analysisResultMasterId)
        {
            return _context.AnalysisResultMasters.AsNoTracking()
                .Include(p => p.Reception)
                .Include(p => p.AnalysisResults).ThenInclude(p => p.AnalysisItem)
                .Where(p => p.Guid == analysisResultMasterId)
                .Select(p => new AnalysisResultMaster
                {
                    ReceptionId = p.ReceptionId,
                    InvoiceDate = p.InvoiceDate,
                    Reception = new Reception
                    {
                        PatientId = p.Reception.PatientId,
                    },
                    AnalysisResults = (p.AnalysisResults == null ? null : (ICollection<AnalysisResult>)p.AnalysisResults.Where(x => x.ShowChart != null && x.ShowChart.Value).Select(s => new AnalysisResult
                    {
                        AnalysisItemId = s.AnalysisItemId
                    }))
                })
                .SingleOrDefault();
        }

        public void UpdateAnalysisResultMasterForServerNumByReceptionId(Guid receptionId, int serverNum,DateTime? date)
        {
            try
            {
                if(_context.AnalysisResultMasters.Local.FirstOrDefault(a => a.ReceptionId == receptionId) != null)
                {
                    AnalysisResultMaster arm = _context.AnalysisResultMasters.Local.FirstOrDefault(a => a.ReceptionId == receptionId);
                    arm.ServerNumber = serverNum;
                    arm.UploadDate = date;
                    _context.AnalysisResultMasters.Update(arm);
                    _context.SaveChanges();
                }
                else
                {
                    AnalysisResultMaster arm = _context.AnalysisResultMasters.AsNoTracking().FirstOrDefault(a => a.ReceptionId == receptionId);
                    arm.ServerNumber = serverNum;
                    arm.UploadDate = date;
                    _context.AnalysisResultMasters.Update(arm);
                    _context.SaveChanges();
                }
                
            }
            catch(Exception e) { throw e; }

        }

        public int GetReceptionServerNumber(Guid patientReceptionId)
        {
            return _context.AnalysisResultMasters.FirstOrDefault(a => a.ReceptionId == patientReceptionId).ServerNumber??0;
        }
    }
}
