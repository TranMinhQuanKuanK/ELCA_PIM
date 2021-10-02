using ContractLayer;
using PersistenceLayer;
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
         private readonly EmployeeRepo _employeeService;

        public EmployeeService(EmployeeRepo employeeService)
        {
            _employeeService = employeeService;
        }
        public List<MemberModel> GetAllMembers()
        {
            List<MemberModel> memberList = new List<MemberModel>();
            _employeeService.GetAllEmployees().ToList().ForEach(x => memberList.Add(new MemberModel
            {
                ID = x.ID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Visa = x.Visa
            }));
            return memberList;
        }
    }
}
