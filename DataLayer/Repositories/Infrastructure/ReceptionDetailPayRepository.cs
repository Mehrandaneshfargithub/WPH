using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReceptionDetailPayRepository : Repository<ReceptionDetailPay>, IReceptionDetailPayRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionDetailPayRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReceptionDetailPay> GetAllReceptionDetailPayBySpecification(Guid receptionId, bool specification)
        {
            return _context.ReceptionDetailPays.Include(a => a.UserPortion).ThenInclude(a => a.User)
                .Where(a=>a.ReceptionId == receptionId && a.UserPortion.Specification == specification)
                .Select(a => new ReceptionDetailPay
            {
                Amount = a.Amount,
                Guid = a.Guid,
                UserPortion = new UserPortion
                {
                    Specification = a.UserPortion.Specification,
                    PortionPercent = a.UserPortion.PortionPercent,
                    User = new User
                    {
                        Name = a.UserPortion.User.Name,
                    }
                }
                
            });
        }
    }
}
