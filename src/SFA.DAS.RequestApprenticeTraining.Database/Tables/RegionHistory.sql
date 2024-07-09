CREATE TABLE [dbo].[RegionHistory](
    [Id] INT NOT NULL,
    [SubregionName] VARCHAR(250) NULL,
    [RegionName] VARCHAR(25) NULL,
    [Latitude] FLOAT NOT NULL,
    [Longitude] FLOAT NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL,
)
GO
