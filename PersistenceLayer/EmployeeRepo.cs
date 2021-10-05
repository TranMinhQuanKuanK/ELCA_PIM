using DomainLayer;
using PersistenceLayer.CustomException.Project;
using PersistenceLayer.Helper;
using PersistenceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly INHibernateSessionHelper _sessionhelper;

        public EmployeeRepo(INHibernateSessionHelper sessionhelper)
        {
            _sessionhelper = sessionhelper;
        }
        public IList<Employee> GetAllEmployees()
        {
            using (var session = _sessionhelper.OpenSession())
            {
                return session.QueryOver<Employee>().List<Employee>();
            }

        }
        public IList<Employee> GetEmployeesBasedOnVisaList(IList<string> visalist)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                //  Employee employeeAlias = null;
                var result = session.QueryOver<Employee>().WhereRestrictionOn(k => k.Visa)
                    .IsIn((List<string>)visalist)
                    .List<Employee>();
                if (result.Count != visalist.Count)
                {
                    throw new InvalidVisaDetectedException();
                }
                else
                {
                    return result;
                }
            }
        }
        public IList<Employee> GetMemberListOfProject(long id)
        {

            using (var session = _sessionhelper.OpenSession())
            {
                Project projectAlias = null;
                Employee memberAlias = null;
                List<Employee> projectList = (List<Employee>)
                session.QueryOver<Employee>(() => memberAlias).JoinAlias(() => memberAlias.Projects, () => projectAlias)
                .Where(() => projectAlias.ID == id).List();
                return projectList;
            }

        }
        public void RemoveEmployeesFromProject(IList<long> employeeIdList, long projectId)
        {

            using (var session = _sessionhelper.OpenSession())
            {
                //Project projectAlias = null;
                //Employee memberAlias = null;
                //List<Project> projectList = (List<Project>)
                //session.QueryOver<Employee>(() => memberAlias).
                //            JoinAlias(
                //                    () => memberAlias.Projects, 
                //                    () => projectAlias
                //                    )
                //.Where(() => projectAlias.ID == id).List();
                //return session.QueryOver<Employee>().List<Employee>();
            }

        }
        public void RemoveProjectsFromEmployee(IList<long> projectIdList, long employeeId)
        {

            using (var session = _sessionhelper.OpenSession())
            {
                //Project projectAlias = null;
                //Employee memberAlias = null;
                //List<Project> projectList = (List<Project>)
                //session.QueryOver<Employee>(() => memberAlias).
                //            JoinAlias(
                //                    () => memberAlias.Projects, 
                //                    () => projectAlias
                //                    )
                //.Where(() => projectAlias.ID == id).List();
                //return session.QueryOver<Employee>().List<Employee>();
            }

        }
        public Employee GetEmployeeByVisa(string visa)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                return session.QueryOver<Employee>().Where(c => c.Visa == visa).SingleOrDefault<Employee>();
            }
        }
    }
}
