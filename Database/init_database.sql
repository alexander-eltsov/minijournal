USE [MiniJournalDB]
GO

CREATE TABLE [dbo].[Articles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [nvarchar](255) NOT NULL,
	[Text] [nvarchar](1024) NULL,
	PRIMARY KEY (ID),
)
GO

CREATE TABLE [dbo].[Comments] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Text] nvarchar(512) NOT NULL,
	[ArticleID] int NOT NULL,
	[User] [nvarchar](128) NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY (ArticleID) REFERENCES Articles(ID) ON DELETE CASCADE
);
GO
