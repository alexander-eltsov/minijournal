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
        public void LoadHeadersCommand_WhenExecuted_FillsHeaders()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var moqArticleService = new Mock<IArticleService>();
                moqArticleService
                    .Setup(service => service.GetArticleHeaders())
                    .Returns(() => new HeaderData[1]
                    {
                        new HeaderData
                        {
                            Id = 1,
                            Caption = "Fake Article"
                        }
                    });
                var sut = autoMock.Create<ArticlesViewModel>(TypedParameter.From(moqArticleService.Object));

                sut.LoadHeadersCommand.Execute(null);

                Assert.AreEqual(1, sut.Headers.Count);
                Assert.AreEqual("Fake Article", sut.Headers.ToArray()[0].Caption);
            }
        }

        [Test]
        public void LoadHeadersCommand_ClearsHeadersBeforeFill()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var moqArticleService = new Mock<IArticleService>();
                moqArticleService
                    .Setup(service => service.GetArticleHeaders())
                    .Returns(() => new HeaderData[1]
                    {
                        new HeaderData
                        {
                            Id = 1,
                            Caption = "Fake Article"
                        }
                    });
                var sut = autoMock.Create<ArticlesViewModel>(TypedParameter.From(moqArticleService.Object));

                sut.LoadHeadersCommand.Execute(null);
                sut.LoadHeadersCommand.Execute(null);

                Assert.AreEqual(1, sut.Headers.Count);
                Assert.AreEqual("Fake Article", sut.Headers.ToArray()[0].Caption);
            }
        }
    }
}
