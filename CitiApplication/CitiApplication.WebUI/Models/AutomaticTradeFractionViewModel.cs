using Framework.MakerChecker;
using Framework.WebUI.Models;
using System;

namespace Application.WebUI.Models
{
    public class AutomaticTradeFractionViewModel
    {
        public StateFilter StateFilter { get; set; }
        public DateTime StateDate { get; set; }
        public GridCrudModel GridModel { get; set; }
        public string StateMaker { get; set; }
    }
}
