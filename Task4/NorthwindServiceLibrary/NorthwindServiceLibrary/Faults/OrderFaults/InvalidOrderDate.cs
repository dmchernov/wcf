using System.Runtime.Serialization;

namespace NorthwindServiceLibrary.Faults.OrderFaults
{
	[DataContract]
	public class InvalidOrderDate : OrderFault
	{
	}
}
