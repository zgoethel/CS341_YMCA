-- =============================================
-- Author:      Zach Goethel
-- Create date: Apr. 8, 2022
-- Description: Deletes a payment record by the specified ID
-- =============================================
CREATE PROCEDURE SiteUserPayments_DeleteById
    @Id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    DELETE FROM [SiteUserPayments]
    WHERE
        [Id] = @Id;
END
