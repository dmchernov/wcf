using System.Runtime.Serialization;

namespace NorthwindServiceLibrary.Faults.OrderFaults
{
	[DataContract]
	class OrderNotFound : OrderFault
	{
	}
}
