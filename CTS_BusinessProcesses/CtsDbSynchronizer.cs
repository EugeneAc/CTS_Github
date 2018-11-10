using CTS_Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using CTS_Models.DBContext;
using System.Threading.Tasks;

namespace CTS_BusinessProcesses
{
	public static class CtsDbSynchronizer
	{
		private static bool SyncFromLocalToCental<TTransfer, TEquip>(ConnectionStringSettings connectionStringSettings)
			where TTransfer : class, ITransfer
			where TEquip : class, IEquip
		{
			List<TTransfer> localTransferList = new List<TTransfer>();
			string stringForLogger = "";
			int[] scalesArray;
			var scalesTime = new Dictionary<int, DateTime>();

			try
			{
				using (CtsEquipContext<TEquip> centralDB = new CtsEquipContext<TEquip>("centralDBConnection"))
				{
					scalesArray = centralDB.DbSet.Where(x => x.LocationID == connectionStringSettings.Name.ToString()).Select(m => m.ID).ToArray();
				}

				using (CtsTransferContext<TTransfer> centralDB = new CtsTransferContext<TTransfer>("centralDBConnection"))
				{
					foreach (var scale in scalesArray)
					{
						DateTime lastTime = new DateTime(2018, 01, 01);
						var transfer = centralDB.DbSet.Where(x => x.EquipID == scale).OrderByDescending(t => t.TransferTimeStamp).FirstOrDefault();
						if (transfer != null)
						{
							lastTime = transfer.TransferTimeStamp;
						}
						scalesTime.Add(scale, lastTime);
					}

				}

				using (CtsTransferContext<TTransfer> localDB = new CtsTransferContext<TTransfer>(connectionStringSettings.ConnectionString))
				{
					foreach (var scale in scalesTime)
					{
						localTransferList.AddRange(localDB.DbSet.Where(s => s.EquipID == scale.Key)
											.Where(x => x.OperatorName == "System Platform").Where(t => t.TransferTimeStamp > scale.Value));
					}
				}

				if (localTransferList.Count != 0)
				{
					using (CtsTransferContext<TTransfer> centralDB = new CtsTransferContext<TTransfer>("centralDBConnection"))
					{
						using (var transaction = centralDB.Database.BeginTransaction())
						{
							foreach (var t in localTransferList)
							{
								TTransfer transfer = t;
								transfer.LasEditDateTime = System.DateTime.Now;
								centralDB.DbSet.Add(transfer);
								stringForLogger = String.Concat(stringForLogger, t.ID, ";");
							}

							centralDB.SaveChanges();
							transaction.Commit();
						}
					}
				}

				Logger.MakeLog(string.Format("{0} Successfully synchronized FromLocalToCentral {1}: {2}", connectionStringSettings.Name.ToString(), typeof(TTransfer).ToString(), stringForLogger));
				return true;
			}
			catch (Exception ex)
			{
				Logger.MakeLog(string.Format("{0} Unsuccess with FromLocalToCentral {1}", connectionStringSettings.Name.ToString(), typeof(TTransfer).ToString()));
				Logger.MakeLog(ex.ToString());

				return false;
			}
		}

		private static bool SyncFromCentralToLocal<TTransfer, TEquip>(ConnectionStringSettings connectionStringSettings)
		  where TTransfer : class, ITransfer
		  where TEquip : class, IEquip
		{
			List<TTransfer> centralTransferList = new List<TTransfer>();
			string stringForLogger = "";
			int[] scalesArray;

			try
			{
				using (CtsEquipContext<TEquip> centralDB = new CtsEquipContext<TEquip>("centralDBConnection"))
				{
					scalesArray = centralDB.DbSet.Where(x => x.LocationID == connectionStringSettings.Name.ToString()).Select(m => m.ID).ToArray();
				}

				using (CtsTransferContext<TTransfer> centralDB = new CtsTransferContext<TTransfer>("centralDBConnection"))
				{
					centralTransferList.AddRange(centralDB.DbSet.Where(x => scalesArray.Contains((int)x.EquipID))
																.Where(n => n.OperatorName != "System Platform")
																.Where(x => x.TransferTimeStamp > DbFunctions.AddDays(System.DateTime.Now, -2)));
				}

				if (centralTransferList.Count != 0)
				{
					using (CtsTransferContext<TTransfer> localDB = new CtsTransferContext<TTransfer>(connectionStringSettings.ConnectionString))
					{
						using (var transaction = localDB.Database.BeginTransaction())
						{
							foreach (var t in centralTransferList)
							{
								if (t is IHaveAnalysis transfer)
								{
									transfer.AnalysisID = null;
								}
								localDB.DbSet.AddOrUpdate(t as TTransfer);
								stringForLogger = String.Concat(stringForLogger, t.ID, ";");
							}

							localDB.SaveChanges();
							transaction.Commit();
						}
					}
				}

				Logger.MakeLog(string.Format("{0} Successfully synchronized FromCentralToLocal {1}: {2}", connectionStringSettings.Name.ToString(), typeof(TTransfer).ToString(), stringForLogger));
				return true;
			}
			catch (Exception ex)
			{
				Logger.MakeLog(string.Format("{0} Unsuccess with FromCentralToLocal {1}", connectionStringSettings.Name.ToString(), typeof(TTransfer).ToString()));
				Logger.MakeLog(ex.ToString());

				return false;
			}
		}

