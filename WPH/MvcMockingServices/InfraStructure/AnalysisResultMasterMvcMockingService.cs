using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.AnalysisResultMaster;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.Analysis_AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItemMinMaxValue;
using WPH.Models.CustomDataModels.AnalysisItemValuesRange;
using WPH.Models.CustomDataModels.AnalysisResult;
using WPH.Models.CustomDataModels.AnalysisResultMaster;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.GroupAnalysis_Analysis;
using WPH.Models.CustomDataModels.GroupAnalysisItem;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.PatientReceptionAnalysis;
using WPH.Models.CustomDataModels.PatientReceptionReceived;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;
using WPH.Models.ReceptionDoctor;
using WPH.MvcMockingServices.Interface;
using WPH.Models.AnalysisItem;

namespace WPH.MvcMockingServices.InfraStructure
{


    public class AnalysisResultMasterMvcMockingService : IAnalysisResultMasterMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _diunit;

        public AnalysisResultMasterMvcMockingService(IUnitOfWork unitOfWork, IDIUnit dunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _diunit = dunit;
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/AnalysisResultMaster/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }


        public AnalysisResultMasterViewModel GetAnalysisResultMasterByInvoiceNum(Guid clinicSectionId, string invoiceNum)
        {
            try
            {
                AnalysisResultMaster AnalysisResultMasterDtos = _unitOfWork.AnalysisResultMaster.GetAnalysisResultMasterByInvoiceNum(clinicSectionId, invoiceNum);
                return convertModelFroAnalysisResult(AnalysisResultMasterDtos);

            }
            catch (Exception) { return null; }
        }

        public void UpdateAnalysisResultMaster(AnalysisResultMasterViewModel analysisResultMaster)
        {
            AnalysisResultMaster masterDto = convertModelsFromViewModelToDto(analysisResultMaster);
            _unitOfWork.AnalysisResultMaster.UpdateAnalysisResultMaster(masterDto);
        }

        public void IncreasePrintNumber(Guid analysisResultMasterId)
        {
            _unitOfWork.AnalysisResultMaster.IncreasePrintNumber(analysisResultMasterId);
        }

