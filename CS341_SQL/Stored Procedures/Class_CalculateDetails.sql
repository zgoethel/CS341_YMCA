-- =============================================
-- Author:      Zach Goethel
-- Create date: Apr. 7, 2022
-- Description: Calculates member-specific data for the enrollment view
-- =============================================
CREATE PROCEDURE [dbo].[Class_CalculateDetails]
    @ClassId INT,
    @UserId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Attempt to find an enrollment record for the user
    DECLARE @EnrollmentId INT = (
        SELECT TOP 1 [Id] FROM [ClassEnrollment]
        WHERE [ClassId] = @ClassId AND [UserId] = @UserId);
    -- Fetch provided user's membership
    DECLARE @UserMemberThru DATETIME = (
        SELECT TOP 1 [MemberThru] FROM [SiteUser]
        WHERE [Id] = @UserId);
    DECLARE @IsMember BIT = CASE WHEN @UserMemberThru > GETDATE() THEN 1 ELSE 0 END;

    DECLARE @EnrollmentOpen DATETIME;
    DECLARE @EnrollmentClose DATETIME;
    -- Determine this user's enrollment range
    SELECT

        @EnrollmentOpen = (CASE WHEN @IsMember = 1
            -- For members, take earliest open date
            THEN (SELECT MIN(i) FROM (VALUES
                    ([MemberEnrollmentStart]),
                    (ISNULL([NonMemberEnrollmentStart], [MemberEnrollmentStart]))
                ) AS T(i))
            -- For non-members, take their date
            ELSE [NonMemberEnrollmentStart] END),

        @EnrollmentClose = (CASE WHEN @IsMember = 1
            -- For members, take the latest close date
            THEN (SELECT MAX(i) FROM (VALUES
                    (DATEADD(DAY, [MemberEnrollmentDays], [MemberEnrollmentStart])), 
                    (DATEADD(DAY, ISNULL([NonMemberEnrollmentDays], [MemberEnrollmentDays]),
                        ISNULL([NonMemberEnrollmentStart], [MemberEnrollmentStart])))
                ) AS T(i))
            -- For non-members, take their date
            ELSE DATEADD(DAY, [NonMemberEnrollmentDays], [NonMemberEnrollmentStart]) END)

    FROM [ClassMain]
    WHERE [Id] = @ClassId;

    SELECT
        (CASE WHEN @EnrollmentId IS NULL THEN 0 ELSE 1 END) AS [IsEnrolled],
        (CASE WHEN @IsMember = 1 THEN [MemberPrice] ELSE [NonMemberPrice] END) AS [ThisUserCost],
        (CASE WHEN @IsMember = 1 OR [AllowNonMembers] = 1 THEN 1 ELSE 0 END) AS [CanEnroll],
        (CASE WHEN GETDATE() >= @EnrollmentOpen AND GETDATE() < @EnrollmentClose THEN 1 ELSE 0 END) AS [OpenForUser],
        (CASE WHEN ISNULL([MaxSeats], 0) = 0 THEN 1 ELSE 0 END) AS [UnlimitedSeats],
        (CASE WHEN GETDATE() >= @EnrollmentClose THEN 1 ELSE 0 END) AS [ClosedForUser],
        @EnrollmentOpen AS [EnrollmentOpen],
        @EnrollmentClose AS [EnrollmentClose]
    FROM [ClassMain]
    WHERE [Id] = @ClassId;
END
