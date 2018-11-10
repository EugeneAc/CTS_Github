namespace CTS_Models.WagonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vagon_nums
    {
        public int id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime date_time { get; set; }

        [Column("id_recogn")]
        public int recognid { get; set; }
        public virtual recogn recogn { get; set; }

        public int? id_sostav { get; set; }

        [Required]
        [StringLength(8)]
        public string number { get; set; }

        [Required]
        [StringLength(8)]
        public string number_operator { get; set; }

        public int? id_operator { get; set; }

        [Required]
        [StringLength(10)]
        public string camera { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] img { get; set; }

		public int? sync { get; set; }
	}
}
