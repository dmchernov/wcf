using System;
using NorthwindModel.Enums;

namespace NorthwindServiceLibrary.Faults
{
	public class OrderFault
	{
		public string Message { get; set; }

		public int OrderId { get; set; }

		public OrderStatus Status { get; set; }

		public Exception InnerException { get; set; }
	}
}
