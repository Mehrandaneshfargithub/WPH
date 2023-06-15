using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.Settings;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    

    public class SettingMvcMockingService : ISettingMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public SettingMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
        }

        public void Update(string SettingName, string SettingVal, string UserName)
        {
            Guid userId = _unitOfWork.Users.GetSingle(x => x.UserName == UserName).Guid;
            int settingId = _unitOfWork.Settings.GetSettingIdByName(SettingName);
            int valueId = _unitOfWork.Settings.GetValueOfSettingIdByName(SettingVal);
            int settingValueId = _unitOfWork.Settings.GetSettingValueIdInSettingValueOfSetting(settingId, valueId);
            UserProfile userProfile = _unitOfWork.Settings.GetUserProfile(userId, settingId);
            userProfile.SettingValueId = settingValueId;
            _unitOfWork.Complete();
        }

        public string GetValueOfSetting(string settingName, string UserName)
        {
            try
            {
                Guid userId = _unitOfWork.Users.GetSingle(x => x.UserName == UserName).Guid;
                int settingId = _unitOfWork.Settings.GetSettingIdByName(settingName);
                int settingValueId = _unitOfWork.Settings.GetSettingValueIdInUserProfile(userId, settingId);
                int valueId = _unitOfWork.Settings.GetValueIdBySettingValueId(settingValueId);
                return _unitOfWork.Settings.GetValueOfSetting(valueId);
            }
            catch (Exception ex) { throw ex; }
        }

        public void SaveSetting(string settingName, string settingValue, string userName)
        {
            Update(settingName, settingValue, userName);
        }
        public void saveMenu(string menu, string userName)
        {
            if (menu == "menu1")
            {
                Update("Navigation", "TopMenu", userName);
                Update("TwoSideBar", "False", userName);
                Update("verticalSideBar", "noSideBar", userName);
                string currentHorizontalSideBarValue = GetValueOfSetting("horizontalSideBar", userName);
                if (currentHorizontalSideBarValue == "noSideBar")
                {
                    Update("horizontalSideBar", "normalSideBar", userName);
                }
                else
                {
                    Update("horizontalSideBar", currentHorizontalSideBarValue, userName);
                }
            }
            else if (menu == "menu2" || menu == "menu3" || menu == "menu4" || menu == "menu5")
            {
                string navigationValue = string.Empty;
                string twoSideBarValue = "False";
                switch (menu)
                {
                    case "menu2":
                        navigationValue = "TowMenu1";
                        twoSideBarValue = "True";
                        break;
                    case "menu3":
                        navigationValue = "MobileMenu1";
                        break;
                    case "menu4":
                        navigationValue = "MobileMenu2";
                        break;
                    case "menu5":
                        navigationValue = "MobileMenu3";
                        break;
                }
                Update("Navigation", navigationValue, userName);
                Update("TwoSideBar", twoSideBarValue, userName);
                Update("horizontalSideBar", "noSideBar", userName);
                string currentVerticalSideBarValue = GetValueOfSetting("verticalSideBar", userName);
                if (currentVerticalSideBarValue == "noSideBar")
                {
                    Update("verticalSideBar", "normalSideBar", userName);
                }
                else
                {
                    Update("verticalSideBar", currentVerticalSideBarValue, userName);
                }
            }
        }


        public void saveMenuMinHorizontal(int menuMin, string userName)
        {
            if (menuMin == 1)
            {
                Update("horizontalSideBar", "minimumSideBar", userName);
            }
            else if (menuMin == 2)
            {
                Update("horizontalSideBar", "normalSideBar", userName);
            }
        }
        public void saveMenuMinVertical(int menuMin, string userName)
        {
            if (menuMin == 1)
            {
                Update("verticalSideBar", "minimumSideBar", userName);
            }
            else if (menuMin == 2)
            {
                Update("verticalSideBar", "compactMinSideBar", userName);
            }
            else if (menuMin == 3)
            {
                Update("verticalSideBar", "normalSideBar", userName);
            }
        }
        public void loadSettings(Guid userId)
        {
            try
            {
                List<SettingViewModel> settings = GetAllUserSettings(userId);
                //session["Rtl"] = "False";
                //viewBag.NameOfUser = userName;
            }
            catch (Exception ex) { throw ex; }

        }

        public List<SettingViewModel> GetAllUserSettings(Guid userId)
        {
            try
            {
                return GetAllValueOfSetting(userId);
            }
            catch (Exception ex) { throw ex; }

        }


        public List<SettingViewModel> GetAllValueOfSetting(Guid userId)
        {
            try
            {
                //Guid userId = _unitOfWork.Users.GetSingle(x => x.UserName == UserName).Guid;
                List<UserProfile> settings = _unitOfWork.Settings.GetAllUserSettings(userId);
                return convertModelsLists(settings);
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<SettingViewModel> convertModelsLists(IEnumerable<UserProfile> langExp)
        {
            List<SettingViewModel> expDtoList = new List<SettingViewModel>();
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<UserProfile, Setting>();
            //});
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfile, SettingViewModel>()
                .ForMember(a => a.UserId, b => b.MapFrom(c => c.UserId))
                .ForMember(a => a.SettingName, b => b.MapFrom(c => c.Setting.Name))
                .ForMember(a => a.ValueName, b => b.MapFrom(c => c.SettingValue.Value.Name));
            });
            IMapper mapper = config.CreateMapper();
            expDtoList = mapper.Map<IEnumerable<UserProfile>, List<SettingViewModel>>(langExp);
            return expDtoList;
        }


        public string GetNavigationType(string selectedNavigation, string userName)
        {
            if (selectedNavigation == "ace-settings-menus_1")
            {
                return "/Views/Shared/PartialViews/InterfacePartials/Navigations/_navigationMenu_TopMenu.cshtml";
            }
            else if (selectedNavigation == "ace-settings-menus_2")
            {
                return "/Views/Shared/PartialViews/InterfacePartials/Navigations/_navigationMenu_TowMenu1.cshtml";
            }
            else if (selectedNavigation == "ace-settings-menus_3")
            {
                return "/Views/Shared/PartialViews/InterfacePartials/Navigations/_navigationMenu_MobileMenu1.cshtml";
            }
            else if (selectedNavigation == "ace-settings-menus_4")
            {
                return "/Views/Shared/PartialViews/InterfacePartials/Navigations/_navigationMenu_MobileMenu2.cshtml";
            }
            else if (selectedNavigation == "ace-settings-menus_5")
            {
                return "/Views/Shared/PartialViews/InterfacePartials/Navigations/_navigationMenu_MobileMenu3.cshtml";
            }
            else
            {
                return string.Empty;
            }
        }
        public void DetrmineRightToLeftBasedOnCulture(string culture, string userName)
        {
            //LanguageMvcService lService = new LanguageMvcService(request, response);
            culture = _idunit.language.LoadLanguageCookie();
            if (culture == "ar" || culture == "ku")
            {
                Update("Rtl", "True", userName);
            }
            else
            {
                Update("Rtl", "False", userName);
            }
        }

        public void GetRightToLeftSettings(dynamic viewBag, string userName)
        {
            //viewBag.Rtl = session["Rtl"];
        }

        

    }


}
