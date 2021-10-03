using ContractLayer;
using DomainLayer;
using PersistenceLayer;
using PersistenceLayer.Interface;
using ServiceLayer.CustomException.ProjectException;
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
        private readonly IProjectRepo _projectRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IGroupService _groupService;
        private readonly IEmployeeService _employeeService;

        public ProjectService(IProjectRepo projectRepo, IEmployeeRepo employeeRepo, IGroupService groupService, IEmployeeService employeeService)
        {
            _projectRepo = projectRepo;
            _employeeRepo = employeeRepo;
            _groupService = groupService;
            _employeeService = employeeService;
        }

        public IList<ProjectListModel> GetProjectList(string searchTerm, string searchStatus)
        {
            List<ProjectListModel> projectList = new List<ProjectListModel>();
            _projectRepo.GetProjectList(searchTerm, searchStatus).ToList().ForEach(x => projectList.Add(new ProjectListModel
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
        public AddEditProjectModel GetProjectByID(long id)
        {
            var project = _projectRepo.GetProjectByID(id);
            IList<string> empList = _employeeRepo.GetMemberListOfProject(id).Select(x => x.Visa).ToList();
            return project == null ? null : new AddEditProjectModel
            {
                ID = project.ID,
                Customer = project.Customer,
                Name = project.Name,
                ProjectNumber = project.ProjectNumber,
                StartDate = project.StartDate,
                Status = project.Status,
                EndDate = project.EndDate,
                GroupID = project.GroupID,
                Members = "",
                MembersList = (List<string>) empList
            };
        }

        public bool CheckProjectNumberExist(short projectNumber) => _projectRepo.GetProjectByProjectNumber(projectNumber)!=null;

        public bool ValidateProjectModelAndUpdate(AddEditProjectModel project)
        {
            //GroupID exist
            if (_groupService.CheckGroupIDExist((long)project.GroupID) == false)
            {
                throw new GroupIDDoesntExistException();
            }
            //check project number duplicate
            if (GetProjectByID((long)project.ID).ProjectNumber != project.ProjectNumber)
            {
                throw new CantChangeProjectNumberException();
            }
            //check visa exist
            foreach (var member in project.MembersList)
            {
                if (_employeeService.CheckExistVisa(member) == false)
                {
                    throw new InvalidVisaException();
                }
            }
            //check tình trạng
            if (project.Status != "NEW" && project.Status 
                != "PLA" && project.Status != "INP" && project.Status != "FIN")
            {
                throw new InvalidStatusException();
            }
            //check enddate
            if (project.EndDate < project.StartDate)
            {
                throw new EndDateSoonerThanStartDateException();
            }
            //---------------update------------------------


            //-----------------------------------------------
            return true;
        }


        public bool DeleteProject(long id)
        {
            return false;
        }
    }
}
