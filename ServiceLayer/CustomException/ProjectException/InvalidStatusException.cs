using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class InvalidStatusException : Exception
    {
        public InvalidStatusException()
        {
        }

        public InvalidStatusException(string message) : base(message)
        {
        }

        public InvalidStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStatusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
