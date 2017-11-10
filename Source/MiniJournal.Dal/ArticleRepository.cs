using System.Collections.Generic;
using System.Linq;
using Infotecs.MiniJournal.Models;
using NHibernate;

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

        public Article GetArticle(int articleId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                Article article = session
                    .QueryOver<Article>()
                    .Fetch(x => x.Comments).Eager
                    .Where(x => x.Id == articleId)
                    .List()
                    .FirstOrDefault();

                return article;
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
                var article = session.Load<Comment>(commentId);
                session.Delete(article);
                session.Flush();
            }
        }
    }
}