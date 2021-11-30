using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractLayer.AngularResponseModel
{
    public class ProjectListResponseModel

    {
        public List<ProjectListModel> ProjectList { get; set; }
        public int ResultCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
