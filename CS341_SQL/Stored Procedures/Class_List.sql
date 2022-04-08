﻿-- =============================================
-- Author:      Zach Goethel
-- Create date: Feb. 23, 2022
-- Description: Lists all courses meeting certain criteria
-- =============================================
CREATE PROCEDURE [dbo].[Class_List]
    @NameFilter NVARCHAR(100) = '',
    @IncludeDisabled BIT = 0
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
		(SELECT COUNT(_cm.[Id]) FROM [ClassEnrollment] _cm WHERE [ClassId] = cm.[Id]) AS [SeatsTaken],
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
    FROM [ClassMain] cm
    WHERE
        -- Only include enabled unless specified
        ([Enabled] = 1 OR @IncludeDisabled = 1)
        -- Check class name against name search filter
        AND [ClassName] LIKE '%' + @NameFilter + '%'
    ORDER BY [Id];
END
