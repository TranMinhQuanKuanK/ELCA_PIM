using ContractLayer;
using PersistenceLayer;
using PersistenceLayer.Helper;
using PersistenceLayer.Interface;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly INHibernateSessionHelper _sessionhelper;

        public EmployeeService(IEmployeeRepo employeeRepo, INHibernateSessionHelper sessionhelper)
        {
            _employeeRepo = employeeRepo;
            _sessionhelper = sessionhelper;
        }

        public List<MemberModel> GetAllMembers()
        {
            using (var session = _sessionhelper.OpenSession())
            {
                List<MemberModel> memberList = (List<MemberModel>)_employeeRepo
                    .GetAllEmployees(session)
                    .Select(x => new MemberModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Visa = x.Visa
                    }).ToList();
                return memberList;
            }
        }
    }
}
