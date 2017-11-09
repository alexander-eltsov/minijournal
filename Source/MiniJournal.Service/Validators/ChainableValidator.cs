using FluentValidation;

namespace Infotecs.MiniJournal.Service.Validators
{
    public class ChainableValidator<T> : AbstractValidator<T>
    {
        protected ChainableValidator()
        {
        }

        public ChainableValidator<T> ChainValidator(ChainableValidator<T> validator)
        {
            var newValidator = new ChainableValidator<T>();
            newValidator
                .RuleFor(context => context)
                .SetValidator(this);
            newValidator
                .RuleFor(context => context)
                .SetValidator(validator);
            return newValidator;
        }
    }
}
