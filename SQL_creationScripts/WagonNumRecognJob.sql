USE [msdb]
GO

/****** Object:  Job [WagonNumRecogn]    Script Date: 4/29/2019 10:26:27 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 4/29/2019 10:26:27 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'WagonNumRecogn', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'WagonUser', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [insert to vagon_nums from tblWagon]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'insert to vagon_nums from tblWagon', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'insert into sync_wagons_recogn (idwagon) Select top(1000)  idWagon from tblWagons where bordNumHand is null order by idWagon desc
go
insert into vagon_nums (date_time, id_recogn, number, img, img2, camera)
	Select timeW, idSource, bordNum, img2, img4, MainCam from tblWagons where idWagon in (select idwagon from sync_wagons_recogn)
go
update tblWagons set bordNumHand=1 where idWagon in (select idWagon from sync_wagons_recogn)
go 
delete from sync_wagons_recogn
go
DELETE  FROM vagon_nums  WHERE id NOT IN (SELECT MIN(b.id) FROM vagon_nums b GROUP BY b.date_time, b.id_recogn, b.number)
go
', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [update ves_vagon]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'update ves_vagon', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'

DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @IDR INT


DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @IDs INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, id_scales from ves_vagon where id_scales is not null and id_vagon_nums is null and date_time_brutto>=DATEADD(DAY,DATEDIFF(day,0,CURRENT_TIMESTAMP)-1,0) order by id desc


/*Открываем курсор*/
OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @IDs

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR
	SELECT top(1) Id, date_time, number, id_recogn FROM vagon_nums where  id_recogn=@IDs and (date_time between DateAdd(ss, -2, convert(datetime2 , @dtv,105)) 
	  and DateAdd(ss, 2, convert(datetime2 , @dtv,105))) 
   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @dt, @num, @idr

   if @@FETCH_STATUS = 0
	update ves_vagon set id_operator=1, id_vagon_nums=@ID, 
		vagon_num=@NUM
	  WHERE id=@IDv 
			    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @IDs
END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [alarm not recognition]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'alarm not recognition', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
/* not recognition */
DECLARE @ID INT	  
DECLARE @dt datetime
 

	  /*Объявляем курсор*/
DECLARE @notrecogn CURSOR

/*Заполняем курсор*/
SET @notrecogn = CURSOR SCROLL
FOR
select date_time_brutto, id  from ves_vagon where (vagon_num like ''%x%'' or vagon_num like ''%*%'' ) and date_time_brutto>=DATEADD(DAY,DATEDIFF(day,0,CURRENT_TIMESTAMP)-1,0)  

/*Открываем курсор*/
OPEN @notrecogn

/*Выбираем первую строку*/
FETCH NEXT FROM @notrecogn INTO @dt, @ID

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
IF NOT EXISTS(select id from alarm where ves_vagon_id=@ID and alarmtype=3) 
	INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id])
		VALUES
           (DateAdd(ss, 2, convert(datetime2 , @dt,105))
           ,3
           ,@ID)
/*Выбираем следующую строку*/
FETCH NEXT FROM @notrecogn INTO @dt, @ID

END
CLOSE @notrecogn
', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [alarm over weight]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'alarm over weight', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/* over weight*/

DECLARE @ID INT	  
DECLARE @dt datetime

	  /*Объявляем курсор*/
DECLARE @notrecogn CURSOR

/*Заполняем курсор*/
SET @notrecogn = CURSOR SCROLL
FOR
select date_time_brutto, id from ves_vagon 
	where ves_brutto>120000 and date_time_brutto>=DATEADD(DAY,DATEDIFF(day,0,CURRENT_TIMESTAMP)-2,0) 
	order by id desc

/*Открываем курсор*/
OPEN @notrecogn

/*Выбираем первую строку*/
FETCH NEXT FROM @notrecogn INTO  @dt, @ID

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
IF NOT EXISTS(select id from alarm where ves_vagon_id=@ID and alarmtype=1)
		INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id])
		VALUES
           (DateAdd(ss, 0, convert(datetime , @dt,105))
           ,1
           ,@ID)
/*Выбираем следующую строку*/
FETCH NEXT FROM @notrecogn INTO  @dt, @ID
END
CLOSE @notrecogn

', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [wagon control Kuzembaeva - Saburkhan]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'wagon control Kuzembaeva - Saburkhan', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

/* 18 vagon_nums id_recogn - 5 id_scale */

