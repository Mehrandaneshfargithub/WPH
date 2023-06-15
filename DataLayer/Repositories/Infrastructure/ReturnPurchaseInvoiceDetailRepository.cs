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
    public class ReturnPurchaseInvoiceDetailRepository : Repository<ReturnPurchaseInvoiceDetail>, IReturnPurchaseInvoiceDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReturnPurchaseInvoiceDetailRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReturnPurchaseInvoiceDetail> GetAllReturnPurchaseInvoiceDetailByMasterId(Guid returnPurchaseInvoiceId)
        {
            return Context.ReturnPurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(P => P.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(P => P.Product).ThenInclude(P => P.Producer)
                .Include(p => p.Currency)
                .Include(p => p.Reason)
                .Where(p => p.MasterId == returnPurchaseInvoiceId)
                .Select(p => new ReturnPurchaseInvoiceDetail
                {
                    Guid = p.Guid,
                    Num = p.Num,
                    FreeNum = p.FreeNum,
                    Price = p.Price,
                    Discount = p.Discount,
                    Currency = new BaseInfoGeneral
                    {
                        Name = p.Currency.Name
                    },
                    Reason = new BaseInfo
                    {
                        Name = p.Reason.Name
                    },
                    PurchaseInvoiceDetail = new PurchaseInvoiceDetail
                    {
                        Product = new Product
                        {
                            Name = p.PurchaseInvoiceDetail.Product.Name,
                            Producer = new BaseInfo
                            {
                                Name = p.PurchaseInvoiceDetail.Product.Producer.Name
                            },
                            ProductType = new BaseInfo
                            {
                                Name = p.PurchaseInvoiceDetail.Product.ProductType.Name
                            }
                        }
                    }
                })
                ;
        }

        public IEnumerable<ReturnPurchaseInvoiceDetail> GetAllReturnPurchaseInvoiceDetailByIds(List<Guid> ids)
        {
            return Context.ReturnPurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(P => P.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(P => P.Product).ThenInclude(P => P.Producer)
                .Include(p => p.Currency)
                .Include(p => p.Reason)
                .Where(p => ids.Contains(p.Guid))
                .Select(p => new ReturnPurchaseInvoiceDetail
                {
                    Guid = p.Guid,
                    Num = p.Num,
                    FreeNum = p.FreeNum,
                    Price = p.Price,
                    Discount = p.Discount,
                    Currency = new BaseInfoGeneral
                    {
                        Name = p.Currency.Name
                    },
                    Reason = new BaseInfo
                    {
                        Name = p.Reason.Name
                    },
                    PurchaseInvoiceDetail = new PurchaseInvoiceDetail
                    {
                        Product = new Product
                        {
                            Name = p.PurchaseInvoiceDetail.Product.Name,
                            Producer = new BaseInfo
                            {
                                Name = p.PurchaseInvoiceDetail.Product.Producer.Name
                            },
                            ProductType = new BaseInfo
                            {
                                Name = p.PurchaseInvoiceDetail.Product.ProductType.Name
                            }
                        }
                    }
                })
                ;
        }

        public IEnumerable<ReturnPurchaseInvoiceDetail> GetAllTotalPrice(Guid returnPurchaseInvoiceId)
        {
            return Context.ReturnPurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.Currency)
                .Where(p => p.MasterId == returnPurchaseInvoiceId)
                .Select(p => new ReturnPurchaseInvoiceDetail
                {
                    Num = p.Num,
                    Price = p.Price,
                    Discount = p.Discount,
                    Currency = new BaseInfoGeneral
                    {
                        Name = p.Currency.Name
                    }
                })
                ;
        }

        //public decimal GetSourceRemCount(Guid purchaseInvoiceDetailId)
        //{
        //    return _context.ReturnPurchaseInvoiceDetails.AsNoTracking()
        //        .Include(p => p.TransferDetails)
        //        .Where(p => p.Guid == purchaseInvoiceDetailId)
        //        .Select(p => (p.Num ?? 0) + (p.FreeNum ?? 0) - p.TransferDetails.Where(x => x.TransferDetailId == null).Sum(s => (s.Num ?? 0)))
        //        .SingleOrDefault();
        //}

        public ReturnPurchaseInvoiceDetail GetReturnPurchaseInvoiceDetailForEdit(Guid returnPurchaseInvoiceDetailId)
        {
            return _context.ReturnPurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(p => p.Master)
                .Include(p => p.Reason)
                .SingleOrDefault(p => p.Guid == returnPurchaseInvoiceDetailId);
        }

        public bool CheckDetailsExistByMasterId(Guid? masterId)
        {
            return _context.ReturnPurchaseInvoiceDetails.AsNoTracking()
                .Where(p => p.MasterId == masterId)
                .Any();
        }

        public bool CheckDetailsExistByPurchaseInvoiceDetailIds(List<Guid?> purchaseInvoiceDetailIds)
        {
            return _context.ReturnPurchaseInvoiceDetails.AsNoTracking()
                .Where(p => purchaseInvoiceDetailIds.Contains(p.PurchaseInvoiceDetailId))
                .Any();
        }
    }
}
