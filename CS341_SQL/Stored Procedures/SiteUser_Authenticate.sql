-- =============================================
-- Author:		Zach Goethel
-- Create date: Jan. 26, 2022
-- Description:	Checks a user's credentials against what is known
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_Authenticate]
	@Email NVARCHAR(100),
	@PasswordHash NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Store individual user values for convenience; values will
	-- be null if not found in the SELECT
	DECLARE @LoginUser INT;
	DECLARE @LoginPassword NVARCHAR(100);
	DECLARE @ExistingToken UNIQUEIDENTIFIER;
	-- Select user record with correct email, set variables
	SELECT TOP 1 
		@LoginUser = [Id],
		@LoginPassword = [PasswordHash],
		@ExistingToken = [ResetToken]
	FROM [SiteUser]
	WHERE [Email] = @Email;

	IF (
		@LoginUser IS NOT NULL AND
		@LoginPassword = @PasswordHash AND
		@ExistingToken IS NULL
	)
	BEGIN
		-- User exists and successful login
		RETURN;
	END
	ELSE IF (
		@ExistingToken IS NOT NULL
	)
	BEGIN
		-- User's account has a pending reset
		RAISERROR('Your account has a pending reset request. Please check your email.', 18, 1);
		RETURN;
	END
	ELSE
	BEGIN
		-- No user found or password didn't match
		RAISERROR('Incorrect email or password. Please try again.', 18, 1);
		RETURN;
	END
END
