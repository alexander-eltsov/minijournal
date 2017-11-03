using System;
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
    }
}
