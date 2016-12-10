using System;
using System.Runtime.Serialization;
using NorthwindModel.Enums;

namespace NorthwindServiceLibrary.Faults
{
	[DataContract]
	public class OrderFault
	{
		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public int OrderId { get; set; }

		[DataMember]
		public OrderStatus Status { get; set; }

		[DataMember]
		public Exception InnerException { get; set; }
	}
}
