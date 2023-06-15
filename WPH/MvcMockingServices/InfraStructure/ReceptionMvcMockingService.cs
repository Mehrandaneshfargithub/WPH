using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.PatientImage;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.CustomDataModels.Visit;
using WPH.Models.Emergency;
using WPH.Models.Reception;
using WPH.Models.ReceptionAmbulance;
using WPH.Models.ReceptionClinicSection;
using WPH.Models.ReceptionRoomBed;
using WPH.Models.ReceptionService;
using WPH.Models.Surgery;
using WPH.MvcMockingServices.Interface;
using WPH.Models.SurgeryDoctor;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.ReceptionServiceReceived;
using WPH.Models.HumanResourceSalary;
using WPH.Models.HumanResource;
using WPH.Models.ReceptionTemperature;
using WPH.Models.Hospital;
using WPH.Models.Service;
using WPH.Models.ReceptionDoctor;
using WPH.Models.HospitalPatients;
using MoreLinq;
using WPH.Models.CustomDataModels.ReserveDetail;
using System.Threading.Tasks;
using WPH.Models.Chart;
using System.Globalization;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReceptionMvcMockingService : IReceptionMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _IDUNIT;
        public ReceptionMvcMockingService(IUnitOfWork unitOfWork, IDIUnit dIUnit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _IDUNIT = dIUnit;
        }

        public OperationStatus RemoveReception(Guid Receptionid, string rootPath)
        {
            try
            {
                if (_unitOfWork.Receptions.GetReceptionDischargeStatus(Receptionid))
                    return OperationStatus.CanNotDelete;

                var images = _unitOfWork.Receptions.RemoveReception(Receptionid);
                FileAttachments deleteIamge = new();
                var imageDto = Common.ConvertModels<PatientImageViewModel, PatientImage>.convertModelsLists(images);
                _unitOfWork.Complete();
                deleteIamge.DeleteAllFiles(imageDto, rootPath);


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

        public OperationStatus RemoveReceptionTemperature(Guid Receptionid)
        {
            try
            {
                _unitOfWork.ReceptionTemperature.Remove(_unitOfWork.ReceptionTemperature.Get(Receptionid));
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
            string controllerName = "/Reception/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewReception(ReceptionViewModel reception)
        {
            bool newPatient = false;
            RoomBed roomBed = null;
            FileAttachments fileAttachments = new();
            try
            {
                if (reception.Guid != Guid.Empty)
                {
                    reception.Patient.DateOfBirth = new DateTime(Convert.ToInt32(reception.Patient.DateOfBirthYear), Convert.ToInt32(reception.Patient.DateOfBirthMonth), Convert.ToInt32(reception.Patient.DateOfBirthDay));

                    //if (newPatient)
                    reception.Patient.User = Common.ConvertModels<UserInformationViewModel, PatientViewModel>.convertModels(reception.Patient);

                    SurgeryViewModel surgery = new();

                    surgery.ReceptionId = reception.Guid;
                    surgery.ClinicSectionId = reception.ClinicSectionId;
                    if (reception.PurposeName == "Surgery")
                    {
                        string[] surgeryTime = reception.SurgeryTimeTime.Split(':');
                        surgery.SurgeryDate = new DateTime(Convert.ToInt32(reception.SurgeryTimeYear), Convert.ToInt32(reception.SurgeryTimeMonth), Convert.ToInt32(reception.SurgeryTimeDay), Convert.ToInt32(surgeryTime[0]), Convert.ToInt32(surgeryTime[1]), 0);

                        reception.Surgeries = new List<SurgeryViewModel>() { surgery };
                    }

                    reception.PatientImages = new List<PatientImageViewModel>();

                    if (reception.MainAttachments != null && reception.MainAttachments.Any())
                    {
                        var attachmentTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("MainAttachment", "AttachmentType");
                        foreach (var attachment in reception.MainAttachments)
                        {
                            var att = fileAttachments.UploadFiles(attachment, reception.RootPath, reception.Guid, reception.PatientId, attachmentTypeId);
                            if (att != null)
                                reception.PatientImages.Add(att);
                        }
                    }

                    if (reception.OtherAttachments != null && reception.OtherAttachments.Any())
                    {
                        var attachmentTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("OtherAttachment", "AttachmentType");
                        foreach (var attachment in reception.OtherAttachments)
                        {
                            var att = fileAttachments.UploadFiles(attachment, reception.RootPath, reception.Guid, reception.PatientId, attachmentTypeId);
                            if (att != null)
                                reception.PatientImages.Add(att);
                        }
                    }

                    if (reception.PoliceReport != null && reception.PoliceReport.Any())
                    {
                        var attachmentTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("PoliceReport", "AttachmentType");
                        foreach (var attachment in reception.PoliceReport)
                        {
                            var att = fileAttachments.UploadFiles(attachment, reception.RootPath, reception.Guid, reception.PatientId, attachmentTypeId);
                            if (att != null)
                                reception.PatientImages.Add(att);
                        }
                    }

                    if (reception.PatientImages.Count == 0)
                        reception.PatientImages = null;
                    else
                    {
                        _unitOfWork.PatientImage.AddRange(Common.ConvertModels<PatientImage, PatientImageViewModel>.convertModelsLists(reception.PatientImages));
                    }

                    Reception Reception2 = ConvertViewModelToModel(reception);

                    Guid receptionId = _unitOfWork.Receptions.UpdateReception(Reception2);

                    if (reception.PurposeName == "Surgery")
                    {
                        int? DispatcherDoctorId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("DispatcherDoctor", "DoctorRole");
                        var surgeryId = _unitOfWork.Surgeries.GetSingle(p => p.ReceptionId == Reception2.Guid).Guid;
                        var dispatcherDoctor = _unitOfWork.SurgeryDoctors.GetDoctorBySurgeryAndRoleId(surgeryId, DispatcherDoctorId);

                        if (!string.IsNullOrWhiteSpace(reception.DispatcherDoctorName))
                        {
                            Guid DoctorId;
                            if (_IDUNIT.doctor.CheckRepeatedNameAndSpeciallity(reception.DispatcherDoctorName, null, reception.OrginalClinicSectionId, false))
                            {
                                DoctorId = _unitOfWork.Doctor.Find(x => x.ClinicSectionId == reception.OrginalClinicSectionId && x.User.Name == reception.DispatcherDoctorName).FirstOrDefault().Guid;
                            }
                            else
                            {
                                Doctor doc = new();
                                doc.ClinicSectionId = reception.OrginalClinicSectionId;
                                doc.Guid = Guid.NewGuid();

                                doc.User = new User()
                                {
                                    ClinicSectionId = reception.OrginalClinicSectionId,
                                    Guid = doc.Guid,
                                    Name = reception.DispatcherDoctorName,
                                    UserName = "123",
                                    UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                    Pass1 = "slsl",
                                };

                                DoctorId = doc.Guid;

                                _unitOfWork.Doctor.Add(doc);
                            }

                            if (dispatcherDoctor == null)
                            {
                                SurgeryDoctor surgeryDoctor = new()
                                {
                                    DoctorId = DoctorId,
                                    SurgeryId = surgeryId,
                                    DoctorRoleId = DispatcherDoctorId
                                };

                                _unitOfWork.SurgeryDoctors.Add(surgeryDoctor);
                            }
                            else
                            {
                                dispatcherDoctor.DoctorId = DoctorId;

                                _unitOfWork.SurgeryDoctors.UpdateState(dispatcherDoctor);
                            }

                        }
                        else
                        {
                            if (dispatcherDoctor != null)
                                _unitOfWork.SurgeryDoctors.Remove(dispatcherDoctor);
                        }
                    }

                    var oldRoomBedReception = _unitOfWork.ReceptionRoomBeds.GetReceptionRMWithReceptionAndBedByReceptionId(receptionId);

                    if (oldRoomBedReception == null)
                    {
                        if (reception.RoomBedId != null && reception.RoomBedId != Guid.Empty)
                        {
                            ReceptionRoomBed newReceptionRoomBed = new()
                            {
                                ReceptionId = reception.Guid,
                                EntranceDate = DateTime.Now,
                                CreatedDate = DateTime.Now,
                                CreatedUserId = reception.UserId,
                                ModifiedUserId = reception.UserId,
                                RoomBedId = reception.RoomBedId,
                            };


                            roomBed = _unitOfWork.RoomBeds.Get(reception.RoomBedId.Value);
                            roomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Full", "RoomStatus");

                            _unitOfWork.ReceptionRoomBeds.Add(newReceptionRoomBed);
                            _unitOfWork.RoomBeds.UpdateState(roomBed);
                        }
                    }
                    else
                    {
                        if (reception.RoomBedId != null && reception.RoomBedId != Guid.Empty)
                        {
                            if (reception.RoomBedId != oldRoomBedReception.RoomBedId)
                            {
                                oldRoomBedReception.ExitDate = DateTime.Now;
                                oldRoomBedReception.RoomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Clean", "RoomStatus");

                                _unitOfWork.ReceptionRoomBeds.UpdateState(oldRoomBedReception);
                                _unitOfWork.RoomBeds.UpdateState(oldRoomBedReception.RoomBed);


                                ReceptionRoomBed newReceptionRoomBed = new()
                                {
                                    ReceptionId = reception.Guid,
                                    EntranceDate = DateTime.Now,
                                    CreatedDate = DateTime.Now,
                                    CreatedUserId = reception.UserId,
                                    ModifiedUserId = reception.UserId,
                                    RoomBedId = reception.RoomBedId,
                                };

                                roomBed = _unitOfWork.RoomBeds.Get(reception.RoomBedId.Value);
                                roomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Full", "RoomStatus");

                                _unitOfWork.ReceptionRoomBeds.Add(newReceptionRoomBed);
                                _unitOfWork.RoomBeds.UpdateState(roomBed);
                            }
                        }
                        else
                        {
                            oldRoomBedReception.ExitDate = DateTime.Now;
                            oldRoomBedReception.RoomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Clean", "RoomStatus");

                            _unitOfWork.ReceptionRoomBeds.UpdateState(oldRoomBedReception);
                            _unitOfWork.RoomBeds.UpdateState(oldRoomBedReception.RoomBed);
                        }
                    }

                    _unitOfWork.Complete();
                    return receptionId.ToString();
                }
                else
                {
                    Guid? patientId = reception.PatientId;
                    //if (reception.PatientId == null)
                    //{
                    PatientViewModel pat = _IDUNIT.patient.CheckNewPatient(ref newPatient, reception.Patient, reception.ClinicId, reception.OrginalClinicSectionId);
                    if (pat == null)
                    {
                        reception.PatientId = _IDUNIT.patient.GetPatientIdByName(reception.Patient.Name, reception.OrginalClinicSectionId);
                        reception.Patient = null;
                        patientId = reception.PatientId;
                    }
                    else
                    {
                        reception.Patient = pat;
                        patientId = pat.Guid;
                    }

                    //}
                    //else
                    //{

                    //}

                    int? unpaidId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");

                    reception.Guid = Guid.NewGuid();
                    reception.Active = true;
                    reception.CreatedDate = DateTime.Now;
                    reception.CreatedUserId = reception.UserId;
                    reception.EntranceDate = DateTime.Now;
                    reception.ReceptionDate = DateTime.Now;
                    reception.PaymentStatusId = unpaidId;
                    reception.Discharge = false;

                    //reception.ReceptionClinicSections = new List<ReceptionClinicSectionViewModel>() {
                    //    new ReceptionClinicSectionViewModel {
                    //        ClinicSectionId = reception.ClinicSectionId,
                    //        CreatedDate = DateTime.Now,
                    //        CreatedUserId = reception.UserId,
                    //        Guid = Guid.NewGuid(),
                    //        ReceptionId = reception.Guid
                    //    }
                    //};

                    if (reception.ClinicSectionName == "Emergency")
                    {
                        if (reception.Emergency == null)
                        {
                            reception.Emergency = new EmergencyViewModel();
                        }
                        reception.Emergency.Guid = reception.Guid;

                        if (reception.ReceptionAmbulance != null)
                        {
                            if (!string.IsNullOrWhiteSpace(reception.ReceptionAmbulance.FromHospitalName))
                            {
                                var hospitalId = _unitOfWork.Hospitals.GetHospitalByName(reception.ReceptionAmbulance.FromHospitalName)?.Guid;
                                reception.ReceptionAmbulance.FromHospitalId = hospitalId;

                                if (hospitalId == null || hospitalId == Guid.Empty)
                                {
                                    reception.ReceptionAmbulance.FromHospital = new HospitalViewModel
                                    {
                                        Name = reception.ReceptionAmbulance.FromHospitalName
                                    };
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(reception.ReceptionAmbulance.ToHospitalName))
                            {
                                var hospitalId = _unitOfWork.Hospitals.GetHospitalByName(reception.ReceptionAmbulance.ToHospitalName)?.Guid;
                                reception.ReceptionAmbulance.ToHospitalId = hospitalId;

                                if (hospitalId == null || hospitalId == Guid.Empty)
                                {
                                    reception.ReceptionAmbulance.ToHospital = new HospitalViewModel
                                    {
                                        Name = reception.ReceptionAmbulance.ToHospitalName
                                    };
                                }
                            }

                            if (reception.ReceptionAmbulance.Cost.GetValueOrDefault(0) > 0)
                            {
                                var serviceId = _unitOfWork.Services.GetServiceByName(null, "Ambulance")?.Guid;
                                ReceptionServiceViewModel service = new()
                                {
                                    ServiceId = serviceId,
                                    Number = 1,
                                    Price = reception.ReceptionAmbulance.Cost,
                                    StatusId = unpaidId,
                                    CreatedDate = DateTime.Now,
                                    CreatedUserId = reception.CreatedUserId
                                };

                                if (serviceId == null || serviceId == Guid.Empty)
                                {
                                    service.Service = new ServiceViewModel
                                    {
                                        Name = "Ambulance",
                                        Price = 1,
                                        TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Other", "ServiceType")

                                    };
                                }

                                if (reception.ReceptionServices == null)
                                    reception.ReceptionServices = new List<ReceptionServiceViewModel>();

                                reception.ReceptionServices.Add(service);
                            }

                            reception.ReceptionAmbulances = new List<ReceptionAmbulanceViewModel>() { reception.ReceptionAmbulance };

                        }

                    }

                    if (reception.RoomBedId != null && reception.RoomBedId != Guid.Empty)
                    {
                        ReceptionRoomBedViewModel roomBedModel = new();
                        roomBedModel.Guid = Guid.NewGuid();
                        roomBedModel.ReceptionId = reception.Guid;
                        roomBedModel.EntranceDate = DateTime.Now;
                        roomBedModel.CreatedDate = DateTime.Now;
                        roomBedModel.CreatedUserId = reception.UserId;
                        roomBedModel.ModifiedUserId = reception.UserId;
                        roomBedModel.RoomBedId = reception.RoomBedId;
                        reception.ReceptionRoomBeds = new List<ReceptionRoomBedViewModel>() { roomBedModel };

                        roomBed = _unitOfWork.RoomBeds.Get(reception.RoomBedId.Value);
                        roomBed.StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Full", "RoomStatus");
                        _unitOfWork.RoomBeds.UpdateState(roomBed);
                    }

                    if (reception.PurposeName == "Surgery")
                    {
                        int? surgeryOneId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Surgery1", "DoctorRole");
                        int? DispatcherDoctorId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("DispatcherDoctor", "DoctorRole");
                        SurgeryViewModel surgery = new();
                        surgery.CreatedDate = DateTime.Now;
                        surgery.CreatedUserId = reception.UserId;
                        surgery.Guid = Guid.NewGuid();
                        surgery.ReceptionId = reception.Guid;
                        surgery.ClinicSectionId = reception.ClinicSectionId;
                        string[] surgeryTime = reception.SurgeryTimeTime.Split(':');
                        surgery.SurgeryDate = new DateTime(Convert.ToInt32(reception.SurgeryTimeYear), Convert.ToInt32(reception.SurgeryTimeMonth), Convert.ToInt32(reception.SurgeryTimeDay), Convert.ToInt32(surgeryTime[0]), Convert.ToInt32(surgeryTime[1]), 0);
                        surgery.SurgeryDoctors = new List<SurgeryDoctorViewModel>();
                        if (reception.SurgeryOne == null)
                        {

                            SurgeryDoctorViewModel surgeryDoctor = new();

                            if (_IDUNIT.doctor.CheckRepeatedNameAndSpeciallity(reception.SurgeryOneName, null, reception.OrginalClinicSectionId, false))
                            {
                                surgeryDoctor.DoctorId = _unitOfWork.Doctor.Find(x => x.ClinicSectionId == reception.OrginalClinicSectionId && x.User.Name == reception.SurgeryOneName).FirstOrDefault().Guid;
                                surgeryDoctor.SurgeryId = surgery.Guid;
                                surgeryDoctor.DoctorRoleId = surgeryOneId;
                                surgery.SurgeryDoctors.Add(surgeryDoctor);
                            }
                            else
                            {
                                DoctorViewModel doc = new();
                                doc.ClinicSectionId = reception.OrginalClinicSectionId;
                                doc.Guid = Guid.NewGuid();

                                doc.User = new UserInformationViewModel()
                                {
                                    ClinicSectionId = reception.OrginalClinicSectionId,
                                    Guid = doc.Guid,
                                    Name = reception.SurgeryOneName,
                                    UserName = "123",
                                    UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                    Pass1 = "slsl",

                                };
                                surgeryDoctor.Doctor = doc;
                                surgeryDoctor.DoctorId = reception.SurgeryOne = doc.Guid;
                                surgeryDoctor.SurgeryId = surgery.Guid;
                                surgeryDoctor.Guid = Guid.NewGuid();
                                surgeryDoctor.DoctorRoleId = surgeryOneId;
                                surgery.SurgeryDoctors.Add(surgeryDoctor);
                            }
                        }
                        else
                        {

                            SurgeryDoctorViewModel surgeryDoctor = new();
                            surgeryDoctor.DoctorId = reception.SurgeryOne;
                            surgeryDoctor.SurgeryId = surgery.Guid;
                            surgeryDoctor.Guid = Guid.NewGuid();
                            surgeryDoctor.DoctorRoleId = surgeryOneId;
                            surgery.SurgeryDoctors.Add(surgeryDoctor);
                        }
                        if (reception.DispatcherDoctorName != null)
                        {
                            if (reception.DispatcherDoctor == null)
                            {

                                SurgeryDoctorViewModel surgeryDoctor = new();

                                if (_IDUNIT.doctor.CheckRepeatedNameAndSpeciallity(reception.DispatcherDoctorName, null, reception.OrginalClinicSectionId, false))
                                {
                                    surgeryDoctor.DoctorId = _unitOfWork.Doctor.Find(x => x.ClinicSectionId == reception.OrginalClinicSectionId && x.User.Name == reception.DispatcherDoctorName).FirstOrDefault().Guid;
                                    surgeryDoctor.SurgeryId = surgery.Guid;
                                    surgeryDoctor.DoctorRoleId = DispatcherDoctorId;
                                    surgery.SurgeryDoctors.Add(surgeryDoctor);
                                }
                                else if (reception.DispatcherDoctorName == reception.SurgeryOneName)
                                {
                                    surgeryDoctor.DoctorId = reception.SurgeryOne;
                                    surgeryDoctor.SurgeryId = surgery.Guid;
                                    surgeryDoctor.DoctorRoleId = DispatcherDoctorId;
                                    surgery.SurgeryDoctors.Add(surgeryDoctor);
                                }
                                else
                                {
                                    DoctorViewModel doc = new();
                                    doc.ClinicSectionId = reception.OrginalClinicSectionId;
                                    doc.Guid = Guid.NewGuid();

                                    doc.User = new UserInformationViewModel()
                                    {
                                        ClinicSectionId = reception.OrginalClinicSectionId,
                                        Guid = doc.Guid,
                                        Name = reception.DispatcherDoctorName,
                                        UserName = "123",
                                        UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                        Pass1 = "slsl",

                                    };
                                    surgeryDoctor.Doctor = doc;
                                    surgeryDoctor.DoctorId = doc.Guid;
                                    surgeryDoctor.SurgeryId = surgery.Guid;
                                    surgeryDoctor.Guid = Guid.NewGuid();
                                    surgeryDoctor.DoctorRoleId = DispatcherDoctorId;
                                    surgery.SurgeryDoctors.Add(surgeryDoctor);
                                }
                            }
                            else
                            {
                                SurgeryDoctorViewModel surgeryDoctor = new();
                                surgeryDoctor.DoctorId = reception.DispatcherDoctor;
                                surgeryDoctor.SurgeryId = surgery.Guid;
                                surgeryDoctor.Guid = Guid.NewGuid();
                                surgeryDoctor.DoctorRoleId = DispatcherDoctorId;
                                surgery.SurgeryDoctors.Add(surgeryDoctor);
                            }
                        }

                        reception.Surgeries = new List<SurgeryViewModel>() { surgery };
                        ReceptionServiceViewModel service = new()
                        {
                            Guid = Guid.NewGuid(),
                            ReceptionId = reception.Guid,
                            ServiceId = reception.ServiceId,
                            Number = reception.ServiceNumber ?? 1,
                            Price = _unitOfWork.Services.Get(reception.ServiceId.Value).Price,
                            StatusId = unpaidId
                        };
                        reception.ReceptionServices = new List<ReceptionServiceViewModel>() { service };
                        reception.PaymentStatusId = unpaidId;
                        //HumanResourceSalary humanResourceSallary = new HumanResourceSalary()
                        //{
                        //    BeginDate = DateTime.Now,
                        //    CreateDate = DateTime.Now,
                        //    CreateUserId = reception.CreatedUserId,
                        //    Guid = Guid.NewGuid(),
                        //    HumanResourceId = reception.Surgeries.FirstOrDefault().SurgeryDoctors.FirstOrDefault().DoctorId,
                        //    PaymentStatusId = unpaidId,
                        //    Salary = _unitOfWork.Services.Get(reception.ReceptionServices.FirstOrDefault().ServiceId ?? Guid.Empty).DoctorWage,
                        //    WorkTime = 1,
                        //    SalaryTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Wage", "SalaryType"),
                        //    SurgeryId = reception.Surgeries.FirstOrDefault().Guid,
                        //    EndDate = DateTime.Now,
                        //    ExtraSalary = 0

                        //};
                        HumanResource humanResource = _unitOfWork.HumanResources.Get(reception.Surgeries.FirstOrDefault().SurgeryDoctors.FirstOrDefault().DoctorId ?? Guid.Empty);
                        if (humanResource == null)
                        {
                            humanResource = new HumanResource()
                            {
                                Guid = reception.Surgeries.FirstOrDefault().SurgeryDoctors.FirstOrDefault().DoctorId ?? Guid.Empty,
                                RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                HumanResourceSalaries = new List<HumanResourceSalary>()
                            };
                            //humanResource.HumanResourceSalaries.Add(humanResourceSallary);
                            _unitOfWork.HumanResources.Add(humanResource);
                        }
                        else
                        {
                            //_unitOfWork.HumanResourceSalaries.Add(humanResourceSallary);
                        }

                    }
                    else if (reception.PurposeName == "Service")
                    {
                        ReceptionServiceViewModel service = new()
                        {
                            Guid = Guid.NewGuid(),
                            ReceptionId = reception.Guid,
                            ServiceId = reception.ServiceId,
                            Number = reception.ServiceNumber ?? 1,
                            Price = _unitOfWork.Services.Get(reception.ServiceId.Value).Price,
                            StatusId = unpaidId,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = reception.CreatedUserId
                        };

                        if (reception.ReceptionServices == null)
                            reception.ReceptionServices = new List<ReceptionServiceViewModel>();

                        reception.ReceptionServices.Add(service);
                        reception.PaymentStatusId = unpaidId;
                    }

                    reception.PatientImages = new List<PatientImageViewModel>();

                    if (reception.MainAttachments != null && reception.MainAttachments.Any())
                    {
                        var attachmentTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("MainAttachment", "AttachmentType");
                        foreach (var attachment in reception.MainAttachments)
                        {
                            var att = fileAttachments.UploadFiles(attachment, reception.RootPath, reception.Guid, patientId, attachmentTypeId);
                            if (att != null)
                                reception.PatientImages.Add(att);
                        }
                    }

                    if (reception.OtherAttachments != null && reception.OtherAttachments.Any())
                    {
                        var attachmentTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("OtherAttachment", "AttachmentType");
                        foreach (var attachment in reception.OtherAttachments)
                        {
                            var att = fileAttachments.UploadFiles(attachment, reception.RootPath, reception.Guid, patientId, attachmentTypeId);
                            if (att != null)
                                reception.PatientImages.Add(att);
                        }
                    }

                    if (reception.PoliceReport != null && reception.PoliceReport.Any())
                    {
                        var attachmentTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("PoliceReport", "AttachmentType");
                        foreach (var attachment in reception.PoliceReport)
                        {
                            var att = fileAttachments.UploadFiles(attachment, reception.RootPath, reception.Guid, patientId, attachmentTypeId);
                            if (att != null)
                                reception.PatientImages.Add(att);
                        }
                    }

                    if (reception.PatientImages.Count == 0)
                        reception.PatientImages = null;

                    if (newPatient)
                        reception.Patient.User = Common.ConvertModels<UserInformationViewModel, PatientViewModel>.convertModels(reception.Patient);
                    Reception Reception2 = ConvertViewModelToModel(reception);
                    _unitOfWork.Receptions.Add(Reception2);
                    _unitOfWork.Complete();

                    if (reception.RoomBedId != null && reception.RoomBedId != Guid.Empty)
                        _IDUNIT.roomBed.UpdateRoomStatus(roomBed.RoomId.Value);

                    return Reception2.Guid.ToString();
                }


            }
            catch (Exception ex)
            {
                if (reception.PatientImages != null && reception.PatientImages.Any())
                {
                    fileAttachments.DeleteAllFiles(reception.PatientImages, reception.RootPath);
                }

                throw ex;
            }
        }

        public IEnumerable<ReceptionDoctorViewModel> GetReceptionDoctor(Guid receptionId)
        {
            var hosp = _unitOfWork.ReceptionDoctor.GetReceptionDoctor(receptionId);
            var result = ConvertModelsLists(hosp);
            return result;
        }

        public async Task<string> AddReceptionForReserve(ReserveDetailViewModel viewModel)
        {
            var reserveDetail = _unitOfWork.ReserveDetails.Get(viewModel.Guid);
            DateTime today = DateTime.Now;

            reserveDetail.StatusId = viewModel.StatusId;

            _unitOfWork.ReserveDetails.UpdateState(reserveDetail);

            var serviceId = _unitOfWork.Services.GetServiceByName(null, "DoctorVisit")?.Guid;

            Reception reception = new Reception
            {
                Guid = Guid.NewGuid(),
                ReceptionNum = _IDUNIT.patientReception.GetLatestReceptionInvoiceNum(viewModel.ClinicSectionId),
                ReceptionDate = today,
                ClinicSectionId = viewModel.ClinicSectionId,
                ReceptionTypeId = _IDUNIT.baseInfo.GetIdByNameAndType("VisitReception", "ReceptionType"),
                PatientId = reserveDetail.PatientId,
                Discount = 0,
                CreatedUserId = viewModel.UserId,
                CreatedDate = today,
                StatusId = _IDUNIT.baseInfo.GetBaseInfoGeneralByName("Visiting"),
                ReserveDetailId = viewModel.Guid,
                PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus")
            };

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(viewModel.OriginalClinicSectionId, "VisitPrice").FirstOrDefault();

            ReceptionService service = new()
            {
                ServiceId = serviceId,
                Number = 1,
                Price = decimal.Parse(sval?.SValue ?? "0"),
                StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus"),
                CreatedDate = today,
                ServiceDate = today,
                CreatedUserId = viewModel.UserId
            };

            if (serviceId == null || serviceId == Guid.Empty)
            {
                service.Service = new Service
                {
                    Name = "DoctorVisit",
                    Price = 1,
                    TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Other", "ServiceType")

                };
            }

            reception.ReceptionServices = new List<ReceptionService>()
            {
                service
            };

            _unitOfWork.Receptions.Add(reception);
            _unitOfWork.Complete();

            _IDUNIT.visit.UpdateReceptionNums(reserveDetail.DoctorId ?? Guid.Empty);
            return reception.Guid.ToString();
        }

        public async Task<decimal?> GetReceptionRemByReserveDetailId(Guid reserveDetailId, Guid clinicSectionId)
        {
            var reception = _unitOfWork.Receptions.GetReceptionWithServiceByReserveDetailId(reserveDetailId);

            if (reception.ReceptionServices == null || !reception.ReceptionServices.Any())
            {
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "VisitPrice").FirstOrDefault();

                return -decimal.Parse(sval?.SValue ?? "0");
            }

            var result = reception.ReceptionServices.Select(p => (p.ReceptionServiceReceiveds.Where(s => !s.AmountStatus.Value).Sum(x => x.Amount.Value) - p.ReceptionServiceReceiveds.Where(s => s.AmountStatus.Value).Sum(x => x.Amount.Value))
                         - ((p.Number * p.Price) - (p.Discount.GetValueOrDefault(0)))
                         ).Sum();

            return result;
        }

        public void AddNewReceptionTemperature(ReceptionTemperatureViewModel reception)
        {
            try
            {
                ReceptionTemperature rt = Common.ConvertModels<ReceptionTemperature, ReceptionTemperatureViewModel>.convertModels(reception);
                _unitOfWork.ReceptionTemperature.Add(rt);
                _unitOfWork.Complete();
            }
            catch (Exception e) { throw e; }
        }

        public void UpdateReceptionCleareance(ReceptionViewModel reception)
        {
            try
            {
                Reception rt = Common.ConvertModels<Reception, ReceptionViewModel>.convertModels(reception);
                _unitOfWork.Receptions.UpdateReceptionCleareance(rt);

            }
            catch (Exception e) { throw e; }
        }

        public void UpdateReceptionChiefComplaint(ReceptionViewModel reception)
        {
            try
            {
                Reception rt = Common.ConvertModels<Reception, ReceptionViewModel>.convertModels(reception);
                _unitOfWork.Receptions.UpdateReceptionChiefComplaint(rt);

            }
            catch (Exception e) { throw e; }
        }

        public string DischargePatient(Guid id, Guid userId, bool confirm)
        {
            var reception = _unitOfWork.Receptions.Get(id);
            var dirty = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Dirty", "RoomStatus");

            int? unpaidId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");

            if (reception.PaymentStatusId == unpaidId && !confirm)
                return "Unpaid";

            var date = DateTime.Now;

            reception.ModifiedDate = date;
            reception.ExitDate = date;
            reception.ModifiedUserId = userId;
            reception.Discharge = true;
            reception.ClearanceTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Clearance", "ClearanceType");

            _unitOfWork.Receptions.UpdateState(reception);

            var roomBeds = _unitOfWork.ReceptionRoomBeds.GetReceptionRoomBedForDischarge(id);
            foreach (var item in roomBeds)
            {
                item.ExitDate = date;
                item.ModifiedDate = date;
                item.ModifiedUserId = userId;

                item.RoomBed.StatusId = dirty;

                _unitOfWork.ReceptionRoomBeds.UpdateState(item);
                _unitOfWork.RoomBeds.UpdateState(item.RoomBed);
            }

            _unitOfWork.Complete();

            return "";
        }

        public IEnumerable<PatientReceptionViewModel> GetAllReceptionsByClinicSection(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo, Guid receptionId, int status)
        {
            try
            {
                if (periodId != (int)Periods.FromDateToDate)
                {
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                }
                IEnumerable<Reception> PatientReceptionDtos;


                if (status == (int)DischargeType.NotDischarge)
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllReceptionsByClinicSection(clinicSectionId, DateFrom, DateTo, receptionId, p => p.Discharge == null || !p.Discharge.Value);
                }
                else if (status == (int)DischargeType.Discharge)
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllReceptionsByClinicSection(clinicSectionId, DateFrom, DateTo, receptionId, p => p.Discharge != null && p.Discharge.Value);
                }
                else
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllReceptionsByClinicSection(clinicSectionId, DateFrom, DateTo, receptionId);
                }

                List<PatientReceptionViewModel> PatientReception = ConvertReceptionToPatientReceptionLists(PatientReceptionDtos).OrderByDescending(x => x.InvoiceDate).ToList();
                Indexing<PatientReceptionViewModel> indexing = new();
                return indexing.AddIndexing(PatientReception);
            }
            catch (Exception e) { throw e; }
        }


        public IEnumerable<PatientReceptionViewModel> GetAllReceptionsForSelectRoomBed(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo, Guid receptionId, int status)
        {
            try
            {
                if (periodId != (int)Periods.FromDateToDate)
                {
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                }
                List<Reception> PatientReceptionDtos = new();


                if (status == (int)DischargeType.NotDischarge)
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllReceptionsForSelectRoomBed(clinicSectionId, DateFrom, DateTo, receptionId, p => p.Discharge == null || !p.Discharge.Value).ToList();
                }
                else if (status == (int)DischargeType.Discharge)
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllReceptionsForSelectRoomBed(clinicSectionId, DateFrom, DateTo, receptionId, p => p.Discharge != null && p.Discharge.Value).ToList();
                }
                else
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllReceptionsForSelectRoomBed(clinicSectionId, DateFrom, DateTo, receptionId).ToList();
                }

                List<PatientReceptionViewModel> PatientReception = ConvertReceptionToPatientReceptionLists(PatientReceptionDtos).OrderByDescending(x => x.InvoiceDate).ToList();
                Indexing<PatientReceptionViewModel> indexing = new();
                return indexing.AddIndexing(PatientReception);
            }
            catch (Exception e) { throw e; }
        }


        public IEnumerable<HospitalPatientReportResultViewModel> GetAllReceptionsForHospitalPatients(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo, int status)
        {
            try
            {
                if (periodId != (int)Periods.FromDateToDate)
                {
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                }
                List<Reception> PatientReceptionDtos = new();


                if (status == (int)DischargeType.NotDischarge)
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllAllHospitalPatientReport(clinicSectionId, DateFrom, DateTo, p => p.Discharge == null || !p.Discharge.Value).ToList();
                }
                else if (status == (int)DischargeType.Discharge)
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllAllHospitalPatientReport(clinicSectionId, DateFrom, DateTo, p => p.Discharge != null && p.Discharge.Value).ToList();
                }
                else
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetAllAllHospitalPatientReport(clinicSectionId, DateFrom, DateTo).ToList();
                }

                List<HospitalPatientReportResultViewModel> PatientReception = PatientReceptionDtos.GroupBy(g => g.Guid).Select(s => new HospitalPatientReportResultViewModel
                {
                    Guid = s.Key,
                    PatientName = s.FirstOrDefault()?.Patient?.User?.Name ?? "",
                    Age = s.FirstOrDefault().Patient.DateOfBirth.GetAge()?.ToString() ?? "",
                    Kind = s.FirstOrDefault()?.ReceptionServices?.FirstOrDefault(w => w.Service.Type.Name == "Operation")?.Service?.Name ?? "",
                    DoctorName = s.FirstOrDefault()?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(w => w.DoctorRole.Name == "Surgery1")?.Doctor?.User?.Name ?? "",
                    RoomId = $"{s.FirstOrDefault()?.ReceptionRoomBeds?.LastOrDefault()?.RoomBed?.Room?.Name ?? string.Empty}|{s.FirstOrDefault()?.ReceptionRoomBeds?.LastOrDefault()?.RoomBed?.Name ?? string.Empty}",
                    SurgeryDate = s.FirstOrDefault()?.Surgeries?.FirstOrDefault()?.SurgeryDate,
                    ReceptionDate = s.FirstOrDefault()?.ReceptionDate.Value
                }).ToList();


                //foreach (var item in PatientReceptionDtos)
                //{
                //    var rec = new HospitalPatientReportResultViewModel
                //    {
                //        Guid = item.Guid,
                //        PatientName = item?.Patient?.User?.Name ?? "",
                //        Age = item.Patient.DateOfBirth.GetAge()?.ToString() ?? "", 
                //        Kind = item?.ReceptionServices?.FirstOrDefault(w => w.Service.Type.Name == "Operation")?.Service?.Name ?? "",
                //        DoctorName = item?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(w => w.DoctorRole.Name == "Surgery1")?.Doctor?.User?.Name ?? "",
                //        RoomId = $"{item?.ReceptionRoomBeds?.LastOrDefault(p => p.ExitDate == null)?.RoomBed?.Room?.Name ?? string.Empty}|{item?.ReceptionRoomBeds?.LastOrDefault(p => p.ExitDate == null)?.RoomBed?.Name ?? string.Empty}",
                //        SurgeryDate = item?.Surgeries?.FirstOrDefault()?.SurgeryDate
                //    };

                //    PatientReception.Add(rec);
                //}

                Indexing<HospitalPatientReportResultViewModel> indexing = new();
                return indexing.AddIndexing(PatientReception);
            }
            catch (Exception) { return new List<HospitalPatientReportResultViewModel>(); }
        }





        public ShowHospitalPatientReportResultViewModel AllHospitalPatientReport(HospitalPatientReportViewModel reportViewModel)
        {
            try
            {
                List<Reception> patientReceptionDtos = new();
                List<Reception> remPatientReceptionDtos = new();


                if (reportViewModel.status == (int)DischargeType.NotDischarge)
                {
                    patientReceptionDtos = _unitOfWork.Receptions.GetAllAllHospitalPatientReportStimul(reportViewModel.section.Id,
                        p => ((p.ReceptionDate <= reportViewModel.ToDate && p.ReceptionDate >= reportViewModel.FromDate) || (p.ExitDate <= reportViewModel.ToDate && p.ExitDate >= reportViewModel.FromDate)) && (p.Discharge == null || !p.Discharge.Value)).ToList();
                }
                else if (reportViewModel.status == (int)DischargeType.Discharge)
                {
                    patientReceptionDtos = _unitOfWork.Receptions.GetAllAllHospitalPatientReportStimul(reportViewModel.section.Id,
                        p => ((p.ReceptionDate <= reportViewModel.ToDate && p.ReceptionDate >= reportViewModel.FromDate) || (p.ExitDate <= reportViewModel.ToDate && p.ExitDate >= reportViewModel.FromDate)) && (p.Discharge != null && p.Discharge.Value)).ToList();
                }
                else
                {
                    patientReceptionDtos = _unitOfWork.Receptions.GetAllAllHospitalPatientReportStimul(reportViewModel.section.Id,
                        p => (p.ReceptionDate <= reportViewModel.ToDate && p.ReceptionDate >= reportViewModel.FromDate) || (p.ExitDate <= reportViewModel.ToDate && p.ExitDate >= reportViewModel.FromDate)).ToList();
                }

                List<HospitalPatientReportResultViewModel> patientReception = new();


                foreach (var item in patientReceptionDtos)
                {
                    var rec = new HospitalPatientReportResultViewModel
                    {
                        PatientName = item?.Patient?.User?.Name ?? "",
                        Age = item.Patient.DateOfBirth.GetAge()?.ToString() ?? "",
                        Kind = item?.ReceptionServices?.FirstOrDefault(w => w.Service.Type.Name == "Operation")?.Service?.Name ?? "",
                        DoctorName = item?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(w => w.DoctorRole.Name == "Surgery1")?.Doctor?.User?.Name ?? "",
                        RoomId = $"{item?.ReceptionRoomBeds?.OrderBy(o => o.CreatedDate)?.LastOrDefault()?.RoomBed?.Room?.Name ?? string.Empty} | {item?.ReceptionRoomBeds?.OrderBy(o => o.CreatedDate)?.LastOrDefault()?.RoomBed?.Name ?? string.Empty}",
                        DateSurgery = item?.Surgeries?.FirstOrDefault()?.SurgeryDate?.ToString("dd/MM/yyyy HH:mm") ?? "",
                        DateExit = item?.ExitDate?.ToString("dd/MM/yyyy HH:mm") ?? "",
                        DateReception = item?.ReceptionDate?.ToString("dd/MM/yyyy HH:mm") ?? "",

                    };

                    patientReception.Add(rec);
                }

                List<HospitalPatientCountReportViewModel> sugeries = patientReception/*.Where(p => !string.IsNullOrEmpty(p.Kind))*/.GroupBy(g => g.Kind)
                    .Select(s => new HospitalPatientCountReportViewModel
                    {
                        Kind = s.Key,
                        Count = s.Count().ToString("#,##")
                    }).ToList();



                remPatientReceptionDtos = _unitOfWork.Receptions.GetAllAllHospitalPatientReportStimul(reportViewModel.section.Id,
                    p => p.ExitDate == null).ToList();

                List<HospitalPatientReportResultViewModel> remPatientReception = new();

                foreach (var item in remPatientReceptionDtos)
                {
                    var rec = new HospitalPatientReportResultViewModel
                    {
                        PatientName = item?.Patient?.User?.Name ?? "",
                        Age = item.Patient.DateOfBirth.GetAge()?.ToString() ?? "",
                        Kind = item?.ReceptionServices?.FirstOrDefault(w => w.Service.Type.Name == "Operation")?.Service?.Name ?? "",
                        DoctorName = item?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(w => w.DoctorRole.Name == "Surgery1")?.Doctor?.User?.Name ?? "",
                        RoomId = $"{item?.ReceptionRoomBeds?.OrderBy(o => o.CreatedDate)?.LastOrDefault()?.RoomBed?.Room?.Name ?? string.Empty} | {item?.ReceptionRoomBeds?.OrderBy(o => o.CreatedDate)?.LastOrDefault()?.RoomBed?.Name ?? string.Empty}",
                        DateSurgery = item?.Surgeries?.FirstOrDefault()?.SurgeryDate?.ToString("dd/MM/yyyy HH:mm") ?? "",
                        DateReception = item?.ReceptionDate?.ToString("dd/MM/yyyy HH:mm") ?? "",

                    };

                    remPatientReception.Add(rec);
                }

                return new ShowHospitalPatientReportResultViewModel
                {
                    CustomPatients = patientReception,
                    RemPatients = remPatientReception,
                    SurgeryTypeCount = sugeries,
                    TotalDischarge = patientReception.Where(p => !string.IsNullOrWhiteSpace(p.DateExit)).Count().ToString("#,##"),
                    TotalNotDischarge = patientReception.Where(p => string.IsNullOrWhiteSpace(p.DateExit)).Count().ToString("#,##"),
                    TotalSurgery = patientReception.Where(p => !string.IsNullOrWhiteSpace(p.Kind)).Count().ToString("#,##"),
                };
            }
            catch (Exception)
            {
                return new ShowHospitalPatientReportResultViewModel
                {
                    CustomPatients = new List<HospitalPatientReportResultViewModel>()
                };
            }
        }


        public IEnumerable<PatientReceptionViewModel> GetReceptionsByClinicSectionForCash(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo, Guid receptionId, int paymentStatus)
        {
            try
            {
                int? unpaidId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
                int? paidId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");

                if (periodId != (int)Periods.FromDateToDate)
                {
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                }
                List<Reception> PatientReceptionDtos = new();
                if (paymentStatus == (int)PaymentStatus.Unpaid)
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetReceptionsByClinicSectionForCash(clinicSectionId, DateFrom, DateTo, receptionId, p => p.PaymentStatusId == unpaidId).ToList();
                }
                else if (paymentStatus == (int)PaymentStatus.Paid)
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetReceptionsByClinicSectionForCash(clinicSectionId, DateFrom, DateTo, receptionId, p => p.PaymentStatusId == paidId).ToList();
                }
                else
                {
                    PatientReceptionDtos = _unitOfWork.Receptions.GetReceptionsByClinicSectionForCash(clinicSectionId, DateFrom, DateTo, receptionId).ToList();
                }

                List<PatientReceptionViewModel> PatientReception = ConvertReceptionToPatientReceptionLists(PatientReceptionDtos).OrderByDescending(x => x.InvoiceDate).ToList();
                Indexing<PatientReceptionViewModel> indexing = new();
                return indexing.AddIndexing(PatientReception);
            }
            catch (Exception e) { return null; }
        }



        public IEnumerable<ReceptionForCashReportViewModel> GetAllReceptionsByClinicSectionForCashReport(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo, string status)
        {
            try
            {

                if (periodId != (int)Periods.FromDateToDate)
                {
                    DateFrom = DateTime.Now;
                    DateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                }
                IEnumerable<ReceptionForCashReport> PatientReceptionDtos;

                PatientReceptionDtos = _unitOfWork.Receptions.GetAllReceptionsByClinicSectionForCashReport(clinicSectionId, DateFrom, DateTo, status).OrderByDescending(a => a.ReceptionDate);

                IEnumerable<ReceptionForCashReportViewModel> MainReceptions = PatientReceptionDtos.GroupBy(g => g.ReceptionId).Select(s => new ReceptionForCashReportViewModel
                {
                    Guid = s.Key,
                    SurgeryName = s.FirstOrDefault(x => x.VType == "Surgery1")?.VName,
                    TempSurgeryWage = s.FirstOrDefault(x => x.VType == "Surgery1")?.Price ?? 0,
                    AnestheticName = s.FirstOrDefault(x => x.VType == "Anesthesiologist")?.VName,
                    TempAnestheticWage = s.FirstOrDefault(x => x.VType == "Anesthesiologist")?.Price ?? 0,
                    TempChilderenDoctorWage = s.FirstOrDefault(x => x.VType == "Pediatrician")?.Price ?? 0,
                    TempResidentWage = s.FirstOrDefault(x => x.VType == "Resident")?.Price ?? 0,
                    InvoiceNum = s.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ReceptionInvoiceNum))?.ReceptionInvoiceNum,
                    Date = s.FirstOrDefault(p => p.ReceptionDate != null)?.ReceptionDate,
                    PatientName = s.FirstOrDefault(p => p.VType == "Patient")?.VName,
                    Operation = s.FirstOrDefault(p => p.VType == "Operation")?.VName,
                    SurgeryId = s.FirstOrDefault(p => p.SurgeryId != null)?.SurgeryId,
                    TempTreatmentStaffWage = s.Where(x => x.VType == "TreatmentStaff").Sum(x => x.Price.GetValueOrDefault(0)),
                    TempSentinelCadre = s.Where(x => x.VType == "SentinelCadre").Sum(x => x.Price.GetValueOrDefault(0)),
                    TempPrematureCadres = s.Where(x => x.VType == "PrematureCadre").Sum(x => x.Price.GetValueOrDefault(0)),
                    //TempServicePrice = s.Where(x => x.VName != "DoctorWage" && (x.VType == "Operation" || x.VType == "Service")).Sum(x => x.Price.GetValueOrDefault(0)),
                    TempService = s.Where(x => x.VName != "DoctorWage" && x.VType == "Service").Sum(x => x.Price.GetValueOrDefault(0)),
                    TempOperationPrice = s.FirstOrDefault(x => x.VType == "Operation")?.Price ?? 0,
                    Description = s.FirstOrDefault()?.Description
                });



                //foreach (var guId in SurgeryGuids)
                //{
                //    //var ServicePrice = PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId).ServicePrice + Convert.ToDecimal(DoctorSalary);
                //    //var HumanResourceSalaryTreatmentIds = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceId != null && x.HumanResourceSalaryType == "TreatmentStaff").Select(a => a.HumanResourceId).Distinct();
                //    //var HumanResourceSalarySentinelIds = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceId != null && x.HumanResourceSalaryType == "SentinelCadre").Select(a => a.HumanResourceId).Distinct();
                //    //var HumanResourceSalaryPrematureIds = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceId != null && x.HumanResourceSalaryType == "PrematureCadre").Select(a => a.HumanResourceId).Distinct();
                //    //var HumanResourceSalaryTreatmentStaff = 0.0m;

                //    //HumanResourceSalaryTreatmentStaff = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceSalaryType == "TreatmentStaff" && HumanResourceSalaryTreatmentIds.Contains(x.HumanResourceId)).DistinctBy(x => x.HumanResourceId).Sum(x => x.HumanResourceSalary);

                //    //var HumanResourceSalarySentinelCadre = 0.0m;
                //    //HumanResourceSalarySentinelCadre = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceSalaryType == "SentinelCadre" && HumanResourceSalarySentinelIds.Contains(x.HumanResourceId)).DistinctBy(x => x.HumanResourceId).Sum(x => x.HumanResourceSalary);

                //    //var HumanResourceSalaryPrematureCadre = 0.0m;
                //    //HumanResourceSalaryPrematureCadre = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceSalaryType == "PrematureCadre" && HumanResourceSalaryPrematureIds.Contains(x.HumanResourceId)).DistinctBy(x => x.HumanResourceId).Sum(x => x.HumanResourceSalary);

                //    //var HospitalRemaining = ServicePrice - Convert.ToDecimal(DoctorSalary) - HumanResourceSalaryTreatmentStaff - HumanResourceSalarySentinelCadre - HumanResourceSalaryPrematureCadre - Convert.ToDecimal(AnethSalary) - Convert.ToDecimal(ResiSalary) - Convert.ToDecimal(PediaSalary);
                //    //MainReceptions.Add(new ReceptionForCashReportViewModel
                //    //{
                //    //    //AnestheticWage = AnethSalary.ToString("N0"),
                //    //    //SurgeryWage = DoctorSalary,
                //    //    //PatientName = PatientName,
                //    //    //Date = ReceptionDate,
                //    //    //InvoiceNum = ReceptionInvoiceNum,
                //    //    //Operation = ServiceName,
                //    //    //OperationPrice = ServicePrice.ToString("N0"),
                //    //    //TreatmentStaffWage = HumanResourceSalaryTreatmentStaff.ToString("N0"),
                //    //    //SentinelCadre = HumanResourceSalarySentinelCadre.ToString("N0"),
                //    //    //PrematureCadres = HumanResourceSalaryPrematureCadre.ToString("N0"),
                //    //    //ChilderenDoctorWage = PediaSalary.ToString("N0"),
                //    //    //ResidentWage = ResiSalary.ToString("N0"),
                //    //    //SurgeryId = SurgeryId,
                //    //    HospitalRemaining = HospitalRemaining.ToString("N0")
                //    //});
                //}

                //IEnumerable<Guid> SurgeryGuids = PatientReceptionDtos.Select(x => x.Guid).Distinct();
                //List<ReceptionForCashReportViewModel> MainReceptions = new List<ReceptionForCashReportViewModel>();

                //foreach (var guId in SurgeryGuids)
                //{

                //    //var PediaName = "";
                //    //try
                //    //{
                //    //    PediaName = PatientReceptionDtos.FirstOrDefault(x => x.DoctorType == "Pediatrician" && x.Guid == guId).DoctorName;
                //    //}
                //    //catch { }

                //    var PediaSalary = 0.0m;
                //    try
                //    {
                //        PediaSalary = PatientReceptionDtos.FirstOrDefault(x => x.DoctorType == "Pediatrician" && x.Guid == guId && x.DoctorId == x.HumanResourceId).HumanResourceSalary;
                //    }
                //    catch { }

                //    //var ResiName = "";
                //    //try
                //    //{
                //    //    ResiName = PatientReceptionDtos.FirstOrDefault(x => x.DoctorType == "Resident" && x.Guid == guId).DoctorName;
                //    //}
                //    //catch { }

                //    var ResiSalary = 0.0m;
                //    try
                //    {
                //        ResiSalary = PatientReceptionDtos.FirstOrDefault(x => x.DoctorType == "Resident" && x.Guid == guId && x.DoctorId == x.HumanResourceId).HumanResourceSalary;
                //    }
                //    catch { }


                //    var AnethName = "";
                //    try
                //    {
                //        AnethName = PatientReceptionDtos.FirstOrDefault(x => x.DoctorType == "Anesthesiologist" && x.Guid == guId).DoctorName;
                //    }
                //    catch { }


                //    var AnethSalary = 0.0m;
                //    try
                //    {
                //        AnethSalary = PatientReceptionDtos.FirstOrDefault(x => x.DoctorType == "Anesthesiologist" && x.Guid == guId && x.DoctorId == x.HumanResourceId).HumanResourceSalary;
                //    }
                //    catch { }

                //    var DoctorSalary = "0";
                //    try
                //    {
                //        DoctorSalary = DoctorSalary = PatientReceptionDtos.FirstOrDefault(x => x.DoctorType == "Surgery1" && x.Guid == guId && x.DoctorId == x.HumanResourceId).HumanResourceSalary.ToString("N0");

                //    }
                //    catch { }


                //    var DoctorName = PatientReceptionDtos.FirstOrDefault(x => x.DoctorType == "Surgery1" && x.Guid == guId).DoctorName;
                //    var Guid = guId;
                //    var PatientName = PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId).PatientName;
                //    var ReceptionDate = PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId).ReceptionDate;
                //    var ReceptionInvoiceNum = PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId).ReceptionInvoiceNum;
                //    var ServiceName = PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId).ServiceName;
                //    var ServicePrice = PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId).ServicePrice + Convert.ToDecimal(DoctorSalary);
                //    var HumanResourceSalaryTreatmentIds = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceId != null && x.HumanResourceSalaryType == "TreatmentStaff").Select(a => a.HumanResourceId).Distinct();
                //    var HumanResourceSalarySentinelIds = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceId != null && x.HumanResourceSalaryType == "SentinelCadre").Select(a => a.HumanResourceId).Distinct();
                //    var HumanResourceSalaryPrematureIds = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceId != null && x.HumanResourceSalaryType == "PrematureCadre").Select(a => a.HumanResourceId).Distinct();

                //    //var dd = PatientReceptionDtos.Where(x => HumanResourceSalaryIds.Contains(x.HumanResourceId));
                //    var HumanResourceSalaryTreatmentStaff = 0.0m;
                //    //IEnumerable<ReceptionForCashReport> PatientReceptionDtos2 = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceSalaryType == "TreatmentStaff" && HumanResourceSalaryTreatmentIds.Contains(x.HumanResourceId)).DistinctBy(x=>x.HumanResourceId);
                //    HumanResourceSalaryTreatmentStaff = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceSalaryType == "TreatmentStaff" && HumanResourceSalaryTreatmentIds.Contains(x.HumanResourceId)).DistinctBy(x => x.HumanResourceId).Sum(x => x.HumanResourceSalary);
                //    //foreach (var it in HumanResourceSalaryTreatmentIds)
                //    //{
                //    //    //HumanResourceSalaryTreatmentStaff = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceSalaryType == "TreatmentStaff").Sum(x=>x.HumanResourceSalary);
                //    //    HumanResourceSalaryTreatmentStaff = HumanResourceSalaryTreatmentStaff + PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId && x.HumanResourceId == it && x.HumanResourceSalaryType == "TreatmentStaff").HumanResourceSalary;
                //    //}
                //    //HumanResourceSalaryTreatmentStaff = HumanResourceSalaryTreatmentStaff - Convert.ToDecimal(DoctorSalary) - Convert.ToDecimal(AnethSalary) - Convert.ToDecimal(ResiSalary) - Convert.ToDecimal(PediaSalary);

                //    var HumanResourceSalarySentinelCadre = 0.0m;
                //    HumanResourceSalarySentinelCadre = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceSalaryType == "SentinelCadre" && HumanResourceSalarySentinelIds.Contains(x.HumanResourceId)).DistinctBy(x => x.HumanResourceId).Sum(x => x.HumanResourceSalary);

                //    //foreach (var it in HumanResourceSalaryIds)
                //    //{
                //    //    HumanResourceSalarySentinelCadre = HumanResourceSalarySentinelCadre + PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId && x.HumanResourceId == it && x.HumanResourceSalaryType == "SentinelCadre").HumanResourceSalary;
                //    //}
                //    //HumanResourceSalarySentinelCadre = HumanResourceSalarySentinelCadre - Convert.ToDecimal(DoctorSalary) - Convert.ToDecimal(AnethSalary) - Convert.ToDecimal(ResiSalary) - Convert.ToDecimal(PediaSalary);

                //    var HumanResourceSalaryPrematureCadre = 0.0m;
                //    HumanResourceSalaryPrematureCadre = PatientReceptionDtos.Where(x => x.Guid == guId && x.HumanResourceSalaryType == "PrematureCadre" && HumanResourceSalaryPrematureIds.Contains(x.HumanResourceId)).DistinctBy(x => x.HumanResourceId).Sum(x => x.HumanResourceSalary);

                //    //foreach (var it in HumanResourceSalaryIds)
                //    //{
                //    //    HumanResourceSalaryPrematureCadre = HumanResourceSalaryPrematureCadre + PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId && x.HumanResourceId == it && x.HumanResourceSalaryType == "SentinelCadre").HumanResourceSalary;
                //    //}
                //    //HumanResourceSalaryPrematureCadre = HumanResourceSalaryPrematureCadre - Convert.ToDecimal(DoctorSalary) - Convert.ToDecimal(AnethSalary) - Convert.ToDecimal(ResiSalary) - Convert.ToDecimal(PediaSalary);


                //    var SurgeryId = PatientReceptionDtos.FirstOrDefault(x => x.Guid == guId).SurgeryId;
                //    var HospitalRemaining = ServicePrice - Convert.ToDecimal(DoctorSalary) - HumanResourceSalaryTreatmentStaff - HumanResourceSalarySentinelCadre - HumanResourceSalaryPrematureCadre - Convert.ToDecimal(AnethSalary) - Convert.ToDecimal(ResiSalary) - Convert.ToDecimal(PediaSalary);
                //    MainReceptions.Add(new ReceptionForCashReportViewModel
                //    {
                //        AnestheticName = AnethName,
                //        SurgeryName = DoctorName,
                //        Guid = Guid,
                //        AnestheticWage = AnethSalary.ToString("N0"),
                //        SurgeryWage = DoctorSalary,
                //        PatientName = PatientName,
                //        Date = ReceptionDate,
                //        InvoiceNum = ReceptionInvoiceNum,
                //        Operation = ServiceName,
                //        OperationPrice = ServicePrice.ToString("N0"),
                //        TreatmentStaffWage = HumanResourceSalaryTreatmentStaff.ToString("N0"),
                //        SentinelCadre = HumanResourceSalarySentinelCadre.ToString("N0"),
                //        PrematureCadres = HumanResourceSalaryPrematureCadre.ToString("N0"),
                //        ChilderenDoctorWage = PediaSalary.ToString("N0"),
                //        ResidentWage = ResiSalary.ToString("N0"),
                //        SurgeryId = SurgeryId,
                //        HospitalRemaining = HospitalRemaining.ToString("N0")
                //    });
                //}

                //List<ReceptionForCashReportViewModel> PatientReception = Common.ConvertModels<ReceptionForCashReportViewModel, ReceptionForCashReport>.convertModelsLists(PatientReceptionDtos);
                return MainReceptions;
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<ReceptionPatientNameViewModel> GetReceptionPatientName()
        {
            IEnumerable<User> patient = _unitOfWork.Receptions.GetReceptionPatient();
            List<ReceptionPatientNameViewModel> patientView = Common.ConvertModels<ReceptionPatientNameViewModel, User>.convertModelsLists(patient);
            Indexing<ReceptionPatientNameViewModel> indexing = new();
            return indexing.AddIndexing(patientView);
        }

        public IEnumerable<ReceptionTemperatureViewModel> GetAllReceptionTemperature(Guid receptionId)
        {
            IEnumerable<ReceptionTemperature> RT = _unitOfWork.ReceptionTemperature.GetAllReceptionTemperatures(receptionId);
            List<ReceptionTemperatureViewModel> RTView = Common.ConvertModels<ReceptionTemperatureViewModel, ReceptionTemperature>.convertModelsLists(RT);
            Indexing<ReceptionTemperatureViewModel> indexing = new();
            return indexing.AddIndexing(RTView);
        }



        public ReceptionViewModel GetReception(Guid ReceptionId)
        {
            try
            {
                Reception Receptiongu = _unitOfWork.Receptions.GetReception(ReceptionId);
                return ConvertModel(Receptiongu);
            }
            catch (Exception) { return null; }
        }

        public ReceptionViewModel GetReceptionOnly(Guid receptionId)
        {
            try
            {
                Reception Receptiongu = _unitOfWork.Receptions.Get(receptionId);
                return Common.ConvertModels<ReceptionViewModel, Reception>.convertModels(Receptiongu);
            }
            catch (Exception) { return null; }
        }


        public string GetServerVisitNum(Guid receptionId)
        {
            try
            {
                string Receptiongu = _unitOfWork.Receptions.GetServerVisitNum(receptionId);
                return Receptiongu;
            }
            catch (Exception e) { throw e; }
        }



        public PieChartViewModel GetReceptionCount(Guid userId)
        {
            try
            {
                IEnumerable<DateTime> Receptiongu = _unitOfWork.Receptions.GetReceptionCount(userId);


                List<PieChartModel> pie = new List<PieChartModel>();

                pie.Add(new PieChartModel
                {
                    Label = "Today",
                    Value = Receptiongu.Where(a => a.Date == DateTime.Now.Date).Count()
                });

                pie.Add(new PieChartModel
                {
                    Label = "Yesterday",
                    Value = Receptiongu.Where(a => a.Date == DateTime.Now.AddDays(-1).Date).Count()
                });

                var culture = CultureInfo.CurrentCulture;
                var diff = DateTime.Now.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
                if (diff < 0)
                    diff += 7;
                //d = d.AddDays(-diff).Date;


                pie.Add(new PieChartModel
                {
                    Label = "Week",
                    Value = Receptiongu.Where(a => a.Date >= DateTime.Now.AddDays(-diff).Date && a.Date <= DateTime.Now.Date).Count()
                });

                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                pie.Add(new PieChartModel
                {
                    Label = "Mounth",
                    Value = Receptiongu.Where(a => a.Date >= firstDayOfMonth.Date && a.Date <= DateTime.Now.Date).Count()
                });


                var firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);

                pie.Add(new PieChartModel
                {
                    Label = "Year",
                    Value = Receptiongu.Where(a => a.Date >= firstDayOfYear.Date && a.Date <= DateTime.Now.Date).Count()
                });

                pie.Add(new PieChartModel
                {
                    Label = "All",
                    Value = Receptiongu.Count()
                });

                PieChartViewModel chart = new PieChartViewModel
                {
                    Labels = pie.Select(a=>a.Label).ToArray(),
                    Value = pie.Select(a => Convert.ToInt32(a.Value)).ToArray()
                };

                return chart;
            }
            catch (Exception e) { throw e; }
        }

        

        // Begin Convert 
        public ReceptionViewModel ConvertModel(Reception reception)
        {


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, ReceptionViewModel>();
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Reception, ReceptionViewModel>(reception);
        }

        public Reception ConvertViewModelToModel(ReceptionViewModel receptionViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionViewModel, Reception>();
                cfg.CreateMap<ReceptionAmbulanceViewModel, ReceptionAmbulance>();
                cfg.CreateMap<HospitalViewModel, Hospital>();
                cfg.CreateMap<ServiceViewModel, Service>();
                cfg.CreateMap<ReceptionClinicSectionViewModel, ReceptionClinicSection>();
                cfg.CreateMap<EmergencyViewModel, Emergency>();
                cfg.CreateMap<PatientViewModel, Patient>();
                cfg.CreateMap<UserInformationViewModel, User>();
                cfg.CreateMap<DoctorViewModel, Doctor>();
                cfg.CreateMap<BaseInfoViewModel, BaseInfo>();
                cfg.CreateMap<SurgeryViewModel, Surgery>();
                cfg.CreateMap<SurgeryDoctorViewModel, SurgeryDoctor>();
                cfg.CreateMap<ReceptionRoomBedViewModel, ReceptionRoomBed>();
                cfg.CreateMap<PatientImageViewModel, PatientImage>();
                cfg.CreateMap<ReceptionServiceViewModel, ReceptionService>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceptionViewModel, Reception>(receptionViewModel);
        }

        public List<ReceptionDoctorViewModel> ConvertModelsLists(IEnumerable<ReceptionDoctor> Receptions)
        {
            List<ReceptionDoctorViewModel> ReceptionDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Doctor, DoctorViewModel>()
                .ForMember(a => a.User, b => b.Ignore());
                //cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<ReceptionDoctor, ReceptionDoctorViewModel>()
                .ForMember(a => a.DoctorRoleName, b => b.MapFrom(c => c.DoctorRole.Name ?? ""));
            });

            IMapper mapper = config.CreateMapper();
            ReceptionDtoList = mapper.Map<IEnumerable<ReceptionDoctor>, List<ReceptionDoctorViewModel>>(Receptions);
            return ReceptionDtoList;
        }

        public List<Reception> ConvertViewModelsLists(IEnumerable<ReceptionViewModel> Receptions)
        {
            List<Reception> ReceptionDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionViewModel, Reception>();
                cfg.CreateMap<ReceptionAmbulanceViewModel, ReceptionAmbulance>();
                cfg.CreateMap<ReceptionClinicSectionViewModel, ReceptionClinicSection>();
                cfg.CreateMap<EmergencyViewModel, Emergency>();
                cfg.CreateMap<PatientViewModel, Patient>();
                cfg.CreateMap<UserInformationViewModel, User>();
            });

            IMapper mapper = config.CreateMapper();
            ReceptionDtoList = mapper.Map<IEnumerable<ReceptionViewModel>, List<Reception>>(Receptions);
            return ReceptionDtoList;
        }

        public List<PatientReceptionViewModel> ConvertReceptionToPatientReceptionLists(IEnumerable<Reception> Receptions)
        {
            List<PatientReceptionViewModel> ReceptionDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, PatientReceptionViewModel>()
                .ForMember(a => a.PurposeName, b => b.MapFrom(c => c.Purpose.Name ?? ""))
                .ForMember(a => a.InvoiceDate, b => b.MapFrom(c => c.ReceptionDate))
                .ForMember(a => a.InvoiceNum, b => b.MapFrom(c => c.ReceptionNum))
                .ForMember(a => a.RoomBedName, b => b.MapFrom(c => $"{c.ReceptionRoomBeds.LastOrDefault(p => p.ExitDate == null).RoomBed.Room.Name ?? string.Empty}|{c.ReceptionRoomBeds.LastOrDefault(p => p.ExitDate == null).RoomBed.Name ?? string.Empty}"))
                .ForMember(a => a.RoomBedId, b => b.MapFrom(c => c.ReceptionRoomBeds.LastOrDefault(p => p.ExitDate == null).RoomBed.Guid))
                .ForMember(a => a.ReceptionStatus, b => b.MapFrom(c => c.PaymentStatus.Name))
                .ForMember(a => a.TotalRecivedAmount, b => b.MapFrom(c => c.ReceptionServices.Sum(p => p.ReceptionServiceReceiveds.Where(p => !p.AmountStatus.Value).Sum(p => p.Amount.Value))))
                .ForMember(a => a.TotalReturnedAmount, b => b.MapFrom(c => c.ReceptionServices.Sum(p => p.ReceptionServiceReceiveds.Where(p => p.AmountStatus.Value).Sum(p => p.Amount.Value))))
                .ForMember(a => a.TotalServiceAmount, b => b.MapFrom(c => c.ReceptionServices.Sum(p => p.Number * p.Price - (p.Discount == null ? 0 : p.Discount.Value))))
                .ForMember(a => a.Insurance, b => b.MapFrom(c => (c.ReceptionInsurances.FirstOrDefault().ReceptionInsuranceReceiveds.Sum(z => (z.AmountStatus.GetValueOrDefault(false)) ? -z.Amount : z.Amount)) - (c.ReceptionServices.Sum(d => d.ReceptionServiceReceiveds.Where(z => z.ReceptionInsuranceId != null).Sum(v => v.Amount)))))
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<ReceptionServiceReceived, ReceptionServiceReceivedViewModel>();
                cfg.CreateMap<ReceptionService, ReceptionServiceViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            ReceptionDtoList = mapper.Map<IEnumerable<Reception>, List<PatientReceptionViewModel>>(Receptions);
            return ReceptionDtoList;
        }

        
    }
}
