using Framework.WebUI.Models;
using System;

namespace Application.WebUI.Models
{
    public class MessageGenerationViewModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public GridCrudModel GridModel { get; set; }

    }
}
