using System.Collections.Generic;
using System.Data;
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

        public IList<Header> GetHeaders()
        {
            string sql = "SELECT [ID], [Caption] FROM Articles";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                List<Header> headers = connection.Query<Header>(sql).ToList();

                return headers;
            }
        }

        public Article GetArticle(int articleId)
        {
            string sql = @"
                SELECT
                    [ID],
                    [Caption],
                    [Text]
                FROM
                    Articles
                WHERE
                    [ID] = @ArticleId;

                SELECT
                    [ID],
                    [Text],
                    [User]
                FROM
                    Comments
                WHERE
                    [ArticleID] = @ArticleID;";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                using (var multi = connection.QueryMultiple(sql, new { ArticleID = articleId }))
                {
                    Article article = multi.Read<Article>().First();
                    IEnumerable<Comment> comments = multi.Read<Comment>();
                    article.Comments.AddRange(comments);

                    return article;
                }
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

        public void UpdateArticle(Article article, bool updateComments = true)
        {
            string sql = @"
                UPDATE
                    Articles
                SET
                    [Caption] = @Caption,
                    [Text] = @Text
                WHERE
                    [ID] = @Id";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                connection.Execute(sql, article);

                if (updateComments)
                {
                    UpdateArticleComments(connection, article);
                }

                connection.Close();
            }
        }

        public void DeleteArticle(int articleId)
        {
            string sql = "DELETE FROM Articles WHERE [ID] = @Id";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                connection.Execute(sql, new { Id = articleId});
            }
        }

        private void UpdateArticleComments(IDbConnection connection, Article article)
        {
            // delete comments
            string deleteCommentsSql = @"
                DELETE FROM
                    Comments
                WHERE
                    [ArticleID] = @ArticleId";
            int[] existingCommentIds = article.Comments
                .Where(comment => comment.Id != 0)
                .Select(comment => comment.Id)
                .ToArray();
            if (existingCommentIds.Length > 0)
            {
                deleteCommentsSql += $" AND [ID] NOT IN ({ string.Join(",", existingCommentIds)})";
            }
            connection.Execute(deleteCommentsSql, new { ArticleId = article.Id });

            // create comments
            IEnumerable<Comment> newComments = article.Comments.Where(comment => comment.Id == 0);
            foreach (Comment newComment in newComments)
            {
                CreateArticleComment(connection, article, newComment);
            }
        }

        private void CreateArticleComment(IDbConnection connection, Article article, Comment comment)
        {
            string sql = @"
                INSERT INTO
                    Comments ([User], [Text], [ArticleID])
                VALUES
                    (@User, @Text, @ArticleID)";
            connection.Execute(sql,
                new
                {
                    User = comment.User,
                    Text = comment.Text,
                    ArticleID = article.Id
                });
        }
    }
}
