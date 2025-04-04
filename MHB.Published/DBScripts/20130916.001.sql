USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetFuelStatistics]    Script Date: 09/16/2013 14:10:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2013-09-16 13:39
-- Description:	Displays expected trip range, fuel quantity and average fuel consumption for a given month
--				based on the planned sum for fuel and what's been spent on fuel last month
-- =============================================
CREATE PROCEDURE [dbo].[spGetFuelStatistics]
	-- Add the parameters for the stored procedure here
	@month INT,
	@year INT,
	@userID INT


AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @productID INT
	DECLARE @productName NVARCHAR(30)
	
	DECLARE @fuelStatistics TABLE
	(
		Name NVARCHAR(30),
		AveragePriceBasedOnLastAndThisMonth MONEY,
		ExpectedQuantityForThisMonth DECIMAL(10,2),
		AverageConsumptionBasedOnLastAndThisMonth DECIMAL(10,2),
		ExpectedRange DECIMAL
	)

DECLARE myCursor CURSOR FOR
	SELECT ID, Name FROM tbProducts WHERE IsDeleted = 0 AND CategoryID = 1 AND UserID = @userID
	
	OPEN myCursor

	FETCH NEXT FROM myCursor INTO @productID, @productName

		WHILE @@fetch_status = 0 
						
		BEGIN
			DECLARE @expectedValue MONEY = 0
			DECLARE @averagePrice MONEY = 0
			DECLARE @averageConsumption DECIMAL(10,2) = 0
			
			-- ==================================================================================================
			IF @userID < 1000
			BEGIN
			
				SET @expectedValue = (SELECT FieldExpectedValue FROM tbMainTable01 WHERE IsDeleted = 0 AND Month = @month AND Year = @year AND UserID = @userID AND CostCategory = 1)
																
				SET @averagePrice = (SELECT AVG(dt1.DetailValue / dt1.Amount)
												FROM tbDetailsTable01 dt1
												INNER JOIN tbMainTable01 mt1 ON mt1.ID = dt1.ExpenditureID
												WHERE 
												mt1.IsDeleted = 0
												AND dt1.IsDeleted = 0
												AND dt1.ProductID = @productID
												AND dt1.Amount > 1
												AND dt1.MeasureTypeID = 2
												AND mt1.Month IN (@month, (@month - 1))
												AND mt1.Year = @year)
																																
				SET @averageConsumption = (SELECT AVG(dt.Amount / pp.Value * 100) FROM tbProductParameters pp
														INNER JOIN tbDetailsTable01 dt ON dt.ID = pp.ParentID
														INNER JOIN tbMainTable01 mt ON mt.ID = dt.ExpenditureID
														WHERE 
														dt.IsDeleted = 0 
														AND dt.MeasureTypeID = 2 
														AND dt.Amount > 1 
														AND mt.CostCategory = 1 
														AND [Month] IN (@month, (@month - 1))
														AND pp.Value > 0
														AND pp.ProductID = @productID)
			END
			-- ==================================================================================================
			ELSE IF @userID >= 1000 AND @userID < 2000
			BEGIN
				SET @expectedValue = (SELECT FieldExpectedValue FROM tbMainTable02 WHERE IsDeleted = 0 AND Month = @month AND Year = @year AND UserID = @userID AND CostCategory = 1)
																
				SET @averagePrice = (SELECT AVG(dt1.DetailValue / dt1.Amount)
												FROM tbDetailsTable02 dt1
												INNER JOIN tbMainTable02 mt1 ON mt1.ID = dt1.ExpenditureID
												WHERE 
												mt1.IsDeleted = 0
												AND dt1.IsDeleted = 0
												AND dt1.ProductID = @productID
												AND dt1.Amount > 1
												AND dt1.MeasureTypeID = 2
												AND mt1.Month IN (@month, (@month - 1))
												AND mt1.Year = @year)
																																
				SET @averageConsumption = (SELECT AVG(dt.Amount / pp.Value * 100) FROM tbProductParameters pp
														INNER JOIN tbDetailsTable02 dt ON dt.ID = pp.ParentID
														INNER JOIN tbMainTable02 mt ON mt.ID = dt.ExpenditureID
														WHERE 
														dt.IsDeleted = 0 
														AND dt.MeasureTypeID = 2 
														AND dt.Amount > 1 
														AND mt.CostCategory = 1 
														AND [Month] IN (@month, (@month - 1))
														AND pp.Value > 0
														AND pp.ProductID = @productID)
			END
			-- ==================================================================================================
			ELSE IF @userID >= 2000 AND @userID < 3000
			BEGIN
				SET @expectedValue = (SELECT FieldExpectedValue FROM tbMainTable03 WHERE IsDeleted = 0 AND Month = @month AND Year = @year AND UserID = @userID AND CostCategory = 1)
																
				SET @averagePrice = (SELECT AVG(dt1.DetailValue / dt1.Amount)
												FROM tbDetailsTable03 dt1
												INNER JOIN tbMainTable03 mt1 ON mt1.ID = dt1.ExpenditureID
												WHERE 
												mt1.IsDeleted = 0
												AND dt1.IsDeleted = 0
												AND dt1.ProductID = @productID
												AND dt1.Amount > 1
												AND dt1.MeasureTypeID = 2
												AND mt1.Month IN (@month, (@month - 1))
												AND mt1.Year = @year)
																																
				SET @averageConsumption = (SELECT AVG(dt.Amount / pp.Value * 100) FROM tbProductParameters pp
														INNER JOIN tbDetailsTable03 dt ON dt.ID = pp.ParentID
														INNER JOIN tbMainTable03 mt ON mt.ID = dt.ExpenditureID
														WHERE 
														dt.IsDeleted = 0 
														AND dt.MeasureTypeID = 2 
														AND dt.Amount > 1 
														AND mt.CostCategory = 1 
														AND [Month] IN (@month, (@month - 1))
														AND pp.Value > 0
														AND pp.ProductID = @productID)
			END
			-- ==================================================================================================
			ELSE IF @userID >= 3000 AND @userID < 4000
			BEGIN
				SET @expectedValue = (SELECT FieldExpectedValue FROM tbMainTable04 WHERE IsDeleted = 0 AND Month = @month AND Year = @year AND UserID = @userID AND CostCategory = 1)
																
				SET @averagePrice = (SELECT AVG(dt1.DetailValue / dt1.Amount)
												FROM tbDetailsTable04 dt1
												INNER JOIN tbMainTable04 mt1 ON mt1.ID = dt1.ExpenditureID
												WHERE 
												mt1.IsDeleted = 0
												AND dt1.IsDeleted = 0
												AND dt1.ProductID = @productID
												AND dt1.Amount > 1
												AND dt1.MeasureTypeID = 2
												AND mt1.Month IN (@month, (@month - 1))
												AND mt1.Year = @year)
																																
				SET @averageConsumption = (SELECT AVG(dt.Amount / pp.Value * 100) FROM tbProductParameters pp
														INNER JOIN tbDetailsTable04 dt ON dt.ID = pp.ParentID
														INNER JOIN tbMainTable04 mt ON mt.ID = dt.ExpenditureID
														WHERE 
														dt.IsDeleted = 0 
														AND dt.MeasureTypeID = 2 
														AND dt.Amount > 1 
														AND mt.CostCategory = 1 
														AND [Month] IN (@month, (@month - 1))
														AND pp.Value > 0
														AND pp.ProductID = @productID)
			END		
			-- ==================================================================================================
			ELSE IF @userID >= 4000 AND @userID < 5000
			BEGIN
				SET @expectedValue = (SELECT FieldExpectedValue FROM tbMainTable05 WHERE IsDeleted = 0 AND Month = @month AND Year = @year AND UserID = @userID AND CostCategory = 1)
																
				SET @averagePrice = (SELECT AVG(dt1.DetailValue / dt1.Amount)
												FROM tbDetailsTable05 dt1
												INNER JOIN tbMainTable05 mt1 ON mt1.ID = dt1.ExpenditureID
												WHERE 
												mt1.IsDeleted = 0
												AND dt1.IsDeleted = 0
												AND dt1.ProductID = @productID
												AND dt1.Amount > 1
												AND dt1.MeasureTypeID = 2
												AND mt1.Month IN (@month, (@month - 1))
												AND mt1.Year = @year)
																																
				SET @averageConsumption = (SELECT AVG(dt.Amount / pp.Value * 100) FROM tbProductParameters pp
														INNER JOIN tbDetailsTable05 dt ON dt.ID = pp.ParentID
														INNER JOIN tbMainTable05 mt ON mt.ID = dt.ExpenditureID
														WHERE 
														dt.IsDeleted = 0 
														AND dt.MeasureTypeID = 2 
														AND dt.Amount > 1 
														AND mt.CostCategory = 1 
														AND [Month] IN (@month, (@month - 1))
														AND pp.Value > 0
														AND pp.ProductID = @productID)
			END
			-- ==================================================================================================			
									
														
			DECLARE @expectedRange DECIMAL = ((@expectedValue / @averagePrice) / @averageConsumption) * 100
								
			INSERT INTO @fuelStatistics (Name, AveragePriceBasedOnLastAndThisMonth, ExpectedQuantityForThisMonth, AverageConsumptionBasedOnLastAndThisMonth, ExpectedRange) 
			VALUES (@productName, @averagePrice, @expectedValue / @averagePrice, ISNULL(@averageConsumption, 0), ISNULL(@expectedRange, 0))
			
			FETCH NEXT FROM myCursor INTO @productID, @productName
		END
		
	CLOSE myCursor
	DEALLOCATE myCursor
	
SELECT * FROM @fuelStatistics
  
END
