using CTS_Manual_Input.Models.ApproveModels;
using CTS_Models;
using CTS_Models.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CTS_BusinessProcesses;

namespace CTS_Manual_Input.Helpers
{
	public static class Approver
	{
		public static TransfersToApprove<TTransfer> GetTransfersToApprove<TTransfer, TEquip>(CtsDbContext cdb, string username)
												where TTransfer : class, ITransfer
												where TEquip : class, IEquip
		{
			var equips = EquipmentProvider.GetUserAuthorizedEquipment<TEquip>(cdb, username);
			var equipsArray = equips.Select(x => x.ID).ToArray();
			var dbcontext = new CtsTransferContext<TTransfer>();
			var transfers = dbcontext.DbSet.Where(t => equipsArray.Contains((int)t.EquipID))
				.Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2)).ToList();

			var transfersToApprove = new TransfersToApprove<TTransfer>();
			transfersToApprove.CreatedTransfers = transfers.Where(m => m.Status == 1).ToList();
			transfersToApprove.EditedTransfers = transfers.Where(m => m.Status == 2).ToList();
			var obsoleteArray = transfersToApprove.EditedTransfers.Select(x => x.InheritedFrom).ToArray();
			transfersToApprove.ObsoleteTransfers = dbcontext.DbSet.Where(m => obsoleteArray.Contains(m.ID)).ToList();
			transfersToApprove.DeletedTransfers = transfers.Where(m => m.Status == 4).ToList();

			return transfersToApprove;
		}

		public static bool ChangeTransfersStatus<TTransfer>(string transfers, bool isApproved, string username)
														where TTransfer : class, ITransfer
		{
			string[] transfersArray = transfers.Split(',');
			var dbcontext = new CtsTransferContext<TTransfer>();
			var editedTransfers = dbcontext.DbSet.Where(x => transfersArray.Contains(x.ID)).ToList();
			var obsoleteArray = editedTransfers.Where(x => x.Status == 2).Select(m => m.InheritedFrom).ToArray();
			editedTransfers.AddRange(dbcontext.DbSet.Where(x => obsoleteArray.Contains(x.ID)).ToList());
			using (var transaction = dbcontext.Database.BeginTransaction())
			{
				foreach (var t in editedTransfers)
				{
					// What the magic is happening with t.Status? 
					// Refer to ApproveStatus Dictionary in ProjectConstants.cs and you'll get it :)
					if (isApproved)
					{
						t.IsValid = !t.IsValid;
						t.Status += 8;
					}
					else
					{
						t.Status += 4;
					}
					t.ApprovedBy = username;
					dbcontext.Entry(t).State = EntityState.Modified;
				}
				dbcontext.SaveChanges();
				transaction.Commit();
			}

			WarehouseHandler.CalculateWarehouseTransferAfterApprove(editedTransfers);

			return true;
		}

		//public static AnalysisToApprove<T> GetAnalysisToApprove<T>(CtsDbContext cdb, string username)
		//								where T : class, IAnalysis
		//{
		//	var dbcontext = new CtsAnalysisContext<T>();
		//	var analysis = dbcontext.DbSet.Where(d => d.AnalysisTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2)).ToList();
		//	var analysisToApprove = new AnalysisToApprove<T>();
		//	analysisToApprove.CreatedAnalysis = analysis.Where(m => m.Status == 1).ToList();
		//	analysisToApprove.EditedAnalysis = analysis.Where(m => m.Status == 2).ToList();
		//	var obsoleteArray = analysisToApprove.EditedAnalysis.Select(x => x.InheritedFrom).ToArray();
		//	analysisToApprove.ObsoleteAnalysis = dbcontext.DbSet.Where(m => obsoleteArray.Contains(m.ID)).ToList();
		//	analysisToApprove.DeletedAnalysis = analysis.Where(m => m.Status == 4).ToList();

		//	return analysisToApprove;
		//}

		//public static bool ChangeAnalysisStatus<T>(string analysis, bool isApproved, string username)
		//												where T : class, IAnalysis
		//{
		//	int[] analysisArray = Array.ConvertAll(analysis.Split(','), item => Int32.Parse(item)); 
		//	var dbcontext = new CtsAnalysisContext<T>(); 
		//	var editedAnalysis = dbcontext.DbSet.Where(x => analysisArray.Contains(x.ID)).ToList();
		//	var obsoleteArray = editedAnalysis.Select(m => m.InheritedFrom).ToArray();
		//	editedAnalysis.AddRange(dbcontext.DbSet.Where(x => obsoleteArray.Contains(x.ID)).ToList());
		//	using (var transaction = dbcontext.Database.BeginTransaction())
		//	{
		//		foreach (var t in editedAnalysis)
		//		{
		//			// What the magic is happening with t.Status? 
		//			// Refer to ApproveStatus Dictionary in ProjectConstants.cs and you'll get it :)
		//			if (isApproved)
		//			{
		//				t.IsValid = !t.IsValid;
		//				t.Status += 8;
		//			}
		//			else
		//			{
		//				t.Status += 4;
		//			}
		//			t.ApprovedBy = username;
		//			dbcontext.Entry(t).State = EntityState.Modified;
		//		}
		//		dbcontext.SaveChanges();
		//		transaction.Commit();
		//	}

		//	return true;
		//}
	}
}