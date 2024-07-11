CREATE TABLE [dbo].[EmployerRequestRegionHistory]
(
    [EmployerRequestId] UNIQUEIDENTIFIER NOT NULL,
    [RegionId] INT NOT NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL,
)
GO