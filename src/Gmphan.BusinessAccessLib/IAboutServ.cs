using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.BusinessAccessLib
{
    public interface IAboutServ
    {
        public Task SaveContactMeServAsync(ContactMe obj);
    }
}