using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class ClinicSection_UserRepository : Repository<ClinicSectionUser>, IClinicSection_UserRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ClinicSection_UserRepository(WASContext context)
            : base(context)
        {
        }
        public ClinicSectionUser ClinicSection_User_BasedClinicIdAndUserName(Guid ClinicId,string userName, string currentUserName = "")
        {
            return _context.ClinicSectionUsers.AsNoTracking()
                .Include(x=>x.ClinicSection)
                .Include(x => x.User)
                .Where(x=>x.User.UserName == userName && x.User.UserName != currentUserName && x.ClinicSection.ClinicId == ClinicId).FirstOrDefault();
        }
        public ClinicSectionUser ClinicSection_User_BasedClinicIdAndNameAndPhoneNumber(Guid ClinicId, string Name, string number, string currentName = "", string currentnumber = "")
        {
            return _context.ClinicSectionUsers.AsNoTracking()
                .Include(x => x.ClinicSection)
                .Include(x => x.User)
                .Where(x => x.User.Name == Name && x.User.PhoneNumber == number && x.User.Name != currentName && x.User.PhoneNumber != currentnumber && x.ClinicSection.ClinicId == ClinicId).FirstOrDefault();
        }

        public IEnumerable<ClinicSection> GetClinicSectionsForUser(Guid userId, string type, Guid? parentId = null)
        {
            if(type.ToLower() == "labratory")
            {
                return _context.ClinicSectionUsers.AsNoTracking()
                .Where(x => x.UserId == userId && x.ClinicSection.ClinicSectionType.Name == "Labratory" && x.ClinicSection.ParentId == parentId)
                .Include(x => x.ClinicSection).ThenInclude(x=>x.ClinicSectionType).OrderBy(x => x.Id)
                .Select(x => x.ClinicSection);
            }
            else if(type.ToLower() == "radiology")
            {
                return _context.ClinicSectionUsers.AsNoTracking()
                .Where(x => x.UserId == userId && x.ClinicSection.ClinicSectionType.Name == "Radiology" && x.ClinicSection.ParentId == parentId)
                .Include(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType).OrderBy(x => x.Id)
                .Select(x => x.ClinicSection);
            }
            else if (type.ToLower() == "notlab")
            {
                return _context.ClinicSectionUsers.AsNoTracking()
                .Where(x => x.UserId == userId && x.ClinicSection.ClinicSectionType.Name == "NotLab" && x.ClinicSection.ParentId == parentId)
                .Include(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType).OrderBy(x => x.Id)
                .Select(x => x.ClinicSection);
            }
            else if (type.ToLower() == "labratoryradiology")
            {
                return _context.ClinicSectionUsers.AsNoTracking()
                .Where(x => x.UserId == userId && (x.ClinicSection.ClinicSectionType.Name == "Labratory" || x.ClinicSection.ClinicSectionType.Name == "Radiology") && x.ClinicSection.ParentId == parentId)
                .Include(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType).OrderBy(x => x.Id)
                .Select(x => x.ClinicSection);
            }
            else
            {
                return _context.ClinicSectionUsers.AsNoTracking()
                    .Include(x => x.ClinicSection).ThenInclude(a=>a.SectionType).OrderBy(x => x.Id)
                .Where(x => x.UserId == userId && x.ClinicSection.ParentId == parentId)
                .Select(x => x.ClinicSection);
            }

        }

        public IEnumerable<ClinicSection> GetAllUserClinicSections(Guid userId, Guid clicnicSectionId)
        {
            return _context.ClinicSectionUsers.AsNoTracking()
                .Where(x => x.UserId == userId && x.ClinicSectionId != clicnicSectionId && x.ClinicSection.ParentId != clicnicSectionId)
                .Include(x => x.ClinicSection)
                .Select(x => x.ClinicSection)
                .OrderBy(x => x.Id);
        }

        public IEnumerable<ClinicSection> GetAllClinicSectionsForUserWithParent(Guid userId, Guid parentId)
        {
            return _context.ClinicSectionUsers.AsNoTracking()
                .Join(_context.ClinicSections,
                CSU => CSU.ClinicSectionId,
                CS => CS.Guid,
                (CSU, CS) => CSU
                )
                .Where(x => (x.UserId == userId && x.ClinicSection.ParentId == parentId) || x.ClinicSection.Guid == parentId)
                .Include(x => x.ClinicSection).OrderBy(x => x.Id)
                .Select(x => x.ClinicSection);

            return _context.ClinicSections.AsNoTracking()
                .Join(_context.ClinicSectionUsers.Include(x => x.ClinicSection).Where(a => a.UserId == userId),
                CS => CS.Guid,
                CSU => CSU.ClinicSectionId,
                (CS, CSU) => CS
                )
                .Where(x => (x.ParentId == parentId) || x.Guid == parentId)
                .OrderBy(x => x.Id);

        }

        public IEnumerable<ClinicSection> GetAllClinicSectionParentsForUser(Guid userId, Guid clinicId)
        {
            return _context.ClinicSectionUsers.Include(x => x.ClinicSection).AsNoTracking()
                .Where(x => x.UserId == userId && x.ClinicSection.ClinicId == clinicId && x.ClinicSection.ClinicSectionTypeId == null)
                .Select(x => x.ClinicSection);
        }
    }
}
