using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Infotecs.MiniJournal.Application.ViewModels;
using Infotecs.MiniJournal.Contracts;
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
                Mock<IArticleService> moqArticleService = CreateMoqArticleService();
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
                Mock<IArticleService> moqArticleService = CreateMoqArticleService();
                var sut = autoMock.Create<ArticlesViewModel>(TypedParameter.From(moqArticleService.Object));

                sut.LoadHeadersCommand.Execute(null);
                sut.LoadHeadersCommand.Execute(null);

                Assert.AreEqual(1, sut.Headers.Count);
                Assert.AreEqual("Fake Article", sut.Headers.ToArray()[0].Caption);
            }
        }

        [Test]
        public void SaveArticleCommand_AfterExecutedSuccessfully_ReloadsArticleUsingServiceGetArticleOperation()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var header = new HeaderData
                {
                    Id = 1,
                    Caption = "Fake Article"
                };
                var moqArticleService = new Mock<IArticleService>();
                var sut = autoMock.Create<ArticlesViewModel>(TypedParameter.From(moqArticleService.Object));
                sut.SelectedHeader = header;
                moqArticleService.ResetCalls();

                sut.SaveArticleCommand.Execute(null);

                moqArticleService.Verify(
                    service => service.GetArticle(It.Is<int>(i => i == header.Id)),
                    Times.Once());
            }
        }

        private Mock<IArticleService> CreateMoqArticleService()
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
            return moqArticleService;
        }
    }
}
