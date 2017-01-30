using NorthwindModel.Enums;

namespace NorthwindServiceLibrary.Subscription
{
	public class OrderNotification
	{
		public OrderStatus NewStatus { get; set; }
		public int OrderId { get; set; }
	}
}
