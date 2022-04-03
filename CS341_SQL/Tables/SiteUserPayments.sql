CREATE TABLE [dbo].[SiteUserPayments] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [UserId]       INT            NOT NULL,
    [Amount]       DECIMAL (18)   NOT NULL,
    [Paid]         DATETIME       CONSTRAINT [DF_SiteUser_Payments_Paid] DEFAULT (getdate()) NOT NULL,
    [CardNumber]   NVARCHAR (50)  NOT NULL,
    [SecurityCode] INT            NOT NULL,
    [PostalCode]   INT            NOT NULL,
    [HolderName]   NVARCHAR (100) NOT NULL,
    [CardExpiry]   DATETIME       NOT NULL, 
    CONSTRAINT [PK_SiteUser_Payments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

