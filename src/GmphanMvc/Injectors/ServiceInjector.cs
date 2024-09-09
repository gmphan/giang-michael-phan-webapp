using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GmphanMvc.Injectors
{
    public static class ServiceInjector
    {
        // this should be a static class, so its properties can be called without initializing an object from it
        public static void InjectServicesFromAssemblies(this IServiceCollection services, IConfiguration configuration)
        {
            // Create a list of injector instance from this Injectors namespace
            var injectors = typeof(Program).Assembly.ExportedTypes
                .Where(t => typeof(IInjector).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInjector>()
                .ToList();

            // Loop through the list to assign services ans configuration as requested
            injectors.ForEach(injector => {
                injector.InjectServices(services, configuration);
            });
        }
    }
}