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
                var cfg = new Configuration();

                cfg.DataBaseIntegration(x =>
                {
                    x.ConnectionString = "Data Source=DESKTOP-V4RQDKU;Initial Catalog=ELCA_PIM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    x.Driver<SqlClientDriver>();
                    x.LogSqlInConsole = true;
                    x.Dialect<MsSql2012Dialect>();
                });

                //cfg.AddFile("Mapping/Product.hbm.xml").AddFile("Mapping/Category.hbm.xml");

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
