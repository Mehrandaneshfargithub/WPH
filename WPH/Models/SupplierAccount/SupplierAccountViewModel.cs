using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.SupplierAccount
{
    public class SupplierAccountViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string RecordType { get; set; }
        public string Date { get; set; }
        public string InvoiceNum { get; set; }
        public string Invoices { get; set; }
        public string MainInvoices { get; set; }
        public string ReturnInvoices { get; set; }
        public string Description { get; set; }
        public string PayAmount { get; set; }
        public string GetAmount { get; set; }
        public string PayStatus { get; set; }
        public string MainInvoicePay { get; set; }
        public string Remain { get; set; }
        public string PayType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PayStatus))
                {
                    return "TotalPay";
                }
                else if (PayStatus.Contains("0"))
                {
                    return "PartialPay";
                }
                else if (PayStatus.Contains("1"))
                {
                    return "Pay";
                }
                else
                {
                    return "NotPay";
                }
            }
        }

        public string LineColor
        {
            get
            {
                if (RecordType == "Purchase")
                {
                    if (PayType == "PartialPay")
                    {
                        return "brown";
                    }
                    else if (PayType == "Pay")
                    {
                        return "green";
                    }
                    else
                    {
                        return "red";
                    }
                }
                else if (RecordType == "Return")
                {
                    if (PayType == "Pay")
                    {
                        return "green";
                    }
                    else
                    {
                        return "pink";
                    }
                }
                else if (RecordType == "Pay")
                {
                    return "light_gray";
                }
                else
                {
                    return " background:gray; ";
                }
            }
        }
        //public string LineColor
        //{
        //    get
        //    {
        //        if (RecordType == "Purchase")
        //        {
        //            if (PayType == "PartialPay")
        //            {
        //                return " background:#e18167;border-bottom: 1px solid #723f31 !important; ";
        //            }
        //            else if (PayType == "Pay")
        //            {
        //                return " background:#5d8402;border-bottom: 1px solid #b3d369 !important;  ";
        //            }
        //            else
        //            {
        //                return " background:#eb514c;border-bottom: 1px solid #fedfce !important;  ";
        //            }
        //        }
        //        else if (RecordType == "Return")
        //        {
        //            if (PayType == "Pay")
        //            {
        //                return " background:#5d8402;border-bottom: 1px solid #b3d369 !important; ";
        //            }
        //            else
        //            {
        //                return " background:#fedfce;border-bottom: 1px solid #6e5e55 !important;  ";
        //            }
        //        }
        //        else if (RecordType == "Pay")
        //        {
        //            return " background:#c2dac9;border-bottom: 1px solid #81ada0 !important; ";
        //        }
        //        else
        //        {
        //            return " background:gray; ";
        //        }
        //    }
        //}
    }
}
