using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class PayRepository : Repository<Pay>, IPayRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PayRepository(WASContext context)
            : base(context)
        {
        }

        public string GetLatestPayInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                return _context.FN_LatestPayInvoiceNum(clinicSectionId).FirstOrDefault().CODE;
            }
            catch (Exception e) { throw e; }

        }

        public Pay GetPayWithPurchaseInvocie(Guid payId)
        {
            return Context.Pays.AsNoTracking()
                .Include(a => a.PayAmounts)
                .Include(a => a.PurchaseInvoicePays)
                .Include(a => a.ReturnPurchaseInvoicePays)
                .Where(p => p.Guid == payId)
                .SingleOrDefault();
        }

        public Pay GetWithSupplier(Guid payId)
        {
            return Context.Pays.AsNoTracking()
                .Include(p => p.Supplier).ThenInclude(p => p.User)
                .Include(p => p.PayAmounts).ThenInclude(p => p.BaseCurrency)
                .Include(p => p.PayAmounts).ThenInclude(p => p.Currency)
                .Select(p => new Pay
                {
                    Guid = p.Guid,
                    SupplierId = p.SupplierId,
                    Description = p.Description,
                    PayDate = p.PayDate,
                    InvoiceNum = p.InvoiceNum,
                    MainInvoiceNum = p.MainInvoiceNum,
                    Supplier = new Supplier
                    {
                        User = new User
                        {
                            Name = p.Supplier.User.Name
                        }
                    },
                    PayAmounts = p.PayAmounts
                })
                .SingleOrDefault(p => p.Guid == payId);
        }

        public Pay GetWithPayAmount(Guid payId)
        {
            return Context.Pays.AsNoTracking()
                .Include(p => p.PayAmounts)
                .SingleOrDefault(p => p.Guid == payId);
        }

        public string CheckPayStatus(Guid payId)
        {
            string query = $@"SELECT STRING_AGG(CONCAT(CAST(pp.FullPay AS NVARCHAR(50)),CAST(rpp.FullPay AS NVARCHAR(50)),' '),' ') Status  
							  FROM dbo.Pay
                              LEFT JOIN dbo.PurchaseInvoicePay pp ON pp.PayId = Pay.Guid
							  LEFT JOIN dbo.ReturnPurchaseInvoicePay rpp ON rpp.PayId = Pay.Guid
							  WHERE Pay.Guid='{payId}'
                            ";
            try
            {
                return Context.Set<CheckStatusModel>().FromSqlRaw(query).FirstOrDefault().Status;
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<Pay> GetPartialPayHistory(IEnumerable<string> ids)
        {
            return null;
            //return _context.Pays.AsNoTracking()
            //    .Include(p => p.Currency)
            //    .Where(p => ids.Contains(p.Guid.ToString()))
            //    .Select(p => new Pay
            //    {
            //        Amount = p.Amount,
            //        BaseAmount = p.BaseAmount,
            //        DestAmount = p.DestAmount,
            //        Description = p.Description,
            //        Discount = p.Discount,
            //        PayDate = p.PayDate,
            //        Currency = new BaseInfoGeneral
            //        {
            //            Name = p.Currency.Name
            //        }
            //    });
        }
    }
}
