USE [master]
GO

/****** Object:  LinkedServer [ORCLDB]    Script Date: 4/29/2019 10:02:20 PM ******/
EXEC master.dbo.sp_addlinkedserver @server = N'ORCLDB', @srvproduct=N'OraOLEDB.Oracle', @provider=N'OraOLEDB.Oracle', @datasrc=N'139.53.6.7:1521/pay'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'ORCLDB',@useself=N'False',@locallogin=NULL,@rmtuser=N'ISS_CTS',@rmtpassword='########'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'collation compatible', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'rpc out', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'ORCLDB', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO

