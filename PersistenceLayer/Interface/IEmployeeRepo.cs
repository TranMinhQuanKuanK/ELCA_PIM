using DomainLayer;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Interface
{
    public interface IEmployeeRepo
    {
        IList<Employee> GetAllEmployees(ISession session);
        Employee GetEmployeeByVisa(string visa, ISession session);
        IList<string> GetMemberListOfProject(long id, ISession session);
        IList<Employee> GetEmployeesBasedOnVisaList(IList<string> visalist, ISession session);
    }
}
