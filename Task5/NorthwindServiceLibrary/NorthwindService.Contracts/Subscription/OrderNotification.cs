﻿using NorthwindModel.Enums;

namespace NorthwindService.Contracts.Subscription
{
	public class OrderNotification
	{
		public OrderStatus NewStatus { get; set; }
		public int OrderId { get; set; }
	}
}
