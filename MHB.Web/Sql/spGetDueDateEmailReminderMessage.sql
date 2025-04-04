USE [100_Test01Db]
GO
/****** Object:  StoredProcedure [100_neonglow].[spGetDueDateEmailReminderMessage]    Script Date: 04/15/2010 21:57:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 17.02.2010
-- Description:	Returns the email, bill and due date
-- =============================================
ALTER PROCEDURE [100_neonglow].[spGetDueDateEmailReminderMessage]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
		SELECT tmt.ID, tmt.FieldName, tmt.FieldDescription, tmt.FieldValue, tu.currency, tmt.DueDate, tu.userID, tu.email 
		FROM tbMainTable01 tmt
		INNER JOIN tbUsers tu ON tu.userID = tmt.UserID 
		WHERE DATEDIFF(dd, 0, tmt.DueDate) - 1 = DATEDIFF(dd, 0, GETDATE())
		AND tmt.Notified = 0 AND (tmt.IsPaid = 0 OR tmt.IsPaid IS NULL)
		
		UNION
	
		SELECT tmt.ID, tmt.FieldName, tmt.FieldDescription, tmt.FieldValue, tu.currency, tmt.DueDate, tu.userID, tu.email 
		FROM tbMainTable02 tmt
		INNER JOIN tbUsers tu ON tu.userID = tmt.UserID 
		WHERE DATEDIFF(dd, 0, tmt.DueDate) - 1 = DATEDIFF(dd, 0, GETDATE())
		AND tmt.Notified = 0 AND (tmt.IsPaid = 0 OR tmt.IsPaid IS NULL)
		
		UNION
	
		SELECT tmt.ID, tmt.FieldName, tmt.FieldDescription, tmt.FieldValue, tu.currency, tmt.DueDate, tu.userID, tu.email 
		FROM tbMainTable03 tmt
		INNER JOIN tbUsers tu ON tu.userID = tmt.UserID 
		WHERE DATEDIFF(dd, 0, tmt.DueDate) - 1 = DATEDIFF(dd, 0, GETDATE())
		AND tmt.Notified = 0 AND (tmt.IsPaid = 0 OR tmt.IsPaid IS NULL)
		
		UNION
	
		SELECT tmt.ID, tmt.FieldName, tmt.FieldDescription, tmt.FieldValue, tu.currency, tmt.DueDate, tu.userID, tu.email 
		FROM tbMainTable04 tmt
		INNER JOIN tbUsers tu ON tu.userID = tmt.UserID 
		WHERE DATEDIFF(dd, 0, tmt.DueDate) - 1 = DATEDIFF(dd, 0, GETDATE())
		AND tmt.Notified = 0 AND (tmt.IsPaid = 0 OR tmt.IsPaid IS NULL)
		
		UNION
	
		SELECT tmt.ID, tmt.FieldName, tmt.FieldDescription, tmt.FieldValue, tu.currency, tmt.DueDate, tu.userID, tu.email 
		FROM tbMainTable05 tmt
		INNER JOIN tbUsers tu ON tu.userID = tmt.UserID 
		WHERE DATEDIFF(dd, 0, tmt.DueDate) - 1 = DATEDIFF(dd, 0, GETDATE())
		AND tmt.Notified = 0 AND (tmt.IsPaid = 0 OR tmt.IsPaid IS NULL)
	

 
END

