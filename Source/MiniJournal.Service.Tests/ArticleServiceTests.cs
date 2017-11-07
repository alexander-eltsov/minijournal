using Autofac;
using Autofac.Extras.Moq;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Models;
using Moq;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests
{
    [TestFixture]
    public class ArticleServiceTests
    {
        [Test]
        public void GetAllArticles_InvokesArticleRepositoryGetArticles()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockRepository = new Mock<IArticleRepository>();
                var sut = autoMock.Create<ArticleService>(TypedParameter.From(mockRepository.Object));

                sut.GetAllArticles();

                mockRepository.Verify(repository => repository.GetArticles(), Times.AtLeastOnce());
            }

        }

        [Test]
        public void CreateArticle_InvokesArticleRepositoryCreateArticle()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockRepository = new Mock<IArticleRepository>();
                var sut = autoMock.Create<ArticleService>(TypedParameter.From(mockRepository.Object));
                var newArticle = new ArticleData
                {
                    Caption = "Fake Article",
                    Text = string.Empty
                };

                sut.CreateArticle(newArticle);

                mockRepository.Verify(repository => repository.CreateArticle(It.IsAny<Article>()), Times.AtLeastOnce());
            }
        }

        [Test]
        public void UpdateArticle_InvokesArticleRepositoryUpdateArticle()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockRepository = new Mock<IArticleRepository>();
                var sut = autoMock.Create<ArticleService>(TypedParameter.From(mockRepository.Object));
                var article = new ArticleData
                {
                    Caption = "Fake Article",
                    Text = string.Empty
                };

                sut.UpdateArticle(article);

                mockRepository.Verify(repository => repository.UpdateArticle(It.IsAny<Article>()), Times.AtLeastOnce());
            }
        }

        [Test]
        public void DeleteArticle_InvokesArticleRepositoryDeleteArticle()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockRepository = new Mock<IArticleRepository>();
                var sut = autoMock.Create<ArticleService>(TypedParameter.From(mockRepository.Object));
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
}
