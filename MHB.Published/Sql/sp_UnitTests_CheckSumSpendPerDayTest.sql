SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2016-04-01
-- Description:	Get total sum spent for a particular date
-- =============================================
CREATE PROCEDURE sp_UnitTests_CheckSumSpendPerDayTest
-- Add the parameters for the stored procedure here
@dateRecordUpdated DATE,
@userID INT
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
DECLARE @sumWithoutTransaction MONEY = 0
DECLARE @sumWithTransaction MONEY = 0
DECLARE @sumDetails MONEY = 0

SET @sumWithoutTransaction =
(SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable01 mt
WHERE ID NOT IN (SELECT ExpenseID from tbTransactionLog) AND mt.IsDeleted = 0 AND mt.UserID = @userID AND CAST(mt.DateRecordUpdated AS DATE) = @dateRecordUpdated AND (mt.HasDetails = 0 OR mt.HasDetails IS NULL))

SET @sumWithTransaction =
(SELECT ISNULL(SUM(tl.NewValue - tl.OldValue), 0) FROM tbMainTable01 mt
INNER JOIN tbTransactionLog tl ON mt.ID = tl.ExpenseID
WHERE mt.IsDeleted = 0 AND mt.UserID = @userID AND CAST(mt.DateRecordUpdated AS DATE) = @dateRecordUpdated AND CAST(tl.DateModified AS DATE) = @dateRecordUpdated AND (mt.HasDetails = 0 OR mt.HasDetails IS NULL))

DECLARE @total MONEY

SET @sumDetails =
(SELECT ISNULL(SUM(DetailValue), 0) FROM tbDetailsTable01 dt
INNER JOIN tbMainTable01 mt ON mt.ID = dt.ExpenditureID
WHERE dt.IsDeleted = 0 AND mt.IsDeleted = 0 AND mt.UserID = @userID AND mt.HasDetails = 1 AND CAST(dt.DetailDate AS DATE) = @dateRecordUpdated)

SET @total = @sumWithoutTransaction + @sumWithTransaction + @sumDetails

SELECT @total
END
GO