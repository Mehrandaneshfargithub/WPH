using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Cash;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.HumanResource;
using WPH.Models.HumanResourceSalary;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class HumanResourceSalaryMvcMockingService : IHumanResourceSalaryMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;
        private IEnumerable<HumanResourceSalary> humanResourceSalaries3;

        public HumanResourceSalaryMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
        }



        public IEnumerable<HumanResourceSalaryViewModel> GetAllHumanSalaryByParameter(List<Guid> sections, Guid guid, int periodId, DateTime dateFrom, DateTime dateTo)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                dateFrom = DateTime.Now;
                dateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref dateFrom, ref dateTo, periodId);
            }

            if (guid == Guid.Empty)
            {
                humanResourceSalaries3 = _unitOfWork.HumanResourceSalaries.GetAllHumanSalaryWithDate(sections, dateFrom, dateTo);
            }
            else
            {
                humanResourceSalaries3 = _unitOfWork.HumanResourceSalaries.GetAllHumanSalaryWithDate(sections, dateFrom, dateTo, p => p.HumanResourceId == guid);
            }

            List<HumanResourceSalaryViewModel> humanResourceSalaries = ConvertModelsListsName(humanResourceSalaries3);
            Indexing<HumanResourceSalaryViewModel> indexing = new Indexing<HumanResourceSalaryViewModel>();
            return indexing.AddIndexing(humanResourceSalaries);
        }

        public IEnumerable<HumanResourceSalaryViewModel> GetAllTreatmentStaffWage(List<Guid> sections, DoctorWageViewModel viewModel)
        {
            var surgeryId = _unitOfWork.Surgeries.GetSimpleSurgeryByReceptionId(viewModel.ReceptionId).Guid;
            int? cadreTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(viewModel.CadreType, "CadreType");

            IEnumerable<HumanResourceSalary> humanResourceSalaries = _unitOfWork.HumanResourceSalaries.GetAllTreatmentStaffWage(sections, surgeryId, cadreTypeId);

            List<HumanResourceSalaryViewModel> humanResourceSalariesVM = ConvertModelsListsName(humanResourceSalaries);
            humanResourceSalariesVM.RemoveAll(s => s.HumanResource.Guid == viewModel.DoctorGuid || s.HumanResource.Guid == viewModel.AnesthesiologistGuid
                            || s.HumanResource.Guid == viewModel.PediatricianGuid || s.HumanResource.Guid == viewModel.ResidentGuid);

            Indexing<HumanResourceSalaryViewModel> indexing = new Indexing<HumanResourceSalaryViewModel>();
            return indexing.AddIndexing(humanResourceSalariesVM);
        }



        public HumanResourceSalaryViewModel GetHumanSalaryByHumanSalaryId(Guid humanSalaryid)
        {
            HumanResourceSalary humanSalary = _unitOfWork.HumanResourceSalaries.GetHumanSalaryByHumanSalaryId(humanSalaryid);
            HumanResourceSalaryViewModel humanResourceSalaryViews = ConvertModelReverse(humanSalary);
            return humanResourceSalaryViews;
        }


        public string AddNewHumanSalary(HumanResourceSalaryViewModel viewModel)
        {
            if (viewModel.SalaryTypeHolderId == null || viewModel.BeginDate == null || viewModel.HumanResourceId == null || viewModel.HumanResourceId == Guid.Empty)
                return "DataNotValid";

            viewModel.CreateDate = DateTime.Now;

            if (viewModel.SalaryTypeHolderId.Value == 1)
            {
                if (viewModel.EndDate == null || viewModel.WorkTime.GetValueOrDefault(0) < 0)
                    return "DataNotValid";

                var humanResource = _unitOfWork.HumanResources.Get(viewModel.HumanResourceId.Value);

                if (!(humanResource.FixSalary.GetValueOrDefault(0) > 0) || !(humanResource.FixSalary.GetValueOrDefault(0) > 0))
                    return "DataNotValid";

                viewModel.SalaryTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Salary", "SalaryType");
                viewModel.Salary = humanResource.FixSalary;
                viewModel.ExtraSalary = humanResource.ExtraSalaryPh;
            }
            else
            {
                if (viewModel.Salary.GetValueOrDefault(0) < 0)
                    return "DataNotValid";

                viewModel.WorkTime = 1;
                viewModel.ExtraSalary = 0;
                viewModel.SalaryTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Reward", "SalaryType");
                viewModel.EndDate = viewModel.BeginDate;
            }
            viewModel.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");

            HumanResourceSalary HumanSalaryDto = ConvertModel(viewModel);
            _unitOfWork.HumanResourceSalaries.Add(HumanSalaryDto);
            _unitOfWork.Complete();
            return "";
        }



        public string AddHumanWage(HumanResourceSalaryViewModel viewModel, string cadreType)
        {
            if (viewModel.Salary.GetValueOrDefault(0) < 0 || viewModel.HumanResourceId == null || viewModel.HumanResourceId == Guid.Empty || viewModel.SurgeryId == null || viewModel.SurgeryId == Guid.Empty)
                return "DataNotValid";

            int? cadreTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(cadreType, "CadreType");
            if (cadreType == null)
                return "DataNotValid";

            var salaryTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Wage", "SalaryType");
            var surgeryId = _unitOfWork.Surgeries.GetSimpleSurgeryByReceptionId(viewModel.SurgeryId.Value).Guid;
            var repeat = _unitOfWork.HumanResourceSalaries.Find(p => p.HumanResourceId == viewModel.HumanResourceId && p.SurgeryId == surgeryId && p.SalaryTypeId == salaryTypeId).Any();
            if (repeat)
                return "RepeatedValue";

            viewModel.EndDate = viewModel.BeginDate = viewModel.CreateDate = DateTime.Now;
            viewModel.WorkTime = 1;
            viewModel.ExtraSalary = 0;
            viewModel.SalaryTypeId = salaryTypeId;
            viewModel.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
            viewModel.SurgeryId = surgeryId;

            HumanResourceSalary HumanSalaryDto = ConvertModel(viewModel);
            HumanSalaryDto.CadreTypeId = cadreTypeId;

            _unitOfWork.HumanResourceSalaries.Add(HumanSalaryDto);
            _unitOfWork.Complete();
            return "";
        }




        public OperationStatus RemoveHumanSalary(Guid HumanSalaryId)
        {
            try
            {
                HumanResourceSalary humanSalary = _unitOfWork.HumanResourceSalaries.Get(HumanSalaryId);
                _unitOfWork.HumanResourceSalaries.Remove(humanSalary);
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

        public string UpdateHumanSalary(HumanResourceSalaryViewModel viewModel)
        {
            if (viewModel.SalaryTypeHolderId == null || viewModel.BeginDate == null || viewModel.HumanResourceId == null || viewModel.HumanResourceId == Guid.Empty)
                return "DataNotValid";

            var salary = _unitOfWork.HumanResourceSalaries.Get(viewModel.Guid);
            if (salary.SalaryTypeId == _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Wage", "SalaryType"))
                return Guid.Empty.ToString();

            salary.ModifiedDate = DateTime.Now;
            salary.ModifiedUserId = viewModel.ModifiedUserId;

            if (salary.SalaryTypeId == _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Salary", "SalaryType"))
            {
                if (viewModel.EndDate == null || viewModel.WorkTime.GetValueOrDefault(0) < 0)
                    return "DataNotValid";

                var _HumanResource = _unitOfWork.HumanResources.Get(salary.HumanResourceId.Value);
                decimal? rem;
                if (viewModel.WorkTime < _HumanResource.MinWorkTime)
                {
                    rem = (salary.Salary / _HumanResource.MinWorkTime) * viewModel.WorkTime;
                }
                else
                {
                    var extra = viewModel.WorkTime - _HumanResource.MinWorkTime;
                    rem = salary.Salary + (extra * salary.ExtraSalary);
                }
                rem -= (_unitOfWork.HumanResourceSalaryPayments.GetSalaryPaymentByHumanSalaryId(salary.Guid));
                if (rem < 0)
                    return "ERROR_Money";

                salary.EndDate = viewModel.EndDate;
                salary.BeginDate = viewModel.BeginDate;
                salary.WorkTime = viewModel.WorkTime;

                if (rem == 0)
                    salary.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");
                else
                    salary.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
            }
            else
            {
                decimal? rem = viewModel.Salary.GetValueOrDefault(0) - (_unitOfWork.HumanResourceSalaryPayments.GetSalaryPaymentByHumanSalaryId(salary.Guid));
                if (rem < 0)
                    return "ERROR_Money";

                salary.WorkTime = 1;
                salary.ExtraSalary = 0;
                salary.Salary = viewModel.Salary;
                salary.EndDate = salary.BeginDate = viewModel.BeginDate;

                if (rem == 0)
                    salary.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");
                else
                    salary.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
            }

            _unitOfWork.HumanResourceSalaries.UpdateState(salary);
            _unitOfWork.Complete();
            return "";
        }


        public string UpdateTreatmentStaff(HumanResourceSalaryViewModel viewModel)
        {
            if (viewModel.Guid == Guid.Empty || !(viewModel.Amount.GetValueOrDefault(0) > 0))
                return "DataNotValid";

            var humanResourceSallary = _unitOfWork.HumanResourceSalaries.Get(viewModel.Guid);
            int? unpaidId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
            int? paidId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");


            decimal? payed = humanResourceSallary.Salary - _unitOfWork.HumanResourceSalaries.GetHumanSalaryRem(humanResourceSallary.Guid);
            decimal? rem = viewModel.Salary.GetValueOrDefault(0) - payed;
            if (rem < 0)
                return "ERROR_Money";

            humanResourceSallary.WorkTime = 1;
            humanResourceSallary.ExtraSalary = 0;
            humanResourceSallary.Salary = viewModel.Salary;
            humanResourceSallary.ModifiedDate = DateTime.Now;
            humanResourceSallary.ModifiedUserId = viewModel.ModifiedUserId;

            if (rem == 0)
                humanResourceSallary.PaymentStatusId = paidId;
            else
                humanResourceSallary.PaymentStatusId = unpaidId;


            _unitOfWork.HumanResourceSalaries.UpdateState(humanResourceSallary);

            _unitOfWork.Complete();
            return "";
        }

        public decimal? GetHumanSalaryRem(Guid humanSalaryId)
        {
            try
            {
                return _unitOfWork.HumanResourceSalaries.GetHumanSalaryRem(humanSalaryId);
            }
            catch
            {
                return 0;
            }
        }


        public SalaryReportResultViewModel SalaryReport(SalaryReportViewModel viewModel)
        {
            SalaryReportResultViewModel report = new();
            report.AllSalary = new List<HumanResourceSalaryReportViewModel>();

            List<HumanResourceSalary> salaryDto = _unitOfWork.HumanResourceSalaries.GetDetailSalaryReport(viewModel.AllClinicSectionGuids, viewModel.FromDate, viewModel.ToDate, p =>
                                                                (viewModel.HumanResource == null || p.HumanResourceId == viewModel.HumanResource) &&
                                                                (viewModel.PaymentStatus == null || p.PaymentStatusId == viewModel.PaymentStatus) &&
                                                                (viewModel.SalaryType == null || p.SalaryTypeId == viewModel.SalaryType)
                                                                ).ToList();

            if (viewModel.OrderBy == 2)
            {
                salaryDto = salaryDto.OrderByDescending(p => p.CreateDate).ToList();
            }
            else
            {
                salaryDto = salaryDto.OrderByDescending(p => p.HumanResource.Gu.Name).ToList();
            }

            CultureInfo cultures = new CultureInfo("en-US");
            if (!viewModel.Detail)
            {

                report.AllSalary = salaryDto.Select(p => new HumanResourceSalaryReportViewModel
                {
                    HumanName = p.HumanResource.Gu.Name,
                    Date = p.CreateDate.Value.ToString("dd/MM/yyyy", cultures),
                    SalaryType = p.SalaryType.Name,
                    Section = p.HumanResource.Gu.ClinicSection.Name,
                    Temp_Recived = p.HumanResourceSalaryPayments.Sum(a=> a.Amount.GetValueOrDefault(0)),
                    MinWorkTime = 0,
                    WorkTime = 0,
                    Salary = 0,
                    ExtraSalary = 0,
                    ServiceName = p.Surgery?.Reception?.ReceptionServices?.FirstOrDefault(a=>a.Service.DoctorWage != null)?.Service.Name,
                    
                }).ToList();

                report.AllHuman = report.AllSalary.GroupBy(p => p.HumanName).Select(p => new HumanResourceSalaryReportViewModel
                {
                    HumanName = p.Key,
                    SalaryType = "",
                    Temp_Recived = p.Sum(s => s.Temp_Recived.GetValueOrDefault(0)),
                    MinWorkTime = 0,
                    WorkTime = 0,
                    Salary = 0,
                    ExtraSalary = 0
                }).ToList();

                //foreach (var item in salaryDto)
                //{
                //    foreach (var payment in item.HumanResourceSalaryPayments)
                //    {
                //        var reportItem = new HumanResourceSalaryReportViewModel
                //        {
                //            HumanName = item.HumanResource.Gu.Name,
                //            Date = payment.CreatedDate.Value.ToString("dd/MM/yyyy", cultures),
                //            SalaryType = item.SalaryType.Name,
                //            Section = item.HumanResource.Gu.ClinicSection.Name,
                //            Temp_Recived = payment.Amount,
                //            MinWorkTime = 0,
                //            WorkTime = 0,
                //            Salary = 0,
                //            ExtraSalary = 0
                //        };

                //        report.AllSalary.Add(reportItem);
                //    }
                //}
            }
            else
            {
                report.AllSalary = salaryDto.GroupBy(g => g.Guid).Select(p => new HumanResourceSalaryReportViewModel
                {
                    HumanName = p.FirstOrDefault()?.HumanResource?.Gu?.Name ?? "",
                    Date = p.LastOrDefault()?.CreateDate.Value.ToString("dd/MM/yyyy", cultures) ?? "",
                    SalaryType = p.FirstOrDefault()?.SalaryType?.Name ?? "",
                    PaymentStatus = p.FirstOrDefault()?.PaymentStatus?.Name ?? "",
                    Section = p.FirstOrDefault()?.HumanResource?.Gu?.ClinicSection?.Name ?? "",
                    MinWorkTime = p.FirstOrDefault()?.HumanResource?.MinWorkTime ?? 0,
                    WorkTime = p.FirstOrDefault()?.WorkTime ?? 0,
                    Salary = p.FirstOrDefault()?.Salary ?? 0,
                    ExtraSalary = p.FirstOrDefault()?.ExtraSalary ?? 0,
                    ServiceName = p.FirstOrDefault()?.Surgery?.Reception?.ReceptionServices?.FirstOrDefault(a => a.Service.DoctorWage != null)?.Service.Name,
                    Temp_Recived = p.Sum(s => s.HumanResourceSalaryPayments.Sum(a=>a.Amount.GetValueOrDefault(0))),
                    PatientName = p.FirstOrDefault().Surgery?.Reception?.Patient?.User?.Name,
                    ReceptionDate = p.FirstOrDefault().Surgery?.Reception?.ReceptionDate.GetValueOrDefault().ToString("dd/MM/yyyy", cultures)
                }).ToList();

                report.AllHuman = report.AllSalary.GroupBy(p => p.HumanName).Select(p => new HumanResourceSalaryReportViewModel
                {
                    HumanName = p.Key,
                    SalaryType = "",
                    MinWorkTime = 0,
                    WorkTime = 0,
                    Salary = p.Sum(s => s.Temp_Amount.GetValueOrDefault(0)),
                    ExtraSalary = 0,
                    Temp_Recived = p.Sum(s => s.Temp_Recived.GetValueOrDefault(0))
                }).ToList();

                //report.AllSalary.AddRange(ConvertModelsListsReport(salaryDto));
            }

            report.AllSalarySection = new List<HumanResourceSalaryReportViewModel>();
            report.AllSalarySection.AddRange(report.AllSalary.GroupBy(g => g.Section).Select(s => new HumanResourceSalaryReportViewModel
            {
                Section = s.Key,
                Temp_Recived = s.Sum(d => d.Temp_Recived),
                MinWorkTime = 0,
                WorkTime = 0,
                Salary = 0,
                ExtraSalary = 0
            }));

            report.AllPay = report.AllSalary.Sum(s => s.Temp_Recived.GetValueOrDefault(0)).ToString("N0");

            return report;
        }

        public List<HumanResourceSalaryViewModel> ConvertModelsListsName(IEnumerable<HumanResourceSalary> humanResourceSalaries)
        {
            List<HumanResourceSalaryViewModel> humanResourceSalaryViewModels = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<HumanResourceSalary, HumanResourceSalaryViewModel>()
                .ForMember(a => a.InvoiceNum, b => b.MapFrom(c => c.Surgery.Reception.ReceptionNum))
                .ForMember(a => a.HumanResourceName, b => b.MapFrom(c => c.HumanResource.Gu.Name))
                .ForMember(a => a.PaymentStatus, b => b.MapFrom(c => c.PaymentStatus.Name))
                .ForMember(a => a.SalaryTypeHolderId, b => b.MapFrom(c => c.SalaryType.Priority ?? 1))
                .ForMember(a => a.RecivedMoney, b => b.MapFrom(c => c.HumanResourceSalaryPayments.Sum(s => s.Amount.GetValueOrDefault(0))))
                .ForMember(a => a.SalaryType, b => b.MapFrom(c => c.SalaryType.Name ?? ""));
                cfg.CreateMap<HumanResource, HumanResourceViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            humanResourceSalaryViewModels = mapper.Map<IEnumerable<HumanResourceSalary>, List<HumanResourceSalaryViewModel>>(humanResourceSalaries);
            return humanResourceSalaryViewModels;
        }

        public static List<HumanResourceSalaryViewModel> ConvertModelsLists(IEnumerable<HumanResourceSalary> humanResourceSalaries)
        {
            List<HumanResourceSalaryViewModel> humanResourceSalaryViewModels = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HumanResourceSalary, HumanResourceSalaryViewModel>()
                .ForMember(a => a.HumanResourceName, b => b.MapFrom(c => c.HumanResource.Gu.Name));
                cfg.CreateMap<HumanResource, HumanResourceViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            humanResourceSalaryViewModels = mapper.Map<IEnumerable<HumanResourceSalary>, List<HumanResourceSalaryViewModel>>(humanResourceSalaries);
            return humanResourceSalaryViewModels;
        }

        public static HumanResourceSalary ConvertModel(HumanResourceSalaryViewModel meds)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HumanResourceSalaryViewModel, HumanResourceSalary>()
                .ForMember(a => a.SalaryType, b => b.Ignore());
                /*cfg.CreateMap<HumanResourceViewModel, HumanResource>();
                cfg.CreateMap<UserInformationViewModel, User>();
                cfg.CreateMap<BaseInfoGeneralViewModel, BaseInfoGeneral>();*/
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<HumanResourceSalaryViewModel, HumanResourceSalary>(meds);
        }
        public static HumanResourceSalaryViewModel ConvertModelReverse(HumanResourceSalary meds)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HumanResourceSalary, HumanResourceSalaryViewModel>()
                .ForMember(a => a.HumanResourceName, b => b.MapFrom(c => c.HumanResource.Gu.Name))
                .ForMember(a => a.SalaryTypeHolderId, b => b.MapFrom(c => c.SalaryType.Priority))
                .ForMember(a => a.SalaryType, b => b.MapFrom(c => c.SalaryType.Name));
                cfg.CreateMap<HumanResource, HumanResourceViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<HumanResourceSalary, HumanResourceSalaryViewModel>(meds);
        }

    }
}
