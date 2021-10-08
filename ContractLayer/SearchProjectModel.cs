using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractLayer
{
    public class SearchProjectModel
    {
        public string SearchTerm { get; set; }
        public string SearchStatus { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
