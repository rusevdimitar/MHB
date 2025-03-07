--use master

--RESTORE DATABASE Test01Db FROM DISK='D:\GoogleDrive\MHB_DB_BACKUPS\MHB_Db_20161207_1600.bak'
--WITH
--   MOVE 'smetkieu_db1' TO 'c:\DATA\Test01Db.mdf',
--   MOVE 'smetkieu_db1_log' TO 'c:\DATA\Test01Db_log.ldf'


use Test01Db
DELETE FROM tbDetailsTable01
GO
DELETE FROM tbDetailsTable02
GO
DELETE FROM tbDetailsTable03
GO

DELETE FROM tbMainTable01
GO
DELETE FROM tbMainTable02
GO
DELETE FROM tbMainTable03
GO

DELETE tbCostCategories
FROM tbCostCategories cc
	INNER JOIN tbCategories c ON cc.CostCategoryID = c.ID
		WHERE c.UserCategoryID <> 25 AND c.UserCategoryID <> 1
GO

DELETE FROM tbProductParameters
GO

DELETE tbProducts
FROM tbProducts p
	INNER JOIN tbCategories c ON c.ID = p.CategoryID
		WHERE c.UserCategoryID <> 25 AND c.UserCategoryID <> 1
GO

DELETE FROM tbCategoryComments
GO

DELETE FROM tbCategories WHERE UserCategoryID <> 25 AND UserCategoryID <> 1
GO

DELETE tbLanguage
FROM tbLanguage l
	INNER JOIN tbCategories c ON ISNUMERIC(l.ControlID)=1 AND c.ID=l.ControlID
		WHERE c.UserCategoryID <> 25 AND c.UserCategoryID <> 1
GO

DELETE FROM tbIncomeLog
GO

DELETE FROM tbIncomes
GO

DELETE FROM tbMonthlyBudget
GO

DELETE FROM tbMonthlyExpenses
GO

DELETE FROM tbMonthlySavings
GO

DELETE FROM tbNotes
GO

DELETE FROM tbTransactionLog
GO

DELETE FROM tbActionLog
GO

DELETE FROM tbLog
GO

DELETE FROM tbCustomListItems
GO

DELETE FROM tbCustomPageControls
GO

DELETE FROM tbVendors WHERE UserID <> 1 AND UserID <> 25
GO

DELETE FROM tbUsersBlackList
GO

DELETE FROM tbAPIUsers
GO

DELETE FROM tbUsers WHERE UserID <> 1 AND UserID <> 25
GO

DELETE FROM tbUsersGeoLocationData
GO

DELETE FROM tbCurrencyExchangeRates
GO

