CREATE TABLE [dbo].[ClassMain] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [ClassName]                NVARCHAR (100) NOT NULL,
    [AllowEnrollment]          BIT            CONSTRAINT [DF_ClassMain_AllowEnrollment] DEFAULT ((1)) NOT NULL,
    [Enabled]                  BIT            CONSTRAINT [DF_ClassMain_Enabled] DEFAULT ((1)) NOT NULL,
    [Created]                  DATETIME       CONSTRAINT [DF_ClassMain_Created] DEFAULT (getdate()) NOT NULL,
    [Updated]                  DATETIME       CONSTRAINT [DF_ClassMain_Updated] DEFAULT (getdate()) NOT NULL,
    [ShortDescription]         NVARCHAR (MAX) CONSTRAINT [DF_ClassMain_ShortDescription] DEFAULT ('') NOT NULL,
    [LongDescription]          NVARCHAR (MAX) CONSTRAINT [DF_ClassMain_LongDescription] DEFAULT ('') NOT NULL,
    [MemberEnrollmentStart]    DATETIME       NULL,
    [MemberEnrollmentDays]     INT            CONSTRAINT [DF_ClassMain_MemberEnrollmentDays] DEFAULT ((7)) NOT NULL,
    [NonMemberEnrollmentStart] DATETIME       NULL,
    [NonMemberEnrollmentDays]  INT            CONSTRAINT [DF_ClassMain_NonMemberEnrollmentDays] DEFAULT ((7)) NOT NULL,
    [AllowNonMembers]          BIT            CONSTRAINT [DF_ClassMain_AllowNonMembers] DEFAULT ((0)) NOT NULL,
    [MemberPrice]              DECIMAL(10, 2)     CONSTRAINT [DF_ClassMain_MemberPrice] DEFAULT ((0)) NOT NULL,
    [NonMemberPrice]           DECIMAL(10, 2)     CONSTRAINT [DF_ClassMain_NonMemberPrice] DEFAULT ((0)) NOT NULL,
    [Location]                 NVARCHAR (100) NULL,
    [MaxSeats]                 INT            NULL,
    [FulfillCsv]               NVARCHAR (MAX) CONSTRAINT [DF_ClassMain_FulfillCsv] DEFAULT ('') NOT NULL,
    [RequireCsv]               NVARCHAR (MAX) CONSTRAINT [DF_ClassMain_RequireCsv] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ClassMain] PRIMARY KEY CLUSTERED ([Id] ASC)
);

