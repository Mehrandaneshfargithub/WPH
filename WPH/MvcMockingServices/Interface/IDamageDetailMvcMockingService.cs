using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.DamageDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface IDamageDetailMvcMockingService
    {
        string RemoveDamageDetail(Guid DamageDetailid);
        string AddNewDamageDetail(DamageDetailViewModel DamageDetail, Guid clinicSectionId);
        string UpdateDamageDetail(DamageDetailViewModel DamageDetail, Guid clinicSectionId);
        IEnumerable<DamageDetailViewModel> GetAllDamageDetails(Guid clinicSectionId);
        DamageDetailViewModel GetDamageDetailForEdit(Guid medicineDamagedDetailId);
        void GetModalsViewBags(dynamic viewBag);
    }
}
