using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Helper
{
    public class NHibernateSessionHelper : INHibernateSessionHelper
    {

        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory != null)
                {
                    return _sessionFactory;
                }
                var cfg = new Configuration();
                cfg.DataBaseIntegration(x =>
                {
                    x.ConnectionStringName = "default";
                    x.Driver<SqlClientDriver>();
                    x.LogSqlInConsole = true;
                    x.Dialect<MsSql2012Dialect>();
                });

                cfg.AddAssembly(Assembly.GetExecutingAssembly());

                _sessionFactory = cfg.BuildSessionFactory();
                return _sessionFactory;
            }
        }

        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
