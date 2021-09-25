using DomainLayer;
using PersistenceLayer;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ProjectService : IProjectService
    {
        private readonly ProjectRepo _projectRepo;

        public ProjectService(ProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }
        public IList<Project> GetProjectList() => _projectRepo.GetProjectList();
    }
}
