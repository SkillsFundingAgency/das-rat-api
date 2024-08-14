﻿CREATE TABLE [dbo].[ProviderResponse]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Email] VARCHAR(256) NOT NULL,
	[PhoneNumber] VARCHAR(25) NOT NULL,
	[Website] VARCHAR(256) NOT NULL,
	[RespondedAt] DATETIME2 NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ProviderResponseHistory]));
GO
