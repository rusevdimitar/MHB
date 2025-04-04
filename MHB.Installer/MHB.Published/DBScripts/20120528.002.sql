USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetBudgetsSavingsSumExpenses]    Script Date: 05/28/2012 09:33:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2010-12-10
-- Modified on: 2012-05-28
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
	DECLARE @savings MONEY
	DECLARE @sumExpenses MONEY
	DECLARE @sumSpentToday MONEY
	DECLARE @sumExpectedExpenses MONEY
	DECLARE @sumFlaggedExpenses MONEY
	DECLARE @sumYearlySavings MONEY
	
	
	DECLARE @result TABLE (
		budget MONEY,
		savings MONEY,
		sumExpenses MONEY,
		sumSpentToday MONEY,
		sumExpectedExpenses MONEY,
		sumFlaggedExpenses MONEY,
		sumYearlySavings MONEY		    
    );
	
	
	IF @month = 1
	BEGIN
		SET @savings =  (SELECT ISNULL(SavingsJan, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetJan, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)			
		GOTO calculateRestThings 
	END
	ELSE IF @month = 2
	BEGIN
		SET @savings =  (SELECT ISNULL(SavingsFeb, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetFeb, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 3
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsMar, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetMar, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 4
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsApr, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetApr, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 5
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsMay, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetMay, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 6
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsJune, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetJune, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 7
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsJuly, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetJuly, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 8
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsAug, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetAug, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 9
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsSept, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetSept, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 10
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsOct, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetOct, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 11
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsNov, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetNov, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
		GOTO calculateRestThings
	END
	ELSE IF @month = 12
	BEGIN 
		SET @savings =  (SELECT ISNULL(SavingsDec, 0) AS Savings FROM tbMonthlySavings WHERE UserID = @userID AND Year = @year)
		SET @budget = (SELECT ISNULL(BudgetDec, 0) FROM tbMonthlyBudget WHERE UserID = @userID AND Year = @year)	
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
SET @sumSpentToday = (SELECT SUM(NewValue - OldValue) FROM tbTransactionLog WHERE UserID = @userID AND CONVERT(VARCHAR, DateModified, 101) = CONVERT(VARCHAR, GETDATE(), 101))
	
INSERT INTO @result (budget, savings, sumExpenses, sumSpentToday, sumExpectedExpenses, sumFlaggedExpenses, sumYearlySavings) VALUES
(@budget, @savings, @sumExpenses, @sumSpentToday, @sumExpectedExpenses, @sumFlaggedExpenses, @sumYearlySavings)
    
SELECT * FROM @result
    
END


