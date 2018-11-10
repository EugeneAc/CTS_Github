using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models.Mnemonic
{
    public class MnemonicMainKotel : MnemonicMain
    {
        public Mine_Kotel shah { get; set; }
        public Mine_Kotel tent { get; set; }
        public Mine_Kotel len { get; set; }
        public Mine_Kotel kaz { get; set; }
        public Mine_Kotel abay { get; set; }
        public Mine_Kotel kost { get; set; }
        public Mine_Kotel kuz { get; set; }
        public Mine_Kotel sar3 { get; set; }
        public Mine_Kotel sar1 { get; set; }

        public override decimal TotalPlan
        {
            get
            {
                return (decimal)
                            (shah.ConsumptionNorm
                            + tent.ConsumptionNorm
                            + len.ConsumptionNorm
                            + kaz.ConsumptionNorm
                            + abay.ConsumptionNorm
                            + sar1.ConsumptionNorm
                            + sar3.ConsumptionNorm
                            + kuz.ConsumptionNorm
                            + kost.ConsumptionNorm);
            }
        }

        public override decimal TotalFact
        {
            get
            {
                return (decimal)
                            (shah.ConsumptionFact
                            + tent.ConsumptionFact
                            + len.ConsumptionFact
                            + kaz.ConsumptionFact
                            + abay.ConsumptionFact
                            + sar1.ConsumptionFact
                            + sar3.ConsumptionFact
                            + kuz.ConsumptionFact
                            + kost.ConsumptionFact);
            }
        }
    }
}

