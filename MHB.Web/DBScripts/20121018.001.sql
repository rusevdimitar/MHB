DROP TABLE dbo.tbAPIUsers

CREATE TABLE dbo.tbAPIUsers
(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserID INT NOT NULL,
	APIKey VARCHAR(32),
	IsAdmin BIT,
	DateGenerated DATETIME,
	DateRequested AS GETDATE()
)

ALTER TABLE dbo.tbAPIUsers WITH CHECK ADD CONSTRAINT [FK_tbAPIUsers_tbUsers] FOREIGN KEY(UserID)
REFERENCES dbo.tbUsers (userID)
