namespace MainForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.resources")]
    public partial class resource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public resource()
        {
            buildings_resources_consume = new HashSet<buildings_resources_consume>();
            buildings_resources_produce = new HashSet<buildings_resources_produce>();
            machines_resources_consume = new HashSet<machines_resources_consume>();
            storage_resources = new HashSet<storage_resources>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int resources_id { get; set; }

        [Required]
        public string resources_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<buildings_resources_consume> buildings_resources_consume { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<buildings_resources_produce> buildings_resources_produce { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<machines_resources_consume> machines_resources_consume { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<storage_resources> storage_resources { get; set; }


        //my line
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<buildings_resources> buildings_resources { get; set; }
    }
}
