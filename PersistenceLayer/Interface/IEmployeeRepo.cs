using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Interface
{
    public interface IEmployeeRepo
    {
        IList<Employee> GetAllEmployees();
        Employee GetEmployeeByVisa(string visa);
        IList<Employee> GetMemberListOfProject(long id);
    }
}
