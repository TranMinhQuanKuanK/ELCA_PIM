using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class Group
    {
        public virtual long Id { get; set; }
        public virtual long GroupLeaderId { get; set; }
        public virtual int Version { get; set; }
    }
}
