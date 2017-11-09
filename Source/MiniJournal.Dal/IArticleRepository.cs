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

        void UpdateArticle(Article article, bool updateComments = true);

        void DeleteArticle(int articleId);

        void CreateArticleComment(Comment comment);

        void DeleteArticleComment(int articleId, int commentId);
    }
}
