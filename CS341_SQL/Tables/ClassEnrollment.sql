CREATE TABLE [dbo].[ClassEnrollment] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [UserId]       INT      NOT NULL,
    [ClassId]      INT      NOT NULL,
    [PaymentId]    INT      NULL,
    [EnrolledDate] DATETIME CONSTRAINT [DF_ClassEnrollment_EnrolledDate] DEFAULT (getdate()) NOT NULL,
    [PassedYN]     BIT      NULL,
    CONSTRAINT [PK_ClassEnrollment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

