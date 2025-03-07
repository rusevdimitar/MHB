CREATE TABLE dbo.tbProducts
(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(300) NOT NULL DEFAULT '',
	[Description] NVARCHAR(MAX) NOT NULL DEFAULT '',
	KeyWords NVARCHAR(MAX) NOT NULL DEFAULT '',
	StandardCost MONEY NOT NULL DEFAULT 0,	
	ListPrice MONEY NOT NULL DEFAULT 0,	
	Color nvarchar(15),
	Picture VARBINARY(MAX),
	[Weight] DECIMAL(18,2),
	Volume DECIMAL(18,2),
	UserID INT,
	DateModified DATETIME,
	IsDeleted BIT NOT NULL DEFAULT 0
	
	FOREIGN KEY (UserID) REFERENCES tbUsers(userID)
)

INSERT INTO dbo.tbProducts (Name, [Description], KeyWords, StandardCost, ListPrice, Color, Picture, [Weight], Volume, UserID, DateModified, IsDeleted)
VALUES ('Default product name', 'Default product desctiption', 'Keyword#1,Keyword#2,Keyword#3', 7.77, 7.77, '#333333', NULL, 7.77, 7.77, 1, GETDATE(), 1)

-- MAIN TABLES
ALTER TABLE dbo.tbMainTable01 ADD ProductID INT NOT NULL DEFAULT 1 
ALTER TABLE dbo.tbMainTable01 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbMainTable02 ADD ProductID INT NOT NULL DEFAULT 1 
ALTER TABLE dbo.tbMainTable02 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbMainTable03 ADD ProductID INT NOT NULL DEFAULT 1 
ALTER TABLE dbo.tbMainTable03 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbMainTable04 ADD ProductID INT NOT NULL DEFAULT 1 
ALTER TABLE dbo.tbMainTable04 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbMainTable05 ADD ProductID INT NOT NULL DEFAULT 1 
ALTER TABLE dbo.tbMainTable05 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbMainTableTemp ADD ProductID INT NOT NULL DEFAULT 1 
ALTER TABLE dbo.tbMainTableTemp ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

-- DETAILS TABLES
ALTER TABLE dbo.tbDetailsTable01 ADD ProductID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable01 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbDetailsTable02 ADD ProductID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable02 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbDetailsTable03 ADD ProductID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable03 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbDetailsTable04 ADD ProductID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable04 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)

ALTER TABLE dbo.tbDetailsTable05 ADD ProductID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable05 ADD FOREIGN KEY (ProductID) REFERENCES tbProducts(ID)
