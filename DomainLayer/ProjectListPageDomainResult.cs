using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class ProjectListPageDomainResult
    {
        public IList<Project> projectList { get; set; }
        public int resultCount { get; set; }
    }
}
