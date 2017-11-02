using System.Collections.Generic;
using System.Linq;
using Dapper;
using Infotecs.MiniJournal.Contracts;

namespace Infotecs.MiniJournal.Dal
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ConnectionFactory connectionFactory;

        public ArticleRepository()
        {
            connectionFactory = new ConnectionFactory();
        }

        public IEnumerable<ArticleData> GetArticles()
        {
            string sql = "SELECT * FROM Articles";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                List<ArticleData> articles = connection.Query<ArticleData>(sql).ToList();

                return articles;
            }

        }
    }
}
