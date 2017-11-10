using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infotecs.MiniJournal.Dal.Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Infotecs.MiniJournal.Dal
{
    public class SessionFactory
    {
        private static ISessionFactory sessionFactory;

        public static ISessionFactory Build(string connectionString)
        {
            if (sessionFactory == null)
            {
                sessionFactory = Fluently
                    .Configure()
                    .Database(MsSqlConfiguration.MsSql2005.ConnectionString(connectionString))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ArticleMap>())
                    //.ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();
            }
            return sessionFactory;
        }

        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config)
                .Create(false, true);
        }
    }
}