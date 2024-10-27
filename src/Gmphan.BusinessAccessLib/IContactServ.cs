using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;

namespace Gmphan.BusinessAccessLib
{
    public interface IContactServ
    {
        public Task SaveContactMeServAsync(ContactMe obj);
        public Task<ContactMeListView> GetContactMeListViewSerAsync();
    }
}