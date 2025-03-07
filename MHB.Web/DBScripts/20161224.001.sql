INSERT INTO tbUsers (Password, UserID, Email, Currency, Language, HasSetLang, RegistrationDate, AttachmentSize, LastLoginTime, LastIPAddress, UserAgent, AutoLoginStartTime, AutoLoginEndTime, AutoLoginIsAllowed, AutoLoginHomeAddress)
VALUES
(CAST('Passw0rd!' AS varbinary), 0, 'support@myhomebills.info', 'BGN', 0, 1, GETDATE(), 5000000, GETDATE(), '127.0.0.1', '', '00:00:00', '00:00:00', 0, '127.0.0.1')
