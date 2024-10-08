using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;

namespace Gmphan.BusinessAccessLib
{
    public interface IProjectServ
    {
        public Task AddNewProjectServAsync(Project obj);
        public Task<IEnumerable<Project>> GetAllProjectServAsync();
        public Task<Project3LayerView> Get3LayerProjectServAsync(int id);
    }
}