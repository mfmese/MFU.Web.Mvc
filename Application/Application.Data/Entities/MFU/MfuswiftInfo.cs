using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Data.Entities
{
    internal partial class MfuswiftInfo
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int? RowNumber { get; set; }
        public string Acc { get; set; }
        public DateTime? TradeDate { get; set; }
        public DateTime? SettDate { get; set; }
        public string Isin { get; set; }
        public decimal? Diff { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Price { get; set; }
        public string Refe { get; set; }
        public string Act { get; set; }
        public string TradeType { get; set; }
        
    }
}
