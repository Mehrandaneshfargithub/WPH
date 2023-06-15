﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.ExpireList
{
    public class ExpireListViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProducerName { get; set; }
        public decimal Num { get; set; }
        public decimal FreeNum { get; set; }
        public decimal PurchasePrice { get; set; }
        public string PurchaseCurrencyName { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal Stock { get; set; }
        public int InvoiceNum { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string InvoiceType { get; set; }
    }
}
