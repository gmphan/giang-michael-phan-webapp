using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib;
using Microsoft.EntityFrameworkCore;

namespace GmphanMvc.Injectors
{
    public class GmphanInjector : IInjector
    {
        public void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(Options => Options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
        }
    }
}