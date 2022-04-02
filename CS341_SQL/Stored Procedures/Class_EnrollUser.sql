-- =============================================
-- Author:		Zach Goethel
-- Create date: Mar. 29, 2022
-- Description:	Enrolls a user in the class, taking a seat
-- =============================================
CREATE PROCEDURE Class_EnrollUser
	@UserId INT,
	@ClassId INT,
	@PaymentId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Create a class enrollment record
	INSERT INTO [ClassEnrollment]
	(
		[UserId],
		[ClassId],
		[PaymentId],
		[PassedYN]
	) VALUES
	(
		@UserId,
		@ClassId,
		@PaymentId,
		NULL
	)
END
