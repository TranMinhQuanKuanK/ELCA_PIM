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
        public static IList<String> SplitVisa(string visaList)
        {
            if (visaList == null || visaList == "")
            {
                return new List<string>();
            }
            else
            {
                String[] spearator = { "," };
                visaList = Regex.Replace(visaList, @"\s+", "");
                // using the method
                return visaList.Split(spearator,
                       StringSplitOptions.RemoveEmptyEntries);
            }

        }
    }
}
