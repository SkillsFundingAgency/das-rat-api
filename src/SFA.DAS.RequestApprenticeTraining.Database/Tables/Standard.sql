CREATE TABLE [dbo].[Standard]
(
    [StandardReference] VARCHAR(6) NOT NULL PRIMARY KEY, 
    [StandardTitle] NVARCHAR(100) NOT NULL,
    [StandardLevel] INT NOT NULL,
    [StandardSector] NVARCHAR(100) NOT NULL
)
GO
