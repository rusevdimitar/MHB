-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2013-08-20
-- Description:	Get search keywords
-- =============================================
ALTER PROCEDURE spGetSearchKeywords
	-- Add the parameters for the stored procedure here
	@UserID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT DISTINCT dt.DetailName AS Keywords FROM tbMainTable01 mt
	INNER JOIN tbDetailsTable01 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailName IS NOT NULL AND LEN(dt.DetailName) > 2

	UNION

	SELECT DISTINCT dt.DetailDescription AS Keywords FROM tbMainTable01 mt
	INNER JOIN tbDetailsTable01 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailDescription IS NOT NULL AND LEN(dt.DetailDescription) > 2

	UNION

	SELECT DISTINCT mt.FieldName AS Keywords FROM tbMainTable01 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldName IS NOT NULL AND LEN(mt.FieldName) > 2

	UNION

	SELECT DISTINCT mt.FieldDescription AS Keywords FROM tbMainTable01 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldDescription IS NOT NULL AND LEN(mt.FieldDescription) > 2
	
	-- ======================================================================================================================
	-- #2
	-- ======================================================================================================================
	
	UNION
	
	SELECT DISTINCT dt.DetailName AS Keywords FROM tbMainTable02 mt
	INNER JOIN tbDetailsTable02 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailName IS NOT NULL AND LEN(dt.DetailName) > 2

	UNION

	SELECT DISTINCT dt.DetailDescription AS Keywords FROM tbMainTable02 mt
	INNER JOIN tbDetailsTable02 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailDescription IS NOT NULL AND LEN(dt.DetailDescription) > 2

	UNION

	SELECT DISTINCT mt.FieldName AS Keywords FROM tbMainTable02 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldName IS NOT NULL AND LEN(mt.FieldName) > 2

	UNION

	SELECT DISTINCT mt.FieldDescription AS Keywords FROM tbMainTable02 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldDescription IS NOT NULL AND LEN(mt.FieldDescription) > 2
	
	-- ======================================================================================================================
	-- #3
	-- ======================================================================================================================
	
	UNION
	
	SELECT DISTINCT dt.DetailName AS Keywords FROM tbMainTable03 mt
	INNER JOIN tbDetailsTable03 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailName IS NOT NULL AND LEN(dt.DetailName) > 2

	UNION

	SELECT DISTINCT dt.DetailDescription AS Keywords FROM tbMainTable03 mt
	INNER JOIN tbDetailsTable03 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailDescription IS NOT NULL AND LEN(dt.DetailDescription) > 2

	UNION

	SELECT DISTINCT mt.FieldName AS Keywords FROM tbMainTable03 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldName IS NOT NULL AND LEN(mt.FieldName) > 2

	UNION

	SELECT DISTINCT mt.FieldDescription AS Keywords FROM tbMainTable03 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldDescription IS NOT NULL AND LEN(mt.FieldDescription) > 2
	
	-- ======================================================================================================================
	-- #4
	-- ======================================================================================================================
	
	UNION
	
	SELECT DISTINCT dt.DetailName AS Keywords FROM tbMainTable04 mt
	INNER JOIN tbDetailsTable04 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailName IS NOT NULL AND LEN(dt.DetailName) > 2

	UNION

	SELECT DISTINCT dt.DetailDescription AS Keywords FROM tbMainTable04 mt
	INNER JOIN tbDetailsTable04 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailDescription IS NOT NULL AND LEN(dt.DetailDescription) > 2

	UNION

	SELECT DISTINCT mt.FieldName AS Keywords FROM tbMainTable04 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldName IS NOT NULL AND LEN(mt.FieldName) > 2

	UNION

	SELECT DISTINCT mt.FieldDescription AS Keywords FROM tbMainTable04 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldDescription IS NOT NULL AND LEN(mt.FieldDescription) > 2
	
	-- ======================================================================================================================
	-- #5
	-- ======================================================================================================================
	
	UNION
	
	SELECT DISTINCT dt.DetailName AS Keywords FROM tbMainTable05 mt
	INNER JOIN tbDetailsTable05 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailName IS NOT NULL AND LEN(dt.DetailName) > 2

	UNION

	SELECT DISTINCT dt.DetailDescription AS Keywords FROM tbMainTable05 mt
	INNER JOIN tbDetailsTable05 dt ON dt.ExpenditureID = mt.ID
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND dt.DetailDescription IS NOT NULL AND LEN(dt.DetailDescription) > 2

	UNION

	SELECT DISTINCT mt.FieldName AS Keywords FROM tbMainTable05 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldName IS NOT NULL AND LEN(mt.FieldName) > 2

	UNION

	SELECT DISTINCT mt.FieldDescription AS Keywords FROM tbMainTable05 mt
	WHERE mt.UserID = @UserID AND mt.IsDeleted = 0 AND mt.FieldDescription IS NOT NULL AND LEN(mt.FieldDescription) > 2
		
END
GO
