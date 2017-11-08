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
                    a.[ID] AS [ArticleID],
                    a.[Caption],
                    a.[Text],
                    c.[ID] AS [CommentID],
                    c.[Text] AS [CommentText],
                    c.[User]
                FROM
                    Articles a
                    LEFT OUTER JOIN Comments c ON a.[ID] = c.[ArticleID]
                WHERE
                    a.[ID] = @Id";

            using (var connection = connectionFactory.Create())
            {
                connection.Open();

                Article fetchedArticle = null;
                fetchedArticle = connection
                    .Query<dynamic>(sql, new { Id = articleId })
                    .Select(item =>
                    {
                        if (fetchedArticle == null)
                        {
                            fetchedArticle = new Article
                            {
                                Id = item.ArticleID,
                                Caption = item.Caption,
                                Text = item.Text
                            };
                        }
                        if (item.CommentID != null)
                        {
                            var comment = new Comment()
                            {
                                Id = item.CommentID,
                                Text = item.CommentText,
                                User = item.User,
                                Article = fetchedArticle
                            };
                            fetchedArticle.Comments.Add(comment);
                        }
                        return fetchedArticle;
                    })
                    .ToList()
                    .FirstOrDefault();

                return fetchedArticle;
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
