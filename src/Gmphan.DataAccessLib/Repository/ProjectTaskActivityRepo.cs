using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public class ProjectTaskActivityRepo : Repository<ProjectTaskActivity>, IProjectTaskActivityRepo
    {
        private readonly AppDbContext _db;
        public ProjectTaskActivityRepo(AppDbContext db): base(db)
        {
            _db = db;
        }
    }
}