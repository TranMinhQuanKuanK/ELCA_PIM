using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public static class VisaHelper
    {
        public static IList<String> SplitVisa(string visaString)
        {
            if (visaString == null || visaString == string.Empty)
            {
                return new List<string>();
            }
            else
            {
                String[] separator = { "," };
                string removedSpaceVisasString = Regex.Replace(visaString, @"\s+", string.Empty);
                // using the method
                return removedSpaceVisasString.Split(separator,
                       StringSplitOptions.RemoveEmptyEntries);
            }

        }
    }
}
