CREATE PROCEDURE [dbo].[SiteUser_List]
	@EmailFilter NVARCHAR(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		[Id],
		[FirstName],
		[LastName],
		[Email],
		-- Don't allow procedure to read back "secrets"
		(CASE WHEN [PasswordHash] IS NULL THEN 0 ELSE 1 END) AS [HasPassword],
		(CASE WHEN [ResetToken] IS NULL THEN 0 ELSE 1 END) AS [HasPendingReset],
		[IsAdmin],
		[Created],
		[Modified],
		[MemberThru],
		-- Calculate whether membership is valid or expired
		CASE WHEN (ISNULL([MemberThru], '1900-01-01') >= GETDATE()) THEN 1 ELSE 0 END AS [IsMember]
	FROM [SiteUser]
	WHERE 
		[Email] LIKE '%' + @EmailFilter + '%'
	ORDER BY [Id];

	END