DECLARE @s as int
DECLARE @index as int 

SET @index = 0

DECLARE MyCursor CURSOR FOR
 select ID from dbo.tbMainTable01

OPEN MyCursor

FETCH NEXT FROM MyCursor INTO @s

WHILE @@fetch_status = 0 BEGIN

	update dbo.tbMainTable01 set OrderID = @index
	WHERE ID = @s
	
	set @index = @index + 1	

	FETCH NEXT FROM MyCursor INTO @s
END

CLOSE MyCursor
DEALLOCATE MyCursor