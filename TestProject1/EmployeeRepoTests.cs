﻿using DomainLayer;
using NHibernate;
using NUnit.Framework;
using PersistenceLayer;
using PersistenceLayer.CustomException.Project;
using PersistenceLayer.Helper;
using System.Collections.Generic;
using Utilities;

namespace TestProject1
{
    public class EmployeeRepoTests
    {
        NHibernateSessionHelper helper;
        EmployeeRepo _empRepo;
        private Employee emp1;
        private Employee emp2;
        private Employee emp3;
        private Group grp1;
        private Group grp2;
        private Project proj1;
        [OneTimeSetUp]
        public void Setup()
        {
            helper = new NHibernateSessionHelper();
            _empRepo = new EmployeeRepo();
            using (ISession session = helper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    emp1 = new Employee
                    {
                        Visa = "TMT",
                        FirstName = "Tran Minh",
                        LastName = "Tuan",
                        Birthday = new System.DateTime(2001, 1, 1),
                        Version = 1
                    };
                    emp2 = new Employee
                    {
                        Visa = "NTH",
                        FirstName = "Nguyen Thi",
                        LastName = "Ha",
                        Birthday = new System.DateTime(2001, 1, 1),
                        Version = 1
                    };
                    emp3 = new Employee
                    {
                        Visa = "HND",
                        FirstName = "Hong Nhat",
                        LastName = "Duong",
                        Birthday = new System.DateTime(2001, 1, 1),
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

                    proj1 = new Project()
                    {
                        GroupId = grp1.Id,
                        Customer = "Customer Test",
                        Name = "Project Test",
                        ProjectNumber = 1234,
                        StartDate = new System.DateTime(2012, 1, 1),
                        Status = "NEW",
                        Version = 1,
                        Members = new List<Employee>() {
                           emp1,emp2
                        }
                    };
                    session.Save(proj1);
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
        public void GetAllEmployees_ExpectedTrueEmployeeList()
        {
            using (ISession session = helper.OpenSession())
            {
                var actualtEmpList = _empRepo.GetAllEmployees(session);
                Assert.IsTrue(actualtEmpList.Contains(emp1));
                Assert.IsTrue(actualtEmpList.Contains(emp2));
                Assert.IsTrue(actualtEmpList.Contains(emp3));
            }
        }
        [Test]
        public void GetEmployeesBasedOnVisaList_TrueVisaList_ExpectedTrueEmployeeList()
        {
            using (ISession session = helper.OpenSession())
            {
                var actualtEmpList = _empRepo.GetEmployeesBasedOnVisaList(new List<string>()
            {
                "TMT","NTH"
            }, session);
                Assert.IsTrue(actualtEmpList.Count == 2);
                Assert.IsTrue(actualtEmpList.Contains(emp1));
                Assert.IsTrue(actualtEmpList.Contains(emp2));
                var actualtEmpList2 = _empRepo.GetEmployeesBasedOnVisaList(new List<string>()
            {
                "TMT"
            }, session);
                Assert.IsTrue(actualtEmpList2.Count == 1);
                Assert.IsTrue(actualtEmpList2.Contains(emp1));
                var actualtEmpList3 = _empRepo.GetEmployeesBasedOnVisaList(new List<string>(), session);
                Assert.IsTrue(actualtEmpList3.Count == 0);
            }
        }
        [Test]
        public void GetEmployeesBasedOnVisaList_WrongVisaList_ExpectedException()
        {
            using (ISession session = helper.OpenSession())
            {
                Assert.Throws<InvalidVisaDetectedException>
                (() => _empRepo.GetEmployeesBasedOnVisaList(new List<string>() { "TMT", "HHH" }, session));

                Assert.Throws<InvalidVisaDetectedException>
                   (() => _empRepo.GetEmployeesBasedOnVisaList(new List<string>() { "ABC", "NTH" }, session));
            }
        }
        [Test]
        public void GetMemberListOfProject_TrueID_ExpectedTrueEmployeeList()
        {
            using (ISession session = helper.OpenSession())
            {
                var actualtEmpList = _empRepo.GetMemberListOfProject(proj1.Id, session);
                Assert.IsTrue(actualtEmpList.Count == 2);
                Assert.IsTrue(actualtEmpList.Contains(emp1.Visa));
                Assert.IsTrue(actualtEmpList.Contains(emp2.Visa));
            }
        }
        [Test]
        public void GetEmployeeByVisa_TrueVisa_ExpectedTrueEmployee()
        {
            using (ISession session = helper.OpenSession())
            {
                var actualtEmp = _empRepo.GetEmployeeByVisa("HND", session);
                Assert.AreEqual(emp3.Id, actualtEmp.Id);
                Assert.AreEqual(emp3.Visa, actualtEmp.Visa);
                Assert.AreEqual(emp3.FirstName, actualtEmp.FirstName);
                Assert.AreEqual(emp3.LastName, actualtEmp.LastName);
                Assert.AreEqual(emp3.Birthday, actualtEmp.Birthday);
                Assert.AreEqual(emp3.Version, actualtEmp.Version);
            }
        }
        [Test]
        public void GetEmployeeByVisa_WrongVisa_ExpectedNull()
        {
            using (ISession session = helper.OpenSession())
            {
                Assert.IsNull(_empRepo.GetEmployeeByVisa("ABC", session));
            }
        }
    }
}