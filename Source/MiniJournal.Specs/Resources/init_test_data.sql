USE MiniJournalDB_Test
GO

insert into [dbo].[Articles]([Caption], [Text]) values('Article 1', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin nec sollicitudin enim, sit amet pharetra leo. Etiam vel mi pulvinar, sodales metus non, dignissim metus. Maecenas aliquam efficitur gravida. Pellentesque semper sit amet massa non bibendum. Cras euismod nibh ac elit pellentesque, ut posuere ligula vestibulum. Proin vel laoreet lacus. Morbi imperdiet enim et vulputate efficitur. Etiam et ipsum id enim dapibus sagittis. Morbi in eros lectus.');
insert into [dbo].[Articles]([Caption], [Text]) values('Article 2', 'Donec lobortis id augue at lacinia. Nam sodales, sem quis fringilla iaculis, dolor nisi ultrices lacus, non fringilla quam enim ultricies libero. Sed ligula ipsum, hendrerit id viverra id, eleifend id urna. Quisque ornare finibus odio. Aenean nisl sem, fringilla eu enim et, ultrices congue nibh. Nullam pellentesque at magna nec vulputate. Donec nec sem maximus, aliquam ligula id, efficitur ex. Aliquam vehicula magna ut est porttitor, sit amet molestie ante vulputate.');

insert into [dbo].[Comments]([ArticleID], [User], [Text]) values((select top 1 [ID] from [dbo].[Articles] where [Caption] = 'Article 1'), 'User 1', 'That article is awesome');
insert into [dbo].[Comments]([ArticleID], [User], [Text]) values((select top 1 [ID] from [dbo].[Articles] where [Caption] = 'Article 1'), 'User 2', 'I don''t know. I don''t really know how to read or write.');
