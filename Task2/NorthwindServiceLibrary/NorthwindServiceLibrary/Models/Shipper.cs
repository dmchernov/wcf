using System.Runtime.Serialization;

namespace NorthwindServiceLibrary.Models
{
	using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

	[DataContract]
    public partial class Shipper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shipper()
        {
            Orders = new HashSet<Order>();
        }
		[DataMember]
        public int ShipperID { get; set; }

        [Required]
        [StringLength(40)]
		[DataMember]
        public string CompanyName { get; set; }

        [StringLength(24)]
		[DataMember]
        public string Phone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
