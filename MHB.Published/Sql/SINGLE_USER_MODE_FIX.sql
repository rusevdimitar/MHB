SELECT request_session_id FROM sys.dm_tran_locks 
WHERE resource_database_id = DB_ID('Test01Db')

KILL 51

-- Start in master
USE MASTER;

-- Add users
ALTER DATABASE Test01Db SET MULTI_USER WITH ROLLBACK IMMEDIATE
GO
