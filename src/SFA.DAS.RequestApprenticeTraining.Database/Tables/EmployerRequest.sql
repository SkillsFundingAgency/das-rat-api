CREATE TABLE [dbo].[EmployerRequest]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [OriginalLocation] NVARCHAR(100) NULL,
    [RequestTypeId] INT NOT NULL,
    [AccountId] BIGINT NOT NULL,
    [StandardReference] VARCHAR(6) NOT NULL,
    [NumberOfApprentices] INT NOT NULL,
    [SameLocation] NVARCHAR(5) NULL,
    [SingleLocation] NVARCHAR(100) NULL,
    [AtApprenticesWorkplace] BIT NOT NULL,
    [DayRelease] BIT NOT NULL,
    [BlockRelease] BIT NOT NULL,
    [RequestedAt] DATETIME2 NOT NULL CONSTRAINT [DF_EmployerRequest_RequestedAt] DEFAULT GETUTCDATE(),
    [RequestedBy] UNIQUEIDENTIFIER NOT NULL,
    [RequestStatusId] INT NOT NULL,
    [ExpiredAt] DATETIME2 NULL,
    [CancelledAt] DATETIME2 NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT [FK_EmployerRequest_RequestType] FOREIGN KEY ([RequestTypeId]) REFERENCES [RequestType]([Id]),
    CONSTRAINT [FK_EmployerRequest_RequestStatus] FOREIGN KEY ([RequestStatusId]) REFERENCES [RequestStatus]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[EmployerRequestHistory]));
GO