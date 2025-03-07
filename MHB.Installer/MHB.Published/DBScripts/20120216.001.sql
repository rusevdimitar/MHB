DROP TABLE SortOptionsTranslations
DROP TABLE SortOptions


CREATE TABLE dbo.SortOptions
(
	ID INT PRIMARY KEY,
	Name NVARCHAR(30),	
	[Enabled] BIT NOT NULL DEFAULT 1
)

CREATE TABLE dbo.SortOptionsTranslations
(
	ID INT PRIMARY KEY IDENTITY (1,1),
	SortOptionID INT,
	LanguageID INT,
	Name NVARCHAR(30)
)

ALTER TABLE dbo.SortOptionsTranslations

ADD CONSTRAINT FK_SORTOPTIONTRANSL_SORTOPTION_ID FOREIGN KEY ( SortOptionID ) REFERENCES dbo.SortOptions(ID)

INSERT INTO dbo.SortOptions (ID, Name, [Enabled]) VALUES (0, 'Price', 1)
INSERT INTO dbo.SortOptions (ID, Name, [Enabled]) VALUES (1, 'Date', 1)
INSERT INTO dbo.SortOptions (ID, Name, [Enabled]) VALUES (2, 'Category', 1)

INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (0, 0, 'Цена')
INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (0, 1, 'Price')
INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (0, 2, 'Preis')

INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (1, 0, 'Дата')
INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (1, 1, 'Date')
INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (1, 2, 'Datum')

INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (2, 0, 'Категория')
INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (2, 1, 'Category')
INSERT INTO dbo.SortOptionsTranslations (SortOptionID, LanguageID, Name) VALUES (2, 2, 'Kategorie')


INSERT INTO [dbo].[tbLanguage]
           ([ControlID]
           ,[ControlTextEN]
           ,[ControlTextBG]
           ,[ControlTextDE])
     VALUES
           ('LabelSortBy'
           ,'Sort By:'
           ,'Сортирай по:'
           ,'Sortieren nach:')

