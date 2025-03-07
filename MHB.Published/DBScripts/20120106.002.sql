USE Test01Db
GO
/****** Object:  StoredProcedure [dbo].[spGetBudgetsSavingsSumExpenses]    Script Date: 01/06/2012 16:32:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-01-05
-- Description:	Checks if record for that year exists in the tbMonthlyBudget, 
--				tbMonthlyExpenses, tbMonthlySavings and if not present inserts a blank row for that year
-- =============================================
CREATE PROCEDURE [dbo].[spCheckAndInsertBudgetsSavingsExpensesPlaceHolderRows]
	
	@year INT,
	@userID INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @BudgetExists INT
	DECLARE @ExpensesExists INT
	DECLARE @SavingsExists INT

	SET @BudgetExists = (SELECT TOP 1 UserID FROM dbo.tbMonthlyBudget WHERE [Year] = @year AND UserID = @userID)

	IF @BudgetExists IS NULL
	BEGIN
		INSERT INTO dbo.tbMonthlyBudget (UserID, [Year]) VALUES (@userID, @year)
	END


	SET @ExpensesExists = (SELECT TOP 1 UserID FROM dbo.tbMonthlyExpenses WHERE [Year] = @year AND UserID = @userID)

	IF @ExpensesExists IS NULL
	BEGIN
		INSERT INTO dbo.tbMonthlyExpenses (UserID, [Year]) VALUES (@userID, @year)
	END


	SET @SavingsExists = (SELECT TOP 1 UserID FROM dbo.tbMonthlySavings WHERE [Year] = @year AND UserID = @userID)

	IF @SavingsExists IS NULL
	BEGIN
		INSERT INTO dbo.tbMonthlySavings (UserID, [Year]) VALUES (@userID, @year)
	END
    
END


