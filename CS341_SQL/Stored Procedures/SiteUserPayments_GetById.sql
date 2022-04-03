-- =============================================
-- Author:		Zach Goethel
-- Create date: Mar. 29, 2022
-- Description:	Gets a set of payments meeting a condition
-- =============================================
CREATE PROCEDURE [dbo].[SiteUserPayments_GetById]
	@Id INT
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
		[CardExpiry]
	FROM [SiteUserPayments]
	WHERE
		[Id] = @Id;
END
