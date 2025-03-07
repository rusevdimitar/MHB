drop table tbCategoryComments

CREATE TABLE dbo.tbCategoryComments
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	UserID INT NOT NULL,
	CategoryID INT NOT NULL,
	Poster NVARCHAR(100) NOT NULL DEFAULT '',
	Comment NVARCHAR(MAX) NOT NULL DEFAULT '',
	PositiveVotes INT NOT NULL DEFAULT 0,
	NegativeVotes INT NOT NULL DEFAULT 0,	
	UsersVoted VARCHAR(MAX),
	IsDeleted BIT NOT NULL DEFAULT 0,
	DateModified DATETIME,
	FOREIGN KEY (UserID) REFERENCES tbUsers(userID),
	FOREIGN KEY (CategoryID) REFERENCES tbCategories (ID)	
)

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE)
VALUES ('LabelNewCategoryComment', 'Comment:', 'Коментар:', 'Kommentar:')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE)
VALUES ('LinkButtonCategoryComments', 'Comments', 'Коментари', 'Kommentare')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE)
VALUES ('Says', 'says:', 'казва:', 'sagt:')

UPDATE tbLanguage SET ControlTextBG = 'Ключови думи: (разделени със запетая)', ControlTextEN = 'Keywords: (comma-separated)', ControlTextDE = 'Schlüsselwörter: (durch Komma getrennt)' WHERE ControlID = 'LabelCategoryKeywords'