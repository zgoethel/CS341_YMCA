-- =============================================
-- Author:		Zach Goethel
-- Create date: Mar. 29, 2022
-- Description:	Gets a set of payments meeting a condition
-- =============================================
CREATE PROCEDURE SiteUserPayments_List
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

	SELECT [Id],
		[UserId],
		[Amount],
		[CardNumber],
		[SecurityCode],
		[PostalCode],
		[HolderName],
		[CardExpiry],
		[Paid],
		-- Search enrollment for references to purchase
		(
			SELECT [ClassName]
			FROM [SiteUserPayments] sup
			LEFT JOIN [ClassEnrollment] ce on ce.[PaymentId] = sup.[Id]
			LEFT JOIN [ClassMain] cm ON cm.Id = ce.ClassId
			WHERE ce.[PaymentId] IS NOT NULL
				AND sup0.[Id] = sup.[Id]
		) AS [Item]
	FROM [SiteUserPayments] sup0;
END
