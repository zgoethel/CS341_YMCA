-- =============================================
-- Author:      Zach Goethel
-- Create date: Feb. 1, 2022
-- Description: Gets user details off of an ID, hiding secrets
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_GetById]
    @Id INT
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
        [dbo].[UserIsMember]([Id], [MemberThru]) AS [IsMember]
    FROM [SiteUser]
    WHERE
        [Id] = @Id;
END
