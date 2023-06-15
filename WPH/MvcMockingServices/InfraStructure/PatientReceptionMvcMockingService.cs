using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
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
using WPH.Models.PatientReceptionAnalysis;
using WPH.Models.CustomDataModels.PatientReceptionReceived;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.PatientImage;
using WPH.Models.Reception;
using WPH.Models.ReceptionDoctor;
using WPH.MvcMockingServices.Interface;
using WPH.Models.ReceptionClinicSection;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class PatientReceptionMvcMockingService : IPatientReceptionMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public PatientReceptionMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/PatientReception/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public Guid AddOrUpdate(ReceptionViewModel patientReception)
        {
            DateTime dt = DateTime.Now;
            dt = dt.AddMilliseconds(-dt.Millisecond);
            dt = dt.AddSeconds(-dt.Second);

            if (patientReception.Guid != Guid.Empty)
            {
                patientReception.ModifiedDate = dt;
                patientReception.ModifiedUserId = patientReception.UserId;

                return CheckPatientAndDoctor(patientReception, false);
            }
            else
            {
                patientReception.Guid = Guid.NewGuid();
                patientReception.CreatedDate = dt;
                patientReception.CreatedUserId = patientReception.UserId;
                patientReception.ReceptionDate = dt;

                AnalysisResultMasterViewModel master = new AnalysisResultMasterViewModel();
                patientReception.AnalysisResultMasters = new List<AnalysisResultMasterViewModel>();

                master.InvoiceDate = dt;
                master.CreatedDate = dt;
                master.CreatedUserId = patientReception.UserId;

                patientReception.AnalysisResultMasters.Add(master);

                Guid receptionId = CheckPatientAndDoctor(patientReception, true);

                return receptionId;
            }
        }


        public Guid CheckPatientAndDoctor(ReceptionViewModel patientReception, bool NewReception)
        {
            DateTime dt = DateTime.Now;
            dt = dt.AddMilliseconds(-dt.Millisecond);
            dt = dt.AddSeconds(-dt.Second);

            if (patientReception.PatientReceptionAnalyses != null)
            {
                foreach (var pra in patientReception.PatientReceptionAnalyses)
                {
                    pra.CreatedDate = dt;
                    pra.CreatedUserId = patientReception.UserId;
                    pra.ReceptionId = patientReception.Guid;
                }
            }

            var exist_patient = _unitOfWork.Patients.GetPatientByName(patientReception.Patient.Name, patientReception.OrginalClinicSectionId);
            if (exist_patient != null)
            {
                patientReception.PatientId = exist_patient.Guid;

                var birthday = new DateTime(int.Parse(patientReception.Patient.DateOfBirthYear ?? "1"), int.Parse(patientReception.Patient.DateOfBirthMonth ?? "1"), int.Parse(patientReception.Patient.DateOfBirthDay ?? "1"));
                exist_patient.DateOfBirth = (birthday.Year < 1753) ? DateTime.Now : birthday;

                exist_patient.User.PhoneNumber = patientReception.Patient.PhoneNumber;
                exist_patient.User.GenderId = patientReception.Patient.GenderId;
                _unitOfWork.Patients.UpdateState(exist_patient);
                _unitOfWork.Users.UpdateState(exist_patient.User);

                patientReception.Patient = null;
                if (!string.IsNullOrWhiteSpace(patientReception.Doctor?.Name))
                {
                    if (_idunit.doctor.CheckRepeatedNameAndSpeciallity(patientReception.Doctor.Name, patientReception.Doctor.SpecialityId, patientReception.OrginalClinicSectionId, true))
                    {
                        patientReception.DoctorId = _unitOfWork.Doctor.Find(x => x.ClinicSectionId == patientReception.OrginalClinicSectionId && x.User.Name == patientReception.Doctor.Name).FirstOrDefault().Guid;
                        patientReception.Doctor = null;

                        if (!NewReception)
                            _idunit.patientReception.UpdatePatientReception(patientReception, false, false);
                        else
                            _idunit.patientReception.AddNewPatientReception(patientReception, false, false);
                        return patientReception.Guid;
                    }
                    else
                    {
                        patientReception.Doctor.Guid = Guid.NewGuid();
                        patientReception.Doctor.UserName = _idunit.patient.RandomString(10);
                        patientReception.Doctor.Pass1 = "123";
                        patientReception.Doctor.ClinicSectionId = patientReception.OrginalClinicSectionId;

                        if (!NewReception)
                            _idunit.patientReception.UpdatePatientReception(patientReception, false, true);
                        else
                            _idunit.patientReception.AddNewPatientReception(patientReception, false, true);
                        return patientReception.Guid;
                    }
                }
                else
                {
                    patientReception.Doctor = null;
                    patientReception.DoctorId = Guid.Empty;
                    if (!NewReception)
                        _idunit.patientReception.UpdatePatientReception(patientReception, false, true);
                    else
                        _idunit.patientReception.AddNewPatientReception(patientReception, false, true);
                    return patientReception.Guid;
                }
            }
            else
            {
                if (!patientReception.HospitalReception ?? false)
                {
                    patientReception.Patient.UserName = _idunit.patient.RandomString(10);
                    patientReception.Patient.Pass1 = "123";
                    patientReception.Patient.ClinicSectionId = patientReception.OrginalClinicSectionId;
                    patientReception.Patient.Guid = Guid.NewGuid();
                    patientReception.PatientId = patientReception.Patient.Guid;
                    var birthday = new DateTime(int.Parse(patientReception.Patient.DateOfBirthYear ?? "1"), int.Parse(patientReception.Patient.DateOfBirthMonth ?? "1"), int.Parse(patientReception.Patient.DateOfBirthDay ?? "1"));
                    patientReception.Patient.DateOfBirth = (birthday.Year < 1753) ? DateTime.Now : birthday;
                }

                if (!string.IsNullOrWhiteSpace(patientReception.Doctor?.Name))
                {
                    if (_idunit.doctor.CheckRepeatedNameAndSpeciallity(patientReception.Doctor.Name, patientReception.Doctor.SpecialityId, patientReception.OrginalClinicSectionId, true))
                    {
                        patientReception.DoctorId = _unitOfWork.Doctor.Find(x => x.ClinicSectionId == patientReception.OrginalClinicSectionId && x.User.Name == patientReception.Doctor.Name).FirstOrDefault().Guid;
                        patientReception.Doctor = null;
                        if (!NewReception)
                            _idunit.patientReception.UpdatePatientReception(patientReception, true, false);
                        else
                            _idunit.patientReception.AddNewPatientReception(patientReception, true, false);
                        return patientReception.Guid;

                    }
                    else
                    {
                        patientReception.Doctor.Guid = Guid.NewGuid();
                        patientReception.Doctor.UserName = _idunit.patient.RandomString(10);
                        patientReception.Doctor.Pass1 = "123";
                        patientReception.Doctor.ClinicSectionId = patientReception.OrginalClinicSectionId;
                        if (!NewReception)
                            _idunit.patientReception.UpdatePatientReception(patientReception, true, true);
                        else
                            _idunit.patientReception.AddNewPatientReception(patientReception, true, true);
                        return patientReception.Guid;
                    }
                }
                else
                {
                    patientReception.Doctor = null;
                    if (!NewReception)
                        _idunit.patientReception.UpdatePatientReception(patientReception, true, true);
                    else
                        _idunit.patientReception.AddNewPatientReception(patientReception, true, true);
                    return patientReception.Guid;
                }
            }
        }


        public string GetLatestReceptionInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                string fileNum = _unitOfWork.PatientReception.GetLatestReceptionInvoiceNum(clinicSectionId);
                return NextInvoiceNum(fileNum);
            }
            catch (Exception) { return ""; }
        }

        public string NextInvoiceNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }


        public ReceptionViewModel GetPatientReceptionByIdWithDoctor(Guid receptionId)
        {
            try
            {
                Reception PatientReceptionDtos = _unitOfWork.PatientReception.GetPatientReceptionByIdWithDoctor(receptionId);
                return ConvertModel(PatientReceptionDtos);

            }
            catch (Exception) { return null; }
        }

        public ReceptionViewModel GetPatientReceptionById(Guid receptionId)
        {
            try
            {
                Reception PatientReceptionDtos = _unitOfWork.PatientReception.GetPatientReceptionById(receptionId);
                return ConvertModel(PatientReceptionDtos);

            }
            catch (Exception) { return null; }
        }

        public ReceptionViewModel GetPatientReceptionByIdForReport(Guid patienReceptionId)
        {
            try
            {
                Reception PatientReceptionDtos = _unitOfWork.PatientReception.GetPatientReceptionByIdForReport(patienReceptionId);
                return ConvertModel(PatientReceptionDtos);

            }
            catch (Exception) { return null; }
        }

        public string GetTodaysFirstReceptionInvoiceNum(Guid clinicSectionId, DateTime today)
        {
            try
            {
                return _unitOfWork.Receptions.Find(x => x.ClinicSectionId == clinicSectionId && x.ReceptionDate.Value.Date == today.Date)
                    .OrderBy(x => x.Id).FirstOrDefault().ReceptionNum;

            }
            catch (Exception ex) { return ""; }
        }


        public ReceptionViewModel GetPatientReceptionByIdForAnalysisResult(Guid id)
        {
            try
            {
                Reception PatientReceptionDtos = _unitOfWork.PatientReception.GetPatientReceptionByIdForAnalysisResult(id);
                return ConvertModelFroAnalysisResult(PatientReceptionDtos);

            }
            catch (Exception) { return null; }
        }

        public Guid AddNewPatientReception(ReceptionViewModel newPatientReception, bool newPatient, bool newDoctor)
        {
            try
            {
                DateTime dt = DateTime.Now;
                dt = dt.AddMilliseconds(-dt.Millisecond);
                dt = dt.AddSeconds(-dt.Second);

                if (newPatientReception.Patient != null)
                    newPatientReception.Patient.User = Common.ConvertModels<UserInformationViewModel, PatientViewModel>.convertModels(newPatientReception.Patient);

                if (newPatientReception.Doctor == null)
                {
                    if (newPatientReception.DoctorId != Guid.Empty)
                    {
                        ReceptionDoctorViewModel rd = new ReceptionDoctorViewModel
                        {
                            Guid = Guid.NewGuid(),
                            CreatedDate = dt,
                            CreatedUserId = newPatientReception.UserId,
                            DoctorId = newPatientReception.DoctorId,
                            ReceptionId = newPatientReception.Guid,
                            Doctor = newPatientReception.Doctor,
                            DoctorRoleId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("DispatcherDoctor", "DoctorRole")
                        };
                        newPatientReception.ReceptionDoctors = new List<ReceptionDoctorViewModel>();
                        newPatientReception.ReceptionDoctors.Add(rd);
                    }
                }
                else
                {
                    newPatientReception.Doctor.User = Common.ConvertModels<UserInformationViewModel, DoctorViewModel>.convertModels(newPatientReception.Doctor);

                    ReceptionDoctorViewModel rd = new ReceptionDoctorViewModel
                    {
                        Guid = Guid.NewGuid(),
                        CreatedDate = dt,
                        CreatedUserId = newPatientReception.UserId,
                        DoctorId = newPatientReception.DoctorId,
                        ReceptionId = newPatientReception.Guid,
                        Doctor = newPatientReception.Doctor,
                        DoctorRoleId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("DispatcherDoctor", "DoctorRole")
                    };
                    newPatientReception.ReceptionDoctors = new List<ReceptionDoctorViewModel>();
                    newPatientReception.ReceptionDoctors.Add(rd);
                }


                Reception PatientReceptionDto = ConvertModelsFromViewModelToDto(newPatientReception);
                if (newPatientReception.HospitalReception ?? false)
                {
                    ReceptionClinicSection re = _unitOfWork.ReceptionClinicSections.GetSingle(x => x.Guid == newPatientReception.ReceptionClinicSectionId);
                    re.DestinationReception = PatientReceptionDto;

                    //var ser = _unitOfWork.Services.GetSingle(x => x.Name == newPatientReception.ClinicSectionName);
                    //Service sectionService = new();
                    //if (ser == null)
                    //{
                    //    sectionService.Name = newPatientReception.ClinicSectionName;
                    //    sectionService.Price = 1;
                    //    sectionService.Priority = 1;
                    //    sectionService.TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Other", "ServiceType");

                    //}

                    //ReceptionService RS = new ReceptionService()
                    //{
                    //    CreatedDate = dt,
                    //    CreatedUserId = PatientReceptionDto.CreatedUserId,
                    //    Discount = PatientReceptionDto.Discount,
                    //    Number = 1,
                    //    Price = PatientReceptionDto.PatientReceptionAnalyses.Sum(x => x.Amount),
                    //    ReceptionId = re.ReceptionId,
                    //    ServiceDate = dt,
                    //    ServiceId = (ser == null) ? null : _unitOfWork.Services.GetSingle(x => x.Name == newPatientReception.ClinicSectionName).Guid,
                    //    StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus"),
                    //    Explanation = PatientReceptionDto.Guid.ToString(),
                    //    Service = (ser == null) ? sectionService : null
                    //};

                    //_unitOfWork.ReceptionServices.Add(RS);
                }

                if (!string.IsNullOrWhiteSpace(newPatientReception.RadiologyDoctorName))
                {
                    if (PatientReceptionDto.ReceptionDoctors == null)
                        PatientReceptionDto.ReceptionDoctors = new List<ReceptionDoctor>();

                    var radiologyDoctorRoleId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("RadiologyDoctor", "DoctorRole");

                    if (newPatientReception.RadiologyDoctorId == Guid.Empty)
                    {
                        ReceptionDoctor receptionDoctor = new();

                        if (_idunit.doctor.CheckRepeatedNameAndSpeciallity(newPatientReception.RadiologyDoctorName, null, newPatientReception.OrginalClinicSectionId, false))
                        {
                            receptionDoctor.DoctorId = _unitOfWork.Doctor.Find(x => x.ClinicSectionId == newPatientReception.OrginalClinicSectionId && x.User.Name == newPatientReception.RadiologyDoctorName).FirstOrDefault().Guid;
                            receptionDoctor.DoctorRoleId = radiologyDoctorRoleId;
                            receptionDoctor.CreatedDate = dt;
                            receptionDoctor.CreatedUserId = PatientReceptionDto.CreatedUserId;

                            PatientReceptionDto.ReceptionDoctors.Add(receptionDoctor);
                        }
                        else
                        {
                            receptionDoctor = new ReceptionDoctor()
                            {
                                DoctorRoleId = radiologyDoctorRoleId,
                                CreatedDate = dt,
                                CreatedUserId = PatientReceptionDto.CreatedUserId,
                                Doctor = new()
                                {
                                    ClinicSectionId = newPatientReception.OrginalClinicSectionId,
                                    User = new User
                                    {
                                        ClinicSectionId = newPatientReception.OrginalClinicSectionId,
                                        Name = newPatientReception.RadiologyDoctorName,
                                        UserName = "123",
                                        UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                        Pass1 = "RadiologyDoctorName",

                                    }
                                }
                            };

                            PatientReceptionDto.ReceptionDoctors.Add(receptionDoctor);
                        }
                    }
                    else
                    {
                        ReceptionDoctor receptionDoctor = new()
                        {
                            DoctorId = newPatientReception.RadiologyDoctorId,
                            DoctorRoleId = radiologyDoctorRoleId,
                            CreatedDate = dt,
                            CreatedUserId = PatientReceptionDto.CreatedUserId
                        };

                        PatientReceptionDto.ReceptionDoctors.Add(receptionDoctor);
                    }
                }


                if (newPatientReception.Patient != null)
                    PatientReceptionDto.Patient.FileNum = _idunit.patient.GetPatientFileNum(PatientReceptionDto.ClinicSectionId ?? Guid.Empty, newPatientReception.OrginalClinicSectionId);

                var analysisService = _unitOfWork.Services.GetSingle(x => x.Name == "Analysis");
                Service anaService = new();
                if (analysisService == null)
                {
                    anaService.Name = "Analysis";
                    anaService.Price = 1;
                    anaService.Priority = 1;
                    anaService.TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Other", "ServiceType");

                }

                PatientReceptionDto.ReceptionServices.Add(new ReceptionService
                {
                    CreatedDate = DateTime.Now,
                    CreatedUserId = PatientReceptionDto.CreatedUserId,
                    Discount = PatientReceptionDto.PatientReceptionAnalyses.Sum(a => a.Amount) - newPatientReception.PatientReceptionReceiveds?.Sum(a => a.Amount),
                    DiscountCurrencyId = 11,
                    Number = 1,
                    Price = PatientReceptionDto.PatientReceptionAnalyses.Sum(x => x.Amount),
                    ServiceDate = DateTime.Now,
                    ServiceId = analysisService?.Guid,
                    //StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus"),
                    //Explanation = PatientReceptionDto.Guid.ToString(),
                    Service = (analysisService == null) ? anaService : null
                });

                PatientReceptionDto.PaymentStatusId = PatientReceptionDto.ReceptionServices.FirstOrDefault().StatusId;

                if (newPatientReception.AutoPay)
                {

                    PatientReceptionDto.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");

                    PatientReceptionDto.ReceptionServices.FirstOrDefault().StatusId = PatientReceptionDto.PaymentStatusId;

                    PatientReceptionDto.ReceptionServices.FirstOrDefault().ReceptionServiceReceiveds.Add(new ReceptionServiceReceived
                    {
                        Amount = PatientReceptionDto.ReceptionServices.FirstOrDefault().Price - PatientReceptionDto.ReceptionServices.FirstOrDefault().Discount,
                        CreatedDate = DateTime.Now,
                        CreatedUserId = PatientReceptionDto.CreatedUserId,
                        CurrencyId = 11,
                        AmountStatus = false
                    });

                }
                else
                {
                    PatientReceptionDto.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");

                    PatientReceptionDto.ReceptionServices.FirstOrDefault().StatusId = PatientReceptionDto.PaymentStatusId;
                }

                return _unitOfWork.PatientReception.AddNewPatientReception(PatientReceptionDto, newDoctor);
            }
            catch (Exception ex) { throw ex; }
        }

        public string BarcodeGenerator(Guid clinicSectionId)
        {
            Random generator = new Random();
            String r = generator.Next(0, 10000000).ToString("D7");
            if (CheckRepeatedBarcode(clinicSectionId, r))
            {
                return BarcodeGenerator(clinicSectionId);
            }
            else
            {
                return r;
            }
        }

        public bool CheckRepeatedBarcode(Guid clinicSectionId, string barcode)
        {
            try
            {
                Reception p = _unitOfWork.Receptions.GetSingle(x => x.ClinicSectionId == clinicSectionId /*&& x.Barcode == barcode*/);
                if (p != null)
                    return true;
                else
                    return false;

            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<ReceptionViewModel> GetAllPatientReceptionInvoiceNums(Guid clinicSectionId)
        {
            IEnumerable<Reception> AllInvoiceNums = _unitOfWork.PatientReception.GetAllPatientReceptionInvoiceNums(clinicSectionId);
            return ConvertModelsLists(AllInvoiceNums);
        }


        public IEnumerable<ReceptionViewModel> GetAllPatientReceptionPatients(Guid clinicSectionId)
        {
            IEnumerable<Reception> AllPatients = _unitOfWork.PatientReception.GetAllPatientReceptionPatients(clinicSectionId);
            return ConvertModelsListsCombinePatientNameAndPhone(AllPatients);
        }


        public Guid UpdatePatientReception(ReceptionViewModel newPatientReception, bool newPatient, bool newDoctor)
        {
            try
            {
                DateTime dt = DateTime.Now;
                dt = dt.AddMilliseconds(-dt.Millisecond);
                dt = dt.AddSeconds(-dt.Second);

                if (newPatientReception.Patient != null)
                    newPatientReception.Patient.User = Common.ConvertModels<UserInformationViewModel, PatientViewModel>.convertModels(newPatientReception.Patient);

                if (newPatientReception.Doctor == null)
                {
                    if (newPatientReception.DoctorId != Guid.Empty)
                    {
                        ReceptionDoctorViewModel rd = new ReceptionDoctorViewModel
                        {
                            Guid = Guid.NewGuid(),
                            CreatedDate = dt,
                            CreatedUserId = newPatientReception.UserId,
                            DoctorId = newPatientReception.DoctorId,
                            ReceptionId = newPatientReception.Guid,
                            DoctorRoleId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("DispatcherDoctor", "DoctorRole"),
                            Doctor = newPatientReception.Doctor
                        };
                        newPatientReception.ReceptionDoctors = new List<ReceptionDoctorViewModel>();
                        newPatientReception.ReceptionDoctors.Add(rd);
                    }
                }
                else
                {
                    newPatientReception.Doctor.User = Common.ConvertModels<UserInformationViewModel, DoctorViewModel>.convertModels(newPatientReception.Doctor);

                    ReceptionDoctorViewModel rd = new ReceptionDoctorViewModel
                    {
                        Guid = Guid.NewGuid(),
                        CreatedDate = dt,
                        CreatedUserId = newPatientReception.UserId,
                        DoctorId = newPatientReception.DoctorId,
                        ReceptionId = newPatientReception.Guid,
                        DoctorRoleId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("DispatcherDoctor", "DoctorRole"),
                        Doctor = newPatientReception.Doctor
                    };
                    newPatientReception.ReceptionDoctors = new List<ReceptionDoctorViewModel>();
                    newPatientReception.ReceptionDoctors.Add(rd);
                }

                //if (newPatientReception.HospitalReception ?? false)
                //{
                //    ReceptionClinicSection re = _unitOfWork.ReceptionClinicSections.GetSingle(x => x.DestinationReceptionId == newPatientReception.Guid);
                //    _unitOfWork.ReceptionServices.Remove(_unitOfWork.ReceptionServices.GetSingle(x => x.Explanation == newPatientReception.Guid.ToString()));
                //    ReceptionService RS = new ReceptionService()
                //    {
                //        CreatedDate = dt,
                //        CreatedUserId = newPatientReception.CreatedUserId,
                //        Discount = newPatientReception.Discount,
                //        Number = 1,
                //        Price = newPatientReception.PatientReceptionAnalyses.Sum(x => x.Amount),
                //        ReceptionId = re.ReceptionId,
                //        ServiceDate = dt,
                //        ServiceId = _unitOfWork.Services.GetSingle(x => x.Name == newPatientReception.ClinicSectionName).Guid,
                //        StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus")
                //    };
                //    //PatientReceptionDto.ReceptionClinicSectionDestinations.Add(re);
                //    _unitOfWork.ReceptionServices.Add(RS);
                //}

                Reception sPatientReceptionDto = ConvertModelsFromViewModelToDto(newPatientReception);


                if (!string.IsNullOrWhiteSpace(newPatientReception.RadiologyDoctorName))
                {
                    if (sPatientReceptionDto.ReceptionDoctors == null)
                        sPatientReceptionDto.ReceptionDoctors = new List<ReceptionDoctor>();

                    var radiologyDoctorRoleId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("RadiologyDoctor", "DoctorRole");

                    if (newPatientReception.RadiologyDoctorId == Guid.Empty)
                    {
                        ReceptionDoctor receptionDoctor = new();

                        if (_idunit.doctor.CheckRepeatedNameAndSpeciallity(newPatientReception.RadiologyDoctorName, null, newPatientReception.OrginalClinicSectionId, false))
                        {
                            receptionDoctor.DoctorId = _unitOfWork.Doctor.Find(x => x.ClinicSectionId == newPatientReception.OrginalClinicSectionId && x.User.Name == newPatientReception.RadiologyDoctorName).FirstOrDefault().Guid;
                            receptionDoctor.DoctorRoleId = radiologyDoctorRoleId;
                            receptionDoctor.CreatedDate = dt;
                            receptionDoctor.CreatedUserId = sPatientReceptionDto.CreatedUserId;

                            sPatientReceptionDto.ReceptionDoctors.Add(receptionDoctor);
                        }
                        else
                        {
                            receptionDoctor = new ReceptionDoctor()
                            {
                                DoctorRoleId = radiologyDoctorRoleId,
                                CreatedDate = dt,
                                CreatedUserId = sPatientReceptionDto.CreatedUserId,
                                Doctor = new()
                                {
                                    ClinicSectionId = newPatientReception.OrginalClinicSectionId,
                                    User = new User
                                    {
                                        ClinicSectionId = newPatientReception.OrginalClinicSectionId,
                                        Name = newPatientReception.RadiologyDoctorName,
                                        UserName = "123",
                                        UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                        Pass1 = "RadiologyDoctorName",

                                    }
                                }
                            };

                            sPatientReceptionDto.ReceptionDoctors.Add(receptionDoctor);
                        }
                    }
                    else
                    {
                        ReceptionDoctor receptionDoctor = new()
                        {
                            DoctorId = newPatientReception.RadiologyDoctorId,
                            DoctorRoleId = radiologyDoctorRoleId,
                            CreatedDate = dt,
                            CreatedUserId = sPatientReceptionDto.CreatedUserId
                        };

                        sPatientReceptionDto.ReceptionDoctors.Add(receptionDoctor);
                    }
                }

                var analysisService = _unitOfWork.Services.GetSingle(x => x.Name == "Analysis");
                Service anaService = new();
                if (analysisService == null)
                {
                    anaService.Name = "Analysis";
                    anaService.Price = 1;
                    anaService.Priority = 1;
                    anaService.TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Other", "ServiceType");

                }
                if (newPatientReception.AutoPay)
                {
                    var RS = _unitOfWork.ReceptionServices.GetSingle(a => a.ReceptionId == newPatientReception.Guid);
                    if (RS != null)
                    {
                        _unitOfWork.ReceptionServiceReceiveds.RemoveRange(_unitOfWork.ReceptionServiceReceiveds.Find(a => a.ReceptionServiceId == RS.Guid));
                        _unitOfWork.ReceptionServices.Remove(RS);
                    }

                    sPatientReceptionDto.ReceptionServices.Add(new ReceptionService
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUserId = newPatientReception.ModifiedUserId,
                        Discount = newPatientReception.PatientReceptionAnalyses.Sum(a => a.Amount) - newPatientReception.PatientReceptionReceiveds.Sum(a => a.Amount),
                        DiscountCurrencyId = 11,
                        Number = 1,
                        Price = newPatientReception.PatientReceptionAnalyses.Sum(x => x.Amount),
                        ServiceDate = DateTime.Now,
                        ServiceId = (analysisService == null) ? null : _unitOfWork.Services.GetSingle(x => x.Name == "Analysis").Guid,
                        StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("paid", "PaymentStatus"),
                        Explanation = newPatientReception.Guid.ToString(),
                        Service = (analysisService == null) ? anaService : null
                    });
                    sPatientReceptionDto.PaymentStatusId = sPatientReceptionDto.ReceptionServices.FirstOrDefault().StatusId;
                    sPatientReceptionDto.ReceptionServices.FirstOrDefault().ReceptionServiceReceiveds.Add(new ReceptionServiceReceived
                    {
                        Amount = sPatientReceptionDto.ReceptionServices.FirstOrDefault().Price - sPatientReceptionDto.ReceptionServices.FirstOrDefault().Discount,
                        CreatedDate = DateTime.Now,
                        CreatedUserId = sPatientReceptionDto.CreatedUserId,
                        CurrencyId = 11,
                        AmountStatus = false
                    });

                }
                else
                {
                    var RS = _unitOfWork.ReceptionServices.GetSingle(a => a.ReceptionId == newPatientReception.Guid);
                    if (RS == null)
                    {
                        sPatientReceptionDto.ReceptionServices.Add(new ReceptionService
                        {
                            CreatedDate = DateTime.Now,
                            CreatedUserId = newPatientReception.ModifiedUserId,
                            Discount = newPatientReception.PatientReceptionAnalyses.Sum(a => a.Amount) - newPatientReception.PatientReceptionReceiveds.Sum(a => a.Amount),
                            DiscountCurrencyId = 11,
                            Number = 1,
                            Price = newPatientReception.PatientReceptionAnalyses.Sum(x => x.Amount),
                            ServiceDate = DateTime.Now,
                            ServiceId = (analysisService == null) ? null : _unitOfWork.Services.GetSingle(x => x.Name == "Analysis").Guid,
                            StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus"),
                            Explanation = newPatientReception.Guid.ToString(),
                            Service = (analysisService == null) ? anaService : null
                        });
                    }
                    else
                    {
                        RS.Price = newPatientReception.PatientReceptionAnalyses.Sum(x => x.Amount);
                        IEnumerable<ReceptionServiceReceived> RSR = _unitOfWork.ReceptionServiceReceiveds.Find(a => a.ReceptionServiceId == RS.Guid);
                        if (RS.Price > RSR.Sum(a => a.AmountStatus.GetValueOrDefault(false) ? -a.Amount : a.Amount))
                        {
                            sPatientReceptionDto.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");

                            RS.StatusId = sPatientReceptionDto.PaymentStatusId;
                        }
                    }

                }

                return _unitOfWork.PatientReception.UpdatePatientReception(sPatientReceptionDto, newPatient, newDoctor);
            }
            catch (Exception ex) { throw ex; }
        }


        public OperationStatus RemovePatientReceptionWithReceives(Guid PatientReceptionId, string rootPath)
        {
            try
            {

                if (_unitOfWork.Receptions.GetReceptionDischargeStatus(PatientReceptionId))
                    return OperationStatus.CanNotDelete;

                var images = _unitOfWork.PatientReception.RemovePatientReceptionWithReceives(PatientReceptionId);
                FileAttachments deleteIamge = new();
                var imageDto = Common.ConvertModels<PatientImageViewModel, PatientImage>.convertModelsLists(images);
                _unitOfWork.Complete();
                deleteIamge.DeleteAllFiles(imageDto, rootPath);

                return OperationStatus.SUCCESSFUL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ReceptionViewModel> ConvertModelsListsCombinePatientNameAndPhone(IEnumerable<Reception> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.ReceptionDoctors, b => b.Ignore())
                .ForMember(a => a.DoctorId, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor.Guid))
                .ForMember(a => a.Doctor, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor));
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.PhoneNumberAndName, b => b.MapFrom(c => c.User.Name + "|" + c.User.PhoneNumber))
                ;

                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<Reception>, List<ReceptionViewModel>>(ress);

        }




        public static List<ReceptionViewModel> ConvertModelsLists(IEnumerable<Reception> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.ReceptionDoctors, b => b.Ignore())
                .ForMember(a => a.DoctorId, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor.Guid))
                .ForMember(a => a.Doctor, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor))
                .ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
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
                //cfg.CreateMap<User, UserInformationViewModel>();
                //cfg.CreateMap<Doctor, DoctorViewModel>();
                //cfg.CreateMap<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<Reception>, List<ReceptionViewModel>>(ress);

        }


        public ReceptionViewModel ConvertModel(Reception ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.ReceptionDoctors, b => b.Ignore())
                .ForMember(a => a.DoctorId, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor.Guid))
                .ForMember(a => a.Doctor, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor))
                .ForMember(a => a.DoctorUserName, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor.User.Name))
                .ForMember(a => a.ClinicSectionName, b => b.MapFrom(c => c.ClinicSection.Name))
                .ForMember(a => a.ClinicSectionTypeName, b => b.MapFrom(c => c.ClinicSection.ClinicSectionType.Name))
                .ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                .ForMember(a => a.ClinicSection, b => b.Ignore())
                .ForMember(a => a.PatientReceptionReceiveds, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()))
                .ForMember(a => a.GenderId, b => b.MapFrom(c => c.User.GenderId))
                ;
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>();
                cfg.CreateMap<Analysis, AnalysisViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore())
                ;
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();
                cfg.CreateMap<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>();
                //cfg.CreateMap<PatientReceptionReceived, PatientReceptionReceivedViewModel>()
                //.ForMember(a => a.ViewDate, b => b.MapFrom(c => c.Date.Value.ToShortDateString()))
                //.ForMember(a => a.CanDelete, b => b.MapFrom(c => CanDelete(c.Date.Value)))
                //;

                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<Reception, ReceptionViewModel>(ress);

        }



        public ReceptionViewModel ConvertModelFroAnalysisResult(Reception ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.ReceptionDoctors, b => b.Ignore())
                .ForMember(a => a.DoctorId, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor.Guid))
                .ForMember(a => a.Doctor, b => b.MapFrom(c => c.ReceptionDoctors.FirstOrDefault().Doctor))
                .ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
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
                cfg.CreateMap<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>();
                cfg.CreateMap<Analysis, AnalysisViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.MapFrom(c => c.AnalysisItemMinMaxValues.FirstOrDefault()))
                ;
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();
                cfg.CreateMap<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>();
                //cfg.CreateMap<PatientReceptionReceived, PatientReceptionReceivedViewModel>()
                //.ForMember(a => a.ViewDate, b => b.MapFrom(c => c.Date.Value.ToShortDateString()))
                //.ForMember(a => a.CanDelete, b => b.MapFrom(c => CanDelete(c.Date.Value)))
                //;

                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<ClinicSection, ClinicSectionViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<Reception, ReceptionViewModel>(ress);

        }

        public Reception ConvertModelsFromViewModelToDto(ReceptionViewModel ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionViewModel, Reception>();
                cfg.CreateMap<ReceptionDoctorViewModel, ReceptionDoctor>();
                cfg.CreateMap<PatientViewModel, Patient>();
                cfg.CreateMap<UserInformationViewModel, User>();
                cfg.CreateMap<DoctorViewModel, Doctor>();
                cfg.CreateMap<ReceptionClinicSectionViewModel, ReceptionClinicSection>();
                cfg.CreateMap<PatientReceptionAnalysisViewModel, PatientReceptionAnalysis>();
                //cfg.CreateMap<PatientReceptionReceivedViewModel, PatientReceptionReceived>();
                cfg.CreateMap<AnalysisResultMasterViewModel, AnalysisResultMaster>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<ReceptionViewModel, Reception>(ress);

        }


    }


}
