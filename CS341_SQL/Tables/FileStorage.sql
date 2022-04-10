CREATE TABLE [dbo].[FileStorage]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StoredName] NVARCHAR(36) NOT NULL, 
    [OriginalName] NVARCHAR(50) NOT NULL, 
    [SizeBytes] INT NOT NULL, 
    [MimeType] NVARCHAR(50) NOT NULL, 
    [Uploaded] DATETIME NOT NULL DEFAULT (GETDATE()), 
    [UploadedBy] INT NULL
)
