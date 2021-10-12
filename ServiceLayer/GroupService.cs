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
    public class GroupService : IGroupService
    {
        private readonly IGroupRepo _groupRepo;
        private readonly INHibernateSessionHelper _sessionhelper;

        public GroupService(IGroupRepo groupRepo, INHibernateSessionHelper sessionhelper)
        {
            _groupRepo = groupRepo;
            _sessionhelper = sessionhelper;
        }

        public bool CheckGroupIdExist(long groupID)
        {
            using (var session = _sessionhelper.OpenSession())
            {
                return _groupRepo.GetGroupById(groupID, session) != null;
            }
        }

        public IList<long> GetGroupIdList()
        {
            using (var session = _sessionhelper.OpenSession())
            {
                List<long> result = (List<long>)_groupRepo.GetAllGroup(session).Select(x => x.Id).ToList();
                return result;
            }
        }
    }
}
