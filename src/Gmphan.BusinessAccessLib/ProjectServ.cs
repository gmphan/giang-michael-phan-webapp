using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.UtilityLib;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Gmphan.BusinessAccessLib
{
    public class ProjectServ : IProjectServ
    {
        private readonly ILogger<ProjectServ> _logger;
        private readonly IUnityOfWork _unityOfWork;
        private readonly IGetTAndCacheGeneric _getTAndCacheGeneric;
        public ProjectServ(ILogger<ProjectServ> logger
                        , IUnityOfWork unityOfWork
                        , IGetTAndCacheGeneric getTAndCacheGeneric)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
            _getTAndCacheGeneric = getTAndCacheGeneric;
        }
        public async Task AddNewProjectServAsync(Project obj)
        {
            await _unityOfWork.ProjectRepoUOW.AddAsync(obj);
            await _unityOfWork.SaveAsync();
            // will add UpdateCache here.
        }
        public async Task<IEnumerable<Project>> GetAllProjectServAsync()
        {
            return await _getTAndCacheGeneric.GetTAsync("project", _unityOfWork.ProjectRepoUOW.GetAllAsync);
        }
    }
}