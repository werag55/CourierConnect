Feature: Login

Check if login functionality works

@tag1
Scenario: Login user as Client
	Given I navigate to application
	And I click the Login link
	And I enter username and password
		| UserName                | Password  |
		| alinkamalinka@gmail.com | Alinka12! |
	And I click login
	Then I should see user logged in to the application
