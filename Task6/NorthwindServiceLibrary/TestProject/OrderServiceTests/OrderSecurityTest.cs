using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using NorthwindService.Contracts.Subscription;
using NUnit.Framework;
using TestProject.Common;
using TestProject.Enums;

namespace TestProject.OrderServiceTests
{
	public class OrderSecurityTest : IOrderSubscription
	{
		public void SendOrderNotification(OrderNotification notification)
		{
			throw new NotImplementedException();
		}

		public void SendServiceData(SubscriptionServiceData data)
		{
			throw new NotImplementedException();
		}

		[Test]
		public void NonExistsUserTest()
		{
			using (var service = ChannelFactoryHelper.GetOrdersFactoryForSecurityTests(new InstanceContext(this), "Ivan", "123456").CreateChannel())
			{
				Assert.Catch<CommunicationException>(() => service.GetOrders());
				if (service.State == CommunicationState.Faulted)
					service.Abort();
			}
		}

		[Test]
		public void IncorrectPasswordTest()
		{
			using (var service = ChannelFactoryHelper.GetOrdersFactoryForSecurityTests(new InstanceContext(this), "Operator1", "1234567890").CreateChannel())
			{
				Assert.Catch<CommunicationException>(() => service.GetOrders());
				if (service.State == CommunicationState.Faulted)
					service.Abort();
			}
		}

		[Test]
		public void AccessDeniedTest()
		{
			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this), UserRole.Operator).CreateChannel())
			{
				var order = new OrdersHelper().AddOrder();
				Assert.Catch<SecurityAccessDeniedException>(() => service.SendOrderToProcess(order.OrderID, DateTime.Now.AddDays(1)));
				if (service.State == CommunicationState.Faulted)
					service.Abort();
			}
		}
	}
}
