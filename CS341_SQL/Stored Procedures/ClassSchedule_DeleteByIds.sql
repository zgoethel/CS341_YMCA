-- =============================================
-- Author:      Zach Goethel
-- Create date: Apr. 3, 2022
-- Description: Deletes class schedule sessions from a list of IDs
-- =============================================
CREATE PROCEDURE [dbo].[ClassSchedule_DeleteByIds]
    @IdCsv NVARCHAR(MAX)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DELETE FROM [ClassSchedule]
    WHERE
        [Id] IN (SELECT [Id] FROM [dbo].[SplitId](@IdCsv, ','));
END
