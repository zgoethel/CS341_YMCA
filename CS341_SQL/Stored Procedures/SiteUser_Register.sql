-- =============================================
-- Author:      Zach Goethel
-- Create date: Jan. 26, 2022
-- Description: Registers a user, generating their initial reset token
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_Register]
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50) = NULL,
    @Email NVARCHAR(100),
    @IsAdmin BIT = 0
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Attempt to find an existing user with the same email
    DECLARE @ExistingUser INT =
    (
        SELECT TOP 1 [Id] FROM [SiteUser]
        WHERE [Email] = @Email
    );

    IF (@ExistingUser IS NOT NULL)
    BEGIN
        -- Email is in use already; invalid request
        RAISERROR('That email is already in use by another account.', 18, 1);
        RETURN;
    END
    ELSE
    BEGIN
        -- User details check out; create new record
        INSERT INTO [SiteUser]
        (
            [FirstName],
            [LastName],
            [Email],
            [ResetToken],
            [IsAdmin]
        ) VALUES
        (
            @FirstName,
            @LastName,
            @Email,
            NEWID(),
            @IsAdmin
        );

        -- Return generated account ID and reset token
        SELECT
            [Id],
            [ResetToken]
        FROM [SiteUser]
        WHERE
            [Id] = SCOPE_IDENTITY();
    END
END