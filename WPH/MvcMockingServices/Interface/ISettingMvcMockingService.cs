using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.Settings;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISettingMvcMockingService
    {
        void SaveSetting(string settingName, string settingValue, string userName);
        void saveMenu(string menu, string userName);
        void saveMenuMinHorizontal(int menuMin, string userName);
        void saveMenuMinVertical(int menuMin, string userName);
        void loadSettings(Guid userId);
        List<SettingViewModel> GetAllUserSettings(Guid userId);
        string GetNavigationType(string selectedNavigation, string userName);
        void DetrmineRightToLeftBasedOnCulture(string culture, string userName);
        void GetRightToLeftSettings(dynamic viewBag, string userName);


    }
}
