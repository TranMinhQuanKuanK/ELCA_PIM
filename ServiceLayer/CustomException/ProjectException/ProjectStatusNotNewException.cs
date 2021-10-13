using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class ProjectStatusNotNewException : Exception
    {
        public ProjectStatusNotNewException()
        {
        }

        public ProjectStatusNotNewException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
