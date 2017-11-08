using FluentValidation;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Service.Validators
{
    public class CommentValidator : ChainableValidator<Comment>
    {
        private const int maxUserLength = 128;
        private const int maxTextLength = 512;

        public CommentValidator()
        {
            RuleFor(comment => comment.User)
                .NotNull()
                .NotEmpty()
                .WithMessage("Имя пользователя не может быть пустым");

            RuleFor(article => article.User)
                .MaximumLength(maxUserLength)
                .WithMessage($"Имя пользователя не может превышать {maxUserLength} символов");

            RuleFor(comment => comment.Text)
                .NotNull()
                .NotEmpty()
                .WithMessage("Комментарий не может быть пустым");

            RuleFor(article => article.Text)
                .MaximumLength(maxTextLength)
                .WithMessage($"Комментарий не может превышать {maxTextLength} символов");
        }
    }
}
