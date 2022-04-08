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

    -- Delete the related class enrollment record
    DELETE FROM [ClassEnrollment]
    WHERE
        [UserId] = @UserId
        AND [ClassId] = @ClassId;
END
