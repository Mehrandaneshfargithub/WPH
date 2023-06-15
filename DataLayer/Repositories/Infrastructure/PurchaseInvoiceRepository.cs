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
    public class PurchaseInvoiceRepository : Repository<PurchaseInvoice>, IPurchaseInvoiceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PurchaseInvoiceRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PurchaseInvoice> GetAllPurchaseInvoiceByInvoiceNum(Guid clinicSectionId, string invoiceNum)
        {
            IQueryable<PurchaseInvoice> result = _context.PurchaseInvoices.AsNoTracking()
                .Include(p => p.Supplier.User)
                .Include(p => p.PurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.Costs).ThenInclude(p => p.Currency)
                .Where(p => p.ClinicSectionId == clinicSectionId && p.InvoiceNum == invoiceNum);

            return result.Select(p => new PurchaseInvoice
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                MainInvoiceNum = p.MainInvoiceNum,
                TotalPrice = p.TotalPrice,
                Status = p.PurchaseInvoicePays.Any(p => p.FullPay != null && p.FullPay.Value),
                Supplier = new Supplier
                {
                    User = new User
                    {
                        Name = p.Supplier.User.Name
                    }
                },
                PurchaseInvoiceDiscounts = (ICollection<PurchaseInvoiceDiscount>)p.PurchaseInvoiceDiscounts.Select(x => new PurchaseInvoiceDiscount
                {
                    Amount = x.Amount,
                    CurrencyName = x.Currency.Name
                }),
                Costs = (ICollection<Cost>)p.Costs.Select(x => new Cost
                {
                    Price = x.Price,
                    CurrencyName = x.Currency.Name
                })
            });
        }

        public IEnumerable<PurchaseInvoice> GetAllPurchaseInvoice(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<PurchaseInvoice, bool>> predicate = null)
        {
            IQueryable<PurchaseInvoice> result = _context.PurchaseInvoices.AsNoTracking()
                .Include(p => p.Supplier.User)
                .Include(p => p.PurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.Costs).ThenInclude(p => p.Currency)
                .Where(p => p.ClinicSectionId == clinicSectionId && p.InvoiceDate >= dateFrom && p.InvoiceDate <= dateTo);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(p => new PurchaseInvoice
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                MainInvoiceNum = p.MainInvoiceNum,
                TotalPrice = p.TotalPrice,
                Status = p.PurchaseInvoicePays.Any(p => p.FullPay != null && p.FullPay.Value),
                Supplier = new Supplier
                {
                    User = new User
                    {
                        Name = p.Supplier.User.Name
                    }
                },
                PurchaseInvoiceDiscounts = (ICollection<PurchaseInvoiceDiscount>)p.PurchaseInvoiceDiscounts.Select(x => new PurchaseInvoiceDiscount
                {
                    Amount = x.Amount,
                    CurrencyName = x.Currency.Name
                }),
                Costs = (ICollection<Cost>)p.Costs.Select(x => new Cost
                {
                    Price = x.Price,
                    CurrencyName = x.Currency.Name
                })
            });
        }

        public PurchaseInvoice GetPurchaseInvoice(Guid purchaseInvoiceId)
        {
            return _context.PurchaseInvoices.AsNoTracking()
                .Include(p => p.Type)
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.PurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.Costs).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == purchaseInvoiceId)
                .Select(p => new PurchaseInvoice
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    SupplierId = p.SupplierId,
                    Description = p.Description,
                    MainInvoiceNum = p.MainInvoiceNum,
                    CreatedUserId = p.CreatedUserId,
                    CreateDate = p.CreateDate,
                    ModifiedUserId = p.ModifiedUserId,
                    ModifiedDate = p.ModifiedDate,
                    ClinicSectionId = p.ClinicSectionId,
                    TotalPrice = p.TotalPrice,
                    OldFactor = p.OldFactor,
                    TypeId = p.TypeId,
                    Type = new BaseInfoGeneral
                    {
                        Name = p.Type.Name
                    },
                    PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Select(x => new PurchaseInvoiceDetail
                    {
                        Guid = x.Guid,
                        Num = x.Num,
                        PurchasePrice = x.PurchasePrice,
                        Discount = x.Discount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                    }),
                    PurchaseInvoiceDiscounts = (ICollection<PurchaseInvoiceDiscount>)p.PurchaseInvoiceDiscounts.Select(x => new PurchaseInvoiceDiscount
                    {
                        Guid = x.Guid,
                        Amount = x.Amount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                    }),
                    Costs = (ICollection<Cost>)p.Costs.Select(x => new Cost
                    {
                        Price = x.Price,
                        CurrencyName = x.Currency.Name,
                    })
                }).SingleOrDefault(p => p.Guid == purchaseInvoiceId);
        }

        public string GetLatestPurchaseInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                return _context.FN_LatestPurchaseInvoiceNum(clinicSectionId).FirstOrDefault().CODE;
            }
            catch (Exception e) { throw e; }

        }

        public PurchaseInvoice GetForUpdateTotalPrice(Guid purchaseInvoiceId)
        {
            return _context.PurchaseInvoices.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.PurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.Costs).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == purchaseInvoiceId)
                .Select(p => new PurchaseInvoice
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    SupplierId = p.SupplierId,
                    Description = p.Description,
                    MainInvoiceNum = p.MainInvoiceNum,
                    CreatedUserId = p.CreatedUserId,
                    CreateDate = p.CreateDate,
                    ModifiedUserId = p.ModifiedUserId,
                    ModifiedDate = p.ModifiedDate,
                    ClinicSectionId = p.ClinicSectionId,
                    TotalPrice = p.TotalPrice,
                    OldFactor = p.OldFactor,
                    PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Select(x => new PurchaseInvoiceDetail
                    {
                        Guid = x.Guid,
                        Num = x.Num,
                        PurchasePrice = x.PurchasePrice,
                        Discount = x.Discount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                    }),
                    PurchaseInvoiceDiscounts = (ICollection<PurchaseInvoiceDiscount>)p.PurchaseInvoiceDiscounts.Select(x => new PurchaseInvoiceDiscount
                    {
                        Guid = x.Guid,
                        Amount = x.Amount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                    }),
                    Costs = (ICollection<Cost>)p.Costs.Select(x => new Cost
                    {
                        Guid = x.Guid,
                        Price = x.Price,
                        CurrencyName = x.Currency.Name,
                    })
                }).SingleOrDefault();
        }

        public IEnumerable<PurchaseInvoice> GetAllPurchaseInvoiceFroReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo, bool detail)
        {
            IQueryable<PurchaseInvoice> result = _context.PurchaseInvoices.AsNoTracking()
                 .Include(p => p.Supplier).ThenInclude(p => p.User)
                 //.Include(p => p.Currency)
                 ;

            if (detail)
                result.Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Product);

            return result.Where(p => clinicSectionIds.Contains(p.ClinicSectionId ?? Guid.Empty) && p.InvoiceDate >= dateFrom && p.InvoiceDate <= dateTo)
                  .Select(a => new PurchaseInvoice
                  {
                      InvoiceNum = a.InvoiceNum,
                      InvoiceDate = a.InvoiceDate,
                      Supplier = new Supplier
                      {
                          User = new User
                          {
                              Name = a.Supplier.User.Name
                          }
                      },
                      PurchaseInvoiceDetails = !detail ? null : a.PurchaseInvoiceDetails.Select(c => new PurchaseInvoiceDetail
                      {
                          PurchasePrice = c.PurchasePrice,
                          Num = c.Num,
                          Discount = c.Discount,
                          Product = new Product
                          {
                              Name = c.Product.Name
                          }
                      }).ToList()

                  });
        }

        public bool CheckRepeatedInvoiceNum(Guid supplierId, string mainInvoiceNum, Expression<Func<PurchaseInvoice, bool>> predicate = null)
        {
            IQueryable<PurchaseInvoice> result = _context.PurchaseInvoices.AsNoTracking()
                .Where(p => p.SupplierId == supplierId && p.MainInvoiceNum == mainInvoiceNum);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }

        public IEnumerable<PartialPayModel> GetNotPartialPayPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId)
        {
            string query = $@"SELECT pii.Guid,pii.InvoiceNum,pii.InvoiceDate ,pii.Description,pii.MainInvoiceNum,pii.TotalPrice,
                            (SELECT STRING_AGG(CAST(pp.PayId AS NVARCHAR(50)),' ') FROM dbo.PurchaseInvoicePay pp LEFT JOIN dbo.Pay p ON p.Guid = pp.PayId WHERE p.BaseCurrencyId={currencyId} AND pp.FullPay=0 AND pp.InvoiceId=pii.Guid) PayIds FROM dbo.PurchaseInvoice pii WHERE 
                            pii.SupplierId='{supplierId}' AND pii.TotalPrice LIKE N'%'+(SELECT Name FROM dbo.BaseInfoGeneral WHERE Id={currencyId})+'%' AND 	
                            pii.Guid NOT IN (SELECT pp.InvoiceId FROM dbo.PurchaseInvoicePay pp LEFT JOIN dbo.Pay p ON p.Guid = pp.PayId WHERE p.BaseCurrencyId={currencyId} AND  (pp.FullPay=1 OR (pp.FullPay=0 AND p.Guid='{payId ?? Guid.Empty}')) AND pp.InvoiceId=pii.Guid) 
                            ";

            try
            {
                return Context.Set<PartialPayModel>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<PartialPayModel> GetPartialPayPurchaseInvoice(Guid? payId, int? currencyId)
        {
            string query = $@"SELECT pii.Guid,pii.InvoiceNum,pii.InvoiceDate ,pii.Description,pii.MainInvoiceNum,pii.TotalPrice,
                            (SELECT STRING_AGG(CAST(pp.PayId AS NVARCHAR(50)),' ') FROM dbo.PurchaseInvoicePay pp LEFT JOIN dbo.Pay p ON p.Guid = pp.PayId WHERE p.BaseCurrencyId={currencyId} AND pp.FullPay=0 AND pp.InvoiceId=pii.Guid) PayIds FROM dbo.PurchaseInvoice pii 
                            LEFT JOIN dbo.PurchaseInvoicePay pip ON pip.InvoiceId = pii.Guid
                            WHERE pip.PayId='{payId}'
                            ";

            try
            {
                return Context.Set<PartialPayModel>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<PurchaseInvoice> GetNotPayPurchaseInvoice(Guid? supplierId)
        {
            string query = $@"SELECT pii.Guid,pii.InvoiceNum,pii.InvoiceDate,pii.Description,pii.MainInvoiceNum,pii.TotalPrice,pii.Id,NULL SupplierId,NULL CreatedUserId,NULL CreateDate,NULL ModifiedUserId,NULL ModifiedDate,NULL ClinicSectionId,NULL OldFactor,NULL TypeId FROM dbo.PurchaseInvoice pii WHERE 
                             pii.SupplierId = '{supplierId}' AND  ISNULL(pii.TotalPrice,'')<>''  AND  
                             NOT EXISTS(SELECT pp.Guid FROM dbo.PurchaseInvoicePay pp LEFT JOIN dbo.Pay p ON p.Guid = pp.PayId WHERE pp.InvoiceId = pii.Guid)
                            ";

            try
            {
                return Context.Set<PurchaseInvoice>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<PurchaseInvoice> GetPayPurchaseInvoice(Guid? payId)
        {
            return _context.PurchaseInvoicePays.AsNoTracking()
                .Include(p => p.Invoice)
                .Where(p => p.PayId == payId)
                .Select(p => new PurchaseInvoice
                {
                    Guid = p.Invoice.Guid,
                    InvoiceNum = p.Invoice.InvoiceNum,
                    InvoiceDate = p.Invoice.InvoiceDate,
                    Description = p.Invoice.Description,
                    MainInvoiceNum = p.Invoice.MainInvoiceNum,
                    TotalPrice = p.Invoice.TotalPrice,
                });
        }

        public IEnumerable<PurchaseInvoice> GetForPay(IEnumerable<Guid> purchaseIds)
        {
            return _context.PurchaseInvoices.AsNoTracking()
                .Where(p => purchaseIds.Contains(p.Guid));
        }

        public PurchaseInvoice GetPurchaseForReport(Guid purchaseInvoiceId)
        {
            return _context.PurchaseInvoices.AsNoTracking()
                .Include(p => p.Supplier.User)
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Product.Producer)
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Product.ProductType)
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.PurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == purchaseInvoiceId)
                .Select(p => new PurchaseInvoice
                {
                    Supplier = new Supplier
                    {
                        User = new User
                        {
                            Name = p.Supplier.User.Name
                        }
                    },
                    InvoiceNum = p.InvoiceNum,
                    MainInvoiceNum = p.MainInvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    Description = p.Description,
                    TotalPrice = p.TotalPrice,
                    PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Select(x => new PurchaseInvoiceDetail
                    {
                        Product = new Product
                        {
                            Name = x.Product.Name,
                            Producer = new BaseInfo
                            {
                                Name = x.Product.Producer.Name
                            },
                            ProductType = new BaseInfo
                            {
                                Name = x.Product.ProductType.Name
                            }
                        },
                        ExpireDate = x.ExpireDate,
                        Num = x.Num,
                        FreeNum = x.FreeNum,
                        PurchasePrice = x.PurchasePrice,
                        Discount = x.Discount,
                        CurrencyName = x.Currency.Name
                    }),
                    PurchaseInvoiceDiscounts = (ICollection<PurchaseInvoiceDiscount>)p.PurchaseInvoiceDiscounts.Select(x => new PurchaseInvoiceDiscount
                    {
                        CurrencyName = x.Currency.Name,
                        Amount = x.Amount
                    })
                })
                .SingleOrDefault();
        }
    }
}
