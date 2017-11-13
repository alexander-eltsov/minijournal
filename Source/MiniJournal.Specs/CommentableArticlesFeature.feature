Feature: Commentable articles feature

Background:
	Given Test server is running

Scenario: Create Test Article with Comment, Then Delete both
	When I send create article request
	Then A new article is created
	And A new article's header is available to user through get article headers request

	When I send add comment request
	Then A new comment for test article is created
	And A new comment is available to user through get article request

	When I send delete article request
	Then A test article is deleted
	And Test Article is no longer available through get article headers request
	And Test Article is no longer available through get article request
