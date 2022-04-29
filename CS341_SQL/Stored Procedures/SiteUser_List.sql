-- =============================================
-- Author:      Zach Goethel
-- Create date: Jan. 26, 2022
-- Description: Lists all of the site users in the database.
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_List]
    @NameFilter NVARCHAR(100) = '',
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
        [dbo].[UserIsMember]([Id], [MemberThru]) AS [IsMember]
    FROM [SiteUser]
    WHERE 
        [FirstName] + ' ' + [LastName] LIKE '%' + @NameFilter + '%'
        AND [Email] LIKE '%' + @EmailFilter + '%'
    ORDER BY [Id];

    END