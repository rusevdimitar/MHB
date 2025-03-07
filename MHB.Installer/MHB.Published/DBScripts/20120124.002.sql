
/****** Object:  StoredProcedure [dbo].[spAddUserGeoLocationInfo]    Script Date: 01/24/2012 14:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-01-24
-- Description:	Inserts Geo Location Data for a given user
-- =============================================

CREATE PROCEDURE [dbo].[spAddUserGeoLocationInfo]
	@Ip VARCHAR(40),
	@UserID INT,
	@CountryCode VARCHAR(5),
	@CountryName VARCHAR(200),
	@RegionCode NVARCHAR(200),
	@RegionName NVARCHAR(200),
	@City NVARCHAR(200),
	@ZipCode NVARCHAR(200),
	@Latitude FLOAT,
	@Longitude FLOAT,
AS
BEGIN
	DECLARE @interval INT
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO tbUsersGeoLocationData (Ip, UserID, CountryCode, CountryName, RegionCode, RegionName, City, ZipCode, Latitude, Longitude, LoginDateTime) 
	VALUES (@Ip, @UserID, @CountryCode, @CountryName, @RegionCode, @RegionName, @City, @ZipCode, @Latitude, @Longitude, GETDATE())
	
END

