using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReceiveAmountRepository : Repository<ReceiveAmount>, IReceiveAmountRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceiveAmountRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReceiveAmount> GetAllReceiveAmount(Guid saleInvoiceId, int currencyId)
        {
            return _context.ReceiveAmounts.Include(a => a.Receive).Include(a => a.BaseCurrency).Where(a => a.CurrencyId == currencyId && a.Receive.SaleInvoiceId == saleInvoiceId).Select(a => new ReceiveAmount
            {
                Guid = a.Guid,
                Amount = a.Amount,
                BaseCurrencyId = a.BaseCurrencyId,
                CurrencyId = a.CurrencyId,
                BaseAmount = a.BaseAmount,
                DestAmount = a.DestAmount,
                Receive = new Receive 
                {
                    ReceiveDate = a.Receive.ReceiveDate
                },
                BaseCurrency = new BaseInfoGeneral
                {
                    Name = a.BaseCurrency.Name
                }

            });
        }
    }
}
