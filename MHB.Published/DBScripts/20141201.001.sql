UPDATE tbUsers SET currency = 'USD' WHERE currency = '$'
UPDATE tbUsers SET currency = 'BGN' WHERE currency IN ('lv', 'lv.', 'lv', 'лв.', ' лв.', 'лев', 'BGL', 'лв', 'leva', 'lev', 'лева')
UPDATE tbUsers SET currency = 'EUR' WHERE currency IN ('евро', '€', 'euro')
UPDATE tbUsers SET currency = 'GBP' WHERE currency IN ('ster')
UPDATE tbUsers SET currency = 'USD' WHERE currency = ''

EXEC sp_rename 'tbUsers.password', 'Password', 'COLUMN';
EXEC sp_rename 'tbUsers.userID', 'UserID', 'COLUMN';
EXEC sp_rename 'tbUsers.email', 'Email', 'COLUMN';
EXEC sp_rename 'tbUsers.currency', 'Currency', 'COLUMN';
EXEC sp_rename 'tbUsers.language', 'Language', 'COLUMN';
EXEC sp_rename 'tbUsers.hassetlang', 'HasSetLang', 'COLUMN';
EXEC sp_rename 'tbUsers.registrationdate', 'RegistrationDate', 'COLUMN';
EXEC sp_rename 'tbUsers.attachmentsize', 'AttachmentSize', 'COLUMN';
EXEC sp_rename 'tbUsers.lastlogintime', 'LastLoginTime', 'COLUMN';
EXEC sp_rename 'tbUsers.lastipaddress', 'LastIPAddress', 'COLUMN';
EXEC sp_rename 'tbUsers.useragent', 'UserAgent', 'COLUMN';

ALTER TABLE tbUsers ADD AutoLoginStartTime TIME NOT NULL DEFAULT '00:00'
ALTER TABLE tbUsers ADD AutoLoginEndTime TIME NOT NULL DEFAULT '00:00'
ALTER TABLE tbUsers ADD AutoLoginIsAllowed BIT NOT NULL DEFAULT 0
ALTER TABLE tbUsers ADD AutoLoginHomeAddress NVARCHAR(100) NOT NULL DEFAULT ''

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('CurrencySetSuccessfully', 'Currency set successfully to {0}.', 'Избраната валута беше променена на {0}.', 'Die Währung wurde erfolgreich an {0} geändert')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('AutoLoginSettingsSetSuccessfully', 'Auto-login settings were saved successfully', 'Настройките за автоматичен достъп бяха запаметени успешно.', 'Auto-Login Einstellungen wurden erfolgreich gespeichert')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('PasswordChangedSuccessfully', 'Your password has been changed successfully.', 'Паролата ви беше променена успешно.', 'Ihr Passwort wurde erfolgreich geändert.')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('UserLanguageSetSuccessfully', 'Your preferred language has been set successfully', 'Езикът беше променен успешно.', 'Ihre bevorzugte Sprache wurde erfolgreich gesetzt')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('LabelBoxDateTimePickerAutoAccessTimeLimitsStart', 'Start time:', 'Начално време:', 'Startzeit:')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('LabelBoxDateTimePickerAutoAccessTimeLimitsEnd', 'End time:', 'Крайно време:', 'Endzeit:')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('ButtonSaveAutoLoginSettings', 'Set auto-login', 'Запази автоматичен достъп', 'Speichern Auto-Login')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('OldPasswordDoesNotMatch', 'Your current password does not match the one you have just entered.', 'Паролата, която въведохте, не съвпада с текущата Ви парола.', 'Ihr aktuelles Passwort entspricht nicht der, die Sie gerade eingegeben haben.')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('LabelRestrictAutoLoginTimeFrame', 'Auto-login configuration', 'Натройка автоматично влизане в системата', 'Auto-login Einstellungen')ю

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('CheckBoxAutoLoginEnabled', 'Turn on automatic logon?', 'Разреши автоматично влизане?', 'Automatische Anmeldung einschalten?')

INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) 
VALUES ('LabelSelectCurrency', 'Select preferred currency', 'Избери валута', 'Wählen Sie die bevorzugte Währung')