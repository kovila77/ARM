namespace MainForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.outpost_missions")]
    public partial class outpost_missions
    {
        [Key]
        public int mission_id { get; set; }

        public int outpost_id { get; set; }

        [Required]
        public string mission_description { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_begin { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_plan_end { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date_actual_end { get; set; }

        public virtual outpost outpost { get; set; }
    }
}
