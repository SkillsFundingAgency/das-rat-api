CREATE TABLE [dbo].[Standard]
(
	[StandardReference] VARCHAR(25) NOT NULL PRIMARY KEY, 
    [StandardTitle] VARCHAR(100) NOT NULL,
    [StandardLevel] INT NOT NULL,
    [StandardSector] VARCHAR(100) NOT NULL
)
GO
