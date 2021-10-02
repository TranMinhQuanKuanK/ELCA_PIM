using DomainLayer;
using PersistenceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer
{
    public class EmployeeRepo: IEmployeeRepo
    {
        public IList<Employee> GetAllEmployees()
        {
            return new List<Employee> {
                new Employee
            {
               ID = 1,FirstName = "Tran Minh",LastName = "Quan",Visa="TMQ",Birthday= DateTime.Now,Version=1
            }, 
                new Employee
            {
                ID = 2,FirstName = "Nguyen Thi",LastName = "Huong",Visa="NTH",Birthday= DateTime.Now,Version=1
            }, new Employee
            {
                 ID = 3,FirstName = "Le Nguyen Ai",LastName = "Quoc",Visa="QLN",Birthday= DateTime.Now,Version=1
            }, new Employee
            {
               ID = 4,FirstName = "Hoang Phuoc",LastName = "Thanh",Visa="HPT",Birthday= DateTime.Now,Version=1
            }, new Employee
            {
                 ID = 5,FirstName = "Luu Duc",LastName = "Hung",Visa="LDH",Birthday= DateTime.Now,Version=1
            },new Employee
            {
                 ID = 6,FirstName = "Nguyen Thi",LastName = "Linh",Visa="TDH",Birthday= DateTime.Now,Version=1
            },new Employee
            {
                 ID = 7,FirstName = "Tran Van",LastName = "Ba",Visa="TVB",Birthday= DateTime.Now,Version=1
            },
            };
        }
    }
}
