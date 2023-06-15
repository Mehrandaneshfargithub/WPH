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
    public class ReturnSaleInvoiceDetailRepository : Repository<ReturnSaleInvoiceDetail>, IReturnSaleInvoiceDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReturnSaleInvoiceDetailRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ReturnSaleInvoiceDetail> GetAllReturnSaleInvoiceDetailByMasterId(Guid returnSaleInvoiceId)
        {
            return Context.ReturnSaleInvoiceDetails.AsNoTracking()
                .Include(p => p.SaleInvoiceDetail).ThenInclude(P => P.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.SaleInvoiceDetail).ThenInclude(P => P.Product).ThenInclude(P => P.Producer)
                .Include(p => p.Currency)
                .Include(p => p.Reason)
                .Where(p => p.MasterId == returnSaleInvoiceId)
                .Select(p => new ReturnSaleInvoiceDetail
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
                    SaleInvoiceDetail = new SaleInvoiceDetail
                    {
                        Product = new Product
                        {
                            Name = p.SaleInvoiceDetail.Product.Name,
                            Producer = new BaseInfo
                            {
                                Name = p.SaleInvoiceDetail.Product.Producer.Name
                            },
                            ProductType = new BaseInfo
                            {
                                Name = p.SaleInvoiceDetail.Product.ProductType.Name
                            }
                        }
                    }
                })
                ;
        }

        public IEnumerable<ReturnSaleInvoiceDetail> GetAllReturnSaleInvoiceDetailByIds(List<Guid> ids)
        {
            return Context.ReturnSaleInvoiceDetails.AsNoTracking()
                .Include(p => p.SaleInvoiceDetail).ThenInclude(P => P.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.SaleInvoiceDetail).ThenInclude(P => P.Product).ThenInclude(P => P.Producer)
                .Include(p => p.Currency)
                .Include(p => p.Reason)
                .Where(p => ids.Contains(p.Guid))
                .Select(p => new ReturnSaleInvoiceDetail
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
                    SaleInvoiceDetail = new SaleInvoiceDetail
                    {
                        Product = new Product
                        {
                            Name = p.SaleInvoiceDetail.Product.Name,
                            Producer = new BaseInfo
                            {
                                Name = p.SaleInvoiceDetail.Product.Producer.Name
                            },
                            ProductType = new BaseInfo
                            {
                                Name = p.SaleInvoiceDetail.Product.ProductType.Name
                            }
                        }
                    }
                })
                ;
        }

        public IEnumerable<ReturnSaleInvoiceDetail> GetAllTotalPrice(Guid returnSaleInvoiceId)
        {
            return Context.ReturnSaleInvoiceDetails.AsNoTracking()
                .Include(p => p.Currency)
                .Where(p => p.MasterId == returnSaleInvoiceId)
                .Select(p => new ReturnSaleInvoiceDetail
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

        public ReturnSaleInvoiceDetail GetWithPurchaseAndTransfer(Guid returnSaleInvoiceId)
        {
            return _context.ReturnSaleInvoiceDetails.AsNoTracking()
                .Include(p => p.SaleInvoiceDetail.PurchaseInvoiceDetail)
                .Include(p => p.SaleInvoiceDetail.TransferDetail)
                .SingleOrDefault(p => p.Guid == returnSaleInvoiceId);
        }

        //public decimal GetSourceRemCount(Guid saleInvoiceDetailId)
        //{
        //    return _context.ReturnSaleInvoiceDetails.AsNoTracking()
        //        .Include(p => p.TransferDetails)
        //        .Where(p => p.Guid == saleInvoiceDetailId)
        //        .Select(p => (p.Num ?? 0) + (p.FreeNum ?? 0) - p.TransferDetails.Where(x => x.TransferDetailId == null).Sum(s => (s.Num ?? 0)))
        //        .SingleOrDefault();
        //}

        public ReturnSaleInvoiceDetail GetReturnSaleInvoiceDetailForEdit(Guid returnSaleInvoiceDetailId)
        {
            return _context.ReturnSaleInvoiceDetails.AsNoTracking()
                .Include(p => p.SaleInvoiceDetail).ThenInclude(p => p.SaleInvoice)
                .Include(p => p.Reason)
                .SingleOrDefault(p => p.Guid == returnSaleInvoiceDetailId);
        }

        public bool CheckDetailsExistByMasterId(Guid? masterId)
        {
            return _context.ReturnSaleInvoiceDetails.AsNoTracking()
                .Where(p => p.MasterId == masterId)
                .Any();
        }

        public bool CheckDetailsExistBySaleInvoiceDetailIds(List<Guid?> saleInvoiceDetailIds)
        {
            return _context.ReturnSaleInvoiceDetails.AsNoTracking()
                .Where(p => saleInvoiceDetailIds.Contains(p.SaleInvoiceDetailId))
                .Any();
        }

        public ReturnSaleInvoiceDetail GetWithSaleInvoice(Guid returnSaleInvoiceDetailId)
        {
            return _context.ReturnSaleInvoiceDetails.AsNoTracking()
                .Include(p => p.SaleInvoiceDetail).ThenInclude(p => p.PurchaseInvoiceDetail)
                .Include(p => p.SaleInvoiceDetail).ThenInclude(p => p.TransferDetail)
                .SingleOrDefault(p => p.Guid == returnSaleInvoiceDetailId);
        }

        public IEnumerable<PieChartModel> GetMostReturnedProducts(Guid clinicSectionId)
        {
            string qury = @$"SELECT TOP 3  SUM(ReturnSaleInvoiceDetail.Num + ReturnSaleInvoiceDetail.FreeNum)  AS Value, dbo.Product.Name AS Label
                                FROM dbo.ReturnSaleInvoiceDetail 
                                INNER JOIN dbo.ReturnSaleInvoice ON ReturnSaleInvoice.Guid = ReturnSaleInvoiceDetail.MasterId
                                INNER JOIN dbo.SaleInvoiceDetails ON  SaleInvoiceDetails.GUID = ReturnSaleInvoiceDetail.SaleInvoiceDetailId
                                INNER JOIN dbo.Product ON  Product.GUID = SaleInvoiceDetails.ProductId
                                WHERE dbo.ReturnSaleInvoice.ClinicSectionId = '{clinicSectionId}'
                                GROUP BY dbo.Product.Name ORDER BY Value DESC";

            return _context.Set<PieChartModel>().FromSqlRaw(qury);
        }
    }
}
