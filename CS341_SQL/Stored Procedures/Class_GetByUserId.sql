-- =============================================
-- Author:      Zach Goethel
-- Create date: Feb. 23, 2022
-- Description: Lists all courses meeting certain criteria
-- =============================================
CREATE PROCEDURE [dbo].[Class_GetByUserId]
    @UserId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT
        -- Select stored fields related to the class ID
        [Id],
        [ClassName],
        [AllowEnrollment],
        [Enabled],
        [Created],
        [Updated],
        [ShortDescription],
        [LongDescription],
        [MemberEnrollmentStart],
        [MemberEnrollmentDays],
        [NonMemberEnrollmentStart],
        [NonMemberEnrollmentDays],
        [AllowNonMembers],
        [MemberPrice],
        [NonMemberPrice],
        [Location],
        [MaxSeats],
        [FulfillCsv],
        [RequireCsv],
        -- Calculate whether member enrollment is open
        CASE WHEN (
            [AllowEnrollment] = 1
            AND [MemberEnrollmentStart] IS NOT NULL
            AND [MemberEnrollmentDays] IS NOT NULL
            AND GETDATE() > [MemberEnrollmentStart]
            AND GETDATE() < DATEADD(DAY, [MemberEnrollmentDays], [MemberEnrollmentStart])
        ) THEN 1 ELSE 0 END AS [MemberEnrollmentOpen],
        -- Calculate whether non-member enrollment is open
        CASE WHEN (
            [AllowEnrollment] = 1
            AND [NonMemberEnrollmentStart] IS NOT NULL
            AND [NonMemberEnrollmentDays] IS NOT NULL
            AND GETDATE() > [NonMemberEnrollmentStart]
            AND GETDATE() < DATEADD(DAY, [NonMemberEnrollmentDays], [NonMemberEnrollmentStart])
        ) THEN 1 ELSE 0 END AS [NonMemberEnrollmentOpen]
    FROM [ClassMain]
    WHERE
        -- Check that class is in user's enrollment set
        [Id] IN (
            SELECT [ClassId]
            FROM [ClassEnrollment]
            WHERE [UserId] = @UserId				
        )
    ORDER BY [Id];
END
