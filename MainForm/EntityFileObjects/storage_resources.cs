namespace MainForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.storage_resources")]
    public partial class storage_resources
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int outpost_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int resources_id { get; set; }

        public int count { get; set; }

        public int accumulation_speed { get; set; }

        [System.ComponentModel.Browsable(false)]
        public virtual outpost outpost { get; set; }

        [System.ComponentModel.Browsable(false)]
        public virtual resource resource { get; set; }
    }
}
