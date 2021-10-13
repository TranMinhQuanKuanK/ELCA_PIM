using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractLayer
{
    public class ProjectListPageContractResult
    {
        public IList<ProjectListModel> ProjectList { get; set; }
        public int ResultCount { get; set; }
    }
}
