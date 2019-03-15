using CTS_Models;
using CTS_Models.DBContext;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using CTS_Analytics.Models.Mnemonic;
using System.IO;
using CTS_Analytics.Controllers;

namespace CTS_Analytics.Services
{
    public class WagonDbService
    {
        private readonly WagonDBcontext wagdb;


        public WagonDbService()
        {
            this.wagdb = new WagonDBcontext();

        }

        public RaspoznItem GetWagonPhoto(string wagonNumber, DateTime recognDateTime, bool manualFlag = false)
        {

            var timeFrom = recognDateTime.AddSeconds(-2);
            var timeTo = recognDateTime.AddSeconds(2);
            try
            {
                var photosDb = wagdb.vagon_nums
                    .Where(t => t.date_time >= timeFrom & t.date_time <= timeTo)
                    .Where(v => ((v.number == wagonNumber) || (v.number_operator == wagonNumber)))
                    .ToList();
                var photos = photosDb
                    .Select(s => s.img != null ? string.Format("data:image/jpg;base64,{0}", System.Convert.ToBase64String(s.img)) : null)
                    .ToList();
                photos.AddRange(photosDb
                    .Select(s => s.img2 != null ? string.Format("data:image/jpg;base64,{0}", System.Convert.ToBase64String(s.img2)) : null));

                if (photos == null || !photos.Any())
                {
                    return new RaspoznItem();
                }

                var item = new RaspoznItem()
                {
                    Date = recognDateTime,
                    WagonNumber = wagonNumber,
                    GallleryName = "G" + wagonNumber + (int)recognDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                    PictureGallery = photos.Where(p => !string.IsNullOrEmpty(p)).ToList(),
                    ManualRecogn = manualFlag,
                };
                return item;
            }
            catch (Exception)
            {

            } 

             return new RaspoznItem();
        }

        public IQueryable<RaspoznItem> GetWagonNumsAndDates(int recognID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var dbvagonNums = wagdb.vagon_nums
               .Where(d => d.date_time >= fromDate && d.date_time <= toDate)
               .Where(f => !string.IsNullOrEmpty(f.number) || !string.IsNullOrEmpty(f.number_operator))
               .Where(i => i.recognid == recognID)
               .Select(s => new RaspoznItem
               {
                   Date = s.date_time,
                   WagonNumber = s.number != null ? s.number :
                    s.number_operator != null ? s.number_operator : "0",
                   IdSostav = s.id_sostav ?? 0
               }).OrderByDescending(d => d.Date);
                return dbvagonNums;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public IQueryable<WagonTransfer> GetDataFromWagonDB(DateTime fromDate, DateTime toDate, string locationID)
        {
            using (var wagdb = new WagonDBcontext())
            {
                var vagons = wagdb.ves_vagon.Where(v => v.scales.objects.name == locationID).
                    Where(t => t.date_time_brutto >= fromDate && t.date_time_brutto <= toDate).
                    Select(s => new WagonTransfer()
                    {
                        ID = s.id.ToString(),
                        Brutto = s.ves_brutto / 1000,
                        SublotName = s.vagon_num,
                        LotName = s.nakladn,
                        Tare = s.ves_tara / 1000,
                        TransferTimeStamp = s.date_time_brutto,
                        ToDest = s.poluch.display_name,
                        FromDestID = s.otpravl.name,
                        IsValid = true,
                    }
                    );
                return vagons.OrderByDescending(t => t.TransferTimeStamp);
            }
        }

    }
}
