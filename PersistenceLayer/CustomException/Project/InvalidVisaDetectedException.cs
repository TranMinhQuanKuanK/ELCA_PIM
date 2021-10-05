using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.CustomException.Project
{
    public class InvalidVisaDetectedException : Exception
    {
        public InvalidVisaDetectedException()
        {
        }

        public InvalidVisaDetectedException(string message) : base(message)
        {
        }

        public InvalidVisaDetectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidVisaDetectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
