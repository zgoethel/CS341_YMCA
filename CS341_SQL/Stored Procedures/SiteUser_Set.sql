-- =============================================
-- Author:      Zach Goethel
-- Create date: Feb. 26, 2022
-- Description: Sets the values for a user including membership data
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_Set]
    -- Provided ID must not be null (users can not be created here)
    @Id INT = NULL,
    @FirstName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50) = NULL,
    @Email NVARCHAR(100) = NULL,
    @IsAdmin BIT = NULL,
    @MemberThru DATETIME = NULL,
    @FulfilledCsv NVARCHAR(MAX) = NULL,
    @Enabled BIT = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    IF (ISNULL(@Id, 0) != 0)
    BEGIN
        UPDATE [SiteUser]
        SET
            [FirstName] = ISNULL(@FirstName, [FirstName]),
            [LastName] = ISNULL(@LastName, [LastName]),
            [Email] = ISNULL(@Email, [Email]),
            [IsAdmin] = ISNULL(@IsAdmin, [IsAdmin]),
            [MemberThru] = ISNULL(@MemberThru, [MemberThru]),
            [FulfilledCsv] = ISNULL(@FulfilledCsv, [FulfilledCsv]),
            [Modified] = GETDATE(),
            [Enabled] = ISNULL(@Enabled, [Enabled])
        WHERE
            [Id] = @Id;
    END
    ELSE
    BEGIN
        -- Provided ID is null; users cannot be created here
        RAISERROR('Users cannot be created through this procedure.', 18, 1);
        RETURN;
    END
END