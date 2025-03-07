-- DETAILS TABLES
ALTER TABLE dbo.tbDetailsTable01 ADD SupplierID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable01 ADD FOREIGN KEY (SupplierID) REFERENCES tbVendors(VendorID)

ALTER TABLE dbo.tbDetailsTable02 ADD SupplierID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable02 ADD FOREIGN KEY (SupplierID) REFERENCES tbVendors(VendorID)

ALTER TABLE dbo.tbDetailsTable03 ADD SupplierID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable03 ADD FOREIGN KEY (SupplierID) REFERENCES tbVendors(VendorID)

ALTER TABLE dbo.tbDetailsTable04 ADD SupplierID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable04 ADD FOREIGN KEY (SupplierID) REFERENCES tbVendors(VendorID)

ALTER TABLE dbo.tbDetailsTable05 ADD SupplierID INT NOT NULL DEFAULT 1
ALTER TABLE dbo.tbDetailsTable05 ADD FOREIGN KEY (SupplierID) REFERENCES tbVendors(VendorID)

INSERT INTO dbo.tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) VALUES
('PickASupplierTitle', 'Pick a supplier','Изберете доставчик за това пазаруване','Bitte Anbieter wahlen')