using System;
using System.Collections.Generic;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Dal
{
    public interface IArticleRepository
    {
        IList<Header> GetHeaders();

        Article GetArticle(int articleId);

        void CreateArticle(Article article);

        void UpdateArticle(Article article);

        void DeleteArticle(int articleId);
    }
}
