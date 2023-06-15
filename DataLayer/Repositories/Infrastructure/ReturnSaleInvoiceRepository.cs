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
    public class ReturnSaleInvoiceRepository : Repository<ReturnSaleInvoice>, IReturnSaleInvoiceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReturnSaleInvoiceRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReturnSaleInvoice> GetAllReturnSaleInvoice(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<ReturnSaleInvoice, bool>> predicate = null)
        {
            IQueryable<ReturnSaleInvoice> result = _context.ReturnSaleInvoices.AsNoTracking()
                .Include(p => p.Customer.User)
                .Include(p => p.ReturnSaleInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.ClinicSectionId == clinicSectionId && p.InvoiceDate >= dateFrom && p.InvoiceDate <= dateTo);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(p => new ReturnSaleInvoice
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                TotalPrice = p.TotalPrice,
                ReturnSaleInvoiceDiscounts = (ICollection<ReturnSaleInvoiceDiscount>)p.ReturnSaleInvoiceDiscounts.Select(x => new ReturnSaleInvoiceDiscount
                {
                    Amount = x.Amount,
                    CurrencyName = x.Currency.Name
                }),
                Customer = new Customer
                {
                    User = new User
                    {
                        Name = p.Customer.User.Name
                    }
                }
            });
        }

        //public IEnumerable<PartialReceiveModel> GetNotPartialReceiveReturnSaleInvoice(Guid? customerId, int? currencyId, Guid? receiveId)
        //{
        //    string query = $@"SELECT rpi.Guid,rpi.InvoiceNum,rpi.InvoiceDate ,rpi.Description,NULL MainInvoiceNum,rpi.TotalPrice,
        //                    (SELECT STRING_AGG(CAST(pp.ReceiveId AS NVARCHAR(50)),' ') FROM dbo.SaleInvoiceReceive pp LEFT JOIN dbo.Receive p ON p.Guid = pp.ReceiveId WHERE p.BaseCurrencyId={currencyId} AND pp.FullReceive=0 AND pp.InvoiceId=rpi.Guid) ReceiveIds FROM dbo.ReturnSaleInvoice rpi WHERE 
        //                    rpi.CustomerId='{customerId}' AND rpi.TotalPrice LIKE N'%'+(SELECT Name FROM dbo.BaseInfoGeneral WHERE Id={currencyId})+'%' AND 	
        //                    rpi.Guid NOT IN (SELECT pp.InvoiceId FROM dbo.ReturnSaleInvoiceReceive pp LEFT JOIN dbo.Receive p ON p.Guid = pp.ReceiveId WHERE p.BaseCurrencyId={currencyId} AND  (pp.FullReceive=1 OR (pp.FullReceive=0 AND p.Guid='{receiveId ?? Guid.Empty}')) AND pp.InvoiceId=rpi.Guid) 
        //                    ";

        //    try
        //    {
        //        return Context.Set<PartialReceiveModel>().FromSqlRaw(query);
        //    }
        //    catch (Exception) { return null; }
        //}

        //public IEnumerable<PartialReceiveModel> GetPartialReceiveReturnSaleInvoice(Guid? receiveId)
        //{
        //    string query = $@"SELECT pii.Guid,pii.InvoiceNum,pii.InvoiceDate ,pii.Description,pii.MainInvoiceNum,pii.TotalPrice,
        //                    (SELECT STRING_AGG(CAST(pp.ReceiveId AS NVARCHAR(50)),' ') FROM dbo.SaleInvoiceReceive pp LEFT JOIN dbo.Receive p ON p.Guid = pp.ReceiveId WHERE p.BaseCurrencyId= AND pp.FullReceive=0 AND pp.InvoiceId=pii.Guid) ReceiveIds FROM dbo.SaleInvoice pii 
        //                    LEFT JOIN dbo.SaleInvoiceReceive pip ON pip.InvoiceId = pii.Guid
        //                    WHERE pip.ReceiveId='{receiveId}'
        //                    ";

        //    try
        //    {
        //        return Context.Set<PartialReceiveModel>().FromSqlRaw(query);
        //    }
        //    catch (Exception) { return null; }
        //}

        public IEnumerable<ReturnSaleInvoice> GetNotReceiveReturnSaleInvoice(Guid? customerId)
        {
            string query = $@"SELECT rpi.Guid,rpi.InvoiceNum,rpi.InvoiceDate,rpi.Description,rpi.TotalPrice,rpi.Id,NULL CustomerId,NULL CreatedUserId,NULL CreateDate,NULL ModifiedUserId,NULL ModifiedDate,NULL ClinicSectionId,NULL OldFactor FROM dbo.ReturnSaleInvoice rpi WHERE 
                              rpi.CustomerId = '{customerId}' AND  ISNULL(rpi.TotalPrice,'')<>''  AND  
                              NOT EXISTS(SELECT pp.Guid FROM dbo.ReturnSaleInvoiceReceive pp LEFT JOIN dbo.Receive p ON p.Guid = pp.ReceiveId WHERE pp.InvoiceId = rpi.Guid)
                            ";

            try
            {
                return Context.Set<ReturnSaleInvoice>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<ReturnSaleInvoice> GetForReceive(IEnumerable<Guid> returnIds)
        {
            return _context.ReturnSaleInvoices.AsNoTracking()
                .Where(p => returnIds.Contains(p.Guid));
        }

        public IEnumerable<ReturnSaleInvoice> GetReceiveReturnSaleInvoice(Guid? receiveId)
        {
            return _context.ReturnSaleInvoiceReceives.AsNoTracking()
                .Include(p => p.Invoice)
                .Where(p => p.ReceiveId == receiveId)
                .Select(p => new ReturnSaleInvoice
                {
                    Guid = p.Invoice.Guid,
                    InvoiceNum = p.Invoice.InvoiceNum,
                    InvoiceDate = p.Invoice.InvoiceDate,
                    Description = p.Invoice.Description,
                    TotalPrice = p.Invoice.TotalPrice,
                });
        }

        public ReturnSaleInvoice GetWithType(Guid returnSaleInvoiceId)
        {
            return _context.ReturnSaleInvoices.AsNoTracking()
                .SingleOrDefault(p => p.Guid == returnSaleInvoiceId);
        }

        public string GetLatestReturnSaleInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                return _context.FN_LatestReturnSaleInvoiceNum(clinicSectionId).FirstOrDefault().CODE;
            }
            catch (Exception e) { throw e; }

        }

        public ReturnSaleInvoice GetForUpdateTotalPrice(Guid returnSaleInvoiceId)
        {
            return _context.ReturnSaleInvoices.AsNoTracking()
                .Include(p => p.ReturnSaleInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.ReturnSaleInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == returnSaleInvoiceId)
                .Select(p => new ReturnSaleInvoice
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    CustomerId = p.CustomerId,
                    Description = p.Description,
                    CreatedUserId = p.CreatedUserId,
                    CreateDate = p.CreateDate,
                    ModifiedUserId = p.ModifiedUserId,
                    ModifiedDate = p.ModifiedDate,
                    ClinicSectionId = p.ClinicSectionId,
                    TotalPrice = p.TotalPrice,
                    OldFactor = p.OldFactor,
                    ReturnSaleInvoiceDetails = (ICollection<ReturnSaleInvoiceDetail>)p.ReturnSaleInvoiceDetails.Select(x => new ReturnSaleInvoiceDetail
                    {
                        Guid = x.Guid,
                        Num = x.Num,
                        Price = x.Price,
                        Discount = x.Discount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                    }),
                    ReturnSaleInvoiceDiscounts = (ICollection<ReturnSaleInvoiceDiscount>)p.ReturnSaleInvoiceDiscounts.Select(x => new ReturnSaleInvoiceDiscount
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
