Feature: Commentable articles feature

Background:
	Given Test server is running

@NUnitRunner
Scenario: Create Test Article with Comments, then Delete all
	Given Test article with caption 'Test article 1' and text 'Some text'
	Given Test article's caption has not been already occupied
	When I send a request to create test article
	Then A new article is created
	And A new article's header is available to user through get article headers request

	Given these Test Comments:
	| User  | Text |
	| User1 | First test comment |
	| User2 | Another user reply |
	When I send add comment requests for each test comment
	Then Test comments for test article are created
	And Test comments are available to user through get article request

	When I send delete article request
	Then A test article is deleted
	And Test Article is no longer available through get article headers request
	And Test Article is no longer available through get article request
