using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReturnSaleInvoiceDiscountRepository : Repository<ReturnSaleInvoiceDiscount>, IReturnSaleInvoiceDiscountRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReturnSaleInvoiceDiscountRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReturnSaleInvoiceDiscount> GetAllReturnSaleInvoiceDiscounts(Guid returnSaleInvoiceId)
        {
            return _context.ReturnSaleInvoiceDiscounts.AsNoTracking()
            .Include(x => x.Currency)
            .Where(x => x.ReturnSaleInvoiceId == returnSaleInvoiceId)
            .Select(p => new ReturnSaleInvoiceDiscount
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

        //public List<ReturnSaleInvoiceDiscount> GetAllReturnSaleInvoiceDiscountsForReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo)
        //{
        //    return Context.ReturnSaleInvoiceDiscounts.AsNoTracking()
        //        .Include(x => x.ReturnSaleInvoiceDiscountType)
        //        .Include(x => x.ClinicSection)
        //        .Where(x => x.ReturnSaleInvoiceDiscountDate <= dateTo && x.ReturnSaleInvoiceDiscountDate >= dateFrom && clinicSectionIds.Contains(x.ClinicSectionId ?? Guid.Empty))
        //        .Select(a => new ReturnSaleInvoiceDiscount
        //        {
        //            ClinicSection = new ClinicSection
        //            {
        //                Name = a.ClinicSection.Name
        //            },
        //            ReturnSaleInvoiceDiscountDate = a.ReturnSaleInvoiceDiscountDate,
        //            ReturnSaleInvoiceDiscountType = new BaseInfo
        //            {
        //                Name = a.ReturnSaleInvoiceDiscountType.Name
        //            },
        //            ClinicSectionId = a.ClinicSectionId,
        //            Price = a.Price
        //        }).OrderBy(x => x.ReturnSaleInvoiceDiscountDate).ThenBy(a => a.ClinicSectionId).ThenBy(a => a.ReturnSaleInvoiceDiscountType.Name).ToList();
        //}

        //public ReturnSaleInvoiceDiscount GetWithType(Guid itemId)
        //{
        //    return Context.ReturnSaleInvoiceDiscounts.AsNoTracking()
        //        .Include(p => p.ReturnSaleInvoiceDiscountType)
        //        .SingleOrDefault(p => p.Guid == itemId);
        //}
    }
}
