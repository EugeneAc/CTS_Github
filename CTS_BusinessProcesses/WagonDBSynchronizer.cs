using CTS_Models;
using CTS_Models.WagonDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using CTS_Models.DBContext;
using System.Data.Entity.Migrations;
using System.Configuration;

namespace CTS_Core
{
	public static class WagonDBSynchronizer
	{
		private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

		public static bool SyncVesWagon()
		{
			string stringForLoggerAccepted = "";
			string stringForLoggerRejected = "";
			string stringForLoggerWrong = "";
			//int lastID;
			List<WagonScale> wagonScales;
			List<Item> items;
            Int32.TryParse(ConfigurationManager.AppSettings["WagonDBSyncDepth_Days"] ,out int syncDepth);
            var dueDate = syncDepth == 0 ? default(DateTime) : DateTime.Now.AddDays(syncDepth * -1);

            try
			{
				using (CtsDbContext centralDB = new CtsDbContext())
				{
					//var ltr = centralDB.WagonTransfers.Where(x => !x.ID.StartsWith("W")).OrderByDescending(t => t.TransferTimeStamp).Take(1000).Select(m => m.ID).ToArray();
					//lastID = (ltr.Length != 0) ? ltr.Select(x => int.Parse(x)).OrderByDescending(m => m).FirstOrDefault() : 0;
					wagonScales = centralDB.WagonScales.Include(m => m.Location).ToList();
					items = centralDB.Items.Include(m => m.Location).ToList();
				}

				var transfers = new List<ves_vagon>();
				using (var wagDB = new WagonDBcontext())
				{
					//transfers = wagDB.ves_vagon.Where(x => x.id > lastID).Include(m => m.scales).Include(h => h.napravlenie)
					//														.Include(n => n.otpravl).Include(k => k.poluch).ToList();

					transfers = wagDB.ves_vagon.Where(x => x.id_operator != null)
												.Where(x => x.id_operator != 0)
												.Where(x => x.sync != 1)
                                                .Where(x => x.date_time_brutto >= dueDate)
												.Include(m => m.scales)
												.Include(h => h.napravlenie)
												.Include(n => n.otpravl).Include(k => k.poluch).ToList();
				}

				if (transfers.Count > 0)
				{
					var acceptedTransfers = new List<WagonTransfer>();

					using (var centralDB = new CtsDbContext())
					{
						using (var transaction = centralDB.Database.BeginTransaction())
						{
							foreach (var trans in transfers)
							{
								try
								{
                                    var scale = GetCTSWagonScale(trans.scales.name, wagonScales);
									var item = items.Where(x => x.Name == trans.gruz).FirstOrDefault();

									var transfer = new WagonTransfer()
									{
										ID = trans.id.ToString(),
										TransferTimeStamp = trans.date_time_brutto,
										LasEditDateTime = DateTime.Now,
										OperatorName = "DBSync",
										LotName = trans.id_sostav.ToString() ?? "",
										SublotName = trans.vagon_num ?? "",
										OrderNumber = trans.nakladn ?? "",
										FromDestID = trans.otpravl.name ?? "",
										ToDest = trans.poluch.display_name ?? "",
										Tare = (float)trans.ves_tara / 1000,
										Brutto = (float)trans.ves_brutto / 1000,
										Netto = (float)trans.ves_netto / 1000,
										NettoByOrder = (float)trans.ves_netto_docs / 1000,
										EquipID = (scale != null) ? scale.ID : 1,
										ItemID = (item != null) ? item.ID : 1,
										Direction = trans.napravlenie.display_name ?? "",
										IsValid = true,
										Status = 0,
									};

									var vc = new ValidationContext(transfer, null, null);
									var vResults = new List<ValidationResult>();
									var isValid = Validator.TryValidateObject(transfer, vc, vResults, true);
									if (isValid && (transfer.SublotName != ""))
									{
										acceptedTransfers.Add(transfer);
										centralDB.WagonTransfers.AddOrUpdate(transfer);
										stringForLoggerAccepted = String.Concat(stringForLoggerAccepted, trans.id, ";");
									}
									else
									{
										stringForLoggerRejected = String.Concat(stringForLoggerRejected, trans.id, ";");
									}
								}
								catch (Exception)
								{
									stringForLoggerWrong = String.Concat(stringForLoggerWrong, trans.id, ";");
								}

							}

							centralDB.SaveChanges();
							transaction.Commit();
						}
					}

					using (var wagDB = new WagonDBcontext())
					{
						foreach (var t in acceptedTransfers)
						{
							var originalTransfer = wagDB.ves_vagon.Find(Int32.Parse(t.ID));
						    if (originalTransfer != null)
						    {
						        originalTransfer.sync = 1;
						        wagDB.Entry(originalTransfer).State = EntityState.Modified;
						    }
						}

						using (var transaction = wagDB.Database.BeginTransaction())
						{
							wagDB.SaveChanges();
							transaction.Commit();
						}
					}
				}

				_logger.Trace(string.Format("Successfully Synchronized VesWagon: {0}", stringForLoggerAccepted));
				_logger.Trace(string.Format("Successfully Synchronized VesWagon, not accepted transfers: {0}", stringForLoggerRejected));
				_logger.Trace(string.Format("Successfully Synchronized VesWagon, transfers with wrong properties: {0}", stringForLoggerWrong));

				return true;
			}
			catch (Exception ex)
			{
				_logger.Error("Unsuccess with SyncVesWagon");
				_logger.Error(ex.Message.ToString());

				return false;
			}
		}

