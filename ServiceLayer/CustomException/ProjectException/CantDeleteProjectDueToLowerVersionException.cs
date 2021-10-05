using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class CantDeleteProjectDueToLowerVersionException : Exception
    {
        public CantDeleteProjectDueToLowerVersionException()
        {
        }

        public CantDeleteProjectDueToLowerVersionException(string message) : base(message)
        {
        }

        public CantDeleteProjectDueToLowerVersionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CantDeleteProjectDueToLowerVersionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
