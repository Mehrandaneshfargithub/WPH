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
    public class SaleInvoiceDetailRepository : Repository<SaleInvoiceDetail>, ISaleInvoiceDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SaleInvoiceDetailRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<SaleInvoiceDetail> GetAllSaleInvoiceDetailByMasterId(Guid saleInvoiceId)
        {
            return Context.SaleInvoiceDetails.AsNoTracking()
                .Include(p => p.Product)
                .Include(p => p.Currency)
                .Include(p => p.SaleInvoice).ThenInclude(a => a.SalePriceType)
                .Include(p => p.MoneyConvert).ThenInclude(a => a.BaseCurrency)
                .Include(p => p.MoneyConvert).ThenInclude(a => a.DestCurrency)
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(a => a.Currency)
                .Include(p => p.TransferDetail).ThenInclude(a => a.PurchaseCurrency)
                .Where(p => p.SaleInvoiceId == saleInvoiceId).Select(a => new SaleInvoiceDetail
                {
                    SaleInvoice = new SaleInvoice
                    {
                        SalePriceType = new BaseInfoGeneral
                        {
                            Name = a.SaleInvoice.SalePriceType.Name
                        }
                    },
                    CreatedDate = a.CreatedDate,
                    BujNumber = a.BujNumber,
                    Consideration = a.Consideration,
                    Currency = new BaseInfoGeneral
                    {
                        Name = a.Currency.Name
                    },
                    CurrencyId = a.CurrencyId,
                    Discount = a.Discount,
                    FreeNum = a.FreeNum,
                    Guid = a.Guid,
                    MoneyConvert = a.MoneyConvert,
                    MoneyConvertId = a.MoneyConvertId,
                    Num = a.Num,
                    Product = new Product
                    {
                        Name = a.Product.Name
                    },
                    ProductId = a.ProductId,
                    PurchaseInvoiceDetailId = a.PurchaseInvoiceDetailId,
                    PurchaseInvoiceDetail = a.PurchaseInvoiceDetail == null ? null : new PurchaseInvoiceDetail
                    {
                        Currency = new BaseInfoGeneral
                        {
                            Name = a.PurchaseInvoiceDetail.Currency.Name
                        },
                        CurrencyId = a.CurrencyId,
                        ExpireDate = a.PurchaseInvoiceDetail.ExpireDate,
                        PurchasePrice = a.PurchaseInvoiceDetail.PurchasePrice,
                        SellingPrice = a.PurchaseInvoiceDetail.SellingPrice,
                        MiddleSellPrice = a.PurchaseInvoiceDetail.MiddleSellPrice,
                        WholeSellPrice = a.PurchaseInvoiceDetail.WholeSellPrice,
                        RemainingNum = a.PurchaseInvoiceDetail.RemainingNum
                    },
                    SalePrice = a.SalePrice,
                    TransferDetailId = a.TransferDetailId,
                    TransferDetail = a.TransferDetail == null ? null : new TransferDetail
                    {
                        PurchaseCurrency = new BaseInfoGeneral
                        {
                            Name = a.TransferDetail.PurchaseCurrency.Name
                        },
                        CurrencyId = a.TransferDetail.CurrencyId,
                        ExpireDate = a.TransferDetail.ExpireDate,
                        RemainingNum = a.TransferDetail.RemainingNum,
                        PurchasePrice = a.TransferDetail.PurchasePrice,
                        SellingPrice = a.TransferDetail.SellingPrice,
                        MiddleSellPrice = a.TransferDetail.MiddleSellPrice,
                        WholeSellPrice = a.TransferDetail.WholeSellPrice,
                    }

                }).OrderByDescending(a => a.CreatedDate);

        }

        public IEnumerable<SaleInvoiceDetail> GetAllDetail(IEnumerable<Guid> saleInvoiceDetailIds)
        {
            return Context.SaleInvoiceDetails.AsNoTracking()
                .Include(p => p.Product)
                .Include(p => p.Currency)
                .Include(p => p.SaleInvoice).ThenInclude(a => a.SalePriceType)
                .Include(p => p.MoneyConvert).ThenInclude(a => a.BaseCurrency)
                .Include(p => p.MoneyConvert).ThenInclude(a => a.DestCurrency)
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(a => a.Currency)
                .Include(p => p.TransferDetail).ThenInclude(a => a.PurchaseCurrency)
                .Where(p => saleInvoiceDetailIds.Contains(p.Guid)).Select(a => new SaleInvoiceDetail
                {
                    SaleInvoice = new SaleInvoice
                    {
                        SalePriceType = new BaseInfoGeneral
                        {
                            Name = a.SaleInvoice.SalePriceType.Name
                        }
                    },
                    CreatedDate = a.CreatedDate,
                    BujNumber = a.BujNumber,
                    Consideration = a.Consideration,
                    Currency = new BaseInfoGeneral
                    {
                        Name = a.Currency.Name
                    },
                    CurrencyId = a.CurrencyId,
                    Discount = a.Discount,
                    FreeNum = a.FreeNum,
                    Guid = a.Guid,
                    MoneyConvert = a.MoneyConvert,
                    MoneyConvertId = a.MoneyConvertId,
                    Num = a.Num,
                    Product = new Product
                    {
                        Name = a.Product.Name
                    },
                    ProductId = a.ProductId,
                    PurchaseInvoiceDetailId = a.PurchaseInvoiceDetailId,
                    PurchaseInvoiceDetail = a.PurchaseInvoiceDetail == null ? null : new PurchaseInvoiceDetail
                    {
                        Currency = new BaseInfoGeneral
                        {
                            Name = a.PurchaseInvoiceDetail.Currency.Name
                        },
                        CurrencyId = a.CurrencyId,
                        ExpireDate = a.PurchaseInvoiceDetail.ExpireDate,
                        PurchasePrice = a.PurchaseInvoiceDetail.PurchasePrice,
                        SellingPrice = a.PurchaseInvoiceDetail.SellingPrice,
                        MiddleSellPrice = a.PurchaseInvoiceDetail.MiddleSellPrice,
                        WholeSellPrice = a.PurchaseInvoiceDetail.WholeSellPrice,
                        RemainingNum = a.PurchaseInvoiceDetail.RemainingNum
                    },
                    SalePrice = a.SalePrice,
                    TransferDetailId = a.TransferDetailId,
                    TransferDetail = a.TransferDetail == null ? null : new TransferDetail
                    {
                        PurchaseCurrency = new BaseInfoGeneral
                        {
                            Name = a.TransferDetail.PurchaseCurrency.Name
                        },
                        CurrencyId = a.TransferDetail.CurrencyId,
                        ExpireDate = a.TransferDetail.ExpireDate,
                        RemainingNum = a.TransferDetail.RemainingNum
                    }

                }).OrderByDescending(a => a.CreatedDate);
        }
    

        public IEnumerable<SaleInvoiceDetail> GetByMultipleIds(List<Guid> details)
        {
            return _context.SaleInvoiceDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetail)
                .Include(p => p.TransferDetail)
                .Where(p => details.Contains(p.Guid) && p.RemainingNum > 0);
        }

        public SaleInvoiceDetail GetWithPurchaseAndTransfer(Guid detailId)
        {
            return _context.SaleInvoiceDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetail)
                .Include(p => p.TransferDetail)
                .SingleOrDefault(p => p.Guid == detailId);
        }

        public IEnumerable<SaleInvoiceDetail> GetDetailsForReturn(Guid productId, Guid masterId, Guid clinicSectionId, bool like)
        {

            IQueryable<SaleInvoiceDetail> result = _context.SaleInvoiceDetails.AsNoTracking()
                .Include(p => p.SaleInvoice)
                .Include(p => p.PurchaseInvoiceDetail)
                .Include(p => p.TransferDetail)
                .Include(p => p.Currency)
                .Where(p => p.ProductId == productId && p.SaleInvoice.ClinicSectionId == clinicSectionId && p.RemainingNum > 0)
                ;
            if (like)
                result = result.Where(p => p.SaleInvoice.CustomerId == _context.ReturnSaleInvoices.FirstOrDefault(x => x.Guid == masterId).CustomerId);

            return result.Select(p => new SaleInvoiceDetail
            {
                Guid = p.Guid,
                RemainingNum = p.RemainingNum,
                Num = p.Num,
                FreeNum = p.FreeNum,
                BujNumber = p.BujNumber,
                Discount = p.Discount,
                Consideration = p.Consideration,
                SalePrice = p.SalePrice,
                SaleInvoice = new SaleInvoice
                {
                    InvoiceDate = p.SaleInvoice.InvoiceDate,
                    InvoiceNum = p.SaleInvoice.InvoiceNum,
                },
                PurchaseInvoiceDetail = new PurchaseInvoiceDetail
                {
                    ExpireDate = p.PurchaseInvoiceDetail.ExpireDate
                },
                TransferDetail = new TransferDetail
                {
                    ExpireDate = p.TransferDetail.ExpireDate
                },
                Currency = new BaseInfoGeneral
                {
                    Name = p.Currency.Name
                }
            });

        }


        public IEnumerable<PieChartModel> GetMostSaledProducts(Guid clinicSectionId)
        {

            string qury = $@"SELECT TOP 3  SUM(RemainingNum)  AS Value, dbo.Product.Name AS Label
                                FROM dbo.SaleInvoiceDetails
                                INNER JOIN dbo.SaleInvoice ON SaleInvoice.GUID = SaleInvoiceDetails.SaleInvoiceId
                                INNER JOIN dbo.Product ON Product.GUID = SaleInvoiceDetails.ProductId
                                WHERE dbo.SaleInvoice.ClinicSectionId = '{clinicSectionId}'
                                GROUP BY dbo.Product.Name ORDER BY Value DESC";

            return Context.Set<PieChartModel>().FromSqlRaw(qury);

            //return _context.SaleInvoiceDetails.AsNoTracking()
            //    .Include(p => p.SaleInvoice)
            //    .Include(p => p.Product)
            //    .Where(p => p.SaleInvoice.ClinicSectionId == clinicSectionId && p.RemainingNum > 0).Select(p => new SaleInvoiceDetail
            //    {
            //        RemainingNum = p.RemainingNum,
            //        Num = p.Num,
            //        FreeNum = p.FreeNum,
            //        Product = new Product
            //        {
            //            Name = p.Product.Name
            //        },
            //        SaleInvoice = new SaleInvoice
            //        {
            //            ClinicSectionId = p.SaleInvoice.ClinicSectionId
            //        }
            //    }); 

        }

        public void UpdatePurchaseAndTransferStock(List<PurchaseOrTransferProductDetail> allupdates)
        {

            foreach(var purchase in allupdates)
            {
                if(purchase.SaleType == "PurchaseInvoiceDetail")
                {
                    PurchaseInvoiceDetail pu = new PurchaseInvoiceDetail()
                    {
                        Guid = purchase.Guid,
                        RemainingNum = purchase.Stock
                    };

                    //Context.Entry(pu).State = EntityState.Modified;
                    Context.Entry(pu).Property(x => x.RemainingNum).IsModified = true;

                }
                else
                {
                    TransferDetail tr = new TransferDetail()
                    {
                        Guid = purchase.Guid,
                        RemainingNum = purchase.Stock
                    };

                    //Context.Entry(tr).State = EntityState.Modified;
                    Context.Entry(tr).Property(x => x.RemainingNum).IsModified = true;
                }
            }

        }
    }
}
