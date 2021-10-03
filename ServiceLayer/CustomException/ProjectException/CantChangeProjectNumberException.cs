using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class CantChangeProjectNumberException : Exception
    {
        public CantChangeProjectNumberException()
        {
        }

        public CantChangeProjectNumberException(string message) : base(message)
        {
        }

        public CantChangeProjectNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CantChangeProjectNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
