using ContractLayer;
using DomainLayer;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using PersistenceLayer;
using PersistenceLayer.Helper;
using PersistenceLayer.Interface;
using ServiceLayer;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class ProjectServiceTests
    {
        private IProjectRepo _projectRepo;
        private IEmployeeRepo _employeeRepo;
        private IGroupService _groupService;
        private INHibernateSessionHelper _sessionhelper;
        private ProjectService _projectService;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
            _projectRepo = Substitute.For<IProjectRepo>();
            _employeeRepo = Substitute.For<IEmployeeRepo>();
            _groupService = Substitute.For<IGroupService>();
            _sessionhelper = Substitute.For<INHibernateSessionHelper>();
            _projectService = new ProjectService(_projectRepo, _employeeRepo, _groupService, _sessionhelper);

        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }
        private bool AssertProjectAndProjectListModel(Project projectDomain, ProjectListModel projectContract)
        {
            return (
                projectContract.Id == projectDomain.Id &&
                projectContract.Name == projectDomain.Name &&
                projectContract.ProjectNumber == projectDomain.ProjectNumber &&
                projectContract.StartDate == projectDomain.StartDate &&
                projectContract.Status == projectDomain.Status &&
                projectContract.Version == projectDomain.Version
                );

        }
        [Test]
        public void GetProjectList_ExpectedTrueProjectList()
        {
            //Arrange
            var proj1 = new Project()
            {
                Id = 3,
                GroupId = 3,
                Customer = "Customer Test 1",
                Name = "ELCA Project Test 1",
                ProjectNumber = 1234,
                StartDate = new System.DateTime(2012, 1, 1),
                Status = "NEW",
                Version = 1,
            };
            var proj2 = new Project()
            {
                Id = 4,
                GroupId = 3,
                Customer = "Customer Test 2",
                Name = "ELCA Project Test 2",
                ProjectNumber = 1235,
                StartDate = new System.DateTime(2012, 1, 1),
                Status = "NEW",
                Version = 1,
            };
            _projectRepo
                .GetProjectList(Arg.Is<SearchProjectRequest>(
                    x =>
                        x.SearchTerm == "ELCA" &&
                        x.SearchStatus == "NEW" &&
                        x.PageSize == 5 &&
                        x.PageIndex == 2
                    ), Arg.Any<ISession>())
                .Returns(new ProjectListPageDomainResult()
                {
                    projectList = new List<Project> { proj1, proj2 },
                    resultCount = 7
                });
            //Assert
            var searchRequest = new SearchProjectRequestModel()
            {
                SearchTerm = "ELCA",
                SearchStatus = "NEW",
                PageSize = 5,
                PageIndex = 2
            };
            _projectService = new ProjectService(_projectRepo, _employeeRepo, _groupService, _sessionhelper);
            var projectList = _projectService.GetProjectList(searchRequest);
            Assert.AreEqual(7, projectList.ResultCount);
            Assert.IsTrue(AssertProjectAndProjectListModel(proj1, projectList.ProjectList[0]));
            Assert.IsTrue(AssertProjectAndProjectListModel(proj2, projectList.ProjectList[1]));

        }
        private bool AssertProjectAndAddEditProjectModel(Project projectDomain, AddEditProjectModel projectContract)
        {
            return (
                projectContract.Id == projectDomain.Id &&
                projectContract.Name == projectDomain.Name &&
                projectContract.ProjectNumber == projectDomain.ProjectNumber &&
                projectContract.StartDate == projectDomain.StartDate &&
                projectContract.Status == projectDomain.Status &&
                projectContract.Version == projectDomain.Version
                );

        }
        [Test]
        public void GetProjectById_TrueId_ExpectedTrueProjectList()
        {
            //Arrange
            var expectedProj = new Project()
            {
                Id = 3,
                GroupId = 3,
                Customer = "Customer Test 1",
                Name = "ELCA Project Test 1",
                ProjectNumber = 1234,
                StartDate = new System.DateTime(2012, 1, 1),
                Status = "NEW",
                Version = 1,
            };

            _projectRepo
                .GetProjectById(3, Arg.Any<ISession>())
                .Returns(new Project
                {
                    Id = 3,
                    GroupId = 3,
                    Customer = "Customer Test 1",
                    Name = "ELCA Project Test 1",
                    ProjectNumber = 1234,
                    StartDate = new System.DateTime(2012, 1, 1),
                    Status = "NEW",
                    Version = 1,
                }
                );
            _employeeRepo
                .GetMemberListOfProject(3, Arg.Any<ISession>())
                .Returns(
                new List<string>
                {
                    "ABC","XYZ"
                }
                );
            //Assert

            var actualProject = _projectService.GetProjectById(3);
            Assert.IsTrue(AssertProjectAndAddEditProjectModel(expectedProj, actualProject));
            Assert.AreEqual("ABC", actualProject.MembersList[0]);
            Assert.AreEqual("XYZ", actualProject.MembersList[1]);

        }
        [Test]
        public void CheckProjectNumberExist_ExpectedTrueValue()
        {
            //Arrange
            _projectRepo
                .GetProjectByProjectNumber(1234, Arg.Any<ISession>())
                .Returns(new Project());
            _projectRepo
                 .GetProjectByProjectNumber(1235, Arg.Any<ISession>())
                 .Returns(x => null);
            //Assert

            Assert.IsTrue(_projectService.CheckProjectNumberExist(1234) == true);
            Assert.IsTrue(_projectService.CheckProjectNumberExist(1235) == false);
        }
        [Test]
        public void Update_ValidProject_ExpectedProjectUpdated()
        {
            //Arrange
            _projectRepo
                .GetProjectByProjectNumber(1234, Arg.Any<ISession>())
                .Returns(new Project());
            _projectRepo
                 .GetProjectByProjectNumber(1235, Arg.Any<ISession>())
                 .Returns(x => null);
            //Assert

            Assert.IsTrue(_projectService.CheckProjectNumberExist(1234) == true);
            Assert.IsTrue(_projectService.CheckProjectNumberExist(1235) == false);
        }

    }
}
