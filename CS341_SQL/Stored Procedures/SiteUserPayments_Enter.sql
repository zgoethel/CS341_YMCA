-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 29, 2022
-- Description: Enters a payment record into the database
-- =============================================
CREATE PROCEDURE [SiteUserPayments_Enter]
    @UserId INT,
    @Amount DECIMAL(10, 2),
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

    IF (GETDATE() > EOMONTH(@CardExpiry))
    BEGIN
        RAISERROR('It appears your card has expired; please provide another.', 18, 1);
        RETURN;
    END
    -- Can't find if T-SQL supports better regular expressions
    IF (@CardNumber NOT LIKE '[0-9][0-9][0-9][0-9][ -][0-9][0-9][0-9][0-9][ -][0-9][0-9][0-9][0-9][ -][0-9][0-9][0-9][0-9]'
        AND @CardNumber NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
    BEGIN
        RAISERROR('Provided credit card should number be a series of 16 numerical digits.', 18, 1);
        RETURN;
    END
    IF (@SecurityCode < 100 OR @SecurityCode > 999)
    BEGIN
        RAISERROR('CVC secuirty pins should be 3 numerical digits.', 18, 1);
        RETURN;
    END
    IF (@PostalCode < 10000 OR @PostalCode > 99999)
    BEGIN
        RAISERROR('Billing postal code should be 5 numerical digits.', 18, 1);
        RETURN;
    END

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
