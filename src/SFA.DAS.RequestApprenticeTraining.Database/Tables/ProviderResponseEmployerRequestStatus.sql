CREATE TABLE [dbo].[ProviderResponseEmployerRequestStatus]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [Ukprn] BIGINT NOT NULL,
    [EmployerRequestId] UNIQUEIDENTIFIER NULL,
    [ResponseStatus] INT NOT NULL,
    CONSTRAINT PK_ProviderResponseEmployerRequestStatus PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProviderResponseEmployerRequestStatus_EmployerRequest] FOREIGN KEY ([EmployerRequestId]) REFERENCES [EmployerRequest]([Id])

)
