using System;
using System.Linq;
using FluentValidation;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Service.Validators
{
    public class ArticleIsUniqueValidator : ChainableValidator<Article>
    {
        private readonly IArticleRepository articleRepository;

        public ArticleIsUniqueValidator(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;

            RuleFor(article => article)
                .Must(article => IsUniqueArticle(article))
                .WithMessage("Статья с таким именем уже существует в БД");
        }

        private bool IsUniqueArticle(Article validatingArticle)
        {
            bool captionIsOccupied = articleRepository
                .GetHeaders()
                .Any(article => article.Id != validatingArticle.Id &&
                    article.Caption.Equals(validatingArticle.Caption, StringComparison.CurrentCultureIgnoreCase));
            return !captionIsOccupied;
        }
    }
}
