using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GmphanMvc.Injectors
{
    public class MvcInjector : IInjector
    {
        public void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
        }
    }
}