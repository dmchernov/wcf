using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using NorthwindModel.Enums;
using NorthwindModel.Models.CustomModels;

namespace NorthwindModel.Models
{
	[DataContract]
	[KnownType(typeof(Status))]
	[KnownType(typeof(Employee))]
	[KnownType(typeof(Customer))]
	[KnownType(typeof(Shipper))]
	[KnownType(typeof(Order))]
	[KnownType(typeof(Order_Detail))]
	public class Order : BasicOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

		/*[DataMember]
		public int OrderID { get; set; }*/

        [StringLength(5)]
		//[IgnoreDataMember]
        public string CustomerID { get; set; }
		//[IgnoreDataMember]
        public int? EmployeeID { get; set; }

		/*[DataMember]
        public DateTime? OrderDate { get; set; }*/
		[DataMember]
        public DateTime? RequiredDate { get; set; }
		/*[DataMember]
        public DateTime? ShippedDate { get; set; }*/
		[DataMember]
        public int? ShipVia { get; set; }

        [Column(TypeName = "money")]
		[DataMember]
        public decimal? Freight { get; set; }

        [StringLength(40)]
		[DataMember]
        public string ShipName { get; set; }

        /*[StringLength(60)]
		[DataMember]
        public string ShipAddress { get; set; }*/

        /*[StringLength(15)]
		[DataMember]
        public string ShipCity { get; set; }*/

        /*[StringLength(15)]
		[DataMember]
        public string ShipRegion { get; set; }*/

        [StringLength(10)]
		[DataMember]
        public string ShipPostalCode { get; set; }

        [StringLength(15)]
		[DataMember]
        public string ShipCountry { get; set; }
		[DataMember]
        public virtual Customer Customer { get; set; }
		[DataMember]
        public virtual Employee Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[DataMember]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
		[DataMember]
        public virtual Shipper Shipper { get; set; }

		[DataMember]
		[NotMapped]
		public Status Status
		{
			get
			{
				if (!OrderDate.HasValue)
					return Status.New;
				else if(ShippedDate.HasValue)
					return Status.Complete;
				else
					return Status.InProgress;
			}
			private set { }
		}
    }
}
