
DECLARE @UserID INT	
	
DECLARE MyCursor CURSOR FOR SELECT userID FROM dbo.tbUsers
OPEN MyCursor

FETCH NEXT FROM MyCursor INTO @UserID

	WHILE @@fetch_status = 0 
	BEGIN
		INSERT INTO dbo.tbMonthlyBudget (UserID, Year) VALUES (@UserID, 2010)
		INSERT INTO dbo.tbMonthlyExpenses (UserID, Year) VALUES (@UserID, 2010)
		INSERT INTO dbo.tbMonthlySavings (UserID, Year) VALUES (@UserID, 2010)
		
		INSERT INTO dbo.tbMonthlyBudget (UserID, Year) VALUES (@UserID, 2011)
		INSERT INTO dbo.tbMonthlyExpenses (UserID, Year) VALUES (@UserID, 2011)
		INSERT INTO dbo.tbMonthlySavings (UserID, Year) VALUES (@UserID, 2011)
		
		INSERT INTO dbo.tbMonthlyBudget (UserID, Year) VALUES (@UserID, 2012)
		INSERT INTO dbo.tbMonthlyExpenses (UserID, Year) VALUES (@UserID, 2012)
		INSERT INTO dbo.tbMonthlySavings (UserID, Year) VALUES (@UserID, 2012)
	
	FETCH NEXT FROM MyCursor INTO @UserID
	END

CLOSE MyCursor
DEALLOCATE MyCursor
