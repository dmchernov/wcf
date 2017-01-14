using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using NorthwindModel.Models.CustomModels;

namespace NorthwindModel.Models
{

	[DataContract]
	public class Category : BasicCategory
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Products = new HashSet<Product>();
        }
		/*[DataMember]
        public int CategoryID { get; set; }*/

        /*[Required]
        [StringLength(15)]
		[DataMember]
        public string CategoryName { get; set; }*/

        [Column(TypeName = "ntext")]
		[DataMember]
        public string Description { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
