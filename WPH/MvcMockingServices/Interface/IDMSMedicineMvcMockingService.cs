using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Product;

namespace WPH.MvcMockingServices.Interface
{
    public interface IDMSMedicineMvcMockingService
    {
        IEnumerable<ProductViewModel> GetAllProduct();
    }
}
