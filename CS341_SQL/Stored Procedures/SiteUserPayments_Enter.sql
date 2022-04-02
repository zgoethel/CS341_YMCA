-- =============================================
-- Author:		Zach Goethel
-- Create date: Mar. 29, 2022
-- Description:	Enters a payment record into the database
-- =============================================
CREATE PROCEDURE [SiteUserPayments_Enter]
	@UserId INT,
	@Amount DECIMAL(18, 0),
	@CardNumber NVARCHAR(50),
	@SecurityCode INT,
	@PostalCode INT,
	@HolderName NVARCHAR(100),
	@CardExpiry DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert the payment into the database and create ID
	INSERT INTO [SiteUserPayments]
	(
		[UserId],
		[Amount],
		[CardNumber],
		[SecurityCode],
		[PostalCode],
		[HolderName],
		[CardExpiry]
	) VALUES
	(
		@UserId,
		@Amount,
		@CardNumber,
		@SecurityCode,
		@PostalCode,
		@HolderName,
		@CardExpiry
	);
	-- Return the newly generated payment ID for reference
	SELECT CAST(SCOPE_IDENTITY() AS INT) AS [Id];
END
