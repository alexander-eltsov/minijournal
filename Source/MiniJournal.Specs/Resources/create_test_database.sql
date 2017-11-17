USE master;
GO
IF DB_ID (N'MiniJournalDB_Test') IS NOT NULL
BEGIN
	-- Drop DB
	ALTER DATABASE MiniJournalDB_Test SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE MiniJournalDB_Test;
END
GO
-- Create DB
CREATE DATABASE MiniJournalDB_Test;
GO
-- Verify the database files and sizes
SELECT name, size, size*1.0/32 AS [Size in MBs]
FROM sys.master_files
WHERE name = N'MiniJournalDB_Test';
GO
