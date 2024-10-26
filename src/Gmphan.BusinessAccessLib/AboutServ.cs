using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Microsoft.Extensions.Logging;

namespace Gmphan.BusinessAccessLib
{
    public class AboutServ : IAboutServ
    {
        private readonly ILogger<AboutServ> _logger;
        private readonly IUnityOfWork _uow;
        public AboutServ(ILogger<AboutServ> logger
                        , IUnityOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }
        public async Task SaveContactMeServAsync(ContactMe obj)
        {
            obj.UpdatedDate = DateTime.UtcNow;
            try
            {
                await _uow.ContactMeRepoUOW.AddAsync(obj);
                await _uow.SaveAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}