﻿/*
Deployment script for CS341_YMCA

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "CS341_YMCA"
:setvar DefaultFilePrefix "CS341_YMCA"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Altering Procedure [dbo].[Class_Set]...';


GO
-- =============================================
-- Author:		Zach Goethel
-- Create date: Feb. 23, 2022
-- Description:	Saves the main details of the course
-- =============================================
ALTER PROCEDURE [dbo].[Class_Set] 
	-- Add the parameters for the stored procedure here
	@Id INT = NULL,
	@ClassName NVARCHAR(100) = NULL,
	@AllowEnrollment BIT = NULL,
	@Enabled BIT = NULL,
	@ShortDescription NVARCHAR(MAX) = NULL,
	@LongDescription NVARCHAR(MAX) = NULL,
	@PrereqIds NVARCHAR(MAX) = NULL,
	@MemberEnrollmentStart DATETIME = NULL,
	@MemberEnrollmentDays INT = NULL,
	@NonMemberEnrollmentStart DATETIME = NULL,
	@NonMemberEnrollmentDays INT = NULL,
	@AllowNonMembers BIT = NULL,
	@MemberPrice FLOAT = NULL,
	@NonMemberPrice FLOAT = NULL,
	@Location NVARCHAR(100) = NULL,
	@MaxSeats INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF ISNULL(@Id, 0) = 0
	BEGIN
		-- Create the new record
		INSERT INTO [ClassMain]
		(
			[ClassName],
			[AllowEnrollment],
			[Enabled],
			[ShortDescription],
			[LongDescription],
			[PrereqIds],
			[MemberEnrollmentStart],
			[MemberEnrollmentDays],
			[NonMemberEnrollmentStart],
			[NonMemberEnrollmentDays],
			[AllowNonMembers],
			[MemberPrice],
			[NonMemberPrice],
			[Location],
			[MaxSeats]
		) VALUES
		(
			ISNULL(@ClassName, ''),
			ISNULL(@AllowEnrollment, 1),
			ISNULL(@Enabled, 1),
			ISNULL(@ShortDescription, ''),
			ISNULL(@LongDescription, ''),
			ISNULL(@PrereqIds, ''),
			@MemberEnrollmentStart,
			ISNULL(@MemberEnrollmentDays, 7),
			@NonMemberEnrollmentStart,
			ISNULL(@NonMemberEnrollmentDays, 7),
			ISNULL(@AllowNonMembers, 0),
			ISNULL(@MemberPrice, 0),
			ISNULL(@NonMemberPrice, 0),
			ISNULL(@Location, ''),
			@MaxSeats
		);
		-- Return the new ID
		SELECT CAST(SCOPE_IDENTITY() AS INT) AS [Id];
	END
	ELSE
	BEGIN
		-- Update the existing record
		UPDATE [ClassMain]
		SET
			[ClassName] = ISNULL(@ClassName, [ClassName]),
			[AllowEnrollment] = ISNULL(@AllowEnrollment, [AllowEnrollment]),
			[Enabled] = ISNULL(@Enabled, [Enabled]),
			[Updated] = GETDATE(),
			[ShortDescription] = ISNULL(@ShortDescription, [ShortDescription]),
			[LongDescription] = ISNULL(@LongDescription, [LongDescription]),
			[PrereqIds] = ISNULL(@PrereqIds, [PrereqIds]),
			[MemberEnrollmentStart] = ISNULL(@MemberEnrollmentStart, [MemberEnrollmentStart]),
			[MemberEnrollmentDays] = ISNULL(@MemberEnrollmentDays, [MemberEnrollmentDays]),
			[NonMemberEnrollmentStart] = ISNULL(@NonMemberEnrollmentStart, [NonMemberEnrollmentStart]),
			[NonMemberEnrollmentDays] = ISNULL(@NonMemberEnrollmentDays, [NonMemberEnrollmentDays]),
			[AllowNonMembers] = ISNULL(@AllowNonMembers, [AllowNonMembers]),
			[MemberPrice] = ISNULL(@MemberPrice, [MemberPrice]),
			[NonMemberPrice] = ISNULL(@NonMemberPrice, [NonMemberPrice]),
			[Location] = ISNULL(@Location, [Location]),
			[MaxSeats] = ISNULL(@MaxSeats, [MaxSeats])
		WHERE
			[Id] = @Id;
		-- Return the same ID
		SELECT @Id AS [Id];
	END
END
GO
PRINT N'Altering Procedure [dbo].[ClassSchedule_Set]...';


GO
-- =============================================
-- Author:		Zach Goethel
-- Create date: Mar. 1, 2022
-- Description:	Saves the details of the course session
-- =============================================
ALTER PROCEDURE [dbo].[ClassSchedule_Set] 
	-- Add the parameters for the stored procedure here
	@Id INT = NULL,
	@ClassId INT = NULL,
	@FirstDate DATETIME = NULL,
	@Recurrence INT = NULL,
	@Duration INT = NULL,
	@Occurrences INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF ISNULL(@Id, 0) = 0
	BEGIN
		-- Create the new record
		INSERT INTO [ClassSchedule]
		(
			[ClassId],
			[FirstDate],
			[Recurrence],
			[Duration],
			[Occurrences]
		) VALUES
		(
			@ClassId,
			@FirstDate,
			@Recurrence,
			@Duration,
			@Occurrences
		);
		-- Return the new ID
		SELECT CAST(SCOPE_IDENTITY() AS INT) AS [Id];
	END
	ELSE
	BEGIN
		-- Update the existing record
		UPDATE [ClassSchedule]
		SET
			[ClassId] = ISNULL(@ClassId, [ClassId]),
			[FirstDate] = ISNULL(@FirstDate, [FirstDate]),
			[Recurrence] = ISNULL(@Recurrence, [Recurrence]),
			[Duration] = ISNULL(@Duration, [Duration]),
			[Occurrences] = ISNULL(@Occurrences, [Occurrences])
		WHERE
			[Id] = @Id;
		-- Return the same ID
		SELECT @Id AS [Id];
	END
END
GO
PRINT N'Creating Procedure [dbo].[SiteUser_Set]...';


GO
-- =============================================
-- Author:		Zach Goethel
-- Create date: Feb. 26, 2022
-- Description:	Sets the values for a user including membership data
-- =============================================
CREATE PROCEDURE [dbo].[SiteUser_Set]
	-- Add the parameters for the stored procedure here
	@Id INT = NULL,
	@FirstName NVARCHAR(50) = NULL,
	@LastName NVARCHAR(50) = NULL,
	@Email NVARCHAR(100) = NULL,
	@IsAdmin BIT = NULL,
	@MemberThru DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (ISNULL(@Id, 0) != 0)
	BEGIN
		UPDATE [SiteUser]
		SET
			[FirstName] = ISNULL(@FirstName, [FirstName]),
			[LastName] = ISNULL(@LastName, [LastName]),
			[Email] = ISNULL(@Email, [Email]),
			[IsAdmin] = ISNULL(@IsAdmin, [IsAdmin]),
			[MemberThru] = ISNULL(@MemberThru, [MemberThru])
		WHERE
			[Id] = @Id
			;
	END
	ELSE
	BEGIN
		-- User details check out; create new record
		RAISERROR('Users cannot be created through this procedure.', 18, 1);
		RETURN;
	END
END
GO
PRINT N'Update complete.';


GO