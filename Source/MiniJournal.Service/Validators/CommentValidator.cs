using FluentValidation;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Service.Validators
{
    public class CommentValidator : ChainableValidator<Comment>
    {
        private const int maxTextLength = 512;
        private const int maxUserLength = 128;

        public CommentValidator()
        {
            RuleFor(comment => comment.Text)
                .NotNull()
                .WithMessage("Комментарий не может быть пустым");

            RuleFor(comment => comment.Text)
                .MaximumLength(maxTextLength)
                .WithMessage($"Комментарий не может превышать {maxTextLength} символов");

            RuleFor(comment => comment.User)
                .NotNull()
                .WithMessage("Имя пользователя не может быть пустым");

            RuleFor(comment => comment.User)
                .MaximumLength(maxUserLength)
                .WithMessage($"Имя пользователя не может превышать {maxUserLength} символов");
        }
    }
}
