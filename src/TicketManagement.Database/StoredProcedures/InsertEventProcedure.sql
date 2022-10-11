CREATE PROCEDURE InsertEvent
(
	@Name NVARCHAR(120),
	@Description NVARCHAR(MAX),
	@LayoutId INT,
	@DateStart DATETIME,
    @DateEnd DATETIME,
	@ImageURL NVARCHAR(MAX),
	@ShowTime TIME,
	@AddedId INT OUTPUT
)
AS
BEGIN
BEGIN TRY 
DECLARE @newID INT;
	INSERT INTO Event(Name, Description, LayoutId, DateStart, DateEnd, ImageURL, ShowTime)
	VALUES(@Name, @Description, @LayoutId, @DateStart, @DateEnd, @ImageURL, @ShowTime);
SELECT 
@newID = SCOPE_IDENTITY();
SET @AddedId = @newID;
END TRY
BEGIN CATCH 
END CATCH
END