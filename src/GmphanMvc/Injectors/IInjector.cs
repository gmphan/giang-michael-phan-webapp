using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GmphanMvc.Injectors
{
    public interface IInjector
    {
        //This interface will be implemented by various installer classes 
        //that are responsible for setting up specific services.
        void InjectServices(IServiceCollection services, IConfiguration configuration);
    }
}