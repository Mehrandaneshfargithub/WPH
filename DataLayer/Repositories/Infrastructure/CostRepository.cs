using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class CostRepository : Repository<Cost>, ICostRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public CostRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Cost> GetAllCosts(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid? costType)
        {
            if (costType == null || costType == Guid.Empty)
            {
                return Context.Costs.AsNoTracking()
                    .Include(x => x.CostType)
                    .Include(x => x.Currency)
                    .Include(x => x.PurchaseInvoice)
                    .Include(x => x.SaleInvoice)
                    .Where(x => x.CostDate <= dateTo && x.CostDate >= dateFrom && x.ClinicSectionId == clinicSectionId).OrderByDescending(x => x.Id);
            }
            else
            {
                return Context.Costs.AsNoTracking()
                    .Include(x => x.CostType)
                    .Include(x => x.Currency)
                    .Include(x => x.PurchaseInvoice)
                    .Include(x => x.SaleInvoice)
                    .Where(x => x.CostDate <= dateTo && x.CostDate >= dateFrom && x.ClinicSectionId == clinicSectionId && x.CostType.Guid == costType).OrderByDescending(x => x.Id);

            }

        }

        public IEnumerable<Cost> GetAllPurchasInvoiceCosts(Guid purchaseInvoiceId)
        {
            return _context.Costs.AsNoTracking()
            .Include(x => x.Currency)
            .Where(x => x.PurchaseInvoiceId == purchaseInvoiceId)
            .Select(p => new Cost
            {
                Guid = p.Guid,
                Explanation = p.Explanation,
                Price = p.Price,
                Currency = new BaseInfoGeneral
                {
                    Name = p.Currency.Name
                }
            });
        }

        public IEnumerable<Cost> GetAllSaleInvoiceCosts(Guid saleInvoiceId)
        {
            return _context.Costs.AsNoTracking()
            .Include(x => x.Currency)
            .Include(x => x.CostType)
            .Where(x => x.SaleInvoiceId == saleInvoiceId)
            .Select(p => new Cost
            {
                Guid = p.Guid,
                Explanation = p.Explanation,
                Price = p.Price,
                Currency = new BaseInfoGeneral
                {
                    Name = p.Currency.Name
                },
                CostType = new BaseInfo
                {
                    Name = p.CostType.Name
                }
            });
        }

        public List<Cost> GetAllCostsForReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo)
        {
            return Context.Costs.AsNoTracking()
                .Include(x => x.CostType)
                .Include(x => x.ClinicSection)
                .Where(x => x.CostDate <= dateTo && x.CostDate >= dateFrom && clinicSectionIds.Contains(x.ClinicSectionId ?? Guid.Empty))
                .Select(a => new Cost
                {
                    ClinicSection = new ClinicSection
                    {
                        Name = a.ClinicSection.Name
                    },
                    CostDate = a.CostDate,
                    CostType = new BaseInfo
                    {
                        Name = a.CostType.Name
                    },
                    ClinicSectionId = a.ClinicSectionId,
                    Price = a.Price
                }).OrderBy(x => x.CostDate).ThenBy(a => a.ClinicSectionId).ThenBy(a => a.CostType.Name).ToList();
        }

        public Cost GetWithType(Guid itemId)
        {
            return Context.Costs.AsNoTracking()
                .Include(p => p.CostType)
                .SingleOrDefault(p => p.Guid == itemId);
        }

    }
}
