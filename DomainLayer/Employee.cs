using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class Employee
    {
        public virtual long ID { get; set; }
        public virtual string Visa { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual int Version { get; set; }
        public virtual IList<Project> Projects { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Employee employee &&
                   ID == employee.ID;
        }

        public override int GetHashCode()
        {
            return 1213502048 + ID.GetHashCode();
        }
    }
}
