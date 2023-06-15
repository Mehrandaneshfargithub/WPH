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
    public class ReceiveRepository : Repository<Receive>, IReceiveRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceiveRepository(WASContext context)
            : base(context)
        {
        }

        public string GetLatestReceiveInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                return _context.FN_LatestReceiveInvoiceNum(clinicSectionId).FirstOrDefault().CODE;
            }
            catch (Exception e) { throw e; }

        }

        public Receive GetReceiveWithSaleInvocie(Guid receiveId)
        {
            return Context.Receives.AsNoTracking()
                .Include(a => a.ReceiveAmounts)
                .Include(a => a.SaleInvoiceReceives)
                .Include(a => a.ReturnSaleInvoiceReceives)
                .Where(p => p.Guid == receiveId)
                .SingleOrDefault();
        }

        public Receive GetWithCustomer(Guid receiveId)
        {
            return Context.Receives.AsNoTracking()
                .Include(p => p.Customer).ThenInclude(p => p.User)
                .Include(p => p.ReceiveAmounts).ThenInclude(p => p.BaseCurrency)
                .Include(p => p.ReceiveAmounts).ThenInclude(p => p.Currency)
                .Select(p => new Receive
                {
                    Guid = p.Guid,
                    CustomerId = p.CustomerId,
                    Description = p.Description,
                    ReceiveDate = p.ReceiveDate,
                    InvoiceNum = p.InvoiceNum,
                    Customer = new Customer
                    {
                        User = new User
                        {
                            Name = p.Customer.User.Name
                        }
                    },
                    ReceiveAmounts = p.ReceiveAmounts
                })
                .SingleOrDefault(p => p.Guid == receiveId);
        }

        public Receive GetWithReceiveAmount(Guid receiveId)
        {
            return Context.Receives.AsNoTracking()
                .Include(p => p.ReceiveAmounts)
                .SingleOrDefault(p => p.Guid == receiveId);
        }

        public string CheckReceiveStatus(Guid receiveId)
        {
            string query = $@"SELECT STRING_AGG(CONCAT(CAST(pp.FullPay AS NVARCHAR(50)),CAST(rpp.FullPay AS NVARCHAR(50)),' '),' ') Status 
							  FROM dbo.Receive
                              LEFT JOIN dbo.SaleInvoiceReceive pp ON pp.ReceiveId = Receive.Guid
							  LEFT JOIN dbo.ReturnSaleInvoiceReceive rpp ON rpp.ReceiveId = Receive.Guid
							  WHERE Receive.Guid='{receiveId}'
                            ";
            try
            {
                return Context.Set<CheckStatusModel>().FromSqlRaw(query).FirstOrDefault().Status;
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<Receive> GetPartialReceiveHistory(IEnumerable<string> ids)
        {
            return null;
            //return _context.Receives.AsNoTracking()
            //    .Include(p => p.Currency)
            //    .Where(p => ids.Contains(p.Guid.ToString()))
            //    .Select(p => new Receive
            //    {
            //        Amount = p.Amount,
            //        BaseAmount = p.BaseAmount,
            //        DestAmount = p.DestAmount,
            //        Description = p.Description,
            //        Discount = p.Discount,
            //        ReceiveDate = p.ReceiveDate,
            //        Currency = new BaseInfoGeneral
            //        {
            //            Name = p.Currency.Name
            //        }
            //    });
        }

        public decimal GetSaleInvoiceReceives(Guid saleInvoiceId, int currencyId)
        {
            return Context.ReceiveAmounts.Include(a => a.Receive)
                .Where(a => a.BaseCurrencyId == currencyId && a.Receive.SaleInvoiceId == saleInvoiceId)
                .Sum(a => a.Amount * (a.DestAmount / a.BaseAmount)).GetValueOrDefault();

        }
    }
}
