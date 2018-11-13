using CTS_Core;
using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;


namespace DBSyncService
{
	partial class DBSync : ServiceBase
	{
		private Timer SyncTimer = null;
		private Timer WeightTimer = null;
		private Timer WarehouseTimer = null;
		private int syncTimerInterval = 60000;
		private int weightTimerInterval = 60000;

		public DBSync()
		{
			InitializeComponent();
			this.CanStop = true;
			this.CanPauseAndContinue = true;
			this.AutoLog = true;
		}

		protected override void OnStart(string[] args)
		{
			ConfigSection config = System.Configuration.ConfigurationManager.GetSection("serviceParameters")
					   as DBSyncService.ConfigSection;

			if (config != null)
			{
				foreach (ConfigInstanceElement e in config.Instances)
				{
					if (e.Name == "timerDBSynce")
						syncTimerInterval = System.Int32.Parse(e.Code) * 60000;
					if (e.Name == "timerSkipWeight")
						weightTimerInterval = System.Int32.Parse(e.Code) * 60000;
				}
			}

			SyncTimer = new Timer();
			SyncTimer.Interval = syncTimerInterval;
			SyncTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.SyncTimer_tick);
			SyncTimer.Enabled = true;

			WeightTimer = new Timer();
			WeightTimer.Interval = weightTimerInterval;
			WeightTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.WeightTimer_tick);
			WeightTimer.Enabled = true;
		
			WarehouseTimer = new Timer();
			WarehouseTimer.Interval = CalculateTimerInterval();
			WarehouseTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.WarehouseTimer_tick);
			WarehouseTimer.Enabled = true;

			Logger.MakeLog(string.Format("Service started. Timer interval DB Synchronization = {0} minutes, Skip weight change = {1} minutes, WarehouseTimer will raise in {2} minutes.", (syncTimerInterval / 60000).ToString(), (weightTimerInterval / 60000).ToString(), (WarehouseTimer.Interval / 60000).ToString()));
		}

		protected override void OnStop()
		{
			SyncTimer.Enabled = false;
			Logger.MakeLog("Service Stopped");
		}

		protected override void OnPause()
		{
			SyncTimer.Enabled = false;
			Logger.MakeLog("Service Paused");
		}

		protected override void OnContinue()
		{
			SyncTimer.Enabled = true;
			Logger.MakeLog("Service Continued");
		}

		private void SyncTimer_tick(object sender, ElapsedEventArgs e)
		{
			SyncTimer.Enabled = false;

			Task VesWagonTask = Task.Factory.StartNew(() => WagonDBSynchronizer.SyncVesWagon());
			Task SyncWagonNumsTask = Task.Factory.StartNew(() => WagonDBSynchronizer.SyncWagonNums());
			Task CtsSyncTask = Task.Factory.StartNew(() => CtsDbSynchronizer.SyncOperation());
			Task.WaitAll(VesWagonTask, SyncWagonNumsTask, CtsSyncTask);

			SyncTimer.Enabled = true;
		}

		private void WeightTimer_tick(object sender, ElapsedEventArgs e)
		{
			WeightTimer.Enabled = false;

			Task WeightsTask = Task.Factory.StartNew(() => SkipWeightsHandler.ManageWeights());
			WeightsTask.Wait();

			WeightTimer.Enabled = true;
		}

		private void WarehouseTimer_tick(object sender, ElapsedEventArgs e)
		{
			WarehouseTimer.Enabled = false;

			WarehouseHandler.CalculateWarehouseTransferAutomatic();

			WarehouseTimer.Interval = CalculateTimerInterval();
			WarehouseTimer.Enabled = true;
		}

		private double CalculateTimerInterval()
		{
			var nextTime = new DateTime(DateTime.Now.AddHours(1).Year, DateTime.Now.AddHours(1).Month, DateTime.Now.AddHours(1).Day, DateTime.Now.AddHours(1).Hour, 10, 0);

			return nextTime.Subtract(DateTime.Now).TotalMilliseconds;
		}
	}
}