using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IProductBarcodeRepository : IRepository<ProductBarcode>
    {
        IEnumerable<ProductBarcode> GetAllProductBarcodeByProductId(Guid productId);
        bool CheckBarcodeExist(Guid? productBarcodeId, string barcode, Guid clinicSectionId);
    }
}
