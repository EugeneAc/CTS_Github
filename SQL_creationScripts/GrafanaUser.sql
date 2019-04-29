USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [Grafana]    Script Date: 4/29/2019 10:24:33 PM ******/
CREATE LOGIN [Grafana] WITH PASSWORD=N't91dn5ALY0g1/GES5VjVfIsewgrU9pX2uphGKlDPkZc=', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [Grafana] DISABLE
GO

