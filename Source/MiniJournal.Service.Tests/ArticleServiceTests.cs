using System;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Moq;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests
{
    [TestFixture]
    public class ArticleServiceTests
    {
        [Test]
        public void Constructor_NullArticleRepositoryProvided_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var sut = new ArticleService(null);
            });
        }

        [Test]
        public void GetAllArticles_InvokesArticleRepositoryGetArticles()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new ArticleService(mockRepository.Object);

            sut.GetAllArticles();

            mockRepository.Verify(repository => repository.GetArticles(), Times.AtLeastOnce());
        }

        [Test]
        public void CreateArticle_InvokesArticleRepositoryCreateArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new ArticleService(mockRepository.Object);
            var newArticle = new ArticleData
            {
                Caption = "Fake Article",
                Text = string.Empty
            };

            sut.CreateArticle(newArticle);

            mockRepository.Verify(repository => repository.CreateArticle(newArticle), Times.AtLeastOnce());
        }

        [Test]
        public void UpdateArticle_InvokesArticleRepositoryUpdateArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new ArticleService(mockRepository.Object);
            var article = new ArticleData
            {
                Caption = "Fake Article",
                Text = string.Empty
            };

            sut.UpdateArticle(article);

            mockRepository.Verify(repository => repository.UpdateArticle(article), Times.AtLeastOnce());
        }

        [Test]
        public void DeleteArticle_InvokesArticleRepositoryDeleteArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new ArticleService(mockRepository.Object);
            var article = new ArticleData
            {
                Caption = "Fake Article",
                Text = string.Empty
            };

            sut.DeleteArticle(article);

            mockRepository.Verify(repository => repository.DeleteArticle(article), Times.AtLeastOnce());
        }
    }
}
