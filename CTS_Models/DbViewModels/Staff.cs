using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models.DbViewModels
{
    [Table("LocalStaff")]
    public class Staff
    {
        public DateTime Date { get; set; }
        public int ShopID { get; set; }
        public string LocationID { get; set; }
        [Key]
        public string CNT { get; set; }
    }
}
