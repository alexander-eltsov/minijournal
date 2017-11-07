using System;
using System.Linq;
using Autofac.Extras.Moq;
using FluentValidation.Results;
using Infotecs.MiniJournal.Models;
using Infotecs.MiniJournal.Service.Validators;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests.Validators
{
    [TestFixture]
    public class ArticleValidatorTests
    {
        [Test]
        public void Validate_CaptionIsNullOrEmpty_ArticleIsInvalid()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var article1 = new Article
                {
                    Id = 1,
                    Caption = null,
                    Text = "some text"
                };
                var article2 = new Article
                {
                    Id = 1,
                    Caption = "",
                    Text = "some text"
                };
                var sut = autoMock.Create<ArticleValidator>();

                ValidationResult results1 = sut.Validate(article1);
                ValidationResult results2 = sut.Validate(article2);

                Assert.IsFalse(results1.IsValid);
                Assert.IsTrue(results1.Errors.Any(error => error.PropertyName == nameof(article1.Caption)));
                Assert.IsFalse(results2.IsValid);
                Assert.IsTrue(results2.Errors.Any(error => error.PropertyName == nameof(article2.Caption)));
            }
        }
    }
}
