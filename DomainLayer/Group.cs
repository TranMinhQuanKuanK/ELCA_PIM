using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    class Group
    {
        public long ID { get; set; }
        public long GroupLeaderID { get; set; }
        public int Version { get; set; }
    }
}
