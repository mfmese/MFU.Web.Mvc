using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Application.Integration
{
	internal class Startup : Framework.Core.IStartup
	{
		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			//services.AddSingleton<Services.IRemoteSystem1Service, RemoteSystem1.RemoteService1>();
			//services.AddSingleton<Services.IRemoteSystem2Service, RemoteSystem2.RemoteService2>();
		}	
	}
}
