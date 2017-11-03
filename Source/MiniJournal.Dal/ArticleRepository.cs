using System.Collections.Generic;
using System.Linq;
using Dapper;
using Infotecs.MiniJournal.Contracts;

namespace Infotecs.MiniJournal.Dal
{
    public class ArticleRepository : IArticleRepository
    {
        private IConnectionFactory ConnectionFactory { get; }

        public ArticleRepository(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public IEnumerable<ArticleData> GetArticles()
        {
            string sql = "SELECT * FROM Articles";

            using (var connection = ConnectionFactory.Create())
            {
                connection.Open();

                List<ArticleData> articles = connection.Query<ArticleData>(sql).ToList();

                return articles;
            }
        }

        public bool CreateArticle(ArticleData article)
        {
            string sql = "INSERT INTO Articles([Caption], [Text]) VALUES (@Caption, @Text)";

            using (var connection = ConnectionFactory.Create())
            {
                connection.Open();

                var affectedRows = connection.Execute(sql, article);

                return affectedRows > 0;
            }
        }

        public bool UpdateArticle(ArticleData article)
        {
            string sql = "UPDATE Articles SET [Caption] = @Caption, [Text] = @Text WHERE [Id] = @Id";

            using (var connection = ConnectionFactory.Create())
            {
                connection.Open();

                var affectedRows = connection.Execute(sql, article);

                return affectedRows > 0;
            }
        }

        public bool DeleteArticle(ArticleData article)
        {
            string sql = "DELETE FROM Articles WHERE [Id] = @Id";

            using (var connection = ConnectionFactory.Create())
            {
                connection.Open();

                var affectedRows = connection.Execute(sql, article);

                return affectedRows > 0;
            }
        }
    }
}
