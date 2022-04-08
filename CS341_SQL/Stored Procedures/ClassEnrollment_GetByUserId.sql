-- =============================================
-- Author:      Zach Goethel
-- Create date: Mar. 29, 2022
-- Description: Gets course enrollment records matching specified parameters
-- =============================================
CREATE PROCEDURE ClassEnrollment_GetByUserId
    @UserId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    -- Get all enrollment records associated with user
    SELECT ce.[Id],
        [UserId],
        [ClassId],
        [PaymentId],
        [EnrolledDate],
        [PassedYN],
        cm.[ClassName],
        cm.[ShortDescription],
        [dbo].[UserIsMember]([UserId], su.[MemberThru]) AS [IsMember]
    FROM [ClassEnrollment] ce
    LEFT JOIN [ClassMain] cm on cm.[Id] = [ClassId]
    LEFT JOIN [SiteUser] su on su.[Id] = [UserId]
    WHERE
        [UserId] = @UserId;
END
