using DMSDataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.Repositories.Interfaces
{
    public interface IDMSMedicineRepository : IRepository<TblMedicine>
    {
        IEnumerable<FN_MedicineNumModel> GetAllProduct();
        IEnumerable<FN_MedicineNumModel> GetAllProductReceptionsByIds(IEnumerable<int?> allDMSProductId);
        IEnumerable<FN_MedicineNumModel> GetAllProductDMSNamesByIds(IEnumerable<int?> allDMSProductId);
    }
}
