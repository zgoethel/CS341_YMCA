-- =============================================
-- Author:		Zach Goethel
-- Create date: Jan. 26, 2022
-- Description:	Allows a user to request a password reset email
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_RequestReset]
	@Email NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Attempt to find the user with the right email
	DECLARE @ResetUser INT =
	(
		SELECT TOP 1 [Id] FROM [SiteUser]
		WHERE [Email] = @Email
	);

	IF (@ResetUser IS NOT NULL)
	BEGIN
		-- Create new token if the user exists
		UPDATE [SiteUser]
		SET
			[ResetToken] = NEWID(),
			[PasswordHash] = NULL
		WHERE
			[Id] = @ResetUser;

		-- Return generated password reset token
		SELECT [ResetToken] FROM [SiteUser]
		WHERE [Id] =  @ResetUser;
	END
END
