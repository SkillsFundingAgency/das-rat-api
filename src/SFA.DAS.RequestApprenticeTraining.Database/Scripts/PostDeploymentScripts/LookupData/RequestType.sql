/*
	Insert or Update each of the [RequestType] look up default values.

	NOTES:

	1) This script uses a temporary table, insert or update the values in the temporary table to apply changes; removed values will
	not take affect (by design) as they may still be referenced by rows in the database.
*/
BEGIN TRANSACTION

CREATE TABLE #RequestType(
	[Id] [int] NOT NULL,
	[Description] [nvarchar](25) NOT NULL
) 

INSERT #RequestType VALUES (1, N'Shortlist')

MERGE [RequestType] [Target] USING #RequestType [Source]
ON ([Source].Id = [Target].Id)
WHEN MATCHED
    THEN UPDATE SET 
        [Target].[Description] = [Source].[Description]

WHEN NOT MATCHED BY TARGET 
    THEN INSERT ([Id], [Description])
         VALUES ([Source].[Id], [Source].[Description]);

COMMIT TRANSACTION
