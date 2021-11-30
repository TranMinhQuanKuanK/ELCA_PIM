using ContractLayer;
using DomainLayer;
using NHibernate;
using PersistenceLayer;
using PersistenceLayer.CustomException.Project;
using PersistenceLayer.Helper;
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
        private readonly INHibernateSessionHelper _sessionhelper;

        public ProjectService(IProjectRepo projectRepo, IEmployeeRepo employeeRepo, 
            IGroupService groupService, INHibernateSessionHelper sessionhelper)
        {
            _projectRepo = projectRepo;
            _employeeRepo = employeeRepo;
            _groupService = groupService;
            _sessionhelper = sessionhelper;
        }

        public ProjectListPageContractResult GetProjectList
            (SearchProjectRequestModel request)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                List<ProjectListModel> projectList = new List<ProjectListModel>();
                SearchProjectRequest requestDomain = new SearchProjectRequest()
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    SearchStatus = request.SearchStatus,
                    SearchTerm = request.SearchTerm,
                };
                ProjectListPageDomainResult result = _projectRepo.GetProjectList(requestDomain, session);
                projectList = result.projectList
                    .Select(x => new ProjectListModel
                    {
                        Id = x.Id,
                        Customer = x.Customer,
                        Name = x.Name,
                        ProjectNumber = x.ProjectNumber,
                        StartDate = x.StartDate,
                        Status = x.Status,
                        Version = x.Version
                    }).ToList();
                return new ProjectListPageContractResult()
                {
                    ProjectList = projectList,
                    ResultCount = result.resultCount
                };
            }
        }
        public AddEditProjectModel GetProjectById(long id)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                var project = _projectRepo.GetProjectById(id, session);
                var empList = _employeeRepo.GetMemberListOfProject(id, session).ToList();
                return project == null ? null : new AddEditProjectModel
                {
                    Id = project.Id,
                    Customer = project.Customer,
                    Name = project.Name,
                    ProjectNumber = project.ProjectNumber,
                    StartDate = project.StartDate,
                    Status = project.Status,
                    EndDate = project.EndDate,
                    GroupId = project.GroupId,
                    MemberString = null,
                    Version = project.Version,
                    MembersList = empList
                };
            }
        }
        public bool CheckProjectNumberExist(short projectNumber)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                return _projectRepo.GetProjectByProjectNumber(projectNumber, session) != null;
            }
        }
        private void CheckGroupID(AddEditProjectModel project)
        {
            if (_groupService.CheckGroupIdExist(project.GroupId.Value) == false)
            {
                throw new GroupIDDoesntExistException();
            }
        }

        private void CheckProjectNumberDuplicate(AddEditProjectModel project)
        {
            var domainProj = GetProjectById(project.Id.Value);
            if (domainProj.ProjectNumber != project.ProjectNumber)
            {
                throw new CantChangeProjectNumberException();
            }
        }
        private IList<Employee> CheckVisaExisted(AddEditProjectModel project)
        {
            try
            {
                using (var session = _sessionhelper.OpenSession())
                {
                    var membersList = _employeeRepo.GetEmployeesBasedOnVisaList(project.MembersList, session);
                    return membersList;
                }
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

        public void Update(AddEditProjectModel project)
        {
            CheckGroupID(project);
            CheckProjectNumberDuplicate(project);
            IList<Employee> membersList = CheckVisaExisted(project);
            CheckStatus(project);
            CheckEndDateSoonerThanStartDate(project);

            try
            {
                using (var session = _sessionhelper.OpenSession())
                {
                    _projectRepo.UpdateProject(new Project()
                    {
                        Id = project.Id.Value,
                        Name = project.Name,
                        Customer = project.Customer,
                        GroupId = project.GroupId.Value,
                        Members = membersList,
                        ProjectNumber = project.ProjectNumber.Value,
                        StartDate = project.StartDate,
                        EndDate = project.EndDate,
                        Status = project.Status,
                        Version = project.Version
                    }, session);
                }
            }
            catch (VersionLowerThanCurrentVersionException e)
            {
                throw new ProjectHaveBeenEditedByAnotherUserException("Version lower than current version", e);
            }
        }
        private void CheckProjectNumberExist(AddEditProjectModel project)
        {
            var session = _sessionhelper.OpenSession();
            if (_projectRepo.GetProjectByProjectNumber(project.ProjectNumber.Value, session) != null)
            {
                throw new ProjectNumberDuplicateException();
            }
        }
        public void Create(AddEditProjectModel project)
        {
            CheckGroupID(project);
            CheckProjectNumberExist(project);
            IList<Employee> membersList = CheckVisaExisted(project);
            CheckStatus(project);
            CheckEndDateSoonerThanStartDate(project);

            using (var session = _sessionhelper.OpenSession())
            {
                _projectRepo.CreateNewProject(new Project()
                {
                    Name = project.Name,
                    Customer = project.Customer,
                    GroupId = project.GroupId.Value,
                    Members = membersList,
                    ProjectNumber = project.ProjectNumber.Value,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status,
                    Version = project.Version
                }, session);
            }
        }
        public void DeleteProject(IList<DeleteProjectRequestModel> projectList)
        {
            IDictionary<long, int> projectListDictionary = new Dictionary<long, int>();
            foreach (var item in projectList)
            {
                projectListDictionary.Add(new KeyValuePair<long, int>(item.Id, item.Version));
            }
            try
            {
                using (var session = _sessionhelper.OpenSession())
                {
                    _projectRepo.DeleteProject(projectListDictionary, session);
                }
            }
            catch (PersistenceLayer.CustomException.Project.ProjectNotExistedException e)
            {
                throw new CustomException.ProjectException.ProjectNotExistedException("ProjectNotExistedException", e);
            }
            catch (CantDeleteProjectDueToLowerVersionException e)
            {
                throw new CantDeleteProjectBecauseProjectHasBeenChangedException("CantDeleteProjectBecauseProjectHasBeenChangedException", e);
            }
            catch (PersistenceLayer.CustomException.Project.ProjectStatusNotNewException e)
            {
                throw new CustomException.ProjectException.ProjectStatusNotNewException("ProjectStatusNotNewException", e);
            }
        }
    }
   
}
