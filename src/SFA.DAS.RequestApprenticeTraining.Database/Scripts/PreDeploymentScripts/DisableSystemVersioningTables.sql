/* 
   All system versioning tables must be explictly created and modified as the DACPAC does not correctly automatically sychronize added columns.
   
   The system versioning is turned off then the DACPAC is allowed to add columns and then the system versioning is turned back on.

   Ensure that any added columns have an explicitly named constraint and that the same constraint is added to both the main table and the system versioning table.
*/
ALTER TABLE [dbo].[EmployerRequest]
SET (
	SYSTEM_VERSIONING = OFF
);

ALTER TABLE [dbo].[EmployerRequestRegion]
SET (
	SYSTEM_VERSIONING = OFF
);

ALTER TABLE [dbo].[Region]
SET (
	SYSTEM_VERSIONING = OFF
);
