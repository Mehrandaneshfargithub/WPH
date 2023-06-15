using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Helper
{
    public class CommonWas
    {
        public static void GetPeriodDateTimes(ref DateTime dateFrom, ref DateTime dateTo, int periodId)
        {
            switch (periodId)
            {
                case (int)Periods.Day:
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                    break;
                case (int)Periods.Week:
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0).AddDays(-7);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                    break;
                case (int)Periods.Month:
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0).AddDays(-30);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                    break;
                case (int)Periods.Year:
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0).AddYears(-1);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                    break;
                case (int)Periods.Allready:
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0).AddYears(-100);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                    break;
            }
        }

    }

    public enum OperationStatus
    {
        SUCCESSFUL,
        ERROR_ThisRecordHasDependencyOnItInAnotherEntity,
        ERROR_SomeThingWentWrong,
        CanNotDelete,
        AreYouSure
    }
    public enum Periods
    {
        Day = 1,
        Week = 2,
        Month = 3,
        Year = 4,
        Allready = 5,
        FromDateToDate = 6
    }
    public enum MonthPeriods
    {
        OneMonth = 1,
        TwoMonth = 2,
        ThreeMonth = 3,
        Allready = 4,
        FromDateToDate = 5,
        SeeAllHuman = 6
    }

    public enum DischargeType
    {
        NotDischarge = 1,
        Discharge = 2,
        All = 3
    }

    public enum PaymentStatus
    {
        Unpaid = 1,
        Paid = 2,
        All = 3
    }

    public enum PurchaseFilter
    {
        Supplier = 1,
        Currency = 2,
        InvoiceNum = 3,
        MainInvoiceNum = 4
    }

    public enum SaleFilter
    {
        Customer = 1,
        Currency = 2,
        InvoiceNum = 3,

    }

    public enum TransferFilter
    {
        Type = 1,
        ClinicSection = 2,
    }

    public enum ProductFilter
    {
        Barcode = 1,
        OrderPointRange = 2,
        Supplier = 3
    }



    public enum ReceiveReportFilter
    {
        All = 1,
        UnpaidInvoice = 2,
        UnpaidInvoice_Sale = 3,
        UnpaidInvoice_ReturnSale = 4,
        PaidInvoice = 5,
        PaidInvoice_Sale = 6,
        PaidInvoice_ReturnSale = 7,
    }

    public enum PayReportFilter
    {
        All = 1,
        UnpaidInvoice = 2,
        UnpaidInvoice_Purchase = 3,
        UnpaidInvoice_ReturnPurchase = 4,
        PaidInvoice = 5,
        PaidInvoice_Purchase = 6,
        PaidInvoice_ReturnPurchase = 7,
    }

    public enum SaleType
    {
        Retail = 1,
        Middle = 2,
        Whole = 3,
    }

    public class Indexing<T> where T : IndexViewModel
    {

        public List<T> AddIndexing(List<T> TGenericList)
        {
            List<T> TGenericWithindex = new List<T>();
            for (int i = 0; i < TGenericList.Count(); i++)
            {
                T TGeneric = TGenericList[i];
                TGeneric.Index = i + 1;
                TGenericWithindex.Add(TGeneric);
            }
            return TGenericWithindex;
        }
    }


}
