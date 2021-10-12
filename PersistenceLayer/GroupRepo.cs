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

        public Group GetGroupById(long groupId)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                return session.QueryOver<Group>().Where(gr => gr.Id==groupId).SingleOrDefault<Group>();
            }
        }
        public IList<Group> GetAllGroup()
        {
            using (var session = _sessionhelper.OpenSession())
            {
                return session.QueryOver<Group>().List<Group>();
            }
        }
    }
}
