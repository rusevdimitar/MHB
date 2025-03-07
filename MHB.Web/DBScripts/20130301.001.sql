--ALTER TABLE tbProducts DROP FK_VENDID
--ALTER TABLE tbProducts DROP FK_PRODCATID

--ALTER TABLE tbProducts DROP COLUMN VendorID
--ALTER TABLE tbProducts DROP COLUMN CategoryID

--DROP TABLE tbVendors

CREATE TABLE dbo.tbVendors
(
	VendorID INT NOT NULL PRIMARY KEY,
	AccountNumber NVARCHAR(15) NOT NULL	DEFAULT '',	
	Name NVARCHAR(50) NOT NULL DEFAULT '',
	[Description] NVARCHAR(4000) NOT NULL DEFAULT '',
	[Address] NVARCHAR(1024) NOT NULL DEFAULT '',
	CreditRating TINYINT NOT NULL DEFAULT 0,
	PreferredVendorStatus BIT NOT NULL DEFAULT 0,
	ActiveFlag BIT NOT NULL DEFAULT 0,
	PurchasingWebServiceURL NVARCHAR(1024) NULL,
	WebsiteURL NVARCHAR(1024) NULL,
	UserID INT NOT NULL FOREIGN KEY (UserID) REFERENCES tbUsers(userID),
	ModifiedDate DATETIME NOT NULL,
	IsDeleted BIT NOT NULL DEFAULT 0
)

ALTER TABLE tbProducts
ADD VendorID INT NULL

ALTER TABLE tbProducts
ADD CategoryID INT NULL

ALTER TABLE tbProducts
ADD CONSTRAINT FK_VENDID FOREIGN KEY (VendorID) REFERENCES tbVendors(VendorID)

ALTER TABLE tbProducts
ADD CONSTRAINT FK_PRODCATID FOREIGN KEY (CategoryID) REFERENCES tbCategories(ID)

INSERT INTO tbVendors (VendorID, AccountNumber, Name, [Description], [Address], CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, WebsiteURL, UserID, ModifiedDate, IsDeleted)
VALUES (1, '000000000000001', 'Kaufland', 'Кауфланд България', 'улица „Македония“ 97 Пловдив 032 503 500', 0, 0, 1, 'http://www.kaufland.bg', 'http://www.kaufland.bg', 1, GETDATE(), 0)

INSERT INTO tbVendors (VendorID, AccountNumber, Name, [Description], [Address], CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, WebsiteURL, UserID, ModifiedDate, IsDeleted)
VALUES (2, '000000000000002', 'Billa', 'Била България', '4000 Пловдив, Пловдивска Ул. Райко Даскалов 8 Пон-Пет.:07:30-21:30 Съб: 08:00-21:30 Нед: 08:30-21:00 GPS: 42.14884,24.74797', 0, 0, 1, 'http://http://www.billa.bg', 'http://http://www.billa.bg', 1, GETDATE(), 0)

INSERT INTO tbVendors (VendorID, AccountNumber, Name, [Description], [Address], CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, WebsiteURL, UserID, ModifiedDate, IsDeleted)
VALUES (3, '000000000000003', 'Piccadilly', 'Пикадили България', 'Пловдив 4000, бул. Менделеев 2Б Работно време: Всеки ден от 08:00 до 22:00 ч. Тел.: 032/ 273 429 Факс: 032/273 428 e-mail: info.rp@plovdiv.piccadilly.bg', 0, 0, 1, 'http://www.piccadilly.bg', 'http://www.piccadilly.bg', 1, GETDATE(), 0)

INSERT INTO tbVendors (VendorID, AccountNumber, Name, [Description], [Address], CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, WebsiteURL, UserID, ModifiedDate, IsDeleted)
VALUES (4, '000000000000003', 'Leksi', 'Лекси България', 'Tелефони: + 032 398001 бул. Марица 19 + 032 397450/51 ул.К.Райчо 56 + 032 397480/81 бул.Пещерско шосе 78 + 032 399934 ул.Царевец 14 FAX + 359 032 398000 Адрес за кореспонденция 4000 Пловдив, ул.Богомил 58 e-mail: leksibg@abv.bg ', 0, 0, 1, 'http://leksi.chepelare-bg.net/', 'http://leksi.chepelare-bg.net/', 1, GETDATE(), 0)