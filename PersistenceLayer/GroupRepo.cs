using DomainLayer;
using PersistenceLayer.Helper;
using PersistenceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer
{
    public class GroupRepo : IGroupRepo
    {
        private readonly INHibernateSessionHelper _sessionhelper;

        public GroupRepo(INHibernateSessionHelper sessionhelper)
        {
            _sessionhelper = sessionhelper;
        }

        public Group GetGroupByID(long groupID)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                return session.QueryOver<Group>().Where(gr => gr.ID==groupID).SingleOrDefault<Group>();
            }
        }
        public IList<Group> GetAllGroup()
        {
            using (var session = _sessionhelper.OpenSession())
            {
                return session.QueryOver<Group>().List<Group>();
            }
            //return new List<Group> {
            //    new Group
            //{
            //   ID = 1,GroupLeaderID = 4,Version=1
            //},
            //new Group
            //{
            //   ID = 2,GroupLeaderID = 7,Version=1
            //},   new Group
            //{
            //   ID = 3,GroupLeaderID = 4,Version=1
            //},
            //new Group
            //{
            //   ID = 4,GroupLeaderID = 7,Version=1
            //},   new Group
            //{
            //   ID = 5,GroupLeaderID = 4,Version=1
            //},
            //new Group
            //{
            //   ID = 6,GroupLeaderID = 7,Version=1
            //},   new Group
            //{
            //   ID = 7,GroupLeaderID = 4,Version=1
            //},
            //new Group
            //{
            //   ID = 8 ,GroupLeaderID = 7,Version=1
            //},  new Group
            //{
            //   ID = 9 ,GroupLeaderID = 7,Version=1
            //},  new Group
            //{
            //   ID = 10 ,GroupLeaderID = 7,Version=1
            //},  new Group
            //{
            //   ID = 11 ,GroupLeaderID = 7,Version=1
            //},  new Group
            //{
            //   ID = 12 ,GroupLeaderID = 7,Version=1
            //},
            //};
        }
    }
}
