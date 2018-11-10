using CTS_Models.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS_BusinessProcesses
{
	public static class SkipWeightsHandler
	{
		public static void ManageWeights ()
		{
			var stringForLogger = new StringBuilder();

			try
			{
				CtsDbContext centralDB = new CtsDbContext();
				var skips = centralDB.Skips.ToList();

				foreach (var s in skips)
				{
					var skipWeight = centralDB.SkipWeights.Where(x => x.SkipID == s.ID)
															.Where(d => d.ValidFrom <= System.DateTime.Now).OrderByDescending(m => m.ValidFrom).FirstOrDefault();
					if (skipWeight != null)
					{
						if (s.Weight != skipWeight.Weight)
						{
							s.Weight = skipWeight.Weight;
							centralDB.Entry(s).State = EntityState.Modified;
							stringForLogger.Append("SkipID ");
							stringForLogger.Append(s.ID);
							stringForLogger.Append(" = ");
							stringForLogger.Append(s.Weight);
							stringForLogger.Append("; ");
						}
					}
				}

				centralDB.SaveChanges();
				stringForLogger.Insert(0, "Weight changed for ");
				Logger.MakeLog(stringForLogger.ToString());
			}
			catch(Exception ex)
			{
				Logger.MakeLog("Unsuccess with SkipWeightHandler");
				Logger.MakeLog(ex.ToString());
			}
		}
	}
}
