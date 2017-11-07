using FluentValidation;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Service.Validators
{
    public class ArticleValidator : ChainableValidator<Article>
    {
        private const int maxCaptionLength = 255;

        public ArticleValidator()
        {
            RuleFor(article => article.Caption)
                .NotNull()
                .NotEmpty()
                .WithMessage("Наименование статьи не может быть пустым");

            RuleFor(article => article.Caption)
                .MaximumLength(maxCaptionLength)
                .WithMessage($"Наименование статьи не может превышать {maxCaptionLength} символов");
        }
    }
}
