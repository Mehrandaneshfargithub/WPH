using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class PurchaseInvoiceDetailRepository : Repository<PurchaseInvoiceDetail>, IPurchaseInvoiceDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PurchaseInvoiceDetailRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<PurchaseInvoiceDetail> GetAllPurchaseInvoiceDetailByMasterId(Guid purchaseInvoiceId)
        {
            return Context.PurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.Product).ThenInclude(P => P.Producer)
                .Include(p => p.Currency)
                .Where(p => p.MasterId == purchaseInvoiceId)
                ;
        }

        public IEnumerable<PurchaseInvoiceDetail> GetPurchaseHistoryByProductId(Guid clinicSectionId, Guid productId)
        {
            return Context.PurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.Master).ThenInclude(p => p.Supplier).ThenInclude(p => p.User)
                .Include(p => p.Currency)
                .Where(p => p.Master.ClinicSectionId == clinicSectionId && p.ProductId == productId)
                .Select(p => new PurchaseInvoiceDetail
                {
                    Num = p.Num,
                    FreeNum = p.FreeNum,
                    BujNumber = p.BujNumber,
                    Discount = p.Discount,
                    PurchasePrice = p.PurchasePrice,
                    SellingPrice = p.SellingPrice,
                    Consideration = p.Consideration,
                    ExpireDate = p.ExpireDate,
                    Currency = new BaseInfoGeneral
                    {
                        Name = p.Currency.Name
                    },

                    Master = new PurchaseInvoice
                    {
                        InvoiceDate = p.Master.InvoiceDate,
                        InvoiceNum = p.Master.InvoiceNum,
                        MainInvoiceNum = p.Master.MainInvoiceNum,
                        Supplier = new Supplier
                        {
                            User = new User
                            {
                                Name = p.Master.Supplier.User.Name
                            }
                        }
                    }
                });
        }

        public IEnumerable<PurchaseInvoiceDetail> GetAllTotalPrice(Guid purchaseInvoiceId)
        {
            return Context.PurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.Currency)
                .Where(p => p.MasterId == purchaseInvoiceId)
                .Select(p => new PurchaseInvoiceDetail
                {
                    Num = p.Num,
                    PurchasePrice = p.PurchasePrice,
                    Discount = p.Discount,
                    Currency = new BaseInfoGeneral
                    {
                        Name = p.Currency.Name
                    }
                })
                ;
        }

        public decimal GetSourceRemCount(Guid purchaseInvoiceDetailId)
        {
            return _context.PurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.TransferDetails)
                .Where(p => p.Guid == purchaseInvoiceDetailId)
                .Select(p => (p.Num ?? 0) + (p.FreeNum ?? 0) - p.TransferDetails.Where(x => x.TransferDetailId == null).Sum(s => (s.Num ?? 0)))
                .SingleOrDefault();
        }

        public PurchaseInvoiceDetail GetPurchaseInvoiceDetailForEdit(Guid purchaseInvoiceDetailId)
        {
            return _context.PurchaseInvoiceDetails.AsNoTracking()
                .SingleOrDefault(p => p.Guid == purchaseInvoiceDetailId);
        }

        public IEnumerable<ReturnPurchaseDetailModel> GetDetailsForReturn(Guid productId, Guid masterId, Guid clinicSectionId, bool like)
        {
            string masster = "";
            string purchase = "";
            if (like)
            {
                masster = $" AND masster.SupplierId=(SELECT SupplierId FROM dbo.ReturnPurchaseInvoice WHERE Guid='{masterId}') ";
                purchase = $" AND purchase.SupplierId=(SELECT SupplierId FROM dbo.ReturnPurchaseInvoice WHERE Guid='{masterId}')";
            }
            string query =
                " SELECT null TransferDetailId,piid.RemainingNum,piid.Guid,piid.Num,piid.FreeNum,piid.ExpireDate,piid.BujNumber,piid.Discount,piid.PurchasePrice,piid.SellingPrice,piid.Consideration, " +
                "     masster.InvoiceNum,masster.MainInvoiceNum,masster.InvoiceDate, " +
                "     currency.Name currency  " +
                " FROM dbo.PurchaseInvoiceDetails piid " +
                " LEFT JOIN dbo.PurchaseInvoice masster ON masster.Guid = piid.MasterId " +
                " LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = piid.CurrencyId " +
                $" WHERE masster.ClinicSectionId = '{clinicSectionId}' AND piid.ProductId = '{productId}' AND ISNULL(piid.RemainingNum,0)> 0 {masster}  " +

                " UNION " +

                " SELECT td.Guid TransferDetailId, td.RemainingNum, " +
                "     purchase.Guid,purchase.Num,purchase.FreeNum,purchase.ExpireDate,purchase.BujNumber,purchase.Discount,purchase.PurchasePrice,purchase.SellingPrice,purchase.Consideration, " +
                "     purchase.InvoiceNum,purchase.MainInvoiceNum,purchase.InvoiceDate,purchase.currency " +
                " FROM dbo.TransferDetail td " +
                " LEFT JOIN dbo.Transfer tf ON tf.Guid = td.MasterId " +
                " LEFT JOIN " +
                " (SELECT piid.Guid, piid.Num, piid.FreeNum, piid.ExpireDate, piid.BujNumber, piid.Discount, piid.PurchasePrice, piid.SellingPrice, piid.Consideration, " +
                "     masster.InvoiceNum, masster.MainInvoiceNum, masster.InvoiceDate, masster.SupplierId, " +
                "     currency.Name currency  " +
                " FROM dbo.PurchaseInvoiceDetails piid " +
                " LEFT JOIN dbo.PurchaseInvoice masster ON masster.Guid = piid.MasterId " +
                " LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id= piid.CurrencyId " +
                $" WHERE masster.ClinicSectionId= '{clinicSectionId}' AND piid.ProductId= '{productId}' {masster} " +
                " ) purchase ON purchase.Guid = td.SourcePurchaseInvoiceDetailId " +
                $" WHERE td.DestinationProductId = '{productId}' AND tf.DestinationClinicSectionId = '{clinicSectionId}' AND ISNULL(td.RemainingNum,0)> 0 {purchase} "
                ;

            try
            {
                return Context.Set<ReturnPurchaseDetailModel>().FromSqlRaw(query);
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<PurchaseInvoiceDetail> GetByMultipleIds(List<Guid> details)
        {
            return _context.PurchaseInvoiceDetails.AsNoTracking()
                .Where(p => details.Contains(p.Guid));
        }

        public IEnumerable<PurchaseInvoiceDetail> GetWithPricesByMultipleIds(List<Guid> details)
        {
            return _context.PurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetailSalePrices)
                .Where(p => details.Contains(p.Guid));
        }

        public PurchaseInvoiceDetail GetWithPricesById(Guid purchaseInvoiceDetailId)
        {
            return _context.PurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.PurchaseInvoiceDetailSalePrices)
                .SingleOrDefault(p => p.Guid == purchaseInvoiceDetailId);
        }

        public PurchaseOrTransferProductDetail GetProductDetails(Guid productId, bool latestPrice, string SaleType, int? SellCurrencyId)
        {

            try
            {

                string query = @"SELECT TOP 1 * FROM
                                    (SELECT PurchaseInvoiceDetails.Guid Guid, ExpireDate, RemainingNum Stock, 0.0 TotalStock, PurchasePrice, PurchaseCurrency.Name PurchaseCurrencyName, 'PurchaseInvoiceDetail' SaleType , Consideration ,
                                        PurchaseInvoiceDetails.CurrencyId CurrencyId,PurchaseInvoiceDetails.BujNumber BujNumber,";

                if (string.Compare(SaleType, "retailPrice", true) == 0)
                {
                    query += "SellingPrice  FirstPrice,";
                }
                else if (string.Compare(SaleType, "middelPrice", true) == 0)
                {
                    query += "MiddleSellPrice FirstPrice,  ";
                }
                else
                {
                    query += "WholeSellPrice FirstPrice,  ";
                }

                string join = "";
                string where = "";

                if (SellCurrencyId == null || SellCurrencyId == 0)
                {
                    query += "PurchaseInvoiceDetails.CurrencyId SellingCurrencyId, PurchaseCurrency.Name SellingCurrencyName, NULL MoneyConvertId,";
                    if (string.Compare(SaleType, "retailPrice", true) == 0)
                    {
                        query += "SellingPrice FROM ";
                    }
                    else if (string.Compare(SaleType, "middelPrice", true) == 0)
                    {
                        query += "MiddleSellPrice SellingPrice FROM ";
                    }
                    else
                    {
                        query += "WholeSellPrice SellingPrice FROM ";
                    }
                }
                else
                {

                    query += $" PurchaseInvoiceDetailSale.CurrencyId SellingCurrencyId, PurchaseInvoiceDetailSale.Name SellingCurrencyName, PurchaseInvoiceDetailSale.MoneyConvertId MoneyConvertId, ";

                    if (string.Compare(SaleType, "retailPrice", true) == 0)
                    {
                        where += $"AND SaleType.Name = 'retailPrice' AND PurchaseInvoiceDetailSalePrice.CurrencyId = {SellCurrencyId}";
                        query += "SellingPrice FROM ";
                    }
                    else if (string.Compare(SaleType, "middelPrice", true) == 0)
                    {
                        where += $"AND SaleType.Name = 'middelPrice' AND PurchaseInvoiceDetailSalePrice.CurrencyId = {SellCurrencyId}";
                        query += "MiddleSellPrice SellingPrice FROM ";
                    }
                    else
                    {
                        where += $"AND SaleType.Name = 'WholePrice' AND PurchaseInvoiceDetailSalePrice.CurrencyId = {SellCurrencyId}";
                        query += "WholeSellPrice SellingPrice FROM ";
                    }


                    join = $@"LEFT JOIN (SELECT PurchaseInvoiceDetailId, PurchaseInvoiceDetailSalePrice.CurrencyId, SaleCurrency.Name, PurchaseInvoiceDetailSalePrice.MoneyConvertId
					           FROM dbo.PurchaseInvoiceDetailSalePrice PurchaseInvoiceDetailSalePrice
                               LEFT JOIN dbo.BaseInfoGeneral SaleCurrency ON SaleCurrency.Id = PurchaseInvoiceDetailSalePrice.CurrencyId
                               LEFT JOIN dbo.BaseInfoGeneral SaleType ON SaleType.Id = PurchaseInvoiceDetailSalePrice.TypeId 
					           WHERE 1=1 {where})
					           PurchaseInvoiceDetailSale ON PurchaseInvoiceDetailSale.PurchaseInvoiceDetailId = PurchaseInvoiceDetails.Guid ";
                }


                query += "dbo.PurchaseInvoiceDetails INNER JOIN dbo.BaseInfoGeneral PurchaseCurrency ON PurchaseCurrency.Id = PurchaseInvoiceDetails.CurrencyId " +
                    join +
                    @$"WHERE ProductId = '{productId}' AND RemainingNum > 0 " +
                    " union SELECT Guid, ExpireDate , RemainingNum Stock, 0.0 TotalStock, PurchasePrice, PurchaseCurrency.Name PurchaseCurrencyName,'TransferDetail' SaleType, Consideration, " +
                    $" TransferDetail.CurrencyId CurrencyId,'' BujNumber,";

                if (string.Compare(SaleType, "retailPrice", true) == 0)
                {
                    query += "SellingPrice  FirstPrice,";
                }
                else if (string.Compare(SaleType, "middelPrice", true) == 0)
                {
                    query += "MiddleSellPrice FirstPrice,  ";
                }
                else
                {
                    query += "WholeSellPrice FirstPrice,  ";
                }
                string transferJoin = "";
                string transferWhere = "";

                if (SellCurrencyId == null || SellCurrencyId == 0)
                {
                    query += "TransferDetail.CurrencyId SellingCurrencyId, PurchaseCurrency.Name SellingCurrencyName, NULL MoneyConvertId,";
                    if (string.Compare(SaleType, "retailPrice", true) == 0)
                    {
                        query += "SellingPrice FROM ";
                    }
                    else if (string.Compare(SaleType, "middelPrice", true) == 0)
                    {
                        query += "MiddleSellPrice SellingPrice FROM ";
                    }
                    else
                    {
                        query += "WholeSellPrice SellingPrice FROM ";
                    }
                }
                else
                {

                    query += $" PurchaseInvoiceDetailSale.CurrencyId SellingCurrencyId, PurchaseInvoiceDetailSale.Name SellingCurrencyName, PurchaseInvoiceDetailSale.MoneyConvertId MoneyConvertId, ";

                    if (string.Compare(SaleType, "retailPrice", true) == 0)
                    {
                        transferWhere += $"AND SaleType.Name = 'retailPrice' AND PurchaseInvoiceDetailSalePrice.CurrencyId = {SellCurrencyId} ";
                        query += "SellingPrice FROM ";
                    }
                    else if (string.Compare(SaleType, "middelPrice", true) == 0)
                    {
                        transferWhere += $"AND SaleType.Name = 'middelPrice' AND PurchaseInvoiceDetailSalePrice.CurrencyId = {SellCurrencyId}";
                        query += "MiddleSellPrice SellingPrice FROM ";
                    }
                    else
                    {
                        transferWhere += $"AND SaleType.Name = 'WholePrice' AND PurchaseInvoiceDetailSalePrice.CurrencyId = {SellCurrencyId}";
                        query += "WholeSellPrice SellingPrice FROM ";
                    }


                    transferJoin = $@"LEFT JOIN (SELECT TransferDetailId, PurchaseInvoiceDetailSalePrice.CurrencyId, SaleCurrency.Name, PurchaseInvoiceDetailSalePrice.MoneyConvertId
					           FROM dbo.PurchaseInvoiceDetailSalePrice PurchaseInvoiceDetailSalePrice
                               LEFT JOIN dbo.BaseInfoGeneral SaleCurrency ON SaleCurrency.Id = PurchaseInvoiceDetailSalePrice.CurrencyId
                               LEFT JOIN dbo.BaseInfoGeneral SaleType ON SaleType.Id = PurchaseInvoiceDetailSalePrice.TypeId 
					           WHERE 1=1 {transferWhere})
					           PurchaseInvoiceDetailSale ON PurchaseInvoiceDetailSale.TransferDetailId = TransferDetail.Guid ";


                    //join = @"LEFT JOIN dbo.PurchaseInvoiceDetailSalePrice PurchaseInvoiceDetailSalePrice ON PurchaseInvoiceDetailSalePrice.PurchaseInvoiceDetailId = PurchaseInvoiceDetails.Guid
                    //            LEFT JOIN dbo.BaseInfoGeneral SaleCurrency ON SaleCurrency.Id = PurchaseInvoiceDetailSalePrice.CurrencyId
                    //            LEFT JOIN dbo.BaseInfoGeneral SaleType ON SaleType.Id = PurchaseInvoiceDetailSalePrice.TypeId ";
                }

                query += $" dbo.TransferDetail INNER JOIN dbo.BaseInfoGeneral PurchaseCurrency ON PurchaseCurrency.Id = TransferDetail.CurrencyId " + transferJoin +
                    $"WHERE DestinationProductId = '{productId}' AND RemainingNum > 0)Temp ORDER BY Temp.ExpireDate ";



                var result = Context.Set<PurchaseOrTransferProductDetail>().FromSqlRaw(query).FirstOrDefault();

                if (result.SellingCurrencyId == null || SellCurrencyId == result.CurrencyId)
                {
                    result.SellingCurrencyId = result.CurrencyId;
                    result.SellingCurrencyName = result.PurchaseCurrencyName;
                    result.MoneyConvertId = null;
                }


                Guid? moneyConvertId = null;

                var purchaseinvoiceType = string.Compare(result?.SaleType, "PurchaseinvoiceDetail", true);

                if (latestPrice)
                {

                    if (string.Compare(SaleType, "retailPrice", true) == 0)
                    {
                        var latestPriceTransferOrPurchase = _context.PurchaseInvoiceDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                        .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                        .AsNoTracking()
                            .Where(p => p.ProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.Currency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").MoneyConvertId
                            })?
                            .Union(_context.TransferDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                            .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                            .AsNoTracking()
                            .Where(p => p.DestinationProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.PurchaseCurrency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").MoneyConvertId
                            }))?
                            .OrderBy(a => a.Id)?.LastOrDefault();


                        result.SellingPrice = latestPriceTransferOrPurchase.SellingPrice;

                        if (SellCurrencyId == null || SellCurrencyId == 0 || SellCurrencyId == latestPriceTransferOrPurchase.CurrencyId || result.SellingCurrencyId == null)
                        {
                            result.SellingCurrencyId = latestPriceTransferOrPurchase.CurrencyId;
                            result.SellingCurrencyName = latestPriceTransferOrPurchase.SellingCurrencyName;
                            result.MoneyConvertId = null;
                        }
                    }
                    else if (string.Compare(SaleType, "middelPrice", true) == 0)
                    {

                        var latestPriceTransferOrPurchase = _context.PurchaseInvoiceDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                        .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                        .AsNoTracking()
                            .Where(p => p.ProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.Currency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").MoneyConvertId
                            })?
                            .Union(_context.TransferDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                            .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                            .AsNoTracking()
                            .Where(p => p.DestinationProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.PurchaseCurrency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").MoneyConvertId
                            }))?
                            .OrderBy(a => a.Id)?.LastOrDefault();

                        result.SellingPrice = latestPriceTransferOrPurchase.MiddleSellPrice;

                        if (SellCurrencyId == null || SellCurrencyId == 0 || SellCurrencyId == latestPriceTransferOrPurchase.CurrencyId || result.SellingCurrencyId == null)
                        {
                            result.SellingCurrencyId = latestPriceTransferOrPurchase.CurrencyId;
                            result.SellingCurrencyName = latestPriceTransferOrPurchase.SellingCurrencyName;
                            result.MoneyConvertId = null;
                        }
                    }
                    else
                    {

                        var latestPriceTransferOrPurchase = _context.PurchaseInvoiceDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                        .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                        .AsNoTracking()
                            .Where(p => p.ProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.Currency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").MoneyConvertId
                            })?
                            .Union(_context.TransferDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                            .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                            .AsNoTracking()
                            .Where(p => p.DestinationProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.PurchaseCurrency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").MoneyConvertId
                            }))?
                            .OrderBy(a => a.Id)?.LastOrDefault();

                        result.SellingPrice = latestPriceTransferOrPurchase.WholeSellPrice;

                        if (SellCurrencyId == null || SellCurrencyId == 0 || SellCurrencyId == latestPriceTransferOrPurchase.CurrencyId || result.SellingCurrencyId == null)
                        {
                            result.SellingCurrencyId = latestPriceTransferOrPurchase.CurrencyId;
                            result.SellingCurrencyName = latestPriceTransferOrPurchase.SellingCurrencyName;
                            result.MoneyConvertId = null;
                        }
                    }


                }

                

                var purchaseStock = _context.PurchaseInvoiceDetails.Where(a => a.ProductId == productId && a.RemainingNum > 0).Sum(a => a.RemainingNum);
                var transferStock = _context.TransferDetails.Where(a => a.DestinationProductId == productId && a.RemainingNum > 0).Sum(a => a.RemainingNum);

                result.TotalStock = (decimal)(purchaseStock + transferStock);
                result.FirstPrice = result.SellingPrice;

                return result;
            }
            catch (Exception e) { return null; }

        }

        public IEnumerable<PurchaseOrTransferProductDetail> GetAllProductExpireList(Guid productId)
        {
            try
            {
                string query = "SELECT * FROM(SELECT Guid, ExpireDate, RemainingNum Stock,0.0 TotalStock,0 SellingCurrencyId,0.0 SellingPrice,'' SellingCurrencyName,'' PurchaseCurrencyName ,0 CurrencyId, 0.0 FirstPrice,'' BujNumber," +
                    $"0.0 PurchasePrice, 'PurchaseInvoiceDetail' SaleType,NULL MoneyConvertId, '' Consideration FROM dbo.PurchaseInvoiceDetails WHERE ProductId = '{productId}' AND RemainingNum > 0" +
                    $" union " +
                    $"SELECT Guid, ExpireDate , RemainingNum Stock,0.0 TotalStock,0 SellingCurrencyId,0.0 SellingPrice,'' SellingCurrencyName,'' PurchaseCurrencyName ,0 CurrencyId, 0.0 FirstPrice,'' BujNumber,0.0 PurchasePrice, 'TransferDetail' SaleType,NULL MoneyConvertId, '' Consideration" +
                    $" FROM dbo.TransferDetail WHERE DestinationProductId = '{productId}' AND RemainingNum > 0)Temp ORDER BY Temp.ExpireDate ";

                var result = Context.Set<PurchaseOrTransferProductDetail>().FromSqlRaw(query);

                return result;
            }
            catch (Exception) { return null; }
        }

        public IEnumerable<ExpireListModel> GetExpiredList(Guid clinicSectionId, string type)
        {

            DateTime expireDate = new();

            switch (type)
            {
                case "expired":
                    expireDate = DateTime.Now;
                    break;
                case "3month":
                    expireDate = DateTime.Now.AddMonths(3);
                    break;
                case "6month":
                    expireDate = DateTime.Now.AddMonths(6);
                    break;
                case "year":
                    expireDate = DateTime.Now.AddYears(1);
                    break;
            }

            try
            {
                string query = $@"SELECT * FROM(SELECT PurchaseInvoice.GUID Guid, product.Name ProductName, productType.Name ProductType, producer.Name ProducerName, Num, FreeNum, PurchasePrice, purchaseCurrency.Name PurchaseCurrencyName, ExpireDate, RemainingNum Stock, dbo.PurchaseInvoice.InvoiceNum InvoiceNum,
                                    dbo.PurchaseInvoiceDetails.CreateDate PurchaseDate, 'purchase' InvoiceType
                                    FROM dbo.PurchaseInvoiceDetails
                                    INNER JOIN dbo.PurchaseInvoice  ON PurchaseInvoice.Guid = PurchaseInvoiceDetails.MasterId
                                    INNER JOIN dbo.BaseInfoGeneral purchaseCurrency  ON purchaseCurrency.Id = PurchaseInvoiceDetails.CurrencyId
                                    INNER JOIN dbo.Product product ON Product.Guid = PurchaseInvoiceDetails.ProductId
                                    LEFT JOIN dbo.BaseInfo productType ON Product.ProductTypeId = productType.GUID
                                    LEFT JOIN dbo.BaseInfo producer ON Product.ProducerId = producer.GUID
                                    WHERE PurchaseInvoice.ClinicSectionId = '{clinicSectionId}' AND RemainingNum > 0 AND ExpireDate < '{expireDate}'
                                    union 
                                    SELECT Transfer.GUID Guid, product.Name ProductName, productType.Name ProductType, producer.Name ProducerName, Num,0.0 FreeNum, PurchasePrice, transferCurrency.Name PurchaseCurrencyName, ExpireDate, RemainingNum Stock, dbo.Transfer.InvoiceNum InvoiceNum,
                                    dbo.TransferDetail.CreatedDate PurchaseDate, 'transfer' InvoiceType
                                    FROM dbo.TransferDetail
                                    INNER JOIN dbo.Transfer ON Transfer.Guid = TransferDetail.MasterId
                                    INNER JOIN dbo.BaseInfoGeneral transferCurrency ON transferCurrency.Id = TransferDetail.CurrencyId
                                    INNER JOIN dbo.Product product ON Product.Guid = TransferDetail.ProductId
                                    LEFT JOIN dbo.BaseInfo productType ON Product.ProductTypeId = productType.GUID
                                    LEFT JOIN dbo.BaseInfo producer ON Product.ProducerId = producer.GUID
                                    WHERE DestinationClinicSectionId = '{clinicSectionId}' AND RemainingNum > 0 AND ExpireDate < '{expireDate}')Temp ORDER BY Temp.ExpireDate ;";

                var result = Context.Set<ExpireListModel>().FromSqlRaw(query);

                return result;
            }
            catch (Exception) { return null; }
        }

        public PurchaseOrTransferProductDetail GetProductDetailsFromExpireList(Guid invoiceId, string invoiceType, string SaleType, int? SellCurrencyId, bool latestPrice, Guid productId)
        {

            try
            {

                string query = "";
                string sellingCurrencyId = "";
                string currencyId = "";
                string leftJoin = "";
                string innerJoin = "";

                if (string.Compare(invoiceType, "TransferDetail", true) == 0)
                {
                    query += "SELECT TransferDetail.Guid Guid,'' BujNumber";
                    sellingCurrencyId += ",TransferDetail.CurrencyId SellingCurrencyId";
                    currencyId += ",TransferDetail.CurrencyId CurrencyId";
                    leftJoin += "PurchaseInvoiceDetailSale ON PurchaseInvoiceDetailSale.TransferDetailId = TransferDetail.Guid";
                    innerJoin += $"dbo.TransferDetail INNER JOIN dbo.BaseInfoGeneral PurchaseCurrency ON PurchaseCurrency.Id = TransferDetail.CurrencyId ";
                }
                else
                {
                    query += "SELECT PurchaseInvoiceDetails.Guid Guid,PurchaseInvoiceDetails.BujNumber BujNumber";
                    sellingCurrencyId += ",PurchaseInvoiceDetails.CurrencyId SellingCurrencyId";
                    currencyId += ",PurchaseInvoiceDetails.CurrencyId CurrencyId";
                    leftJoin += "PurchaseInvoiceDetailSale ON PurchaseInvoiceDetailSale.PurchaseInvoiceDetailId = PurchaseInvoiceDetails.Guid";
                    innerJoin += $"dbo.PurchaseInvoiceDetails INNER JOIN dbo.BaseInfoGeneral PurchaseCurrency ON PurchaseCurrency.Id = PurchaseInvoiceDetails.CurrencyId ";
                }

                 query += ", ExpireDate , RemainingNum Stock, 0.0 TotalStock, PurchasePrice, PurchaseCurrency.Name PurchaseCurrencyName,'" +
                    invoiceType + "' SaleType,Consideration "+ currencyId;

                if (string.Compare(SaleType, "retailPrice", true) == 0)
                {
                    query += ",SellingPrice  FirstPrice";
                }
                else if (string.Compare(SaleType, "middelPrice", true) == 0)
                {
                    query += ",MiddleSellPrice FirstPrice ";
                }
                else
                {
                    query += ",WholeSellPrice FirstPrice  ";
                }


                string join = "";
                string where = "";

                if (SellCurrencyId == null || SellCurrencyId == 0)
                {
                    query += sellingCurrencyId + ", PurchaseCurrency.Name SellingCurrencyName, NULL MoneyConvertId";

                    if (string.Compare(SaleType, "retailPrice", true) == 0)
                    {
                        query += ",SellingPrice FROM ";
                    }
                    else if (string.Compare(SaleType, "middelPrice", true) == 0)
                    {
                        query += ",MiddleSellPrice SellingPrice FROM ";
                    }
                    else
                    {
                        query += ",WholeSellPrice SellingPrice FROM ";
                    }
                }
                else
                {

                    query += $" ,PurchaseInvoiceDetailSale.CurrencyId SellingCurrencyId, PurchaseInvoiceDetailSale.Name SellingCurrencyName, PurchaseInvoiceDetailSale.MoneyConvertId MoneyConvertId ";

                    if (string.Compare(SaleType, "retailPrice", true) == 0)
                    {
                        where += "AND SaleType.Name = 'retailPrice'";
                        query += ",SellingPrice FROM ";
                    }
                    else if (string.Compare(SaleType, "middelPrice", true) == 0)
                    {
                        where += "AND SaleType.Name = 'middelPrice'";
                        query += ",MiddleSellPrice SellingPrice FROM ";
                    }
                    else
                    {
                        where += "AND SaleType.Name = 'WholePrice'";
                        query += ",WholeSellPrice SellingPrice FROM ";
                    }


                    join = $@"LEFT JOIN (SELECT PurchaseInvoiceDetailId, PurchaseInvoiceDetailSalePrice.CurrencyId, SaleCurrency.Name, PurchaseInvoiceDetailSalePrice.MoneyConvertId
					           FROM dbo.PurchaseInvoiceDetailSalePrice PurchaseInvoiceDetailSalePrice
                               LEFT JOIN dbo.BaseInfoGeneral SaleCurrency ON SaleCurrency.Id = PurchaseInvoiceDetailSalePrice.CurrencyId
                               LEFT JOIN dbo.BaseInfoGeneral SaleType ON SaleType.Id = PurchaseInvoiceDetailSalePrice.TypeId 
					           WHERE PurchaseInvoiceDetailSalePrice.CurrencyId = {SellCurrencyId}  {where})" + leftJoin;
                }

                query += innerJoin + join + $" WHERE Guid = '{invoiceId}'";


                var result = Context.Set<PurchaseOrTransferProductDetail>().FromSqlRaw(query).FirstOrDefault();

                if (SellCurrencyId == null || SellCurrencyId == 0 || SellCurrencyId == result.CurrencyId || result.SellingCurrencyId == null)
                {
                    result.SellingCurrencyId = result.CurrencyId;
                    result.SellingCurrencyName = result.PurchaseCurrencyName;
                    result.MoneyConvertId = null;
                }

                Guid? moneyConvertId = null;

                if (latestPrice)
                {

                    if (string.Compare(SaleType, "retailPrice", true) == 0)
                    {
                        var latestPriceTransferOrPurchase = _context.PurchaseInvoiceDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                        .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                        .AsNoTracking()
                            .Where(p => p.ProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.Currency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").MoneyConvertId
                            })?
                            .Union(_context.TransferDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                            .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                            .AsNoTracking()
                            .Where(p => p.DestinationProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.PurchaseCurrency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "retailPrice").MoneyConvertId
                            }))?
                            .OrderBy(a => a.Id)?.LastOrDefault();


                        result.SellingPrice = latestPriceTransferOrPurchase.SellingPrice;

                        if (SellCurrencyId == null || SellCurrencyId == 0 || SellCurrencyId == latestPriceTransferOrPurchase.CurrencyId || result.SellingCurrencyId == null)
                        {
                            result.SellingCurrencyId = latestPriceTransferOrPurchase.CurrencyId;
                            result.SellingCurrencyName = latestPriceTransferOrPurchase.SellingCurrencyName;
                            result.MoneyConvertId = null;
                        }
                    }
                    else if (string.Compare(SaleType, "middelPrice", true) == 0)
                    {

                        var latestPriceTransferOrPurchase = _context.PurchaseInvoiceDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                        .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                        .AsNoTracking()
                            .Where(p => p.ProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.Currency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").MoneyConvertId
                            })?
                            .Union(_context.TransferDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                            .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                            .AsNoTracking()
                            .Where(p => p.DestinationProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.PurchaseCurrency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "middelPrice").MoneyConvertId
                            }))?
                            .OrderBy(a => a.Id)?.LastOrDefault();

                        result.SellingPrice = latestPriceTransferOrPurchase.MiddleSellPrice;

                        if (SellCurrencyId == null || SellCurrencyId == 0 || SellCurrencyId == latestPriceTransferOrPurchase.CurrencyId || result.SellingCurrencyId == null)
                        {
                            result.SellingCurrencyId = latestPriceTransferOrPurchase.CurrencyId;
                            result.SellingCurrencyName = latestPriceTransferOrPurchase.SellingCurrencyName;
                            result.MoneyConvertId = null;
                        }
                    }
                    else
                    {

                        var latestPriceTransferOrPurchase = _context.PurchaseInvoiceDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                        .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                        .AsNoTracking()
                            .Where(p => p.ProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.Currency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").MoneyConvertId
                            })?
                            .Union(_context.TransferDetails.Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Type)
                            .Include(a => a.PurchaseInvoiceDetailSalePrices).ThenInclude(a => a.Currency)
                            .AsNoTracking()
                            .Where(p => p.DestinationProductId == productId)?.Select(a =>
                            new {
                                a.Id,
                                a.SellingPrice,
                                a.MiddleSellPrice,
                                a.WholeSellPrice,
                                a.CurrencyId,
                                SellingCurrencyName = a.PurchaseCurrency.Name,
                                PurchaseInvoiceDetailSalePriceCurrencyId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").CurrencyId,
                                PurchaseInvoiceDetailSalePriceCurrencyName = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").Currency.Name,
                                PurchaseInvoiceDetailSalePriceMoneyConvertId = a.PurchaseInvoiceDetailSalePrices.Where(a => a.CurrencyId == SellCurrencyId).FirstOrDefault(a => a.Type.Name == "wholeprice").MoneyConvertId
                            }))?
                            .OrderBy(a => a.Id)?.LastOrDefault();

                        result.SellingPrice = latestPriceTransferOrPurchase.WholeSellPrice;

                        if (SellCurrencyId == null || SellCurrencyId == 0 || SellCurrencyId == latestPriceTransferOrPurchase.CurrencyId || result.SellingCurrencyId == null)
                        {
                            result.SellingCurrencyId = latestPriceTransferOrPurchase.CurrencyId;
                            result.SellingCurrencyName = latestPriceTransferOrPurchase.SellingCurrencyName;
                            result.MoneyConvertId = null;
                        }
                    }

                    


                }
                
                return result;

            }
            catch (Exception e) { return null; }
        }

        public void IncreaseUpdateWithLocal(PurchaseInvoiceDetail detail, decimal remainingNum)
        {
            var track = _context.Set<PurchaseInvoiceDetail>().Local.SingleOrDefault(e => e.Guid == detail.Guid);
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

        public IEnumerable<PieChartModel> GetProductStocks(Guid clinicSectionId)
        {
            string qury = @$"SELECT SUM(temp.Value) Value,temp.Label
                                FROM(
									(SELECT TOP 10  SUM(RemainingNum)  AS Value, dbo.Product.Name AS Label
									FROM dbo.PurchaseInvoiceDetails
									INNER JOIN dbo.PurchaseInvoice ON PurchaseInvoice.Guid = PurchaseInvoiceDetails.MasterId
									INNER JOIN dbo.Product ON Product.GUID = PurchaseInvoiceDetails.ProductId
									WHERE dbo.PurchaseInvoice.ClinicSectionId = '{clinicSectionId}' AND RemainingNum BETWEEN 1 AND 100
									GROUP BY dbo.Product.Name )
													UNION
                                    (SELECT TOP 10  SUM(RemainingNum)  AS Value, dbo.Product.Name AS Label
									FROM dbo.TransferDetail
									INNER JOIN dbo.Transfer ON Transfer.Guid = TransferDetail.MasterId
									INNER JOIN dbo.Product ON Product.GUID = TransferDetail.ProductId
									WHERE dbo.Transfer.DestinationClinicSectionId = '{clinicSectionId}' AND RemainingNum BETWEEN 1 AND 100
									GROUP BY dbo.Product.Name )
													) AS temp GROUP BY temp.Label";

            return _context.Set<PieChartModel>().FromSqlRaw(qury);

        }

        public PurchaseInvoiceDetail GetForNewSalePrice(Guid purchaseInvoiceDetailId)
        {
            return Context.PurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.Product).ThenInclude(P => P.Producer)
                .Include(p => p.Currency)
                .Where(p => p.Guid == purchaseInvoiceDetailId)
                .SingleOrDefault()
                ;
        }

        public PurchaseInvoiceDetail GetForSalePrice(Guid purchaseInvoiceDetailId)
        {
            return Context.PurchaseInvoiceDetails.AsNoTracking()
                .Where(p => p.Guid == purchaseInvoiceDetailId)
                .Select(p => new PurchaseInvoiceDetail
                {
                    MasterId = p.MasterId,
                    CurrencyId = p.CurrencyId,
                })
                .SingleOrDefault()
                ;
        }

        public PurchaseInvoiceDetail GetParentCurrency(Guid purchaseInvoiceDetailId)
        {
            return Context.PurchaseInvoiceDetails.AsNoTracking()
                .Include(p => p.Currency)
                .Where(p => p.Guid == purchaseInvoiceDetailId)
                .Select(p => new PurchaseInvoiceDetail
                {
                    MasterId = p.MasterId,
                    CurrencyId = p.CurrencyId,
                    SellingPrice = p.SellingPrice,
                    MiddleSellPrice = p.MiddleSellPrice,
                    WholeSellPrice = p.WholeSellPrice,
                    CurrencyName = p.Currency.Name
                })
                .SingleOrDefault()
                ;
        }

        public IEnumerable<PurchaseInvoiceDetailPriceModel> GetProductLastPricesByProducId(Guid productId, Guid transferId)
        {
            try
            {
                var clinicSectionId = _context.Transfers.AsNoTracking().SingleOrDefault(p => p.Guid == transferId).SourceClinicSectionId;
                string query = $@"SELECT InvoiceType,InvoiceDate,ExpireDate,BujNumber,RemainingNum,Guid,CurrencyName,ISNULL(Consideration,'') Consideration,PurchasePrice,SellingPrice,WholeSellPrice,MiddleSellPrice FROM 
                                (SELECT 'Purchase' InvoiceType,invoice.InvoiceDate,detail.ExpireDate,detail.BujNumber,detail.RemainingNum,detail.Guid,detail.Consideration,currency.Name CurrencyName,ISNULL(detail.PurchasePrice,0) PurchasePrice,ISNULL(detail.SellingPrice,0) SellingPrice,ISNULL(detail.WholeSellPrice,0) WholeSellPrice,ISNULL(detail.MiddleSellPrice,0) MiddleSellPrice,detail.CreateDate FROM dbo.PurchaseInvoiceDetails detail
                                LEFT JOIN dbo.PurchaseInvoice invoice ON invoice.Guid = detail.MasterId
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = detail.CurrencyId
                                WHERE detail.ProductId='{productId}' AND detail.RemainingNum > 0 AND invoice.ClinicSectionId='{clinicSectionId}'
                                UNION
                                SELECT 'Transfer' InvoiceType,invoice.InvoiceDate,detail.ExpireDate,purchase.BujNumber,detail.RemainingNum,detail.Guid,detail.Consideration,currency.Name CurrencyName,ISNULL(detail.PurchasePrice,0) PurchasePrice,ISNULL(detail.SellingPrice,0) SellingPrice,ISNULL(detail.WholeSellPrice,0) WholeSellPrice,ISNULL(detail.MiddleSellPrice,0) MiddleSellPrice,detail.CreatedDate FROM dbo.TransferDetail detail
                                LEFT JOIN dbo.Transfer invoice ON invoice.Guid = detail.MasterId
                                LEFT JOIN dbo.BaseInfoGeneral currency ON currency.Id = detail.CurrencyId
                                LEFT JOIN dbo.PurchaseInvoiceDetails purchase ON purchase.Guid = detail.SourcePurchaseInvoiceDetailId
                                WHERE detail.DestinationProductId='{productId}' AND detail.RemainingNum > 0 AND invoice.DestinationClinicSectionId='{clinicSectionId}') detail
                                WHERE detail.CurrencyName IS NOT NULL AND detail.SellingPrice IS NOT NULL
                                ORDER BY detail.InvoiceDate DESC,detail.CreateDate DESC";

                var result = Context.Set<PurchaseInvoiceDetailPriceModel>().FromSqlRaw(query);

                return result;
            }
            catch (Exception) { return null; }
        }

        public bool CheckDetailInUse(Guid detailId)
        {
            return _context.PurchaseInvoiceDetails.AsNoTracking()
                .Where(p => p.Guid == detailId)
                .Select(p => p.DamageDetails.Any() || p.ReturnPurchaseInvoiceDetails.Any() || p.SaleInvoiceDetails.Any())
                .SingleOrDefault();
        }

    }
}
