using AutoMapper;
using Framework.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Application.Business
{
	internal class Startup : Framework.Core.IStartup
	{
		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<Helpers.MFUOperationsHelper, Helpers.MFUOperationsHelper>();

            //Instantiate the License class
            Aspose.Cells.License license = new Aspose.Cells.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense("Aspose.Cells.lic");
        }	
	}
}
