USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetFrequentlyPurchasedProductsByDate]    Script Date: 09/06/2014 12:39:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 21.08.2014
-- Modify date: 06.09.2014
-- Description:	Gets most frequently purchased items for a given date
-- =============================================
ALTER PROCEDURE [dbo].[spGetFrequentlyPurchasedProductsByDate]
	-- Add the parameters for the stored procedure here
	@top INT,
	@month INT,
	@year INT,	
	@userID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @detailsTableName NVARCHAR(50);
	DECLARE @mainTableName NVARCHAR(50);

	IF @userID < 1000 
	BEGIN
		SELECT @mainTableName = N'tbMainTable01'  
		SELECT @detailsTableName = N'tbDetailsTable01'  
	END
	ELSE IF @userID >= 1000 AND @userID < 2000
	BEGIN
		SELECT @mainTableName = N'tbMainTable02'  
		SELECT @detailsTableName = N'tbDetailsTable02'
	END
	ELSE IF @userID >= 2000 AND @userID < 3000
	BEGIN
		SELECT @mainTableName = N'tbMainTable03'
		SELECT @detailsTableName = N'tbDetailsTable03'
	END
	ELSE IF @userID >= 3000 AND @userID < 4000
	BEGIN
		SELECT @mainTableName = N'tbMainTable04'
		SELECT @detailsTableName = N'tbDetailsTable04' 
	END
	ELSE IF @userID >= 4000 AND @userID < 5000
	BEGIN
		SELECT @mainTableName = N'tbMainTable05'
		SELECT @detailsTableName = N'tbDetailsTable05'
	END
	ELSE IF @userID >= 5000 AND @userID < 6000
	BEGIN
		SELECT @mainTableName = N'tbMainTable06'
		SELECT @detailsTableName = N'tbDetailsTable06'
	END
			
	DECLARE @query NVARCHAR(MAX);
	
	SET @query =	
	'SELECT TOP(' + CAST(@top AS NVARCHAR) + ') * FROM
	(
		SELECT * FROM 
		(
			SELECT v.Name AS SupplierName, CAST(COUNT(dt.ProductID) AS MONEY) AS TotalSumAmount, 0 AS [Count], dt.ProductID, dt.DetailName FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt
			INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON mt.ID = dt.ExpenditureID
			INNER JOIN tbVendors v ON v.VendorID = dt.SupplierID
			WHERE mt.UserID = ' + CAST(@userID AS NVARCHAR) + ' AND mt.Month = ' + CAST(@month AS NVARCHAR) + ' AND mt.Year = ' + CAST(@year AS NVARCHAR) + ' AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.ProductID <> 1 AND dt.MeasureTypeID = 3
			GROUP BY dt.ProductID, dt.DetailName, v.Name
		) t1

		UNION

		SELECT * FROM
		(
			SELECT v.Name AS SupplierName, SUM(dt.Amount) AS TotalSumAmount, COUNT(dt.ProductID) AS [Count], dt.ProductID, dt.DetailName FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt
			INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON mt.ID = dt.ExpenditureID
			INNER JOIN tbVendors v ON v.VendorID = dt.SupplierID
			WHERE mt.UserID = ' + CAST(@userID AS NVARCHAR) + ' AND mt.Month = ' + CAST(@month AS NVARCHAR) + ' AND mt.Year = ' + CAST(@year AS NVARCHAR) + ' AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.ProductID <> 1 AND (dt.MeasureTypeID = 1 OR dt.MeasureTypeID = 2)
			GROUP BY dt.ProductID, dt.DetailName, v.Name
		) t2
	) t3
	ORDER BY TotalSumAmount DESC'

	EXEC sp_executesql @query
	
END
