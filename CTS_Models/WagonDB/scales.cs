namespace CTS_Models.WagonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class scales
    {
        public int id { get; set; }

        [Column("id_obj")]
        public int objectsid { get; set; }
        public virtual objects objects { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }
    }
}
