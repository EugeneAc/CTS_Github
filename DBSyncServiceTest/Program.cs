using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DBSyncServiceTest
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				//CTS_BusinessProcesses.CtsDbSynchronizer.SyncOperation();
				//CTS_BusinessProcesses.WagonDBSynchronizer.SyncVesWagon();
				//CTS_BusinessProcesses.WagonDBSynchronizer.SyncWagonNums();
				//CTS_BusinessProcesses.SkipWeightsHandler.ManageWeights();
				//CTS_BusinessProcesses.WarehouseHandler.CalculateWarehouseTransferAutomatic();
				CTS_BusinessProcesses.CtsEmailSender.SendMailTest();
			}
		}
	}
}
