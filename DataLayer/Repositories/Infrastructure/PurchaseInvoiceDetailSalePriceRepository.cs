using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Infrastructure
{
    public class PurchaseInvoiceDetailSalePriceRepository : Repository<PurchaseInvoiceDetailSalePrice>, IPurchaseInvoiceDetailSalePriceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PurchaseInvoiceDetailSalePriceRepository(WASContext context)
            : base(context)
        {
        }

        public PurchaseInvoiceDetailSalePrice GetForTransferEdit(Guid salePriceId)
        {
            return _context.PurchaseInvoiceDetailSalePrices.AsNoTracking()
                .Include(p => p.TransferDetail).ThenInclude(p => p.PurchaseCurrency)
                .Include(p => p.MoneyConvert)
                .Include(p => p.Currency)
                .SingleOrDefault(p => p.Guid == salePriceId);
        }

        public PurchaseInvoiceDetailSalePrice GetForEdit(Guid salePriceId)
        {
            return _context.PurchaseInvoiceDetailSalePrices.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(p => p.Currency)
                .Include(p => p.MoneyConvert)
                .Include(p => p.Currency)
                .SingleOrDefault(p => p.Guid == salePriceId);
        }

        public bool CheckTransferCurrencyExist(Guid? transferDetailId, int? currencyId, int? typeId, Expression<Func<PurchaseInvoiceDetailSalePrice, bool>> predicate = null)
        {
            IQueryable<PurchaseInvoiceDetailSalePrice> result = _context.PurchaseInvoiceDetailSalePrices.AsNoTracking()
                .Where(p => p.TransferDetailId == transferDetailId && p.CurrencyId == currencyId && p.TypeId == typeId);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }

        public bool CheckConflictCurrency(Guid? purchaseInvoiceDetailId, int? currencyId)
        {
            IQueryable<PurchaseInvoiceDetailSalePrice> result = _context.PurchaseInvoiceDetailSalePrices.AsNoTracking()
                .Where(p => p.PurchaseInvoiceDetailId == purchaseInvoiceDetailId && p.CurrencyId == currencyId);

            return result.Any();
        }

        public bool CheckCurrencyExist(Guid? purchaseInvoiceDetailId, int? currencyId, int? typeId, Expression<Func<PurchaseInvoiceDetailSalePrice, bool>> predicate = null)
        {
            IQueryable<PurchaseInvoiceDetailSalePrice> result = _context.PurchaseInvoiceDetailSalePrices.AsNoTracking()
                .Where(p => p.PurchaseInvoiceDetailId == purchaseInvoiceDetailId && p.CurrencyId == currencyId && p.TypeId == typeId);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }

        public IEnumerable<PurchaseInvoiceDetailSalePrice> GetAllTransferDetailSalePrice(Guid transferDetailId)
        {
            return _context.PurchaseInvoiceDetailSalePrices.AsNoTracking()
                .Include(p => p.TransferDetail).ThenInclude(p => p.PurchaseCurrency)
                .Include(p => p.Currency)
                .Include(p => p.Type)
                .Include(p => p.MoneyConvert).ThenInclude(p => p.BaseCurrency)
                .Include(p => p.MoneyConvert).ThenInclude(p => p.DestCurrency)
                .Where(p => p.TransferDetailId == transferDetailId)
                .Select(p => new PurchaseInvoiceDetailSalePrice
                {
                    TransferDetail = new TransferDetail
                    {
                        SellingPrice = p.TransferDetail.SellingPrice,
                        MiddleSellPrice = p.TransferDetail.MiddleSellPrice,
                        WholeSellPrice = p.TransferDetail.WholeSellPrice,
                        PurchaseCurrency = new BaseInfoGeneral
                        {
                            Name = p.TransferDetail.PurchaseCurrency.Name
                        }
                    },
                    Guid = p.Guid,
                    Currency = new BaseInfoGeneral
                    {
                        Name = p.Currency.Name
                    },
                    Type = new BaseInfoGeneral
                    {
                        Name = p.Type.Name
                    },
                    MoneyConvert = new MoneyConvert
                    {
                        BaseAmount = p.MoneyConvert.BaseAmount,
                        DestAmount = p.MoneyConvert.DestAmount,
                        BaseCurrency = new BaseInfoGeneral
                        {
                            Name = p.MoneyConvert.BaseCurrency.Name
                        },
                        DestCurrency = new BaseInfoGeneral
                        {
                            Name = p.MoneyConvert.DestCurrency.Name
                        },
                    }
                });
        }

        public IEnumerable<PurchaseInvoiceDetailSalePrice> GetAllPurchaseInvoiceDetailSalePrice(Guid purchaseInvoiceDetailId)
        {
            return _context.PurchaseInvoiceDetailSalePrices.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(p => p.Currency)
                .Include(p => p.Currency)
                .Include(p => p.Type)
                .Include(p => p.MoneyConvert).ThenInclude(p => p.BaseCurrency)
                .Include(p => p.MoneyConvert).ThenInclude(p => p.DestCurrency)
                .Where(p => p.PurchaseInvoiceDetailId == purchaseInvoiceDetailId)
                .Select(p => new PurchaseInvoiceDetailSalePrice
                {
                    PurchaseInvoiceDetail = new PurchaseInvoiceDetail
                    {
                        SellingPrice = p.PurchaseInvoiceDetail.SellingPrice,
                        MiddleSellPrice = p.PurchaseInvoiceDetail.MiddleSellPrice,
                        WholeSellPrice = p.PurchaseInvoiceDetail.WholeSellPrice,
                        Currency = new BaseInfoGeneral
                        {
                            Name = p.PurchaseInvoiceDetail.Currency.Name
                        }
                    },
                    Guid = p.Guid,
                    Currency = new BaseInfoGeneral
                    {
                        Name = p.Currency.Name
                    },
                    Type = new BaseInfoGeneral
                    {
                        Name = p.Type.Name
                    },
                    MoneyConvert = new MoneyConvert
                    {
                        BaseAmount = p.MoneyConvert.BaseAmount,
                        DestAmount = p.MoneyConvert.DestAmount,
                        BaseCurrency = new BaseInfoGeneral
                        {
                            Name = p.MoneyConvert.BaseCurrency.Name
                        },
                        DestCurrency = new BaseInfoGeneral
                        {
                            Name = p.MoneyConvert.DestCurrency.Name
                        },
                    }
                });
        }
    }
}
