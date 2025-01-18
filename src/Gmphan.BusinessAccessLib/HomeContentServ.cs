using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.UtilityLib;
using Microsoft.Extensions.Logging;

namespace Gmphan.BusinessAccessLib
{
    public class HomeContentServ : IHomeContentServ
    {
        private readonly ILogger<HomeContentServ> _logger;
        private readonly IUnityOfWork _uow;
        private readonly IGetTAndCacheGeneric _getTAndCacheGeneric;
        public HomeContentServ(ILogger<HomeContentServ> logger
                        , IUnityOfWork uow
                        , IGetTAndCacheGeneric getTAndCacheGeneric)
        {
            _logger = logger;
            _uow = uow;
            _getTAndCacheGeneric = getTAndCacheGeneric;
        }
        public async Task<IEnumerable<HomeContent>> GetHomeContentAsync()
        {
            IEnumerable<HomeContent> homeContents = await _getTAndCacheGeneric.GetTAsync(
                "homeContents", 
                _uow.HomeContentRepoUOW.GetAllAsync
                );
            return homeContents;
        }
    }
}