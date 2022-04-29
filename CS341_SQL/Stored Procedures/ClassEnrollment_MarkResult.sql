-- =============================================
-- Author:      Zach Goethel
-- Create date: Apr. 29, 2022
-- Description: Finds the proper course enrollment record and marks the class grade
-- =============================================
CREATE PROCEDURE ClassEnrollment_MarkResult
    @userId INT,
    @classId INT,
    @result BIT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Find the proper enrollment record
    DECLARE @Record INT = (
        SELECT [Id]
        FROM [ClassEnrollment]
        WHERE [UserId] = @userId
            AND [ClassId] = @classId);
    -- Verify that the record was found
    IF @Record IS NULL
    BEGIN
        RAISERROR('That user is not enrolled in that class, thus cannot hae a grade.', 18, 1);
        RETURN;
    END

    -- Update the found record with result grade
    UPDATE [ClassEnrollment]
    SET [PassedYN] = @result
    WHERE [Id] = @Record;
END
