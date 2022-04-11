CREATE FUNCTION [dbo].[CheckClassActive] (
    @ClassId INT)
RETURNS INT
AS
BEGIN
    -- Issue error if trying to access disabled class
    DECLARE @ClassEnabled INT = (
        SELECT TOP 1 [Enabled] FROM [ClassMain]
        WHERE [Id] = @ClassId);

    RETURN ISNULL(@ClassEnabled, 0);
END