		private static bool SyncFromCentralToLocalAndDeleteWarehouseTransfers(ConnectionStringSettings connectionStringSettings)
		{
			List<WarehouseTransfer> centralTransferList = new List<WarehouseTransfer>();
			string stringForLogger = "";

			try
			{
				using (CtsDbContext centralDB = new CtsDbContext())
				{
					centralTransferList.AddRange(centralDB.WarehouseTransfers.Where(x => x.Warehouse.LocationID == connectionStringSettings.Name.ToString())
																.Where(x => x.TransferTimeStamp > DbFunctions.AddDays(System.DateTime.Today, -2)));
				}

				if (centralTransferList.Count != 0)
				{
					using (CtsDbContext localDB = new CtsDbContext(connectionStringSettings.ConnectionString))
					{
						using (var transaction = localDB.Database.BeginTransaction())
						{
							foreach (var t in centralTransferList)
							{
								localDB.WarehouseTransfers.AddOrUpdate(t);
								stringForLogger = String.Concat(stringForLogger, t.ID, ";");
							}

							List<WarehouseTransfer> localTransferList = new List<WarehouseTransfer>();
							localTransferList.AddRange(localDB.WarehouseTransfers.Where(x => x.TransferTimeStamp <= DbFunctions.AddDays(System.DateTime.Today, -3)));
							localDB.WarehouseTransfers.RemoveRange(localTransferList);

							localDB.SaveChanges();
							transaction.Commit();
						}
					}
				}

				Logger.MakeLog(string.Format("{0} Successfully synchronized SyncFromCentralToLocalAndDeleteWarehouseTransfers: {1}", connectionStringSettings.Name.ToString(), stringForLogger));
				return true;
			}
			catch (Exception ex)
			{
				Logger.MakeLog(string.Format("{0} Unsuccess with SyncFromCentralToLocalAndDeleteWarehouseTransfers", connectionStringSettings.Name.ToString()));
				Logger.MakeLog(ex.ToString());

				return false;
			}
		}

		private static bool SyncFromCentralToLocalAndDeleteWagonNumsCache(ConnectionStringSettings connectionStringSettings)
		{
			var nums = new List<WagonNumsCache>();

			try
			{
				using (CtsDbContext centralDB = new CtsDbContext())
				{
					nums.AddRange(centralDB.WagonNumsCache.Where(x => x.Recogn.LocationID == connectionStringSettings.Name.ToString()).ToList());
				}

				if (nums.Any())
				{
					using (CtsDbContext localDB = new CtsDbContext(connectionStringSettings.ConnectionString))
					{
						using (var transaction = localDB.Database.BeginTransaction())
						{
							localDB.WagonNumsCache.RemoveRange(localDB.WagonNumsCache);
							localDB.WagonNumsCache.AddRange(nums);
							localDB.SaveChanges();
							transaction.Commit();
						}
					}
				}

				Logger.MakeLog(string.Format("{0} Successfully synchronized SyncFromCentralToLocalAndDeleteWagonNumsCache", connectionStringSettings.Name.ToString()));
				return true;
			}
			catch (Exception ex)
			{
				Logger.MakeLog(string.Format("{0} Unsuccess with SyncFromCentralToLocalAndDeleteWagonNumsCache", connectionStringSettings.Name.ToString()));
				Logger.MakeLog(ex.ToString());

				return false;
			}
		}

