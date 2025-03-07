CREATE TABLE dbo.tbIncomeActions
(
	ID INT NOT NULL PRIMARY KEY,
	Name VARCHAR(6) NOT NULL DEFAULT '',
	[Description] NVARCHAR(400) NOT NULL DEFAULT ''
)

INSERT INTO tbIncomeActions (ID, Name, [Description]) VALUES (1, 'Add', 'Add new income')
INSERT INTO tbIncomeActions (ID, Name, [Description]) VALUES (2, 'Update', 'Update income')
INSERT INTO tbIncomeActions (ID, Name, [Description]) VALUES (3, 'Delete', 'Delete income')

CREATE TABLE dbo.tbIncomeLog
(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IncomeID INT,
	Name NVARCHAR(200) NOT NULL DEFAULT '',
	Value MONEY NOT NULL DEFAULT 0,
	UserID INT,
	[Month] INT,
	[Year] INT,
	IncomeDate DATETIME,
	DateModified DATETIME,
	IncomeAction INT,	
	FOREIGN KEY (IncomeID) REFERENCES tbIncomes(ID),
	FOREIGN KEY (UserID) REFERENCES tbUsers(userID),
	FOREIGN KEY (IncomeAction) REFERENCES tbIncomeActions
)

INSERT INTO tbIncomeLog (Name, Value, UserID, [Month], [Year], IncomeDate, DateModified, IncomeAction)
SELECT IncomeName, IncomeValue, UserID, [Month], [Year], IncomeDate, GETDATE(), 3 FROM tbIncomes WHERE ID = @id

ALTER TABLE tbIncomes ADD IsDeleted BIT NOT NULL DEFAULT 0
ALTER TABLE tbIncomes ADD DateModified DATETIME