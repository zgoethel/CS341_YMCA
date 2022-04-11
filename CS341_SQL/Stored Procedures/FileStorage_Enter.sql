-- =============================================
-- Author:      Zach Goethel
-- Create date: Apr. 10, 2022
-- Description: Saves the file details to track uploaded/stored files
-- =============================================
CREATE PROCEDURE [dbo].[FileStorage_Enter]
    -- Provide ID of zero or `NULL` to insert a new record
    @StoredName NVARCHAR(50),
    @OriginalName NVARCHAR(50),
    @SizeBytes INT,
    @MimeType NVARCHAR(50),
    @UploadedBy INT = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO [FileStorage] (
        [StoredName],
        [OriginalName],
        [SizeBytes],
        [MimeType],
        [UploadedBy]
    ) VALUES (
        @StoredName,
        @OriginalName,
        @SizeBytes,
        @MimeType,
        @UploadedBy
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS [Id];
END
