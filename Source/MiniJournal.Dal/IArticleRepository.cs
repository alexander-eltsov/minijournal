using System.Collections.Generic;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Dal
{
    public interface IArticleRepository
    {
        IList<Article> GetArticles();

        void CreateArticle(Article article);

        void UpdateArticle(Article article);

        void DeleteArticle(int articleId);
    }
}
