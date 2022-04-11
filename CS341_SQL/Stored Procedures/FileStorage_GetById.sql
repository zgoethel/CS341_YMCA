-- =============================================
-- Author:      Zach Goethel
-- Create date: Apr. 10, 2022
-- Description: Returns details associated with the provided ID
-- =============================================
CREATE PROCEDURE [dbo].[FileStorage_GetById]
    @Id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Validate that the file exists
    DECLARE @Find INT = (
        SELECT TOP 1 [Id] FROM [FileStorage]
        WHERE [Id] = @Id);
    IF @Find IS NULL
    BEGIN
        RAISERROR('Could not find provided file in application storage.', 18, 1);
        RETURN;
    END

    -- Return the proper details for ID
    SELECT TOP (1000) [Id],
        [StoredName],
        [OriginalName],
        [SizeBytes],
        [MimeType],
        [Uploaded],
        [UploadedBy]
    FROM [FileStorage]
    WHERE
        [Id] = @Id;
END
