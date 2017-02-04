using System.Runtime.Serialization;

namespace NorthwindService.Contracts.FaultContracts.OrderFaults
{
	[DataContract]
	public class OrderFault
	{
		[DataMember]
		public int OrderId { get; set; }
	}
}
