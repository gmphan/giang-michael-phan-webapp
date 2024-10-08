using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.BusinessAccessLib
{
    public interface IProjectServ
    {
        public Task AddNewProjectServAsync(Project obj);
        public Task<IEnumerable<Project>> GetAllProjectServAsync();
    }
}