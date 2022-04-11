CREATE FUNCTION [dbo].[CheckWindowOpen] (
    @AllowEnrollment BIT,
    @EnrollmentStart DATETIME,
    @EnrollmentDays INT)
RETURNS BIT
AS
BEGIN
    -- Check window against current date and return
    RETURN CASE WHEN (
        @AllowEnrollment = 1
        AND @EnrollmentStart IS NOT NULL
        AND @EnrollmentDays IS NOT NULL
        AND GETDATE() > @EnrollmentStart
        AND GETDATE() < DATEADD(DAY, @EnrollmentDays, @EnrollmentStart))
    THEN 1 ELSE 0 END;
END