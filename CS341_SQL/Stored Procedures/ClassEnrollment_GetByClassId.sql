-- =============================================
-- Author:		Zach Goethel
-- Create date: Mar. 29, 2022
-- Description:	Gets course enrollment records matching specified parameters
-- =============================================
CREATE PROCEDURE ClassEnrollment_GetByClassId
	@ClassId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Get all enrollment records associated with class
	SELECT [Id],
		[UserId],
		[ClassId],
		[PaymentId],
		[EnrolledDate],
		[PassedYN]
	FROM [ClassEnrollment]
	WHERE
		[ClassId] = @ClassId;
END
