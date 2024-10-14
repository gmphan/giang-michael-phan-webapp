using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Microsoft.EntityFrameworkCore;

namespace Gmphan.DataAccessLib.Repository
{
    public class ProjectTaskRepo : Repository<ProjectTask>, IProjectTaskRepo
    {
        private readonly AppDbContext _db;
        public ProjectTaskRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        
        public async Task<ProjectTask> GetProjectTaskDetailRepo(int id)
        {
            ProjectTask projectTask = await _db.ProjectTasks
                        .Include(pt => pt.ProjectTaskActivities)  // Eager load the related ProjectTaskActivities within each ProjectTask
                        .SingleOrDefaultAsync(p => p.Id == id);            // Assuming you're fetching by the Project's ID
            return projectTask;
        }
        public async Task UpdateAsync(ProjectTask projectTask)
        {
            _db.ProjectTasks.Update(projectTask);
        }
    }
}