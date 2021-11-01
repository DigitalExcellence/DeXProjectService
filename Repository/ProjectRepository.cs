using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProjectRepository
    {
        readonly Context context;

        public ProjectRepository(Context context)
        {
            this.context = context;
        }
        public async Task<Project> GetProjectById(int id)
        {
            return await context.Project.FindAsync(id);
        }


        public async Task<List<Project>> GetAllProjects()
        {
            return await context.Project.ToListAsync();
        }


        public async Task AddProject(Project project)
        {
            await context.Project.AddAsync(project);
            await context.SaveChangesAsync();
        }


        public async Task UpdateProject(Project project)
        {
            context.Project.Update(project);
            await context.SaveChangesAsync();
        }

        public async Task RemoveProject(int id)
        {
            Project project = context.Project.Find(id);
            context.Project.Remove(project);

            await context.SaveChangesAsync();
        }
    }
}
