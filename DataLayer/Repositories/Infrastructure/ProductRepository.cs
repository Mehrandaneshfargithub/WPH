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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ProductRepository(WASContext context)
            : base(context)
        {
        }

        public Product GetWithProducerAndProduct(Guid MasterProductId, Guid productId)
        {

            if (MasterProductId != Guid.Empty)
            {
                var result = Context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .Where(p => p.ProductMasterId == MasterProductId);

                Guid[] ids = result.Select(a => a.ClinicSectionId.GetValueOrDefault()).ToArray();

                var product = result.FirstOrDefault();

                product.ClinicSectionIds = ids;

                return product;
            }
            else
            {
                var result = Context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .FirstOrDefault(p => p.Guid == productId);

                return result;
            }

        }

        public Product GetWithChild(Guid productId)
        {
            return Context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .Include(p => p.Unit)
                .SingleOrDefault(p => p.Guid == productId);
        }

        public Product GetProductName(Guid productId, Expression<Func<Product, bool>> predicate = null)
        {
            IQueryable<Product> result = Context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .Where(x => x.Guid == productId);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(p => new Product
            {
                Guid = p.Guid,
                Name = p.Name,
                Producer = new BaseInfo
                {
                    Name = p.Producer.Name
                },
                ProductType = new BaseInfo
                {
                    Name = p.ProductType.Name
                }
            }).SingleOrDefault();
        }

        public IEnumerable<Product> GetProductsWithBarcode(Guid clinicSectionId, int count, Expression<Func<Product, bool>> predicate = null)
        {
            IQueryable<Product> result = Context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .Include(p => p.ProductBarcodes)
                .Where(x => x.ClinicSectionId == clinicSectionId);

            if (predicate != null)
                result = result.Where(predicate);

            result.Select(p => new Product
            {
                Guid = p.Guid,
                Name = p.Name,
                Producer = new BaseInfo
                {
                    Name = p.Producer.Name
                },
                ProductType = new BaseInfo
                {
                    Name = p.ProductType.Name
                },
                ProductBarcodes = (ICollection<ProductBarcode>)p.ProductBarcodes.Select(x => new ProductBarcode
                {
                    Barcode = x.Barcode
                })
            });

            if (count > 0)
                return result.Take(count);

            return result;
        }

        public IEnumerable<Product> GetAllProductsName(Guid clinicSectionId, int count, Expression<Func<Product, bool>> predicate = null)
        {
            IQueryable<Product> result = Context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .Where(x => x.ClinicSectionId == clinicSectionId);

            if (predicate != null)
                result = result.Where(predicate);

            result.Select(p => new Product
            {
                Guid = p.Guid,
                Name = p.Name,
                Producer = new BaseInfo
                {
                    Name = p.Producer.Name
                },
                ProductType = new BaseInfo
                {
                    Name = p.ProductType.Name
                }
            });

            if (count > 0)
                return result.Take(count);

            return result;
        }

        public Product GetProductCount(Guid productId, Guid clinicSectionId/*, Guid originalClinicSectionId*/)
        {
            return _context.Products.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Master)
                //.Include(p => p.TransferDetailProducts).ThenInclude(p => p.Master)
                .Include(p => p.TransferDetailDestinationProducts).ThenInclude(p => p.Master)
                .Where(p => p.Guid == productId /*&& p.ClinicSectionId == originalClinicSectionId*/)
                .Select(p => new Product
                {
                    Guid = p.Guid,
                    PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Where(x => x.Master.ClinicSectionId == clinicSectionId && x.RemainingNum > 0).Select(s => new PurchaseInvoiceDetail
                    {
                        Guid = s.Guid,
                        ExpireDate = s.ExpireDate,
                        SellingPrice = s.SellingPrice,
                        PurchasePrice = s.PurchasePrice,
                        Num = s.Num,
                        FreeNum = s.FreeNum,
                        RemainingNum = s.RemainingNum
                    }),
                    //TransferDetailProducts = (ICollection<TransferDetail>)p.TransferDetailProducts.Where(x => x.Master.SourceClinicSectionId == clinicSectionId).Select(s => new TransferDetail
                    //{
                    //    Guid = s.Guid,
                    //    Num = s.Num,
                    //    ExpireDate = s.ExpireDate,
                    //    PurchaseInvoiceDetailId = s.PurchaseInvoiceDetailId,
                    //    SellingPrice = s.SellingPrice,
                    //    PurchasePrice = s.PurchasePrice,
                    //    SourceTransferDetailId = s.SourceTransferDetailId,
                    //    //Master = new Transfer
                    //    //{
                    //    //    SourceClinicSectionId = s.Master.SourceClinicSectionId,
                    //    //    DestinationClinicSectionId = s.Master.DestinationClinicSectionId
                    //    //}
                    //}),
                    TransferDetailDestinationProducts = (ICollection<TransferDetail>)p.TransferDetailDestinationProducts.Where(x => x.Master.DestinationClinicSectionId == clinicSectionId && x.RemainingNum > 0).Select(s => new TransferDetail
                    {
                        Guid = s.Guid,
                        Num = s.Num,
                        ExpireDate = s.ExpireDate,
                        PurchaseInvoiceDetailId = s.PurchaseInvoiceDetailId,
                        SellingPrice = s.SellingPrice,
                        PurchasePrice = s.PurchasePrice,
                        TransferDetailId = s.TransferDetailId,
                        RemainingNum = s.RemainingNum,
                        SourcePurchaseInvoiceDetailId = s.SourcePurchaseInvoiceDetailId
                    })
                }).SingleOrDefault();
        }


        public Product GetProductHistory(Guid productId, Guid clinicSectionId, Guid originalClinicSectionId)
        {
            return _context.Products.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Master)
                .Include(p => p.TransferDetailProducts).ThenInclude(p => p.Master).ThenInclude(p => p.SourceClinicSection)
                .Include(p => p.TransferDetailProducts).ThenInclude(p => p.Master).ThenInclude(p => p.DestinationClinicSection)
                .Include(p => p.TransferDetailDestinationProducts).ThenInclude(p => p.Master).ThenInclude(p => p.SourceClinicSection)
                .Include(p => p.TransferDetailDestinationProducts).ThenInclude(p => p.Master).ThenInclude(p => p.DestinationClinicSection)
                .Where(p => p.Guid == productId && p.ClinicSectionId == originalClinicSectionId)
                .Select(p => new Product
                {
                    Guid = p.Guid,
                    PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Where(x => x.Master.ClinicSectionId == clinicSectionId).Select(s => new PurchaseInvoiceDetail
                    {
                        Guid = s.Guid,
                        Num = s.Num,
                        ExpireDate = s.ExpireDate,
                        SellingPrice = s.SellingPrice,
                        PurchasePrice = s.PurchasePrice,
                        FreeNum = s.FreeNum,
                        CreateDate = s.CreateDate
                    }),
                    TransferDetailProducts = (ICollection<TransferDetail>)p.TransferDetailProducts.Where(x => x.Master.SourceClinicSectionId == clinicSectionId).Select(s => new TransferDetail
                    {
                        Guid = s.Guid,
                        Num = s.Num,
                        ExpireDate = s.ExpireDate,
                        SellingPrice = s.SellingPrice,
                        PurchasePrice = s.PurchasePrice,
                        CreatedDate = s.CreatedDate,
                        Master = new Transfer
                        {
                            SourceClinicSection = new ClinicSection
                            {
                                Name = s.Master.SourceClinicSection.Name
                            },
                            DestinationClinicSection = new ClinicSection
                            {
                                Name = s.Master.DestinationClinicSection.Name
                            }
                        }
                    }),
                    TransferDetailDestinationProducts = (ICollection<TransferDetail>)p.TransferDetailDestinationProducts.Where(x => x.Master.DestinationClinicSectionId == clinicSectionId).Select(s => new TransferDetail
                    {
                        Guid = s.Guid,
                        Num = s.Num,
                        ExpireDate = s.ExpireDate,
                        SellingPrice = s.SellingPrice,
                        PurchasePrice = s.PurchasePrice,
                        CreatedDate = s.CreatedDate,
                        Master = new Transfer
                        {
                            SourceClinicSection = new ClinicSection
                            {
                                Name = s.Master.SourceClinicSection.Name
                            },
                            DestinationClinicSection = new ClinicSection
                            {
                                Name = s.Master.DestinationClinicSection.Name
                            }
                        }
                    })
                }).SingleOrDefault();
        }

        public IEnumerable<Product> GetAllMaterialProducts(Guid originalClinicSectionId, Guid clinicSectionId)
        {
            return _context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Master)
                //.Include(p => p.TransferDetailProducts).ThenInclude(p => p.Master)
                .Include(p => p.TransferDetailDestinationProducts).ThenInclude(p => p.Master)
                .Include(p => p.MaterialType).ThenInclude(p => p.Type)
                .Where(p => p.ClinicSectionId == originalClinicSectionId && p.MaterialType.Name == "Material" && p.MaterialType.Type.Ename == "MaterialType")
                .Select(p => new Product
                {
                    Guid = p.Guid,
                    Name = p.Name,
                    Barcode = p.Barcode,
                    Producer = new BaseInfo
                    {
                        Name = p.Producer.Name
                    },
                    ProductType = new BaseInfo
                    {
                        Name = p.ProductType.Name,
                    },
                    PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Where(x => x.Master.ClinicSectionId == clinicSectionId).Select(s => new PurchaseInvoiceDetail
                    {
                        //Num = s.Num,
                        //FreeNum = s.FreeNum
                        RemainingNum = s.RemainingNum,
                    }),
                    //TransferDetailProducts = (ICollection<TransferDetail>)p.TransferDetailProducts.Where(x => x.Master.SourceClinicSectionId == clinicSectionId).Select(s => new TransferDetail
                    //{
                    //    Num = s.Num,
                    //}),
                    TransferDetailDestinationProducts = (ICollection<TransferDetail>)p.TransferDetailDestinationProducts.Where(x => x.Master.DestinationClinicSectionId == clinicSectionId)
                    .Select(s => new TransferDetail
                    {
                        //Num = s.Num,
                        RemainingNum = s.RemainingNum,
                    })
                });
        }

        public IEnumerable<Product> GetAllStoreroomProducts(Guid originalClinicSectionId, Guid clinicSectionId, string productBarcode, int? FromOrderPoint, int? toOrderPoint, Guid? supplierId)
        {
            IQueryable<Product> result = _context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Master)
                .Include(p => p.TransferDetailDestinationProducts).ThenInclude(p => p.Master)
                .Include(p => p.MaterialType).ThenInclude(p => p.Type)
                .Where(p => p.ClinicSectionId == originalClinicSectionId && p.MaterialType.Name == "Medicine" && p.MaterialType.Type.Ename == "MaterialType");

            if (!string.IsNullOrWhiteSpace(productBarcode))
                result = result.Where(p => p.ProductBarcodes.Select(x => x.Barcode).Contains(productBarcode));

            if (FromOrderPoint != null && toOrderPoint != null)
                result = result.Where(p => p.OrderPoint != null && p.OrderPoint >= FromOrderPoint.Value && p.OrderPoint <= toOrderPoint.Value);

            return result.Select(p => new Product
            {
                Guid = p.Guid,
                ProductMasterId = p.ProductMasterId,
                Name = p.Name,
                ScientificName = p.ScientificName,
                OrderPoint = p.OrderPoint,
                ProductLocation = p.ProductLocation,
                Description = p.Description,
                Producer = new BaseInfo
                {
                    Name = p.Producer.Name
                },
                ProductType = new BaseInfo
                {
                    Name = p.ProductType.Name,
                },
                PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Where(x => (supplierId == null || x.Master.SupplierId == supplierId) && x.Master.ClinicSectionId == clinicSectionId).Select(s => new PurchaseInvoiceDetail
                {
                    RemainingNum = s.RemainingNum,
                }),
                TransferDetailDestinationProducts = (ICollection<TransferDetail>)p.TransferDetailDestinationProducts.Where(x => (supplierId == null || x.SourcePurchaseInvoiceDetail.Master.SupplierId == supplierId) && x.Master.DestinationClinicSectionId == clinicSectionId)
                .Select(s => new TransferDetail
                {
                    RemainingNum = s.RemainingNum,
                })
            });
        }

        public IEnumerable<Product> GetAllClinicSectionsStoreroomProducts(Guid originalClinicSectionId, Guid clinicSectionId, string productBarcode, int? FromOrderPoint, int? toOrderPoint, Guid? supplierId)
        {
            IQueryable<Product> result = _context.Products.AsNoTracking()
                .Include(p => p.ProductType)
                .Include(p => p.Producer)
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Master)
                .Include(p => p.TransferDetailDestinationProducts).ThenInclude(p => p.Master)
                .Include(p => p.MaterialType).ThenInclude(p => p.Type)
                .Where(p => /*p.ClinicSectionId == originalClinicSectionId && */p.MaterialType.Name == "Medicine" && p.MaterialType.Type.Ename == "MaterialType");

            if (!string.IsNullOrWhiteSpace(productBarcode))
                result = result.Where(p => p.ProductBarcodes.Select(x => x.Barcode).Contains(productBarcode));

            if (FromOrderPoint != null && toOrderPoint != null)
                result = result.Where(p => p.OrderPoint != null && p.OrderPoint >= FromOrderPoint.Value && p.OrderPoint <= toOrderPoint.Value);

            return result.Select(p => new Product
            {
                Guid = p.Guid,
                Name = p.Name,
                ProductMasterId = p.ProductMasterId,
                ScientificName = p.ScientificName,
                OrderPoint = p.OrderPoint,
                ProductLocation = p.ProductLocation,
                Description = p.Description,
                Producer = new BaseInfo
                {
                    Name = p.Producer.Name
                },
                ProductType = new BaseInfo
                {
                    Name = p.ProductType.Name,
                },
                PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Where(x => supplierId == null || x.Master.SupplierId == supplierId /*x.Master.ClinicSectionId == clinicSectionId*/).Select(s => new PurchaseInvoiceDetail
                {
                    RemainingNum = s.RemainingNum,
                }),
                TransferDetailDestinationProducts = (ICollection<TransferDetail>)p.TransferDetailDestinationProducts.Where(x => supplierId == null || x.SourcePurchaseInvoiceDetail.Master.SupplierId == supplierId /*x.Master.DestinationClinicSectionId == clinicSectionId*/)
                .Select(s => new TransferDetail
                {
                    RemainingNum = s.RemainingNum,
                })
            });
        }


        public Product GetProductByNameAndProducerAndType(Guid clinicSectionId, string productName, string producerName, int? materialTypeId, string productType)
        {
            return _context.Products.AsNoTracking()
                .Include(p => p.Producer)
                .Include(p => p.ProductType)
                .Where(p => p.ClinicSectionId == clinicSectionId && p.MaterialTypeId == materialTypeId && p.Name == productName && p.Producer.Name == producerName && p.ProductType.Name == productType)
                .Select(p => new Product
                {
                    Guid = p.Guid,
                    Name = p.Name,
                    MaterialTypeId = materialTypeId,
                    ProductTypeId = p.ProductTypeId,
                    ProducerId = p.ProducerId,
                    Barcode = p.Barcode,
                    Description = p.Description,
                    Code = p.Code,
                    ScientificName = p.ScientificName,
                    UnitId = p.UnitId
                }).SingleOrDefault();
        }

        public IEnumerable<Product> GetAllProductByMaterialTypeJustName(Guid clinicSectionId, string materialType)
        {

            IQueryable<Product> allP = Context.Products.AsNoTracking()
                 .Include(p => p.MaterialType)
                 .Include(p => p.Producer)
                 .Include(p => p.ProductType)
                 .Where(p => p.ClinicSectionId == clinicSectionId && (materialType == "" || p.MaterialType.Name.ToLower() == materialType))
                 .Select(p => new Product
                 {
                     Guid = p.Guid,
                     Id = p.Id,
                     Name = p.Name,
                     Barcode = p.Barcode,
                     Producer = new BaseInfo
                     {
                         Name = p.Producer.Name
                     },
                     ProductType = new BaseInfo
                     {
                         Name = p.ProductType.Name,
                     },

                 });

            return allP;
        }

        public bool CheckRepeatedProductBarcode(string productName, string barcode)
        {
            return _context.Products.AsNoTracking()
                .Where(p => p.Name.Trim() != productName.Trim() && p.Barcode.Trim() == barcode.Trim())
                .Any();
        }

        public IEnumerable<ProductWithBarcodeModel> GetAllProductsWithBarcode(Guid clinicSectionId)
        {
            try
            {
                //string qu = $"SELECT Product.GUID ProductId,Name ProductName,ProductBarcode.Barcode FROM dbo.Product INNER JOIN dbo.ProductBarcode ON ProductBarcode.ProductId = Product.GUID WHERE ClinicSectionId = '{clinicSectionId}'";
                string qu = $@"SELECT Product.GUID ProductId,Product.Name+' | '+productType.Name+' | '+producer.Name ProductName,ProductBarcode.Barcode FROM dbo.Product 
                                LEFT JOIN dbo.ProductBarcode ON ProductBarcode.ProductId = Product.GUID 
                                LEFT JOIN dbo.BaseInfo productType ON productType.GUID = Product.ProductTypeId 
                                LEFT JOIN dbo.BaseInfo producer ON producer.GUID = Product.ProducerId
                                LEFT JOIN dbo.BaseInfoGeneral material ON material.Id = Product.MaterialTypeId
                                WHERE Product.ClinicSectionId ='{clinicSectionId}' AND material.Name = 'Medicine'";
                return Context.Set<ProductWithBarcodeModel>().FromSqlRaw(qu);
            }
            catch (Exception e) { throw e; }
        }

        //public IEnumerable<PieChartModel> GetExpiredProducts(Guid clinicSectionId)
        //{
        //    string qury = @$"SELECT SUM(temp.Value) Value,temp.Label
        //                        FROM(
        //	(SELECT TOP 10  SUM(RemainingNum)  AS Value, dbo.Product.Name AS Label
        //	FROM dbo.PurchaseInvoiceDetails
        //	INNER JOIN dbo.PurchaseInvoice ON PurchaseInvoice.Guid = PurchaseInvoiceDetails.MasterId
        //	INNER JOIN dbo.Product ON Product.GUID = PurchaseInvoiceDetails.ProductId
        //	WHERE dbo.PurchaseInvoice.ClinicSectionId = '{clinicSectionId}' AND RemainingNum BETWEEN 1 AND 100
        //	GROUP BY dbo.Product.Name )
        //					UNION
        //                            (SELECT TOP 10  SUM(RemainingNum)  AS Value, dbo.Product.Name AS Label
        //	FROM dbo.TransferDetail
        //	INNER JOIN dbo.Transfer ON Transfer.Guid = TransferDetail.MasterId
        //	INNER JOIN dbo.Product ON Product.GUID = TransferDetail.ProductId
        //	WHERE dbo.Transfer.DestinationClinicSectionId = '{clinicSectionId}' AND RemainingNum BETWEEN 1 AND 100
        //	GROUP BY dbo.Product.Name )
        //					) AS temp GROUP BY temp.Label";

        //    return _context.Set<PieChartModel>().FromSqlRaw(qury);
        //}

        public IEnumerable<ExpiredProductModel> GetExpiredProducts(Guid clinicSectionId)
        {

            return _context.PurchaseInvoiceDetails.Include(a => a.Master).AsNoTracking()
                .Where(p => p.Master.ClinicSectionId == clinicSectionId && p.ExpireDate < DateTime.Now.AddYears(1)).Select(a => new ExpiredProductModel { ExpireDate = a.ExpireDate, ProductId = a.ProductId })
                .Union(_context.TransferDetails.Include(a => a.Master).AsNoTracking()
                .Where(p => p.Master.DestinationClinicSectionId == clinicSectionId && p.ExpireDate < DateTime.Now.AddYears(1)).Select(a => new ExpiredProductModel { ExpireDate = a.ExpireDate, ProductId = a.ProductId }));

        }

        public Product GetProductDetailById(Guid productId, Guid clinicSectionId)
        {
            return _context.Products.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Master.Supplier.User)
                .Include(p => p.PurchaseInvoiceDetails).ThenInclude(p => p.Currency)
                .Include(p => p.TransferDetailDestinationProducts).ThenInclude(p => p.SourcePurchaseInvoiceDetail).ThenInclude(p => p.Master.Supplier.User)
                .Include(p => p.TransferDetailDestinationProducts).ThenInclude(p => p.PurchaseCurrency)
                .Where(p => p.Guid == productId)
                .Select(p => new Product
                {
                    Name = p.Name,
                    PurchaseInvoiceDetails = (ICollection<PurchaseInvoiceDetail>)p.PurchaseInvoiceDetails.Where(x => x.Master.ClinicSectionId == clinicSectionId).Select(x => new PurchaseInvoiceDetail
                    {
                        ExpireDate = x.ExpireDate,
                        BujNumber = x.BujNumber,
                        RemainingNum = x.RemainingNum,
                        PurchasePrice = x.PurchasePrice,
                        SellingPrice = x.SellingPrice,
                        MiddleSellPrice = x.MiddleSellPrice,
                        WholeSellPrice = x.WholeSellPrice,
                        CurrencyName = x.Currency.Name,
                        Consideration = x.Consideration,
                        Master = new PurchaseInvoice
                        {
                            Supplier = new Supplier
                            {
                                User = new User
                                {
                                    Name = x.Master.Supplier.User.Name
                                }
                            }
                        }
                    }),
                    TransferDetailDestinationProducts = (ICollection<TransferDetail>)p.TransferDetailDestinationProducts.Where(x => x.Master.DestinationClinicSectionId == clinicSectionId).Select(x => new TransferDetail
                    {
                        RemainingNum = x.RemainingNum,
                        PurchasePrice = x.PurchasePrice,
                        SellingPrice = x.SellingPrice,
                        MiddleSellPrice = x.MiddleSellPrice,
                        WholeSellPrice = x.WholeSellPrice,
                        CurrencyName = x.PurchaseCurrency.Name,
                        ExpireDate = x.ExpireDate,
                        Consideration = x.Consideration,
                        SourcePurchaseInvoiceDetail = new PurchaseInvoiceDetail
                        {
                            BujNumber = x.SourcePurchaseInvoiceDetail.BujNumber,
                            Master = new PurchaseInvoice
                            {
                                Supplier = new Supplier
                                {
                                    User = new User
                                    {
                                        Name = x.SourcePurchaseInvoiceDetail.Master.Supplier.User.Name
                                    }
                                }
                            }
                        }
                    })
                })
                .SingleOrDefault();
        }


        public IEnumerable<ProductCardexReportModel> GetProductCardexReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            string pur_currency = "", rpur_currency = "", sal_currency = "", rsal_currency = "", dd_currency = "", tr_currency = "", rtr_currency = "";
            if (currencyId != null)
            {
                pur_currency = $" AND pur.CurrencyId={currencyId} ";
                rpur_currency = $" AND rpur.CurrencyId={currencyId} ";
                sal_currency = $" AND sal.CurrencyId={currencyId} ";
                rsal_currency = $" AND rsal.CurrencyId={currencyId} ";
                dd_currency = $" AND dd.CurrencyId={currencyId} ";
                tr_currency = $" AND tr.CurrencyId={currencyId} ";
                rtr_currency = $" AND rtr.CurrencyId={currencyId} ";
            }

            try
            {
                string qu = $@"SELECT 'Purchase' type,pur.CreateDate,masster.InvoiceDate,us.Name name,masster.InvoiceNum,pur.Num InNum,pur.FreeNum InFree,pur.PurchasePrice,0.0 OutNum,0.0 OutFreeNum,0.0 SalePrice,pur.ExpireDate,pur.BujNumber,currency.Name CurrencyName,pur.RemainingNum FROM dbo.PurchaseInvoiceDetails pur
                                LEFT JOIN dbo.PurchaseInvoice masster ON masster.Guid = pur.MasterId
                                LEFT JOIN dbo.[User] us ON us.GUID = masster.SupplierId
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = pur.CurrencyId
                                WHERE pur.ProductId='{productId}' AND masster.ClinicSectionId='{clinicSectionId}' AND masster.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND masster.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {pur_currency} 
                                
                                UNION
                                SELECT 'ReturnPurchase' type,rpur.CreateDate,masster.InvoiceDate,us.Name name,masster.InvoiceNum,0.0 InNum,0.0 InFree,0.0 PurchasePrice,rpur.Num OutNum,rpur.FreeNum OutFreeNum,rpur.Price SalePrice,pr.ExpireDate,pr.BujNumber,currency.Name CurrencyName,0.0 RemainingNum FROM dbo.ReturnPurchaseInvoiceDetail rpur
                                LEFT JOIN dbo.ReturnPurchaseInvoice masster ON masster.Guid = rpur.MasterId
                                LEFT JOIN dbo.PurchaseInvoiceDetails pr ON pr.Guid = rpur.PurchaseInvoiceDetailId
                                LEFT JOIN dbo.[User] us ON us.GUID = masster.SupplierId
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = rpur.CurrencyId
                                WHERE pr.ProductId='{productId}' AND masster.ClinicSectionId='{clinicSectionId}' AND masster.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND masster.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {rpur_currency}
                                
                                UNION
                                SELECT 'Sale' type,sal.CreatedDate CreateDate,masster.InvoiceDate,us.Name name,masster.InvoiceNum,0.0 InNum,0.0 InFree,0.0 PurchasePrice,sal.Num OutNum,sal.FreeNum OutFreeNum, sal.SalePrice,ISNULL(tr.ExpireDate,pur.ExpireDate) ExpireDate,ISNULL(maintr.BujNumber,pur.BujNumber) BujNumber,currency.Name CurrencyName,sal.RemainingNum FROM dbo.SaleInvoiceDetails sal
                                LEFT JOIN dbo.SaleInvoice masster ON masster.GUID = sal.SaleInvoiceId
                                LEFT JOIN dbo.[User] us ON us.GUID = masster.CustomerId
                                LEFT JOIN dbo.PurchaseInvoiceDetails pur ON pur.Guid = sal.PurchaseInvoiceDetailId
                                LEFT JOIN dbo.TransferDetail tr ON tr.Guid = sal.TransferDetailId
                                LEFT JOIN dbo.PurchaseInvoiceDetails maintr ON maintr.Guid = tr.SourcePurchaseInvoiceDetailId
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = sal.CurrencyId
                                WHERE sal.ProductId='{productId}' AND masster.ClinicSectionId='{clinicSectionId}' AND masster.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND masster.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {sal_currency}

                                UNION
                                SELECT 'ReturnSale' type,rsal.CreateDate,masster.InvoiceDate,us.Name name,masster.InvoiceNum,rsal.Num InNum,rsal.FreeNum InFree,rsal.Price PurchasePrice,0.0 OutNum,0.0 OutFreeNum,0.0 SalePrice,ISNULL(tr.ExpireDate,pur.ExpireDate) ExpireDate,ISNULL(maintr.BujNumber,pur.BujNumber) BujNumber,currency.Name CurrencyName,0.0 RemainingNum   FROM dbo.ReturnSaleInvoiceDetail rsal
                                LEFT JOIN dbo.ReturnSaleInvoice masster ON masster.Guid = rsal.MasterId
                                LEFT JOIN dbo.SaleInvoiceDetails sal ON sal.GUID = rsal.SaleInvoiceDetailId
                                LEFT JOIN dbo.[User] us ON us.GUID = masster.CustomerId
                                LEFT JOIN dbo.PurchaseInvoiceDetails pur ON pur.Guid = sal.PurchaseInvoiceDetailId
                                LEFT JOIN dbo.TransferDetail tr ON tr.Guid = sal.TransferDetailId
                                LEFT JOIN dbo.PurchaseInvoiceDetails maintr ON maintr.Guid = tr.SourcePurchaseInvoiceDetailId
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = rsal.CurrencyId
                                WHERE sal.ProductId='{productId}' AND masster.ClinicSectionId='{clinicSectionId}' AND masster.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND masster.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {rsal_currency}

                                UNION
                                SELECT 'Damage' type,dd.CreateDate,masster.InvoiceDate,res.Name name,masster.InvoiceNum,0.0 InNum,0.0 InFree,0.0 PurchasePrice,dd.Num OutNum,dd.FreeNum OutFreeNum,dd.Price SalePrice,ISNULL(tr.ExpireDate,pur.ExpireDate) ExpireDate,ISNULL(maintr.BujNumber,pur.BujNumber) BujNumber,currency.Name CurrencyName,0.0 RemainingNum  FROM dbo.DamageDetails dd
                                LEFT JOIN dbo.Damage masster ON masster.Guid = dd.MasterId
                                LEFT JOIN dbo.BaseInfo res ON res.GUID = masster.ReasonId
                                LEFT JOIN dbo.PurchaseInvoiceDetails pur ON pur.Guid = dd.PurchaseInvoiceDetailId
                                LEFT JOIN dbo.TransferDetail tr ON tr.Guid = dd.TransferDetailId
                                LEFT JOIN dbo.PurchaseInvoiceDetails maintr ON maintr.Guid = tr.SourcePurchaseInvoiceDetailId
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = dd.CurrencyId
                                WHERE dd.ProductId='{productId}' AND masster.ClinicSectionId='{clinicSectionId}' AND masster.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND masster.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {dd_currency}

                                UNION
                                SELECT 'Transfer' type,tr.CreatedDate CreateDate,masster.InvoiceDate,dest.Name name,CAST(masster.InvoiceNum AS NVARCHAR(10)) InvoiceNum,0.0 InNum,0.0 InFree,0.0 PurchasePrice,tr.Num OutNum,0.0 OutFreeNum,tr.PurchasePrice SalePrice,tr.ExpireDate,pur.BujNumber,currency.Name CurrencyName,tr.RemainingNum  FROM dbo.TransferDetail tr
                                LEFT JOIN dbo.Transfer masster ON masster.Guid = tr.MasterId
                                LEFT JOIN dbo.ClinicSection dest ON dest.GUID = masster.DestinationClinicSectionId
                                LEFT JOIN dbo.PurchaseInvoiceDetails pur ON pur.Guid = tr.SourcePurchaseInvoiceDetailId  
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = tr.CurrencyId
                                WHERE tr.ProductId='{productId}' AND masster.SourceClinicSectionId='{clinicSectionId}' AND masster.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND masster.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {tr_currency}

                                UNION
                                SELECT 'Receive' type,rtr.CreatedDate CreateDate,masster.InvoiceDate,sour.Name name,CAST(masster.InvoiceNum AS NVARCHAR(10)) InvoiceNum,rtr.Num InNum,0.0 InFree,rtr.PurchasePrice,0.0 OutNum,0.0 OutFreeNum,0.0 SalePrice,rtr.ExpireDate,pur.BujNumber,currency.Name CurrencyName,rtr.RemainingNum   FROM dbo.TransferDetail rtr
                                LEFT JOIN dbo.Transfer masster ON masster.Guid = rtr.MasterId
                                LEFT JOIN dbo.ClinicSection sour ON sour.GUID = masster.SourceClinicSectionId
                                LEFT JOIN dbo.PurchaseInvoiceDetails pur ON pur.Guid = rtr.SourcePurchaseInvoiceDetailId 
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = rtr.CurrencyId
                                WHERE rtr.DestinationProductId='{productId}' AND masster.DestinationClinicSectionId='{clinicSectionId}' AND masster.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND masster.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {rtr_currency} ";

                return Context.Set<ProductCardexReportModel>().FromSqlRaw(qu);
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<ProductPurchaseReportModel> GetProductPurchaseReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            string currency = "";
            if (currencyId != null)
                currency = $" AND pid.CurrencyId={currencyId} ";
            try
            {
                string qu = $@"SELECT pid.Guid,mastter.InvoiceNum,mastter.InvoiceDate,mastter.MainInvoiceNum,supplier.Name,pid.ExpireDate,pid.BujNumber,pid.Num,pid.FreeNum,pid.PurchasePrice,pid.Discount,pid.Consideration,currency.Name CurrencyName FROM dbo.PurchaseInvoiceDetails pid
                               LEFT JOIN dbo.PurchaseInvoice mastter ON mastter.Guid = pid.MasterId
                               LEFT JOIN dbo.[User] supplier ON supplier.GUID = mastter.SupplierId
                               LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = pid.CurrencyId
                               WHERE pid.ProductId='{productId}' AND mastter.ClinicSectionId='{clinicSectionId}' AND mastter.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND mastter.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {currency} ";

                return Context.Set<ProductPurchaseReportModel>().FromSqlRaw(qu);
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<ProductSaleReportModel> GetProductSaleReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId)
        {
            IQueryable<ProductSaleReportModel> res = _context.SaleInvoiceDetails
                .Include(a => a.Currency)
                .Include(a => a.PurchaseInvoiceDetail).ThenInclude(p => p.Currency)
                .Include(a => a.MoneyConvert).ThenInclude(p => p.BaseCurrency)
                .Include(a => a.MoneyConvert).ThenInclude(p => p.DestCurrency)
                .Include(a => a.SaleInvoice).ThenInclude(p => p.Customer).ThenInclude(p => p.User)
                .AsNoTracking()
                .Where(p => p.ProductId == productId && p.SaleInvoice.ClinicSectionId == clinicSectionId
                && p.SaleInvoice.InvoiceDate >= fromDate && p.SaleInvoice.InvoiceDate <= toDate)
                .Select(a => new ProductSaleReportModel
                {
                    BujNumber = a.BujNumber,
                    Consideration = a.Consideration,
                    Discount = a.Discount,
                    ExpireDate = a.PurchaseInvoiceDetail.ExpireDate.Value,
                    Num = a.Num,
                    FreeNum = a.FreeNum,
                    InvoiceDate = a.SaleInvoice.InvoiceDate.Value,
                    InvoiceNum = a.SaleInvoice.InvoiceNum,
                    Name = a.SaleInvoice.Customer.User.Name,
                    PurchasePrice = a.PurchaseInvoiceDetail.PurchasePrice,
                    PurchaseCurrencyName = a.PurchaseInvoiceDetail.Currency.Name,
                    SalePrice = a.SalePrice.Value,
                    SaleCurrencyName = a.Currency.Name,
                    MoneyConvert = a.MoneyConvert ?? null,
                    CurrencyId = a.CurrencyId.Value
                });

            if (currencyId != null)
            {
                return res.Where(a => a.CurrencyId == currencyId);
            }
            else
            {
                return res;
            }

        }

        public IEnumerable<ReturnProductPurchaseReportModel> GetProductReturnPurchaseReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            string currency = "";
            if (currencyId != null)
                currency = $" AND pid.CurrencyId={currencyId} ";
            try
            {
                string qu = $@"SELECT pid.Guid,mastter.InvoiceNum,mastter.InvoiceDate,purchase.ExpireDate,supplier.Name,pid.Num,pid.FreeNum,pid.Price PurchasePrice,pid.Discount,currency.Name CurrencyName,reason.Name reason FROM dbo.ReturnPurchaseInvoiceDetail pid
                               LEFT JOIN dbo.ReturnPurchaseInvoice mastter ON mastter.Guid = pid.MasterId
                               LEFT JOIN dbo.[User] supplier ON supplier.GUID = mastter.SupplierId
                               LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = pid.CurrencyId
                               LEFT JOIN dbo.BaseInfo reason ON reason.GUID = pid.ReasonId
                               LEFT JOIN dbo.PurchaseInvoiceDetails purchase ON purchase.Guid = pid.PurchaseInvoiceDetailId
                               LEFT JOIN dbo.TransferDetail trans ON trans.Guid = pid.TransferDetailId
                               WHERE (purchase.ProductId='{productId}' OR trans.DestinationProductId='{productId}') AND mastter.ClinicSectionId='{clinicSectionId}' AND mastter.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND mastter.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {currency} ";

                return Context.Set<ReturnProductPurchaseReportModel>().FromSqlRaw(qu);
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<ReturnProductPurchaseReportModel> GetProductReturnSaleReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            string currency = "";
            if (currencyId != null)
                currency = $" AND pid.CurrencyId={currencyId} ";
            try
            {
                string qu = $@"SELECT pid.Guid,mastter.InvoiceNum,mastter.InvoiceDate,ISNULL(tr.ExpireDate,pur.ExpireDate) ExpireDate,customer.Name,pid.Num,pid.FreeNum,pid.Price PurchasePrice,pid.Discount,currency.Name CurrencyName,reason.Name reason FROM dbo.ReturnSaleInvoiceDetail pid
                               LEFT JOIN dbo.ReturnSaleInvoice mastter ON mastter.Guid = pid.MasterId
							   LEFT JOIN dbo.SaleInvoiceDetails sal ON sal.GUID = pid.SaleInvoiceDetailId
                               LEFT JOIN dbo.[User] customer ON customer.GUID = mastter.CustomerId
                               LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = pid.CurrencyId
                               LEFT JOIN dbo.BaseInfo reason ON reason.GUID = pid.ReasonId
							   LEFT JOIN dbo.PurchaseInvoiceDetails pur ON pur.Guid = sal.PurchaseInvoiceDetailId
                                LEFT JOIN dbo.TransferDetail tr ON tr.Guid = sal.TransferDetailId
                               WHERE sal.ProductId='{productId}' AND mastter.ClinicSectionId='{clinicSectionId}' AND mastter.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND mastter.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {currency} ";

                return Context.Set<ReturnProductPurchaseReportModel>().FromSqlRaw(qu);
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<ProductTransferReportModel> GetProductTransferReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            string currency = "";
            if (currencyId != null)
                currency = $" AND trans.CurrencyId={currencyId} ";
            try
            {
                string qu = $@"SELECT trans.Guid,mastter.InvoiceDate,mastter.InvoiceNum,clinic.Name,trans.ExpireDate,trans.Num,trans.PurchasePrice,trans.Consideration,currency.Name CurrencyName FROM dbo.TransferDetail trans
                              LEFT JOIN dbo.Transfer mastter ON mastter.Guid = trans.MasterId
                              LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = trans.CurrencyId
                              LEFT JOIN dbo.ClinicSection clinic ON clinic.GUID = mastter.DestinationClinicSectionId
                              WHERE trans.ProductId='{productId}' AND mastter.SourceClinicSectionId='{clinicSectionId}' AND mastter.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND mastter.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {currency} ";

                return Context.Set<ProductTransferReportModel>().FromSqlRaw(qu);
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<ProductTransferReportModel> GetProductReceiveReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            string currency = "";
            if (currencyId != null)
                currency = $" AND trans.CurrencyId={currencyId} ";
            try
            {
                string qu = $@"SELECT trans.Guid,mastter.InvoiceDate,mastter.InvoiceNum,clinic.Name,trans.ExpireDate,trans.Num,trans.PurchasePrice,trans.Consideration,currency.Name CurrencyName FROM dbo.TransferDetail trans
                               LEFT JOIN dbo.Transfer mastter ON mastter.Guid = trans.MasterId
                               LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = trans.CurrencyId
                               LEFT JOIN dbo.ClinicSection clinic ON clinic.GUID = mastter.SourceClinicSectionId
                               WHERE trans.DestinationProductId='{productId}' AND mastter.DestinationClinicSectionId='{clinicSectionId}' AND mastter.InvoiceDate>='{fromDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' AND mastter.InvoiceDate<='{toDate.ToString("MM/dd/yyyy HH:mm:ss", cultures)}' {currency} ";

                return Context.Set<ProductTransferReportModel>().FromSqlRaw(qu);
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<Product> GetAllClinicSectionsMaterialProducts(Guid originalClinicSectionId, Guid clinicSectionId)
        {
            throw new NotImplementedException();
        }
    }
}
