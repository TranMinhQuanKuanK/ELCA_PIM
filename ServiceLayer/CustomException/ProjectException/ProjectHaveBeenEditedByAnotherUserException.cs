using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class ProjectHaveBeenEditedByAnotherUserException : Exception
    {
        public ProjectHaveBeenEditedByAnotherUserException()
        {
        }
        public ProjectHaveBeenEditedByAnotherUserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
