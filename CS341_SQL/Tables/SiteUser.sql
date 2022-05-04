CREATE TABLE [dbo].[SiteUser] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (50)    NOT NULL,
    [LastName]     NVARCHAR (50)    NULL,
    [Email]        NVARCHAR (100)   NOT NULL,
    [PasswordHash] NVARCHAR (100)   NULL,
    [ResetToken]   UNIQUEIDENTIFIER NULL,
    [IsAdmin]      BIT              CONSTRAINT [DF_SiteUser_IsAdmin] DEFAULT ((0)) NOT NULL,
    [Created]      DATETIME         CONSTRAINT [DF_SiteUser_Created] DEFAULT (getdate()) NOT NULL,
    [Modified]     DATETIME         CONSTRAINT [DF_SiteUser_Modified] DEFAULT (getdate()) NOT NULL,
    [MemberThru]   DATETIME         NULL,
    [FulfilledCsv] NVARCHAR (MAX)   CONSTRAINT [DF_SiteUser_FulfilledCsv] DEFAULT ('') NOT NULL,
    [Enabled]      BIT              NULL,
    CONSTRAINT [PK_SiteUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);



