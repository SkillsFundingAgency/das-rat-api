CREATE TABLE [dbo].[ProviderResponseHistory]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Email] VARCHAR(256) NOT NULL,
	[PhoneNumber] VARCHAR(25) NOT NULL,
	[Website] VARCHAR(256) NOT NULL,
	[AcknowledgedAt] DATETIME2 NULL,
	[AcknowledgedBy] UNIQUEIDENTIFIER NULL,
	[RespondedAt] DATETIME2 NOT NULL,
	[RespondedBy] VARCHAR(256) NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)
