CREATE TABLE [dbo].[ProviderResponseEmployerRequest]
(
    [EmployerRequestId] UNIQUEIDENTIFIER NOT NULL,
    [Ukprn] BIGINT NOT NULL,
    [ProviderResponseId] UNIQUEIDENTIFIER NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT PK_ProviderResponseEmployerRequest PRIMARY KEY ([EmployerRequestId], [Ukprn]),
    CONSTRAINT [FK_ProviderResponseEmployerRequest_EmployerRequest] FOREIGN KEY ([EmployerRequestId]) REFERENCES [EmployerRequest]([Id]),
    CONSTRAINT [FK_ProviderResponseEmployerRequest_ProviderResponse] FOREIGN KEY ([ProviderResponseId]) REFERENCES [ProviderResponse]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ProviderResponseEmployerRequestHistory]))
GO