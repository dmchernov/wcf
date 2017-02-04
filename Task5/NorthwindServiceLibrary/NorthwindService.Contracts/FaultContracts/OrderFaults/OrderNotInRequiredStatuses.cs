using System.Runtime.Serialization;
using NorthwindModel.Enums;

namespace NorthwindService.Contracts.FaultContracts.OrderFaults
{
	[DataContract]
	public class OrderNotInRequiredStatuses : OrderFault
	{
		[DataMember]
		public OrderStatus[] RequiredStatuses { get; set; }
	}
}
