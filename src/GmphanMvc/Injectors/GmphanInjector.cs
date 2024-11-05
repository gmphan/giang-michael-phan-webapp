using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib;
using Gmphan.DataAccessLib;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.UtilityLib;
using Microsoft.EntityFrameworkCore;

namespace GmphanMvc.Injectors
{
    public class GmphanInjector : IInjector
    {
        public void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(Options => Options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnityOfWork, UnityOfWork>();
            services.AddScoped<IQuoteCollectionServ, QuoteCollectionServ>();
            services.AddScoped<IResumeServ, ResumeServ>();
            services.AddScoped<IProjectServ, ProjectServ>();
            services.AddScoped<IAboutServ, AboutServ>();
            services.AddScoped<IContactServ, ContactServ>();
            services.AddScoped<IPostServ, PostServ>();

            // Add Utilities
            services.AddScoped<IGetTAndCacheGeneric, GetTAndCacheGeneric>();
            // services// Seed the admin user
            services.AddScoped<ISeedAdminIdentityServ, SeedAdminIdentityServ>();
        }
    }
}