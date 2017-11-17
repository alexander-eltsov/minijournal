using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Models;
using Infotecs.MiniJournal.Service.MessageProcessors;
using Moq;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Service.Tests.MessageProcessors
{
    [TestFixture]
    public class ArticleProcessorTests
    {
        [Test]
        public void Post_CreateArticleRequest_InvokesArticleRepositoryCreateArticle()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                Mock<IArticleRepository> mockRepository = CreateMockRepository();
                ArticleProcessor sut = CreateArticleProcessor(autoMock, mockRepository);
                var newArticle = new ArticleData
                {
                    Caption = "Fake Article",
                    Text = string.Empty
                };

                sut.Post(new CreateArticleRequest { NewArticle = newArticle });

                mockRepository.Verify(repository => repository.CreateArticle(It.IsAny<Article>()), Times.AtLeastOnce());
            }
        }

        [Test]
        public void Put_UpdateArticleRequest_InvokesArticleRepositoryUpdateArticle()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                Mock<IArticleRepository> mockRepository = CreateMockRepository();
                ArticleProcessor sut = CreateArticleProcessor(autoMock, mockRepository);
                var article = new ArticleData
                {
                    Caption = "Fake Article",
                    Text = string.Empty
                };

                sut.Put(new UpdateArticleRequest { Article = article });

                mockRepository.Verify(repository => repository.UpdateArticle(It.IsAny<Article>()), Times.AtLeastOnce());
            }
        }

        [Test]
        public void DeleteOneWay_DeleteArticleRequest_InvokesArticleRepositoryDeleteArticle()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                Mock<IArticleRepository> mockRepository = CreateMockRepository();
                ArticleProcessor sut = CreateArticleProcessor(autoMock, mockRepository);
                var article = new ArticleData
                {
                    Id = 1,
                    Caption = "Fake Article",
                    Text = string.Empty
                };

                sut.DeleteOneWay(new DeleteArticleRequest { ArticleId = article.Id });

                mockRepository.Verify(repository => repository.DeleteArticle(It.IsAny<int>()), Times.AtLeastOnce());
            }
        }

        private ArticleProcessor CreateArticleProcessor(AutoMock autoMock, Mock<IArticleRepository> mockRepository)
        {
            var sut = autoMock.Create<ArticleProcessor>(
                new TypedParameter(typeof(IArticleRepository), mockRepository.Object),
                new TypedParameter(typeof(IMapper), new MiniJournalMapper()));
            return sut;
        }

        private Mock<IArticleRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<IArticleRepository>();
            mockRepository.Setup(repository => repository.GetHeaders()).Returns(() => new List<Header>());
            return mockRepository;
        }
    }
}
