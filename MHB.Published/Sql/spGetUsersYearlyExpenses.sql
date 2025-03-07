USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetUsersYearlyExpenses]    Script Date: 5/5/2016 2:24:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2016-05-05
-- Description:	Returns a pivot table with sum of expenses for each month, months being column names;
-- =============================================
ALTER PROCEDURE [dbo].[spGetUsersYearlyExpenses]
	-- Add the parameters for the stored procedure here
	@UserID INT,
	@Year INT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM
	(
		SELECT 
			CASE 
			WHEN [Month] = 1 THEN 'SumJanuary' WHEN [Month] = 2 THEN 'SumFebruary' WHEN [Month] = 3 THEN 'SumMarch' WHEN [Month] = 4 THEN 'SumApril' WHEN [Month] = 5 THEN 'SumMay' WHEN [Month] = 6 THEN 'SumJune' WHEN [Month] = 7 THEN 'SumJuly' WHEN [Month] = 8 THEN 'SumAugust' WHEN [Month] = 9 THEN 'SumSeptember' WHEN [Month] = 10 THEN 'SumOctober' WHEN [Month] = 11 THEN 'SumNovember' WHEN [Month] = 12 THEN 'SumDecember'
			END AS [MonthName],
		ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable01 WHERE UserID = @UserID AND IsDeleted = 0 AND [Month] IS NOT NULL AND [Year] = @Year
		GROUP BY [Month], [Year]

		UNION
		SELECT CASE WHEN [Month] = 1 THEN 'SumJanuary' WHEN [Month] = 2 THEN 'SumFebruary' WHEN [Month] = 3 THEN 'SumMarch' WHEN [Month] = 4 THEN 'SumApril' WHEN [Month] = 5 THEN 'SumMay' WHEN [Month] = 6 THEN 'SumJune' WHEN [Month] = 7 THEN 'SumJuly' WHEN [Month] = 8 THEN 'SumAugust' WHEN [Month] = 9 THEN 'SumSeptember' WHEN [Month] = 10 THEN 'SumOctober' WHEN [Month] = 11 THEN 'SumNovember' WHEN [Month] = 12 THEN 'SumDecember' END AS [MonthName], ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable02 WHERE UserID = @UserID AND IsDeleted = 0 AND [Month] IS NOT NULL AND [Year] = @Year GROUP BY [Month], [Year]
		UNION
		SELECT CASE WHEN [Month] = 1 THEN 'SumJanuary' WHEN [Month] = 2 THEN 'SumFebruary' WHEN [Month] = 3 THEN 'SumMarch' WHEN [Month] = 4 THEN 'SumApril' WHEN [Month] = 5 THEN 'SumMay' WHEN [Month] = 6 THEN 'SumJune' WHEN [Month] = 7 THEN 'SumJuly' WHEN [Month] = 8 THEN 'SumAugust' WHEN [Month] = 9 THEN 'SumSeptember' WHEN [Month] = 10 THEN 'SumOctober' WHEN [Month] = 11 THEN 'SumNovember' WHEN [Month] = 12 THEN 'SumDecember' END AS [MonthName], ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable03 WHERE UserID = @UserID AND IsDeleted = 0 AND [Month] IS NOT NULL AND [Year] = @Year GROUP BY [Month], [Year]
		UNION
		SELECT CASE WHEN [Month] = 1 THEN 'SumJanuary' WHEN [Month] = 2 THEN 'SumFebruary' WHEN [Month] = 3 THEN 'SumMarch' WHEN [Month] = 4 THEN 'SumApril' WHEN [Month] = 5 THEN 'SumMay' WHEN [Month] = 6 THEN 'SumJune' WHEN [Month] = 7 THEN 'SumJuly' WHEN [Month] = 8 THEN 'SumAugust' WHEN [Month] = 9 THEN 'SumSeptember' WHEN [Month] = 10 THEN 'SumOctober' WHEN [Month] = 11 THEN 'SumNovember' WHEN [Month] = 12 THEN 'SumDecember' END AS [MonthName], ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable04 WHERE UserID = @UserID AND IsDeleted = 0 AND [Month] IS NOT NULL AND [Year] = @Year GROUP BY [Month], [Year]
		UNION
		SELECT CASE WHEN [Month] = 1 THEN 'SumJanuary' WHEN [Month] = 2 THEN 'SumFebruary' WHEN [Month] = 3 THEN 'SumMarch' WHEN [Month] = 4 THEN 'SumApril' WHEN [Month] = 5 THEN 'SumMay' WHEN [Month] = 6 THEN 'SumJune' WHEN [Month] = 7 THEN 'SumJuly' WHEN [Month] = 8 THEN 'SumAugust' WHEN [Month] = 9 THEN 'SumSeptember' WHEN [Month] = 10 THEN 'SumOctober' WHEN [Month] = 11 THEN 'SumNovember' WHEN [Month] = 12 THEN 'SumDecember' END AS [MonthName], ISNULL(SUM(FieldValue), 0) AS FieldValue FROM tbMainTable05 WHERE UserID = @UserID AND IsDeleted = 0 AND [Month] IS NOT NULL AND [Year] = @Year GROUP BY [Month], [Year]
	) src
	PIVOT
	(
	 max(FieldValue)
	 for [MonthName] in ([SumJanuary], [SumFebruary], [SumMarch], [SumApril], [SumMay], [SumJune], [SumJuly], [SumAugust], [SumSeptember], [SumOctober], [SumNovember], [SumDecember])
	) p
END
