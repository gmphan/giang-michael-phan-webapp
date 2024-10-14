using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IProjectTaskRepo : IRepository<ProjectTask>
    {
        public Task<ProjectTask> GetProjectTaskDetailRepo(int id);
        public Task UpdateAsync(ProjectTask obj);
    }
}