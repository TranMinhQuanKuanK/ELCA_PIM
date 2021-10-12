using PersistenceLayer;
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
        private readonly GroupRepo _groupRepo;

        public GroupService(GroupRepo groupRepo)
        {
            _groupRepo = groupRepo;
        }
        public bool CheckGroupIdExist(long groupID) => _groupRepo.GetGroupById(groupID) != null;

        public IList<long> GetGroupIdList()
        {
            List<long> result = new List<long>();
            _groupRepo.GetAllGroup().ToList().ForEach(x => result.Add(x.Id));
            return result;
        }
    }
}
