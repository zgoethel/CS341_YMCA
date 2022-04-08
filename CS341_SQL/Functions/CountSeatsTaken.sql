CREATE FUNCTION [dbo].[CountSeatsTaken] (
    @ClassId INT)
RETURNS INT
AS
BEGIN
    -- Count number of related enrollment records
    RETURN (SELECT COUNT([Id]) FROM [ClassEnrollment] WHERE [ClassId] = @ClassId);
END