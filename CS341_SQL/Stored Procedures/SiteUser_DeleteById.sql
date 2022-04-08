-- =============================================
-- Author:      Mathew Krings
-- Create date: Mar. 2, 2022
-- Description: Deletes an account, its payments, and its enrollment
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_DeleteById]
    @Id INT = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DELETE FROM [SiteUser]
    WHERE
        [Id] = @Id;

    -- See `Class_DeleteById` for explanation of these commented queries
    -- DELETE FROM [ClassEnrollment] WHERE [UserId] NOT IN (SELECT [Id] FROM [SiteUser] WHERE [Id] = [UserId]);
    DELETE FROM [ClassEnrollment]
    WHERE
        [UserId] = @Id;
    -- DELETE FROM [SiteUserPayments] WHERE [UserId] NOT IN (SELECT [Id] FROM [SiteUser] WHERE [Id] = [UserId]);
    DELETE FROM [SiteUserPayments]
    WHERE
        [UserId] = @Id;
END