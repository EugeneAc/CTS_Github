using CTS_Manual_Input.Models.ApproveModels;
using CTS_Models;
using CTS_Models.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CTS_Core;
using System.Security.Principal;

namespace CTS_Manual_Input.Helpers
{
	public static class Approver
	{
		public static TransfersToApprove<TTransfer> GetTransfersToApprove<TTransfer, TEquip>(CtsDbContext cdb, IIdentity user)
												where TTransfer : class, ITransfer
												where TEquip : class, IEquip
		{
			var equips = EquipmentProvider.GetUserAuthorizedEquipment<TEquip>(cdb, user);
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

		public static bool ChangeTransfersStatus<TTransfer>(string transfers, bool isApproved, IIdentity user)
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
					t.ApprovedBy = user.Name;
					dbcontext.Entry(t).State = EntityState.Modified;
				}
				dbcontext.SaveChanges();
				transaction.Commit();
			}

			WarehouseHandler.CalculateWarehouseTransferAfterApprove(editedTransfers);

			return true;
		}
	}
}