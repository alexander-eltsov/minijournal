using System.Collections.Generic;
using System.Linq;
using Dapper;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Dal
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IConnectionFactory connectionFactory;

        public ArticleRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IList<Article> GetArticles()
        {
            string sql = "SELECT * FROM Articles";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                List<Article> articles = connection.Query<Article>(sql).ToList();

                return articles;
            }
        }

        public void CreateArticle(Article article)
        {
            string sql = "INSERT INTO Articles([Caption], [Text]) VALUES (@Caption, @Text)";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                connection.Execute(sql, article);
            }
        }

        public void UpdateArticle(Article article)
        {
            string sql = "UPDATE Articles SET [Caption] = @Caption, [Text] = @Text WHERE [Id] = @Id";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                connection.Execute(sql, article);
            }
        }

        public void DeleteArticle(int articleId)
        {
            string sql = "DELETE FROM Articles WHERE [Id] = @Id";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                connection.Execute(sql, new { Id = articleId});
            }
        }
    }
}
