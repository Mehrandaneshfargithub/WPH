using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISettingRepository : IRepository<UserProfile>
    {
        string GetValueOfSetting(int valueId);
        int GetValueIdBySettingValueId(int settingValueId);
        UserProfile GetUserProfile(Guid userId, int settingId);
        int GetSettingIdByName(string settingName);
        int GetValueOfSettingIdByName(string ValueName);
        int GetSettingValueIdInSettingValueOfSetting(int settingId, int valueId);
        int GetSettingValueIdInUserProfile(Guid userId, int settingId);
        List<UserProfile> GetAllUserSettings(Guid userId);
    }
}