		private static bool SyncDictionaries<T>(ConnectionStringSettings connectionStringSettings) where T : class
		{
			List<T> items = new List<T>();

			try
			{
				using (CtsUniversalContext<T> centralDB = new CtsUniversalContext<T>("centralDBConnection"))
				{
					items.AddRange(centralDB.DbSet.ToList());
				}

				if (items.Count != 0)
				{
					using (CtsUniversalContext<T> localDB = new CtsUniversalContext<T>(connectionStringSettings.ConnectionString))
					{
						using (var transaction = localDB.Database.BeginTransaction())
						{
							foreach (var item in items)
							{
								localDB.DbSet.AddOrUpdate(item);
							}

							localDB.SaveChanges();
							transaction.Commit();
						}
					}
				}

				Logger.MakeLog(string.Format("{0} Successfully synchronized Dictionary {1}", connectionStringSettings.Name.ToString(), typeof(T).ToString()));
				return true;
			}

			catch (Exception ex)
			{
				Logger.MakeLog(string.Format("{0} Unsuccess with SyncDictionary {1}", connectionStringSettings.Name.ToString(), typeof(T).ToString()));
				Logger.MakeLog(ex.ToString());

				return false;
			}
		}

		private static bool RemoveOldLocal<T>(ConnectionStringSettings connectionStringSettings) where T : class, ITransfer
		{
			List<T> localTransferList = new List<T>();
			string stringForLogger = "";

			try
			{
				using (CtsTransferContext<T> localDB = new CtsTransferContext<T>(connectionStringSettings.ConnectionString))
				{
					localTransferList.AddRange(localDB.DbSet.Where(x => x.TransferTimeStamp <= DbFunctions.AddDays(System.DateTime.Now, -3)));
					localDB.DbSet.RemoveRange(localTransferList);
					localDB.SaveChanges();
					foreach (var t in localTransferList)
					{
						stringForLogger = String.Concat(stringForLogger, t.ID, ";");
					}
				}

				Logger.MakeLog(string.Format("{0} Successfully RemovedOldLocal {1}: {2}", connectionStringSettings.Name.ToString(), typeof(T).ToString(), stringForLogger));
				return true;
			}

			catch (Exception ex)
			{
				Logger.MakeLog(string.Format("{0} Unsuccess with RemoveOldLocal {1}", connectionStringSettings.Name.ToString(), typeof(T).ToString()));
				Logger.MakeLog(ex.ToString());

				return false;
			}
		}

		public static void SyncOperation()
		{
			var SyncTasks = new List<Task>();

			foreach (ConnectionStringSettings connection in System.Configuration.ConfigurationManager.ConnectionStrings)
			{
				if ((connection.Name != "CentralDBConnection") && (connection.Name != "LocalDBConnection")
					&& (connection.Name != "LocalSqlServer") && (connection.Name != "WagonDB"))
				{
					SyncTasks.Add(Task.Factory.StartNew(() =>
				   {
					   SyncDictionaries<Location>(connection);
					   SyncDictionaries<Item>(connection);
					   SyncDictionaries<InnerDestination>(connection);
					   SyncDictionaries<WagonScale>(connection);
					   SyncDictionaries<VehiScale>(connection);
					   SyncDictionaries<BeltScale>(connection);
					   SyncDictionaries<Skip>(connection);
					   SyncDictionaries<RockUtil>(connection);
					   SyncDictionaries<Warehouse>(connection);
					   SyncDictionaries<Recogn>(connection);
					   SyncDictionaries<BoilerConsNorm>(connection);

					   //if (SyncFromLocalToCental<WagonTransfer, WagonScale>(connection))
					   //{
					   // RemoveOldLocal<WagonTransfer>(connection);
					   //}
					   SyncFromCentralToLocal<WagonTransfer, WagonScale>(connection);

					   //if (SyncFromLocalToCental<VehiTransfer, VehiScale>(connection))
					   //{
					   // RemoveOldLocal<VehiTransfer>(connection);
					   //}
					   //SyncFromCentralToLocal<VehiTransfer, VehiScale>(connection);

					   if (SyncFromLocalToCental<BeltTransfer, BeltScale>(connection))
					   {
						   RemoveOldLocal<BeltTransfer>(connection);
					   }
					   SyncFromCentralToLocal<BeltTransfer, BeltScale>(connection);

					   if (SyncFromLocalToCental<SkipTransfer, Skip>(connection))
					   {
						   RemoveOldLocal<SkipTransfer>(connection);
					   }
					   SyncFromCentralToLocal<SkipTransfer, Skip>(connection);

					   SyncFromCentralToLocal<RockUtilTransfer, RockUtil>(connection);

					   SyncFromCentralToLocalAndDeleteWarehouseTransfers(connection);

					   SyncFromCentralToLocalAndDeleteWagonNumsCache(connection);
				   }));
				}
			}

			Task.WaitAll(SyncTasks.ToArray());
		}
	}
}