select id, date_time, number 
  from vagon_nums
  where date_time < DateAdd(ss, -7200, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
	  and date_time > DateAdd(ss, -14400, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
  and id_recogn=18
  order by date_time desc


OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

select top(1) id
  from vagon_nums 
  where date_time < DateAdd(ss, -1, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
	  and date_time > DateAdd(ss, -14400, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105)) 
  and id_recogn=5
  and number=@num
 order by date_time desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID 
    
  if @@FETCH_STATUS <> 0
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=5)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
            )
	 	VALUES
           (@dtv
           ,5
           ,@IDv
            )
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [wagon control  Kostenko - diametr]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'wagon control  Kostenko - diametr', 
		@step_id=6, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/* 6 - 26 - 20 
Костенко Выезд	10.21.208.6 - 23
Костенко ст. Драмтеатр	10.21.208.26 - 4 - Весы
Костенко ст. Драмтеатр	10.21.208.20 - 22 
*/


DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time, number 
  from vagon_nums
  where date_time < DateAdd(ss, -7200, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
	  and date_time > DateAdd(ss, -14400, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
  and id_recogn=23
  order by date_time desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

select top(1) id
  from vagon_nums 
  where date_time < DateAdd(ss, -1, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
	  and date_time > DateAdd(ss, -14400, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105)) 
  and id_recogn=4
  and number=@num
 order by date_time desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID 
    
  if @@FETCH_STATUS <> 0
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=5)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
            )
	 	VALUES
           (@dtv
           ,5
           ,@IDv
            )
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [wagon control  Kazakhstanskaya-Kazakhstanskaya]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'wagon control  Kazakhstanskaya-Kazakhstanskaya', 
		@step_id=7, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/*
wagon control  Kostenko - diametr

 24 - 3  
*/


DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time, number 
  from vagon_nums
  where date_time < DateAdd(ss, -7200, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
	  and date_time > DateAdd(ss, -14400, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
  and id_recogn=24
  order by date_time desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

select top(1) id
  from vagon_nums 
  where date_time < DateAdd(ss, -1, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105))
	  and date_time > DateAdd(ss, -14400, convert(datetime2 , (select top(1) date_time from vagon_nums order by date_time desc),105)) 
  and id_recogn=3
  and number=@num
 order by date_time desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID 
    
  if @@FETCH_STATUS <> 0
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=5)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
            )
	 	VALUES
           (@dtv
           ,5
           ,@IDv
            )
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num

