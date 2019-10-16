using System;
using System.Collections.Generic;

namespace Application.Data.Entities
{
    internal partial class Mfumodel
    {
        public int ModelId { get; set; }
        public int FileId { get; set; }
        public DateTime ExchangeDate { get; set; }
        public string Member { get; set; }
        public string AccountNo { get; set; }
        public string Isin { get; set; }
        public string DefineDetail { get; set; }
        public decimal BeginOfDay { get; set; }
        public decimal Balance { get; set; }
        public string DebtOrMoneyOwedToOne { get; set; }
        public string Columns { get; set; }
        public string ClearingType { get; set; }
    }
}
