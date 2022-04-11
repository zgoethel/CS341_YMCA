CREATE FUNCTION [dbo].[SplitOnDelim] (
    @List VARCHAR(MAX),
    @SplitOn VARCHAR(5))
RETURNS @Results TABLE (VALUE VARCHAR(MAX))
AS
BEGIN
    WHILE (CHARINDEX(@SplitOn, @List) > 0)
    BEGIN

        INSERT INTO @Results (VALUE)
        SELECT Value = LTRIM(
            RTRIM(
                SUBSTRING(@List, 1, CHARINDEX(@SplitOn, @List) - 1)
            )
        );
        
        SET @List = SUBSTRING(@List, CHARINDEX(@SplitOn, @List) + LEN(@SplitOn), LEN(@List));
        
    END
    
    INSERT INTO @Results (VALUE)
    SELECT Value = LTRIM(RTRIM(@List));
    
    RETURN
END