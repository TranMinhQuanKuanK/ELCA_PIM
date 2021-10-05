using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.CustomException.Project
{
    public class ProjectStatusNotNewException : Exception
    {
        public ProjectStatusNotNewException()
        {
        }

        public ProjectStatusNotNewException(string message) : base(message)
        {
        }

        public ProjectStatusNotNewException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProjectStatusNotNewException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
