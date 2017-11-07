using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
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
        public void LoadArticlesCommand_WhenExecuted_FillsArticles()
        {
            using (var autoMock = AutoMock.GetLoose())
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
                var sut = autoMock.Create<ArticlesViewModel>(TypedParameter.From(moqArticleService.Object));

                sut.LoadArticlesCommand.Execute(null);

                Assert.AreEqual(1, sut.Articles.Count);
                Assert.AreEqual("Fake Article", sut.Articles.ToArray()[0].Caption);
            }
        }

        [Test]
        public void LoadArticlesCommand_ClearsArticlesBeforeFill()
        {
            using (var autoMock = AutoMock.GetLoose())
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
                var sut = autoMock.Create<ArticlesViewModel>(TypedParameter.From(moqArticleService.Object));

                sut.LoadArticlesCommand.Execute(null);
                sut.LoadArticlesCommand.Execute(null);

                Assert.AreEqual(1, sut.Articles.Count);
                Assert.AreEqual("Fake Article", sut.Articles.ToArray()[0].Caption);
            }
        }
    }
}
