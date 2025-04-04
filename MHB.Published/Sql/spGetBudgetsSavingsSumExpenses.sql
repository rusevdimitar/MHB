USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetBudgetsSavingsSumExpenses]    Script Date: 5/5/2016 11:31:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2010-12-10
-- Modified on: 2016-05-05
-- Description:	Returns all the budgets, savings, sum of expenses for the month
--				it replaces the multiple queries in the code
-- =============================================
ALTER PROCEDURE [dbo].[spGetBudgetsSavingsSumExpenses]
	
	@year INT,
	@month INT,
	@userID INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @budget MONEY
	DECLARE @budgetTotal MONEY
	DECLARE @savings MONEY
	DECLARE @sumExpenses MONEY
	DECLARE @sumSpentToday MONEY
	DECLARE @sumExpectedExpenses MONEY
	DECLARE @sumFlaggedExpenses MONEY
	DECLARE @sumYearlySavings MONEY
	
	
	DECLARE @result TABLE (
		budget MONEY,
		budgetTotal MONEY,
		savings MONEY,
		sumExpenses MONEY,
		sumSpentToday MONEY,
		sumExpectedExpenses MONEY,
		sumFlaggedExpenses MONEY,
		sumYearlySavings MONEY		    
    );
	
	--SET @budgetTotal = (SELECT ISNULL(BudgetJan, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)			
	SET @budgetTotal = (SELECT SUM(IncomeValue) FROM tbIncomes WHERE UserID = @userID AND [Month] = @month AND [Year] = @year AND IsDeleted = 0)
	SET @budget = (SELECT SUM(IncomeValue) FROM tbIncomes WHERE UserID = @userID AND [Month] = @month AND [Year] = @year AND IsDeleted = 0 AND (IncomeDate <= GETDATE() OR IncomeDate IS NULL))
	
	IF @month = 1
	BEGIN
		SET @savings =  (SELECT ISNULL(SavingsJan, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)		
		GOTO calculateRestThings 
	END
	ELSE IF @month = 2
	BEGIN
		SET @savings =  (SELECT ISNULL(SavingsFeb, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)		
		GOTO calculateRestThings
	END
	ELSE IF @month = 3
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsMar, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 4
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsApr, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 5
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsMay, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)		
		GOTO calculateRestThings
	END
	ELSE IF @month = 6
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsJune, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		GOTO calculateRestThings
	END
	ELSE IF @month = 7
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsJuly, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)		
		GOTO calculateRestThings
	END
	ELSE IF @month = 8
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsAug, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 9
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsSept, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)		
		GOTO calculateRestThings
	END
	ELSE IF @month = 10
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsOct, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)		
		GOTO calculateRestThings
	END
	ELSE IF @month = 11
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsNov, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 12
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsDec, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)		
		GOTO calculateRestThings
	END

calculateRestThings:
	
	SET @sumExpenses = 
(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable01 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)
+(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable02 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)
+(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable03 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)
+(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable04 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)
+(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable05 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)

	SET @sumExpectedExpenses = 
(SELECT ISNULL(SUM(FieldExpectedValue), 0) AS FieldExpectedValue FROM tbMainTable01 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)	
+(SELECT ISNULL(SUM(FieldExpectedValue), 0) AS FieldExpectedValue FROM tbMainTable02 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)	
+(SELECT ISNULL(SUM(FieldExpectedValue), 0) AS FieldExpectedValue FROM tbMainTable03 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)	
+(SELECT ISNULL(SUM(FieldExpectedValue), 0) AS FieldExpectedValue FROM tbMainTable04 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)	
+(SELECT ISNULL(SUM(FieldExpectedValue), 0) AS FieldExpectedValue FROM tbMainTable05 WHERE UserID = @userID AND Month = @month AND Year = @year AND IsDeleted = 0)	

	SET @sumFlaggedExpenses = 
(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable01 WHERE UserID = @userID AND IsDeleted = 0 AND Flagged = 1)
+(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable02 WHERE UserID = @userID AND IsDeleted = 0 AND Flagged = 1)
+(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable03 WHERE UserID = @userID AND IsDeleted = 0 AND Flagged = 1)
+(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable04 WHERE UserID = @userID AND IsDeleted = 0 AND Flagged = 1)
+(SELECT ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable05 WHERE UserID = @userID AND IsDeleted = 0 AND Flagged = 1)
	
	SET @sumYearlySavings =  (SELECT 
								ISNULL(SavingsJan,0) + 
								  ISNULL(SavingsFeb,0) +
								  ISNULL(SavingsMar,0) +
								  ISNULL(SavingsApr,0) +
								  ISNULL(SavingsMay,0) +
								  ISNULL(SavingsJune,0) +
								  ISNULL(SavingsJuly,0) +
								  ISNULL(SavingsAug,0) +
								  ISNULL(SavingsSept,0) +
								  ISNULL(SavingsOct,0) +
								  ISNULL(SavingsNov,0) +
								  ISNULL(SavingsDec,0)
							  FROM dbo.tbMonthlySavings WHERE UserID = @userID AND Year = @year)
							  
							  
SET @sumSpentToday = (SELECT SUM(tl.NewValue - tl.OldValue) FROM tbTransactionLog tl
LEFT JOIN tbMainTable01 mt1 ON tl.ExpenseID = mt1.ID
LEFT JOIN tbMainTable02 mt2 ON tl.ExpenseID = mt2.ID
LEFT JOIN tbMainTable03 mt3 ON tl.ExpenseID = mt3.ID
LEFT JOIN tbMainTable04 mt4 ON tl.ExpenseID = mt4.ID
LEFT JOIN tbMainTable05 mt5 ON tl.ExpenseID = mt5.ID
WHERE tl.UserID = @userID AND CONVERT(VARCHAR, tl.DateModified, 101) = CONVERT(VARCHAR, GETDATE(), 101)
AND 
(mt1.IsDeleted = 0
OR mt2.IsDeleted = 0
OR mt3.IsDeleted = 0
OR mt4.IsDeleted = 0
OR mt5.IsDeleted = 0)
)
	
INSERT INTO @result (budget, budgetTotal, savings, sumExpenses, sumSpentToday, sumExpectedExpenses, sumFlaggedExpenses, sumYearlySavings) VALUES
(@budget, @budgetTotal, @savings, @sumExpenses, ISNULL(@sumSpentToday, 0), @sumExpectedExpenses, @sumFlaggedExpenses, @sumYearlySavings)
    
SELECT * FROM @result
    
END


