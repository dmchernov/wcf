using System.Runtime.Serialization;
using NorthwindModel.Enums;

namespace NorthwindServiceLibrary.Faults.OrderFaults
{
	[DataContract]
	class OrderNotInRequiredStatuses : OrderFault
	{
		[DataMember]
		public OrderStatus[] RequiredStatuses { get; set; }
	}
}
