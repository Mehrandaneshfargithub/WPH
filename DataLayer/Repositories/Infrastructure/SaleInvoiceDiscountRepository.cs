using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DataLayer.Repositories.Infrastructure
{
    public class SaleInvoiceDiscountRepository : Repository<SaleInvoiceDiscount>, ISaleInvoiceDiscountRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SaleInvoiceDiscountRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<SaleInvoiceDiscount> GetAllSaleInvoiceDiscounts(Guid SaleInvoiceId)
        {
            return _context.SaleInvoiceDiscounts.AsNoTracking()
            .Include(x => x.Currency)
            .Where(x => x.SaleInvoiceId == SaleInvoiceId)
            .Select(p => new SaleInvoiceDiscount
            {
                Guid = p.Guid,
                Amount = p.Amount,
                Description = p.Description,
                Currency = new BaseInfoGeneral
                {
                    Name = p.Currency.Name
                }
            });
        }

        public SaleInvoiceDiscount GetSaleInvoiceDiscountByCurrencyId(Guid saleInvoiceId, int currencyId)
        {
            return _context.SaleInvoiceDiscounts.AsNoTracking()
                .FirstOrDefault(a => a.SaleInvoiceId == saleInvoiceId && a.CurrencyId == currencyId);
                
        }

        //public List<SaleInvoiceDiscount> GetAllSaleInvoiceDiscountsForReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo)
        //{
        //    return Context.SaleInvoiceDiscounts.AsNoTracking()
        //        .Include(x => x.SaleInvoiceDiscountType)
        //        .Include(x => x.ClinicSection)
        //        .Where(x => x.SaleInvoiceDiscountDate <= dateTo && x.SaleInvoiceDiscountDate >= dateFrom && clinicSectionIds.Contains(x.ClinicSectionId ?? Guid.Empty))
        //        .Select(a=> new SaleInvoiceDiscount 
        //        {
        //            ClinicSection = new ClinicSection
        //            {
        //                Name = a.ClinicSection.Name
        //            },
        //            SaleInvoiceDiscountDate = a.SaleInvoiceDiscountDate,
        //            SaleInvoiceDiscountType = new BaseInfo
        //            {
        //                Name = a.SaleInvoiceDiscountType.Name
        //            },
        //            ClinicSectionId = a.ClinicSectionId,
        //            Price = a.Price
        //        }).OrderBy(x=>x.SaleInvoiceDiscountDate).ThenBy(a=>a.ClinicSectionId).ThenBy(a => a.SaleInvoiceDiscountType.Name).ToList();
        //}

        //public SaleInvoiceDiscount GetWithType(Guid itemId)
        //{
        //    return Context.SaleInvoiceDiscounts.AsNoTracking()
        //        .Include(p => p.SaleInvoiceDiscountType)
        //        .SingleOrDefault(p => p.Guid == itemId);
        //}
    }
}
