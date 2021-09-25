using ContractLayer;
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
        public IList<ProjectListModel> GetProjectList()
        {
            List<ProjectListModel> projectList = new List<ProjectListModel>();
            _projectRepo.GetProjectList().ToList().ForEach(x => projectList.Add(new ProjectListModel
            {
                ID = x.ID,
                Customer = x.Customer,
                Name = x.Name,
                ProjectNumber = x.ProjectNumber,
                StartDate = x.StartDate,
                Status = x.Status
            }));
            return projectList;
        }

    }
}
