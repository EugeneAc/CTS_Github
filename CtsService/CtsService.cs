using CTS_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CtsService
{
	public partial class CtsService : ServiceBase
	{
		private Timer _weightTimer = null;
		private Timer _warehouseTimer = null;
		private int _weightTimerInterval = 60000;
		private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

		public CtsService()
		{
			InitializeComponent();
			this.CanStop = true;
			this.CanPauseAndContinue = true;
			this.AutoLog = true;
		}

		protected override void OnStart(string[] args)
		{
			int.TryParse(ConfigurationManager.AppSettings["timerSkipWeight"], out _weightTimerInterval);

			_weightTimer = new Timer {Interval = _weightTimerInterval};
			_weightTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.WeightTimer_tick);
			_weightTimer.Enabled = true;

			_warehouseTimer = new Timer {Interval = CalculateTimerInterval()};
			_warehouseTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.WarehouseTimer_tick);
			_warehouseTimer.Enabled = true;

			_logger.Trace(string.Format("Service started. Skip weight change interval = {0} minutes, WarehouseTimer will raise in {1} minutes.", (_weightTimerInterval / 60000).ToString(), (_warehouseTimer.Interval / 60000).ToString()));
		}

		protected override void OnStop()
		{
			_weightTimer.Enabled = false;
			_warehouseTimer.Enabled = false;
			_logger.Trace("Service Stopped");
		}

		protected override void OnPause()
		{
			_weightTimer.Enabled = false;
			_warehouseTimer.Enabled = false;
			_logger.Trace("Service Paused");
		}

		protected override void OnContinue()
		{
			_weightTimer.Enabled = true;
			_warehouseTimer.Enabled = true;
			_logger.Trace("Service Continued");
		}

		private void WeightTimer_tick(object sender, ElapsedEventArgs e)
		{
			_weightTimer.Enabled = false;

			SkipWeightsHandler.ManageWeights();

			_weightTimer.Enabled = true;
		}

		private void WarehouseTimer_tick(object sender, ElapsedEventArgs e)
		{
			_warehouseTimer.Enabled = false;

			WarehouseHandler.CalculateWarehouseTransferAutomatic();

			_warehouseTimer.Interval = CalculateTimerInterval();
			_warehouseTimer.Enabled = true;
		}

		private double CalculateTimerInterval()
		{
			var nextTime = new DateTime(DateTime.Now.AddHours(1).Year, DateTime.Now.AddHours(1).Month, DateTime.Now.AddHours(1).Day, DateTime.Now.AddHours(1).Hour, 10, 0);

			return nextTime.Subtract(DateTime.Now).TotalMilliseconds;
		}
	}
}
