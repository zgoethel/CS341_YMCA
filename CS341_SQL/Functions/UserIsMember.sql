CREATE FUNCTION [dbo].[UserIsMember] (
    @UserId INT,
    @MemberThru DATETIME = NULL)
RETURNS BIT
AS
BEGIN
    -- Fetch provided user's membership if not provided
    IF (@MemberThru IS NULL)
    BEGIN
        SET @MemberThru = (
            SELECT TOP 1 [MemberThru] FROM [SiteUser]
            WHERE [Id] = @UserId);
    END
    
    -- Check window against current date and return
    RETURN CASE WHEN ISNULL(@MemberThru, '1900-01-01') >= GETDATE() THEN 1 ELSE 0 END;
END