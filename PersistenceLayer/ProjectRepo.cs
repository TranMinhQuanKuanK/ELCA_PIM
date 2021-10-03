using DomainLayer;
using NHibernate;
using NHibernate.Criterion;
using PersistenceLayer.Helper;
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
        private readonly INHibernateSessionHelper _sessionhelper;

        public ProjectRepo(INHibernateSessionHelper sessionhelper)
        {
            _sessionhelper = sessionhelper;
        }

        public IList<Project> GetProjectList(string searchTerm, string searchStatus)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                IList<Project> result = new List<Project>();
                searchTerm = searchTerm == null ? "" : searchTerm.Trim().ToUpper();
                searchStatus = searchStatus == null ? "" : searchStatus.Trim().ToUpper();


                var criteria = session.CreateCriteria<Project>();
                if (searchTerm != "") 
                    criteria.Add(Expression.Disjunction()
                                          .Add(Expression.Like("Name", $"%{searchTerm}%"))
                                          .Add(Expression.Like(Projections.Cast(NHibernateUtil.String, Projections.Property("ProjectNumber")), $"%{searchTerm}%"))
                                          .Add(Expression.Like("Customer", $"%{searchTerm}%"))
                                        );
                if (searchStatus != "")
                    criteria.Add(Expression.Like("Status", $"%{searchStatus}%"));

                result = criteria.List<Project>();
                return result;
            }
        }

        public Project GetProjectByID(long id)
        {

            using (var session = _sessionhelper.OpenSession())
            {
                Project result = null;

                result = session.QueryOver<Project>().Where(item => item.ID == id).SingleOrDefault<Project>();

                return result;
            }
            //List<Project> projList = new List<Project> {
            //    new Project { ID=1,GroupID = 1, Name="The Morning Ceremony 2332",Customer = "Brigham Malcom" , ProjectNumber=1232, StartDate=new DateTime(2001,11,23),EndDate=new DateTime(2001,2,23),Status="NEW",Version=23},
            //    new Project { ID=2,GroupID = 2, Name="The Coding Awards",Customer = "Mark Clayton" , ProjectNumber=2332, StartDate=new DateTime(2001,7,23),EndDate=new DateTime(2001,3,23),Status="PLA",Version=22},
            //    new Project { ID=3,GroupID = 3, Name="Evening Shindig",Customer = "Cecil Baxter" , ProjectNumber=5523, StartDate=new DateTime(2001,6,23),EndDate=new DateTime(2001,4,23),Status="NEW",Version=1},
            //    new Project { ID=4,GroupID = 4, Name="Associations Now",Customer = "Obadiah Law" , ProjectNumber=6364, StartDate=new DateTime(2001,5,23),EndDate=new DateTime(2001,5,23),Status="FIN",Version=3},
            //    new Project { ID=5,GroupID = 5, Name="Remembering Our Ancestors",Customer = "Tran Minh Quan" , ProjectNumber=1199, StartDate=new DateTime(2001,4,23),EndDate=new DateTime(2001,6,23),Status="INP",Version=24},
            //    new Project { ID=6,GroupID = 6, Name="Project Explained",Customer = "Benjamin Glover" , ProjectNumber=8435, StartDate=new DateTime(2001,3,23),EndDate=new DateTime(2001,7,23),Status="PLA",Version=15},
            //    new Project { ID=7,GroupID = 7, Name="School Leadership 2.0",Customer = "Luke Bourn" , ProjectNumber=8345, StartDate=new DateTime(2001,2,23),EndDate=new DateTime(2001,8,23),Status="NEW",Version=5},
            //};
            //return projList.Where(x => x.ID == id).FirstOrDefault();
        }

        public Project GetProjectByProjectNumber(short projNumber)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                Project result = null;

                result = session.QueryOver<Project>()
                    .Where(item => item.ProjectNumber == projNumber)
                    .SingleOrDefault<Project>();

                return result;
            }
        }

    }
}
