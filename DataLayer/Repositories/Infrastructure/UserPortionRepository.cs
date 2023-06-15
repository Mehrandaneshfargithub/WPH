using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class UserPortionRepository : Repository<UserPortion>, IUserPortionRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public UserPortionRepository(WASContext context)
            : base(context)
        {
        }
        

        public IEnumerable<UserPortion> GetAllUserPortions(Guid clinicSectionId)
        {
            return _context.UserPortions.Include(a => a.User)
                .Where(a => a.User.ClinicSectionId == clinicSectionId)
                .Select(a => new UserPortion
                {
                    Guid = a.Guid,
                    PortionPercent = a.PortionPercent,
                    Specification = a.Specification,
                    User = new User
                    {
                        Name = a.User.Name
                    }
                });
        }

        public IEnumerable<UserPortion> GetAllUserPortionsBySpecification(Guid clinicSectionId, bool specification,Guid ReceptionId)
        {
            try
            {

                return _context.UserPortions
                    .Include(a => a.ReceptionDetailPays)
                    .Include(a => a.User)
                    .Where(a=>a.User.ClinicSectionId == clinicSectionId && a.Specification == specification && !_context.ReceptionDetailPays.Where(a=>a.ReceptionId == ReceptionId).Select(a=>a.UserPortionId).Contains(a.Guid))
                    .Select(a => new UserPortion
                    {
                        Guid = a.Guid,
                        PortionPercent = a.PortionPercent,
                        User = new User
                        {
                            Name = a.User.Name
                        }
                    });


            }
            catch(Exception e)
            {
                throw e;
            }
            

        }

        public IEnumerable<UserPortionReport> GetAllUserPortionForReport(Guid userId, DateTime fromDate, DateTime toDate, bool detail)
        {
            try
            {
                IQueryable<UserPortionReport> result;

                if (!detail)
                {
                    result = _context.ReceptionDetailPays
                    .Include(a => a.Reception)
                    .Include(a => a.UserPortion).ThenInclude(a => a.User)
                    .Where(a => a.Reception.ReceptionDate >= fromDate && a.Reception.ReceptionDate <= toDate)
                    .Select(a => new UserPortionReport
                    {
                        UserPortionId = a.UserPortionId.Value,
                        ReceptionDate = a.Reception.ReceptionDate.Value,
                        DoctorName = a.UserPortion.User.Name,
                        Amount = a.Amount.Value
                    });



                    if(userId != Guid.Empty)
                    {
                        result = result.Where(a => a.UserPortionId == userId);
                    }

                    var all = result.GroupBy(a => new { a.ReceptionDate, a.DoctorName }).Select(a => new UserPortionReport
                    {
                        ReceptionDate = a.Key.ReceptionDate,
                        DoctorName = a.Key.DoctorName,
                        Amount = a.Sum(a => a.Amount)
                    });

                    return all;
                }
                else
                {
                    result = _context.ReceptionDetailPays
                    .Include(a => a.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User)
                    .Include(a => a.Reception).ThenInclude(a => a.ReceptionServices)
                    .Include(a => a.UserPortion).ThenInclude(a => a.User)
                    .Where(a => a.Reception.ReceptionDate >= fromDate && a.Reception.ReceptionDate <= toDate)
                    .Select(a => new UserPortionReport
                    {

                        UserPortionId = a.UserPortionId.Value,
                        ReceptionDate = a.Reception.ReceptionDate.Value,
                        DoctorName = a.UserPortion.User.Name,
                        Amount = a.Amount.Value,
                        PatientName = a.Reception.Patient.User.Name,
                        ServiceAmount = a.Reception.ReceptionServices.Sum(a=>a.Price??0),
                        
                    });

                    if (userId != Guid.Empty)
                    {
                        result = result.Where(a => a.UserPortionId == userId);
                    }

                    

                    return result;

                }
                


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<UserPortionReport> GetPortionReport(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, string status, Guid doctorId)
        {
            throw new NotImplementedException();
        }
    }
}
