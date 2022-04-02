-- =============================================
-- Author:		Zach Goethel
-- Create date: Mar. 1, 2022
-- Description:	Lists all sessions for a specified class
-- =============================================
CREATE PROCEDURE [dbo].[ClassSchedule_List]
	@ClassId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Get all schedule records associated with the class
	SELECT
		[Id],
		[ClassId],
		[FirstDate],
		[Recurrence],
		[Duration],
		[Created],
		[Updated],
		[Occurrences]
	FROM [ClassSchedule]
	WHERE
		[ClassId] = @ClassId
	ORDER BY [FirstDate];
END
