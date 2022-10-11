CREATE TABLE [dbo].[Event]
(
	[Id] int primary key identity,
	[Name] nvarchar(120) NOT NULL,
	[Description] nvarchar(max) NOT NULL,
	[LayoutId] int NOT NULL, 
    [DateStart] DATETIME NOT NULL, 
    [DateEnd] DATETIME NOT NULL,
	[ImageURL] nvarchar(max),
	[ShowTime] TIME NOT NULL,
)
