using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NorthwindModel.Models.CustomModels
{
	[DataContract]
	[KnownType(typeof(Order))]
	//[Table("Orders")]
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

		[DataMember]
		public string ShipAddress { get; set; }

		[DataMember]
		public string ShipCity { get; set; }

		[DataMember]
		public string ShipRegion { get; set; }
	}
}
