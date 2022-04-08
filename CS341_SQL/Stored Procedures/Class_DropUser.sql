-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 29, 2022
-- Description: Drops a user from the class, freeing a seat
-- =============================================
CREATE PROCEDURE Class_DropUser
    @UserId INT,
    @ClassId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Ensure the user was actually enrolled
    DECLARE @RecordId INT = (
        SELECT TOP 1 [Id] FROM [ClassEnrollment]
        WHERE [UserId] = @UserId AND [ClassId] = @ClassId);
    IF (@RecordId IS NULL)
    BEGIN
        RAISERROR('There is no enrollment record for the specified user and class.', 18, 1);
        RETURN;
    END

    -- Delete the related class enrollment record
    DELETE FROM [ClassEnrollment]
    WHERE [Id] = @RecordId;
END
