using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models.DbViewModels
{
    public interface iLocalPlan
    {
        DateTime Date { get; set; }
        int Plan { get; set; }
    }

    public interface iLocalPlanWithLocationID : iLocalPlan
    {
        string LocationID { get; set; }
    }

    [Table("LocalPlanBWithLocationID")]
    public class LocalPlanBWithLocationID : iLocalPlanWithLocationID
    {
        public string LocationID { get; set; }
        public DateTime Date { get; set; }
        [Key]
        public int Plan { get; set; }
    }

    [Table("LocalPlanWithLocationID")]
    public class LocalPlanWithLocationID : iLocalPlanWithLocationID
    {
        public string LocationID { get; set; }
        public DateTime Date { get; set; }
        [Key]
        public int Plan { get; set; }
    }

    [Table("LocalPlanB")]
    public class LocalMiningPlanB
    {
        public DateTime Date { get; set; }
        [Key]
        public decimal Plan { get; set; }
    }

    [Table("LocalPlan")]
    public class LocalMiningPlan
    {

        public string LocationID { get; set; }
        public DateTime Date { get; set; }
        [Key]
        public decimal Plan { get; set; }
    }
}