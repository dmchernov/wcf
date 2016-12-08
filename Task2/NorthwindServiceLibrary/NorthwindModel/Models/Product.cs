using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace NorthwindModel.Models
{

	[DataContract]
	[KnownType(typeof(Supplier))]
	[KnownType(typeof(Category))]
	[KnownType(typeof(Supplier))]
	public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }
		[DataMember]
        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
		[DataMember]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
		[DataMember]
        public decimal? UnitPrice { get; set; }
		[DataMember]
        public short? UnitsInStock { get; set; }
		[DataMember]
        public short? UnitsOnOrder { get; set; }
		[DataMember]
        public short? ReorderLevel { get; set; }
		[DataMember]
        public bool Discontinued { get; set; }
		[DataMember]
        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
		[DataMember]
        public virtual Supplier Supplier { get; set; }
    }
}
