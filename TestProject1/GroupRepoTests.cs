using DomainLayer;
using NHibernate;
using NUnit.Framework;
using PersistenceLayer;
using PersistenceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class GroupRepoTests
    {
        NHibernateSessionHelper helper;
        GroupRepo _groupRepo;
        static Employee emp1;
        static Employee emp2;
        static Employee emp3;
        [OneTimeSetUp]
        public void Setup()
        {
            helper = new NHibernateSessionHelper();
            _groupRepo = new GroupRepo(helper);
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
        public void GetGroupByID_ValidID_ExpectedTrueGroup()
        {
            Group expectedGrp1;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    //-----------------------Setup--------------------------
                    expectedGrp1 = new Group
                    {
                        GroupLeaderId = emp2.Id,
                        Version = 1,
                    };
                    session.Save(expectedGrp1);
                    tx.Commit();
                }
            }
            //-----------------------Test--------------------------
            var actualGrp = _groupRepo.GetGroupById(expectedGrp1.Id);
            Assert.AreEqual(expectedGrp1.Id, actualGrp.Id);
            Assert.AreEqual(expectedGrp1.GroupLeaderId, actualGrp.GroupLeaderId);
            Assert.AreEqual(expectedGrp1.Version, actualGrp.Version);
            //--------------------Cleaning-----------------------------  
        }
        [Test]
        public void GetGroupByID_WrongID_ExpectedNull()
        {
            Group expectedGrp1;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    expectedGrp1 = new Group
                    {
                        GroupLeaderId = emp2.Id,
                        Version = 1,
                    };
                    session.Save(expectedGrp1);
                    tx.Commit();
                }
            }
            var actualGrp = _groupRepo.GetGroupById(expectedGrp1.Id + 999);
            Assert.IsNull(actualGrp);
        }
        [Test]
        public void GetAllGroup_ExpectedValidGroups()
        {
            Group expectedGrp1, expectedGrp2, expectedGrp3;
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    expectedGrp1 = new Group
                    {
                        GroupLeaderId = emp2.Id,
                        Version = 1,
                    };
                     expectedGrp2 = new Group
                     {
                         GroupLeaderId = emp2.Id,
                         Version = 1,
                     };
                    expectedGrp3 = new Group
                    {
                        GroupLeaderId = emp2.Id,
                        Version = 1,
                    };
                    session.Save(expectedGrp1);
                    session.Save(expectedGrp2);
                    session.Save(expectedGrp3);
                    tx.Commit();
                }
            }
            var actualGroupList = _groupRepo.GetAllGroup();
            Assert.AreEqual(3, actualGroupList.Count);
            Assert.AreEqual(expectedGrp1.Id, actualGroupList[0].Id);
            Assert.AreEqual(expectedGrp1.GroupLeaderId, actualGroupList[0].GroupLeaderId);
            Assert.AreEqual(expectedGrp1.Version, actualGroupList[0].Version);

            Assert.AreEqual(expectedGrp2.Id, actualGroupList[1].Id);
            Assert.AreEqual(expectedGrp2.GroupLeaderId, actualGroupList[1].GroupLeaderId);
            Assert.AreEqual(expectedGrp2.Version, actualGroupList[1].Version);

            Assert.AreEqual(expectedGrp3.Id, actualGroupList[2].Id);
            Assert.AreEqual(expectedGrp3.GroupLeaderId, actualGroupList[2].GroupLeaderId);
            Assert.AreEqual(expectedGrp3.Version, actualGroupList[2].Version);
        }
    }
}
