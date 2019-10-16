using Framework.Configuration;
using Framework.WebUI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Application.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MvcStartup.ExecuteMain(() => CreateWebHostBuilder(args).Build().Run());
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration(MvcStartup.AddFrameworkConfiguration).UseStartup<Startup>();
    }
}
