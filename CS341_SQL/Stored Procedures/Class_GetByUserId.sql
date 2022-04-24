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
        -- Count taken seats in enrollment table
        [dbo].CountSeatsTaken([Id]) AS [SeatsTaken],
        -- Calculate whether member enrollment is open
        [dbo].[CheckWindowOpen](
            [AllowEnrollment],
            [MemberEnrollmentStart],
            [MemberEnrollmentDays]) AS [MemberEnrollmentOpen],
        -- Calculate whether non-member enrollment is open
        [dbo].[CheckWindowOpen](
            [AllowEnrollment],
            [NonMemberEnrollmentStart],
            [NonMemberEnrollmentDays]) AS [NonMemberEnrollmentOpen],
        [ClassThumbId],
        [ClassPhotoId],
        [CanceledDate]
    FROM [ClassMain] cm
    WHERE
        -- Check that class is in user's enrollment set
        [Id] IN (
            SELECT [ClassId]
            FROM [ClassEnrollment]
            WHERE [UserId] = @UserId)
    ORDER BY [Id];
END
