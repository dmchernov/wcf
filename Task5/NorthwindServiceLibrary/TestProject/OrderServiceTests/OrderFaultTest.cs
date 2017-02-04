using System;
using System.Linq;
using System.ServiceModel;
using NorthwindModel.Enums;
using NorthwindService.Contracts.FaultContracts.OrderFaults;
using NorthwindService.Contracts.ServiceContracts;
using NorthwindService.Contracts.Subscription;
using NUnit.Framework;
using TestProject.Common;

namespace TestProject.OrderServiceTests
{
	public class OrderFaultTest : IOrderSubscription
	{
		[Test]
		public void GetOrderExFaultTest()
		{
			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				var ex = Assert.Catch<FaultException<OrderNotFound>>(() => { service.GetOrderEx(-1); });
				Assert.AreEqual(ex.Detail.OrderId, -1);
			}
		}

		[Test]
		public void SendOrderToProcessFaultTest()
		{
			var order = new OrdersHelper().AddOrder();

			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				var processedOrder = service.SendOrderToProcess(order.OrderID, DateTime.Now);

				var invalidStatusEx = Assert.Catch<FaultException<OrderNotInRequiredStatuses>>(() => { service.SendOrderToProcess(processedOrder.OrderID, DateTime.Now); });
				Assert.True(invalidStatusEx.Detail.RequiredStatuses.Contains(OrderStatus.New) && invalidStatusEx.Detail.RequiredStatuses.Length == 1);

				var invalidDateEx = Assert.Catch<FaultException<InvalidOrderDate>>(() => { service.SendOrderToProcess(processedOrder.OrderID, DateTime.Now.AddDays(-7)); });
				Assert.AreEqual(invalidDateEx.Detail.OrderId, processedOrder.OrderID);
			}
		}

		[Test]
		public void SendOrderToCustomerFaultTest()
		{
			var order = new OrdersHelper().AddOrder();

			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				var processedOrder = service.SendOrderToProcess(order.OrderID, DateTime.Now);
				var shippedOrder = service.SendOrderToCustomer(processedOrder.OrderID, DateTime.Now.AddDays(7));

				var ex = Assert.Catch<FaultException<OrderNotInRequiredStatuses>>(() => { service.SendOrderToCustomer(shippedOrder.OrderID, DateTime.Now.AddDays(10)); });
				Assert.True(ex.Detail.RequiredStatuses.Contains(OrderStatus.InProgress) && ex.Detail.RequiredStatuses.Length == 1);
			}
		}

		[Test]
		public void UpdateOrderFaultTest()
		{
			var order = new OrdersHelper().AddOrder();

			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				var oldId = order.OrderID;
				order.OrderID = -100;

				Assert.Catch<FaultException<OrderNotFound>>(() => { service.UpdateOrder(order); });

				order.OrderID = oldId;
				order = service.SendOrderToProcess(order.OrderID, DateTime.Now);

				Assert.Catch<FaultException<OrderNotInRequiredStatuses>>(() => { service.UpdateOrder(order); });
			}
		}

		[Test]
		public void DeleteOrderFaultTest()
		{
			var order = new OrdersHelper().AddOrder();

			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				Assert.Catch<FaultException<OrderNotFound>>(() => { service.DeleteOrder(-1); });

				order = service.SendOrderToProcess(order.OrderID, DateTime.Now);
				order = service.SendOrderToCustomer(order.OrderID, DateTime.Now);

				var ex = Assert.Catch<FaultException<OrderNotInRequiredStatuses>>(() => { service.DeleteOrder(order.OrderID); });
				Assert.True(ex.Detail.RequiredStatuses.Length == 2 && !ex.Detail.RequiredStatuses.Contains(OrderStatus.Complete));
			}
		}

		public void SendOrderNotification (OrderNotification notification) {}
		public void SendServiceData (SubscriptionServiceData data) {}
	}
}
