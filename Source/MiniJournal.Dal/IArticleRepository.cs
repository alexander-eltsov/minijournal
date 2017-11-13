using System;
using System.Collections.Generic;
using Infotecs.MiniJournal.Models;
using LanguageExt;

namespace Infotecs.MiniJournal.Dal
{
    public interface IArticleRepository
    {
        IList<Header> GetHeaders();

        Option<Article> GetArticle(int articleId);

        void CreateArticle(Article article);

        void UpdateArticle(Article article);

        void DeleteArticle(int articleId);

        void CreateArticleComment(Comment comment);

        void DeleteArticleComment(int articleId, int commentId);
    }
}
