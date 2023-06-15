using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.HospitalPatients;
using WPH.Models.Reception;
using WPH.Models.ReceptionClinicSection;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class ReceptionClinicSectionMvcMockingService : IReceptionClinicSectionMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReceptionClinicSectionMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveReceptionClinicSection(Guid ReceptionClinicSectionid)
        {
            try
            {
                ReceptionClinicSection Hos = _unitOfWork.ReceptionClinicSections.Get(ReceptionClinicSectionid);
                _unitOfWork.ReceptionClinicSections.Remove(Hos);
                _unitOfWork.Complete();
                return OperationStatus.SUCCESSFUL;
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

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/ReceptionClinicSection/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }
        public Guid AddNewReceptionClinicSection(ReceptionClinicSectionViewModel ReceptionClinicSection)
        {
            try
            {
                ReceptionClinicSection ReceptionClinicSection1 = Common.ConvertModels<ReceptionClinicSection, ReceptionClinicSectionViewModel>.convertModels(ReceptionClinicSection);
                ReceptionClinicSection1.Guid = Guid.NewGuid();
                _unitOfWork.ReceptionClinicSections.Add(ReceptionClinicSection1);
                _unitOfWork.Complete();
                return ReceptionClinicSection1.Guid;
            }
            catch (Exception ex) { throw ex; }
        }



        public Guid UpdateReceptionClinicSection(ReceptionClinicSectionViewModel hosp)
        {
            try
            {
                ReceptionClinicSection ReceptionClinicSection2 = Common.ConvertModels<ReceptionClinicSection, ReceptionClinicSectionViewModel>.convertModels(hosp);
                _unitOfWork.ReceptionClinicSections.UpdateState(ReceptionClinicSection2);
                _unitOfWork.Complete();
                return ReceptionClinicSection2.Guid;
            }
            catch (Exception ex) { throw ex; }

        }


        public IEnumerable<ReceptionClinicSectionViewModel> GetAllReceptionClinicSections()
        {
            IEnumerable<ReceptionClinicSection> hosp = _unitOfWork.ReceptionClinicSections.GetAllReceptionClinicSection();
            List<ReceptionClinicSectionViewModel> hospconvert = convertModelsLists(hosp);
            Indexing<ReceptionClinicSectionViewModel> indexing = new Indexing<ReceptionClinicSectionViewModel>();
            return indexing.AddIndexing(hospconvert);
        }


        public ReceptionClinicSectionViewModel GetReceptionClinicSectionByDestinationReceptionId(Guid DestinationReceptionId)
        {
            ReceptionClinicSection hosp = _unitOfWork.ReceptionClinicSections.GetReceptionClinicSectionByDestinationReceptionId(DestinationReceptionId);
            return Common.ConvertModels<ReceptionClinicSectionViewModel, ReceptionClinicSection>.convertModels(hosp);

        }


        public IEnumerable<ReceptionClinicSectionViewModel> GetAllReceptionClinicSectionByReceptionId(Guid ReceptionId)
        {
            IEnumerable<ReceptionClinicSection> hosp = _unitOfWork.ReceptionClinicSections.GetAllReceptionClinicSectionByReceptionId(ReceptionId);
            List<ReceptionClinicSectionViewModel> hospconvert = convertModelsLists(hosp);
            Indexing<ReceptionClinicSectionViewModel> indexing = new Indexing<ReceptionClinicSectionViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public IEnumerable<ReceptionClinicSectionViewModel> GetAllReceptionClinicSectionByClinicSectionId(Guid clinicSectionId, int periodId, DateTime fromDate, DateTime toDate, Guid receptionId, int status)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                fromDate = DateTime.Now;
                toDate = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref fromDate, ref toDate, periodId);
            }
            IEnumerable<ReceptionClinicSection> hosp;

            //if (status == (int)DischargeType.NotDischarge)
            //{
            //    hosp = _unitOfWork.ReceptionClinicSections.GetAllReceptionClinicSectionByClinicSectionId(clinicSectionId, fromDate, toDate, receptionId, p => p.Reception.Discharge == null || !p.Reception.Discharge.Value).ToList();
            //}
            //else if (status == (int)DischargeType.Discharge)
            //{
            //    hosp = _unitOfWork.ReceptionClinicSections.GetAllReceptionClinicSectionByClinicSectionId(clinicSectionId, fromDate, toDate, receptionId, p => p.Reception.Discharge != null && p.Reception.Discharge.Value).ToList();
            //}
            //else
            //{
            //}
            hosp = _unitOfWork.ReceptionClinicSections.GetAllReceptionClinicSectionByClinicSectionId(clinicSectionId, fromDate, toDate, receptionId).ToList();

            List<ReceptionClinicSectionViewModel> hospconvert = convertModelsLists(hosp);
            Indexing<ReceptionClinicSectionViewModel> indexing = new Indexing<ReceptionClinicSectionViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public ShowPatientToAnotherSectionReportResultViewModel GetPatientToAnotherSectionReport(Guid clinicSectionId, DateTime fromDate, DateTime toDate)
        {
            var result = new ShowPatientToAnotherSectionReportResultViewModel();
            IEnumerable<ReceptionClinicSection> hosp;

            hosp = _unitOfWork.ReceptionClinicSections.GetPatientToAnotherSectionReport(fromDate, toDate, clinicSectionId).ToList();

            CultureInfo cultures = new CultureInfo("en-US");
            result.Human = hosp.Select(p => new PatientToAnotherSectionReportViewModel
            {
                PatientName = p.Reception.Patient.User.Name,
                Date = p.DestinationReception.ReceptionDate.Value.ToString("dd/MM/yyyy", cultures),
                Section = p.ClinicSection.Name,
                TempAmount = p.DestinationReception.PatientReceptionAnalyses.Sum(s => s.Amount.GetValueOrDefault(0) - s.Discount.GetValueOrDefault(0)),
                Analysis = string.Join(',', p.DestinationReception.PatientReceptionAnalyses.Select(s => s.AnalysisItem?.Name ?? s.GroupAnalysis?.Name ?? s.Analysis?.Name ?? ""))
            }).ToList();

            result.Section = result.Human.GroupBy(g => g.Section).Select(s => new PatientToAnotherSectionReportViewModel
            {
                Section = s.Key,
                TempAmount = s.Sum(p => p.TempAmount),
                AnalysisCount = s.Count().ToString("N0")
            }).ToList();

            result.Total = result.Section.Sum(p => p.TempAmount.GetValueOrDefault(0)).ToString("N0");

            return result;
        }

        public ReceptionClinicSectionViewModel GetReceptionClinicSection(Guid ReceptionClinicSectionId)
        {
            try
            {
                ReceptionClinicSection ReceptionClinicSectiongu = _unitOfWork.ReceptionClinicSections.Get(ReceptionClinicSectionId);
                return convertModel(ReceptionClinicSectiongu);
            }
            catch { return null; }
        }

        // Begin Convert 
        public ReceptionClinicSectionViewModel convertModel(ReceptionClinicSection Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionClinicSection, ReceptionClinicSectionViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceptionClinicSection, ReceptionClinicSectionViewModel>(Users);
        }
        public List<ReceptionClinicSectionViewModel> convertModelsLists(IEnumerable<ReceptionClinicSection> ReceptionClinicSections)
        {
            List<ReceptionClinicSectionViewModel> ReceptionClinicSectionDtoList = new List<ReceptionClinicSectionViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionClinicSection, ReceptionClinicSectionViewModel>();
                cfg.CreateMap<ClinicSection, ClinicSectionViewModel>();
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.DoctorId, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().DoctorId))
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.User, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            ReceptionClinicSectionDtoList = mapper.Map<IEnumerable<ReceptionClinicSection>, List<ReceptionClinicSectionViewModel>>(ReceptionClinicSections);
            return ReceptionClinicSectionDtoList;
        }



        // End Convert
    }
}
