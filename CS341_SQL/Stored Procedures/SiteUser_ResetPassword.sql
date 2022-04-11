-- =============================================
-- Author:      Zach Goethel
-- Create date: Jan. 26, 2022
-- Description: Allows a user to (re)set their password via an email link
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_ResetPassword]
    @ResetToken UNIQUEIDENTIFIER,
    @PasswordHash NVARCHAR(100)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Attempt to find the user based on the token
    DECLARE @ResetUser INT = (
        SELECT TOP 1 [Id] FROM [SiteUser]
        WHERE [ResetToken] = @ResetToken);

    IF (@ResetUser IS NULL)
    BEGIN
        RAISERROR('Could not find provided token. Please try again.', 18, 1);
        RETURN;
    END
    ELSE
    BEGIN
        -- Update the password when token is valid
        UPDATE [SiteUser]
        SET
            [ResetToken] = NULL,
            [PasswordHash] = @PasswordHash
        WHERE
            [Id] = @ResetUser;
    END
END
