using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
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
                var response = InvokeGetArticleHeaders();
            }, "Ensure test server is running and available");
        }

        [Given(@"Test article with caption '(.*)' and text '(.*)'")]
        public void GivenTestArticleWithCaptionAndText(string caption, string text)
        {
            var newArticle = new ArticleData
            {
                Caption = caption,
                Text = text
            };
            ScenarioContext.Current["NewArticle"] = newArticle;
        }

        [Given(@"Test article's caption has not been already occupied")]
        public void GivenTestArticleSCaptionHasNotBeenAlreadyOccupied()
        {
            var articleData = (ArticleData)ScenarioContext.Current["NewArticle"];
            var response = InvokeGetArticleHeaders();

            Assert.IsFalse(response.Headers.Any(header =>
                    header.Caption == articleData.Caption),
                $"Article caption {articleData.Caption} should be available after in test server");
        }

        [Given(@"these Test Comments:")]
        public void GivenTheseTestComments(Table table)
        {
            var testComments = new List<CommentData>();
            List<string> columnsList = table.Header.ToList();
            var userIndex = columnsList.IndexOf(columnsList.First(s => s.Equals("user", StringComparison.CurrentCultureIgnoreCase)));
            var textIndex = columnsList.IndexOf(columnsList.First(s => s.Equals("text", StringComparison.CurrentCultureIgnoreCase)));
            foreach (TableRow row in table.Rows)
            {
                string[] valuesArray = row.Values.ToArray();
                testComments.Add(new CommentData
                {
                    User = valuesArray[userIndex],
                    Text = valuesArray[textIndex]
                });
            }

            ScenarioContext.Current["TestComments"] = testComments;
        }

        [When(@"I send a request to create test article")]
        public void WhenISendARequestToCreateArticleWithCaption()
        {
            var request = new CreateArticleRequest
            {
                NewArticle = (ArticleData) ScenarioContext.Current["NewArticle"]
            };
            ScenarioContext.Current["AddArticleResult"] = MiniJournalContext.ServiceClient.Post<CreateArticleResponse>(request);
        }

        [When(@"I send add comment requests for each test comment")]
        public void WhenISendAddCommentRequests()
        {
            var articleData = (ArticleData) ScenarioContext.Current["NewArticle"];
            var testComments = (IList<CommentData>) ScenarioContext.Current["TestComments"];

            var addCommentResults = new List<AddCommentResponse>();
            foreach (var testComment in testComments)
            {
                var request = new AddCommentRequest
                {
                    ArticleId = articleData.Id,
                    Comment = testComment
                };
                var response = MiniJournalContext.ServiceClient.Post<AddCommentResponse>(request);
                addCommentResults.Add(response);
            }

            ScenarioContext.Current["AddCommentResults"] = addCommentResults;
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

        [Then(@"Test comments for test article are created")]
        public void ThenANewCommentForTestArticleIsCreated()
        {
            var testComments = (IList<CommentData>)ScenarioContext.Current["TestComments"];
            var responses = (IList<AddCommentResponse>) ScenarioContext.Current["AddCommentResults"];

            Assert.AreEqual(testComments.Count, responses.Count, "Number of AddComment responses should be equal to number of requests");
            for (var index = 0; index < responses.Count; index++)
            {
                AddCommentResponse addCommentResponse = responses[index];
                Assert.IsTrue(addCommentResponse.ArticleId > 0, "Invalid ArticleId");
                Assert.IsTrue(addCommentResponse.CommentId > 0, "Invalid CommentId");
                testComments[index].Id = addCommentResponse.CommentId;
            }
        }

        [Then(@"Test comments are available to user through get article request")]
        public void ThenANewCommentIsAvailableToUserThroughGetArticleRequest()
        {
            var newArticleData = (ArticleData)ScenarioContext.Current["NewArticle"];
            var testComments = (IList<CommentData>)ScenarioContext.Current["TestComments"];

            var request = new GetArticleRequest
            {
                ArticleId = newArticleData.Id
            };
            var response = MiniJournalContext.ServiceClient.Get<GetArticleResponse>(request);

            Assert.IsNotNull(response, "Response from GetArticle returned null");

            foreach (CommentData testComment in testComments)
            {
                Assert.That(
                    response.Article.Comments.Any(comment =>
                        comment.Id == testComment.Id &&
                        comment.User == testComment.User &&
                        comment.Text == testComment.Text),
                    "Test comment for test article is not found");
            }
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
            var response = InvokeGetArticleHeaders();

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

            Assert.Throws<WebFaultException>(() =>
            {
                MiniJournalContext.ServiceClient.Get<GetArticleResponse>(request);
            }, "Test article should not be available after detele");
        }

        private GetArticleHeadersResponse InvokeGetArticleHeaders()
        {
            var request = new GetArticleHeadersRequest();
            var response = MiniJournalContext.ServiceClient.Get<GetArticleHeadersResponse>(request);
            return response;
        }
    }
}
