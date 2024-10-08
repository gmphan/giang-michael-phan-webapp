using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IProjectRepo : IRepository<Project>
    {
        public Task<Project> Get3LayerProjectRepo(int id);
    }
}