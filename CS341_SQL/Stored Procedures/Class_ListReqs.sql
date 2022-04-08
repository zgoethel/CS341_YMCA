-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 28, 2022
-- Description: Finds distinct prereq entries in all "requires" and "fulfills"
-- =============================================
CREATE PROCEDURE Class_ListReqs
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Make one long CSV with all the CSVs
    DECLARE @AllValuesCsv NVARCHAR(MAX);
    SELECT @AllValuesCsv = STRING_AGG([RequireCsv] + ',' + [FulfillCsv], ',')
    FROM [ClassMain];

    -- Split it out and select distinct values
    SELECT DISTINCT [Value]
    FROM [dbo].[SplitOnDelim](@AllValuesCsv, ',')
    WHERE ISNULL([Value], '') != ''
    ORDER BY [Value];
END
