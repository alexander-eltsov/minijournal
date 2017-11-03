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
    }
}
