using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IGroupService
    {
        IList<long> GetGroupIdList();
        bool CheckGroupIdExist(long groupID);
    }
}
