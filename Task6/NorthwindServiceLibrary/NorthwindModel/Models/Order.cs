using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using NorthwindModel.Enums;
using NorthwindModel.Models.CustomModels;

namespace NorthwindModel.Models
{
	[DataContract]
	[KnownType(typeof(OrderStatus))]
	[KnownType(typeof(Employee))]
	[KnownType(typeof(Customer))]
	[KnownType(typeof(Shipper))]
	[KnownType(typeof(Order))]
	[KnownType(typeof(Order_Detail))]
	public class Order : BasicOrder
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        [StringLength(5)]
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }

		[DataMember]
        public DateTime? RequiredDate { get; set; }

		[DataMember]
        public int? ShipVia { get; set; }

        [Column(TypeName = "money")]
		[DataMember]
        public decimal? Freight { get; set; }

        [StringLength(40)]
		[DataMember]
        public string ShipName { get; set; }

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

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[DataMember]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
		[DataMember]
        public virtual Shipper Shipper { get; set; }

		[DataMember]
		[NotMapped]
		public OrderStatus Status
		{
			get
			{
				if (!OrderDate.HasValue && !ShippedDate.HasValue)
					return OrderStatus.New;

				if(ShippedDate.HasValue)
					return OrderStatus.Complete;

				return OrderStatus.InProgress;
			}
			set { }
		}
    }
}
