
/****** Object:  StoredProcedure [dbo].[spGetSumsForCategoriesPerMonth]    Script Date: 05/17/2012 11:53:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Dimitar Rusev
-- Create date:		11.12.2009
-- Modified date:	17.05.2012
-- Description:		Returns the sums spent for each category of a given month
-- =============================================
ALTER PROCEDURE [dbo].[spGetSumsForCategoriesPerMonth]
	@UserID INT,
	@Month INT,
	@Year INT,
	@Lang INT
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @costCategory INT
	DECLARE @sum MONEY
	DECLARE @sumOther MONEY
	DECLARE @costCategoryName NVARCHAR(50)
	DECLARE @categoriesCount INT

	SET @categoriesCount = (SELECT COUNT(ID) FROM tbCategories)
		
	DECLARE @sums TABLE  (
	costSum MONEY,
	costCategory INT,
	costCategoryName NVARCHAR(50) )
	
	SET @sumOther = 0
	
	IF @userID < 1000
	BEGIN 
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	SET @sumOther = (SELECT SUM(FieldValue) FROM tbMainTable01
						WHERE [UserID] = @UserID 
						AND [Month] = @Month
						AND [Year] = @Year
						AND (CostCategory IS NULL OR CostCategory = 0 AND [IsDeleted] = 0)
	)
	
	IF @Lang = 0
	BEGIN
		SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 1
	BEGIN
		SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 2
	BEGIN
		SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	
	
	INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sumOther, 0, @costCategoryName)

	DECLARE MyCursor CURSOR FOR SELECT ID FROM tbCategories WHERE UserCategoryID = @UserID OR UserCategoryID = 0

	OPEN MyCursor

	FETCH NEXT FROM MyCursor INTO @costCategory

	WHILE @@fetch_status = 0 
	BEGIN		
		
		SET @sum = 
		(SELECT SUM(FieldValue)
		FROM [tbMainTable01]
		WHERE 
		[UserID] = @UserID 
		AND [Month] = @Month
		AND [Year] = @Year
		AND [CostCategory] = @costCategory AND [IsDeleted] = 0)
				
		IF @Lang = 0
		BEGIN
			SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 1
		BEGIN
			SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 2
		BEGIN
			SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
				
		IF @sum IS NOT NULL AND @sum <> 0
		BEGIN								
			INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sum, @costCategory, @costCategoryName)
		END					
		
		FETCH NEXT FROM MyCursor INTO @costCategory
		
	END

	CLOSE MyCursor
	DEALLOCATE MyCursor
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	END 
	ELSE IF @userID >= 1000 AND @userID < 2000
	BEGIN
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
		SET @sumOther = (SELECT SUM(FieldValue) FROM tbMainTable02
						WHERE [UserID] = @UserID 
						AND [Month] = @Month
						AND [Year] = @Year
						AND (CostCategory IS NULL OR CostCategory = 0 AND [IsDeleted] = 0)
	)
	
	IF @Lang = 0
	BEGIN
		SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 1
	BEGIN
		SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 2
	BEGIN
		SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	
	
	INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sumOther, 0, @costCategoryName)

	DECLARE MyCursor CURSOR FOR SELECT ID FROM tbCategories WHERE UserCategoryID = @UserID OR UserCategoryID = 0

	OPEN MyCursor

	FETCH NEXT FROM MyCursor INTO @costCategory

	WHILE @@fetch_status = 0 
	BEGIN		
		
		SET @sum = 
		(SELECT SUM(FieldValue)
		FROM [tbMainTable02]
		WHERE 
		[UserID] = @UserID 
		AND [Month] = @Month
		AND [Year] = @Year
		AND [CostCategory] = @costCategory AND [IsDeleted] = 0)
				
		IF @Lang = 0
		BEGIN
			SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 1
		BEGIN
			SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 2
		BEGIN
			SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
				
		IF @sum IS NOT NULL AND @sum <> 0
		BEGIN								
			INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sum, @costCategory, @costCategoryName)
		END					
		
		FETCH NEXT FROM MyCursor INTO @costCategory
		
	END

	CLOSE MyCursor
	DEALLOCATE MyCursor
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	END


	ELSE IF @userID >= 2000 AND @userID < 3000
	BEGIN
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	SET @sumOther = (SELECT SUM(FieldValue) FROM tbMainTable03
						WHERE [UserID] = @UserID 
						AND [Month] = @Month
						AND [Year] = @Year
						AND (CostCategory IS NULL OR CostCategory = 0 AND [IsDeleted] = 0)
	)
	
	IF @Lang = 0
	BEGIN
		SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 1
	BEGIN
		SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 2
	BEGIN
		SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	
	
	INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sumOther, 0, @costCategoryName)

	DECLARE MyCursor CURSOR FOR SELECT ID FROM tbCategories WHERE UserCategoryID = @UserID OR UserCategoryID = 0

	OPEN MyCursor

	FETCH NEXT FROM MyCursor INTO @costCategory

	WHILE @@fetch_status = 0 
	BEGIN		
		
		SET @sum = 
		(SELECT SUM(FieldValue)
		FROM [tbMainTable03]
		WHERE 
		[UserID] = @UserID 
		AND [Month] = @Month
		AND [Year] = @Year
		AND [CostCategory] = @costCategory AND [IsDeleted] = 0)
				
		IF @Lang = 0
		BEGIN
			SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 1
		BEGIN
			SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 2
		BEGIN
			SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
				
		IF @sum IS NOT NULL AND @sum <> 0
		BEGIN								
			INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sum, @costCategory, @costCategoryName)
		END					
		
		FETCH NEXT FROM MyCursor INTO @costCategory
		
	END

	CLOSE MyCursor
	DEALLOCATE MyCursor
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	END

ELSE IF @userID >= 3000 AND @userID < 4000
	BEGIN
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
		SET @sumOther = (SELECT SUM(FieldValue) FROM tbMainTable04
						WHERE [UserID] = @UserID 
						AND [Month] = @Month
						AND [Year] = @Year
						AND (CostCategory IS NULL OR CostCategory = 0 AND [IsDeleted] = 0)
	)
	
	IF @Lang = 0
	BEGIN
		SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 1
	BEGIN
		SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 2
	BEGIN
		SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	
	
	INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sumOther, 0, @costCategoryName)

	DECLARE MyCursor CURSOR FOR SELECT ID FROM tbCategories WHERE UserCategoryID = @UserID OR UserCategoryID = 0

	OPEN MyCursor

	FETCH NEXT FROM MyCursor INTO @costCategory

	WHILE @@fetch_status = 0 
	BEGIN		
		
		SET @sum = 
		(SELECT SUM(FieldValue)
		FROM [tbMainTable04]
		WHERE 
		[UserID] = @UserID 
		AND [Month] = @Month
		AND [Year] = @Year
		AND [CostCategory] = @costCategory AND [IsDeleted] = 0)
				
		IF @Lang = 0
		BEGIN
			SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 1
		BEGIN
			SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 2
		BEGIN
			SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
				
		IF @sum IS NOT NULL AND @sum <> 0
		BEGIN								
			INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sum, @costCategory, @costCategoryName)
		END					
		
		FETCH NEXT FROM MyCursor INTO @costCategory
		
	END

	CLOSE MyCursor
	DEALLOCATE MyCursor
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	END	

ELSE IF @userID >= 4000 
	BEGIN
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
		SET @sumOther = (SELECT SUM(FieldValue) FROM tbMainTable05
						WHERE [UserID] = @UserID 
						AND [Month] = @Month
						AND [Year] = @Year
						AND (CostCategory IS NULL OR CostCategory = 0 AND [IsDeleted] = 0)
	)
	
	IF @Lang = 0
	BEGIN
		SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 1
	BEGIN
		SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	ELSE IF @Lang = 2
	BEGIN
		SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = '0')
	END
	
	
	INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sumOther, 0, @costCategoryName)

	DECLARE MyCursor CURSOR FOR SELECT ID FROM tbCategories WHERE UserCategoryID = @UserID OR UserCategoryID = 0

	OPEN MyCursor

	FETCH NEXT FROM MyCursor INTO @costCategory

	WHILE @@fetch_status = 0 
	BEGIN		
		
		SET @sum = 
		(SELECT SUM(FieldValue)
		FROM [tbMainTable01]
		WHERE 
		[UserID] = @UserID 
		AND [Month] = @Month
		AND [Year] = @Year
		AND [CostCategory] = @costCategory AND [IsDeleted] = 0)
				
		IF @Lang = 0
		BEGIN
			SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 1
		BEGIN
			SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
		ELSE IF @Lang = 2
		BEGIN
			SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = CAST(@costCategory AS VARCHAR))
		END
				
		IF @sum IS NOT NULL AND @sum <> 0
		BEGIN								
			INSERT INTO @sums (costSum, costCategory, costCategoryName) VALUES (@sum, @costCategory, @costCategoryName)
		END					
		
		FETCH NEXT FROM MyCursor INTO @costCategory
		
	END

	CLOSE MyCursor
	DEALLOCATE MyCursor
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	--//////////////////////////////////////////////////////////////////////////////////////////////
	END	


	SELECT * FROM @sums 

   
END


