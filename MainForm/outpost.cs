namespace MainForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.outposts")]
    public partial class outpost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public outpost()
        {
            buildings = new HashSet<building>();
            outpost_missions = new HashSet<outpost_missions>();
            storage_resources = new HashSet<storage_resources>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int outpost_id { get; set; }

        [Required]
        public string outpost_name { get; set; }

        public int outpost_economic_value { get; set; }

        public int outpost_coordinate_x { get; set; }

        public int outpost_coordinate_y { get; set; }

        public int outpost_coordinate_z { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<building> buildings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<outpost_missions> outpost_missions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<storage_resources> storage_resources { get; set; }
    }
}
