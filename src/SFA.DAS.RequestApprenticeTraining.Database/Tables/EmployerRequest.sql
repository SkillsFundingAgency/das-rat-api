CREATE TABLE [dbo].[EmployerRequest]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [OriginalLocation] NVARCHAR(100) NULL,
    [RequestTypeId] INT NOT NULL,
    [AccountId] BIGINT NOT NULL,
    [StandardReference] VARCHAR(6) NOT NULL,
    [NumberOfApprentices] INT NOT NULL,
    [SingleLocation] NVARCHAR(100) NULL,
    [AtApprenticesWorkplace] BIT NOT NULL,
    [DayRelease] BIT NOT NULL,
    [BlockRelease] BIT NOT NULL,
    [RequestedBy] UNIQUEIDENTIFIER NOT NULL,
    [StatusId] INT NOT NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT [FK_EmployerRequest_RequestType] FOREIGN KEY ([RequestTypeId]) REFERENCES [RequestType]([Id]),
    CONSTRAINT [FK_EmployerRequest_Status] FOREIGN KEY ([StatusId]) REFERENCES [Status]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[EmployerRequestHistory]));
GO