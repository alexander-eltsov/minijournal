using System;
using System.Collections.Generic;

namespace Infotecs.MiniJournal.Models
{
    public interface IArticleRepository
    {
        IList<Article> GetArticles();

        Article FindArticle(int articleId);

        void CreateArticle(Article article);

        void UpdateArticle(Article article);

        void DeleteArticle(int articleId);
    }
}
