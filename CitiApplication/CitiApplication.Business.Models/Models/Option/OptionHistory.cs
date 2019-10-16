using Framework.MakerChecker;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Business.Models
{
    public partial class OptionHistory: MakerCheckerListItem
    {
        [Key]
        public int HistoryId { get; set; }
        public int OptionId { get; set; }
        public string OptionKey { get; set; }
        public string OptionValue { get; set; }
    }
}
