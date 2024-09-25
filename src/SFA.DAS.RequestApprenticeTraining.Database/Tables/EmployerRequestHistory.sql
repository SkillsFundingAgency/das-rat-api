CREATE TABLE [dbo].[EmployerRequestHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
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
    [RequestedAt] DATETIME2 NOT NULL CONSTRAINT [DF_EmployerRequestHistory_RequestedAt] DEFAULT GETUTCDATE(),
    [RequestedBy] UNIQUEIDENTIFIER NOT NULL,
    [RequestStatusId] INT NOT NULL,
    [ExpiredAt] DATETIME2 NULL,
    [CancelledAt] DATETIME2 NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)
