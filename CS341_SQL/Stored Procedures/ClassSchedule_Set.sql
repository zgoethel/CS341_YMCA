-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 1, 2022
-- Description: Saves the details of the course session
-- =============================================
CREATE PROCEDURE [dbo].[ClassSchedule_Set]
    -- Provide ID of zero or `NULL` to insert a new record
    @Id INT = NULL,
    @ClassId INT = NULL,
    @FirstDate DATETIME = NULL,
    @Recurrence INT = NULL,
    @Duration INT = NULL,
    @Occurrences INT = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    IF ISNULL(@Id, 0) = 0
    BEGIN
        -- Create the new record
        INSERT INTO [ClassSchedule]
        (
            [ClassId],
            [FirstDate],
            [Recurrence],
            [Duration],
            [Occurrences]
        ) VALUES
        (
            @ClassId,
            @FirstDate,
            @Recurrence,
            @Duration,
            @Occurrences
        );
        -- Return the newly created ID
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS [Id];
    END
    ELSE
    BEGIN
        -- Update the existing record
        UPDATE [ClassSchedule]
        SET
            [ClassId] = ISNULL(@ClassId, [ClassId]),
            [FirstDate] = ISNULL(@FirstDate, [FirstDate]),
            [Recurrence] = ISNULL(@Recurrence, [Recurrence]),
            [Duration] = ISNULL(@Duration, [Duration]),
            [Occurrences] = ISNULL(@Occurrences, [Occurrences])
        WHERE
            [Id] = @Id;
        -- Return the existing ID
        SELECT @Id AS [Id];
    END
END
