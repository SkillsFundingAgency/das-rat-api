/*
	Insert or Update each of the [Status] look up default values.

	NOTES:

	1) This script uses a temporary table, insert or update the values in the temporary table to apply changes; removed values will
	not take affect (by design) as they may still be referenced by rows in the database.
*/
BEGIN TRANSACTION

CREATE TABLE #Status(
	[Id] [int] NOT NULL,
	[Description] [nvarchar](25) NOT NULL
) 

INSERT #Status VALUES (0, N'Active')

MERGE [Status] [Target] USING #Status [Source]
ON ([Source].Id = [Target].Id)
WHEN MATCHED
    THEN UPDATE SET 
        [Target].[Description] = [Source].[Description]

WHEN NOT MATCHED BY TARGET 
    THEN INSERT ([Id], [Description])
         VALUES ([Source].[Id], [Source].[Description]);

COMMIT TRANSACTION
