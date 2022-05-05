-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 29, 2022
-- Description: Gets course enrollment records matching specified parameters
-- =============================================
CREATE PROCEDURE ClassEnrollment_GetByClassId
    @ClassId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Get all enrollment records associated with class
    SELECT ce.[Id],
        [UserId],
        [ClassId],
        [PaymentId],
        [EnrolledDate],
        [PassedYN],
        cm.[ClassName],
        cm.[ShortDescription],
        su.[FirstName],
        su.[LastName],
        su.[Email],
        [dbo].[UserIsMember]([UserId], su.[MemberThru]) AS [IsMember],
        cm.[CanceledDate],
        ISNULL(su.[Enabled], 1) AS [UserEnabled]
    FROM [ClassEnrollment] ce
    LEFT JOIN [ClassMain] cm on cm.[Id] = [ClassId]
    LEFT JOIN [SiteUser] su on su.[Id] = [UserId]
    WHERE
        [ClassId] = @ClassId;
END
