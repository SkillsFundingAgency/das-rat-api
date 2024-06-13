CREATE TABLE [dbo].[EmployerRequestRegion]
(
    [EmployerRequestId] UNIQUEIDENTIFIER NOT NULL,
    [RegionId] INT NOT NULL,
    [ModifiedBy] NVARCHAR(100) NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT [PK_EmployerRequestRegion] PRIMARY KEY ([EmployerRequestId], [RegionId]),
    CONSTRAINT [FK_EmployerRequestRegion_EmployerRequest] FOREIGN KEY ([EmployerRequestId]) REFERENCES [EmployerRequest]([Id]),
    CONSTRAINT [FK_EmployerRequestRegion_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[EmployerRequestRegionHistory]))
GO