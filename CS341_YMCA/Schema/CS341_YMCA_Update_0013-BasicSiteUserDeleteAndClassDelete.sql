﻿/*
Deployment script for CS341_YMCA

This code was notgenerated by a tool.
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
PRINT N'Creating Procedure [dbo].[Class_DeleteById]...';


GO
CREATE PROCEDURE [dbo].[Class_DeleteById]
	-- Add the parameters for the stored procedure here
	@Id INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM [ClassMain]
	WHERE
		[Id] = @Id;

	END
GO
PRINT N'Creating Procedure [dbo].[SiteUser_DeleteById]...';


GO
CREATE PROCEDURE [dbo].[SiteUser_DeleteById]
	-- Add the parameters for the stored procedure here
	@Id INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM [SiteUser]
	WHERE
		[Id] = @Id;

	END
GO
PRINT N'Update complete.';


GO