END
CLOSE @record_num
', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check kuz-cof_r3]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check kuz-cof_r3', 
		@step_id=8, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=28
   and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
 order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR
  select top(1)id, ves_brutto
  from ves_vagon 
  where date_time_brutto < DateAdd(ss, -3601, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
	  and date_time_brutto > DateAdd(ss, -260000, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
    and vagon_num=@num2
  and id_operator=1  
  and id_scales=5
 and (select top(1) id_scales from ves_vagon where date_time_brutto <= DateAdd(ss, -60, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))<>28
  order by date_time_brutto desc
   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check kost-cof_r3]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check kost-cof_r3', 
		@step_id=9, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=28
    and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
 order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

  select top(1)id, ves_brutto
  from ves_vagon 
  where date_time_brutto < DateAdd(ss, -3601, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
	  and date_time_brutto > DateAdd(ss, -260000, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
    and vagon_num=@num2
  and id_operator=1
and id_scales=4
  and (select top(1) id_scales from ves_vagon where date_time_brutto <= DateAdd(ss, -60, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))<>28
 order by date_time_brutto desc
   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check shah-cof_r3]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check shah-cof_r3', 
		@step_id=10, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=28
  and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
  order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

   select top(1)id, ves_brutto
  from ves_vagon 
  where date_time_brutto < DateAdd(ss, -3601, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
	  and date_time_brutto > DateAdd(ss, -260000, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
    and vagon_num=@num2
  and id_operator=1
    and id_scales=12
and (select top(1) id_scales from ves_vagon where date_time_brutto <= DateAdd(ss, -60, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))<>28
   order by date_time_brutto desc
   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check len-cof_r3]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check len-cof_r3', 
		@step_id=11, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=28
  and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
  order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

    select top(1)id, ves_brutto
  from ves_vagon 
  where date_time_brutto < DateAdd(ss, -3601, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
	  and date_time_brutto > DateAdd(ss, -260000, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
    and vagon_num=@num2
  and id_operator=1    
and id_scales=6
 and (select top(1) id_scales from ves_vagon where date_time_brutto <= DateAdd(ss, -60, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))<>28
  order by date_time_brutto desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check sar3-cof_r3]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check sar3-cof_r3', 
		@step_id=12, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=28
  and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
  order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

  select top(1)id, ves_brutto
  from ves_vagon 
  where date_time_brutto < DateAdd(ss, -3601, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
	  and date_time_brutto > DateAdd(ss, -260000, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
    and vagon_num=@num2
  and id_operator=1
    and id_scales=10
 and (select top(1) id_scales from ves_vagon where date_time_brutto <= DateAdd(ss, -60, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))<>28
  order by date_time_brutto desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check sar1-cof_r3]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check sar1-cof_r3', 
		@step_id=13, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=28
  and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
  order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

  select top(1)id, ves_brutto
  from ves_vagon 
  where date_time_brutto < DateAdd(ss, -3601, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
	  and date_time_brutto > DateAdd(ss, -260000, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
    and vagon_num=@num2
  and id_operator=1
    and id_scales=9 
  and (select top(1) id_scales from ves_vagon where date_time_brutto <= DateAdd(ss, -60, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))<>28
 order by date_time_brutto desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check sar-cof_r3]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check sar-cof_r3', 
		@step_id=14, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=28
  and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
  order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

  select top(1)id, ves_brutto
  from ves_vagon 
  where date_time_brutto < DateAdd(ss, -3601, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
	  and date_time_brutto > DateAdd(ss, -260000, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
    and vagon_num=@num2
  and id_operator=1
    and id_scales=8 
and (select top(1) id_scales from ves_vagon where date_time_brutto <= DateAdd(ss, -60, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))<>28
   order by date_time_brutto desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check kaz-cof_r3]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check kaz-cof_r3', 
		@step_id=15, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=28
  and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
  order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

   select top(1)id, ves_brutto
  from ves_vagon 
  where date_time_brutto < DateAdd(ss, -3601, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
	  and date_time_brutto > DateAdd(ss, -260000, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105))
    and vagon_num=@num2
  and id_operator=1
    and id_scales=3
 and (select top(1) id_scales from ves_vagon where date_time_brutto <= DateAdd(ss, -60, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))<>28
  order by date_time_brutto desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      
    	    
	CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num


', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [2 point check tent-kaz]    Script Date: 4/29/2019 10:26:28 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'2 point check tent-kaz', 
		@step_id=16, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @ID INT	  
DECLARE @dt datetime2
DECLARE @NUM varchar (8)
DECLARE @vesb int
DECLARE @IDR INT

DECLARE @IDv INT	  
DECLARE @dtv datetime2
DECLARE @NUM2 varchar (8)
DECLARE @vesb2 int
DECLARE @IDn INT
	  
	  /*Объявляем курсор*/
DECLARE @record_num CURSOR

/*Заполняем курсор*/
SET @record_num  = CURSOR SCROLL
FOR

select id, date_time_brutto, vagon_num, id_vagon_nums, ves_brutto 
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -3600, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)) 
	  and DateAdd(ss, 0, convert(datetime2 , (select top(1) date_time_brutto from ves_vagon order by date_time_brutto desc),105)))
    and id_scales=14
  and (vagon_num<>'''' and vagon_num<>''0'' and vagon_num not like ''%x%'' and vagon_num not like ''%*%'')
  and id_operator=1
  order by date_time_brutto desc

OPEN @record_num

/*Выбираем первую строку*/
FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

/*Выполняем в цикле перебор строк*/
while  @@FETCH_STATUS = 0
BEGIN
/*Вставляем параметры в третью таблицу если условие соблюдается*/
   
   	/*Объявляем курсор*/
	DECLARE @sel_num CURSOR
	
	/*Заполняем курсор*/
	SET @sel_num  = CURSOR SCROLL
	FOR

  select id, ves_brutto
  from ves_vagon 
  where (date_time_brutto between DateAdd(ss, -21600, convert(datetime2 , @dtv,105)) 
	      and DateAdd(ss, -600, convert(datetime2 , @dtv,105)))
    and vagon_num=@NUM2
  and id_operator=1
    and id_scales=3
  order by id desc

   /*Открываем курсор*/
	OPEN @sel_num
   /*Выбираем первую строку*/
	FETCH NEXT FROM @sel_num INTO @ID, @vesb 
    
  if @@FETCH_STATUS = 0
   if (cast(@vesb*1.0/@vesb2*1.0 as real)<0.98) or (CAST(@vesb*1.0/@vesb2*1.0 as real)>1.02) 
    begin
    IF NOT EXISTS(select id from alarm where ves_vagon_id=@IDv and alarmtype=4)
	 	  INSERT INTO alarm
           ([datetime]
           ,[alarmtype]
           ,[ves_vagon_id]
           ,[ves_vagon_id2]
            )
	 	VALUES
           (@dtv
           ,4
           ,@IDv
           ,@ID
            )
      end    	    
CLOSE @sel_num  		
	
/*Выбираем следующую строку*/
  FETCH NEXT FROM @record_num INTO @IDv, @dtv, @num2, @IDn, @vesb2

END
CLOSE @record_num', 
		@database_name=N'WagonDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'3_minutes', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=3, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20181109, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'377c5eec-21c0-49a3-8132-a506c2dc252c'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

