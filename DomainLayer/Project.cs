using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class Project
    {
        public virtual long ID { get; set; }
        public virtual long GroupID { get; set; }
        public virtual short ProjectNumber { get; set; }
        public virtual string Name { get; set; }
        public virtual string Customer { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual  int Version { get; set; }
        public virtual IList<Employee> Members { get; set; }
    }
}
