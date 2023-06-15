using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices.Interface;
using WPH.Models.PatientImage;
using WPH.Models.Patient;
using WPH.Models.Chart;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class PatientMvcMockingService : IPatientMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public PatientMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
        }

        public string AddPatient(PatientViewModel patientt, Guid clinicSectionId)
        {
            if (!string.IsNullOrWhiteSpace(patientt.PhoneNumber) && patientt.PhoneNumber.Length < 8)
                return "WrongMobile";

            Patient patient = Common.ConvertModels<Patient, PatientViewModel>.convertModels(patientt);
            patient.FileNum = GetPatientFileNum(clinicSectionId, patientt.ClinicSectionId ?? Guid.Empty);
            User user = Common.ConvertModels<User, PatientViewModel>.convertModels(patientt);
            user.Pass1 = "123";

            user.Guid = patient.Guid = Guid.NewGuid();
            _unitOfWork.Users.Add(user);
            _unitOfWork.Patients.Add(patient);

            _unitOfWork.Complete();

            return patient.Guid.ToString();
        }

        public string GetPatientFileNum(Guid clinicSectionId, Guid clinicId)
        {
            try
            {
                string fileNum = _unitOfWork.Patients.GetLatestPatientFileNum(clinicSectionId, clinicId);
                return NextInvoiceNum(fileNum);
            }
            catch (Exception ex) { return "1"; }
        }

        public string NextInvoiceNum(string str)
        {
            string digits = new string(str.Where(char.IsDigit).ToArray());
            string letters = new string(str.Where(char.IsLetter).ToArray());
            int.TryParse(digits, out int number);
            return letters + (++number).ToString("D" + digits.Length.ToString());
        }

        public IEnumerable<PatientViewModel> GetAllPatients(bool forGrid, Guid? clinicSectionId = null)
        {
            try
            {

                IEnumerable<Patient> patients = _unitOfWork.Patients.GetAllPatientS(forGrid, clinicSectionId).OrderByDescending(x => x.Id);
                List<PatientViewModel> patientsView = ConvertModelsListsPatientDtoToPatientViewModelForReserve(patients, false).ToList();
                Indexing<PatientViewModel> indexing = new Indexing<PatientViewModel>();
                return indexing.AddIndexing(patientsView);

            }
            catch (Exception e) { throw e; }
        }

        public async Task<List<PatientViewModel>> GetAllPatientsWithCombinedNameAndFileNum(bool forGrid, Guid clinicSectionId, Guid clinicId, int sectionTypeId)
        {
            var sval = _idunit.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseFormNumber").FirstOrDefault();

            bool useFormNum;
            try
            {
                useFormNum = bool.Parse(sval?.SValue ?? "false");
            }
            catch { useFormNum = false; }

            IEnumerable<Patient> patients = _unitOfWork.Patients.GetAllPatientS(forGrid, clinicSectionId);
            return ConvertModelsListsPatientDtoToPatientViewModelForReserve(patients, useFormNum);
        }

        public async Task<List<PatientFilterViewModel>> GetAllPatientForFilter(Guid clinicSectionId)
        {

            var sval = _idunit.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseFormNumber").FirstOrDefault();

            bool useFormNum;
            try
            {
                useFormNum = bool.Parse(sval?.SValue ?? "false");
            }
            catch { useFormNum = false; }

            IEnumerable<Patient> patients = _unitOfWork.Patients.GetAllPatientForFilter(clinicSectionId);
            return ConvertPatientModelsForFilter(patients, useFormNum);
        }

        public async Task<List<PatientViewModel>> GetAllPatientsWithCombinedNameAndFileNumForReserve(bool forGrid, Guid clinicSectionId, Guid clinicId, int sectionTypeId)
        {
            try
            {
                var sval = _idunit.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseFormNumber").FirstOrDefault();

                bool useFormNum;
                try
                {
                    useFormNum = bool.Parse(sval?.SValue ?? "false");
                }
                catch { useFormNum = false; }

                IEnumerable<Patient> patients = _unitOfWork.Patients.GetAllPatientS(forGrid, clinicSectionId);
                return ConvertModelsListsPatientDtoToPatientViewModelForReserve(patients, useFormNum);
            }
            catch (Exception e) { throw e; }

        }

        public List<PatientViewModel> GetAllPatientsWithCombinedNameAndPhoneNumber(bool forGrid, Guid clinicSectionId)
        {
            try
            {

                IEnumerable<Patient> patients = _unitOfWork.Patients.GetAllPatientS(forGrid, clinicSectionId);
                return ConvertModelsListsPatientDtoToPatientViewModelForReserve(patients, false);
            }
            catch (Exception e) { throw e; }

        }

        public PatientViewModel GetPatient(Guid patientId)
        {
            try
            {
                Patient patientDto = _unitOfWork.Patients.GetPatient(patientId);
                return ConvertModelEntityToDto(patientDto);
            }
            catch { return null; }
        }

        public IEnumerable<PatientViewModel> GetPatientJustNameAndGuid(Guid clinicSectionId)
        {
            try
            {
                IEnumerable<Patient> patientDto = _unitOfWork.Patients.GetPatientJustNameAndGuid(clinicSectionId);
                return ConvertModelsJustNameAndGuid(patientDto);
            }
            catch { return null; }
        }


        public PatientViewModel GetPatientWithCombinedNameAndPhone(Guid patientId)
        {
            try
            {
                Patient patientDto = _unitOfWork.Patients.GetPatient(patientId);
                return ConvertModelPatientDtoToPatientViewModel(patientDto, true);
            }
            catch { return null; }
        }

        public Guid GetPatientIdByName(string name, Guid clinicSectionId)
        {
            //Guid ClinicId = _unitOfWork.ClinicSections.GetSingle(x => x.Guid == clinicSectionId).ClinicId;
            return _unitOfWork.Users.GetUserByName(name, clinicSectionId).Guid;
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Patient/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridPatientLink = controllerName + "PatientRecordForm?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public OperationStatus RemovePatient(Guid patientId)
        {
            try
            {

                Patient patient = _unitOfWork.Patients.Get(patientId);
                IEnumerable<Reception> allPatientVisit = _unitOfWork.Receptions.Find(x => x.ReserveDetail.PatientId == patientId);
                if (allPatientVisit.Any())
                {
                    foreach (Reception visit in allPatientVisit)
                    {
                        IEnumerable<PrescriptionDetail> preDetail = _unitOfWork.PrescriptionDetails.Find(x => x.ReceptionId == visit.Guid);
                        IEnumerable<PrescriptionTestDetail> preTestDetail = _unitOfWork.PrescriptionTests.Find(x => x.ReceptionId == visit.Guid);
                        IEnumerable<PatientVariablesValue> vp = _unitOfWork.PatientVariablesValue.Find(x => x.ReceptionId == visit.Guid);
                        IEnumerable<VisitPatientDisease> vpd = _unitOfWork.VisitDiseasePatients.Find(x => x.ReceptionId == visit.Guid);
                        IEnumerable<VisitSymptom> vs = _unitOfWork.Visit_Symptoms.Find(x => x.ReceptionId == visit.Guid);
                        if (preDetail.Any())
                            _unitOfWork.PrescriptionDetails.RemoveRange(preDetail);
                        if (preTestDetail.Any())
                            _unitOfWork.PrescriptionTests.RemoveRange(preTestDetail);
                        if (vpd.Any())
                            _unitOfWork.VisitDiseasePatients.RemoveRange(vpd);
                        if (vp.Any())
                            _unitOfWork.PatientVariablesValue.RemoveRange(vp);
                        if (vs.Any())
                            _unitOfWork.Visit_Symptoms.RemoveRange(vs);
                        _unitOfWork.ReceptionServices.RemoveRange(_unitOfWork.ReceptionServices.Find(a => a.ReceptionId == visit.Guid));
                        _unitOfWork.ReceptionDetailPaies.RemoveRange(_unitOfWork.ReceptionDetailPaies.Find(a => a.ReceptionId == visit.Guid));
                    }
                    _unitOfWork.Receptions.RemoveRange(allPatientVisit);
                }

                IEnumerable<PatientDiseaseRecord> allPatientDisease = _unitOfWork.PatientDiseaseRecords.Find(x => x.Patientid == patientId);
                if (allPatientDisease.Any())
                    _unitOfWork.PatientDiseaseRecords.RemoveRange(allPatientDisease);

                IEnumerable<ReserveDetail> allPatientReserves = _unitOfWork.ReserveDetails.Find(x => x.PatientId == patientId);
                if (allPatientReserves.Any())
                    _unitOfWork.ReserveDetails.RemoveRange(allPatientReserves);


                _unitOfWork.Patients.Remove(patient);
                User pat = _unitOfWork.Users.Get(patientId);
                IEnumerable<ClinicSectionUser> clipat = _unitOfWork.ClinicSection_Users.Find(x => x.UserId == patientId);
                _unitOfWork.ClinicSection_Users.RemoveRange(clipat);
                
                _unitOfWork.Users.Remove(pat);
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

        public void UpdatePatient(PatientViewModel patient)
        {
            try
            {
                Patient updatedPatient = Common.ConvertModels<Patient, PatientViewModel>.convertModels(patient);
                User updatedUser = Common.ConvertModels<User, PatientViewModel>.convertModels(patient);
                User oldUser = _unitOfWork.Users.Get(updatedUser.Guid);
                Patient oldPatient = _unitOfWork.Patients.Get(updatedPatient.Guid);
                _unitOfWork.Users.Detach(oldUser);
                _unitOfWork.Patients.Detach(oldPatient);
                _unitOfWork.Patients.UpdateState(updatedPatient);
                _unitOfWork.Users.UpdateUserExceptPass(updatedUser);
                _unitOfWork.Complete();
            }
            catch (Exception ex) { throw ex; }
        }


        public bool CheckRepeatedNameAndNumber(string name, string phoneNumber, Guid clinicSectionId, bool NewOrUpdate, string oldName = "", string oldNumber = "")
        {
            return _idunit.user.IsNameExist(name, clinicSectionId, NewOrUpdate, oldName);
        }

        public string getLastPatientFileNumber(Guid clinicSectionId, Guid clinicId)
        {
            return GetPatientFileNum(clinicSectionId, clinicId);
        }


        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public IEnumerable<PatientViewModel> GetAllClinicPatients(Guid clinicSectionId)
        {
            IEnumerable<Patient> all = _unitOfWork.Patients.GetAllClinicPatients(clinicSectionId);
            return ConvertModelsListsPatientAllClinics(all);

        }

        public PatientViewModel GetPatientIdAndNameFromReserveDetailId(Guid reserveDetailId)
        {
            return ConvertModelEntityToDto(_unitOfWork.Patients.GetPatientIdAndNameFromReserveDetailId(reserveDetailId));
        }


        public IEnumerable<PatientViewModel> GetAllReceptionClinicSectionPatients()
        {
            IEnumerable<Patient> hosp = _unitOfWork.ReceptionClinicSections.GetAllReceptionClinicSectionPatients();
            return ConvertModelsJustNameAndGuid(hosp);
        }

        




        ////////////////////////////////////////////////////////////////////////converters
        ///

        public List<PatientViewModel> ConvertModelsJustNameAndGuid(IEnumerable<Patient> patients)
        {
            List<PatientViewModel> PatientViewModelList = new List<PatientViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Patient, PatientViewModel>()
                          .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                        .ForMember(a => a.User, b => b.Ignore());

            });
            IMapper mapper = config.CreateMapper();
            PatientViewModelList = mapper.Map<IEnumerable<Patient>, List<PatientViewModel>>(patients);
            return PatientViewModelList;
        }

        public List<PatientViewModel> ConvertModelsListsPatientAllClinics(IEnumerable<Patient> patients)
        {
            List<PatientViewModel> PatientViewModelList = new List<PatientViewModel>();
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Patient, PatientViewModel>()
                      .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name + " | " + c.User.ClinicSection.Name))
                      .ForMember(a => a.Guid, b => b.MapFrom(c => c.Guid))
                      .ForAllOtherMembers(b => b.Ignore());

            });
            IMapper mapper = config.CreateMapper();
            PatientViewModelList = mapper.Map<IEnumerable<Patient>, List<PatientViewModel>>(patients);
            return PatientViewModelList;
        }

        public List<PatientFilterViewModel> ConvertPatientModelsForFilter(IEnumerable<Patient> patients, bool useFormNum)
        {
            List<PatientFilterViewModel> PatientViewModelList = new List<PatientFilterViewModel>();
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Patient, PatientFilterViewModel>()
                  .ForMember(a => a.NameAndFileNum, b => b.MapFrom(c => $"{c.User.Name} | {(useFormNum ? c.FormNumber : c.FileNum)}"))
                  .ForMember(a => a.PhoneNumberAndName, b => b.MapFrom(c => c.User.PhoneNumber + " | " + c.User.Name))
                  .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                  .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                  .ForMember(a => a.GenderId, b => b.MapFrom(c => c.User.GenderId))
                  .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();

            });
            IMapper mapper = config.CreateMapper();
            PatientViewModelList = mapper.Map<IEnumerable<Patient>, List<PatientFilterViewModel>>(patients);
            return PatientViewModelList;
        }

        public List<PatientViewModel> ConvertModelsListsPatientDtoToPatientViewModelForReserve(IEnumerable<Patient> patients, bool useFormNum)
        {
            List<PatientViewModel> PatientViewModelList = new List<PatientViewModel>();
            var config = new MapperConfiguration(cfg =>
            {

                if (useFormNum)
                {
                    cfg.CreateMap<Patient, PatientViewModel>()
                      .ForMember(a => a.NameAndFileNum, b => b.MapFrom(c => c.User.Name + " | " + c.FormNumber))
                      .ForMember(a => a.PhoneNumberAndName, b => b.MapFrom(c => c.User.PhoneNumber + " | " + c.User.Name))
                      .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                      .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                      .ForMember(a => a.GenderId, b => b.MapFrom(c => c.User.GenderId))
                      .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                    cfg.CreateMap<User, UserInformationViewModel>();
                    cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                }
                else
                {
                    cfg.CreateMap<Patient, PatientViewModel>()
                     .ForMember(a => a.NameAndFileNum, b => b.MapFrom(c => c.User.Name + " | " + c.FileNum))
                     .ForMember(a => a.PhoneNumberAndName, b => b.MapFrom(c => c.User.PhoneNumber + " | " + c.User.Name))
                     .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                     .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                     .ForMember(a => a.GenderId, b => b.MapFrom(c => c.User.GenderId))
                     .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                    cfg.CreateMap<User, UserInformationViewModel>();
                    cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                }

            });
            IMapper mapper = config.CreateMapper();
            PatientViewModelList = mapper.Map<IEnumerable<Patient>, List<PatientViewModel>>(patients);
            return PatientViewModelList;
        }

        public PatientViewModel ConvertModelPatientDtoToPatientViewModel(Patient patient, bool CombinedNameAndPhone)
        {
            var config = new MapperConfiguration(cfg =>
            {

                if (CombinedNameAndPhone)
                {
                    cfg.CreateMap<Patient, PatientViewModel>()
                   .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name + " | " + c.User.PhoneNumber))
                   .ForMember(a => a.GenderId, b => b.MapFrom(c => c.User.GenderId))
                   .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                   .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                    cfg.CreateMap<User, UserInformationViewModel>();
                }
                else
                {
                    cfg.CreateMap<Patient, PatientViewModel>()
                    .ForMember(a => a.GenderId, b => b.MapFrom(c => c.User.GenderId))
                    .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                   .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                    cfg.CreateMap<User, UserInformationViewModel>();
                }

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<Patient, PatientViewModel>(patient);
        }


        public PatientViewModel ConvertModelEntityToDto(Patient patient)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.GenderId, b => b.MapFrom(a => a.User.GenderId))
                .ForMember(a => a.Email, b => b.MapFrom(a => a.User.Email))
                .ForMember(a => a.Name, b => b.MapFrom(a => a.User.Name))
                .ForMember(a => a.Pass1, b => b.MapFrom(a => a.User.Pass1))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(a => a.User.PhoneNumber))
                .ForMember(a => a.ClinicSectionId, b => b.MapFrom(a => a.User.ClinicSectionId))
                .ForMember(a => a.DateOfBirthDay, b => b.MapFrom(a => a.DateOfBirth.GetValueOrDefault().Day))
                .ForMember(a => a.DateOfBirthMonth, b => b.MapFrom(a => a.DateOfBirth.GetValueOrDefault().Month))
                .ForMember(a => a.DateOfBirthYear, b => b.MapFrom(a => a.DateOfBirth.GetValueOrDefault().Year))
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.BaseInfoGeneral, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                ;

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Patient, PatientViewModel>(patient);
        }

        public PatientViewModel CheckNewPatient(ref bool newPatient, PatientViewModel patient, Guid clinicId, Guid clinicSectionId)
        {
            if (CheckRepeatedNameAndNumber(patient.Name, patient.PhoneNumber, clinicSectionId, false))
            {
                return null;
            }


            if (patient.AddressName != null)
            {
                BaseInfoViewModel address = _idunit.baseInfo.GetBaseInfoByName(patient.AddressName, clinicSectionId);

                if (address != null)
                {
                    patient.AddressId = address.Guid;
                }
                else
                {

                    patient.Address = new BaseInfoViewModel
                    {
                        Guid = Guid.NewGuid(),
                        ClinicSectionId = clinicSectionId,
                        Name = patient.AddressName,
                        Priority = 1,
                        TypeId = _idunit.baseInfo.GetBaseInfoTypeIdByName("Address")
                    };

                }
            }


            DateTime date = DateTime.Now;
            if (patient.DateOfBirthYear != null)
                date = new DateTime(Convert.ToInt32(patient.DateOfBirthYear), Convert.ToInt32(patient.DateOfBirthMonth), Convert.ToInt32(patient.DateOfBirthDay));
            patient.DateOfBirth = date;
            patient.Guid = Guid.NewGuid();
            patient.ClinicSectionId = clinicSectionId;
            patient.UserName = RandomString(10);
            patient.Pass1 = "123";
            newPatient = true;
            return patient;
        }

        
    }

}
