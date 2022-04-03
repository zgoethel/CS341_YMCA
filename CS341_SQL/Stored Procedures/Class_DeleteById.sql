-- =============================================
-- Author:		Mathew Krings
-- Create date: Mar. 2, 2022
-- Description:	Deletes a class, its schedule, and its enrollment
-- =============================================
CREATE PROCEDURE [dbo].[Class_DeleteById]
	@Id INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM [ClassMain]
	WHERE
		[Id] = @Id;

	-- NOTE
	--
	-- You can use this `DELETE` query to flush out schedules not linked
	-- to a class (e.g., in case a class was deleted manually via SQL).
	--
	-- DELETE FROM [ClassSchedule] WHERE [ClassId] NOT IN (SELECT [Id] FROM [ClassMain] WHERE [Id] = [ClassId]);
	DELETE FROM [ClassSchedule]
	WHERE
		[ClassId] = @Id;
	-- DELETE FROM [ClassEnrollment] WHERE [ClassId] NOT IN (SELECT [Id] FROM [ClassMain] WHERE [Id] = [ClassId]);
	DELETE FROM [ClassEnrollment]
	WHERE
		[ClassId] = @Id;
END