using FluentValidation;

namespace Infotecs.MiniJournal.Service.Validators
{
    public abstract class ChainableValidator<T> : AbstractValidator<T>
    {
        public ChainableValidator<T> ChainValidator(ChainableValidator<T> validator)
        {
            RuleFor(context => context).SetValidator(validator);
            return validator;
        }
    }
}
