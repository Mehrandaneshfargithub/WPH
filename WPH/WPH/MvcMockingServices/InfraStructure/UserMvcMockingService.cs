using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.UserClinicSection;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.MvcMockingServices;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class UserMvcMockingService : IUserMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public UserMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
        }

        public void saveCookie(string cookie, string cookieName, string cookieValue)
        {
            CookieOptions languageCulture = new CookieOptions();

            languageCulture.Expires = DateTime.MaxValue;
            //response.Cookies.Append(cookieName, cookieValue, languageCulture);

        }
        public string LoadLoginThemeCookie()
        {
            string themeName = "";
            try
            {
                //IResponseCookies theme = response.Cookies;
                //if (theme == null)
                //{
                //    themeName = "blur-login";
                //    saveCookie("Theme", "loginTheme", "btn-login-blur", response);
                //}
                //else
                //{

                //    themeName = "blur-login";
                //    saveCookie("Theme", "loginTheme", "btn-login-blur", response);

                //}
            }
            catch { }
            return themeName;
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/UserManagment/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.AccessLink = controllerName + "AccessModal?Id=";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal?typeId=";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public IEnumerable<UserClinicSectionViewModel> GetAllClinicsUsers(Guid clinicSectionId)
        {
            IEnumerable<ClinicSectionUser> users = _unitOfWork.Users.GetAllClinicsUsers(clinicSectionId);
            return Common.ConvertModels<UserClinicSectionViewModel, ClinicSectionUser>.convertModelsLists(users);

        }

        public void SaveUserAccess(Guid userId, List<string> checkedIds, Guid clinicSectionId)
        {
            _idunit.subSystem.SaveAccessForUser(userId, checkedIds, clinicSectionId);
        }


        public Guid CheckClinicExist(string code)
        {
            Clinic clinic = _unitOfWork.Clinics.GetSingle(x => x.SystemCode == code);
            if (clinic == null)
            {
                return Guid.Empty;
            }
            else
            {
                return clinic.Guid;
            }
        }

        public UserInformationViewModel GetUserByName(string user)
        {
            User users = _unitOfWork.Users.GetSingle(x => x.UserName == user);
            return Common.ConvertModels<UserInformationViewModel, User>.convertModels(users);
        }

        public Guid GetUserIdByUserName(Guid clinicSectionId, string userName)
        {
            try
            {
                return _unitOfWork.Users.GetUserIdByUserName(clinicSectionId, userName);
            }
            catch (Exception ex) { throw ex; }
        }


        public void AddUserClinicSections(string itemList, Guid UserId, Guid clinicSectionId)
        {
            try
            {
                string[] ClinicSectionsIds = itemList.Split(',');
                List<ClinicSectionUser> clinicSectionUsers = new List<ClinicSectionUser>();
                if (itemList != "")
                {
                    foreach (var id in ClinicSectionsIds)
                    {
                        ClinicSectionUser clinicSectionUser = new ClinicSectionUser();
                        clinicSectionUser.UserId = UserId;
                        clinicSectionUser.ClinicSectionId = new Guid(id);
                        clinicSectionUser.Guid = Guid.NewGuid();
                        clinicSectionUsers.Add(clinicSectionUser);
                    }
                    Guid? ii = clinicSectionUsers.First().UserId;
                    List<ClinicSectionUser> userOldCS = _unitOfWork.ClinicSection_Users.Find(x => x.UserId == ii).ToList();
                    _unitOfWork.ClinicSection_Users.RemoveRange(userOldCS);
                    _unitOfWork.ClinicSection_Users.AddRange(clinicSectionUsers);
                    _unitOfWork.Complete();
                }
                else
                {
                    ClinicSectionUser clinicSectionUser = new ClinicSectionUser();
                    clinicSectionUser.UserId = UserId;
                    clinicSectionUser.ClinicSectionId = clinicSectionId;
                    clinicSectionUser.Guid = Guid.NewGuid();
                    clinicSectionUsers.Add(clinicSectionUser);
                    Guid? ii = clinicSectionUsers.First().UserId;
                    List<ClinicSectionUser> userOldCS = _unitOfWork.ClinicSection_Users.Find(x => x.UserId == ii).ToList();
                    _unitOfWork.ClinicSection_Users.RemoveRange(userOldCS);
                    _unitOfWork.ClinicSection_Users.AddRange(clinicSectionUsers);
                    _unitOfWork.Complete();
                }

            }
            catch (Exception ex) { throw ex; }
        }



        public void RemoveUserClinicSection(Guid userClinicSectionId)
        {
            ClinicSectionUser UCS = _unitOfWork.ClinicSection_Users.Get(userClinicSectionId);
            _unitOfWork.ClinicSection_Users.Remove(UCS);
            _unitOfWork.Complete();
        }

        public IEnumerable<UserInformationViewModel> GetAllUsers(Guid clinicSectionId, Guid humanId/*, string UserAccessType*/)
        {
            List<User> alluser = new List<User>();
            //if (UserAccessType == "ClinicAdmin" || UserAccessType == "FullAccessClinicAdmin")
            //{
            //     alluser = _unitOfWork.Users.GetAllClinicUsersExept("Patient", clinicId).ToList();

            //}
            //else
            //{
            //Guid ClinicId = _unitOfWork.ClinicSections.GetSingle(x => x.Guid == clinicSectionId).ClinicId;
            if (humanId == Guid.Empty)
            {
                alluser = _unitOfWork.Users.GetAllClinicSectionUsersExept("Patient", clinicSectionId).ToList();
            }
            else
            {
                alluser = _unitOfWork.Users.GetAllClinicSectionUsersExept("Patient", clinicSectionId, p => p.Guid == humanId).ToList();
            }

            //}


            List<UserInformationViewModel> users = Common.ConvertModels<UserInformationViewModel, User>.convertModelsLists(alluser).ToList();
            Indexing<UserInformationViewModel> indexing = new Indexing<UserInformationViewModel>();
            return indexing.AddIndexing(users);
        }




        public void loginSettings(dynamic viewBag)
        {
            viewBag.ShowCaptcha = GetValueOfSoftwareSetting("ShowCaptcha");
            viewBag.ShowClinicCode = GetValueOfSoftwareSetting("ShowClinicCode");
        }
        public string GetValueOfSoftwareSetting(string settingName)
        {
            try
            {
                return _unitOfWork.SoftwareSettings.GetSingle(x => x.SettingName == settingName).Value.ToString();
            }
            catch (Exception ex) { throw ex; }

        }
        public bool GetSoftwareSetting(string setting)
        {
            if (GetValueOfSoftwareSetting(setting) == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string encrypt(string clearText, string key)
        {
            string EncryptionKey = key;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


        public void AddNewUserToClinic(UserInformationViewModel user)
        {
            try
            {
                if (user.Pass3 != null)
                {
                    String password = Crypto.Hash(user.Pass3, "MD5");
                    //String password = encrypt(user.Pass3, "M.1370.d");

                    user.Pass3 = password;
                }
                else
                {
                    user.Pass3 = user.Pass3;
                }
                Random ra = new Random();
                String password1 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
                String password2 = Crypto.Hash(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
                String password4 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
                //String password1 = encrypt(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
                //String password2 = encrypt(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
                //String password4 = encrypt(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
                user.Pass1 = password1;
                user.Pass2 = password2;
                user.Pass4 = password4;
                User userDto = Common.ConvertModels<User, UserInformationViewModel>.convertModels(user);
                //Guid Id = _userService.AddNewUserToClinic(userDto);
                IEnumerable<ClinicSectionViewModel> allsections = _idunit.clinicSection.GetAllClinicSectionsBasedOnClinicId(user.ClinicSectionId ?? Guid.Empty);
                List<ClinicSectionUser> allUserClinicSection = new List<ClinicSectionUser>();

                foreach (var section in allsections)
                {
                    ClinicSectionUser ucs = new ClinicSectionUser() { ClinicSectionId = section.Guid, Guid = Guid.NewGuid() };
                    allUserClinicSection.Add(ucs);

                }

                User newUser = Common.ConvertModels<User, UserInformationViewModel>.convertModels(user);
                newUser.Guid = Guid.NewGuid();
                allUserClinicSection.ForEach(a => a.User = newUser);
                _unitOfWork.ClinicSection_Users.AddRange(allUserClinicSection);
                IEnumerable<UserProfile> ups = GetAllDefaultUserSettings(newUser.Guid);
                _unitOfWork.Settings.AddRange(ups);
                _unitOfWork.Complete();
            }
            catch (Exception ex) { throw ex; }
        }



        public UserInformationViewModel GetUser(Guid clinicSectionId, string userName)
        {
            try
            {
                User userDto = _unitOfWork.Users.GetUserBasedOnUserName(clinicSectionId, userName);
                return Common.ConvertModels<UserInformationViewModel, User>.convertModels(userDto);
            }
            catch { return null; }
        }


        public bool CheckRepeatedUserName(string username, Guid clinicSectionId, bool NewOrUpdate, string oldName = "")
        {
            try
            {
                User clinicSection_user = null;
                if (NewOrUpdate)
                {
                    clinicSection_user = _unitOfWork.Users.CheckUserExistBaseOnUserName(clinicSectionId, username);
                }
                else
                {
                    clinicSection_user = _unitOfWork.Users.CheckUserExistBaseOnUserName(clinicSectionId, username, oldName);
                }
                if (clinicSection_user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }


        public List<SubSystemsWithAccessViewModel> GetAllSubSystemsWithAccessNames(Guid userId, int sectionTypeId, Guid clinicSectionId, Guid parentUserId)
        {
            int LanguageId = _idunit.language.GetLanguageId();
            List<AllSubSystemsWithAccess> allSubSystems = _idunit.subSystem.GetAllSubSystemsWithAccessNames(userId, sectionTypeId, clinicSectionId, LanguageId, parentUserId).ToList();
            return Common.ConvertModels<SubSystemsWithAccessViewModel, AllSubSystemsWithAccess>.convertModelsLists(allSubSystems);
        }

        public void DeleteAllUserAccess(Guid userId, Guid clinicSectionId)
        {
            _idunit.subSystem.DeleteAllUserAccess(userId, clinicSectionId);
        }
        public void SaveAccess(Guid userId, int subSystemAccessId, Guid clinicSectionId)
        {
            //_subSystemService.SaveAccessForUser(userId, subSystemAccessId, clinicSectionId);
            throw new NotImplementedException();
        }

        public int CheckUserCount(string userName, string password, Guid clinicSectionId, int? userId)
        {
            try
            {
                var result = _unitOfWork.Users.GetUserCountByUserNameAndPass(userName, password, clinicSectionId, userId);
                return result;
            }
            catch (Exception e) { return 0; }
        }

        public UserInformationViewModel CheckUserExist(string userName, string password, Guid clinicSectionId, int? userId)
        {
            try
            {
                User userDto = _unitOfWork.Users.GetUserByUserNameAndPass(userName, password, clinicSectionId, userId);
                return ConvertToUser(userDto);
                //Common.ConvertModels<UserInformationViewModel, User>.convertModels(userDto);
            }
            catch (Exception e) { return null; }
        }

        public UserInformationViewModel CheckAdminExist(string userName, string password)
        {
            try
            {
                User userDto = _unitOfWork.Users.CheckAdminLogin(userName, password);
                return ConvertToUser(userDto);
                //Common.ConvertModels<UserInformationViewModel, User>.convertModels(userDto);
            }
            catch (Exception e) { return null; }
        }

        public static UserInformationViewModel ConvertToUser(User ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserInformationViewModel>()
                .ForMember(a => a.ClinicSectionGuids, b => b.MapFrom(c => c.ClinicSectionUsers.OrderBy(x => x.Id).Select(x => x.ClinicSectionId)))
                .ForMember(a => a.ClinicSectionUsers, b => b.Ignore())
                ;

            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<User, UserInformationViewModel>(ress);
        }




        public UserInformationViewModel CheckUserExist(string userName, string password)
        {
            User user = _unitOfWork.Users.GetSingle(x => x.UserName == userName && x.Pass3 == password);
            if (user == null)
            {
                return null;
            }
            else
            {
                return Common.ConvertModels<UserInformationViewModel, User>.convertModels(user);
            }
        }

        public string UpdateUser(UserInformationViewModel user, bool IsMD5 = false)
        {
            if (user.Pass3 != user.Pass4)
                return "IncorrectPass";

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber) && user.PhoneNumber.Length < 8)
                return "WrongMobile";

            if (!string.IsNullOrWhiteSpace(user.Email) && !user.Email.IsValidEmail())
                return "WrongEmail";

            if (user.Pass3 != null)
            {
                string password = user.Pass3;
                if (!IsMD5)
                {
                    password = Crypto.Hash(user.Pass3, "MD5");
                    //password = encrypt(user.Pass3, "M.1370.d");
                }

                //String Encryptedpassword = StringCipher.Encrypt(password, Common.Secret);
                user.Pass3 = password;
                Random ra = new Random();
                string password1 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
                string password2 = Crypto.Hash(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
                string password4 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
                //String password1 = encrypt(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
                //String password2 = encrypt(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
                //String password4 = encrypt(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
                user.Pass1 = password1;
                user.Pass2 = password2;
                user.Pass4 = password4;
            }
            User userDto = ConvertViewModelToUser(user);
            userDto.IsUser = true;
            return _unitOfWork.Users.UpdateUser(userDto).ToString();

        }

        public bool IsUserNameExist(string username, Guid clinicSectionId, bool newOrUpdate, string currentUserName)
        {
            try
            {
                User clinicSection_user = null;
                if (newOrUpdate)
                {
                    clinicSection_user = _unitOfWork.Users.CheckUserExistBaseOnUserName(clinicSectionId, username);
                }
                else
                {
                    clinicSection_user = _unitOfWork.Users.CheckUserExistBaseOnUserName(clinicSectionId, username, currentUserName);
                }
                if (clinicSection_user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }


        public bool IsNameExist(string name, Guid clinicSectionId, bool newOrUpdate, string currentName)
        {
            try
            {
                User clinicSection_user = null;
                if (newOrUpdate)
                {
                    clinicSection_user = _unitOfWork.Users.CheckUserExistBaseOnName(clinicSectionId, name);
                }
                else
                {
                    clinicSection_user = _unitOfWork.Users.CheckUserExistBaseOnName(clinicSectionId, name, currentName);
                }
                if (clinicSection_user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public void UserActivationStatus(Guid id)
        {
            try
            {
                User user = _unitOfWork.Users.Get(id);
                if ((bool?)user.Active ?? false)
                {
                    user.Active = false;
                }
                else
                {
                    user.Active = true;
                }
                _unitOfWork.Users.UpdateActivationStatus(user);
                _unitOfWork.Complete();
            }
            catch { }
        }

        public void loginTheme(dynamic viewBag)
        {
            viewBag.Theme = LoadLoginThemeCookie();
        }

        public IEnumerable<UserInformationViewModel> GetAllUsers()
        {
            IEnumerable<User> allUsers = _unitOfWork.Users.GetAll();
            List<UserInformationViewModel> users = Common.ConvertModels<UserInformationViewModel, User>.convertModelsLists(allUsers).ToList();
            Indexing<UserInformationViewModel> indexing = new Indexing<UserInformationViewModel>();
            return indexing.AddIndexing(users);
        }

        public IEnumerable<UserInformationViewModel> GetAllUsersExeptUserPortions(Guid clinicSectionId)
        {
            IEnumerable<User> allUsers = _unitOfWork.Users.GetAllUsersExeptUserPortions(clinicSectionId);
            return Common.ConvertModels<UserInformationViewModel, User>.convertModelsLists(allUsers);
        }

        public string AddNewUser(UserInformationViewModel newUser)
        {
            if (newUser.Pass3 != newUser.Pass4)
                return "IncorrectPass";

            if (!string.IsNullOrWhiteSpace(newUser.PhoneNumber) && newUser.PhoneNumber.Length < 8)
                return "WrongMobile";

            if (!string.IsNullOrWhiteSpace(newUser.Email) && !newUser.Email.IsValidEmail())
                return "WrongEmail";

            if (newUser.Pass3 != null)
            {
                String password = Crypto.Hash(newUser.Pass3, "MD5");
                //String password = encrypt(newUser.Pass3, "M.1370.d");
                //String Encryptedpassword = StringCipher.Encrypt(password, Common.Secret);
                newUser.Pass3 = password;
            }
            else
            {
                newUser.Pass3 = newUser.Pass3;
            }
            Random ra = new Random();
            String password1 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
            String password2 = Crypto.Hash(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
            String password4 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
            //String password1 = encrypt(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
            //String password2 = encrypt(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
            //String password4 = encrypt(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
            newUser.Pass1 = password1;
            newUser.Pass2 = password2;
            newUser.Pass4 = password4;
            User userDto = ConvertViewModelToUser(newUser);
            userDto.Guid = Guid.NewGuid();
            userDto.Active = true;
            userDto.IsUser = true;
            _unitOfWork.Users.Add(userDto);
            IEnumerable<UserProfile> ups = GetAllDefaultUserSettings(userDto.Guid);
            _unitOfWork.Settings.AddRange(ups);

            var userType = _unitOfWork.BaseInfoGenerals.GetByIdAndType(newUser.UserTypeId == null ? 0 : newUser.UserTypeId.Value, "UserType");
            if (userType != null && userType.Name == "Doctor")
            {
                var doctor = new Doctor
                {
                    Guid = userDto.Guid,
                    ClinicSectionId = userDto.ClinicSectionId
                };

                _unitOfWork.Doctor.Add(doctor);
            }

            _unitOfWork.Complete();
            return userDto.Guid.ToString();

        }

        public static User ConvertViewModelToUser(UserInformationViewModel ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserInformationViewModel, User>();
                cfg.CreateMap<ClinicSectionUserViewModel, ClinicSectionUser>();

            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<UserInformationViewModel, User>(ress);
        }


        public List<UserProfile> GetAllDefaultUserSettings(Guid UserId)
        {
            List<UserProfile> ups = new List<UserProfile>();
            UserProfile up = new UserProfile
            {
                UserId = UserId,
                SettingId = 1,
                SettingValueId = 2,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 2,
                SettingValueId = 5,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 3,
                SettingValueId = 10,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 5,
                SettingValueId = 18,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 6,
                SettingValueId = 20,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 7,
                SettingValueId = 22,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 8,
                SettingValueId = 24,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 11,
                SettingValueId = 26,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 12,
                SettingValueId = 33,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 13,
                SettingValueId = 32,
                User = null
            };
            ups.Add(up);


            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 14,
                SettingValueId = 35,
                User = null
            };
            ups.Add(up);

            up = new UserProfile
            {
                UserId = UserId,
                SettingId = 15,
                SettingValueId = 37,
                User = null
            };
            ups.Add(up);

            return ups;
        }

        public UserInformationViewModel GetUser(Guid userId)
        {
            try
            {
                User user = _unitOfWork.Users.GetUserWithClinicSections(userId);
                return ConvertToUser(user);
            }
            catch { return null; }
        }

        public UserInformationViewModel GetUserWithAccess(Guid userId)
        {
            try
            {
                User user = _unitOfWork.Users.GetUserWithAccess(userId);
                return Common.ConvertModels<UserInformationViewModel, User>.convertModels(user);
            }
            catch { return null; }
        }

        public UserInformationViewModel GetUserWithRole(Guid userId)
        {
            User user = _unitOfWork.Users.GetUserWithRole(userId);
            return Common.ConvertModels<UserInformationViewModel, User>.convertModels(user);

        }

        public bool CheckRepeated(string name, string nameHolder = "")
        {
            try
            {
                User ad = _unitOfWork.Users.GetSingle(x => x.UserName == name && x.UserName != nameHolder);

                if (ad != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }


        public OperationStatus RemoveUser(Guid userId)
        {
            try
            {
                _unitOfWork.Users.RemoveUser(userId);
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

        public IEnumerable<UserInformationViewModel> GetAllDoctors()
        {
            try
            {
                IEnumerable<User> allDoctor = _unitOfWork.Users.Find(x => x.UserTypeId == 1);
                return Common.ConvertModels<UserInformationViewModel, User>.convertModelsLists(allDoctor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditCurrentUser(UserInformationViewModel user)
        {

            try
            {
                User oldUser = _unitOfWork.Users.Get(user.Guid);
                String password = Crypto.Hash(user.Pass3, "MD5");
                oldUser.UserName = user.UserName;
                oldUser.Pass3 = password;
                _unitOfWork.Complete();
            }
            catch { }

        }

        public bool PastPassIsCorrect(UserInformationViewModel user)
        {
            User oldUser = _unitOfWork.Users.Get(user.Guid);
            String password = Crypto.Hash(user.Pass2, "MD5");

            if (oldUser.Pass3 != password)
                return false;
            return true;
        }

        
    }
}
