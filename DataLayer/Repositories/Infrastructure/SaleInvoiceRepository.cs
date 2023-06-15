using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class SaleInvoiceRepository : Repository<SaleInvoice>, ISaleInvoiceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SaleInvoiceRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<SaleInvoice> GetAllSaleInvoice(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid? customerId, string invoiceNum, Guid? productId)
        {

            if (productId != null)
            {
                return Context.SaleInvoices.AsNoTracking()
                .Include(p => p.Customer).ThenInclude(a => a.User)
                .Include(p => p.SaleInvoiceDetails)
                .Where(p => p.SaleInvoiceDetails.Any(a=>a.ProductId == productId) && p.InvoiceDate >= dateFrom && p.InvoiceDate <= dateTo)
                .Select(p => new SaleInvoice
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    Description = p.Description,
                    Status = p.SaleInvoiceReceives.Any(p => p.FullPay != null && p.FullPay.Value),
                    Customer = new Customer
                    {
                        User = new User
                        {
                            Name = p.Customer.User.Name
                        }
                    },
                    TotalPrice = p.TotalPrice,
                    SalePriceType = new BaseInfoGeneral
                    {
                        Name = p.SalePriceType.Name
                    }
                    //Discount = p.Discount,
                }).OrderByDescending(a => a.InvoiceDate).ThenByDescending(a => a.InvoiceNum); ;
            }


            IQueryable<SaleInvoice> result = Context.SaleInvoices.AsNoTracking()
            .Include(p => p.Customer).ThenInclude(a => a.User)
            .Where(p => p.ClinicSectionId == clinicSectionId);

            if (!string.IsNullOrEmpty(invoiceNum))
                return result.Where(a=>a.InvoiceNum == invoiceNum).Select(p => new SaleInvoice
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    Description = p.Description,
                    Status = p.SaleInvoiceReceives.Any(p => p.FullPay != null && p.FullPay.Value),
                    Customer = new Customer
                    {
                        User = new User
                        {
                            Name = p.Customer.User.Name
                        }
                    },
                    TotalPrice = p.TotalPrice,
                    SalePriceType = new BaseInfoGeneral
                    {
                        Name = p.SalePriceType.Name
                    }
                    //Discount = p.Discount,
                });


            var re = result.Where(p => p.InvoiceDate >= dateFrom && p.InvoiceDate <= dateTo);

            IQueryable<SaleInvoice> finall;

            if (customerId != null)
                finall = re.Where(p => p.CustomerId == customerId);
            else
                finall = re;




            return finall.Select(p => new SaleInvoice
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                Status = p.SaleInvoiceReceives.Any(p => p.FullPay != null && p.FullPay.Value),
                Customer = new Customer
                {
                    User = new User
                    {
                        Name = p.Customer.User.Name
                    }
                },
                TotalPrice = p.TotalPrice,
                SalePriceType = new BaseInfoGeneral
                {
                    Name = p.SalePriceType.Name
                }
                //Discount = p.Discount,
            }).OrderByDescending(a => a.InvoiceDate).ThenByDescending(a=>a.InvoiceNum);
        }

        public SaleInvoice GetByReceiveId(Guid receiveId)
        {
            return Context.Receives.AsNoTracking()
                .Include(p => p.SaleInvoice)
                .Where(p => p.Guid == receiveId)
                .Select(p => p.SaleInvoice)
                .SingleOrDefault();
        }

        public SaleInvoice GetWithType(Guid saleInvoiceId)
        {
            return Context.SaleInvoices.AsNoTracking()
                .SingleOrDefault(p => p.Guid == saleInvoiceId);
        }

        public string GetLatestSaleInvoiceNum(Guid clinicSectionId)
        {
            try
            {
                return _context.FN_LatestSaleInvoiceNum(clinicSectionId).FirstOrDefault().CODE;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public SaleInvoice GetForUpdateTotalPrice(Guid saleInvoiceId)
        {
            return _context.SaleInvoices.AsNoTracking()
                .Include(p => p.SaleInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.SaleInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == saleInvoiceId)
                .Select(p => new SaleInvoice
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    CustomerId = p.CustomerId,
                    Description = p.Description,
                    CreatedUserId = p.CreatedUserId,
                    CreatedDate = p.CreatedDate,
                    ModifiedUserId = p.ModifiedUserId,
                    ModifiedDate = p.ModifiedDate,
                    ClinicSectionId = p.ClinicSectionId,
                    TotalPrice = p.TotalPrice,
                    OldFactor = p.OldFactor,
                    SalePriceTypeId = p.SalePriceTypeId,
                    SaleInvoiceDetails = (ICollection<SaleInvoiceDetail>)p.SaleInvoiceDetails.Select(x => new SaleInvoiceDetail
                    {
                        Num = x.Num,
                        SalePrice = x.SalePrice,
                        Discount = x.Discount,
                        Guid = x.Guid,
                        Currency = new BaseInfoGeneral
                        {
                            Name = x.Currency.Name
                        }
                    }),
                    SaleInvoiceDiscounts = (ICollection<SaleInvoiceDiscount>)p.SaleInvoiceDiscounts.Select(x => new SaleInvoiceDiscount
                    {
                        Amount = x.Amount,
                        Guid = x.Guid,
                        Currency = new BaseInfoGeneral
                        {
                            Name = x.Currency.Name
                        }
                    })
                }).SingleOrDefault();
        }

        public SaleInvoice CheckForCurrency(Guid saleInvoiceId, int? currencyId)
        {
            return _context.SaleInvoices.AsNoTracking()
                .Include(p => p.SaleInvoiceDetails)
                .Include(p => p.SaleInvoiceDiscounts)
                .Where(p => p.Guid == saleInvoiceId)
                .Select(p => new SaleInvoice
                {
                    SaleInvoiceDetails = (ICollection<SaleInvoiceDetail>)p.SaleInvoiceDetails.Where(x => x.CurrencyId == currencyId).Select(s => new SaleInvoiceDetail
                    {
                        Guid = s.Guid,
                        Num = s.Num,
                        SalePrice = s.SalePrice,
                        Discount = s.Discount
                    }),
                    SaleInvoiceDiscounts = (ICollection<SaleInvoiceDiscount>)p.SaleInvoiceDiscounts.Where(x => x.CurrencyId == currencyId).Select(s => new SaleInvoiceDiscount
                    {
                        Guid = s.Guid,
                        Amount = s.Amount
                    })
                })
                .SingleOrDefault();
        }

        public IEnumerable<SaleInvoice> GetForReceive(IEnumerable<Guid> receiveIds)
        {
            return _context.SaleInvoices.AsNoTracking()
                .Where(p => receiveIds.Contains(p.Guid));
        }

        public IEnumerable<SaleInvoice> GetReceiveSaleInvoice(Guid? receiveId)
        {
            return _context.SaleInvoiceReceives.AsNoTracking()
                .Include(p => p.Invoice)
                .Where(p => p.ReceiveId == receiveId)
                .Select(p => new SaleInvoice
                {
                    Guid = p.Invoice.Guid,
                    InvoiceNum = p.Invoice.InvoiceNum,
                    InvoiceDate = p.Invoice.InvoiceDate,
                    Description = p.Invoice.Description,
                    TotalPrice = p.Invoice.TotalPrice,
                });
        }

        public IEnumerable<SaleInvoice> GetNotReceiveSaleInvoice(Guid? customerId)
        {
            string query = $@"SELECT pii.Guid,pii.InvoiceNum,pii.InvoiceDate,pii.Description,pii.TotalPrice,pii.Id,NULL CustomerId,NULL CreatedUserId,NULL CreatedDate,NULL ModifiedUserId,NULL ModifiedDate,NULL ClinicSectionId,NULL OldFactor,NULL SalePriceTypeId FROM dbo.SaleInvoice pii WHERE 
                             pii.CustomerId = '{customerId}' AND  ISNULL(pii.TotalPrice,'')<>''  AND  
                             NOT EXISTS(SELECT pp.Guid FROM dbo.SaleInvoiceReceive pp LEFT JOIN dbo.Receive p ON p.Guid = pp.ReceiveId WHERE pp.InvoiceId = pii.Guid
	                         UNION 
	                         SELECT rec.Guid FROM dbo.Receive rec WHERE rec.SaleInvoiceId=pii.Guid )
                            ";

            try
            {
                return Context.Set<SaleInvoice>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public void RemoveSaleInvoice(Guid saleInvoiceid)
        {
            try
            {
                var allDetail = _context.SaleInvoiceDetails.AsNoTracking().Include(a => a.PurchaseInvoiceDetail).Include(a => a.TransferDetail)
                .Where(a => a.SaleInvoiceId == saleInvoiceid).Select(a => new SaleInvoiceDetail
                {
                    Guid = a.Guid,
                    Num = a.Num,
                    FreeNum = a.FreeNum,
                    SaleInvoiceId = a.SaleInvoiceId,
                    PurchaseInvoiceDetail = (a.PurchaseInvoiceDetail == null) ? null : new PurchaseInvoiceDetail
                    {
                        RemainingNum = a.PurchaseInvoiceDetail.RemainingNum,
                        Guid = a.PurchaseInvoiceDetail.Guid
                    },
                    TransferDetail = (a.TransferDetail == null) ? null : new TransferDetail
                    {
                        RemainingNum = a.TransferDetail.RemainingNum,
                        Guid = a.TransferDetail.Guid
                    }
                });

                foreach (var detail in allDetail)
                {
                    if (detail.PurchaseInvoiceDetail != null)
                    {
                        detail.PurchaseInvoiceDetail.RemainingNum += (detail.Num + detail.FreeNum);
                        //_context.Entry(detail.PurchaseInvoiceDetail).State = EntityState.Detached;
                        var localPu = _context.PurchaseInvoiceDetails.Local.SingleOrDefault(x => x.Guid == detail.PurchaseInvoiceDetail.Guid);
                        if (localPu == null)
                        {
                            _context.Entry(detail.PurchaseInvoiceDetail).Property(a => a.RemainingNum).IsModified = true;
                        }
                        else
                        {
                            localPu.RemainingNum += (detail.Num + detail.FreeNum);
                        }

                    }
                    else
                    {
                        detail.TransferDetail.RemainingNum += (detail.Num + detail.FreeNum);
                        var localTr = _context.TransferDetails.Local.SingleOrDefault(x => x.Guid == detail.TransferDetail.Guid);
                        if (localTr == null)
                        {
                            _context.Entry(detail.TransferDetail).Property(a => a.RemainingNum).IsModified = true;
                        }
                        else
                        {
                            localTr.RemainingNum += (detail.Num + detail.FreeNum);
                        }
                    }
                    _context.Entry(detail).State = EntityState.Deleted;

                }
                //var gu = allDetail.Select(a => new SaleInvoiceDetail 
                //{
                //    Guid = a.Guid,
                //    SaleInvoiceId = a.SaleInvoiceId
                //});
                //_context.SaleInvoiceDetails.RemoveRange(allDetail);
                _context.SaleInvoices.Remove(_context.SaleInvoices.FirstOrDefault(a => a.Guid == saleInvoiceid));
                _context.SaveChanges();

                //var sale = _context.SaleInvoices.Include(a => a.SaleInvoiceDetails).FirstOrDefault(a=>a.Guid == saleInvoiceid);
                //_context.SaleInvoices.Remove(sale);
                //_context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public IEnumerable<IncomeModel> GetAllIncomes(Guid clinicSectionId)
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime lastWeek = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-7).Day, 0, 0, 0);
            return _context.SaleInvoiceDetails.Include(a => a.SaleInvoice).Include(a => a.Currency)
                .Where(a => a.SaleInvoice.ClinicSectionId == clinicSectionId && a.SaleInvoice.InvoiceDate < today && a.SaleInvoice.InvoiceDate > lastWeek)
                .Select(a => new IncomeModel
                {
                    Date = a.SaleInvoice.InvoiceDate.GetValueOrDefault().Date,
                    SalePrice = ((a.SalePrice - a.Discount) * a.Num) ?? 0,
                    CurrencyName = a.Currency.Name
                });
        }

        public IEnumerable<IncomeModel> GetAllStoreInCome(Guid clinicSectionId)
        {
            return _context.SaleInvoiceDetails.Include(a => a.SaleInvoice).Include(a => a.Currency)
                .Where(a => a.SaleInvoice.ClinicSectionId == clinicSectionId)
                .Select(a => new IncomeModel
                {
                    Date = a.SaleInvoice.InvoiceDate.Value,
                    SalePrice = ((a.SalePrice - a.Discount) * a.Num) ?? 0,
                    CurrencyName = a.Currency.Name
                });
        }
    }
}
