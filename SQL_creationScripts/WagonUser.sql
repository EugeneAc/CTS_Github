USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [WagonUser]    Script Date: 4/29/2019 10:24:46 PM ******/
CREATE LOGIN [WagonUser] WITH PASSWORD=N'JnBMnK4GYpVCtCk+UlayvdhFDEeNtW8VWYoE6rqq/jA=', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [WagonUser] DISABLE
GO

