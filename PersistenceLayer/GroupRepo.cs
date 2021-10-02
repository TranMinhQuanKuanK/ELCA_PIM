using DomainLayer;
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
        public IList<Group> GetAllGroup() {
            return new List<Group> {
                new Group
            {
               ID = 1,GroupLeaderID = 4,Version=1
            },
            new Group
            {
               ID = 2,GroupLeaderID = 7,Version=1
            },   new Group
            {
               ID = 3,GroupLeaderID = 4,Version=1
            },
            new Group
            {
               ID = 4,GroupLeaderID = 7,Version=1
            },   new Group
            {
               ID = 5,GroupLeaderID = 4,Version=1
            },
            new Group
            {
               ID = 6,GroupLeaderID = 7,Version=1
            },   new Group
            {
               ID = 7,GroupLeaderID = 4,Version=1
            },
            new Group
            {
               ID = 888888,GroupLeaderID = 7,Version=1
            },
            };
        }
    }
}
