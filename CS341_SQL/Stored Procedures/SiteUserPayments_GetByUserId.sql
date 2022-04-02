-- =============================================
-- Author:		Zach Goethel
-- Create date: Mar. 29, 2022
-- Description:	Gets a set of payments meeting a condition
-- =============================================
CREATE PROCEDURE SiteUserPayments_GetByUserId
	@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Fetch all payments associated with the user's ID
	SELECT
		[UserId],
		[Amount],
		[CardNumber],
		[SecurityCode],
		[PostalCode],
		[HolderName],
		[CardExpiry]
	FROM [SiteUserPayments]
	WHERE
		[UserId] = @UserId;
END
