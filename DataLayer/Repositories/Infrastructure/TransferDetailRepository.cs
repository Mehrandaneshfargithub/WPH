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
    public class TransferDetailRepository : Repository<TransferDetail>, ITransferDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public TransferDetailRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<TransferDetail> GetAllWithChildByMasterId(Guid transferId)
        {
            return Context.TransferDetails.AsNoTracking()
                .Include(p => p.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.Product).ThenInclude(P => P.Producer)
                .Include(p => p.DestinationProduct).ThenInclude(P => P.ProductType)
                .Include(p => p.DestinationProduct).ThenInclude(P => P.Producer)
                .Include(p => p.PurchaseCurrency)
                .Where(p => p.MasterId == transferId)
                .Select(p => new TransferDetail
                {
                    Guid = p.Guid,
                    ExpireDate = p.ExpireDate,
                    Consideration = p.Consideration,
                    PurchasePrice = p.PurchasePrice,
                    SellingPrice = p.SellingPrice,
                    MiddleSellPrice = p.MiddleSellPrice,
                    WholeSellPrice = p.WholeSellPrice,
                    Num = p.Num,
                    PurchaseCurrency = new BaseInfoGeneral
                    {
                        Name = p.PurchaseCurrency.Name
                    },
                    Product = new Product
                    {
                        Name = p.Product.Name,
                        ProductType = new BaseInfo
                        {
                            Name = p.Product.ProductType.Name
                        },
                        Producer = new BaseInfo
                        {
                            Name = p.Product.Producer.Name
                        }
                    },
                    DestinationProduct = new Product
                    {
                        Name = p.DestinationProduct.Name,
                        ProductType = new BaseInfo
                        {
                            Name = p.DestinationProduct.ProductType.Name
                        },
                        Producer = new BaseInfo
                        {
                            Name = p.DestinationProduct.Producer.Name
                        }
                    }
                })
                ;
        }

        public IEnumerable<TransferDetail> GetUnreceivedTransferDetailByMasterId(Guid transferId)
        {
            return Context.TransferDetails.AsNoTracking()
                .Include(p => p.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.Product).ThenInclude(P => P.Producer)
                .Where(p => p.MasterId == transferId && p.DestinationProductId == null)
                .Select(p => new TransferDetail
                {
                    Guid = p.Guid,
                    ExpireDate = p.ExpireDate,
                    Consideration = p.Consideration,
                    SellingPrice = p.SellingPrice,
                    PurchasePrice = p.PurchasePrice,
                    MiddleSellPrice = p.MiddleSellPrice,
                    WholeSellPrice = p.WholeSellPrice,
                    Num = p.Num,
                    PurchaseCurrency = new BaseInfoGeneral
                    {
                        Name = p.PurchaseCurrency.Name
                    },
                    Product = new Product
                    {
                        Name = p.Product.Name,
                        ProductType = new BaseInfo
                        {
                            Name = p.Product.ProductType.Name
                        },
                        Producer = new BaseInfo
                        {
                            Name = p.Product.Producer.Name
                        }
                    }
                })
                ;
        }

        public bool CheckTransferDetailInUse(Guid transferDetailId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Any(p => p.TransferDetailId == transferDetailId);
        }

        public TransferDetail GetWithMasterByDetailId(Guid transferDetailId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.Master)
                .SingleOrDefault(p => p.Guid == transferDetailId);
        }

        public decimal GetTransferSourceRemCount(Guid transferDetailId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.InverseSourceTransferDetail)
                .Where(p => p.Guid == transferDetailId)
                .Select(p => (p.Num ?? 0) - p.InverseSourceTransferDetail.Sum(s => (s.Num ?? 0)))
                .SingleOrDefault();
        }

        public IEnumerable<TransferDetail> GetTransferDetailReport(List<Guid> clinicSectionId, DateTime fromDate, DateTime toDate, Expression<Func<TransferDetail, bool>> predicate = null)
        {
            IQueryable<TransferDetail> result = _context.TransferDetails.AsNoTracking()
                 .Include(p => p.CreatedUser)
                 .Include(p => p.Master).ThenInclude(p => p.SourceClinicSection)
                 .Include(p => p.Master).ThenInclude(p => p.DestinationClinicSection)
                 .Include(p => p.Product)
                 .Include(p => p.DestinationProduct)
                 .Where(p => p.Master.InvoiceDate >= fromDate && p.Master.InvoiceDate <= toDate &&
                        (clinicSectionId.Contains(p.Master.DestinationClinicSectionId ?? Guid.Empty) || clinicSectionId.Contains(p.Master.DestinationClinicSectionId ?? Guid.Empty)))
                 ;

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(s => new TransferDetail
            {
                Num = s.Num,
                Master = new Transfer
                {
                    InvoiceDate = s.Master.InvoiceDate,
                    ReceiverName = s.Master.ReceiverName,
                    SourceClinicSection = new ClinicSection
                    {
                        Name = s.Master.SourceClinicSection.Name
                    },
                    DestinationClinicSection = new ClinicSection
                    {
                        Name = s.Master.DestinationClinicSection.Name
                    }
                },
                Product = new Product
                {
                    Name = s.Product.Name
                },
                DestinationProduct = new Product
                {
                    Name = s.DestinationProduct.Name
                },
                CreatedUser = new User
                {
                    Name = s.Master.CreatedUser.Name
                }
            });
        }

        public bool CheckConfirmAllProductRecive(Guid transferId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Any(p => p.MasterId == transferId && p.DestinationProductId == null);
        }

        public TransferDetail GetWithSourceProduct(Guid transferDetailId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.Product)
                .Where(p => p.Guid == transferDetailId)
                .Select(p => new TransferDetail
                {
                    Guid = p.Guid,
                    Product = new Product
                    {
                        Name = p.Product.Name
                    }
                })
                .SingleOrDefault();
        }

        public IEnumerable<TransferDetail> GetWithPurchaseInvoiceDetail(List<Guid> transfers)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.SourcePurchaseInvoiceDetail)
                .Where(p => transfers.Contains(p.Guid));
        }

        public IEnumerable<TransferDetail> GetWithPricesByMultipleIds(List<Guid> transfers)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetailSalePrices)
                .Where(p => transfers.Contains(p.Guid));
        }

        public TransferDetail GetWithPricesById(Guid transferDetailId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetailSalePrices)
                .SingleOrDefault(p => p.Guid == transferDetailId);
        }

        public TransferDetail GetWithSourcePurchaseInvoice(Guid detailId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.SourcePurchaseInvoiceDetail)
                .SingleOrDefault(p => p.Guid == detailId);
        }

        public void IncreaseUpdateWithLocal(TransferDetail detail, decimal remainingNum)
        {
            var track = _context.Set<TransferDetail>().Local.SingleOrDefault(e => e == detail);
            if (track == null)
            {
                detail.RemainingNum += remainingNum;
                UpdateState(detail);
            }
            else
            {
                track.RemainingNum += remainingNum;
            }
        }

        public TransferDetail GetTransferDetailForUpdate(Guid transferDetailId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.Product).ThenInclude(p => p.ProductType)
                .Include(p => p.Product).ThenInclude(p => p.Producer)
                .Select(p => new TransferDetail
                {
                    Guid = p.Guid,
                    MasterId = p.MasterId,
                    Num = p.Num,
                    Consideration = p.Consideration,
                    Product = new Product
                    {
                        Name = p.Product.Name,
                        ProductType = new BaseInfo
                        {
                            Name = p.Product.ProductType.Name
                        },
                        Producer = new BaseInfo
                        {
                            Name = p.Product.Producer.Name
                        }
                    }
                })
                .SingleOrDefault(p => p.Guid == transferDetailId);
        }


        public TransferDetail GetForNewSalePrice(Guid transferDetailId)
        {
            return Context.TransferDetails.AsNoTracking()
                .Include(p => p.DestinationProduct).ThenInclude(P => P.ProductType)
                .Include(p => p.DestinationProduct).ThenInclude(P => P.Producer)
                .Include(p => p.PurchaseCurrency)
                .Where(p => p.Guid == transferDetailId)
                .SingleOrDefault()
                ;
        }

        public TransferDetail GetForSalePrice(Guid transferDetailId)
        {
            return Context.TransferDetails.AsNoTracking()
                .Where(p => p.Guid == transferDetailId)
                .Select(p => new TransferDetail
                {
                    MasterId = p.MasterId,
                    CurrencyId = p.CurrencyId,
                })
                .SingleOrDefault()
                ;
        }

        public TransferDetail GetParentCurrency(Guid transferDetailId)
        {
            return Context.TransferDetails.AsNoTracking()
                .Include(p => p.PurchaseCurrency)
                .Where(p => p.Guid == transferDetailId)
                .Select(p => new TransferDetail
                {
                    MasterId = p.MasterId,
                    CurrencyId = p.CurrencyId,
                    SellingPrice = p.SellingPrice,
                    MiddleSellPrice = p.MiddleSellPrice,
                    WholeSellPrice = p.WholeSellPrice,
                    CurrencyName = p.PurchaseCurrency.Name
                })
                .SingleOrDefault()
                ;
        }

        public TransferDetail GetPricesByDetailId(Guid detailId)
        {
            return _context.TransferDetails.AsNoTracking()
                .Include(p => p.PurchaseCurrency)
                .Where(p => p.Guid == detailId)
                .Select(p => new TransferDetail
                {
                    SellingPrice = p.SellingPrice,
                    MiddleSellPrice = p.MiddleSellPrice,
                    WholeSellPrice = p.WholeSellPrice,
                    CurrencyName = p.PurchaseCurrency.Name
                })
                .SingleOrDefault();

        }
    }
}
