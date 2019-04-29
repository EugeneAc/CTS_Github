USE [msdb]
GO

/****** Object:  Job [OracleSync]    Script Date: 4/29/2019 10:25:44 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [Data Collector]    Script Date: 4/29/2019 10:25:44 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'Data Collector' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'Data Collector'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'OracleSync', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'Data Collector', 
		@owner_login_name=N'MST-UGTRKC12\Administrator', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [GetMiningPlan]    Script Date: 4/29/2019 10:25:44 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'GetMiningPlan', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=3, 
		@retry_interval=10, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'delete from CoalTracking.dbo.MiningPlan


insert into CoalTracking.dbo.MiningPlan ([Date], [Plan], [ShopID])


SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''01'', 112) AS ''Date'', sum(isnull([DAY1], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''01'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''02'', 112) AS  ''Date'', sum(isnull([DAY2], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''02'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''03'', 112) AS  ''Date'', sum(isnull([DAY3], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''03'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''04'', 112) AS  ''Date'', sum(isnull([DAY4], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''04'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''05'', 112) AS  ''Date'', sum(isnull([DAY5], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''05'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''06'', 112) AS  ''Date'', sum(isnull([DAY6], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''06'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''07'', 112) AS  ''Date'', sum(isnull([DAY7], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''07'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''08'', 112) AS  ''Date'', sum(isnull([DAY8], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''08'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''09'', 112) AS  ''Date'', sum(isnull([DAY9], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''09'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''10'', 112) AS  ''Date'', sum(isnull([DAY10], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''10'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''11'', 112) AS  ''Date'', sum(isnull([DAY11], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''11'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''12'', 112) AS  ''Date'', sum(isnull([DAY12], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''12'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''13'', 112) AS  ''Date'', sum(isnull([DAY13], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''13'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''14'', 112) AS  ''Date'', sum(isnull([DAY14], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''14'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''15'', 112) AS  ''Date'', sum(isnull([DAY15], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''15'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''16'', 112) AS  ''Date'', sum(isnull([DAY16], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''16'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''17'', 112) AS  ''Date'', sum(isnull([DAY17], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''17'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''18'', 112) AS  ''Date'', sum(isnull([DAY18], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''18'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''19'', 112) AS  ''Date'', sum(isnull([DAY19], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''19'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''20'', 112) AS  ''Date'', sum(isnull([DAY20], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''20'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''21'', 112) AS  ''Date'', sum(isnull([DAY21], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''21'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''22'', 112) AS  ''Date'', sum(isnull([DAY22], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''22'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''23'', 112) AS  ''Date'', sum(isnull([DAY23], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''23'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''24'', 112) AS  ''Date'', sum(isnull([DAY24], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''24'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''25'', 112) AS  ''Date'', sum(isnull([DAY25], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''25'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''26'', 112) AS  ''Date'', sum(isnull([DAY26], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''26'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''27'', 112) AS  ''Date'', sum(isnull([DAY27], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''27'', 112), shop_id
UNION
SELECT        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''28'', 112) AS  ''Date'', sum(isnull([DAY28], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
GROUP BY TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''28'', 112), shop_id
UNION
SELECT        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''29'', 112) AS  ''Date'', sum(isnull([DAY29], 0)) AS ''Plan'', shop_ID AS''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
WHERE        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''29'', 112) IS NOT NULL
GROUP BY TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''29'', 112), shop_id
UNION
SELECT        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''30'', 112) AS  ''Date'', sum(isnull([DAY30], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
WHERE        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''30'', 112) IS NOT NULL
GROUP BY TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''30'', 112), shop_id
UNION
SELECT        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''31'', 112) AS ''Date'', sum(isnull([DAY31], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN]
WHERE        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''31'', 112) IS NOT NULL
GROUP BY TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''31'', 112), shop_id


update CoalTracking.dbo.MiningPlan set
    LocationID = 
	(
	case when ShopID = 7003 then ''kost''
	     when ShopID = 7004 then ''kuz'' 
		 when ShopID = 7006 then ''sar'' 
		 when ShopID = 7010 then ''abay'' 
		 when ShopID = 7011 then ''kaz'' 
		 when ShopID = 7012 then ''len'' 
		 when ShopID = 7014 then ''shah'' 
		 when ShopID = 7015 then ''tent'' 
		 when ShopID = 7025 then ''cofv'' 

	end)
', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [GetMiningplanB]    Script Date: 4/29/2019 10:25:44 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'GetMiningplanB', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=3, 
		@retry_interval=10, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'delete from CoalTracking.dbo.MiningPlanB


insert into CoalTracking.dbo.MiningPlanB ([Date], [Plan], [ShopID])


SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''01'', 112) AS ''Date'', sum(isnull([DAY1], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''01'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''02'', 112) AS  ''Date'', sum(isnull([DAY2], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''02'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''03'', 112) AS  ''Date'', sum(isnull([DAY3], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''03'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''04'', 112) AS  ''Date'', sum(isnull([DAY4], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''04'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''05'', 112) AS  ''Date'', sum(isnull([DAY5], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''05'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''06'', 112) AS  ''Date'', sum(isnull([DAY6], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''06'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''07'', 112) AS  ''Date'', sum(isnull([DAY7], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''07'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''08'', 112) AS  ''Date'', sum(isnull([DAY8], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''08'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''09'', 112) AS  ''Date'', sum(isnull([DAY9], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''09'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''10'', 112) AS  ''Date'', sum(isnull([DAY10], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''10'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''11'', 112) AS  ''Date'', sum(isnull([DAY11], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''11'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''12'', 112) AS  ''Date'', sum(isnull([DAY12], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''12'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''13'', 112) AS  ''Date'', sum(isnull([DAY13], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''13'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''14'', 112) AS  ''Date'', sum(isnull([DAY14], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''14'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''15'', 112) AS  ''Date'', sum(isnull([DAY15], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''15'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''16'', 112) AS  ''Date'', sum(isnull([DAY16], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''16'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''17'', 112) AS  ''Date'', sum(isnull([DAY17], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''17'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''18'', 112) AS  ''Date'', sum(isnull([DAY18], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''18'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''19'', 112) AS  ''Date'', sum(isnull([DAY19], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''19'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''20'', 112) AS  ''Date'', sum(isnull([DAY20], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''20'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''21'', 112) AS  ''Date'', sum(isnull([DAY21], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''21'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''22'', 112) AS  ''Date'', sum(isnull([DAY22], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''22'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''23'', 112) AS  ''Date'', sum(isnull([DAY23], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''23'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''24'', 112) AS  ''Date'', sum(isnull([DAY24], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''24'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''25'', 112) AS  ''Date'', sum(isnull([DAY25], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''25'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''26'', 112) AS  ''Date'', sum(isnull([DAY26], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''26'', 112), shop_id
UNION
SELECT        CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''27'', 112) AS  ''Date'', sum(isnull([DAY27], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''27'', 112), shop_id
UNION
SELECT        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''28'', 112) AS  ''Date'', sum(isnull([DAY28], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
GROUP BY TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''28'', 112), shop_id
UNION
SELECT        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''29'', 112) AS  ''Date'', sum(isnull([DAY29], 0)) AS ''Plan'', shop_ID AS''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
WHERE        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''29'', 112) IS NOT NULL
GROUP BY TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''29'', 112), shop_id
UNION
SELECT        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''30'', 112) AS  ''Date'', sum(isnull([DAY30], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
WHERE        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''30'', 112) IS NOT NULL
GROUP BY TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''30'', 112), shop_id
UNION
SELECT        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''31'', 112) AS ''Date'', sum(isnull([DAY31], 0)) AS ''Plan'', shop_ID AS ''ShopID''
FROM            [ORCLDB]..[ISS].[V_MINING_PLAN_B]
WHERE        TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''31'', 112) IS NOT NULL
GROUP BY TRY_CONVERT(DATETIME, CAST([YEAR_MONTH] AS varchar) + ''31'', 112), shop_id



update CoalTracking.dbo.MiningPlanB set
    LocationID = 
	(
	case when ShopID = 7003 then ''kost''
	     when ShopID = 7004 then ''kuz'' 
		 when ShopID = 7006 then ''sar'' 
		 when ShopID = 7010 then ''abay'' 
		 when ShopID = 7011 then ''kaz'' 
		 when ShopID = 7012 then ''len'' 
		 when ShopID = 7014 then ''shah'' 
		 when ShopID = 7015 then ''tent'' 
		 when ShopID = 7025 then ''cofv'' 

	end)
', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [GetMiningFact]    Script Date: 4/29/2019 10:25:44 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'GetMiningFact', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=3, 
		@retry_interval=10, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'delete from [CoalTracking].[dbo].[MiningFact]
where [Date] > DATEADD(day,-60,getdate())

insert into [CoalTracking].[dbo].[MiningFact] ([Date], [ShopID], [Shift1], [Shift2], [Shift3], [Shift4], [Total])
(select * from [ORCLDB]..[ISS].[V_MINING_FACT] where [Day] > DATEADD(day,-60,getdate()))

update CoalTracking.dbo.MiningFact 
set
    LocationID = 
	(
	case when ShopID = 7003 then ''kost''
	     when ShopID = 7004 then ''kuz'' 
		 when ShopID = 7006 then ''sar'' 
		 when ShopID = 7010 then ''abay'' 
		 when ShopID = 7011 then ''kaz'' 
		 when ShopID = 7012 then ''len'' 
		 when ShopID = 7014 then ''shah'' 
		 when ShopID = 7015 then ''tent'' 
		 when ShopID = 7025 then ''cofv'' 

	end)
where [Date] > DATEADD(day,-60,getdate())', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [GetStaff]    Script Date: 4/29/2019 10:25:44 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'GetStaff', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=3, 
		@retry_interval=10, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'delete from [CoalTracking].[dbo].[Staff]
insert into [CoalTracking].[dbo].[Staff] ([Date], [ShopID], [CNT])


SELECT        [DAY] AS ''Date'', [SHOP_ID] AS ''ShopID'', [CNT] AS ''CNT''
FROM           [ORCLDB]..[ISS].[V_STAFF]

update CoalTracking.dbo.Staff set
    LocationID = 
	(
	case when ShopID = 7003 then ''kost''
	     when ShopID = 7004 then ''kuz'' 
		 when ShopID = 7006 then ''sar'' 
		 when ShopID = 7010 then ''abay'' 
		 when ShopID = 7011 then ''kaz'' 
		 when ShopID = 7012 then ''len'' 
		 when ShopID = 7014 then ''shah'' 
		 when ShopID = 7015 then ''tent'' 
		 when ShopID = 7025 then ''cofv'' 

	end)
	', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'GetPlanSchedule', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20180515, 
		@active_end_date=99991231, 
		@active_start_time=90000, 
		@active_end_time=235959, 
		@schedule_uid=N'b81e3253-ce34-45ba-a9f0-667938bcc9b8'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

