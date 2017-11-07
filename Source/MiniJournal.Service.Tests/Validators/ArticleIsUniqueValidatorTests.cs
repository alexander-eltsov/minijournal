using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Infotecs.MiniJournal.Models;
using Infotecs.MiniJournal.Service.Validators;
using Moq;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests.Validators
{
    [TestFixture]
    public class ArticleIsUniqueValidatorTests
    {
        [Test]
        public void Validate_CaptionAlreadyExistsInRepository_ArticleIsInvalid()
        {
            var newArticle = new Article
            {
                Id = 3,
                Caption = "article 1",
                Text = "some text"
            };
            var mockRepository = new Mock<IArticleRepository>();
            mockRepository
                .Setup(repository => repository.GetArticles())
                .Returns(() => new List<Article>
                {
                    new Article { Id = 1, Caption = "article 1" },
                    new Article { Id = 2, Caption = "article 2" }
                });
            var sut = new ArticleIsUniqueValidator(mockRepository.Object);

            ValidationResult result = sut.Validate(newArticle);

            Assert.IsFalse(result.IsValid);
        }
    }
}
