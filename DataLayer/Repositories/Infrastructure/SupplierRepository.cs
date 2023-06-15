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
using static Common.Enums;

namespace DataLayer.Repositories.Infrastructure
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SupplierRepository(WASContext context)
            : base(context)
        {
        }

        public bool CheckSupplierExistBaseOnName(Guid clinicSectionId, string name, Expression<Func<Supplier, bool>> predicate = null)
        {
            IQueryable<Supplier> result = _context.Suppliers.AsNoTracking()
                .Where(p => p.ClinicSectionId == clinicSectionId && p.User.Name == name);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }

        public IEnumerable<Supplier> GetAllSupplier(Guid clinicSectionId)
        {
            return Context.Suppliers
                .AsNoTracking()
                .Include(a => a.City)
                .Include(a => a.Country)
                .Include(a => a.SupplierType)
                .Include(a => a.User)
                .Where(p => p.ClinicSectionId == clinicSectionId);
        }

        public IEnumerable<Supplier> GetAllSupplierName(Guid clinicSectionId)
        {
            return Context.Suppliers
                .AsNoTracking()
                .Include(a => a.User)
                .Where(p => p.ClinicSectionId == clinicSectionId)
                .Select(p => new Supplier
                {
                    Guid = p.Guid,
                    User = new User
                    {
                        Name = p.User.Name
                    }
                });
        }

        public Supplier GetSupplier(Guid supplierId)
        {
            return Context.Suppliers
                .AsNoTracking()
                .Include(a => a.City)
                .Include(a => a.Country)
                .Include(a => a.SupplierType)
                .Include(a => a.User)
                .SingleOrDefault(x => x.Guid == supplierId);
        }

        public Supplier GetSupplierName(Guid supplierId)
        {
            return Context.Suppliers.AsNoTracking()
                .Include(a => a.User)
                .Select(p => new Supplier
                {
                    Guid = p.Guid,
                    User = new User
                    {
                        Name = p.User.Name
                    }
                })
                .SingleOrDefault(x => x.Guid == supplierId);
        }

        public IEnumerable<SupplierAccountModel> GetAllSupplierAccount(Guid supplierId, int? currencyId, SupplierFilter filer, DateTime fromDate, DateTime toDate)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            string fromDate_txt = fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures);
            string toDate_txt = toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures);
            string currency = currencyId == null ? " " : $" AND pa.BaseCurrencyId={currencyId} ";
            string purchase_invoice =
                            $@"UNION
                              SELECT Guid,'Purchase' RecordType,InvoiceDate,InvoiceNum,Description,'0' PayAmount,TotalPrice GetAmount FROM dbo.PurchaseInvoice
                              WHERE SupplierId='{supplierId}' AND InvoiceDate>='{fromDate_txt}' AND InvoiceDate<='{toDate_txt}' AND TotalPrice<>'' AND TotalPrice LIKE N'%'+(ISNULL((SELECT * FROM (SELECT Name FROM dbo.BaseInfoGeneral WHERE Id='{currencyId}') n),''))+'%' 
                            ";

            string return_invoice =
                            $@"UNION
                              SELECT Guid,'Return' RecordType,InvoiceDate,InvoiceNum,Description,'0' PayAmount,TotalPrice GetAmount FROM dbo.ReturnPurchaseInvoice
                              WHERE SupplierId='{supplierId}' AND InvoiceDate>='{fromDate_txt}' AND InvoiceDate<='{toDate_txt}' AND TotalPrice<>'' AND TotalPrice LIKE N'%'+(ISNULL((SELECT * FROM (SELECT Name FROM dbo.BaseInfoGeneral WHERE Id='{currencyId}') n),''))+'%' 
                            ";

            string pay_invoice =
                            $@"UNION
                              SELECT p.Guid,'Pay' RecordType,p.PayDate InvoiceDate,p.InvoiceNum InvocieNum,CONCAT(am.Description,' | ',p.Description) Description,am.PayAmount,'0' GetAmount FROM dbo.Pay p
							  LEFT JOIN (
							  SELECT pa.PayId,STRING_AGG(CONCAT('Pay ',base.Name,' -> ',b.Name,' ',CAST(FORMAT(((pa.Amount-ISNULL(pa.Discount,0))*pa.DestAmount/pa.BaseAmount),'#,#.##')AS NVARCHAR(200))),'|') Description,STRING_AGG( CONCAT( base.Name,' ', CAST(FORMAT(pa.Amount,'0.##') AS NVARCHAR(200))) ,'_') PayAmount FROM dbo.PayAmount pa
							  LEFT JOIN dbo.BaseInfoGeneral b ON b.Id = pa.CurrencyId
							  LEFT JOIN dbo.BaseInfoGeneral base ON base.Id = pa.BaseCurrencyId
							  WHERE 1=1 {currency} 
							  GROUP BY pa.PayId
							  ) am ON am.PayId = p.Guid
							  WHERE p.SupplierId='{supplierId}' AND p.PayDate>='{fromDate_txt}' AND p.PayDate<='{toDate_txt}'  
                            "; 

            string not_recived_invoice = " AND pay.InvoiceNum is NULL  AND pay.RetunInvoiceNum IS NULL ";
            string partial_recived_invoice = " AND pay.payStatus LIKE N'%0%' ";
            string recived_invoice = " AND pay.payStatus LIKE N'%1%' ";

            switch (filer)
            {
                case SupplierFilter.All:
                    {
                        not_recived_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.CashPayment:
                    {
                        purchase_invoice = return_invoice = not_recived_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.GetCash:
                    {
                        purchase_invoice = return_invoice = pay_invoice = not_recived_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.Invoice:
                    {
                        pay_invoice = not_recived_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.NotReceivedInvoices:
                    {
                        pay_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.PartialReceivedInvoices:
                    {
                        pay_invoice = not_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.ReceivedInvoices:
                    {
                        pay_invoice = not_recived_invoice = partial_recived_invoice = "";
                    }
                    break;
            }

            string query =
                $@"SELECT total.*,pay.PayStatus,pay.InvoiceNum PayInvoiceNum,pay.MainInvoiceNum,pay.RetunInvoiceNum,pay.MainInvoicePay FROM (
                   SELECT NULL Guid,NULL RecordType,NULL InvoiceDate,NULL InvoiceNum,NULL Description,'0' PayAmount,NULL GetAmount
                   {purchase_invoice}
                   {return_invoice}
                   {pay_invoice}
                    ) total LEFT JOIN (

                     SELECT CONCAT(CAST(InvoiceId AS NVARCHAR(50)),',',STRING_AGG(CAST(PayId AS NVARCHAR(50)),',')) ids,STRING_AGG(ISNULL(FullPay,0),' ') payStatus,STRING_AGG(PurchaseInvoice.InvoiceNum,',') InvoiceNum,STRING_AGG(PurchaseInvoice.MainInvoiceNum,',') MainInvoiceNum,NULL RetunInvoiceNum,STRING_AGG(Pay.MainInvoiceNum,',') MainInvoicePay  FROM dbo.PurchaseInvoicePay 
                     LEFT JOIN dbo.PurchaseInvoice ON PurchaseInvoice.Guid = PurchaseInvoicePay.InvoiceId
                     LEFT JOIN dbo.Pay ON Pay.Guid = PurchaseInvoicePay.PayId
                     GROUP BY InvoiceId
					 UNION
					 SELECT CONCAT(CAST(InvoiceId AS NVARCHAR(50)),',',STRING_AGG(CAST(PayId AS NVARCHAR(50)),',')) ids,STRING_AGG(ISNULL(FullPay,0),' ') payStatus,NULL InvoiceNum,NULL MainInvoiceNum,STRING_AGG(ReturnPurchaseInvoice.InvoiceNum,',') RetunInvoiceNum,STRING_AGG(MainInvoiceNum,',') MainInvoicePay FROM dbo.ReturnPurchaseInvoicePay
                     LEFT JOIN dbo.ReturnPurchaseInvoice ON ReturnPurchaseInvoice.Guid = ReturnPurchaseInvoicePay.InvoiceId
                     LEFT JOIN dbo.Pay ON Pay.Guid = ReturnPurchaseInvoicePay.PayId
                     GROUP BY InvoiceId
					 UNION
					 SELECT CAST(Guid AS NVARCHAR(50)) ids,NULL payStatus,NULL InvoiceNum,NULL MainInvoiceNum,NULL RetunInvoiceNum,Pay.MainInvoiceNum MainInvoicePay  FROM dbo.Pay
					 WHERE MainInvoiceNum IS NOT NULL AND Guid NOT IN (SELECT PayId FROM PurchaseInvoicePay)
                     ) pay ON pay.ids LIKE '%'+CAST(total.Guid AS NVARCHAR(50))+'%'
                     WHERE total.Guid IS NOT NULL  AND total.PayAmount IS NOT NULL {not_recived_invoice} {partial_recived_invoice} {recived_invoice}
                     ORDER BY total.InvoiceDate
                ";

            IEnumerable<SupplierAccountModel> sa;
            try
            {
                sa = Context.Set<SupplierAccountModel>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }

            return sa;
        }

        public Supplier GetSupplierAccountDetailReport(Guid supplierId, bool? paid, bool? purchase, string currencyName, int? currencyId, DateTime fromDate, DateTime toDate)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            return _context.Suppliers.AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.PurchaseInvoices).ThenInclude(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Product.ProductType)
                .Include(p => p.PurchaseInvoices).ThenInclude(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Product.Producer)
                .Include(p => p.PurchaseInvoices).ThenInclude(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.PurchaseInvoices).ThenInclude(p => p.PurchaseInvoicePays)
                .Include(p => p.PurchaseInvoices).ThenInclude(p => p.PurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoiceDetails).ThenInclude(p => p.PurchaseInvoiceDetail.Product.ProductType)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoiceDetails).ThenInclude(p => p.PurchaseInvoiceDetail.Product.Producer)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoiceDetails).ThenInclude(p => p.PurchaseInvoiceDetail.Currency)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoiceDetails).ThenInclude(p => p.TransferDetail.Product.ProductType)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoiceDetails).ThenInclude(p => p.TransferDetail.Product.Producer)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoiceDetails).ThenInclude(p => p.TransferDetail.PurchaseCurrency)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoicePays)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == supplierId)
                .Select(p => new Supplier
                {
                    User = new User
                    {
                        Name = p.User.Name
                    },
                    PurchaseInvoices = !purchase.GetValueOrDefault(true) ? null : (ICollection<PurchaseInvoice>)p.PurchaseInvoices.Where(x => (paid == null || (paid.Value ? x.PurchaseInvoicePays.Any() : !x.PurchaseInvoicePays.Any())) && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate && !string.IsNullOrWhiteSpace(x.TotalPrice) && x.TotalPrice.Contains(currencyName)).Select(x => new PurchaseInvoice
                    {
                        Guid = x.Guid,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNum = x.InvoiceNum,
                        MainInvoiceNum = x.MainInvoiceNum,
                        TotalPrice = x.TotalPrice,
                        PurchaseInvoiceDiscounts = (ICollection<PurchaseInvoiceDiscount>)x.PurchaseInvoiceDiscounts.Where(w => currencyId == null || w.CurrencyId == currencyId).Select(s => new PurchaseInvoiceDiscount
                        {
                            Amount = s.Amount,
                            CurrencyName = s.Currency.Name
                        }),
                        PurchaseInvoicePays = (ICollection<PurchaseInvoicePay>)x.PurchaseInvoicePays.Select(s => new PurchaseInvoicePay
                        {
                            FullPay = s.FullPay
                        }),
                        PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)x.PurchaseInvoiceDetails.Where(w => currencyId == null || w.CurrencyId == currencyId).Select(s => new PurchaseInvoiceDetail
                        {
                            CurrencyName = s.Currency.Name,
                            ExpireDate = s.ExpireDate,
                            Num = s.Num,
                            FreeNum = s.FreeNum,
                            PurchasePrice = s.PurchasePrice,
                            Discount = s.Discount,
                            Product = new Product
                            {
                                Name = s.Product.Name,
                                ProductType = new BaseInfo
                                {
                                    Name = s.Product.ProductType.Name
                                },
                                Producer = new BaseInfo
                                {
                                    Name = s.Product.Producer.Name
                                }
                            },

                        })
                    }),
                    ReturnPurchaseInvoices = purchase.GetValueOrDefault(false) ? null : (ICollection<ReturnPurchaseInvoice>)p.ReturnPurchaseInvoices.Where(x => (paid == null || (paid.Value ? x.ReturnPurchaseInvoicePays.Any() : !x.ReturnPurchaseInvoicePays.Any())) && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate && !string.IsNullOrWhiteSpace(x.TotalPrice) && x.TotalPrice.Contains(currencyName)).Select(x => new ReturnPurchaseInvoice
                    {
                        Guid = x.Guid,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNum = x.InvoiceNum,
                        TotalPrice = x.TotalPrice,
                        ReturnPurchaseInvoiceDiscounts = (ICollection<ReturnPurchaseInvoiceDiscount>)x.ReturnPurchaseInvoiceDiscounts.Where(w => currencyId == null || w.CurrencyId == currencyId).Select(s => new ReturnPurchaseInvoiceDiscount
                        {
                            Amount = s.Amount,
                            CurrencyName = s.Currency.Name
                        }),
                        ReturnPurchaseInvoicePays = (ICollection<ReturnPurchaseInvoicePay>)x.ReturnPurchaseInvoicePays.Select(s => new ReturnPurchaseInvoicePay
                        {
                            FullPay = s.FullPay
                        }),
                        ReturnPurchaseInvoiceDetails = (ICollection<ReturnPurchaseInvoiceDetail>)x.ReturnPurchaseInvoiceDetails.Where(w => currencyId == null || w.CurrencyId == currencyId).Select(s => new ReturnPurchaseInvoiceDetail
                        {
                            CurrencyName = s.Currency.Name,
                            Num = s.Num,
                            FreeNum = s.FreeNum,
                            Price = s.Price,
                            Discount = s.Discount,
                            PurchaseInvoiceDetail = new PurchaseInvoiceDetail
                            {
                                Product = new Product
                                {
                                    Name = s.PurchaseInvoiceDetail.Product.Name,
                                    ProductType = new BaseInfo
                                    {
                                        Name = s.PurchaseInvoiceDetail.Product.ProductType.Name
                                    },
                                    Producer = new BaseInfo
                                    {
                                        Name = s.PurchaseInvoiceDetail.Product.Producer.Name
                                    }
                                },
                                ExpireDate = s.PurchaseInvoiceDetail.ExpireDate,
                            },
                            TransferDetail = s.TransferDetail == null ? null : new TransferDetail
                            {
                                Product = new Product
                                {
                                    Name = s.TransferDetail.Product.Name,
                                    ProductType = new BaseInfo
                                    {
                                        Name = s.TransferDetail.Product.ProductType.Name
                                    },
                                    Producer = new BaseInfo
                                    {
                                        Name = s.TransferDetail.Product.Producer.Name
                                    }
                                },
                                ExpireDate = s.TransferDetail.ExpireDate,
                            },

                        })
                    })
                })
                .SingleOrDefault();
        }

        public Supplier GetSupplierAccountReport(Guid supplierId, bool? paid, bool? purchase, string currencyName, DateTime fromDate, DateTime toDate)
        {
            return _context.Suppliers.AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.PurchaseInvoices).ThenInclude(p => p.PurchaseInvoicePays)
                .Include(p => p.PurchaseInvoices).ThenInclude(p => p.PurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoicePays)
                .Include(p => p.ReturnPurchaseInvoices).ThenInclude(p => p.ReturnPurchaseInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == supplierId)
                .Select(p => new Supplier
                {
                    User = new User
                    {
                        Name = p.User.Name
                    },
                    PurchaseInvoices = !purchase.GetValueOrDefault(true) ? null : (ICollection<PurchaseInvoice>)p.PurchaseInvoices.Where(x => (paid == null || (paid.Value ? x.PurchaseInvoicePays.Any() : !x.PurchaseInvoicePays.Any())) && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate && !string.IsNullOrWhiteSpace(x.TotalPrice) && x.TotalPrice.Contains(currencyName)).Select(x => new PurchaseInvoice
                    {
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNum = x.InvoiceNum,
                        MainInvoiceNum = x.MainInvoiceNum,
                        TotalPrice = x.TotalPrice,
                        PurchaseInvoiceDiscounts = (ICollection<PurchaseInvoiceDiscount>)x.PurchaseInvoiceDiscounts.Select(s => new PurchaseInvoiceDiscount
                        {
                            Amount = s.Amount,
                            CurrencyName = s.Currency.Name

                        }),
                        PurchaseInvoicePays = (ICollection<PurchaseInvoicePay>)x.PurchaseInvoicePays.Select(s => new PurchaseInvoicePay
                        {
                            FullPay = s.FullPay
                        })
                    }),
                    ReturnPurchaseInvoices = purchase.GetValueOrDefault(false) ? null : (ICollection<ReturnPurchaseInvoice>)p.ReturnPurchaseInvoices.Where(x => (paid == null || (paid.Value ? x.ReturnPurchaseInvoicePays.Any() : !x.ReturnPurchaseInvoicePays.Any())) && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate && !string.IsNullOrWhiteSpace(x.TotalPrice) && x.TotalPrice.Contains(currencyName)).Select(x => new ReturnPurchaseInvoice
                    {
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNum = x.InvoiceNum,
                        TotalPrice = x.TotalPrice,
                        ReturnPurchaseInvoiceDiscounts = (ICollection<ReturnPurchaseInvoiceDiscount>)x.ReturnPurchaseInvoiceDiscounts.Select(s => new ReturnPurchaseInvoiceDiscount
                        {
                            Amount = s.Amount,
                            CurrencyName = s.Currency.Name
                        }),
                        ReturnPurchaseInvoicePays = (ICollection<ReturnPurchaseInvoicePay>)x.ReturnPurchaseInvoicePays.Select(s => new ReturnPurchaseInvoicePay
                        {
                            FullPay = s.FullPay
                        })
                    })
                })
                .SingleOrDefault();
        }

    }
}
