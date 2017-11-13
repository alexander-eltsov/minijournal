using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Infotecs.MiniJournal.Models;
using LanguageExt;
using NHibernate;
using NHibernate.Util;

namespace Infotecs.MiniJournal.Dal
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ISessionFactory sessionFactory;

        public ArticleRepository(string connectionString)
        {
            this.sessionFactory = SessionFactory.Build(connectionString);
        }

        public IList<Header> GetHeaders()
        {
            using (var session = sessionFactory.OpenSession())
            {
                IList<Header> headers = session
                    .CreateCriteria(typeof(Header))
                    .List<Header>();

                return headers;
            }
        }

        public Option<Article> GetArticle(int articleId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session
                    .QueryOver<Article>()
                    .Fetch(x => x.Comments).Eager
                    .Where(x => x.Id == articleId)
                    //.Where(x => x.Id == 99999)
                    .List()
                    .ToOption();
            }
        }

        public void CreateArticle(Article article)
        {
            using (var session = sessionFactory.OpenSession())
            {
                session.Save(article);
                session.Flush();
            }
        }

        public void UpdateArticle(Article article)
        {
            using (var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(article);
                session.Flush();
            }
        }

        public void DeleteArticle(int articleId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var article = session.Load<Article>(articleId);
                session.Delete(article);
                session.Flush();
            }
        }

        public void CreateArticleComment(Comment comment)
        {
            using (var session = sessionFactory.OpenSession())
            {
                session.Save(comment);
                session.Flush();
            }
        }

        public void DeleteArticleComment(int articleId, int commentId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var comment = session.Load<Comment>(commentId);
                session.Delete(comment);
                session.Flush();
            }
        }
    }
}