using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Project> Get3LayerProjectRepo(int id)
        {
            Project project = await _db.Projects
                        .Include(p => p.ProjectTasks)                     // Eager load the related ProjectTasks
                            .ThenInclude(pt => pt.ProjectTaskActivities)  // Eager load the related ProjectTaskActivities within each ProjectTask
                        .SingleOrDefaultAsync(p => p.Id == id);            // Assuming you're fetching by the Project's ID
            return project;
        }
        public async Task UpdateAsync(Project obj)
        {
            _db.Projects.Update(obj);
        }
    }
}