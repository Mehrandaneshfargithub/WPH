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
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ItemRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<Item> GetAllItem(Guid clinicSectionId)
        {
            return Context.Items.AsNoTracking()
                .Include(p => p.ItemType)
                .Include(p => p.Section)
                .Where(p => p.ClinicSectionId == clinicSectionId);
        }

        public Item GetWithType(Guid itemId)
        {
            return Context.Items.AsNoTracking()
                .Include(p => p.ItemType)
                .Include(p => p.Section)
                .SingleOrDefault(p => p.Guid == itemId);
        }
    }
}
