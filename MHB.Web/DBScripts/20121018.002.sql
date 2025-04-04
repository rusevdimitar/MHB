
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-10-18
-- Description:	Generates and returns a new API Key for a given user ID
-- =============================================
CREATE PROCEDURE GenerateAPIKey
	-- Add the parameters for the stored procedure here
	@UserID INT,
	@IsAdmin BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @APIKey VARCHAR(32)

	SET @APIKey = (SELECT APIKey FROM dbo.tbAPIUsers WHERE UserID = @UserID)

	IF @APIKey IS NULL
	BEGIN
		SET @APIKey = REPLACE(NEWID(), '-', SPACE(0))
		INSERT INTO dbo.tbAPIUsers (UserID, APIKey, IsAdmin, DateGenerated) VALUES (@UserID, @APIKey, @IsAdmin, GETDATE())
	END
	ELSE 
	BEGIN
		SET @APIKey = REPLACE(NEWID(), '-', SPACE(0))
		UPDATE dbo.tbAPIUsers SET APIKey = @APIKey, IsAdmin = @IsAdmin, DateGenerated = GETDATE() WHERE UserID = @UserID
	END
	
	SELECT @APIKey	
END
GO
