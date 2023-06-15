using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.Child;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ChildMvcMockingService : IChildMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChildMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public OperationStatus RemoveChild(Guid childId)
        {
            try
            {
                Child child = _unitOfWork.Children.Get(childId);
                User user = _unitOfWork.Users.Get(childId);

                _unitOfWork.Children.Remove(child);
                _unitOfWork.Users.Remove(user);

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


        public string AddToHospitalPatient(ChildHospitalPatientViewModel viewModel)
        {
            if (viewModel.Guid == Guid.Empty || viewModel.ReceptionId.GetValueOrDefault(Guid.Empty) == Guid.Empty ||
                viewModel.DoctorId.GetValueOrDefault(Guid.Empty) == Guid.Empty || viewModel.RoomId.GetValueOrDefault(Guid.Empty) == Guid.Empty)
                return "DataNotValid";

            if (!DateTime.TryParseExact(viewModel.TxtReceptionDate, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out DateTime receptionDate))
                return "DateNotValid";

            Child child = _unitOfWork.Children.Get(viewModel.Guid);
            child.ReceptionId = viewModel.ReceptionId;
            child.ReceptionDate = receptionDate;
            child.DoctorId = viewModel.DoctorId;
            child.RoomId = viewModel.RoomId;
            child.ModifiedUserId = viewModel.UserId;
            child.ModifiedDate = DateTime.Now;

            _unitOfWork.Children.UpdateState(child);
            _unitOfWork.Complete();
            return "";

        }

        public string RemoveFromHospitalPatient(Guid childId, Guid userId)
        {
            Child child = _unitOfWork.Children.Get(childId);
            child.ReceptionId = null;
            child.ReceptionDate = null;
            child.DoctorId = null;
            child.RoomId = null;
            child.ModifiedUserId = userId;
            child.ModifiedDate = DateTime.Now;

            _unitOfWork.Children.UpdateState(child);
            _unitOfWork.Complete();
            return "";

        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Child/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewChild(ChildViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Name) || viewModel.ChildStatus == null || viewModel.TimeOfBirth == null ||
                !float.TryParse(viewModel.ChildWeight, NumberStyles.Any, CultureInfo.InvariantCulture, out float weightResult) || weightResult <= 0 ||
                (viewModel.NeedOperation.GetValueOrDefault(false) && string.IsNullOrWhiteSpace(viewModel.OperationOrder)))
                return "DataNotValid";

            if (!DateTime.TryParseExact($"{viewModel.DateBirth} {viewModel.TimeOfBirth.Value.Hours:00}:{viewModel.TimeOfBirth.Value.Minutes:00}", "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out DateTime birthDay))
                return "DateNotValid";

            var user = new User
            {
                ClinicSectionId = viewModel.ClinicSectionId,
                UserName = "",
                Pass1 = "child",
                Name = viewModel.Name.Trim(),
                GenderId = viewModel.GenderId,

                Child = new Child
                {
                    BirthDate = birthDay,
                    ChildStatus = viewModel.ChildStatus,
                    VitalActivities = viewModel.VitalActivities,
                    CongenitalAnomalies = viewModel.CongenitalAnomalies,
                    CreateDate = DateTime.Now,
                    CreateUserId = viewModel.UserId,
                    NeedOperation = viewModel.NeedOperation,
                    OperationOrder = viewModel.NeedOperation.GetValueOrDefault(false) ? viewModel.OperationOrder : "",
                    Weight = (decimal)weightResult
                }

            };

            _unitOfWork.Users.Add(user);
            _unitOfWork.Complete();
            return user.Guid.ToString();
        }


        public string UpdateChild(ChildViewModel viewModel)
        {
            if (viewModel.Guid == Guid.Empty || string.IsNullOrWhiteSpace(viewModel.Name) || viewModel.ChildStatus == null || viewModel.TimeOfBirth == null ||
                !float.TryParse(viewModel.ChildWeight, NumberStyles.Any, CultureInfo.InvariantCulture, out float weightResult) || weightResult <= 0 ||
                (viewModel.NeedOperation.GetValueOrDefault(false) && string.IsNullOrWhiteSpace(viewModel.OperationOrder)))
                return "DataNotValid";

            if (!DateTime.TryParseExact($"{viewModel.DateBirth} {viewModel.TimeOfBirth.Value.Hours:00}:{viewModel.TimeOfBirth.Value.Minutes:00}", "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out DateTime birthDay))
                return "DateNotValid";

            var child = _unitOfWork.Children.GetChildWithUser(viewModel.Guid);

            child.User.GenderId = viewModel.GenderId;
            child.User.Name = viewModel.Name.Trim();

            child.BirthDate = birthDay;
            child.ChildStatus = viewModel.ChildStatus;
            child.VitalActivities = viewModel.VitalActivities;
            child.CongenitalAnomalies = viewModel.CongenitalAnomalies;
            child.ModifiedDate = DateTime.Now;
            child.ModifiedUserId = viewModel.UserId;
            child.NeedOperation = viewModel.NeedOperation;
            child.OperationOrder = viewModel.NeedOperation.GetValueOrDefault(false) ? viewModel.OperationOrder : "";
            child.Weight = (decimal)weightResult;

            _unitOfWork.Users.UpdateState(child.User);
            _unitOfWork.Children.UpdateState(child);
            _unitOfWork.Complete();
            return child.Guid.ToString();

        }

        public bool CheckRepeatedChildName(Guid clinicSectionId, string name, bool NewOrUpdate, string oldName = "")
        {
            bool child = false;
            if (NewOrUpdate)
            {
                child = _unitOfWork.Users.Find(x => x.Name.Trim() == name.Trim() && x.ClinicSectionId == clinicSectionId && x.UserTypeId == null).Any();
            }
            else
            {
                child = _unitOfWork.Users.Find(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName && x.ClinicSectionId == clinicSectionId && x.UserTypeId == null).Any();
            }
            return child;
        }

        public IEnumerable<ChildViewModel> GetAllChildren(Guid clinicSectionId)
        {
            IEnumerable<Child> hosp = _unitOfWork.Children.GetAllChild(clinicSectionId);
            List<ChildViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<ChildViewModel> indexing = new Indexing<ChildViewModel>();
            return indexing.AddIndexing(hospconvert);
        }


        public IEnumerable<ChildHospitalPatientViewModel> GetAllUnknownChildren(Guid clinicSectionId)
        {
            IEnumerable<Child> hosp = _unitOfWork.Children.GetAllUnknownChildren(clinicSectionId);
            List<ChildHospitalPatientViewModel> hospconvert = hosp.Select(p => new ChildHospitalPatientViewModel
            {
                Guid = p.Guid,
                Name = p.User.Name
            }).ToList();
            Indexing<ChildHospitalPatientViewModel> indexing = new Indexing<ChildHospitalPatientViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public ChildViewModel GetChild(Guid ChildId)
        {
            Child Childgu = _unitOfWork.Children.GetChildWithUser(ChildId);
            return ConvertModel(Childgu);
        }

        public IEnumerable<ChildHospitalPatientViewModel> GetAllHospitalPatientChildren(Guid receptionId)
        {

            IEnumerable<Child> hosp = _unitOfWork.Children.GetAllHospitalPatientChildren(receptionId);
            List<ChildHospitalPatientViewModel> hospconvert = ConvertHospitalPatientModelsLists(hosp);
            Indexing<ChildHospitalPatientViewModel> indexing = new Indexing<ChildHospitalPatientViewModel>();
            return indexing.AddIndexing(hospconvert);
        }


        public IEnumerable<NewBornBabiesReportViewModel> GetAllHospitalPatientChildrenReport(Guid receptionId)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            IEnumerable<Child> hosp = _unitOfWork.Children.GetAllHospitalPatientChildren(receptionId);
            List<NewBornBabiesReportViewModel> hospconvert = hosp.Select(p => new NewBornBabiesReportViewModel
            {
                Name = p.User.Name,
                Weight = p.Weight?.ToString("G", CultureInfo.InvariantCulture),
                GenderName = p.User.Gender.Name,
                BirthDate = p.BirthDate?.ToString("dd/MM/yyyy", cultures),
                BirthTime = p.BirthDate?.ToString("HH:mm"),
                ReceptionDoctor = p.Doctor.User.Name,
                RecivedDate = p.ReceptionDate?.ToString("dd/MM/yyyy HH:mm", cultures),
            }).ToList();

            return hospconvert;
        }

        public ChildReportResultViewModel ChildReport(ChildReportViewModel reportViewModel)
        {
            ChildReportResultViewModel report = new();
            report.AllChildren = new List<NewBornBabiesReportViewModel>();

            List<Child> childDto = _unitOfWork.Children.GetDetailChildReport(reportViewModel.AllClinicSectionGuids, reportViewModel.FromDate, reportViewModel.ToDate, p =>
                            (reportViewModel.GenderId == null || p.User.GenderId == reportViewModel.GenderId) &&
                            (reportViewModel.ChildStatus == null || p.ChildStatus == reportViewModel.ChildStatus)
            ).ToList();

            CultureInfo cultures = new CultureInfo("en-US");
            if (reportViewModel.Detail)
            {
                foreach (var item in childDto)
                {
                    var reportItem = new NewBornBabiesReportViewModel
                    {
                        Name = item.User.Name,
                        GenderName = item.User.Gender.Name,
                        BirthDate = item.BirthDate?.ToString("dd/MM/yyyy", cultures),
                        BirthTime = item.BirthDate?.ToString("HH:mm"),
                        StatusName = item.Status.Name,
                        Weight = item.Weight?.ToString("G", CultureInfo.InvariantCulture),
                        VitalActivities = item.VitalActivities,
                        CongenitalAnomalies = item.CongenitalAnomalies,
                        OperationOrder = item.OperationOrder,
                    };

                    report.AllChildren.Add(reportItem);
                }

            }
            else
            {
                foreach (var item in childDto)
                {
                    var reportItem = new NewBornBabiesReportViewModel
                    {
                        Name = item.User.Name,
                        GenderName = item.User.Gender.Name,
                        BirthDate = item.BirthDate?.ToString("dd/MM/yyyy", cultures),
                        BirthTime = item.BirthDate?.ToString("HH:mm"),
                        StatusName = item.Status.Name,
                        Weight = item.Weight?.ToString("G", CultureInfo.InvariantCulture),
                    };

                    report.AllChildren.Add(reportItem);
                }

            }

            report.ChildrenStatus = new List<NewBornBabiesReportViewModel>();
            report.ChildrenStatus.AddRange(report.AllChildren.GroupBy(g => g.BirthDate).Select(s => new NewBornBabiesReportViewModel
            {
                BirthDate = s.Key,
                Count = s.Count().ToString("N0"),
            }));

            report.TotalChildren = report.AllChildren.Count.ToString("N0");

            return report;
        }

        // Begin Convert 
        public ChildViewModel ConvertModel(Child Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Child, ChildViewModel>()
                .ForMember(a => a.UserId, b => b.Ignore())
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.GenderId, b => b.MapFrom(c => c.User.GenderId))
                .ForMember(a => a.DateOfBirth, b => b.MapFrom(c => c.BirthDate))
                .ForMember(a => a.TimeOfBirth, b => b.MapFrom(c => c.BirthDate.Value.TimeOfDay))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Child, ChildViewModel>(Users);
        }

        public List<ChildViewModel> ConvertModelsLists(IEnumerable<Child> childs)
        {
            List<ChildViewModel> childDtoList = new List<ChildViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Child, ChildViewModel>()
                .ForMember(a => a.UserId, b => b.Ignore())
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.GenderName, b => b.MapFrom(c => c.User.Gender.Name))
                .ForMember(a => a.DateOfBirth, b => b.MapFrom(c => c.BirthDate))
                .ForMember(a => a.TimeOfBirth, b => b.MapFrom(c => c.BirthDate.Value.TimeOfDay))
                ;
            });

            IMapper mapper = config.CreateMapper();
            childDtoList = mapper.Map<IEnumerable<Child>, List<ChildViewModel>>(childs);
            return childDtoList;
        }

        public List<ChildHospitalPatientViewModel> ConvertHospitalPatientModelsLists(IEnumerable<Child> childs)
        {
            List<ChildHospitalPatientViewModel> childDtoList = new List<ChildHospitalPatientViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Child, ChildHospitalPatientViewModel>()
                .ForMember(a => a.UserId, b => b.Ignore())
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.GenderName, b => b.MapFrom(c => c.User.Gender.Name))
                .ForMember(a => a.DoctorName, b => b.MapFrom(c => c.Doctor.User.Name))
                .ForMember(a => a.RoomName, b => b.MapFrom(c => $" {c.Room.ClinicSection.Name} | {c.Room.Name}"))
                ;
            });

            IMapper mapper = config.CreateMapper();
            childDtoList = mapper.Map<IEnumerable<Child>, List<ChildHospitalPatientViewModel>>(childs);
            return childDtoList;
        }
        // End Convert
    }
}
