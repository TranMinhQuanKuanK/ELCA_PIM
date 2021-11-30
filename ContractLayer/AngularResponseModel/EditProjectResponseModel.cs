using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractLayer.AngularResponseModel
{
    public class EditProjectResponseModel

    {
        public AddEditProjectModel projectModel { get; set; }
        public bool hasError { get; set; }
        public List<String> errorList { get; set; }
    }
}
