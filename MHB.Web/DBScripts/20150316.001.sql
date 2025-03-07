INSERT INTO tbMeasureTypes VALUES (0, 'not set', 'default measure type', 0)
ALTER TABLE tbProducts ADD IsFixedMeasureType BIT NOT NULL DEFAULT 0
ALTER TABLE tbProducts ADD DefaultMeasureType INT NOT NULL DEFAULT 0 FOREIGN KEY REFERENCES tbMeasureTypes(ID)