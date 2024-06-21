CREATE TABLE [dbo].[Region](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [SubregionName] VARCHAR(250) NULL,
    [RegionName] VARCHAR(25) NULL,
    [Latitude] FLOAT NOT NULL,
    [Longitude] FLOAT NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT [UQ_Subregion_Region] UNIQUE ([SubregionName], [RegionName])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[RegionHistory]))
GO
