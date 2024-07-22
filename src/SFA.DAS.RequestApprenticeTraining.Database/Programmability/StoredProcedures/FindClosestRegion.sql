CREATE PROCEDURE [dbo].[FindClosestRegion]
    @Latitude FLOAT,
    @Longitude FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 
        Id,
        SubregionName,
        RegionName,
        Latitude,
        Longitude
    FROM 
        [Region]
    ORDER BY 
        geography::Point(Latitude, Longitude, 4326)
        .STDistance(geography::Point(@Latitude, @Longitude, 4326));
END
GO
