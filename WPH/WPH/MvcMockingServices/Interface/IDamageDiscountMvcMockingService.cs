using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.DamageDiscount;

namespace WPH.MvcMockingServices.Interface
{
    public interface IDamageDiscountMvcMockingService
    {
        IEnumerable<DamageDiscountViewModel> GetAllDamageDiscounts(Guid damageId);

        string AddNewDamageDiscount(DamageDiscountViewModel viewModel);
        string UpdateDamageDiscount(DamageDiscountViewModel viewModel);
        string RemoveDamageDiscount(Guid damageDiscountId);
        DamageDiscountViewModel GetDamageDiscount(Guid damageDiscountId);

    }
}
