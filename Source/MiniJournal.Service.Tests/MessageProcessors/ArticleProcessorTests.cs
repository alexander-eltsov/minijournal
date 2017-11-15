using System;
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
        private class FakeArticleProcessor : ArticleProcessor
        {
            public FakeArticleProcessor(IArticleRepository articleRepository) : base(articleRepository, new MiniJournalMapper(), new Mock<INotificationService>().Object)
            {
            }

            public bool SupressArticleValidation { get; set; }

            protected override void ValidateArticle(Article article)
            {
                if (SupressArticleValidation)
                {
                    return;
                }
                base.ValidateArticle(article);
            }
        }

        [Test]
        public void Post_CreateArticleRequest_InvokesArticleRepositoryCreateArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new FakeArticleProcessor(mockRepository.Object)
            {
                SupressArticleValidation = true
            };
            var newArticle = new ArticleData
            {
                Caption = "Fake Article",
                Text = string.Empty
            };

            sut.Post(new CreateArticleRequest { NewArticle = newArticle });

            mockRepository.Verify(repository => repository.CreateArticle(It.IsAny<Article>()), Times.AtLeastOnce());
        }

        [Test]
        public void Put_UpdateArticleRequest_InvokesArticleRepositoryUpdateArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new FakeArticleProcessor(mockRepository.Object)
            {
                SupressArticleValidation = true
            };
            var article = new ArticleData
            {
                Caption = "Fake Article",
                Text = string.Empty
            };

            sut.Put(new UpdateArticleRequest {Article = article });

            mockRepository.Verify(repository => repository.UpdateArticle(It.IsAny<Article>()), Times.AtLeastOnce());
        }

        [Test]
        public void DeleteOneWay_DeleteArticleRequest_InvokesArticleRepositoryDeleteArticle()
        {
            var mockRepository = new Mock<IArticleRepository>();
            var sut = new FakeArticleProcessor(mockRepository.Object)
            {
                SupressArticleValidation = true
            };
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
}
