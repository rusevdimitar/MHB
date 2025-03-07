USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetDetailsProductPriceStatistics]    Script Date: 4/11/2016 10:10:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2013-07-19
-- Modified:	2014-01-27
-- Description:	Gets product price statistics
-- =============================================
ALTER PROCEDURE [dbo].[spGetDetailsProductPriceStatistics]
	@productID INT,
	@measureTypeID INT = -1
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @userID AS INT = (SELECT UserID FROM tbProducts WHERE ID = @productID)
	
	DECLARE @totalProductEntries INT = (SELECT COUNT(ID) FROM tbDetailsTable01 WHERE ProductID = @productID AND IsDeleted = 0)
	
	IF @userID < 1000
	BEGIN
		-- If no measureTypeID is passed get the most frequently occurring one:
		IF @measureTypeID = -1 OR @measureTypeID = 0
		BEGIN
			SET @measureTypeID = (SELECT TOP 1
			  MeasureTypeID
			FROM
			  tbDetailsTable01
			WHERE IsDeleted = 0 AND ProductID = @productID
			GROUP BY
			  MeasureTypeID
			ORDER BY
			  COUNT(MeasureTypeID) DESC)
		END
		
		SELECT 
		CASE
			WHEN MeasureTypeID = 1 OR MeasureTypeID = 2 THEN DetailValue / Amount			
			WHEN MeasureTypeID = 3 THEN DetailValue
		END AS Price,
		DetailDate,
		SupplierID,
		Amount,
		@totalProductEntries AS TotalProductEntriesCount

		FROM tbDetailsTable01 
		WHERE
		MeasureTypeID = @measureTypeID AND IsDeleted = 0 AND ProductID = @productID
		ORDER BY DetailDate DESC
	END
	-- ==================================================================================================
	ELSE IF @userID >= 1000 AND @userID < 2000
	BEGIN
		
		-- If no measureTypeID is passed get the most frequently occurring one:
		IF @measureTypeID = -1
		BEGIN
			SET @measureTypeID = (SELECT TOP 1
			  MeasureTypeID
			FROM
			  tbDetailsTable02
			WHERE IsDeleted = 0 AND ProductID = @productID
			GROUP BY
			  MeasureTypeID
			ORDER BY
			  COUNT(MeasureTypeID) DESC)
		END
	
		SELECT 
		CASE
			WHEN MeasureTypeID = 1 OR MeasureTypeID = 2 THEN DetailValue / Amount			
			WHEN MeasureTypeID = 3 THEN DetailValue
		END AS Price,
		DetailDate,
		SupplierID,
		Amount,
		@totalProductEntries AS TotalProductEntriesCount

		FROM tbDetailsTable02 
		WHERE
		MeasureTypeID = @measureTypeID AND IsDeleted = 0 AND ProductID = @productID
		ORDER BY DetailDate DESC
	END
	-- ==================================================================================================
	ELSE IF @userID >= 2000 AND @userID < 3000
	BEGIN
	
		-- If no measureTypeID is passed get the most frequently occurring one:
		IF @measureTypeID = -1
		BEGIN
			SET @measureTypeID = (SELECT TOP 1
			  MeasureTypeID
			FROM
			  tbDetailsTable03
			WHERE IsDeleted = 0 AND ProductID = @productID
			GROUP BY
			  MeasureTypeID
			ORDER BY
			  COUNT(MeasureTypeID) DESC)
		END
		
		SELECT 
		CASE
			WHEN MeasureTypeID = 1 OR MeasureTypeID = 2 THEN DetailValue / Amount			
			WHEN MeasureTypeID = 3 THEN DetailValue
		END AS Price,
		DetailDate,
		SupplierID,
		Amount,
		@totalProductEntries AS TotalProductEntriesCount

		FROM tbDetailsTable03
		WHERE
		MeasureTypeID = @measureTypeID AND IsDeleted = 0 AND ProductID = @productID
		ORDER BY DetailDate DESC
	END
	-- ==================================================================================================
	ELSE IF @userID >= 3000 AND @userID < 4000
	BEGIN
	
		-- If no measureTypeID is passed get the most frequently occurring one:
		IF @measureTypeID = -1
		BEGIN
			SET @measureTypeID = (SELECT TOP 1
			  MeasureTypeID
			FROM
			  tbDetailsTable04
			WHERE IsDeleted = 0 AND ProductID = @productID
			GROUP BY
			  MeasureTypeID
			ORDER BY
			  COUNT(MeasureTypeID) DESC)
		END
		
		SELECT 
		CASE
			WHEN MeasureTypeID = 1 OR MeasureTypeID = 2 THEN DetailValue / Amount
			WHEN MeasureTypeID = 3 THEN DetailValue
		END AS Price,
		DetailDate,
		SupplierID,
		Amount,
		@totalProductEntries AS TotalProductEntriesCount

		FROM tbDetailsTable04
		WHERE
		MeasureTypeID = @measureTypeID AND IsDeleted = 0 AND ProductID = @productID
		ORDER BY DetailDate DESC
	END
	-- ==================================================================================================
	ELSE IF @userID >= 4000 AND @userID < 5000
	BEGIN
	
		-- If no measureTypeID is passed get the most frequently occurring one:
		IF @measureTypeID = -1
		BEGIN
			SET @measureTypeID = (SELECT TOP 1
			  MeasureTypeID
			FROM
			  tbDetailsTable05
			WHERE IsDeleted = 0 AND ProductID = @productID
			GROUP BY
			  MeasureTypeID
			ORDER BY
			  COUNT(MeasureTypeID) DESC)
		END
		
		SELECT 
		CASE			
			WHEN MeasureTypeID = 1 OR MeasureTypeID = 2 THEN DetailValue / Amount
			WHEN MeasureTypeID = 3 THEN DetailValue
		END AS Price,
		DetailDate,
		SupplierID,
		Amount,
		@totalProductEntries AS TotalProductEntriesCount

		FROM tbDetailsTable05
		WHERE
		MeasureTypeID = @measureTypeID AND IsDeleted = 0 AND ProductID = @productID
		ORDER BY DetailDate DESC
	END
	-- ==================================================================================================

END
