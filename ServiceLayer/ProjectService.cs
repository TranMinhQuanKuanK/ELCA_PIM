using ContractLayer;
using DomainLayer;
using NHibernate;
using PersistenceLayer;
using PersistenceLayer.CustomException.Project;
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

        public ProjectListPageContractResult GetProjectList
            (SearchProjectRequestModel request)
        {
            List<ProjectListModel> projectList = new List<ProjectListModel>();
            SearchProjectRequest requestDomain = new SearchProjectRequest()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SearchStatus = request.SearchStatus,
                SearchTerm = request.SearchTerm,
            };
            ProjectListPageDomainResult result = _projectRepo.GetProjectList(requestDomain);

            result.projectList.ToList().ForEach(x => projectList.Add(new ProjectListModel
            {
                ID = x.ID,
                Customer = x.Customer,
                Name = x.Name,
                ProjectNumber = x.ProjectNumber,
                StartDate = x.StartDate,
                Status = x.Status,
                Version = x.Version
            }));
            return new ProjectListPageContractResult()
            {
                projectList = projectList,
                resultCount = result.resultCount
            };
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
                MemberString  = "",//khong can thiet
                Version = project.Version,
                MembersList = (List<string>)empList
            };
        }
        public bool CheckProjectNumberExist(short projectNumber) => _projectRepo.GetProjectByProjectNumber(projectNumber) != null;
        private void CheckGroupID(AddEditProjectModel project)
        {
            if (_groupService.CheckGroupIDExist((long)project.GroupID) == false)
            {
                throw new GroupIDDoesntExistException();
            }
        }

        private void CheckProjectNumberDuplicate(AddEditProjectModel project)
        {
            if (GetProjectByID((long)project.ID).ProjectNumber != project.ProjectNumber)
            {
                throw new CantChangeProjectNumberException();
            }
        }
        private void CheckVisaExisted(AddEditProjectModel project, out IList<Employee> membersList)
        {
            try
            {
                membersList = _employeeRepo.GetEmployeesBasedOnVisaList(project.MembersList);
            }
            catch (InvalidVisaDetectedException e)
            {
                throw new InvalidVisaException("Invalid visa detected!", e);
            }
        }
        private void CheckStatus(AddEditProjectModel project)
        {
            if (project.Status != "NEW" && project.Status
                 != "PLA" && project.Status != "INP" && project.Status != "FIN")
            {
                throw new InvalidStatusException();
            }
        }
        private void CheckEndDateSoonerThanStartDate(AddEditProjectModel project)
        {
            if (project.EndDate < project.StartDate)
            {
                throw new EndDateSoonerThanStartDateException();
            }
        }

        public bool ValidateProjectModelAndUpdate(AddEditProjectModel project)
        {
            //GroupID exist
            CheckGroupID(project);
            //check project number duplicate
            CheckProjectNumberDuplicate(project);
            //check visa exist
            IList<Employee> membersList = null;
            CheckVisaExisted(project, out membersList);
            //check tình trạng
            CheckStatus(project);
            //check enddate
            CheckEndDateSoonerThanStartDate(project);

            try
            {
                _projectRepo.UpdateProject(new Project()
                {
                    ID = (long)project.ID,
                    Name = project.Name,
                    Customer = project.Customer,
                    GroupID = (long)project.GroupID,
                    Members = membersList,
                    ProjectNumber = (short)project.ProjectNumber,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status,
                    Version = project.Version
                }); ;
            }
            catch (PersistenceLayer.CustomException.Project.VersionLowerThanCurrentVersionException e)
            {
                throw new CustomException.ProjectException.VersionLowerThanCurrentVersionException("Version lower than current version", e);
            }
            return true;
        }
        private void CheckProjectNumberExist(AddEditProjectModel project)
        {
            if (_projectRepo.GetProjectByProjectNumber((short)project.ProjectNumber) != null)
            {
                throw new ProjectNumberDuplicateException();
            }
        }
        public bool ValidateAndCreateNewProject(AddEditProjectModel project)
        {
            //GroupID exist
            CheckGroupID(project);
            //check project number duplicate
            CheckProjectNumberExist(project);
            //check visa exist
            IList<Employee> membersList = null;
            CheckVisaExisted(project, out membersList);
            //check tình trạng
            CheckStatus(project);
            //check enddate
            CheckEndDateSoonerThanStartDate(project);
            try
            {
                _projectRepo.CreateNewProject(new Project()
                {
                    Name = project.Name,
                    Customer = project.Customer,
                    GroupID = (long)project.GroupID,
                    Members = membersList,
                    ProjectNumber = (short)project.ProjectNumber,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status,
                    Version = project.Version
                });
            }
            catch (PersistenceLayer.CustomException.Project.VersionLowerThanCurrentVersionException e)
            {
                throw new CustomException.ProjectException.VersionLowerThanCurrentVersionException("Version lower than current version", e);
            }
            return true;
        }
        public bool DeleteProject(IList<DeleteProjectRequestModel> projectList)
        {
            IDictionary<long, int> projectListDictionary = new Dictionary<long, int>();
            foreach (var item in projectList)
            {
                projectListDictionary.Add(new KeyValuePair<long, int>(item.Id, item.Version));
            }
            try
            {
                _projectRepo.DeleteProject(projectListDictionary);
            }
            catch (PersistenceLayer.CustomException.Project.ProjectNotExistedException e)
            {
                throw new CustomException.ProjectException.ProjectNotExistedException("ProjectNotExistedException", e);
            }
            catch (PersistenceLayer.CustomException.Project.CantDeleteProjectDueToLowerVersionException e)
            {
                throw new CustomException.ProjectException.CantDeleteProjectDueToLowerVersionException("CantDeleteProjectDueToLowerVersionException", e);
            }
            catch (PersistenceLayer.CustomException.Project.ProjectStatusNotNewException e)
            {
                throw new CustomException.ProjectException.ProjectStatusNotNewException("ProjectStatusNotNewException", e);
            }
            return false;
        }
    }
}
