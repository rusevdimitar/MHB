ALTER TABLE tbProducts DROP CONSTRAINT FK_VENDID
ALTER TABLE tbVendors DROP CONSTRAINT PK__tbVendor__FC8618D3753864A1
ALTER TABLE tbVendors DROP COLUMN VendorID
ALTER TABLE tbVendors ADD VendorID INT IDENTITY(1,1) PRIMARY KEY

UPDATE tbLanguage SET ControlTextEN = 'ERP Entities', ControlTextBG = 'ERP обекти', ControlTextDE = 'ERP Entities' 
WHERE ControlID = 'ButtonProductsManagement'

INSERT INTO dbo.tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) VALUES
('Products', 'Products','Продукти','Produkte')

INSERT INTO dbo.tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) VALUES
('Suppliers', 'Suppliers','Доставчици','Lieferanten')

INSERT INTO dbo.tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) VALUES
('Categories', 'Categories','Категории','Kategorien')

INSERT INTO dbo.tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) VALUES
('ButtonAddNewSupplier', 'Add new supplier','Добави доставчик','Lieferant hinzufügen')