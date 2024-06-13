CREATE PROCEDURE [dbo].[CreateEmployerRequestRegion]
    @EmployerRequestId UNIQUEIDENTIFIER,
    @Latitude FLOAT,
    @Longitude FLOAT,
    @ModifiedBy NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RegionId INT;
    DECLARE @SubregionName VARCHAR(250);
    DECLARE @RegionName VARCHAR(25);
    
    EXEC [dbo].[FindClosestRegion] 
        @Latitude,
        @Longitude,
        @RegionId OUTPUT,
        @SubregionName OUTPUT,
        @RegionName OUTPUT;

    INSERT INTO [dbo].[EmployerRequestRegion] 
        ([EmployerRequestId], [RegionId], [ModifiedBy])
    VALUES 
        (@EmployerRequestId, @RegionId, @ModifiedBy);
END
GO
