using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Microsoft.Extensions.Logging;

namespace Gmphan.DataAccessLib.Repository
{
    public class ProjectRepo : Repository<Project>, IProjectRepo
    {
        private readonly AppDbContext _db;
        public ProjectRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}