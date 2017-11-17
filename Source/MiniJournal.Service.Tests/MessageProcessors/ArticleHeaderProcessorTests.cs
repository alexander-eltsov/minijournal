using System;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Service.MessageProcessors;
using Moq;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests.MessageProcessors
{
    [TestFixture]
    public class ArticleHeaderProcessorTests
    {
        [Test]
        public void Get_GetArticleHeadersRequest_InvokesArticleRepositoryGetHeaders()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new ArticleHeaderProcessor(mockRepository.Object, new MiniJournalMapper());

            sut.Get(new GetArticleHeadersRequest());

            mockRepository.Verify(repository => repository.GetHeaders(), Times.AtLeastOnce());

            Assert.IsFalse(true);
        }
    }
}
