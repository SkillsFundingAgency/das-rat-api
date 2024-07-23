﻿CREATE TABLE [dbo].[ProviderResponseEmployerRequestHistory]
(
    [EmployerRequestId] UNIQUEIDENTIFIER NOT NULL,
    [Ukprn] BIGINT NOT NULL,
    [ProviderResponseId] UNIQUEIDENTIFIER NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL,
)
GO