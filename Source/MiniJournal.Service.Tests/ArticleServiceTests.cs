using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Models;
using Moq;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests
{
    [TestFixture]
    public class ArticleServiceTests
    {
        private class FakeArticleService : ArticleService
        {
            public FakeArticleService(IArticleRepository articleRepository) : base(articleRepository, new MiniJournalMapper())
            {
            }
        }

        [Test]
        public void GetArticleHeaders_InvokesArticleRepositoryGetArticles()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new FakeArticleService(mockRepository.Object);

            sut.GetArticleHeaders();

            mockRepository.Verify(repository => repository.GetArticles(), Times.AtLeastOnce());
        }

        [Test]
        public void CreateArticle_InvokesArticleRepositoryCreateArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new FakeArticleService(mockRepository.Object);
            var newArticle = new ArticleData
            {
                Caption = "Fake Article",
                Text = string.Empty
            };

            sut.CreateArticle(newArticle);

            mockRepository.Verify(repository => repository.CreateArticle(It.IsAny<Article>()), Times.AtLeastOnce());
        }

        [Test]
        public void UpdateArticle_InvokesArticleRepositoryUpdateArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new FakeArticleService(mockRepository.Object);
            var article = new ArticleData
            {
                Caption = "Fake Article",
                Text = string.Empty
            };

            sut.UpdateArticle(article);

            mockRepository.Verify(repository => repository.UpdateArticle(It.IsAny<Article>()), Times.AtLeastOnce());
        }

        [Test]
        public void DeleteArticle_InvokesArticleRepositoryDeleteArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new FakeArticleService(mockRepository.Object);
            var article = new ArticleData
            {
                Id = 1,
                Caption = "Fake Article",
                Text = string.Empty
            };

            sut.DeleteArticle(article.Id);

            mockRepository.Verify(repository => repository.DeleteArticle(It.IsAny<int>()), Times.AtLeastOnce());
        }
    }
}
