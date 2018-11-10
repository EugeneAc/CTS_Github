using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models.DbViewModels
{
    [Table("OrcalePlanWithShop_ID")]
    public class OraclePlanWithShop_ID
    {
        public decimal Shop_ID { get; set; }
        public DateTime TransferTimeStamp { get; set; }
        [Key]
        public decimal Plan { get; set; }
    }

    [Table("OraclePlanB")]
    public class OralcleMiningPlanB
    {
        public DateTime TransferTimeStamp { get; set; }
        [Key]
        public decimal Plan { get; set; }
    }

    [Table("OraclePlan")]
    public class OracleMiningPlan
    {
        public DateTime TransferTimeStamp { get; set; }
        [Key]
        public decimal Plan { get; set; }
    }

    [Table("OracleStaff")]
    public class OracleStaff
    {
        public DateTime Date { get; set; }
        public int Shop_ID { get; set; }
        [Key]
        public decimal Cnt { get; set; }
    }
}