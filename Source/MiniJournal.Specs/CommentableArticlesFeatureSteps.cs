using System;
using System.Linq;
using Infotecs.MiniJournal.Contracts;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Infotecs.MiniJournal.Specs
{
    [Binding]
    public class CommentableArticlesFeatureSteps
    {
        private MiniJournalContext MiniJournalContext { get; }

        public CommentableArticlesFeatureSteps()
        {   
            MiniJournalContext = new MiniJournalContext();
        }

        [Given(@"Test server is running")]
        public void GivenServerIsRunning()
        {
            Assert.DoesNotThrow(() =>
            {
                var request = new GetArticleHeadersRequest();
                MiniJournalContext.ServiceClient.Get<GetArticleHeadersResponse>(request);
            }, "Ensure test server is running and available");
        }

        [When(@"I send create article request")]
        public void WhenISendCreateArticleRequest()
        {
            var newArticle = new ArticleData
            {
                Caption = Guid.NewGuid().ToString(),
                Text = "This a test article with unique caption"
            };
            var request = new CreateArticleRequest
            {
                NewArticle = newArticle
            };
            ScenarioContext.Current["NewArticle"] = newArticle;
            ScenarioContext.Current["AddArticleResult"] = MiniJournalContext.ServiceClient.Post<CreateArticleResponse>(request);
        }

        [When(@"I send add comment request")]
        public void WhenISendAddCommentRequest()
        {
            var newComment = new CommentData
            {
                User = "SomeUser",
                Text = "This a unique comment for a test article."
            };
            var articleData = (ArticleData) ScenarioContext.Current["NewArticle"];
            var request = new AddCommentRequest
            {
                ArticleId = articleData.Id,
                Comment = newComment
            };
            ScenarioContext.Current["NewComment"] = newComment;
            ScenarioContext.Current["AddCommentResult"] = MiniJournalContext.ServiceClient.Post<AddCommentResponse>(request);
        }

        [When(@"I send delete article request")]
        public void WhenISendDeleteArticleRequest()
        {
            var articleData = (ArticleData)ScenarioContext.Current["NewArticle"];
            var request = new DeleteArticleRequest
            {
                ArticleId = articleData.Id
            };
            MiniJournalContext.ServiceClient.Delete(request);
        }

        [Then(@"A new article is created")]
        public void ThenANewArticleIsCreated()
        {
            var repsonse = ScenarioContext.Current["AddArticleResult"] as CreateArticleResponse;

            Assert.IsNotNull(repsonse, "Response from CreateArticle should not be null");
            Assert.IsTrue(repsonse.ArticleId > 0, "Invalid ArticleId");

            var newArticleData = (ArticleData)ScenarioContext.Current["NewArticle"];
            newArticleData.Id = repsonse.ArticleId;
        }

        [Then(@"A new article's header is available to user through get article headers request")]
        public void ThenANewArticleSHeaderIsAvailableToUserThroughGetArticleHeadersRequest()
        {
            var newArticleData = (ArticleData) ScenarioContext.Current["NewArticle"];

            var request = new GetArticleHeadersRequest();
            var response = MiniJournalContext.ServiceClient.Get<GetArticleHeadersResponse>(request);

            Assert.That(response.Headers.Any(header =>
                header.Id == newArticleData.Id && header.Caption == newArticleData.Caption),
                "Header for test article is not found");
        }
        
        [Then(@"A new comment for test article is created")]
        public void ThenANewCommentForTestArticleIsCreated()
        {
            var repsonse = ScenarioContext.Current["AddCommentResult"] as AddCommentResponse;

            Assert.IsNotNull(repsonse, "Response from AddComment should not be null");
            Assert.IsTrue(repsonse.ArticleId > 0, "Invalid ArticleId");
            Assert.IsTrue(repsonse.CommentId > 0, "Invalid CommentId");

            var newCommentData = (CommentData)ScenarioContext.Current["NewComment"];
            newCommentData.Id = repsonse.CommentId;
        }

        [Then(@"A new comment is available to user through get article request")]
        public void ThenANewCommentIsAvailableToUserThroughGetArticleRequest()
        {
            var newArticleData = (ArticleData)ScenarioContext.Current["NewArticle"];
            var newCommentData = (CommentData)ScenarioContext.Current["NewComment"];

            var request = new GetArticleRequest
            {
                ArticleId = newArticleData.Id
            };
            var response = MiniJournalContext.ServiceClient.Get<GetArticleResponse>(request);

            Assert.IsNotNull(response, "Response from GetArticle returned null");
            Assert.That(
                response.Article.Comments.Any(comment =>
                    comment.Id == newCommentData.Id &&
                    comment.User == newCommentData.User &&
                    comment.Text == newCommentData.Text),
                "Test comment for test article is not found");
        }

        [Then(@"A test article is deleted")]
        public void ThenATestArticleIsDeleted()
        {
            // no separate response class from Delete article
        }
        
        [Then(@"Test Article is no longer available through get article headers request")]
        public void ThenTestArticleIsNoLongerAvailableThroughGetArticleHeadersRequest()
        {
            var articleData = (ArticleData)ScenarioContext.Current["NewArticle"];

            var request = new GetArticleHeadersRequest();
            var response = MiniJournalContext.ServiceClient.Get<GetArticleHeadersResponse>(request);

            Assert.IsFalse(response.Headers.Any(header =>
                header.Id == articleData.Id && header.Caption == articleData.Caption),
                "Test article should not be available after detele");
        }

        [Then(@"Test Article is no longer available through get article request")]
        public void ThenTestArticleIsNoLongerAvailableThroughGetArticleRequest()
        {
            var articleData = (ArticleData)ScenarioContext.Current["NewArticle"];
            var request = new GetArticleRequest
            {
                ArticleId = articleData.Id
            };
            var response = MiniJournalContext.ServiceClient.Get<GetArticleResponse>(request);

            Assert.IsNotNull(response);
            Assert.IsNull(response.Article, "Test article should not be available after detele");
        }
    }
}
