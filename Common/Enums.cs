using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Enums
    {
        public enum SupplierFilter
        {
            All = 1,
            CashPayment = 2,
            GetCash = 3,
            Invoice = 4,
            NotReceivedInvoices = 5,
            PartialReceivedInvoices = 6,
            ReceivedInvoices = 7
        }

        public enum ProductReportFilter
        {
            Cardex = 1,
            Purchase = 2,
            Sale = 3,
            ReturnPurchase = 4,
            ReturnSale = 5,
            TransferProduct = 6,
            ReceiveProduct = 7,
        }
    }
}
