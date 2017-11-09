using System;
using System.Linq;
using FluentValidation.Results;
using Infotecs.MiniJournal.Models;
using Infotecs.MiniJournal.Service.Validators;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests.Validators
{
    [TestFixture]
    public class CommentValidatorTests
    {
        [Test]
        public void Validate_TextIsNull_CommentIsInvalid()
        {
            var comment = new Comment
            {
                Id = 1,
                Text = null
            };
            var sut = new CommentValidator();

            ValidationResult results = sut.Validate(comment);

            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors.Any(error => error.PropertyName == nameof(comment.Text)));
        }
    }
}
