using DomainLayer;
using NHibernate;
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
        public Group GetGroupById(long groupId, ISession session)
        {
            return session.Get<Group>(groupId);
        }
        public IList<Group> GetAllGroup(ISession session)
        {
            return session.QueryOver<Group>()
                .List<Group>();
        }
    }
}
