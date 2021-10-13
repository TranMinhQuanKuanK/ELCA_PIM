using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class InvalidVisaException : Exception
    {
        public InvalidVisaException()
        {
        }

        public InvalidVisaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
