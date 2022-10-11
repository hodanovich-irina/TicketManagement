CREATE PROCEDURE UpdateEvent
(
	@Id INT,
	@Name NVARCHAR(120),
	@Description NVARCHAR(MAX),
	@LayoutId INT,		
	@DateStart DATETIME,
    @DateEnd DATETIME,
	@ImageURL NVARCHAR(MAX),
	@ShowTime TIME
)
AS
BEGIN
	UPDATE Event
	SET Name = @Name,
		Description = @Description, 
		LayoutId = @LayoutId,
		DateStart = @DateStart,
		DateEnd = @DateEnd,
		ImageURL = @ImageURL,
	    ShowTime = @ShowTime

	WHERE Id = @Id
END