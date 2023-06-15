using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Item;

namespace WPH.MvcMockingServices.Interface
{
    public interface IItemMvcMockingService
    {
        OperationStatus RemoveItem(Guid Itemid);
        string AddNewItem(ItemViewModel Item);
        string UpdateItem(ItemViewModel Item);
        bool CheckRepeatedItemName(Guid clinicSectionId, string name, bool NewOrUpdate, string oldName = "");
        IEnumerable<ItemViewModel> GetAllItems(Guid clinicSectionId);
        ItemViewModel GetItem(Guid ItemId);
        void GetModalsViewBags(dynamic viewBag);
    }
}
