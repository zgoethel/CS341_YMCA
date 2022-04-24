-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 1, 2022
-- Description: Lists all sessions for a specified class
-- =============================================
CREATE PROCEDURE [dbo].[ClassSchedule_List]
    @ClassId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Get all schedule records associated with the class
    SELECT cs.[Id],
        [ClassId],
        [FirstDate],
        [Recurrence],
        [Duration],
        cs.[Created],
        cs.[Updated],
        [Occurrences],
        cm.[ClassName],
        cm.[ShortDescription],
        cm.[CanceledDate]
    FROM [ClassSchedule] cs
    LEFT JOIN [ClassMain] cm on cm.[Id] = [ClassId]
    WHERE
        [ClassId] = @ClassId
    ORDER BY [FirstDate];
END
