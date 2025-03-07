USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[sp_UnitTests_GetDetailsPerCategory]    Script Date: 4/1/2016 9:50:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2015-04-24 11:01
-- Description:	Unit Tests - Get details per category
-- =============================================
CREATE PROCEDURE [dbo].[sp_UnitTests_GetDetailsPerCategory]

	@Year INT,
	@Month INT,
	@CategoryID INT
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @PrevailingMeasureTypeID INT

SET @PrevailingMeasureTypeID = 
	(
		SELECT TOP 1 MeasureTypeID FROM tbDetailsTable01 dt
		INNER JOIN tbMainTable01 mt ON dt.ExpenditureID = mt.ID
		WHERE dt.IsDeleted = 0 AND mt.IsDeleted = 0 AND 		
		MONTH(dt.DetailDate) = CASE WHEN @Month=0 THEN MONTH(dt.DetailDate) ELSE @Month END AND
		YEAR(dt.DetailDate) = @Year AND UserID = 1 AND CategoryID = @CategoryID
		GROUP BY dt.MeasureTypeID
		ORDER BY COUNT(MeasureTypeID) DESC
	)

SELECT dt.* FROM tbDetailsTable01 dt
	INNER JOIN tbMainTable01 mt ON dt.ExpenditureID = mt.ID
	WHERE dt.IsDeleted = 0 AND mt.IsDeleted = 0 AND 

	MONTH(dt.DetailDate) = CASE WHEN @Month=0 THEN MONTH(dt.DetailDate) ELSE @Month END AND
	YEAR(dt.DetailDate) = @Year AND UserID = 1 AND CategoryID = @CategoryID AND

	UserID = 1 AND CategoryID = @CategoryID AND MeasureTypeID = @PrevailingMeasureTypeID
	
	
END
