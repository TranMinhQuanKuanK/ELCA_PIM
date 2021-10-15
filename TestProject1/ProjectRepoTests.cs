using DomainLayer;
using NHibernate;
using NUnit.Framework;
using PersistenceLayer;
using PersistenceLayer.CustomException.Project;
using PersistenceLayer.Helper;
using System.Collections.Generic;
using Utilities;

namespace TestProject1
{
    public class ProjectRepoTests
    {
        NHibernateSessionHelper helper;
        ProjectRepo _proRepo;
        private Employee emp1;
        private Employee emp2;
        private Employee emp3;
        private Group grp1;
        private Group grp2;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            helper = new NHibernateSessionHelper();
            _proRepo = new ProjectRepo();
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    emp1 = new Employee
                    {
                        Visa = "TMT",
                        FirstName = "Tran Minh",
                        LastName = "Tuan",
                        Birthday = System.DateTime.Now,
                        Version = 1
                    };
                    emp2 = new Employee
                    {
                        Visa = "NTH",
                        FirstName = "Nguyen Thi",
                        LastName = "Ha",
                        Birthday = System.DateTime.Now,
                        Version = 1
                    };
                    emp3 = new Employee
                    {
                        Visa = "HND",
                        FirstName = "Hong Nhat",
                        LastName = "Dương",
                        Birthday = System.DateTime.Now,
                        Version = 1
                    };
                    session.Save(emp1);
                    session.Save(emp2);
                    session.Save(emp3);
                    grp1 = new Group
                    {
                        GroupLeaderId = emp1.Id,
                        Version = 1,
                    };
                    grp2 = new Group
                    {
                        GroupLeaderId = emp2.Id,
                        Version = 1,
                    };
                    session.Save(grp1);
                    session.Save(grp2);
                    tx.Commit();
                }
            }
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            using (ISession session = helper.OpenSession())
            {

                using (var tx = session.BeginTransaction())
                {
                    //delete all 
                    session.CreateSQLQuery("DELETE FROM [PROJECT_EMPLOYEE]").ExecuteUpdate();
                    //delete all project
                    session.CreateSQLQuery("DELETE FROM [PROJECT]").ExecuteUpdate();
                    //delete all group
                    session.CreateSQLQuery("DELETE FROM [GROUP]").ExecuteUpdate();
                    //delete all employee
                    session.CreateSQLQuery("DELETE FROM [EMPLOYEE]").ExecuteUpdate();
                    tx.Commit();
                }
            }
        }
        [Test]
        public void GetProjectList_ExpectedTrueProjectList()
        {
            Project expectedProj1, expectedProj2, expectedProj3, expectedProj4, expectedProj5, expectedProj6;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    expectedProj1 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj2 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Unique 2",
                        ProjectNumber = 2,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj3 = new Project()
                    {
                        GroupId = grp2.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Unique 2",
                        ProjectNumber = 3,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj4 = new Project()
                    {
                        GroupId = grp2.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 2",
                        ProjectNumber = 4,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj5 = new Project()
                    {
                        GroupId = grp2.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 5",
                        ProjectNumber = 5,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj6 = new Project()
                    {
                        GroupId = grp2.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 999",
                        ProjectNumber = 6,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    session.Save(expectedProj1);
                    session.Save(expectedProj2);
                    session.Save(expectedProj4);
                    session.Save(expectedProj5);
                    session.Save(expectedProj6);
                    session.Save(expectedProj3);
                    tx.Commit();
                }
                //Test
                var actualProj1 = _proRepo.GetProjectList(new SearchProjectRequest()
                {
                    SearchTerm = "Unique",
                    PageIndex = 1,
                    PageSize = 3
                }, session);
                Assert.IsTrue(actualProj1.resultCount == 2);
                Assert.IsTrue(actualProj1.projectList.Count == 2);
                Assert.AreEqual(expectedProj3.ProjectNumber, actualProj1.projectList[1].ProjectNumber);
                Assert.AreEqual(expectedProj3.Name, actualProj1.projectList[1].Name);
                Assert.AreEqual(expectedProj3.StartDate, actualProj1.projectList[1].StartDate);
                Assert.AreEqual(expectedProj3.EndDate, actualProj1.projectList[1].EndDate);
                Assert.AreEqual(expectedProj3.Status, actualProj1.projectList[1].Status);

                Assert.AreEqual(expectedProj2.ProjectNumber, actualProj1.projectList[0].ProjectNumber);
                Assert.AreEqual(expectedProj2.Name, actualProj1.projectList[0].Name);
                Assert.AreEqual(expectedProj2.StartDate, actualProj1.projectList[0].StartDate);
                Assert.AreEqual(expectedProj2.EndDate, actualProj1.projectList[0].EndDate);
                Assert.AreEqual(expectedProj2.Status, actualProj1.projectList[0].Status);
            }
        }
        [Test]
        public void GetProjectByID_ValidID_ExpectedTrueProject()
        {
            Project expectedProj1;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    expectedProj1 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1234,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    session.Save(expectedProj1);
                    tx.Commit();
                }

                var actualProj1 = _proRepo.GetProjectById(expectedProj1.Id, session);
                Assert.AreEqual(expectedProj1.ProjectNumber, actualProj1.ProjectNumber);
                Assert.AreEqual(expectedProj1.Name, actualProj1.Name);
                Assert.AreEqual(expectedProj1.StartDate, actualProj1.StartDate);
                Assert.AreEqual(expectedProj1.EndDate, actualProj1.EndDate);
                Assert.AreEqual(expectedProj1.Status, actualProj1.Status);
            }
        }
        [Test]
        public void GetProjectByID_WrongID_ExpectedNull()
        {
            Project proj;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1235,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    session.Save(proj);
                    tx.Commit();
                }
                var actualProj1 = _proRepo.GetProjectById(-1, session);
                var actualProj2 = _proRepo.GetProjectById(proj.Id + 100, session);
                Assert.IsNull(actualProj1);
                Assert.IsNull(actualProj2);
            }
        }
        [Test]
        public void GetProjectByProjectNumber_ValidProjectNumber_ExpectedTrueProject()
        {
            Project expectedProj1;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    expectedProj1 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 6979,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    session.Save(expectedProj1);
                    tx.Commit();
                }
            var actualProj1 = _proRepo.GetProjectByProjectNumber(6979,session);
            Assert.AreEqual(expectedProj1.ProjectNumber, actualProj1.ProjectNumber);
            Assert.AreEqual(expectedProj1.Name, actualProj1.Name);
            Assert.AreEqual(expectedProj1.StartDate, actualProj1.StartDate);
            Assert.AreEqual(expectedProj1.EndDate, actualProj1.EndDate);
            Assert.AreEqual(expectedProj1.Status, actualProj1.Status);
            }
        }
        [Test]
        public void GetProjectByProjectNumber_WrongProjectNumber_ExpectedNull()
        {
            Project expectedProj1;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    expectedProj1 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 7823,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    session.Save(expectedProj1);
                    tx.Commit();
                }
            var actualProj1 = _proRepo.GetProjectByProjectNumber(1111,session);
            var actualProj2 = _proRepo.GetProjectByProjectNumber(-1,session);
            Assert.IsNull(actualProj1);
            Assert.IsNull(actualProj2);
            }

        }
        [Test]
        public void UpdateProject_ValidProject_ExpectedDataIsUpdated()
        {
            Project proj;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1112,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 10
                    };
                    session.Save(proj);
                    tx.Commit();
                }
                Project toUpdateProj = new Project()
                {
                    Id = proj.Id,
                    GroupId = grp1.Id,
                    Customer = "Customer Test Updated",
                    Name = "Project Test 1",
                    ProjectNumber = 1119,
                    StartDate = new System.DateTime(2012, 1, 1),
                    Status = "NEW",
                };
                _proRepo.UpdateProject(toUpdateProj, session);
                var actualUpdatedProject = _proRepo.GetProjectById(proj.Id, session);
                Assert.AreEqual(toUpdateProj.ProjectNumber, actualUpdatedProject.ProjectNumber);
                Assert.AreEqual(toUpdateProj.Name, actualUpdatedProject.Name);
                Assert.AreEqual(toUpdateProj.StartDate, actualUpdatedProject.StartDate);
                Assert.AreEqual(toUpdateProj.EndDate, actualUpdatedProject.EndDate);
                Assert.AreEqual(toUpdateProj.Status, actualUpdatedProject.Status);
                Assert.AreEqual(toUpdateProj.Status, actualUpdatedProject.Status);
                Assert.AreEqual(11, actualUpdatedProject.Version);
            }

        }
        [Test]
        public void UpdateProject_VersionLowerProject_ExpectedException()
        {
            Project proj;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1113,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
                Project toUpdateProj = new Project()
                {
                    Id = proj.Id,
                    GroupId = grp1.Id,
                    Customer = "Customer Test Updated",
                    Name = "Project Test 1",
                    ProjectNumber = 1115,
                    StartDate = new System.DateTime(2012, 1, 1),
                    Status = "NEW",
                    Version = proj.Version - 1,
                };

                Assert.Throws<VersionLowerThanCurrentVersionException>
                    (() => _proRepo.UpdateProject(toUpdateProj, session));
            }

        }
        [Test]
        public void CreateNewProject_ValidProject_ExpectedDataIsUpdated()
        {
            using (ISession session = helper.OpenSession())
            {
                Project newProj = new Project()
                {
                    GroupId = grp2.Id,
                    Customer = "Customer Test 2",
                    Name = "Project Test 2",
                    ProjectNumber = 1213,
                    StartDate = new System.DateTime(2012, 1, 1),
                    Status = "NEW",
                };
                _proRepo.CreateNewProject(newProj, session);
                var actualCreatedProject = _proRepo.GetProjectByProjectNumber(1213, session);
                Assert.AreEqual(newProj.ProjectNumber, actualCreatedProject.ProjectNumber);
                Assert.AreEqual(newProj.Name, actualCreatedProject.Name);
                Assert.AreEqual(newProj.StartDate, actualCreatedProject.StartDate);
                Assert.AreEqual(newProj.EndDate, actualCreatedProject.EndDate);
                Assert.AreEqual(newProj.Status, actualCreatedProject.Status);
            }
        }
        [Test]
        public void DeleteProject_ValidSingleProject_ExpectedProjectIsDeleted()
        {
            Project proj;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 4544,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
                var deleteProjectList = new Dictionary<long, int>();
                deleteProjectList.Add(proj.Id, proj.Version);
                _proRepo.DeleteProject(deleteProjectList, session);
                var deletedProject = _proRepo.GetProjectById(proj.Id, session);
                Assert.IsNull(deletedProject);
            }

        }
        [Test]
        public void DeleteProject_LowerVersionSingleProject_ExpectedException()
        {
            Project proj;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 4549,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
                var deleteProjectList = new Dictionary<long, int>();
                deleteProjectList.Add(proj.Id, proj.Version - 1);
                Assert.Throws<CantDeleteProjectDueToLowerVersionException>
               (() => _proRepo.DeleteProject(deleteProjectList, session));
            }
        }
        [Test]
        public void DeleteProject_WrongStatusSingleProject_ExpectedException()
        {
            Project proj;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 7291,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "PLA",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
                var deleteProjectList = new Dictionary<long, int>();
                deleteProjectList.Add(proj.Id, proj.Version);
                Assert.Throws<ProjectStatusNotNewException>
                   (() => _proRepo.DeleteProject(deleteProjectList, session));
            }

        }
        [Test]
        public void DeleteProject_DoesntExistSingleProject_ExpectedException()
        {
            Project proj;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 4241,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "PLA",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.Id + 999, proj.Version);
            Assert.Throws<ProjectNotExistedException>
               (() => _proRepo.DeleteProject(deleteProjectList,session));
            }

        }
        [Test]
        public void DeleteProject_ValidMultipleProject_ExpectedProjectsAreDeleted()
        {
            Project proj, proj2, proj3;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1291,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj2 = new Project()
                    {
                        GroupId = grp2.Id,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 1209,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj3 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 1978,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    session.Save(proj2);
                    session.Save(proj3);
                    tx.Commit();
                }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.Id, proj.Version);
            deleteProjectList.Add(proj2.Id, proj2.Version);
            deleteProjectList.Add(proj3.Id, proj3.Version);
            _proRepo.DeleteProject(deleteProjectList,session);
            var deletedProject1 = _proRepo.GetProjectById(proj.Id,session);
            var deletedProject2 = _proRepo.GetProjectById(proj2.Id,session);
            var deletedProject3 = _proRepo.GetProjectById(proj3.Id,session);
            Assert.IsNull(deletedProject1);
            Assert.IsNull(deletedProject2);
            Assert.IsNull(deletedProject3);
            }

        }
        [Test]
        public void DeleteProject_LowerVersionMultipleProject_ExpectedException()
        {
            Project proj, proj2, proj3;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1411,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj2 = new Project()
                    {
                        GroupId = grp2.Id,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 1412,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj3 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 1413,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    session.Save(proj2);
                    session.Save(proj3);
                    tx.Commit();
                }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.Id, proj.Version - 1);
            deleteProjectList.Add(proj2.Id, proj2.Version);
            deleteProjectList.Add(proj3.Id, proj3.Version);
            Assert.Throws<CantDeleteProjectDueToLowerVersionException>
               (() => _proRepo.DeleteProject(deleteProjectList,session));
            }

        }
        [Test]
        public void DeleteProject_WrongStatusMultipleProject_ExpectedException()
        {
            Project proj, proj2, proj3;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 5411,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj2 = new Project()
                    {
                        GroupId = grp2.Id,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 5412,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj3 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 5413,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "PLA",
                    };
                    session.Save(proj);
                    session.Save(proj2);
                    session.Save(proj3);
                    tx.Commit();
                }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.Id, proj.Version);
            deleteProjectList.Add(proj2.Id, proj2.Version);
            deleteProjectList.Add(proj3.Id, proj3.Version);
            Assert.Throws<ProjectStatusNotNewException>
               (() => _proRepo.DeleteProject(deleteProjectList,session));
            }

        }
        [Test]
        public void DeleteProject_DoesntExistMultipleProject_ExpectedException()
        {
            Project proj, proj2, proj3;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 5492,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj2 = new Project()
                    {
                        GroupId = grp2.Id,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 5912,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj3 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 9413,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "PLA",
                    };
                    session.Save(proj);
                    session.Save(proj2);
                    session.Save(proj3);
                    tx.Commit();
                }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.Id, proj.Version);
            deleteProjectList.Add(proj2.Id + 999, proj2.Version);
            deleteProjectList.Add(proj3.Id, proj3.Version);
            Assert.Throws<ProjectNotExistedException>
               (() => _proRepo.DeleteProject(deleteProjectList,session));
            }
        }
    }
}