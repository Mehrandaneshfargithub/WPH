using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using static Common.Enums;

namespace DataLayer.Repositories.Infrastructure
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public CustomerRepository(WASContext context)
            : base(context)
        {
        }

        public bool CheckCustomerExistBaseOnName(Guid clinicSectionId, string name, Expression<Func<Customer, bool>> predicate = null)
        {
            IQueryable<Customer> result = _context.Customers.AsNoTracking()
                .Where(p => p.ClinicSectionId == clinicSectionId && p.User.Name == name);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }

        public IEnumerable<Customer> GetAllCustomer(Guid clinicSectionId)
        {
            return Context.Customers
                .AsNoTracking()
                .Include(a => a.City)
                .Include(a => a.CustomerType)
                .Include(a => a.User)
                .Where(p => p.ClinicSectionId == clinicSectionId)
                .Select(p => new Customer
                {
                    Guid = p.Guid,
                    Address = p.Address,
                    User = new User
                    {
                        Name = p.User.Name,
                        PhoneNumber = p.User.PhoneNumber
                    },
                    CustomerType = new BaseInfo
                    {
                        Name = p.CustomerType.Name
                    },
                    City = new BaseInfo
                    {
                        Name = p.City.Name
                    }
                });
        }

        public IEnumerable<Customer> GetAllCustomerName(Guid clinicSectionId)
        {
            return Context.Customers
                .AsNoTracking()
                .Include(a => a.User)
                .Where(p => p.ClinicSectionId == clinicSectionId)
                .Select(p => new Customer
                {
                    Guid = p.Guid,
                    User = new User
                    {
                        Name = p.User.Name
                    }
                });
        }

        public Customer GetCustomerWithUser(Guid customerId)
        {
            return Context.Customers
                .AsNoTracking()
                .Include(a => a.User)
                .SingleOrDefault(x => x.Guid == customerId);
        }

        public Customer GetCustomer(Guid customerId)
        {
            return Context.Customers
                .AsNoTracking()
                .Include(a => a.City)
                .Include(a => a.CustomerType)
                .Include(a => a.User)
                .SingleOrDefault(x => x.Guid == customerId);
        }

        public Customer GetCustomerName(Guid customerId)
        {
            return Context.Customers.AsNoTracking()
                .Include(a => a.User)
                .Select(p => new Customer
                {
                    Guid = p.Guid,
                    User = new User
                    {
                        Name = p.User.Name
                    }
                })
                .SingleOrDefault(x => x.Guid == customerId);
        }

        public IEnumerable<CustomerAccountModel> GetAllCustomerAccount(Guid CustomerId, int? currencyId, SupplierFilter filer, DateTime fromDate, DateTime toDate)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            string fromDate_txt = fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures);
            string toDate_txt = toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures);
            string currency = currencyId == null ? " " : $" AND pa.BaseCurrencyId={currencyId} ";
            string sale_invoice =
                            $@"UNION
                              SELECT Guid,'Sale' RecordType,InvoiceDate,InvoiceNum,Description,'0' ReceiveAmount,TotalPrice GetAmount FROM dbo.SaleInvoice
                              WHERE CustomerId='{CustomerId}' AND InvoiceDate>='{fromDate_txt}' AND InvoiceDate<='{toDate_txt}' AND TotalPrice<>'' AND TotalPrice LIKE N'%'+(ISNULL((SELECT * FROM (SELECT Name FROM dbo.BaseInfoGeneral WHERE Id='{currencyId}') n),''))+'%' 
                            ";

            string return_invoice =
                            $@"UNION
                              SELECT Guid,'Return' RecordType,InvoiceDate,InvoiceNum,Description,'0' ReceiveAmount,TotalPrice GetAmount FROM dbo.ReturnSaleInvoice
                              WHERE CustomerId='{CustomerId}' AND InvoiceDate>='{fromDate_txt}' AND InvoiceDate<='{toDate_txt}' AND TotalPrice<>'' AND TotalPrice LIKE N'%'+(ISNULL((SELECT * FROM (SELECT Name FROM dbo.BaseInfoGeneral WHERE Id='{currencyId}') n),''))+'%' 
                            ";

            string receive_invoice =
                            $@"UNION
                              SELECT p.Guid,'Receive' RecordType,p.ReceiveDate InvoiceDate,p.InvoiceNum InvocieNum,CONCAT(am.Description,' | ',p.Description) Description,am.ReceiveAmount,'0' GetAmount FROM dbo.Receive p
							  LEFT JOIN (
							  SELECT pa.ReceiveId,STRING_AGG(CONCAT('Receive ',base.Name,' -> ',b.Name,' ',CAST(FORMAT(((pa.Amount-ISNULL(pa.Discount,0))*pa.DestAmount/pa.BaseAmount),'#,#.##')AS NVARCHAR(200))),'|') Description,STRING_AGG( CONCAT( base.Name,' ', CAST(FORMAT(pa.Amount,'0.##') AS NVARCHAR(200))) ,'_') ReceiveAmount FROM dbo.ReceiveAmount pa
							  LEFT JOIN dbo.BaseInfoGeneral b ON b.Id = pa.CurrencyId
							  LEFT JOIN dbo.BaseInfoGeneral base ON base.Id = pa.BaseCurrencyId
							  WHERE 1=1 {currency} 
							  GROUP BY pa.ReceiveId
							  ) am ON am.ReceiveId = p.Guid
							  WHERE p.CustomerId='{CustomerId}' AND p.ReceiveDate>='{fromDate_txt}' AND p.ReceiveDate<='{toDate_txt}'  
                            ";

            string not_recived_invoice = " AND receive.InvoiceNum is NULL  AND receive.RetunInvoiceNum  is NULL ";
            string partial_recived_invoice = " AND receive.ReceiveStatus LIKE N'%0%' ";
            string recived_invoice = " AND receive.ReceiveStatus LIKE N'%1%' ";

            switch (filer)
            {
                case SupplierFilter.All:
                    {
                        not_recived_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.CashPayment:
                    {
                        sale_invoice = return_invoice = not_recived_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.GetCash:
                    {
                        sale_invoice = return_invoice = receive_invoice = not_recived_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.Invoice:
                    {
                        receive_invoice = not_recived_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.NotReceivedInvoices:
                    {
                        receive_invoice = partial_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.PartialReceivedInvoices:
                    {
                        receive_invoice = not_recived_invoice = recived_invoice = "";
                    }
                    break;
                case SupplierFilter.ReceivedInvoices:
                    {
                        receive_invoice = not_recived_invoice = partial_recived_invoice = "";
                    }
                    break;
            }

            string query =
                $@"SELECT total.*,receive.ReceiveStatus,receive.InvoiceNum ReceiveInvoiceNum,receive.RetunInvoiceNum FROM (
                   SELECT NULL Guid,NULL RecordType,NULL InvoiceDate,NULL InvoiceNum,NULL Description,'0' ReceiveAmount,NULL GetAmount
                   {sale_invoice}
                   {return_invoice}
                   {receive_invoice}
                    ) total LEFT JOIN (

                     SELECT CONCAT(CAST(InvoiceId AS NVARCHAR(50)),',',STRING_AGG(CAST(ReceiveId AS NVARCHAR(50)),',')) ids,STRING_AGG(ISNULL(FullPay,0),' ') receiveStatus,STRING_AGG(SaleInvoice.InvoiceNum,',') InvoiceNum,NULL RetunInvoiceNum FROM dbo.SaleInvoiceReceive 
                     LEFT JOIN dbo.SaleInvoice ON SaleInvoice.Guid = SaleInvoiceReceive.InvoiceId
                     LEFT JOIN dbo.Receive ON Receive.Guid = SaleInvoiceReceive.ReceiveId
                     GROUP BY InvoiceId
					 UNION
					 SELECT CONCAT(CAST(InvoiceId AS NVARCHAR(50)),',',STRING_AGG(CAST(ReceiveId AS NVARCHAR(50)),',')) ids,STRING_AGG(ISNULL(FullPay,0),' ') receiveStatus,NULL InvoiceNum,STRING_AGG(ReturnSaleInvoice.InvoiceNum,',') RetunInvoiceNum FROM dbo.ReturnSaleInvoiceReceive
                     LEFT JOIN dbo.ReturnSaleInvoice ON ReturnSaleInvoice.Guid = ReturnSaleInvoiceReceive.InvoiceId
                     LEFT JOIN dbo.Receive ON Receive.Guid = ReturnSaleInvoiceReceive.ReceiveId
                     GROUP BY InvoiceId
					 UNION
					 SELECT CAST(Receive.Guid AS NVARCHAR(50)) ids,'2' receiveStatus,SaleInvoice.InvoiceNum,NULL RetunInvoiceNum FROM dbo.SaleInvoice 
                     LEFT JOIN dbo.Receive ON Receive.SaleInvoiceId = SaleInvoice.GUID
					 WHERE SaleInvoiceId IS NOT NULL
                     ) receive ON receive.ids LIKE '%'+CAST(total.Guid AS NVARCHAR(50))+'%'
                     WHERE total.Guid IS NOT NULL  AND total.ReceiveAmount IS NOT NULL {not_recived_invoice} {partial_recived_invoice} {recived_invoice}
                     ORDER BY total.InvoiceDate
                ";

            IEnumerable<CustomerAccountModel> sa;
            try
            {
                sa = Context.Set<CustomerAccountModel>().FromSqlRaw(query);
            }
            catch (Exception e) { return null; }

            return sa;

        }



        public Customer GetCustomerAccountDetailReport(Guid customerId, bool? paid, bool? purchase, string currencyName, int? currencyId, DateTime fromDate, DateTime toDate)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            return _context.Customers.AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.SaleInvoiceDetails).ThenInclude(p => p.Product.ProductType)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.SaleInvoiceDetails).ThenInclude(p => p.Product.Producer)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.SaleInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.SaleInvoiceDetails).ThenInclude(p => p.PurchaseInvoiceDetail)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.SaleInvoiceDetails).ThenInclude(p => p.TransferDetail)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.Receives)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.SaleInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceDetails).ThenInclude(p => p.SaleInvoiceDetail.Product.ProductType)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceDetails).ThenInclude(p => p.SaleInvoiceDetail.Product.Producer)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceDetails).ThenInclude(p => p.SaleInvoiceDetail.Currency)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceDetails).ThenInclude(p => p.SaleInvoiceDetail.PurchaseInvoiceDetail)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceDetails).ThenInclude(p => p.SaleInvoiceDetail.TransferDetail)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceReceives)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == customerId)
                .Select(p => new Customer
                {
                    User = new User
                    {
                        Name = p.User.Name
                    },
                    SaleInvoices = !purchase.GetValueOrDefault(true) ? null : (ICollection<SaleInvoice>)p.SaleInvoices.Where(x => (paid == null || (paid.Value ? x.SaleInvoiceReceives.Any() : !x.SaleInvoiceReceives.Any())) && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate && !string.IsNullOrWhiteSpace(x.TotalPrice) && x.TotalPrice.Contains(currencyName)).Select(x => new SaleInvoice
                    {
                        Guid = x.Guid,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNum = x.InvoiceNum,
                        TotalPrice = x.TotalPrice,
                        Description = x.Description,
                        SaleInvoiceDiscounts = (ICollection<SaleInvoiceDiscount>)x.SaleInvoiceDiscounts.Where(w => currencyId == null || w.CurrencyId == currencyId).Select(s => new SaleInvoiceDiscount
                        {
                            Amount = s.Amount,
                            CurrencyName = s.Currency.Name
                        }),
                        SaleInvoiceReceives = (ICollection<SaleInvoiceReceive>)x.SaleInvoiceReceives.Select(s => new SaleInvoiceReceive
                        {
                            FullPay = s.FullPay
                        }),
                        SaleInvoiceDetails = (ICollection<SaleInvoiceDetail>)x.SaleInvoiceDetails.Where(w => currencyId == null || w.CurrencyId == currencyId).Select(s => new SaleInvoiceDetail
                        {
                            CurrencyName = s.Currency.Name,
                            Num = s.Num,
                            FreeNum = s.FreeNum,
                            SalePrice = s.SalePrice,
                            Discount = s.Discount,
                            PurchaseInvoiceDetail = new PurchaseInvoiceDetail
                            {
                                ExpireDate = s.PurchaseInvoiceDetail.ExpireDate,
                            },
                            TransferDetail = new TransferDetail
                            {
                                ExpireDate = s.TransferDetail.ExpireDate,
                            },
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
                    ReturnSaleInvoices = purchase.GetValueOrDefault(false) ? null : (ICollection<ReturnSaleInvoice>)p.ReturnSaleInvoices.Where(x => (paid == null || (paid.Value ? x.ReturnSaleInvoiceReceives.Any() : !x.ReturnSaleInvoiceReceives.Any())) && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate && !string.IsNullOrWhiteSpace(x.TotalPrice) && x.TotalPrice.Contains(currencyName)).Select(x => new ReturnSaleInvoice
                    {
                        Guid = x.Guid,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNum = x.InvoiceNum,
                        TotalPrice = x.TotalPrice,
                        Description = x.Description,
                        ReturnSaleInvoiceDiscounts = (ICollection<ReturnSaleInvoiceDiscount>)x.ReturnSaleInvoiceDiscounts.Where(w => currencyId == null || w.CurrencyId == currencyId).Select(s => new ReturnSaleInvoiceDiscount
                        {
                            Amount = s.Amount,
                            CurrencyName = s.Currency.Name
                        }),
                        ReturnSaleInvoiceReceives = (ICollection<ReturnSaleInvoiceReceive>)x.ReturnSaleInvoiceReceives.Select(s => new ReturnSaleInvoiceReceive
                        {
                            FullPay = s.FullPay
                        }),
                        ReturnSaleInvoiceDetails = (ICollection<ReturnSaleInvoiceDetail>)x.ReturnSaleInvoiceDetails.Where(w => currencyId == null || w.CurrencyId == currencyId).Select(s => new ReturnSaleInvoiceDetail
                        {
                            CurrencyName = s.Currency.Name,
                            Num = s.Num,
                            FreeNum = s.FreeNum,
                            Price = s.Price,
                            Discount = s.Discount,
                            SaleInvoiceDetail = new SaleInvoiceDetail
                            {
                                Product = new Product
                                {
                                    Name = s.SaleInvoiceDetail.Product.Name,
                                    ProductType = new BaseInfo
                                    {
                                        Name = s.SaleInvoiceDetail.Product.ProductType.Name
                                    },
                                    Producer = new BaseInfo
                                    {
                                        Name = s.SaleInvoiceDetail.Product.Producer.Name
                                    }
                                },
                                PurchaseInvoiceDetail = new PurchaseInvoiceDetail
                                {
                                    ExpireDate = s.SaleInvoiceDetail.PurchaseInvoiceDetail.ExpireDate,
                                },
                                TransferDetail = new TransferDetail
                                {
                                    ExpireDate = s.SaleInvoiceDetail.TransferDetail.ExpireDate,
                                }
                            }

                        })
                    })
                })
                .SingleOrDefault();
        }

        public Customer GetCustomerAccountReport(Guid customerId, bool? paid, bool? purchase, string currencyName, DateTime fromDate, DateTime toDate)
        {
            return _context.Customers.AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.SaleInvoiceReceives)
                .Include(p => p.SaleInvoices).ThenInclude(p => p.SaleInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceReceives)
                .Include(p => p.ReturnSaleInvoices).ThenInclude(p => p.ReturnSaleInvoiceDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == customerId)
                .Select(p => new Customer
                {
                    User = new User
                    {
                        Name = p.User.Name
                    },
                    SaleInvoices = !purchase.GetValueOrDefault(true) ? null : (ICollection<SaleInvoice>)p.SaleInvoices.Where(x => (paid == null || (paid.Value ? x.SaleInvoiceReceives.Any() : !x.SaleInvoiceReceives.Any())) && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate && !string.IsNullOrWhiteSpace(x.TotalPrice) && x.TotalPrice.Contains(currencyName)).Select(x => new SaleInvoice
                    {
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNum = x.InvoiceNum,
                        TotalPrice = x.TotalPrice,
                        Description = x.Description,
                        SaleInvoiceDiscounts = (ICollection<SaleInvoiceDiscount>)x.SaleInvoiceDiscounts.Select(s => new SaleInvoiceDiscount
                        {
                            Amount = s.Amount,
                            CurrencyName = s.Currency.Name

                        }),
                        SaleInvoiceReceives = (ICollection<SaleInvoiceReceive>)x.SaleInvoiceReceives.Select(s => new SaleInvoiceReceive
                        {
                            FullPay = s.FullPay
                        })
                    }),
                    ReturnSaleInvoices = purchase.GetValueOrDefault(false) ? null : (ICollection<ReturnSaleInvoice>)p.ReturnSaleInvoices.Where(x => (paid == null || (paid.Value ? x.ReturnSaleInvoiceReceives.Any() : !x.ReturnSaleInvoiceReceives.Any())) && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate && !string.IsNullOrWhiteSpace(x.TotalPrice) && x.TotalPrice.Contains(currencyName)).Select(x => new ReturnSaleInvoice
                    {
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNum = x.InvoiceNum,
                        TotalPrice = x.TotalPrice,
                        Description = x.Description,
                        ReturnSaleInvoiceDiscounts = (ICollection<ReturnSaleInvoiceDiscount>)x.ReturnSaleInvoiceDiscounts.Select(s => new ReturnSaleInvoiceDiscount
                        {
                            Amount = s.Amount,
                            CurrencyName = s.Currency.Name
                        }),
                        ReturnSaleInvoiceReceives = (ICollection<ReturnSaleInvoiceReceive>)x.ReturnSaleInvoiceReceives.Select(s => new ReturnSaleInvoiceReceive
                        {
                            FullPay = s.FullPay
                        })
                    })
                })
                .SingleOrDefault();
        }

    }
}
