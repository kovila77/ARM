namespace MainForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.buildings_resources_produce")]
    public partial class buildings_resources_produce
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int building_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int resources_id { get; set; }

        public int produce_speed { get; set; }

        public virtual building building { get; set; }

        public virtual resource resource { get; set; }
    }
}
