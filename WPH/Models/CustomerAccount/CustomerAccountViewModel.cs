using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.CustomerAccount
{
    public class CustomerAccountViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string RecordType { get; set; }
        public string Date { get; set; }
        public string InvoiceNum { get; set; }
        public string Invoices { get; set; }
        public string ReturnInvoices { get; set; }
        public string Description { get; set; }
        public string ReceiveAmount { get; set; }
        public string GetAmount { get; set; }
        public string ReceiveStatus { get; set; }
        public string Remain { get; set; }
        public string ReceiveType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ReceiveStatus))
                {
                    return "TotalReceive";
                }
                else if (ReceiveStatus.Contains("2"))
                {
                    return "ReceiveInFactor";
                }
                else if (ReceiveStatus.Contains("0"))
                {
                    return "PartialReceive";
                }
                else if (ReceiveStatus.Contains("1"))
                {
                    return "Receive";
                }
                else
                {
                    return "NotReceive";
                }
            }
        }

        public string LineColor
        {
            get
            {
                if (RecordType == "Sale")
                {
                    if (ReceiveType == "PartialReceive")
                    {
                        return "brown";
                    }
                    else if (ReceiveType == "Receive")
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
                    if (ReceiveType == "Receive")
                    {
                        return "green";
                    }
                    else
                    {
                        return "pink";
                    }
                }
                else if (RecordType == "Receive")
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
        //        if (RecordType == "Sale")
        //        {
        //            if (ReceiveType == "PartialReceive")
        //            {
        //                return " background:#e18167;border-bottom: 1px solid #723f31 !important; ";
        //            }
        //            else if (ReceiveType == "Receive")
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
        //            if (ReceiveType == "Receive")
        //            {
        //                return " background:#5d8402;border-bottom: 1px solid #b3d369 !important; ";
        //            }
        //            else
        //            {
        //                return " background:#fedfce;border-bottom: 1px solid #6e5e55 !important;  ";
        //            }
        //        }
        //        else if (RecordType == "Receive")
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
