USE [MiniJournalDB]
GO

CREATE TABLE [dbo].[Articles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [nvarchar](255) NOT NULL,
	[Text] [nvarchar](1024) NULL,
	PRIMARY KEY (ID),
)
GO

CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Text] [nvarchar](1024) NULL,
	PRIMARY KEY (ID),
)
GO

CREATE TABLE [dbo].[Comments] (
	[ID] int NOT NULL,
	[Text] nvarchar(512) NOT NULL,
	[ArticleID] int,
	[UserID] int,
	PRIMARY KEY (ID),
	FOREIGN KEY (ArticleID) REFERENCES Articles(ID),
	FOREIGN KEY (UserID) REFERENCES Users(ID)
);
GO