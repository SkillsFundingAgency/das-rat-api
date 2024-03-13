﻿CREATE TABLE [dbo].[EmployerRequest]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [RequestTypeId] INT NOT NULL, 
    [AccountId] BIGINT NOT NULL , 
    CONSTRAINT [FK_EmployerRequest_RequestType] FOREIGN KEY ([RequestTypeId]) REFERENCES [RequestType]([Id])
)
GO