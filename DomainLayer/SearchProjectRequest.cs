using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class SearchProjectRequest
    {
        public string SearchTerm { get; set; }
        public string SearchStatus { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
