--EXEC sp_spaceused tbDetailsTable01

--EXEC sp_helpindex tbUsers 



DROP INDEX [IDX_CategoryName] ON tbCategories

DROP INDEX [IDX_CostCategoryID] ON tbCostCategories

DROP INDEX [IDX_ControlID] ON tbLanguage

DROP INDEX [IDX_email] ON dbo.tbUsers 

DROP INDEX [password] ON dbo.tbUsers

DROP INDEX [IDX_ProductName] ON dbo.tbProducts

DROP INDEX [IDX_VendorName] ON dbo.tbVendors

DROP INDEX [IDX_VendorAccountNumber] ON dbo.tbVendors


/*
	tbMainTable01
*/
DROP INDEX [IDX_Year_mt1] ON tbMainTable01

DROP INDEX [IDX_UserID_mt1] ON tbMainTable01

DROP INDEX [IDX_Month_mt1] ON tbMainTable01

DROP INDEX [IDX_IsDeleted_mt1] ON tbMainTable01
-- ================================================

/*
	tbMainTable02
*/
DROP INDEX [IDX_Year_mt2] ON tbMainTable02

DROP INDEX [IDX_UserID_mt2] ON tbMainTable02

DROP INDEX [IDX_Month_mt2] ON tbMainTable02

DROP INDEX [IDX_IsDeleted_mt2] ON tbMainTable01
-- ================================================





CREATE INDEX [IDX_CategoryName] ON tbCategories (CategoryName)

CREATE INDEX [IDX_CostCategoryID] ON tbCostCategories (CostCategoryID)

CREATE INDEX [IDX_ControlID] ON tbLanguage (ControlID)

CREATE INDEX [IDX_email] ON dbo.tbUsers (email)

CREATE INDEX [password] ON dbo.tbUsers ([password])

CREATE INDEX [IDX_ProductName] ON dbo.tbProducts (Name)

CREATE INDEX [IDX_VendorName] ON dbo.tbVendors (Name)

CREATE INDEX [IDX_VendorAccountNumber] ON dbo.tbVendors (AccountNumber)

/*
	tbMainTable01
*/
CREATE INDEX [IDX_Year_mt1] ON tbMainTable01 ([Year])

CREATE INDEX [IDX_UserID_mt1] ON tbMainTable01 (UserID)

CREATE INDEX [IDX_Month_mt1] ON tbMainTable01 ([Month])

CREATE INDEX [IDX_IsDeleted_mt1] ON tbMainTable01 (IsDeleted)--EXEC sp_spaceused tbDetailsTable01


/*
	tbMainTable02
*/
CREATE INDEX [IDX_Year_mt2] ON tbMainTable02 ([Year])

CREATE INDEX [IDX_UserID_mt2] ON tbMainTable02 (UserID)

CREATE INDEX [IDX_Month_mt2] ON tbMainTable02 ([Month])

CREATE INDEX [IDX_IsDeleted_mt2] ON tbMainTable01 (IsDeleted)
-- ================================================





--CREATE UNIQUE CLUSTERED INDEX [IDX_ControlID] ON tbLanguage (ControlID)