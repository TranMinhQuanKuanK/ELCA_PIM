using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class Group
    {
        public virtual long ID { get; set; }
        public virtual long GroupLeaderID { get; set; }
        public virtual int Version { get; set; }
    }
}
