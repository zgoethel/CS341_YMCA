-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 29, 2022
-- Description: Gets a set of payments meeting a condition
-- =============================================
CREATE PROCEDURE [dbo].[SiteUserPayments_GetById]
    @Id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT sup.[Id],
        sup.[UserId],
        [Amount],
        [CardNumber],
        [SecurityCode],
        [PostalCode],
        [HolderName],
        [CardExpiry],
        [Paid],
        cm.[ClassName] AS [Item]
    FROM [SiteUserPayments] sup
    LEFT JOIN [ClassEnrollment] ce ON ce.[PaymentId] = sup.[Id]
    LEFT JOIN [ClassMain] cm ON cm.[Id] = ce.[ClassId]
    WHERE
        sup.[Id] = @Id;
END
