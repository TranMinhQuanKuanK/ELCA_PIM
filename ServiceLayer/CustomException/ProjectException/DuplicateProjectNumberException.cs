using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class DuplicateProjectNumberException : Exception
    {
        public DuplicateProjectNumberException()
        {
        }

        public DuplicateProjectNumberException(string message) : base(message)
        {
        }

        public DuplicateProjectNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateProjectNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
