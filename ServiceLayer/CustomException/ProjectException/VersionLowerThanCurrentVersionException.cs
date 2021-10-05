using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class VersionLowerThanCurrentVersionException : Exception
    {
        public VersionLowerThanCurrentVersionException()
        {
        }

        public VersionLowerThanCurrentVersionException(string message) : base(message)
        {
        }

        public VersionLowerThanCurrentVersionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VersionLowerThanCurrentVersionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
