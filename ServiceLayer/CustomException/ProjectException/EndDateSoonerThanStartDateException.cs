using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class EndDateSoonerThanStartDateException : Exception
    {
        public EndDateSoonerThanStartDateException()
        {
        }

        public EndDateSoonerThanStartDateException(string message) : base(message)
        {
        }

        public EndDateSoonerThanStartDateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EndDateSoonerThanStartDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
