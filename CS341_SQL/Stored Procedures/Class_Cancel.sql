-- =============================================
-- Author:      Zach Goethel
-- Create date: Apr. 24, 2022
-- Description: Marks the class as canceled, remembering when it happened
-- =============================================
CREATE PROCEDURE [dbo].[Class_Cancel]
    -- Provide ID of zero or `NULL` to insert a new record
    @Id INT,
    @IsUndo BIT = 0
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Validate the the canceled class exists
    DECLARE @ClassFound INT = (
        SELECT TOP 1 [Id]
        FROM [ClassMain]
        WHERE [Id] = @Id);
    IF (@ClassFound IS NULL)
    BEGIN
        RAISERROR('That class cannot be canceld as it does not exist.', 18, 1);
        RETURN;
    END

    -- Cancel (or undo the cancelation) of the class
    UPDATE [ClassMain]
    SET
        [CanceledDate] = CASE WHEN @IsUndo = 1 THEN NULL ELSE GETDATE() END
    WHERE [Id] = @Id;
END
