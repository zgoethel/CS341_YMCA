CREATE FUNCTION [dbo].[SplitId] (
	@List NVARCHAR(MAX),
	@SplitOn NVARCHAR(5)
)
RETURNS @Results TABLE (Id NUMERIC)
AS
BEGIN
	DECLARE @Value NVARCHAR(100)
	
	WHILE (CHARINDEX(@SplitOn, @List) > 0)
	BEGIN
	
		SELECT @Value = LTRIM(
							RTRIM(
								SUBSTRING(@List, 1, CHARINDEX(@SplitOn, @List) - 1)
							)
						)
		
		SET @List = SUBSTRING(@List, CHARINDEX(@SplitOn, @List) + LEN(@SplitOn), LEN(@List))
		
		IF ISNUMERIC(@Value) = 1
		BEGIN
			INSERT INTO @Results (Id) VALUES (CONVERT(NUMERIC, @Value))
		END

	END
	
	SELECT @Value = LTRIM(RTRIM(@List))
	IF ISNUMERIC(@Value) = 1
	BEGIN
		INSERT INTO @Results (Id) VALUES (CONVERT(NUMERIC, @Value))
	END
	
	RETURN
END