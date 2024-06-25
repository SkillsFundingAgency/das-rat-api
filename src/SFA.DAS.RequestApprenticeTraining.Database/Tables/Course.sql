CREATE TABLE [dbo].[Course]
(
	[Reference] VARCHAR(25) NOT NULL PRIMARY KEY, 
    [Title] VARCHAR(100) NOT NULL,
    [Level] INT NOT NULL,
    [Sector] VARCHAR(100) NOT NULL
)
GO
