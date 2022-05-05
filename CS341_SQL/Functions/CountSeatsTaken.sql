CREATE FUNCTION [dbo].[CountSeatsTaken] (
    @ClassId INT)
RETURNS INT
AS
BEGIN
    -- Count number of related enrollment records
    RETURN (
        SELECT COUNT(ce.[Id])
        FROM [ClassEnrollment] ce
            LEFT JOIN [SiteUser] su ON su.[Id] = ce.[UserId]
        WHERE ce.[ClassId] = @ClassId
            AND ISNULL(su.[Enabled], 1) = 1
    );
END