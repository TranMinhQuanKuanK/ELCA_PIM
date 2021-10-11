using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities
{
    public static class CultureHelper
    {
        private static readonly List<string> _validCultures = new List<string> { "en-US", "vi-VN" };

        public static string GetImplementedCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return GetDefaultCulture();
            }

            if (_validCultures.Any(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return GetDefaultCulture();
            }

            if (_validCultures.Any(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return name;
            }

            return GetDefaultCulture();
        }

        public static string GetDefaultCulture()
        {
            return _validCultures[0];
        }
    }
}
