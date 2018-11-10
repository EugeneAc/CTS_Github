namespace CTS_Models.WagonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sostav")]
    public partial class sostav
    {
        public int id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime date_time { get; set; }

        [Required]
        [StringLength(20)]
        public string number { get; set; }

        public int wagon_count { get; set; }

        public int ves { get; set; }

		public int? sync { get; set; }
	}
}
