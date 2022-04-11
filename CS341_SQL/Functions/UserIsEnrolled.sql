CREATE FUNCTION [dbo].[UserIsEnrolled] (
    @UserId NVARCHAR(MAX),
    @ClassId DATETIME = NULL)
RETURNS BIT
AS
BEGIN
    -- Attempt to find an enrollment record for the user
    DECLARE @EnrollmentId INT = (
        SELECT TOP 1 [Id] FROM [ClassEnrollment]
        WHERE [ClassId] = @ClassId AND [UserId] = @UserId);
    
    -- Check if enrollment was found and return
    RETURN CASE WHEN @EnrollmentId IS NULL THEN 0 ELSE 1 END;
END