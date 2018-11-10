namespace CTS_Models.WagonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ves_telega
    {
        public int id { get; set; }

        public int id_scale { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime date_time { get; set; }

        public int ves { get; set; }

        public int telega { get; set; }

        public int kolesa { get; set; }

        [Required]
        [StringLength(10)]
        public string speed { get; set; }

        public int id_naprav { get; set; }
    }
}