		public static bool SyncWagonNums()
		{
			var recogns = new List<Recogn>();
			var nums = new List<WagonNumsCache>();

			try
			{
				using (CtsDbContext centralDB = new CtsDbContext())
				{
					recogns.AddRange(centralDB.Recogn.ToList());
				}

				using (var wagDB = new WagonDBcontext())
				{
					foreach (var r in recogns)
					{
						nums.AddRange(wagDB.vagon_nums.Where(x => x.recognid == r.ID).OrderByDescending(x => x.date_time).Take(50)
							.Select(x => new WagonNumsCache
							{
								ID = x.id,
								Date_time = x.date_time,
								RecognID = x.recognid,
								Id_sostav = x.id_sostav,
								Number = x.number,
								Number_operator = x.number_operator,
								Id_operator = x.id_operator,
								Camera = x.camera,
							}).ToList());
					}
				}

				if (nums.Any())
				{
					using (CtsDbContext centralDB = new CtsDbContext())
					{
						using (var transaction = centralDB.Database.BeginTransaction())
						{
							centralDB.WagonNumsCache.RemoveRange(centralDB.WagonNumsCache);
							centralDB.WagonNumsCache.AddRange(nums);
							centralDB.SaveChanges();
							transaction.Commit();
						}
					}
				}

				_logger.Trace("Successfully Synchronized WagonNums");
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error("Unsuccess with SyncWagonNums");
				_logger.Error(ex.Message.ToString());

				return false;
			}
		}

        private static WagonScale GetCTSWagonScale(string wagonDbScaleName, IEnumerable<WagonScale> ctsWagonScales)
        {
            switch (wagonDbScaleName.ToLower())
            {
                case "cofv_r1":
                    return ctsWagonScales.FirstOrDefault(s => s.ID == 17); // ID=17 => ЮГ1
                case "cofv_r2":
                    return ctsWagonScales.FirstOrDefault(s => s.ID == 18); // ID=18 => ЮГ1
                case "cofv_r3":
                    return ctsWagonScales.FirstOrDefault(s => s.ID == 11); // ID=18 => Север
                default:
                    return ctsWagonScales.FirstOrDefault(x => x.LocationID == wagonDbScaleName);
            }
        }
    }


}
