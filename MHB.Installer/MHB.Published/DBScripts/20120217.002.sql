USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetSumsForCategoriesPerYear]    Script Date: 02/17/2012 15:43:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Dimitar Rusev
-- Create date:		17.02.2012
-- Modified date:	17.02.2012
-- Description:		Returns the sums spent for each category for a given year
-- =============================================
CREATE PROCEDURE [dbo].[spGetSumsForCategoryPerYear]
	@UserID INT,	
	@StartYear INT,
	@Lang INT,
	@CategoryID INT
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @sum MONEY	
	DECLARE @costCategoryName NVARCHAR(50)	
	
	DECLARE @sums TABLE  
	(
		CostSum MONEY,
		CostCategory INT,
		CostCategoryName NVARCHAR(50),
		[Year] INT 
	)

	-- Get category name translation
	IF @Lang = 0
	BEGIN
		SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = CAST(@CategoryID AS VARCHAR))
	END
	ELSE IF @Lang = 1
	BEGIN
		SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = CAST(@CategoryID AS VARCHAR))
	END
	ELSE IF @Lang = 2
	BEGIN
		SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = CAST(@CategoryID AS VARCHAR))
	END
	
	
	WHILE @StartYear < YEAR(GETDATE())
	BEGIN
	
		SET @sum = 
		(
			SELECT ISNULL(SUM(FieldValue), 0)
			FROM [tbMainTable01]
			WHERE 
			[UserID] = @UserID 
			AND [Year] = @StartYear
			AND [CostCategory] = @CategoryID
		)
															
		INSERT INTO @sums (CostSum, CostCategory, CostCategoryName, [Year]) VALUES (@sum, @CategoryID, @costCategoryName, @StartYear)
		
		SET @StartYear = @StartYear + 1
	END

	SELECT * FROM @sums 
   
END

