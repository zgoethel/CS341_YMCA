-- =============================================
-- Author:      Zach Goethel
-- Create date: Feb. 23, 2022
-- Description: Saves the main detail fields of the course
-- =============================================
CREATE PROCEDURE [dbo].[Class_Set]
    -- Provide ID of zero or `NULL` to insert a new record
    @Id INT = NULL,
    @ClassName NVARCHAR(100) = NULL,
    @AllowEnrollment BIT = NULL,
    @Enabled BIT = NULL,
    @ShortDescription NVARCHAR(MAX) = NULL,
    @LongDescription NVARCHAR(MAX) = NULL,
    @MemberEnrollmentStart DATETIME = NULL,
    @MemberEnrollmentDays INT = NULL,
    @NonMemberEnrollmentStart DATETIME = NULL,
    @NonMemberEnrollmentDays INT = NULL,
    @AllowNonMembers BIT = NULL,
    @MemberPrice FLOAT = NULL,
    @NonMemberPrice FLOAT = NULL,
    @Location NVARCHAR(100) = NULL,
    @MaxSeats INT = NULL,
    @FulfillCsv NVARCHAR(MAX) = NULL,
    @RequireCsv NVARCHAR(MAX) = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    IF ISNULL(@Id, 0) = 0
    BEGIN
        -- Create the new record when necessary
        INSERT INTO [ClassMain]
        (
            [ClassName],
            [AllowEnrollment],
            [Enabled],
            [ShortDescription],
            [LongDescription],
            [MemberEnrollmentStart],
            [MemberEnrollmentDays],
            [NonMemberEnrollmentStart],
            [NonMemberEnrollmentDays],
            [AllowNonMembers],
            [MemberPrice],
            [NonMemberPrice],
            [Location],
            [MaxSeats],
            [FulfillCsv],
            [RequireCsv]
        ) VALUES
        (
            ISNULL(@ClassName, ''),
            ISNULL(@AllowEnrollment, 1),
            ISNULL(@Enabled, 1),
            ISNULL(@ShortDescription, ''),
            ISNULL(@LongDescription, ''),
            @MemberEnrollmentStart,
            ISNULL(@MemberEnrollmentDays, 7),
            @NonMemberEnrollmentStart,
            ISNULL(@NonMemberEnrollmentDays, 7),
            ISNULL(@AllowNonMembers, 0),
            ISNULL(@MemberPrice, 0),
            ISNULL(@NonMemberPrice, 0),
            ISNULL(@Location, ''),
            @MaxSeats,
            ISNULL(@FulfillCsv, ''),
            ISNULL(@RequireCsv, '')
        );
        -- Return the newly created ID
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS [Id];
    END
    ELSE
    BEGIN
        -- Update the existing course record
        UPDATE [ClassMain]
        SET
            [ClassName] = ISNULL(@ClassName, [ClassName]),
            [AllowEnrollment] = ISNULL(@AllowEnrollment, [AllowEnrollment]),
            [Enabled] = ISNULL(@Enabled, [Enabled]),
            [Updated] = GETDATE(),
            [ShortDescription] = ISNULL(@ShortDescription, [ShortDescription]),
            [LongDescription] = ISNULL(@LongDescription, [LongDescription]),
            [MemberEnrollmentStart] = ISNULL(@MemberEnrollmentStart, [MemberEnrollmentStart]),
            [MemberEnrollmentDays] = ISNULL(@MemberEnrollmentDays, [MemberEnrollmentDays]),
            [NonMemberEnrollmentStart] = ISNULL(@NonMemberEnrollmentStart, [NonMemberEnrollmentStart]),
            [NonMemberEnrollmentDays] = ISNULL(@NonMemberEnrollmentDays, [NonMemberEnrollmentDays]),
            [AllowNonMembers] = ISNULL(@AllowNonMembers, [AllowNonMembers]),
            [MemberPrice] = ISNULL(@MemberPrice, [MemberPrice]),
            [NonMemberPrice] = ISNULL(@NonMemberPrice, [NonMemberPrice]),
            [Location] = ISNULL(@Location, [Location]),
            [MaxSeats] = ISNULL(@MaxSeats, [MaxSeats]),
            [FulfillCsv] = ISNULL(@FulfillCsv, [FulfillCsv]),
            [RequireCsv] = ISNULL(@RequireCsv, [RequireCsv])
        WHERE
            [Id] = @Id;
        -- Return the same existing ID
        SELECT @Id AS [Id];
    END
END
