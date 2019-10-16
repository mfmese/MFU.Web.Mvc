using Framework.MakerChecker;
using System;

namespace Application.Business.Models
{
    public partial class Option: MakerCheckerListItem
    {
        public int OptionId { get; set; }
        public string OptionKey { get; set; }
        public string OptionValue { get; set; }
    }
}
