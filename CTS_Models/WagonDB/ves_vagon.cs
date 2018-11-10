namespace CTS_Models.WagonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ves_vagon
    {
        public int id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime date_time_brutto { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime date_time_tara { get; set; }

        [Required]
        [StringLength(8)]
        public string vagon_num { get; set; }

        public int? id_vagon_nums { get; set; }

        [Required]
        [StringLength(20)]
        public string nakladn { get; set; }

        [Column("id_otpravl")]
        public int? otpravlid { get; set; }
        public virtual otprav_poluch otpravl { get; set; }

        [Column("id_poluch")]
        public int? poluchid { get; set; }
        public virtual otprav_poluch poluch { get; set; }

        [Required]
        [StringLength(30)]
        public string gruz { get; set; }

        public int ves_netto { get; set; }

        public int? ves_netto_docs { get; set; }

        public int ves_brutto { get; set; }

        public int ves_tara { get; set; }

        [Column("id_scales")]
        public int? scalesid { get; set; }
        public virtual scales scales { get; set; }

        public int? id_operator { get; set; }

        [Required]
        [StringLength(10)]
        public string speed { get; set; }

        public int? id_sostav { get; set; }

		[Column("id_naprav")]
		public int? napravlenieid { get; set; }

		public virtual napravlenie napravlenie { get; set; }

		public int? sync { get; set; }
	}
}
