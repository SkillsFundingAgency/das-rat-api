CREATE TABLE [dbo].[ProviderResponseHistory]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Email] VARCHAR(256) NOT NULL,
	[PhoneNumber] VARCHAR(25) NOT NULL,
	[Website] VARCHAR(256) NOT NULL,
	[RespondedAt] DATETIME2 NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)
