using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class SettingRepository : Repository<UserProfile>, ISettingRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SettingRepository(WASContext context)
            : base(context)
        {
        }
       
        public string GetValueOfSetting(int valueId)
        {
            return _context.ValueOfSettings.AsNoTracking().SingleOrDefault(x => x.Id == valueId).Name;
        }
        public int GetValueIdBySettingValueId(int settingValueId)
        {
            return _context.SettingValueOfSettings.AsNoTracking().SingleOrDefault(x => x.Id == settingValueId).ValueId;
        }
        public UserProfile GetUserProfile(Guid userId, int settingId)
        {
            return _context.UserProfiles.AsNoTracking().SingleOrDefault(x => x.UserId == userId && x.SettingId == settingId);
        }
        public int GetSettingIdByName(string settingName)
        {
            return _context.Settings.AsNoTracking().SingleOrDefault(x => x.Name == settingName).Id;
        }
        public int GetValueOfSettingIdByName(string ValueName)
        {
            return _context.ValueOfSettings.AsNoTracking().SingleOrDefault(x => x.Name == ValueName).Id;
        }
        public int GetSettingValueIdInSettingValueOfSetting(int settingId, int valueId)
        {
            return _context.SettingValueOfSettings.AsNoTracking().SingleOrDefault(x => x.SettingId == settingId && x.ValueId == valueId).Id;
        }
        public int GetSettingValueIdInUserProfile(Guid userId, int settingId)
        {
            return _context.UserProfiles.AsNoTracking().SingleOrDefault(x => x.UserId == userId && x.SettingId == settingId).SettingValueId;
        }
        public List<UserProfile> GetAllUserSettings(Guid userId)
        {
            return _context.UserProfiles
                .Include(x => x.Setting)
                .Include(x => x.SettingValue).ThenInclude(x => x.Value)
                .Where(x => x.UserId == userId).ToList();
        }
    }
}


