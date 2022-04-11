-- =============================================
-- Author:      Zach Goethel
-- Create date: Feb. 24, 2022
-- Description: Returns details associated with the provided IDs
-- =============================================
CREATE PROCEDURE [dbo].[Class_GetByIds]
    @Csv NVARCHAR(MAX)
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
        [ClassPhotoId]
    FROM [ClassMain] cm
    WHERE
        [Id] IN (SELECT [Id] FROM [dbo].[SplitIdOnDelim](@Csv, ','));
END
