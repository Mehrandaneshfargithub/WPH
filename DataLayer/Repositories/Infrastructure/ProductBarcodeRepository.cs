using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ProductBarcodeRepository : Repository<ProductBarcode>, IProductBarcodeRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ProductBarcodeRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ProductBarcode> GetAllProductBarcodeByProductId(Guid productId)
        {
            return _context.ProductBarcodes.AsNoTracking()
                .Where(p => p.ProductId == productId);
        }

        public bool CheckBarcodeExist(Guid? productBarcodeId, string barcode, Guid clinicSectionId)
        {
            IQueryable<ProductBarcode> result = _context.ProductBarcodes.AsNoTracking()
                .Where(p => p.Barcode == barcode && p.Product.ClinicSectionId == clinicSectionId);

            if (productBarcodeId != null)
                result = result.Where(p => p.Guid != productBarcodeId);

            return result.Any();
        }
    }
}
