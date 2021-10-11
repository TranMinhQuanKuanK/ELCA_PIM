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
        static Employee emp1;
        static Employee emp2;
        static Employee emp3;
        static Group grp1;
        static Group grp2;
        [OneTimeSetUp]
        public void Setup()
        {
            helper = new NHibernateSessionHelper();
            _proRepo = new ProjectRepo(helper);
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
                        GroupLeaderID = emp1.ID,
                        Version = 1,
                    };
                    grp2 = new Group
                    {
                        GroupLeaderID = emp2.ID,
                        Version = 1,
                    };
                    session.Save(grp1);
                    session.Save(grp2);
                    tx.Commit();
                }
            }
        }
        [OneTimeTearDown]
        public void TearDown()
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj2 = new Project()
                    {
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Unique 2",
                        ProjectNumber = 2,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj3 = new Project()
                    {
                        GroupID = grp2.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Unique 2",
                        ProjectNumber = 3,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj4 = new Project()
                    {
                        GroupID = grp2.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 2",
                        ProjectNumber = 4,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj5 = new Project()
                    {
                        GroupID = grp2.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 5",
                        ProjectNumber = 5,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                    };
                    expectedProj6 = new Project()
                    {
                        GroupID = grp2.ID,
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
            }
            //Test
            var actualProj1 = _proRepo.GetProjectList(new SearchProjectRequest()
            {
                SearchTerm="Unique",
                PageIndex = 1,
                PageSize = 3
            });
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
                        GroupID = grp1.ID,
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
            }
            var actualProj1 = _proRepo.GetProjectByID(expectedProj1.ID);
            Assert.AreEqual(expectedProj1.ProjectNumber, actualProj1.ProjectNumber);
            Assert.AreEqual(expectedProj1.Name, actualProj1.Name);
            Assert.AreEqual(expectedProj1.StartDate, actualProj1.StartDate);
            Assert.AreEqual(expectedProj1.EndDate, actualProj1.EndDate);
            Assert.AreEqual(expectedProj1.Status, actualProj1.Status);
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
                        GroupID = grp1.ID,
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
            }
            var actualProj1 = _proRepo.GetProjectByID(-1);
            var actualProj2 = _proRepo.GetProjectByID(proj.ID + 100);
            Assert.IsNull(actualProj1);
            Assert.IsNull(actualProj2);
        }
        [Test]
        public void GetProjectByProjectNumber_ValidGroupID_ExpectedTrueProject()
        {
            Project expectedProj1;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    expectedProj1 = new Project()
                    {
                        GroupID = grp1.ID,
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
            }
            var actualProj1 = _proRepo.GetProjectByProjectNumber(6979);
            Assert.AreEqual(expectedProj1.ProjectNumber, actualProj1.ProjectNumber);
            Assert.AreEqual(expectedProj1.Name, actualProj1.Name);
            Assert.AreEqual(expectedProj1.StartDate, actualProj1.StartDate);
            Assert.AreEqual(expectedProj1.EndDate, actualProj1.EndDate);
            Assert.AreEqual(expectedProj1.Status, actualProj1.Status);
        }
        [Test]
        public void GetProjectByProjectNumber_WrongGroupID_ExpectedNullt()
        {
            Project expectedProj1;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    expectedProj1 = new Project()
                    {
                        GroupID = grp1.ID,
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
            }
            var actualProj1 = _proRepo.GetProjectByProjectNumber(1111);
            var actualProj2 = _proRepo.GetProjectByProjectNumber(-1);
            Assert.IsNull(actualProj1);
            Assert.IsNull(actualProj2);
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1112,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
            }
            Project toUpdateProj = new Project()
            {
                ID = proj.ID,
                GroupID = grp1.ID,
                Customer = "Customer Test Updated",
                Name = "Project Test 1",
                ProjectNumber = 1112,
                StartDate = new System.DateTime(2012, 1, 1),
                Status = "NEW",
                Version = proj.Version,
            };
            _proRepo.UpdateProject(toUpdateProj);
            var actualUpdatedProject = _proRepo.GetProjectByID(proj.ID);
            Assert.AreEqual(toUpdateProj.ProjectNumber, actualUpdatedProject.ProjectNumber);
            Assert.AreEqual(toUpdateProj.Name, actualUpdatedProject.Name);
            Assert.AreEqual(toUpdateProj.StartDate, actualUpdatedProject.StartDate);
            Assert.AreEqual(toUpdateProj.EndDate, actualUpdatedProject.EndDate);
            Assert.AreEqual(toUpdateProj.Status, actualUpdatedProject.Status);
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1113,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
            }
            Project toUpdateProj = new Project()
            {
                ID = proj.ID,
                GroupID = grp1.ID,
                Customer = "Customer Test Updated",
                Name = "Project Test 1",
                ProjectNumber = 1113,
                StartDate = new System.DateTime(2012, 1, 1),
                Status = "NEW",
                Version = proj.Version - 1,
            };

            Assert.Throws<VersionLowerThanCurrentVersionException>
                (() => _proRepo.UpdateProject(toUpdateProj));

        }
        [Test]
        public void CreateNewProject_ValidProject_ExpectedDataIsUpdated()
        {

            Project newProj = new Project()
            {
                GroupID = grp2.ID,
                Customer = "Customer Test 2",
                Name = "Project Test 2",
                ProjectNumber = 1213,
                StartDate = new System.DateTime(2012, 1, 1),
                Status = "NEW",
            };
            _proRepo.CreateNewProject(newProj);
            var actualCreatedProject = _proRepo.GetProjectByProjectNumber(1213);
            Assert.AreEqual(newProj.ProjectNumber, actualCreatedProject.ProjectNumber);
            Assert.AreEqual(newProj.Name, actualCreatedProject.Name);
            Assert.AreEqual(newProj.StartDate, actualCreatedProject.StartDate);
            Assert.AreEqual(newProj.EndDate, actualCreatedProject.EndDate);
            Assert.AreEqual(newProj.Status, actualCreatedProject.Status);
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 4544,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
            }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.ID, proj.Version);
            _proRepo.DeleteProject(deleteProjectList);
            var deletedProject = _proRepo.GetProjectByID(proj.ID);
            Assert.IsNull(deletedProject);
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 4549,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
            }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.ID, proj.Version - 1);
            Assert.Throws<CantDeleteProjectDueToLowerVersionException>
               (() => _proRepo.DeleteProject(deleteProjectList));
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 7291,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "PLA",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
            }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.ID, proj.Version);
            Assert.Throws<ProjectStatusNotNewException>
               (() => _proRepo.DeleteProject(deleteProjectList));
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 4241,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "PLA",
                    };
                    session.Save(proj);
                    tx.Commit();
                }
            }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.ID+999, proj.Version);
            Assert.Throws<ProjectNotExistedException>
               (() => _proRepo.DeleteProject(deleteProjectList));
        }
        [Test]
        public void DeleteProject_ValidMultipleProject_ExpectedProjectsAreDeleted()
        {
            Project proj, proj2,proj3;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    proj = new Project()
                    {
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1291,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj2 = new Project()
                    {
                        GroupID = grp2.ID,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 1209,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                     proj3 = new Project()
                     {
                         GroupID = grp1.ID,
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
            }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.ID, proj.Version);
            deleteProjectList.Add(proj2.ID, proj2.Version);
            deleteProjectList.Add(proj3.ID, proj3.Version);
            _proRepo.DeleteProject(deleteProjectList);
            var deletedProject1 = _proRepo.GetProjectByID(proj.ID);
            var deletedProject2 = _proRepo.GetProjectByID(proj2.ID);
            var deletedProject3 = _proRepo.GetProjectByID(proj3.ID);
            Assert.IsNull(deletedProject1);
            Assert.IsNull(deletedProject2);
            Assert.IsNull(deletedProject3);
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 1411,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj2 = new Project()
                    {
                        GroupID = grp2.ID,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 1412,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj3 = new Project()
                    {
                        GroupID = grp1.ID,
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
            }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.ID, proj.Version - 1);
            deleteProjectList.Add(proj2.ID, proj2.Version);
            deleteProjectList.Add(proj3.ID, proj3.Version );
            Assert.Throws<CantDeleteProjectDueToLowerVersionException>
               (() => _proRepo.DeleteProject(deleteProjectList));
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 5411,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj2 = new Project()
                    {
                        GroupID = grp2.ID,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 5412,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj3 = new Project()
                    {
                        GroupID = grp1.ID,
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
            }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.ID, proj.Version);
            deleteProjectList.Add(proj2.ID, proj2.Version);
            deleteProjectList.Add(proj3.ID, proj3.Version);
            Assert.Throws<ProjectStatusNotNewException>
               (() => _proRepo.DeleteProject(deleteProjectList));
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
                        GroupID = grp1.ID,
                        Customer = "Customer Test 1",
                        Name = "Project Test 1",
                        ProjectNumber = 5492,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj2 = new Project()
                    {
                        GroupID = grp2.ID,
                        Customer = "Customer Test 2",
                        Name = "Project Test 1",
                        ProjectNumber = 5912,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                    };
                    proj3 = new Project()
                    {
                        GroupID = grp1.ID,
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
            }
            var deleteProjectList = new Dictionary<long, int>();
            deleteProjectList.Add(proj.ID, proj.Version);
            deleteProjectList.Add(proj2.ID + 999, proj2.Version);
            deleteProjectList.Add(proj3.ID, proj3.Version);
            Assert.Throws<ProjectNotExistedException>
               (() => _proRepo.DeleteProject(deleteProjectList));
        }
    }
}