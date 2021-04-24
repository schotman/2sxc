/* Oqtane db fix for https://github.com/oqtane/oqtane.framework/issues/1269 */
BEGIN TRANSACTION
GO
IF COLUMNPROPERTY(OBJECT_ID('Folder', 'U'), 'Path', 'charmaxlen')=50
ALTER TABLE dbo.Folder ALTER COLUMN [Path] nvarchar(1000) NOT NULL
GO
IF COLUMNPROPERTY(OBJECT_ID('Folder', 'U'), 'Name', 'charmaxlen')=50
ALTER TABLE dbo.Folder ALTER COLUMN [Name] nvarchar(250) NOT NULL
GO
COMMIT
