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
         private readonly EmployeeRepo _employeeRepo;

        public EmployeeService(EmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        public List<MemberModel> GetAllMembers()
        {
            List<MemberModel> memberList = new List<MemberModel>();
            _employeeRepo.GetAllEmployees().ToList().ForEach(x => memberList.Add(new MemberModel
            {
                ID = x.ID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Visa = x.Visa
            }));
            return memberList;
        }

        public bool CheckExistVisa(string visa)
        {
            return _employeeRepo.GetEmployeeByVisa(visa)!=null;
        }
    }
}
