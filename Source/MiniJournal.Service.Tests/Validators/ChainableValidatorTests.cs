using System;
using FluentValidation;
using FluentValidation.Results;
using Infotecs.MiniJournal.Service.Validators;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests.Validators
{
    [TestFixture]
    public class ChainableValidatorTests
    {
        private class FakeChainableValidator : ChainableValidator<object>
        {
            public int ValidateInvokedTimes { get; private set; }

            public override ValidationResult Validate(ValidationContext<object> context)
            {
                ValidateInvokedTimes++;
                return base.Validate(context);
            }
        }

        [Test]
        public void Validate_ChainedValidatorsProvided_EachValidatorInvoked()
        {
            var validator1 = new FakeChainableValidator();
            var validator2 = new FakeChainableValidator();
            var sut = validator1.ChainValidator(validator2);

            sut.Validate(new object());

            Assert.AreEqual(1, validator1.ValidateInvokedTimes);
            Assert.AreEqual(1, validator2.ValidateInvokedTimes);
        }
    }
}
