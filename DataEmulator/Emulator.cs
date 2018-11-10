using CTS_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataEmulator
{
  static class Emulator
  {
    static CentralDBContext ldb = new CentralDBContext();
    static Random rnd = new Random();
    public static int cycleQnt = 0;

    public static void wagonTransfersEmul(int qntFrom, int qntTo,
      int bruttoFrom, int bruttoTo, int tareFrom, int tareTo)
    {
      try
      {
        WagonTransfer transfer = new WagonTransfer();
        var bbb = ldb.Locations.ToList();
        transfer.FromDestID = ldb.Locations.ToList()[rnd.Next(1, ldb.Locations.Count())].ID;

        //transfer.ToDestID = ldb.Locations.ToList()[rnd.Next(1, ldb.Locations.Count())].ID;

        //while (transfer.FromDestID == transfer.ToDestID)
        //{
        //  transfer.ToDestID = ldb.Locations.ToList()[rnd.Next(1, ldb.Locations.Count())].ID;
        //}

        if (transfer.FromDestID == "kuz")
        {
          transfer.EquipID = 2;
          transfer.ItemID = 2;
        }
        else
        {
          transfer.EquipID = 1;
          transfer.ItemID = 1;
        }

        transfer.ID = "W" + transfer.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        transfer.TransferTimeStamp = DateTime.Now;
        transfer.LasEditDateTime = DateTime.Now;
        transfer.LotName = transfer.FromDestID + "_" + DateTime.Now.ToString("yyMMddHHmmss");
        transfer.IsValid = true;
        transfer.OperatorName = "emulator";

        int wagonQnt = rnd.Next(qntFrom, qntTo);
        for (int i = 0; i < wagonQnt; i++)
        {
          WagonTransfer transfer1 = new WagonTransfer()
          {
            ID = transfer.ID.Substring(0, 2) + (Int32.Parse(transfer.ID.Substring(2)) + i).ToString(),
            Brutto = rnd.Next(bruttoFrom, bruttoTo),
            Tare = rnd.Next(tareFrom, tareTo),
            FromDestID = transfer.FromDestID,
            //ToDestID = transfer.ToDestID,
            IsValid = transfer.IsValid,
            LotName = transfer.LotName,
            OperatorName = transfer.OperatorName,
            LasEditDateTime = transfer.LasEditDateTime,
            TransferTimeStamp = transfer.TransferTimeStamp,
            SublotName = rnd.Next(100000, 999999).ToString(),
			  EquipID = transfer.EquipID,
            ItemID = transfer.ItemID
          };

          ldb.WagonTransfers.Add(transfer1);

          // Путь .\\Log
          string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
          if (!Directory.Exists(pathToLog))
            Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
          string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
          AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
          string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}] [{2}]\r\n",
          DateTime.Now, wagonQnt, transfer1.ID);

          File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
        }

        ldb.SaveChanges();
      }
      catch (Exception ex)
      {

      }
    }

    public static void beltTransfersEmul(int qntFrom, int qntTo)
    {
      try
      {
        BeltTransfer transfer = new BeltTransfer();

        transfer.EquipID = ldb.BeltScales.ToList()[rnd.Next(1, ldb.BeltScales.Count())].ID;

        transfer.ID = "B" + transfer.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        var scale = ldb.BeltScales.Where(x => x.ID == transfer.EquipID).ToList();
        transfer.LotName = scale[0].Name + "-" + DateTime.Now.ToString("yyMMddHHmmss");
        transfer.LotQuantity = rnd.Next(qntFrom, qntTo);
        transfer.TransferTimeStamp = DateTime.Now;
        transfer.LasEditDateTime = DateTime.Now;
        transfer.IsValid = true;
        transfer.OperatorName = "emulator";

        ldb.BeltTransfers.Add(transfer);
        ldb.SaveChanges();
      }
      catch (Exception ex)
      {

      }
    }

    public static void vehiTransfersEmul(int bruttoFrom, int bruttoTo, int tareFrom, int tareTo)
    {
      try
      {
        VehiTransfer transfer = new VehiTransfer();

        transfer.EquipID = ldb.VehiScales.ToList()[rnd.Next(1, ldb.VehiScales.Count())].ID;
        transfer.ID = "V" + transfer.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        var scale = ldb.VehiScales.Where(x => x.ID == transfer.EquipID).ToList();
        transfer.FromDestID = ldb.Locations.ToList()[rnd.Next(0, ldb.Locations.Count() - 1)].ID;

        transfer.LotName = scale[0].Name + "-" + DateTime.Now.ToString("yyMMddHHmmss");
        transfer.Brutto = rnd.Next(bruttoFrom, bruttoTo);
        transfer.Tare = rnd.Next(tareFrom, tareTo);
        transfer.TransferTimeStamp = DateTime.Now;
        transfer.LasEditDateTime = DateTime.Now;
        transfer.IsValid = true;
        transfer.OperatorName = "emulator";

        ldb.VehiTransfers.Add(transfer);
        ldb.SaveChanges();
      }
      catch (Exception ex)
      {

      }
    }

    public static void skipTransfersEmul()
    {
      try
      {
        SkipTransfer transfer = new SkipTransfer();

        transfer.EquipID = ldb.Skips.ToList()[rnd.Next(1, ldb.Skips.Count())].ID;
        transfer.ID = "S" + transfer.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        transfer.LiftingID = rnd.Next(1, 350).ToString();
        transfer.TransferTimeStamp = DateTime.Now;
        transfer.LasEditDateTime = DateTime.Now;
        transfer.IsValid = true;
        transfer.OperatorName = "emulator";

        ldb.SkipTransfers.Add(transfer);
        ldb.SaveChanges();
      }
      catch (Exception ex)
      {

      }
    }
  }
}
