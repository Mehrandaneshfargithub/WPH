using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReturnPurchaseInvoiceDiscountRepository : Repository<ReturnPurchaseInvoiceDiscount>, IReturnPurchaseInvoiceDiscountRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReturnPurchaseInvoiceDiscountRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReturnPurchaseInvoiceDiscount> GetAllReturnPurchaseInvoiceDiscounts(Guid returnPurchaseInvoiceId)
        {
            return _context.ReturnPurchaseInvoiceDiscounts.AsNoTracking()
            .Include(x => x.Currency)
            .Where(x => x.ReturnPurchaseInvoiceId == returnPurchaseInvoiceId)
            .Select(p => new ReturnPurchaseInvoiceDiscount
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

        //public List<ReturnPurchaseInvoiceDiscount> GetAllReturnPurchaseInvoiceDiscountsForReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo)
        //{
        //    return Context.ReturnPurchaseInvoiceDiscounts.AsNoTracking()
        //        .Include(x => x.ReturnPurchaseInvoiceDiscountType)
        //        .Include(x => x.ClinicSection)
        //        .Where(x => x.ReturnPurchaseInvoiceDiscountDate <= dateTo && x.ReturnPurchaseInvoiceDiscountDate >= dateFrom && clinicSectionIds.Contains(x.ClinicSectionId ?? Guid.Empty))
        //        .Select(a => new ReturnPurchaseInvoiceDiscount
        //        {
        //            ClinicSection = new ClinicSection
        //            {
        //                Name = a.ClinicSection.Name
        //            },
        //            ReturnPurchaseInvoiceDiscountDate = a.ReturnPurchaseInvoiceDiscountDate,
        //            ReturnPurchaseInvoiceDiscountType = new BaseInfo
        //            {
        //                Name = a.ReturnPurchaseInvoiceDiscountType.Name
        //            },
        //            ClinicSectionId = a.ClinicSectionId,
        //            Price = a.Price
        //        }).OrderBy(x => x.ReturnPurchaseInvoiceDiscountDate).ThenBy(a => a.ClinicSectionId).ThenBy(a => a.ReturnPurchaseInvoiceDiscountType.Name).ToList();
        //}

        //public ReturnPurchaseInvoiceDiscount GetWithType(Guid itemId)
        //{
        //    return Context.ReturnPurchaseInvoiceDiscounts.AsNoTracking()
        //        .Include(p => p.ReturnPurchaseInvoiceDiscountType)
        //        .SingleOrDefault(p => p.Guid == itemId);
        //}
    }
}
