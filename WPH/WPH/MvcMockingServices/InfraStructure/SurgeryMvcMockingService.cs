using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.FunctionModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;
using WPH.Models.Service;
using WPH.Models.Surgery;
using WPH.Models.SurgeryDoctor;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class SurgeryMvcMockingService : ISurgeryMvcMockingService
    {


        private readonly IUnitOfWork _unitOfWork;
        public SurgeryMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveSurgery(Guid Surgeryid)
        {
            try
            {
                _unitOfWork.Surgeries.RemoveSurgery(Surgeryid);

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
            string controllerName = "/Surgery/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string UpdateSurgery(SurgeryViewModel surgeryViewModel, Guid clinicSectionId)
        {
            var doctors = new[]
            {
                new {Name = surgeryViewModel.SurgeryOneName, Role = "Surgery1" },
                new {Name = surgeryViewModel.SurgeryTwoName, Role = "Surgery2" },
                new {Name = surgeryViewModel.AnesthesiologistName, Role = "Anesthesiologist" },
                new {Name = surgeryViewModel.PediatricianName, Role = "Pediatrician" },
            }.ToList();

            var dup = doctors.Where(p => !string.IsNullOrWhiteSpace(p.Name))?.GroupBy(p => p.Name).Where(g => g.Count() > 1)
                .Select(s => s.Select(x => x.Role)).FirstOrDefault()?.ToArray();

            if (dup != null)
                return $"DuplicateDoctor*{dup[0]}*{dup[1]}";

            surgeryViewModel.SurgeryDoctors = new List<SurgeryDoctorViewModel>();
            surgeryViewModel.ModifiedDate = DateTime.Now;

            var salaryTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Wage", "SalaryType");
            var surgeryDoctors = _unitOfWork.SurgeryDoctors.GetDoctorsAndRoleBySurgeryId(surgeryViewModel.Guid);

            if (!string.IsNullOrWhiteSpace(surgeryViewModel.SurgeryOneName))
            {
                var surgeryOneId = _unitOfWork.Doctor.GetDoctorByName(clinicSectionId, surgeryViewModel.SurgeryOneName)?.Guid;
                var doctorRole = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Surgery1", "DoctorRole");

                //var duplicateDoctor = surgeryDoctors.FirstOrDefault(p => p.DoctorId == surgeryOneId && p.DoctorRoleId != doctorRole);
                //if (duplicateDoctor != null)
                //    return $"DuplicateDoctor*Surgery1*{duplicateDoctor.DoctorRole.Name}";

                var existsDoctor = surgeryDoctors.SingleOrDefault(p => p.DoctorRoleId == doctorRole)?.DoctorId;
                if (existsDoctor != null && existsDoctor != surgeryOneId)
                {
                    var wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == existsDoctor
                                                                    && f.SurgeryId == surgeryViewModel.Guid && f.SalaryTypeId == salaryTypeId);

                    if (wage != null)
                        return "ERROR_HumanResourceSallaryDependency*Surgery1";
                }

                SurgeryDoctorViewModel surgery1;

                if (surgeryOneId != null && surgeryOneId != Guid.Empty)
                {
                    surgery1 = new SurgeryDoctorViewModel()
                    {
                        DoctorId = surgeryOneId,
                        DoctorRoleId = doctorRole,
                        SurgeryId = surgeryViewModel.Guid,
                    };
                }
                else
                {
                    surgery1 = new SurgeryDoctorViewModel()
                    {
                        DoctorRoleId = doctorRole,
                        SurgeryId = surgeryViewModel.Guid,
                        Doctor = new DoctorViewModel()
                        {
                            ClinicSectionId = clinicSectionId,
                            User = new UserInformationViewModel
                            {
                                Name = surgeryViewModel.SurgeryOneName,
                                ClinicSectionId = clinicSectionId,
                                UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                Pass1 = "surgery",
                                UserName = "fgh"
                            }
                        }
                    };
                }
                surgeryViewModel.SurgeryDoctors.Add(surgery1);
            }


            if (!string.IsNullOrWhiteSpace(surgeryViewModel.SurgeryTwoName))
            {
                var surgeryTwoId = _unitOfWork.Doctor.GetDoctorByName(clinicSectionId, surgeryViewModel.SurgeryTwoName)?.Guid;
                var doctorRole = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Surgery2", "DoctorRole");

                //var duplicateDoctor = surgeryDoctors.FirstOrDefault(p => p.DoctorId == surgeryTwoId && p.DoctorRoleId != doctorRole);
                //if (duplicateDoctor != null)
                //    return $"DuplicateDoctor*Surgery2*{duplicateDoctor.DoctorRole.Name}";


                var existsDoctor = surgeryDoctors.SingleOrDefault(p => p.DoctorRoleId == doctorRole)?.DoctorId;
                if (existsDoctor != null && existsDoctor != surgeryTwoId)
                {
                    var wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == existsDoctor
                                                                    && f.SurgeryId == surgeryViewModel.Guid && f.SalaryTypeId == salaryTypeId);

                    if (wage != null)
                        return "ERROR_HumanResourceSallaryDependency*Surgery2";
                }

                SurgeryDoctorViewModel surgery2;

                if (surgeryTwoId != null && surgeryTwoId != Guid.Empty)
                {
                    surgery2 = new SurgeryDoctorViewModel()
                    {
                        DoctorId = surgeryTwoId,
                        DoctorRoleId = doctorRole,
                        SurgeryId = surgeryViewModel.Guid,
                    };
                }
                else
                {
                    surgery2 = new SurgeryDoctorViewModel()
                    {
                        DoctorRoleId = doctorRole,
                        SurgeryId = surgeryViewModel.Guid,
                        Doctor = new DoctorViewModel()
                        {
                            ClinicSectionId = clinicSectionId,
                            User = new UserInformationViewModel
                            {
                                Name = surgeryViewModel.SurgeryTwoName,
                                ClinicSectionId = clinicSectionId,
                                UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                Pass1 = "surgery",
                                UserName = "fgh"
                            }
                        }
                    };
                }

                surgeryViewModel.SurgeryDoctors.Add(surgery2);
            }

            Surgery surgery = ConvertModel(surgeryViewModel);


            if (!string.IsNullOrWhiteSpace(surgeryViewModel.AnesthesiologistName))
            {
                var anesthesiologistId = _unitOfWork.Doctor.GetDoctorByName(clinicSectionId, surgeryViewModel.AnesthesiologistName)?.Guid;
                var doctorRole = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Anesthesiologist", "DoctorRole");

                var existsDoctor = surgeryDoctors.SingleOrDefault(p => p.DoctorRoleId == doctorRole);

                if (existsDoctor == null)
                {
                    var anesthesiologist_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Anesthesiologist", "DoctorRole");
                    var old_wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == null && f.CadreTypeId == anesthesiologist_role
                                                                   && f.SurgeryId == surgeryViewModel.Guid && f.SalaryTypeId == salaryTypeId);

                    SurgeryDoctor surgery1;
                    if (anesthesiologistId != null && anesthesiologistId != Guid.Empty)
                    {
                        surgery1 = new SurgeryDoctor()
                        {
                            DoctorId = anesthesiologistId,
                            DoctorRoleId = doctorRole,
                            SurgeryId = surgeryViewModel.Guid,
                        };

                        if (old_wage != null)
                        {
                            HumanResource humanResource_Pediatrician = _unitOfWork.HumanResources.Get(anesthesiologistId.Value);
                            if (humanResource_Pediatrician == null)
                            {
                                humanResource_Pediatrician = new HumanResource()
                                {
                                    Guid = anesthesiologistId.Value,
                                    RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType")
                                };

                                _unitOfWork.HumanResources.Add(humanResource_Pediatrician);
                            }

                        }
                    }
                    else
                    {
                        Guid doctor_id = Guid.NewGuid();
                        surgery1 = new SurgeryDoctor()
                        {
                            DoctorRoleId = doctorRole,
                            DoctorId = doctor_id,
                            SurgeryId = surgeryViewModel.Guid,
                            Doctor = new Doctor()
                            {
                                Guid = doctor_id,
                                ClinicSectionId = clinicSectionId,
                                User = new User
                                {
                                    Guid = doctor_id,
                                    Name = surgeryViewModel.AnesthesiologistName,
                                    ClinicSectionId = clinicSectionId,
                                    UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                    Pass1 = "surgery",
                                    UserName = "fgh",
                                    HumanResource = new HumanResource
                                    {
                                        Guid = doctor_id,
                                        RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType")
                                    }
                                }
                            }
                        };

                    }

                    _unitOfWork.SurgeryDoctors.Add(surgery1);

                    if (old_wage != null)
                    {
                        old_wage.HumanResourceId = surgery1.DoctorId;
                        _unitOfWork.HumanResourceSalaries.UpdateState(old_wage);
                    }
                }
                else
                {
                    if (existsDoctor.DoctorId != anesthesiologistId)
                    {
                        var anesthesiologist_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Anesthesiologist", "DoctorRole");
                        var old_wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == existsDoctor.DoctorId
                                                                       && f.SurgeryId == surgeryViewModel.Guid && f.SalaryTypeId == salaryTypeId);

                        if (old_wage != null)
                            return "ERROR_HumanResourceSallaryDependency*Anesthesiologist";

                        _unitOfWork.SurgeryDoctors.Remove(existsDoctor);

                        SurgeryDoctor surgery1;
                        if (anesthesiologistId != null && anesthesiologistId != Guid.Empty)
                        {
                            surgery1 = new SurgeryDoctor()
                            {
                                DoctorId = anesthesiologistId,
                                DoctorRoleId = doctorRole,
                                SurgeryId = surgeryViewModel.Guid,
                            };
                        }
                        else
                        {
                            surgery1 = new SurgeryDoctor()
                            {
                                DoctorRoleId = doctorRole,
                                SurgeryId = surgeryViewModel.Guid,
                                Doctor = new Doctor()
                                {
                                    ClinicSectionId = clinicSectionId,
                                    User = new User
                                    {
                                        Name = surgeryViewModel.AnesthesiologistName,
                                        ClinicSectionId = clinicSectionId,
                                        UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                        Pass1 = "surgery",
                                        UserName = "fgh"
                                    }
                                }
                            };
                        }
                        _unitOfWork.SurgeryDoctors.Add(surgery1);
                    }
                }

            }
            else
            {
                var doctorRole = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Anesthesiologist", "DoctorRole");
                var existsDoctor = surgeryDoctors.SingleOrDefault(p => p.DoctorRoleId == doctorRole);

                if (existsDoctor != null)
                {
                    var old_wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == existsDoctor.DoctorId
                                                                   && f.SurgeryId == surgeryViewModel.Guid && f.SalaryTypeId == salaryTypeId);

                    if (old_wage != null)
                        return "ERROR_HumanResourceSallaryDependency*Anesthesiologist";

                    _unitOfWork.SurgeryDoctors.Remove(existsDoctor);
                }
            }


            if (!string.IsNullOrWhiteSpace(surgeryViewModel.PediatricianName))
            {
                var pediatricianId = _unitOfWork.Doctor.GetDoctorByName(clinicSectionId, surgeryViewModel.PediatricianName)?.Guid;
                var doctorRole = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Pediatrician", "DoctorRole");

                var existsDoctor = surgeryDoctors.SingleOrDefault(p => p.DoctorRoleId == doctorRole);

                if (existsDoctor == null)
                {
                    var pediatrician_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Pediatrician", "DoctorRole");
                    var old_wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == null && f.CadreTypeId == pediatrician_role
                                                                   && f.SurgeryId == surgeryViewModel.Guid && f.SalaryTypeId == salaryTypeId);

                    SurgeryDoctor surgery1;
                    if (pediatricianId != null && pediatricianId != Guid.Empty)
                    {
                        surgery1 = new SurgeryDoctor()
                        {
                            DoctorId = pediatricianId,
                            DoctorRoleId = doctorRole,
                            SurgeryId = surgeryViewModel.Guid,
                        };

                        if (old_wage != null)
                        {
                            HumanResource humanResource_Pediatrician = _unitOfWork.HumanResources.Get(pediatricianId.Value);
                            if (humanResource_Pediatrician == null)
                            {
                                humanResource_Pediatrician = new HumanResource()
                                {
                                    Guid = pediatricianId.Value,
                                    RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType")
                                };

                                _unitOfWork.HumanResources.Add(humanResource_Pediatrician);
                            }

                        }
                    }
                    else
                    {
                        Guid doctor_id = Guid.NewGuid();
                        surgery1 = new SurgeryDoctor()
                        {
                            DoctorRoleId = doctorRole,
                            DoctorId = doctor_id,
                            SurgeryId = surgeryViewModel.Guid,
                            Doctor = new Doctor()
                            {
                                Guid = doctor_id,
                                ClinicSectionId = clinicSectionId,
                                User = new User
                                {
                                    Guid = doctor_id,
                                    Name = surgeryViewModel.PediatricianName,
                                    ClinicSectionId = clinicSectionId,
                                    UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                    Pass1 = "surgery",
                                    UserName = "fgh",
                                    HumanResource = new HumanResource
                                    {
                                        Guid = doctor_id,
                                        RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType")
                                    }
                                }
                            }
                        };

                    }

                    _unitOfWork.SurgeryDoctors.Add(surgery1);

                    if (old_wage != null)
                    {
                        old_wage.HumanResourceId = surgery1.DoctorId;
                        _unitOfWork.HumanResourceSalaries.UpdateState(old_wage);
                    }
                }
                else
                {
                    if (existsDoctor.DoctorId != pediatricianId)
                    {
                        var pediatrician_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Pediatrician", "DoctorRole");
                        var old_wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == existsDoctor.DoctorId
                                                                       && f.SurgeryId == surgeryViewModel.Guid && f.SalaryTypeId == salaryTypeId);

                        if (old_wage != null)
                            return "ERROR_HumanResourceSallaryDependency*Pediatrician";

                        _unitOfWork.SurgeryDoctors.Remove(existsDoctor);

                        SurgeryDoctor surgery1;
                        if (pediatricianId != null && pediatricianId != Guid.Empty)
                        {
                            surgery1 = new SurgeryDoctor()
                            {
                                DoctorId = pediatricianId,
                                DoctorRoleId = doctorRole,
                                SurgeryId = surgeryViewModel.Guid,
                            };
                        }
                        else
                        {
                            surgery1 = new SurgeryDoctor()
                            {
                                DoctorRoleId = doctorRole,
                                SurgeryId = surgeryViewModel.Guid,
                                Doctor = new Doctor()
                                {
                                    ClinicSectionId = clinicSectionId,
                                    User = new User
                                    {
                                        Name = surgeryViewModel.PediatricianName,
                                        ClinicSectionId = clinicSectionId,
                                        UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                        Pass1 = "surgery",
                                        UserName = "fgh"
                                    }
                                }
                            };
                        }
                        _unitOfWork.SurgeryDoctors.Add(surgery1);
                    }
                }

            }
            else
            {
                var doctorRole = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Pediatrician", "DoctorRole");
                var existsDoctor = surgeryDoctors.SingleOrDefault(p => p.DoctorRoleId == doctorRole);

                if (existsDoctor != null)
                {
                    var old_wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == existsDoctor.DoctorId
                                                                   && f.SurgeryId == surgeryViewModel.Guid && f.SalaryTypeId == salaryTypeId);

                    if (old_wage != null)
                        return "ERROR_HumanResourceSallaryDependency*Pediatrician";

                    _unitOfWork.SurgeryDoctors.Remove(existsDoctor);
                }
            }

            _unitOfWork.Surgeries.UpdateSurgery(surgery);

            var paid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");
            var unPaid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
            var operation = _unitOfWork.ReceptionServices.GetReceptionOperation(surgeryViewModel.ReceptionId.Value);
            if (operation == null)
            {
                var receptionService = new ReceptionService()
                {
                    ReceptionId = surgeryViewModel.Guid,
                    ServiceId = surgeryViewModel.OperationId,
                    Number = 1,
                    Price = _unitOfWork.Services.Get(surgeryViewModel.OperationId).Price,
                    StatusId = unPaid
                };

                _unitOfWork.ReceptionServices.Add(receptionService);
            }
            else if (operation.ServiceId != surgeryViewModel.OperationId)
            {
                var rem = _unitOfWork.ReceptionServices.GetReceptionServiceRem(operation.Guid);
                var old_price = operation.Price;
                var new_price = _unitOfWork.Services.Get(surgeryViewModel.OperationId).Price;

                if (new_price > old_price - rem)
                {
                    operation.StatusId = unPaid;
                }
                else
                {
                    operation.StatusId = paid;
                }
                operation.ServiceId = surgeryViewModel.OperationId;
                operation.Price = new_price;

                _unitOfWork.ReceptionServices.UpdateState(operation);
            }

            _unitOfWork.Complete();


            var _reception = _unitOfWork.Receptions.GetReceptionWithServices(surgeryViewModel.ReceptionId.Value);
            var pay = _reception.ReceptionServices.All(p => p.StatusId == paid);
            _reception.PaymentStatusId = pay ? paid : unPaid;

            _unitOfWork.Receptions.UpdateState(_reception);
            _unitOfWork.Complete();


            return surgery.Guid.ToString();
        }

        public SurgeryViewModel GetSurgery(Guid SurgeryId)
        {
            Surgery Surgerygu = _unitOfWork.Surgeries.GetSurgery(SurgeryId);
            var result = ConvertModel(Surgerygu);
            return result;
        }


        public ServiceViewModel GetReceptionOperation(Guid ReceptionId)
        {
            ReceptionService ser = _unitOfWork.ReceptionServices.GetReceptionOperation(ReceptionId);
            var result = Common.ConvertModels<ServiceViewModel, Service>.convertModels(ser.Service);
            return result;
        }

        public SurgeryViewModel GetSurgeryReportForPrint(Guid surgeryId)
        {
            Surgery Surgerygu = _unitOfWork.Surgeries.GetSurgeryReportForPrint(surgeryId);
            return ConvertModelForPrint(Surgerygu);
        }

        public SurgeryViewModel GetSurgeryByReceptionId(Guid receptionId)
        {
            Surgery Surgerygu = _unitOfWork.Surgeries.GetSurgeryByReceptionId(receptionId);
            SurgeryViewModel su = ConvertModelForPrint(Surgerygu);
            try
            {
                su.Anesthesiologist = su.SurgeryDoctors.FirstOrDefault(x => x.DoctorRole.Name == "Anesthesiologist").Doctor;
            }
            catch { }
            try
            {
                su.SurgeryOne = su.SurgeryDoctors.FirstOrDefault(x => x.DoctorRole.Name == "Surgery1").Doctor;
            }
            catch { }
            try
            {
                su.DispatcherDoctor = su.SurgeryDoctors.FirstOrDefault(x => x.DoctorRole.Name == "DispatcherDoctor").Doctor;
            }
            catch { }

            return su;
        }

        public PieChartViewModel GetNearestOperations(Guid userId)
        {
            try
            {
                return null;
                //IEnumerable<DateTime> Receptiongu = _unitOfWork.Surgeries.GetNearestOperations(userId);


                //List<PieChartModel> pie = new List<PieChartModel>();

                //pie.Add(new PieChartModel
                //{
                //    Label = "Today",
                //    Value = Receptiongu.Where(a => a.Date == DateTime.Now.Date).Count()
                //});

                //pie.Add(new PieChartModel
                //{
                //    Label = "Yesterday",
                //    Value = Receptiongu.Where(a => a.Date == DateTime.Now.AddDays(-1).Date).Count()
                //});

                //var culture = CultureInfo.CurrentCulture;
                //var diff = DateTime.Now.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
                //if (diff < 0)
                //    diff += 7;
                ////d = d.AddDays(-diff).Date;


                //pie.Add(new PieChartModel
                //{
                //    Label = "Week",
                //    Value = Receptiongu.Where(a => a.Date >= DateTime.Now.AddDays(-diff).Date && a.Date <= DateTime.Now.Date).Count()
                //});

                //var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                //pie.Add(new PieChartModel
                //{
                //    Label = "Mounth",
                //    Value = Receptiongu.Where(a => a.Date >= firstDayOfMonth.Date && a.Date <= DateTime.Now.Date).Count()
                //});


                //var firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);

                //pie.Add(new PieChartModel
                //{
                //    Label = "Year",
                //    Value = Receptiongu.Where(a => a.Date >= firstDayOfYear.Date && a.Date <= DateTime.Now.Date).Count()
                //});

                //pie.Add(new PieChartModel
                //{
                //    Label = "All",
                //    Value = Receptiongu.Count()
                //});

                //PieChartViewModel chart = new PieChartViewModel
                //{
                //    Labels = pie.Select(a => a.Label).ToArray(),
                //    Value = pie.Select(a => Convert.ToInt32(a.Value)).ToArray()
                //};

                //return chart;
            }
            catch (Exception e) { throw e; }
        }


        public SurgeryViewModel ConvertModelForPrint(Surgery Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Surgery, SurgeryViewModel>();
                cfg.CreateMap<SurgeryDoctor, SurgeryDoctorViewModel>();
                cfg.CreateMap<Reception, ReceptionViewModel>();
                cfg.CreateMap<Patient, PatientViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Surgery, SurgeryViewModel>(Users);
        }

        public Surgery ConvertModel(SurgeryViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SurgeryViewModel, Surgery>();
                cfg.CreateMap<SurgeryDoctorViewModel, SurgeryDoctor>();
                cfg.CreateMap<DoctorViewModel, Doctor>();
                cfg.CreateMap<UserInformationViewModel, User>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<SurgeryViewModel, Surgery>(Users);
        }

        public SurgeryViewModel ConvertModel(Surgery Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Surgery, SurgeryViewModel>()
                .ForMember(a => a.OperationId, b => b.MapFrom(c => c.Reception.ReceptionServices.FirstOrDefault().Service.Guid))
                .ForMember(a => a.AnesthesiologistId, b => b.MapFrom(c => c.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Anesthesiologist").DoctorId))
                .ForMember(a => a.AnesthesiologistName, b => b.MapFrom(c => c.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Anesthesiologist").Doctor.User.Name))
                .ForMember(a => a.SurgeryOneId, b => b.MapFrom(c => c.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Surgery1").DoctorId))
                .ForMember(a => a.SurgeryOneName, b => b.MapFrom(c => c.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Surgery1").Doctor.User.Name))
                .ForMember(a => a.SurgeryTwoId, b => b.MapFrom(c => c.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Surgery2").DoctorId))
                .ForMember(a => a.SurgeryTwoName, b => b.MapFrom(c => c.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Surgery2").Doctor.User.Name))
                .ForMember(a => a.PediatricianId, b => b.MapFrom(c => c.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Pediatrician").DoctorId))
                .ForMember(a => a.PediatricianName, b => b.MapFrom(c => c.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Pediatrician").Doctor.User.Name))
                .ForMember(a => a.SurgeryDoctors, b => b.Ignore())
                ;
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.AddressName, b => b.MapFrom(c => c.Patient.Address.Name))
                .ForMember(a => a.ReceptionServices, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Address, b => b.Ignore())
                ;
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Surgery, SurgeryViewModel>(Users);
        }

        public Surgery ConvertViewModelToModel(SurgeryViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SurgeryViewModel, Surgery>();
                cfg.CreateMap<UserInformationViewModel, User>();
                cfg.CreateMap<BaseInfoViewModel, BaseInfo>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<SurgeryViewModel, Surgery>(Users);
        }

        public List<SurgeryViewModel> ConvertModelsLists(IEnumerable<Surgery> Surgerys)
        {
            List<SurgeryViewModel> SurgeryDtoList = new List<SurgeryViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Surgery, SurgeryViewModel>()
                .ForMember(a => a.OperationName, b => b.MapFrom(c => c.Reception.ReceptionServices.FirstOrDefault().Service.Name ?? ""))
                .ForMember(a => a.SurgeryOneName, b => b.MapFrom(c => c.SurgeryDoctors.FirstOrDefault().Doctor.User.Name ?? ""))
                .ForMember(a => a.SurgeryDoctors, b => b.Ignore());
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.ReceptionServices, b => b.Ignore());
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            SurgeryDtoList = mapper.Map<IEnumerable<Surgery>, List<SurgeryViewModel>>(Surgerys);
            return SurgeryDtoList;
        }


        public List<Surgery> ConvertViewModelsLists(IEnumerable<SurgeryViewModel> Surgerys)
        {
            List<Surgery> SurgeryDtoList = new List<Surgery>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SurgeryViewModel, Surgery>();
                cfg.CreateMap<PatientViewModel, Patient>();
                cfg.CreateMap<UserInformationViewModel, User>();
            });

            IMapper mapper = config.CreateMapper();
            SurgeryDtoList = mapper.Map<IEnumerable<SurgeryViewModel>, List<Surgery>>(Surgerys);
            return SurgeryDtoList;
        }

        public List<SurgeryGridViewModel> SurgeryGridConvertViewModelsLists(IEnumerable<SurgeryGrid> Surgerys)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SurgeryGrid, SurgeryGridViewModel>()
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<SurgeryGrid>, List<SurgeryGridViewModel>>(Surgerys);
        }

        public IEnumerable<SurgeryGridViewModel> GetAllSurgeryByClinicSectionId(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo, Guid? doctorId, Guid? operationId)
        {
            try
            {
                IEnumerable<SurgeryGrid> PatientReceptionDtos = new List<SurgeryGrid>();

                if (periodId != (int)Periods.FromDateToDate)
                {
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                }

                PatientReceptionDtos = _unitOfWork.Surgeries.GetAllSurgeryByClinicSectionId(clinicSectionId, DateFrom, DateTo, doctorId, operationId);

                List<SurgeryGridViewModel> PatientReception = SurgeryGridConvertViewModelsLists(PatientReceptionDtos);
                Indexing<SurgeryGridViewModel> indexing = new Indexing<SurgeryGridViewModel>();
                return indexing.AddIndexing(PatientReception);
            }
            catch (Exception e) { return null; }
        }

        
    }
}
