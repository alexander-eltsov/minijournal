using System.Collections.Generic;
using Infotecs.MiniJournal.Contracts;

namespace Infotecs.MiniJournal.Dal
{
    public interface IArticleRepository
    {
        IEnumerable<ArticleData> GetArticles();

        bool CreateArticle(ArticleData article);

        bool UpdateArticle(ArticleData article);

        bool DeleteArticle(ArticleData article);
    }
}
