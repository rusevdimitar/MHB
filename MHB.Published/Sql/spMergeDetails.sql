USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spMergeDetails]    Script Date: 12/09/2013 16:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2013-12-09
-- Description:	Merges two details records
-- =============================================
ALTER PROCEDURE [dbo].[spMergeDetails]
	-- Add the parameters for the stored procedure here
	@ids NVARCHAR(MAX),
	@detailsTableName NVARCHAR(21)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @qry NVARCHAR(MAX)
	
	SET @qry = 'INSERT INTO ' + CAST(@detailsTableName AS NVARCHAR) + '

		SELECT ExpenditureID, 
		DetailName, 
		DetailDescription, 
		SUM(DetailValue) AS DetailValue, 
		GETDATE() AS DetailDate, 
		NULL AS Attachment, 
		NULL AS AttachmentFileType, 
		NULL AS HasAttachment, 
		0 AS IsDeleted, 
		ProductID, 
		SupplierID, 
		MeasureTypeID, 
		SUM(Amount) AS Amount, 
		GETDATE() AS DetailDateCreated, 
		SUM(DetailValue) AS DetailInitialValue
		
	FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' 
	WHERE ID IN (' + CAST(@ids AS NVARCHAR) + ') 
	GROUP BY DetailName, DetailDescription, ProductID, SupplierID, MeasureTypeID, ExpenditureID, Attachment'


	EXECUTE sp_sqlexec @qry
	END