        public IEnumerable<AnalysisResultMasterViewModel> GetAllAnalysisResultMaster(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                List<AnalysisResultMaster> AnalysisResultMasterDtos = new List<AnalysisResultMaster>();

                if (periodId != (int)Periods.FromDateToDate)
                {
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                }

                AnalysisResultMasterDtos = _unitOfWork.AnalysisResultMaster.GetAllAnalysisResultMaster(clinicSectionId, DateFrom, DateTo).ToList();

                List<AnalysisResultMasterViewModel> AnalysisResultMaster = convertModelsLists(AnalysisResultMasterDtos).OrderByDescending(x => x.InvoiceDate).ToList();
                Indexing<AnalysisResultMasterViewModel> indexing = new Indexing<AnalysisResultMasterViewModel>();
                return indexing.AddIndexing(AnalysisResultMaster);
            }
            catch (Exception) { return null; }
        }


        public IEnumerable<AnalysisResultMasterGridViewModel> GetAllAnalysisResultMasterByUserId(Guid userId, int periodId, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                IEnumerable<AnalysisResultMasterGrid> AnalysisResultMasterDtos;

                if (periodId != (int)Periods.FromDateToDate)
                {
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                }

                AnalysisResultMasterDtos = _unitOfWork.AnalysisResultMaster.GetAllAnalysisResultMasterByUserId(userId, DateFrom, DateTo);

                List<AnalysisResultMasterGridViewModel> AnalysisResultMaster = convertModelsListsForGrid(AnalysisResultMasterDtos);
                Indexing<AnalysisResultMasterGridViewModel> indexing = new Indexing<AnalysisResultMasterGridViewModel>();
                return indexing.AddIndexing(AnalysisResultMaster);
            }
            catch (Exception e) { return null; }
        }



        public IEnumerable<AnalysisResultMasterGridViewModel> GetAnalysisResultByPatientId(Guid patientId)
        {

            IEnumerable<AnalysisResultMasterGrid> AnalysisResultMasterDtos = _unitOfWork.AnalysisResultMaster.GetAnalysisResultByPatientId(patientId);
            List<AnalysisResultMasterGridViewModel> AnalysisResultMaster = convertModelsListsForGrid(AnalysisResultMasterDtos);
            Indexing<AnalysisResultMasterGridViewModel> indexing = new Indexing<AnalysisResultMasterGridViewModel>();
            return indexing.AddIndexing(AnalysisResultMaster);
        }

        public IEnumerable<AnalysisResultMasterGridViewModel> GetAllPatientAnalysisResult(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public static List<AnalysisResultMasterGridViewModel> convertModelsListsForGrid(IEnumerable<AnalysisResultMasterGrid> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisResultMasterGrid, AnalysisResultMasterGridViewModel>()
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()))
                ;
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<AnalysisResultMasterGrid>, List<AnalysisResultMasterGridViewModel>>(ress);

        }


        public OperationStatus RemoveAnalysisResultMaster(Guid AnalysisResultMasterId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity;
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong;
                }
            }
        }

        public AnalysisResultMasterViewModel GetAnalysisResultMasterByIdForAnalysisResult(Guid AnalysisResultMasterId)
        {
            try
            {
                AnalysisResultMaster AnalysisResultMasterDtos = _unitOfWork.AnalysisResultMaster.GetAnalysisResultMasterByIdForAnalysisResult(AnalysisResultMasterId);
                return convertModelFroAnalysisResult(AnalysisResultMasterDtos);

            }
            catch (Exception) { return null; }
        }



        public AnalysisResultMasterViewModel GetAnalysisResultMasterForAnalysisResultReport(Guid AnalysisResultMasterId)
        {
            try
            {
                AnalysisResultMaster AnalysisResultMasterDtos = _unitOfWork.AnalysisResultMaster.GetAnalysisResultMasterForAnalysisResultReport(AnalysisResultMasterId);
                List<AnalysisResult> AllResult = _unitOfWork.AnalysisResult.GetAnalysisResultForAnalysisResultReport(AnalysisResultMasterId);
                //foreach (var ana in AnalysisResultMasterDtos.AnalysisResults)
                //{
                //    if (ana.Analysis != null)
                //    {
                //        ana.Analysis.AnalysisAnalysisItems.OrderBy(a=>a.Priority);
                //    }
                //}

                foreach (var ana in AllResult)
                {
                    if (ana.Analysis != null)
                    {
                        ana.AnalysisItem.Priority = ana.Analysis.AnalysisAnalysisItems.SingleOrDefault(a => a.AnalysisItemId == ana.AnalysisItemId).Priority;
                    }
                }
                AllResult.OrderBy(a => a.AnalysisItem.Priority);

                List<AnalysisResultViewModel> all = new List<AnalysisResultViewModel>();
                AnalysisResultMasterViewModel analy = new AnalysisResultMasterViewModel();
                all = convertModelResult(AllResult);
                analy = convertModeltwo(AnalysisResultMasterDtos);
                analy.AnalysisResults.AddRange(all);
                return analy;

            }
            catch (Exception) { return null; }
        }

        public List<AnalysisItemInChartViewModel> GetAnalysisItemForChart(Guid analysisResultMasterId)
        {
            var master = _unitOfWork.AnalysisResultMaster.GetWithPatientAndItem(analysisResultMasterId);

            if (master == null || master.AnalysisResults == null || !master.AnalysisResults.Any())
                return null;

            var analysisGuid = master.AnalysisResults.Select(p => p.AnalysisItemId).ToList();
            var items = _unitOfWork.AnalysisResult.GetAnalysisResultHistoryForChart(master.Reception.PatientId.Value, master.InvoiceDate.Value, analysisGuid);

            var result = items.Where(p => float.TryParse(p.Value, out float rr)).GroupBy(g => g.AnalysisItem.Name).Select(p => new AnalysisItemInChartViewModel
            {
                AnalysisName = p.Key,
                History = p.Select(x => new AnalysisItemInChartViewModel
                {
                    AnalysisName = x.AnalysisItem.Name,
                    AnalysisArgument = x.AnalysisResultMaster.InvoiceDate.Value.ToString("yyyy_MM_dd"),
                    AnalysisValue = float.Parse(x.Value)
                })
            }).ToList();

            return result;
        }

        public AnalysisResultMasterViewModel GetAnalysisResultMasterByReceptionId(Guid receptionId)
        {
            var re = _unitOfWork.AnalysisResultMaster.GetSingle(a=>a.ReceptionId == receptionId);
            return Common.ConvertModels<AnalysisResultMasterViewModel, AnalysisResultMaster>.convertModels(re);
        }

        public void UpdateAnalysisResultMasterForServerNumByReceptionId(Guid receptionId, int serverNum, DateTime? date)
        {
            _unitOfWork.AnalysisResultMaster.UpdateAnalysisResultMasterForServerNumByReceptionId(receptionId, serverNum,date);
        }

        public List<AnalysisResultViewModel> convertModelResult(IEnumerable<AnalysisResult> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Analysis, AnalysisViewModel>()

                ;
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore())
                ;
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();


                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();

                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();

            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<AnalysisResult>, List<AnalysisResultViewModel>>(ress);

        }




        public IEnumerable<PastValuesViewModel> GetPastAnalysisResults(Guid analysisResultMasterId, Guid patientId)
        {
            try
            {
                try
                {
                    IEnumerable<FN_GetPastAnalysisResult_Result> pastResults = _unitOfWork.AnalysisResultMaster.GetPastAnalysisResults(analysisResultMasterId, patientId);
                    List<PastValuesViewModel> all = new List<PastValuesViewModel>();
                    IEnumerable<string> names = pastResults.Select(x => x.Name).Distinct();
                    foreach (var name in names)
                    {
                        IEnumerable<FN_GetPastAnalysisResult_Result> sp = pastResults.Where(x => x.Name == name).OrderByDescending(x => x.Date);
                        PastValuesViewModel past = new PastValuesViewModel() { Name = name };
                        if (sp.ElementAt(0) != null)
                        {
                            past.Date = sp.ElementAt(0).Date.Value.ToShortDateString();
                            past.Value = sp.ElementAt(0).Value;
                        }
                        if (sp.ElementAt(1) != null)
                        {
                            past.Date2 = sp.ElementAt(1).Date.Value.ToShortDateString();
                            past.Value2 = sp.ElementAt(1).Value;
                        }
                        if (sp.ElementAt(2) != null)
                        {
                            past.Date3 = sp.ElementAt(2).Date.Value.ToShortDateString();
                            past.Value3 = sp.ElementAt(2).Value;
                        }
                        all.Add(past);
                    }



                    //AnalysisResultMasterDto analysisResultMasterDto = convertAnalysisResultMasterToDtoModelsLists(pastResults);
                    return all;
                }
                catch (Exception) { return null; }


            }
            catch (Exception) { return null; }
        }

        public AnalysisResultMasterViewModel GetAnalysisResultMaster(Guid analysisResultMasterId)
        {
           var re = _unitOfWork.AnalysisResultMaster.Get(analysisResultMasterId);
            return Common.ConvertModels<AnalysisResultMasterViewModel, AnalysisResultMaster>.convertModels(re);
        }

        public int GetReceptionServerNumber(Guid patientReceptionId)
        {
            return _unitOfWork.AnalysisResultMaster.GetReceptionServerNumber(patientReceptionId);
        }


        public static List<AnalysisResultMasterViewModel> convertModelsListsCombinePatientNameAndPhone(IEnumerable<AnalysisResultMaster> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisResultMaster, AnalysisResultMasterViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.PhoneNumberAndName, b => b.MapFrom(c => c.User.Name + "|" + c.User.PhoneNumber))
                ;

                cfg.CreateMap<Doctor, DoctorViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<AnalysisResultMaster>, List<AnalysisResultMasterViewModel>>(ress);

        }



        public static List<AnalysisResultMasterViewModel> convertModelsLists(IEnumerable<AnalysisResultMaster> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisResultMaster, AnalysisResultMasterViewModel>()
                .ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                .ForMember(a => a.PatientReception, b => b.MapFrom(c => c.Reception))
                .ForMember(a => a.ClinicSectionTypeName, b => b.MapFrom(c => c.Reception.ClinicSection.ClinicSectionType.Name))
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()))
                ;
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.ClinicSection, b => b.Ignore())
                ;
                cfg.CreateMap<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>();
                cfg.CreateMap<Analysis, AnalysisViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore());
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<AnalysisResultMaster>, List<AnalysisResultMasterViewModel>>(ress);

        }


        public AnalysisResultMasterViewModel convertModeltwo(AnalysisResultMaster ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisResultMaster, AnalysisResultMasterViewModel>()
                .ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                .ForMember(a => a.PatientReception, b => b.MapFrom(c => c.Reception))
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()))
                ;
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<Analysis, AnalysisViewModel>()

                ;
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore())
                ;
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();


                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
                cfg.CreateMap<ReceptionDoctor, ReceptionDoctorViewModel>();
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.Doctor, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor))
                ;
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();

            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<AnalysisResultMaster, AnalysisResultMasterViewModel>(ress);

        }



        public AnalysisResultMasterViewModel convertModel(AnalysisResultMaster ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisResultMaster, AnalysisResultMasterViewModel>()
                .ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                .ForMember(a => a.PatientReception, b => b.MapFrom(c => c.Reception))
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()))
                ;
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<Analysis, AnalysisViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore())
                ;
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();


                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
                cfg.CreateMap<ReceptionDoctor, ReceptionDoctorViewModel>();
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.Doctor, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor))
                ;
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();

            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<AnalysisResultMaster, AnalysisResultMasterViewModel>(ress);

        }



        public AnalysisResultMasterViewModel convertModelFroAnalysisResult(AnalysisResultMaster ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisResultMaster, AnalysisResultMasterViewModel>()
                .ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                .ForMember(a => a.PatientReception, b => b.MapFrom(c => c.Reception))
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()))
                ;
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<Analysis, AnalysisViewModel>()
                .ForMember(a => a.Analysis_AnalysisItem, b => b.MapFrom(c => c.AnalysisAnalysisItems));
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.MapFrom(c => c.AnalysisItemMinMaxValues.FirstOrDefault()))
                ;
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>()
                .ForMember(a => a.GroupAnalysisAnalyses, b => b.MapFrom(c => c.GroupAnalysisAnalyses))
                .ForMember(a => a.GroupAnalysisItems, b => b.MapFrom(c => c.GroupAnalysisItems));
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();
                cfg.CreateMap<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>();
                cfg.CreateMap<Reception, ReceptionViewModel>();
                cfg.CreateMap<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>();

                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<ClinicSection, ClinicSectionViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<AnalysisResultMaster, AnalysisResultMasterViewModel>(ress);

        }

        public bool CanDelete(DateTime date)
        {
            if (date.Date == DateTime.Now.Date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AnalysisResultMaster convertModelsFromViewModelToDto(AnalysisResultMasterViewModel ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisResultMasterViewModel, AnalysisResultMaster>();
                cfg.CreateMap<PatientViewModel, Patient>();
                cfg.CreateMap<UserInformationViewModel, User>();
                cfg.CreateMap<DoctorViewModel, Doctor>();
                cfg.CreateMap<AnalysisResultViewModel, AnalysisResult>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<AnalysisResultMasterViewModel, AnalysisResultMaster>(ress);

        }

        
    }
}
