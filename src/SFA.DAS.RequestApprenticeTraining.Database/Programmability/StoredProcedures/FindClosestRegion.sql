CREATE PROCEDURE [dbo].[FindClosestRegion]
    @Latitude FLOAT,
    @Longitude FLOAT,
    @RegionId INT OUTPUT,
    @SubregionName VARCHAR(250) OUTPUT,
    @RegionName VARCHAR(25) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 
        @RegionId = Id,
        @SubregionName = SubregionName,
        @RegionName = RegionName
    FROM 
        [Region]
    ORDER BY 
        geography::Point(Latitude, Longitude, 4326)
        .STDistance(geography::Point(@Latitude, @Longitude, 4326));
END
GO
