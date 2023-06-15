using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class PurchaseInvoiceDiscountRepository : Repository<PurchaseInvoiceDiscount>, IPurchaseInvoiceDiscountRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PurchaseInvoiceDiscountRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PurchaseInvoiceDiscount> GetAllPurchaseInvoiceDiscounts(Guid purchaseInvoiceId)
        {
            return _context.PurchaseInvoiceDiscounts.AsNoTracking()
            .Include(x => x.Currency)
            .Where(x => x.PurchaseInvoiceId == purchaseInvoiceId)
            .Select(p => new PurchaseInvoiceDiscount
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

        //public List<PurchaseInvoiceDiscount> GetAllPurchaseInvoiceDiscountsForReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo)
        //{
        //    return Context.PurchaseInvoiceDiscounts.AsNoTracking()
        //        .Include(x => x.PurchaseInvoiceDiscountType)
        //        .Include(x => x.ClinicSection)
        //        .Where(x => x.PurchaseInvoiceDiscountDate <= dateTo && x.PurchaseInvoiceDiscountDate >= dateFrom && clinicSectionIds.Contains(x.ClinicSectionId ?? Guid.Empty))
        //        .Select(a=> new PurchaseInvoiceDiscount 
        //        {
        //            ClinicSection = new ClinicSection
        //            {
        //                Name = a.ClinicSection.Name
        //            },
        //            PurchaseInvoiceDiscountDate = a.PurchaseInvoiceDiscountDate,
        //            PurchaseInvoiceDiscountType = new BaseInfo
        //            {
        //                Name = a.PurchaseInvoiceDiscountType.Name
        //            },
        //            ClinicSectionId = a.ClinicSectionId,
        //            Price = a.Price
        //        }).OrderBy(x=>x.PurchaseInvoiceDiscountDate).ThenBy(a=>a.ClinicSectionId).ThenBy(a => a.PurchaseInvoiceDiscountType.Name).ToList();
        //}

        //public PurchaseInvoiceDiscount GetWithType(Guid itemId)
        //{
        //    return Context.PurchaseInvoiceDiscounts.AsNoTracking()
        //        .Include(p => p.PurchaseInvoiceDiscountType)
        //        .SingleOrDefault(p => p.Guid == itemId);
        //}
    }
}
