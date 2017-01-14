using System.Runtime.Serialization;

namespace NorthwindServiceLibrary.Faults.OrderFaults
{
	[DataContract]
	public class OrderFault
	{
		[DataMember]
		public int OrderId { get; set; }
	}
}
