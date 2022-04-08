-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 29, 2022
-- Description: Enrolls a user in the class, taking a seat
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

    -- Ensure the user isn't already enrolled
    IF ([dbo].[UserIsEnrolled](@UserId, @ClassId) = 1)
    BEGIN
        -- "Revert" payment
        IF @PaymentId IS NOT NULL EXEC [dbo].[SiteUserPayments_DeleteById] @PaymentId;

        RAISERROR('There already exists an enrollment for this user and class. Any pending payment was canceled.', 18, 1);
        RETURN;
    END

    -- Fetch misc. required class data
    DECLARE @MaxSeats INT, @AllowNonMembers BIT;
    SELECT 
        @MaxSeats = [MaxSeats],
        @AllowNonMembers = [AllowNonMembers] 
    FROM [ClassMain]
    WHERE [Id] = @ClassId;

    -- Fetch enrollment counts
    DECLARE @TakenSeats INT = [dbo].[CountSeatsTaken](@ClassId);
    -- Fetch user membership data
    DECLARE @IsMember BIT = [dbo].[UserIsMember](@UserId, NULL);

    -- Perform calculations from other procedure
    DECLARE @Calculations TABLE (
        [IsEnrolled] BIT,
        [ThisUserCost] DECIMAL(10, 2),
        [CanEnroll] BIT,
        [OpenForUser] BIT,
        [UnlimitedSeats] BIT,
        [ClosedForUser] BIT,
        [EnrollmentOpen] DATETIME,
        [EnrollmentClose] DATETIME);
    -- Perform and store the calculations
    INSERT INTO @Calculations
    EXEC [dbo].[Class_CalculateDetails] @ClassId, @UserId;

    IF ((SELECT [OpenForUser] FROM @Calculations) = 0)
    BEGIN
        -- "Revert" payment
        IF @PaymentId IS NOT NULL EXEC [dbo].[SiteUserPayments_DeleteById] @PaymentId;

        RAISERROR('Enrollment failed as the enrollment window is not open. Any pending payment was canceled.', 18, 1);
        RETURN;
    END
    IF (ISNULL(@IsMember, 0) = 0 AND @AllowNonMembers = 0)
    BEGIN
        -- "Revert" payment
        IF @PaymentId IS NOT NULL EXEC [dbo].[SiteUserPayments_DeleteById] @PaymentId;

        RAISERROR('Only users with an active membership can enroll in this course. Any pending payment was canceled.', 18, 1);
        RETURN;
    END
    IF ((SELECT [ThisUserCost] FROM @Calculations) > 0 AND @PaymentId IS NULL)
    BEGIN
        -- "Revert" payment
        IF @PaymentId IS NOT NULL EXEC [dbo].[SiteUserPayments_DeleteById] @PaymentId;

        RAISERROR('This class is not free, but no payment was provided. Any pending payment was canceled.', 18, 1);
        RETURN;
    END
    IF (ISNULL(@MaxSeats, 0) != 0 AND @TakenSeats >= @MaxSeats)
    BEGIN
        -- "Revert" payment
        IF @PaymentId IS NOT NULL EXEC [dbo].[SiteUserPayments_DeleteById] @PaymentId;

        -- Throw error to prevent over-enrollment
        RAISERROR('Enrollment failed as all seats are taken for this course. Any pending payment was canceled.', 18, 1);
        RETURN;
    END
    
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
