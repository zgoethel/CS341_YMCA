-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 1, 2022
-- Description: Lists all sessions for a specified user's enrollment
-- =============================================
CREATE PROCEDURE [dbo].[ClassSchedule_GetByUserId]
    @UserId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Get all schedule records associated with the user
    SELECT cs.[Id],
        [ClassId],
        [FirstDate],
        [Recurrence],
        [Duration],
        cs.[Created],
        cs.[Updated],
        [Occurrences],
        cm.[ClassName],
        cm.[ShortDescription]
    FROM [ClassSchedule] cs
    LEFT JOIN [ClassMain] cm on cm.[Id] = [ClassId]
    WHERE
        -- Check each ID to see if it's in the enrollment set
        [ClassId] IN (
            SELECT [ClassId]
            FROM [ClassEnrollment]
            WHERE [UserId] = @UserId
        )
    ORDER BY [FirstDate];
END
