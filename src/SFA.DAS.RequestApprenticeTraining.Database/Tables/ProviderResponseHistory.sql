﻿CREATE TABLE [dbo].[ProviderResponseHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [ContactName] NVARCHAR(256) NOT NULL CONSTRAINT [DF_ProviderResponseHistory_ContactName] DEFAULT '',
    [Email] VARCHAR(256) NOT NULL,
    [PhoneNumber] VARCHAR(25) NOT NULL,
    [Website] VARCHAR(256) NOT NULL,
    [RespondedAt] DATETIME2 NOT NULL,
    [RespondedBy] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_ProviderResponseHistory_RespondedBy] DEFAULT '00000000-0000-0000-0000-000000000000',
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)
