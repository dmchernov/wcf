using System.Runtime.Serialization;

namespace NorthwindService.Contracts.FaultContracts.OrderFaults
{
	[DataContract]
	public class OrderNotFound : OrderFault
	{
	}
}
