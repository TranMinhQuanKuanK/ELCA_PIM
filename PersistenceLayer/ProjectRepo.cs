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
        public IList<Project> GetProjectList(string searchTerm, string searchStatus)
        {
            //logic Nhibernate here
            List<Project> projList = new List<Project> {
                new Project { ID=1,GroupID = 1, Name="The Morning Ceremony 2332",Customer = "Brigham Malcom" , ProjectNumber=1232, StartDate=new DateTime(2001,11,23),EndDate=new DateTime(2001,2,23),Status="NEW",Version=23},
                new Project { ID=2,GroupID = 2, Name="The Coding Awards",Customer = "Mark Clayton" , ProjectNumber=2332, StartDate=new DateTime(2001,7,23),EndDate=new DateTime(2001,3,23),Status="PLA",Version=22},
                new Project { ID=3,GroupID = 3, Name="Evening Shindig",Customer = "Cecil Baxter" , ProjectNumber=5523, StartDate=new DateTime(2001,6,23),EndDate=new DateTime(2001,4,23),Status="NEW",Version=1},
                new Project { ID=4,GroupID = 4, Name="Associations Now",Customer = "Obadiah Law" , ProjectNumber=6364, StartDate=new DateTime(2001,5,23),EndDate=new DateTime(2001,5,23),Status="FIN",Version=3},
                new Project { ID=5,GroupID = 5, Name="Remembering Our Ancestors",Customer = "Tran Minh Quan" , ProjectNumber=1199, StartDate=new DateTime(2001,4,23),EndDate=new DateTime(2001,6,23),Status="INP",Version=24},
                new Project { ID=6,GroupID = 6, Name="Project Explained",Customer = "Benjamin Glover" , ProjectNumber=8435, StartDate=new DateTime(2001,3,23),EndDate=new DateTime(2001,7,23),Status="PLA",Version=15},
                new Project { ID=7,GroupID = 7, Name="School Leadership 2.0",Customer = "Luke Bourn" , ProjectNumber=8345, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,8,23),Status="NEW",Version=5},
            };
            //-----phần dưới không đổi
            List<Project> result = new List<Project>();
            searchTerm = searchTerm == null ? "" : searchTerm.Trim().ToUpper();
            searchStatus = searchStatus == null ? "" : searchStatus.Trim().ToUpper();
            foreach (var item in projList)
            {
                if (
                        (
                        item.Name.ToUpper().Contains(searchTerm)
                        || item.Customer.ToUpper().Contains(searchTerm)
                        || item.ProjectNumber.ToString().Contains(searchTerm)
                        )
                             && item.Status.ToUpper().Contains(searchStatus)
                    )
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public Project GetProjectByID(long id)
        {
            List<Project> projList = new List<Project> {
                new Project { ID=1,GroupID = 1, Name="The Morning Ceremony 2332",Customer = "Brigham Malcom" , ProjectNumber=1232, StartDate=new DateTime(2001,11,23),EndDate=new DateTime(2001,2,23),Status="NEW",Version=23},
                new Project { ID=2,GroupID = 2, Name="The Coding Awards",Customer = "Mark Clayton" , ProjectNumber=2332, StartDate=new DateTime(2001,7,23),EndDate=new DateTime(2001,3,23),Status="PLA",Version=22},
                new Project { ID=3,GroupID = 3, Name="Evening Shindig",Customer = "Cecil Baxter" , ProjectNumber=5523, StartDate=new DateTime(2001,6,23),EndDate=new DateTime(2001,4,23),Status="NEW",Version=1},
                new Project { ID=4,GroupID = 4, Name="Associations Now",Customer = "Obadiah Law" , ProjectNumber=6364, StartDate=new DateTime(2001,5,23),EndDate=new DateTime(2001,5,23),Status="FIN",Version=3},
                new Project { ID=5,GroupID = 5, Name="Remembering Our Ancestors",Customer = "Tran Minh Quan" , ProjectNumber=1199, StartDate=new DateTime(2001,4,23),EndDate=new DateTime(2001,6,23),Status="INP",Version=24},
                new Project { ID=6,GroupID = 6, Name="Project Explained",Customer = "Benjamin Glover" , ProjectNumber=8435, StartDate=new DateTime(2001,3,23),EndDate=new DateTime(2001,7,23),Status="PLA",Version=15},
                new Project { ID=7,GroupID = 7, Name="School Leadership 2.0",Customer = "Luke Bourn" , ProjectNumber=8345, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,8,23),Status="NEW",Version=5},
            };
            return projList.Where(x => x.ID == id).FirstOrDefault();
        }

    }
}
