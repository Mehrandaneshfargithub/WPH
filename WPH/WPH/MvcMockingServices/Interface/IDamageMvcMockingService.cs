using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using WPH.Models.Damage;

namespace WPH.MvcMockingServices.Interface
{
    public interface IDamageMvcMockingService
    {
        string RemoveDamage(Guid PurchaseInvoiceid, Guid userId, string pass);
        string AddNewDamage(DamageViewModel viewModel);
        string UpdateDamage(DamageViewModel viewModel);
        IEnumerable<DamageViewModel> GetAllDamages(Guid clinicSectionId, DamageFilterViewModel filterViewModel);
        DamageViewModel GetDamage(Guid PurchaseInvoiceId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<DamageTotalPriceViewModel> GetAllTotalPrice(Guid damageId);
        string UpdateTotalPrice(Damage invoice);
    }
}
