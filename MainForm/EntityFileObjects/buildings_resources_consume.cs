namespace MainForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.buildings_resources_consume")]
    public partial class buildings_resources_consume
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int building_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int resources_id { get; set; }

        public int consume_speed { get; set; }

        [System.ComponentModel.Browsable(false)]
        public virtual building building { get; set; }

        [System.ComponentModel.Browsable(false)]
        public virtual resource resource { get; set; }
    }
}
