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
    public class ReturnPurchaseInvoiceRepository : Repository<ReturnPurchaseInvoice>, IReturnPurchaseInvoiceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReturnPurchaseInvoiceRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReturnPurchaseInvoice> GetAllReturnPurchaseInvoice(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<ReturnPurchaseInvoice, bool>> predicate = null)
        {
            IQueryable<ReturnPurchaseInvoice> result = _context.ReturnPurchaseInvoices.AsNoTracking()
                .Include(p => p.Supplier.User)
                .Include(p => p.ReturnPurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.ClinicSectionId == clinicSectionId && p.InvoiceDate >= dateFrom && p.InvoiceDate <= dateTo);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(p => new ReturnPurchaseInvoice
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                TotalPrice = p.TotalPrice,
                ReturnPurchaseInvoiceDiscounts = (ICollection<ReturnPurchaseInvoiceDiscount>)p.ReturnPurchaseInvoiceDiscounts.Select(x => new ReturnPurchaseInvoiceDiscount
                {
                    Amount = x.Amount,
                    CurrencyName = x.Currency.Name
                }),
                Supplier = new Supplier
                {
                    User = new User
                    {
                        Name = p.Supplier.User.Name
                    }
                }
            });
        }

        public IEnumerable<PartialPayModel> GetNotPartialPayReturnPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId)
        {
            string query = $@"SELECT rpi.Guid,rpi.InvoiceNum,rpi.InvoiceDate ,rpi.Description,NULL MainInvoiceNum,rpi.TotalPrice,
                            (SELECT STRING_AGG(CAST(pp.PayId AS NVARCHAR(50)),' ') FROM dbo.PurchaseInvoicePay pp LEFT JOIN dbo.Pay p ON p.Guid = pp.PayId WHERE p.BaseCurrencyId={currencyId} AND pp.FullPay=0 AND pp.InvoiceId=rpi.Guid) PayIds FROM dbo.ReturnPurchaseInvoice rpi WHERE 
                            rpi.SupplierId='{supplierId}' AND rpi.TotalPrice LIKE N'%'+(SELECT Name FROM dbo.BaseInfoGeneral WHERE Id={currencyId})+'%' AND 	
                            rpi.Guid NOT IN (SELECT pp.InvoiceId FROM dbo.ReturnPurchaseInvoicePay pp LEFT JOIN dbo.Pay p ON p.Guid = pp.PayId WHERE p.BaseCurrencyId={currencyId} AND  (pp.FullPay=1 OR (pp.FullPay=0 AND p.Guid='{payId ?? Guid.Empty}')) AND pp.InvoiceId=rpi.Guid) 
                            ";

            try
            {
                return Context.Set<PartialPayModel>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<PartialPayModel> GetPartialPayReturnPurchaseInvoice(Guid? payId)
        {
            string query = $@"SELECT pii.Guid,pii.InvoiceNum,pii.InvoiceDate ,pii.Description,pii.MainInvoiceNum,pii.TotalPrice,
                            (SELECT STRING_AGG(CAST(pp.PayId AS NVARCHAR(50)),' ') FROM dbo.PurchaseInvoicePay pp LEFT JOIN dbo.Pay p ON p.Guid = pp.PayId WHERE p.BaseCurrencyId= AND pp.FullPay=0 AND pp.InvoiceId=pii.Guid) PayIds FROM dbo.PurchaseInvoice pii 
                            LEFT JOIN dbo.PurchaseInvoicePay pip ON pip.InvoiceId = pii.Guid
                            WHERE pip.PayId='{payId}'
                            ";

            try
            {
                return Context.Set<PartialPayModel>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<ReturnPurchaseInvoice> GetNotPayReturnPurchaseInvoice(Guid? supplierId)
        {
            string query = $@"SELECT rpi.Guid,rpi.InvoiceNum,rpi.InvoiceDate,rpi.Description,rpi.TotalPrice,rpi.Id,NULL SupplierId,NULL CreatedUserId,NULL CreateDate,NULL ModifiedUserId,NULL ModifiedDate,NULL ClinicSectionId,NULL OldFactor FROM dbo.ReturnPurchaseInvoice rpi WHERE 
                              rpi.SupplierId = '{supplierId}' AND  ISNULL(rpi.TotalPrice,'')<>''  AND  
                              NOT EXISTS(SELECT pp.Guid FROM dbo.ReturnPurchaseInvoicePay pp LEFT JOIN dbo.Pay p ON p.Guid = pp.PayId WHERE pp.InvoiceId = rpi.Guid)
                            ";

            try
            {
                return Context.Set<ReturnPurchaseInvoice>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<ReturnPurchaseInvoice> GetForPay(IEnumerable<Guid> returnIds)
        {
            return _context.ReturnPurchaseInvoices.AsNoTracking()
                .Where(p => returnIds.Contains(p.Guid));
        }

        public IEnumerable<ReturnPurchaseInvoice> GetPayReturnPurchaseInvoice(Guid? payId)
        {
            return _context.ReturnPurchaseInvoicePays.AsNoTracking()
                .Include(p => p.Invoice)
                .Where(p => p.PayId == payId)
                .Select(p => new ReturnPurchaseInvoice
                {
                    Guid = p.Invoice.Guid,
                    InvoiceNum = p.Invoice.InvoiceNum,
                    InvoiceDate = p.Invoice.InvoiceDate,
                    Description = p.Invoice.Description,
                    TotalPrice = p.Invoice.TotalPrice,
                });
        }

        public ReturnPurchaseInvoice GetReturnPurchaseInvoice(Guid returnPurchaseInvoiceId)
        {
            return _context.ReturnPurchaseInvoices.AsNoTracking()
                .SingleOrDefault(p => p.Guid == returnPurchaseInvoiceId);
        }

        public string GetLatestReturnPurchaseInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                return _context.FN_LatestReturnPurchaseInvoiceNum(clinicSectionId).FirstOrDefault().CODE;
            }
            catch (Exception e) { throw e; }

        }

        public ReturnPurchaseInvoice GetForUpdateTotalPrice(Guid returnPurchaseInvoiceId)
        {
            return _context.ReturnPurchaseInvoices.AsNoTracking()
                .Include(p => p.ReturnPurchaseInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.ReturnPurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == returnPurchaseInvoiceId)
                .Select(p => new ReturnPurchaseInvoice
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    SupplierId = p.SupplierId,
                    Description = p.Description,
                    CreatedUserId = p.CreatedUserId,
                    CreateDate = p.CreateDate,
                    ModifiedUserId = p.ModifiedUserId,
                    ModifiedDate = p.ModifiedDate,
                    ClinicSectionId = p.ClinicSectionId,
                    TotalPrice = p.TotalPrice,
                    OldFactor = p.OldFactor,
                    ReturnPurchaseInvoiceDetails = (ICollection<ReturnPurchaseInvoiceDetail>)p.ReturnPurchaseInvoiceDetails.Select(x => new ReturnPurchaseInvoiceDetail
                    {
                        Guid = x.Guid,
                        Num = x.Num,
                        Price = x.Price,
                        Discount = x.Discount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,

                    }),
                    ReturnPurchaseInvoiceDiscounts = (ICollection<ReturnPurchaseInvoiceDiscount>)p.ReturnPurchaseInvoiceDiscounts.Select(x => new ReturnPurchaseInvoiceDiscount
                    {
                        Guid = x.Guid,
                        Amount = x.Amount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,

                    })
                }).SingleOrDefault();
        }
    }
}
