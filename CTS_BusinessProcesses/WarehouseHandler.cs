using CTS_Models;
using CTS_Models.DBContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BusinessProcesses
{
	public static class WarehouseHandler
	{
		static CtsDbContext _centalDB = new CtsDbContext();

		static MineWarehouseLogicModel _kuzEquip = new MineWarehouseLogicModel(plusSkipID: new List<int> { 2, 3 }, minusWagonID: new List<int> { 3 }, minusRockID: new List<int> { 2 }, warehouseID: 2);
		static MineWarehouseLogicModel _kostEquip = new MineWarehouseLogicModel(plusSkipID: new List<int> { 5, 6 }, minusWagonID: new List<int> { 2 }, minusRockID: new List<int> { 3 }, warehouseID: 3);
		static MineWarehouseLogicModel _abayEquip = new MineWarehouseLogicModel(plusSkipID: new List<int> { 17, 18 }, minusWagonID: new List<int> { 10 }, minusRockID: new List<int> { 5 }, warehouseID: 4);
		static MineWarehouseLogicModel _sar1Equip = new MineWarehouseLogicModel(plusSkipID: new List<int> { 7, 8 }, minusWagonID: new List<int> { 4 }, minusRockID: new List<int> { 9 }, warehouseID: 6);
		static MineWarehouseLogicModel _sar3Equip = new MineWarehouseLogicModel(plusSkipID: new List<int> { 10 }, minusWagonID: new List<int> { 5 }, minusRockID: new List<int> { 10 }, warehouseID: 7);
		static MineWarehouseLogicModel _shahEquip = new MineWarehouseLogicModel(plusSkipID: new List<int> { 4, 9 }, minusWagonID: new List<int> { 7 }, minusRockID: new List<int> { 6 }, warehouseID: 5);
		static MineWarehouseLogicModel _tentEquip = new MineWarehouseLogicModel(plusBeltID: new List<int> { 21 }, minusWagonID: new List<int> { 6 }, minusRockID: new List<int> { 8 }, warehouseID: 9);
		static MineWarehouseLogicModel _lenEquip = new MineWarehouseLogicModel(plusSkipID: new List<int> { 13, 14 }, minusWagonID: new List<int> { 8 }, minusRockID: new List<int> { 7 }, warehouseID: 8);

		/// <summary>
		/// Performs calculation of warehouses' balances according to automatic transfers for the previous hour.
		/// </summary>
		static public void CalculateWarehouseTransferAutomatic()
		{
			var skipTransfers = new List<SkipTransfer>();
			var beltTransfers = new List<BeltTransfer>();
			var wagonTransfers = new List<WagonTransfer>();
			var startTime = new DateTime(DateTime.Now.AddHours(-1).Year, DateTime.Now.AddHours(-1).Month, DateTime.Now.AddHours(-1).Day, DateTime.Now.AddHours(-1).Hour, 0, 0);
			var timeStamp = startTime.AddMinutes(59);

			try
			{
				skipTransfers.AddRange(_centalDB.SkipTransfers.Where(x => x.Status == 0).Where(m => m.LasEditDateTime >= startTime).ToList());
				beltTransfers.AddRange(_centalDB.InternalTransfers.Where(x => x.Status == 0).Where(m => m.LasEditDateTime >= startTime).ToList());
				wagonTransfers.AddRange(_centalDB.WagonTransfers.Where(x => x.Status == 0).Where(m => m.LasEditDateTime >= startTime).ToList());
			}
			catch (Exception ex)
			{
				Logger.MakeLog("Unsuccess with CalculateWarehouseTransferAutomatic");
				Logger.MakeLog(ex.ToString());
				return;
			}

			var kuzQuantity = CalculateBalance(_kuzEquip, skipTransfers, beltTransfers, wagonTransfers, null);
			var kostQuantity = CalculateBalance(_kostEquip, skipTransfers, beltTransfers, wagonTransfers, null);
			var abayQuantity = CalculateBalance(_abayEquip, skipTransfers, beltTransfers, wagonTransfers, null);
			var sar1Quantity = CalculateBalance(_sar1Equip, skipTransfers, beltTransfers, wagonTransfers, null);
			var sar3Quantity = CalculateBalance(_sar3Equip, skipTransfers, beltTransfers, wagonTransfers, null);
			var shahQuantity = CalculateBalance(_shahEquip, skipTransfers, beltTransfers, wagonTransfers, null);
			var tentQuantity = CalculateBalance(_tentEquip, skipTransfers, beltTransfers, wagonTransfers, null);
			var lenQuantity = CalculateBalance(_lenEquip, skipTransfers, beltTransfers, wagonTransfers, null);

			var kuzInsertTask = ExecuteStoredProcedure(_kuzEquip.WarehouseID, kuzQuantity, timeStamp);
			var kostInsertTask = ExecuteStoredProcedure(_kostEquip.WarehouseID, kostQuantity, timeStamp);
			var abayInsertTask = ExecuteStoredProcedure(_abayEquip.WarehouseID, abayQuantity, timeStamp);
			var sar1InsertTask = ExecuteStoredProcedure(_sar1Equip.WarehouseID, sar1Quantity, timeStamp);
			var sar3InsertTask = ExecuteStoredProcedure(_sar3Equip.WarehouseID, sar3Quantity, timeStamp);
			var shahInsertTask = ExecuteStoredProcedure(_shahEquip.WarehouseID, shahQuantity, timeStamp);
			var tentInsertTask = ExecuteStoredProcedure(_tentEquip.WarehouseID, tentQuantity, timeStamp);
			var lenInsertTask = ExecuteStoredProcedure(_lenEquip.WarehouseID, lenQuantity, timeStamp);

			Logger.MakeLog(String.Format("Successfully finished CalculateWarehouseTransferAutomatic. kuz: {0}, kost: {1}, abay: {2}, sar1: {3}, sar3: {4}, shah: {5}, tent: {6}, len: {7}",
															kuzInsertTask, kostInsertTask, abayInsertTask, sar1InsertTask, sar3InsertTask, shahInsertTask, tentInsertTask, lenInsertTask));
		}

		static double CalculateBalance(MineWarehouseLogicModel mine, List<SkipTransfer> skipTransfers, List<BeltTransfer> beltTransfers, List<WagonTransfer> wagonTransfers, List<RockUtilTransfer> rockTransfers)
		{
			double total = 0;

			try
			{
				//for automatic, approved added and editted transfers
				if ((mine.PlusSkipID != null) && (skipTransfers != null))
				{
					foreach (var t in skipTransfers.Where(s => (s.Status == 0) || (s.Status == 9) || (s.Status == 10)).Where(x => mine.PlusSkipID.Contains((int)x.EquipID)))
					{
						try
						{
							total += Int32.Parse(t.LiftingID) * t.SkipWeight;
						}
						catch (Exception ex)
						{
							Logger.MakeLog("Unsuccess with CalculateBalance - LiftingID is not a number");
							Logger.MakeLog(ex.ToString());
						}
					}
				}
				total += ((mine.PlusBeltID != null) && (beltTransfers != null)) ?
					(double)beltTransfers.Where(s => (s.Status == 0) || (s.Status == 9) || (s.Status == 10)).Where(x => mine.PlusBeltID.Contains((int)x.EquipID)).Sum(t => t.LotQuantity) : 0;
				total -= ((mine.MinusBeltID != null) && (beltTransfers != null)) ?
					(double)beltTransfers.Where(s => (s.Status == 0) || (s.Status == 9) || (s.Status == 10)).Where(x => mine.MinusBeltID.Contains((int)x.EquipID)).Sum(t => t.LotQuantity) : 0;
				total += ((mine.PlusWagonID != null) && (wagonTransfers != null)) ?
					(double)wagonTransfers.Where(s => (s.Status == 0) || (s.Status == 9) || (s.Status == 10)).Where(x => mine.PlusWagonID.Contains((int)x.EquipID)).Sum(t => t.Netto) : 0;
				total -= ((mine.MinusWagonID != null) && (wagonTransfers != null)) ?
					(double)wagonTransfers.Where(s => (s.Status == 0) || (s.Status == 9) || (s.Status == 10)).Where(x => mine.MinusWagonID.Contains((int)x.EquipID)).Sum(t => t.Netto) : 0;
				total -= ((mine.MinusRockID != null) && (rockTransfers != null)) ?
					(double)rockTransfers.Where(s => (s.Status == 0) || (s.Status == 9) || (s.Status == 10)).Where(x => mine.MinusRockID.Contains((int)x.EquipID)).Sum(t => t.LotQuantity) : 0;

				//for approved obsolete and deleted transfers
				if ((mine.PlusSkipID != null) && (skipTransfers != null))
				{
					foreach (var t in skipTransfers.Where(s => (s.Status == 11) || (s.Status == 12)).Where(x => mine.PlusSkipID.Contains((int)x.EquipID)))
					{
						try
						{
							total -= Int32.Parse(t.LiftingID) * t.SkipWeight;
						}
						catch (Exception ex)
						{
							Logger.MakeLog("Unsuccess with CalculateBalance - LiftingID is not a number");
							Logger.MakeLog(ex.ToString());
						}
					}
				}
				total -= ((mine.PlusBeltID != null) && (beltTransfers != null)) ?
					(double)beltTransfers.Where(s => (s.Status == 11) || (s.Status == 12)).Where(x => mine.PlusBeltID.Contains((int)x.EquipID)).Sum(t => t.LotQuantity) : 0;
				total += ((mine.MinusBeltID != null) && (beltTransfers != null)) ?
					(double)beltTransfers.Where(s => (s.Status == 11) || (s.Status == 12)).Where(x => mine.MinusBeltID.Contains((int)x.EquipID)).Sum(t => t.LotQuantity) : 0;
				total -= ((mine.PlusWagonID != null) && (wagonTransfers != null)) ?
					(double)wagonTransfers.Where(s => (s.Status == 11) || (s.Status == 12)).Where(x => mine.PlusWagonID.Contains((int)x.EquipID)).Sum(t => t.Netto) : 0;
				total += ((mine.MinusWagonID != null) && (wagonTransfers != null)) ?
					(double)wagonTransfers.Where(s => (s.Status == 11) || (s.Status == 12)).Where(x => mine.MinusWagonID.Contains((int)x.EquipID)).Sum(t => t.Netto) : 0;
				total += ((mine.MinusRockID != null) && (rockTransfers != null)) ?
					(double)rockTransfers.Where(s => (s.Status == 11) || (s.Status == 12)).Where(x => mine.MinusRockID.Contains((int)x.EquipID)).Sum(t => t.LotQuantity) : 0;
			}
			catch (Exception ex)
			{
				Logger.MakeLog("Unsuccess with CalculateBalance");
				Logger.MakeLog(ex.ToString());
			}

			return total;
		}

		static string ExecuteStoredProcedure(int WarehouseID, double LotQuantity, DateTime TransferTimeStamp, bool IsAfterApprove = false)
		{
			if ((LotQuantity == 0) && (IsAfterApprove == true))
			{
				return "Nothing inserted";
			}

			try
			{
				var id = new SqlParameter { ParameterName = "Id", Value = "WH" + WarehouseID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds };
				var warehouseID = new SqlParameter { ParameterName = "WarehouseID", Value = WarehouseID };
				var timeStamp = new SqlParameter { ParameterName = "TimeStamp", Value = TransferTimeStamp };
				var isManual = new SqlParameter { ParameterName = "IsManual", Value = 0 };
				var lotQuantity = new SqlParameter { ParameterName = "LotQuantity", Value = LotQuantity };
				var result = new SqlParameter { ParameterName = "Result", Value = 42, Direction = ParameterDirection.Output }; // default value = 42. Why? cause it's the "Answer to the Ultimate Question of Life, the Universe, and Everything"
				var query = _centalDB.Database.SqlQuery<List<int>>("InsertNewWarehouseTransfer" + " @Id, @WarehouseID, @TimeStamp, @IsManual, @LotQuantity, @Result out",
																								id, warehouseID, timeStamp, isManual, lotQuantity, result).ToList();

				switch ((int)result.Value)
				{
					case 1: return "Value inserted";
					case 2: return "Initial transfer";
					default: return "Smth strange";
				}
			}
			catch (Exception ex)
			{
				Logger.MakeLog("Unsuccess with ExecuteStoredProcedure");
				Logger.MakeLog(ex.ToString());

				return "Exception occured";
			}
		}

		/// <summary>
		/// Performs calculation of warehouses' balances after transfers' approving in MI.
		/// </summary>
		public static void CalculateWarehouseTransferAfterApprove<TTransfer>(List<TTransfer> transfers) where TTransfer : class, ITransfer
		{
			var skipTransfers = new List<SkipTransfer>();
			var beltTransfers = new List<BeltTransfer>();
			var wagonTransfers = new List<WagonTransfer>();
			var rockTransfers = new List<RockUtilTransfer>();
			var startTime = new DateTime(DateTime.Now.AddHours(-1).Year, DateTime.Now.AddHours(-1).Month, DateTime.Now.AddHours(-1).Day, DateTime.Now.AddHours(-1).Hour, 0, 0);
			var timeStamp = startTime.AddMinutes(59);

			if (transfers is List<SkipTransfer>)
			{
				skipTransfers.AddRange(transfers as List<SkipTransfer>);
			}
			else if (transfers is List<BeltTransfer>)
			{
				beltTransfers.AddRange(transfers as List<BeltTransfer>);
			}
			else if (transfers is List<WagonTransfer>)
			{
				wagonTransfers.AddRange(transfers as List<WagonTransfer>);
			}
			else if (transfers is List<RockUtilTransfer>)
			{
				rockTransfers.AddRange(transfers as List<RockUtilTransfer>);
			}

			var kuzQuantity = CalculateBalance(_kuzEquip, skipTransfers, beltTransfers, wagonTransfers, rockTransfers);
			var kostQuantity = CalculateBalance(_kostEquip, skipTransfers, beltTransfers, wagonTransfers, rockTransfers);
			var abayQuantity = CalculateBalance(_abayEquip, skipTransfers, beltTransfers, wagonTransfers, rockTransfers);
			var sar1Quantity = CalculateBalance(_sar1Equip, skipTransfers, beltTransfers, wagonTransfers, rockTransfers);
			var sar3Quantity = CalculateBalance(_sar3Equip, skipTransfers, beltTransfers, wagonTransfers, rockTransfers);
			var shahQuantity = CalculateBalance(_shahEquip, skipTransfers, beltTransfers, wagonTransfers, rockTransfers);
			var tentQuantity = CalculateBalance(_tentEquip, skipTransfers, beltTransfers, wagonTransfers, rockTransfers);
			var lenQuantity = CalculateBalance(_lenEquip, skipTransfers, beltTransfers, wagonTransfers, rockTransfers);

			var kuzInsertTask = ExecuteStoredProcedure(_kuzEquip.WarehouseID, kuzQuantity, timeStamp, true);
			var kostInsertTask = ExecuteStoredProcedure(_kostEquip.WarehouseID, kostQuantity, timeStamp, true);
			var abayInsertTask = ExecuteStoredProcedure(_abayEquip.WarehouseID, abayQuantity, timeStamp, true);
			var sar1InsertTask = ExecuteStoredProcedure(_sar1Equip.WarehouseID, sar1Quantity, timeStamp, true);
			var sar3InsertTask = ExecuteStoredProcedure(_sar3Equip.WarehouseID, sar3Quantity, timeStamp, true);
			var shahInsertTask = ExecuteStoredProcedure(_shahEquip.WarehouseID, shahQuantity, timeStamp, true);
			var tentInsertTask = ExecuteStoredProcedure(_tentEquip.WarehouseID, tentQuantity, timeStamp, true);
			var lenInsertTask = ExecuteStoredProcedure(_lenEquip.WarehouseID, lenQuantity, timeStamp, true);

			Logger.MakeLog(String.Format("Successfully finished CalculateWarehouseTransferAfterApprove. kuz: {0}, kost: {1}, abay: {2}, sar1: {3}, sar3: {4}, shah: {5}, tent: {6}, len: {7}",
																kuzInsertTask, kostInsertTask, abayInsertTask, sar1InsertTask, sar3InsertTask, shahInsertTask, tentInsertTask, lenInsertTask));
		}

		class MineWarehouseLogicModel
		{
			List<int> _plusWagonID;
			List<int> _minusWagonID;
			List<int> _plusBeltID;
			List<int> _minusBeltID;
			List<int> _plusSkipID;
			List<int> _minusRockID;
			int _warehouseID;

			public List<int> PlusWagonID => _plusWagonID;
			public List<int> MinusWagonID => _minusWagonID;
			public List<int> PlusBeltID => _plusBeltID;
			public List<int> MinusBeltID => _minusBeltID;
			public List<int> PlusSkipID => _plusSkipID;
			public List<int> MinusRockID => _minusRockID;
			public int WarehouseID => _warehouseID;

			public MineWarehouseLogicModel(List<int> plusWagonID = null, List<int> minusWagonID = null, List<int> plusBeltID = null, List<int> minusBeltID = null,
						List<int> plusSkipID = null, List<int> minusRockID = null, int warehouseID = 1)
			{
				_plusWagonID = plusWagonID;
				_minusWagonID = minusWagonID;
				_plusBeltID = plusBeltID;
				_minusBeltID = minusBeltID;
				_plusSkipID = plusSkipID;
				_minusRockID = minusRockID;
				_warehouseID = warehouseID;
			}
		}
	}
}
