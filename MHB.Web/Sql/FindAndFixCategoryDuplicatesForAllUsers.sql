	

DECLARE @DuplicatedCategories TABLE (MaxCategoryID INT, MinCategoryID INT, UserID INT, CategoryName NVARCHAR(70)) 

INSERT INTO @DuplicatedCategories
	SELECT DISTINCT MAX(c.ID) AS MaxCategoryID, MIN(c.ID) AS MinCategoryID, c.UserCategoryID AS UserID, c.CategoryName FROM tbUsers u
	INNER JOIN tbCategories c ON u.UserID = c.UserCategoryID
	WHERE c.CategoryName <> ''
	GROUP BY c.CategoryName, c.UserCategoryID
	HAVING COUNT(*) > 1

UPDATE mt SET CostCategory = dup.MinCategoryID
FROM tbMainTable01 AS mt
INNER JOIN @DuplicatedCategories dup ON mt.CostCategory = dup.MaxCategoryID OR mt.CostCategory = dup.MinCategoryID

UPDATE dt SET CategoryID = dup.MinCategoryID
FROM tbDetailsTable01 AS dt
INNER JOIN @DuplicatedCategories dup ON dt.CategoryID = dup.MaxCategoryID OR dt.CategoryID = dup.MinCategoryID

UPDATE mt SET CostCategory = dup.MinCategoryID
FROM tbMainTable02 AS mt
INNER JOIN @DuplicatedCategories dup ON mt.CostCategory = dup.MaxCategoryID OR mt.CostCategory = dup.MinCategoryID

UPDATE dt SET CategoryID = dup.MinCategoryID
FROM tbDetailsTable02 AS dt
INNER JOIN @DuplicatedCategories dup ON dt.CategoryID = dup.MaxCategoryID OR dt.CategoryID = dup.MinCategoryID


UPDATE p SET CategoryID = dup.MinCategoryID
FROM tbProducts AS p
INNER JOIN @DuplicatedCategories dup ON p.CategoryID = dup.MaxCategoryID OR p.CategoryID = dup.MinCategoryID

DELETE FROM tbCostCategories WHERE CostCategoryID IN (SELECT MaxCategoryID FROM @DuplicatedCategories)
DELETE FROM tbCategories WHERE ID IN (SELECT MaxCategoryID FROM @DuplicatedCategories)
DELETE FROM tbLanguage WHERE ISNUMERIC(ControlID) = 1 AND CAST(ControlID AS INT) IN (SELECT MaxCategoryID FROM @DuplicatedCategories)







