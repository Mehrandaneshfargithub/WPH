using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        IEnumerable<Item> GetAllItem(Guid clinicSectionId);
        Item GetWithType(Guid itemId);
    }
}
