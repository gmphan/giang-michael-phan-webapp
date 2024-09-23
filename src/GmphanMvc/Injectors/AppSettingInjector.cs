using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace GmphanMvc.Injectors
{
    public class AppSettingInjector : IInjector
    {
        public void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            // binding InitAdminUser info to services to use with IOptions
            services.Configure<InitAdminUser>(configuration.GetSection("InitAdminUser"));
        }
    }
}