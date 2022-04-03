CREATE TABLE [dbo].[ClassSchedule] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [ClassId]     INT      NOT NULL,
    [FirstDate]   DATETIME NOT NULL,
    [Recurrence]  INT      CONSTRAINT [DF_ClassSchedule_Recurrence] DEFAULT ((7)) NOT NULL,
    [Duration]    INT      CONSTRAINT [DF_ClassSchedule_Duration] DEFAULT ((60)) NOT NULL,
    [Created]     DATETIME CONSTRAINT [DF_ClassSchedule_Created] DEFAULT (getdate()) NOT NULL,
    [Updated]     DATETIME CONSTRAINT [DF_ClassSchedule_Updated] DEFAULT (getdate()) NOT NULL,
    [Occurrences] INT      NOT NULL,
    CONSTRAINT [PK_ClassSchedule] PRIMARY KEY CLUSTERED ([Id] ASC)
);

