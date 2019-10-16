using Microsoft.Extensions.Configuration;

namespace Application.WebUI
{
    public class Startup : Framework.WebUI.MvcStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
