SET NOCOUNT ON

-- Get all stored procedures' names
DECLARE getStoredProcedursCursor CURSOR FOR
	SELECT SPECIFIC_NAME FROM Test01Db.information_schema.routines
	WHERE ROUTINE_TYPE = 'PROCEDURE' AND LEFT(ROUTINE_NAME, 3) NOT IN ('sp_', 'xp_', 'ms_')
	ORDER BY SPECIFIC_NAME

DECLARE @storedProcedureName VARCHAR(100)
DECLARE @result TABLE (Line NVARCHAR(512))
DECLARE @cmd NVARCHAR(1000)

-- Loop through all stored procedures' names:
OPEN getStoredProcedursCursor;
	FETCH NEXT FROM getStoredProcedursCursor INTO @storedProcedureName;
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
       				
		SET @cmd = 'findstr /spin /c:"' + CAST(@storedProcedureName AS VARCHAR(100)) + '" C:\PROJECTS\Test02\*.cs C:\PROJECTS\Test02\*.vb'

		INSERT INTO @result
			EXECUTE master..xp_CMDShell @cmd

		DECLARE @entriesFoundCount INT = (SELECT COUNT(*) FROM @result WHERE Line <> 'NULL')

		IF @entriesFoundCount = 0
		BEGIN

			 DECLARE @dropProcedureCmd VARCHAR(512) = 'DROP PROCEDURE ' + @storedProcedureName
			 EXEC(@dropProcedureCmd)
			--print (CAST(@storedProcedureName AS VARCHAR(100)) + ' exists in ' + CAST(@entriesFoundCount AS VARCHAR(MAX)) + ' lines of code')

		END

		DELETE FROM @result


		FETCH NEXT FROM getStoredProcedursCursor INTO @storedProcedureName;
	END;
CLOSE getStoredProcedursCursor;
DEALLOCATE getStoredProcedursCursor;