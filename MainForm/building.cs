namespace MainForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.buildings")]
    public partial class building
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public building()
        {
            buildings_resources_consume = new HashSet<buildings_resources_consume>();
            buildings_resources_produce = new HashSet<buildings_resources_produce>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int building_id { get; set; }

        [Required]
        public string building_name { get; set; }

        public int? outpost_id { get; set; }

        public virtual outpost outpost { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<buildings_resources_consume> buildings_resources_consume { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<buildings_resources_produce> buildings_resources_produce { get; set; }

        //my line
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<buildings_resources> buildings_resources { get; set; }
    }
}
