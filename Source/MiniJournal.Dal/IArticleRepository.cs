using System.Collections.Generic;
using Infotecs.MiniJournal.Contracts;

namespace Infotecs.MiniJournal.Dal
{
    public interface IArticleRepository
    {
        IEnumerable<ArticleData> GetArticles();
    }
}
