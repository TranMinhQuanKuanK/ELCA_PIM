using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Helper
{
    public interface INHibernateSessionHelper
    {
        ISession OpenSession();
    }
}
