using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NorthwindModel.Models.CustomModels
{
	[DataContract]
	[KnownType(typeof(Order))]
	public class BasicOrder
	{
		public BasicOrder () { }

		[DataMember]
		[Key]
		public int OrderID { get; set; }

		[DataMember]
		public DateTime? OrderDate { get; set; }

		[DataMember]
		public DateTime? ShippedDate { get; set; }

		[StringLength(60)]
		[DataMember]
		public string ShipAddress { get; set; }

		[StringLength(15)]
		[DataMember]
		public string ShipCity { get; set; }

		[StringLength(15)]
		[DataMember]
		public string ShipRegion { get; set; }
	}
}
