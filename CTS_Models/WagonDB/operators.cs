namespace CTS_Models.WagonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class operators
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string login { get; set; }

        [Required]
        [StringLength(50)]
        public string pass { get; set; }

        [Required]
        [StringLength(50)]
        public string display_name { get; set; }
    }
}
