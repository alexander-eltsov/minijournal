using System;
using System.Linq;
using Infotecs.MiniJournal.Application.ArticleServiceReference;
using Infotecs.MiniJournal.Application.ViewModels;
using Moq;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Application.Tests.ViewModels
{
    [TestFixture]
    public class ArticlesViewModelTests
    {
        [Test]
        public void Constructor_NullArticleServiceProvided_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var sut = new ArticlesViewModel(null);
            });
        }

        [Test]
        public void LoadArticlesCommand_WhenExecuted_FillsArticles()
        {
            var moqArticleService = new Mock<IArticleService>();
            moqArticleService
                .Setup(service => service.GetAllArticles())
                .Returns(() =>
                {
                    return new ArticleData[1]
                    {
                        new ArticleData
                        {
                            Caption = "Fake Article",
                            Text = string.Empty
                        }
                    };
                });
            var sut = new ArticlesViewModel(moqArticleService.Object);

            sut.LoadArticlesCommand.Execute(null);

            Assert.AreEqual(1, sut.Articles.Count);
            Assert.AreEqual("Fake Article", sut.Articles.ToArray()[0].Caption);
        }

        [Test]
        public void LoadArticlesCommand_ClearsArticlesBeforeFill()
        {
            var moqArticleService = new Mock<IArticleService>();
            moqArticleService
                .Setup(service => service.GetAllArticles())
                .Returns(() =>
                {
                    return new ArticleData[1]
                    {
                        new ArticleData
                        {
                            Caption = "Fake Article",
                            Text = string.Empty
                        }
                    };
                });
            var sut = new ArticlesViewModel(moqArticleService.Object);

            sut.LoadArticlesCommand.Execute(null);
            sut.LoadArticlesCommand.Execute(null);

            Assert.AreEqual(1, sut.Articles.Count);
            Assert.AreEqual("Fake Article", sut.Articles.ToArray()[0].Caption);
        }
    }
}
