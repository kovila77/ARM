namespace MainForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.machines")]
    public partial class machine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public machine()
        {
            machines_resources_consume = new HashSet<machines_resources_consume>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int machine_id { get; set; }

        [Required]
        public string machine_name { get; set; }

        [Required]
        public string machine_class { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<machines_resources_consume> machines_resources_consume { get; set; }
    }
}
