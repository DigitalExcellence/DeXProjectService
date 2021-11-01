using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProjectService
    {
        ProjectRepository projectRepository;
        public ProjectService(ProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<Project> GetProjectById(int id)
        {
            return await projectRepository.GetProjectById(id);
        }


        public async Task<List<Project>> GetAllProjects()
        {
            return await projectRepository.GetAllProjects();
        }

        public async Task AddProject(Project project)
        {
           await projectRepository.AddProject(project);
        }


        public async Task UpdateProject(Project project)
        {
            await projectRepository.UpdateProject(project);
        }

        public async Task RemoveProject(int id)
        {
            await projectRepository.RemoveProject(id);
        }
    }
}
