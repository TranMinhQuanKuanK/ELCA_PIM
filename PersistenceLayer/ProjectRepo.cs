using DomainLayer;
using PersistenceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer
{
    public class ProjectRepo : IProjectRepo
    {
        public IList<Project> GetProjectList()
        {
            List<Project> projList = new List<Project> {
                new Project { ID=1,GroupID = 1, Name="ABC",Customer = "cus" , ProjectNumber=2323, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,2,23),Status="NEW",Version=23},
                new Project { ID=2,GroupID = 2, Name="AB2C",Customer = "cus1" , ProjectNumber=2324, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,2,23),Status="PKA",Version=22},
                new Project { ID=3,GroupID = 3, Name="AB3C",Customer = "cus2" , ProjectNumber=2325, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,2,23),Status="NEW",Version=1},
                new Project { ID=4,GroupID = 4, Name="AB4C",Customer = "cus3" , ProjectNumber=2326, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,2,23),Status="NEW",Version=3},
                new Project { ID=5,GroupID = 5, Name="AB5C",Customer = "cus4" , ProjectNumber=2326, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,2,23),Status="NEW",Version=24},
                new Project { ID=6,GroupID = 6, Name="AB25C",Customer = "cus5" , ProjectNumber=2343, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,2,23),Status="HRw",Version=15},
                new Project { ID=7,GroupID = 7, Name="AB6C",Customer = "cus6" , ProjectNumber=2325, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,2,23),Status="NEW",Version=5},
            };
            return projList;
        }


    }
}
