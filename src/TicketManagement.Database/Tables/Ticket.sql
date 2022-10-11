CREATE TABLE [dbo].[Ticket]
(
	[Id] INT IDENTITY PRIMARY KEY,
	[Price] decimal, 
	[UserId] nvarchar(max),
	[EventSeatId] int,
	[DateOfPurchase] DATETIME,
)
