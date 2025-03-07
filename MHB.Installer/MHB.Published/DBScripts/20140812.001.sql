ALTER TABLE tbDetailsTable01 ADD CategoryID INT NULL FOREIGN KEY REFERENCES dbo.tbCategories (ID)
ALTER TABLE tbDetailsTable02 ADD CategoryID INT NULL FOREIGN KEY REFERENCES dbo.tbCategories (ID)
ALTER TABLE tbDetailsTable03 ADD CategoryID INT NULL FOREIGN KEY REFERENCES dbo.tbCategories (ID)
ALTER TABLE tbDetailsTable04 ADD CategoryID INT NULL FOREIGN KEY REFERENCES dbo.tbCategories (ID)
ALTER TABLE tbDetailsTable05 ADD CategoryID INT NULL FOREIGN KEY REFERENCES dbo.tbCategories (ID)

SELECT * FROM tbCategories WHERE CategoryName = 'Бира'

DECLARE @categoryID INT = 0

SET @categoryID = (SELECT ID FROM tbCategories WHERE CategoryName = 'Бира')

DECLARE @tbProductIds TABLE
(
	ID INT
)

INSERT INTO @tbProductIds
SELECT DISTINCT dt.ProductID FROM tbDetailsTable01 dt
INNER JOIN tbMainTable01 mt ON dt.ExpenditureID = mt.ID
WHERE mt.IsDeleted = 0 AND dt.IsDeleted = 0 AND mt.UserID = 1 AND dt.ProductID <> 1
AND 
(
	dt.DetailName LIKE '%бира%' OR
	dt.DetailName LIKE '%каменица%' OR
	dt.DetailName LIKE '%болярка%' OR
	dt.DetailName LIKE '%шуменско%' OR
	dt.DetailName LIKE '%хайнекен%' OR
	dt.DetailName LIKE '%столично%' OR
	dt.DetailName LIKE '%аргус%' OR	
	dt.DetailName LIKE '%астика%' OR
	dt.DetailName LIKE '%туборг%' OR
	dt.DetailName LIKE '%загорка%' OR
	dt.DetailName LIKE '%амстел%' OR
	dt.DetailName LIKE '%пилзенер%' OR
	dt.DetailName LIKE '%пиринско%' OR
	dt.DetailName LIKE '%бекс%' OR
	dt.DetailName LIKE '%beck''s%' OR
	dt.DetailName LIKE '%zahringer%' OR
	dt.DetailName LIKE '%пшенично%' OR
	dt.DetailName LIKE '%Shoffenhofer%' OR
	dt.DetailName LIKE '%artois%' OR
	dt.DetailName LIKE '%Mythos%'
)

UPDATE tbDetailsTable01 SET CategoryID = @categoryID WHERE ProductID IN (SELECT ID FROM @tbProductIds)
UPDATE tbProducts SET CategoryID = @categoryID WHERE ID IN (SELECT ID FROM @tbProductIds)

SELECT CategoryID, * FROM tbDetailsTable01 WHERE ProductID IN (SELECT ID FROM @tbProductIds)
SELECT CategoryID, * FROM tbProducts WHERE ID IN (SELECT ID FROM @tbProductIds)
