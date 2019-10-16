using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Framework.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Data
{
	internal class Startup : Framework.Core.IStartup
	{
		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
            string connectionString = configuration.GetConnectionString(Framework.Enums.ConnectionStrings.ApplicationConnection);
            services.AddDbContext<MFUContext>(o => o.UseSqlServer(connectionString), ServiceLifetime.Transient);
		}	
	}
}
