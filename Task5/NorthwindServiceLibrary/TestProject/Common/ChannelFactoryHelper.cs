using System.ServiceModel;
using NorthwindService.Contracts.ServiceContracts;
using TestProject.Contracts;

namespace TestProject.Common
{
	public class ChannelFactoryHelper
	{
		private static DuplexChannelFactory<IOrderChannelService> _ordersFactory;

		public static DuplexChannelFactory<IOrderChannelService> GetOrdersFactory(InstanceContext context)
		{
			// Проверка состояния используется для одновременного запуска всех тестов
			return _ordersFactory?.State != CommunicationState.Faulted ? (_ordersFactory = new DuplexChannelFactory<IOrderChannelService>(context, "ordersHttp")) : _ordersFactory;
		}

		private static ChannelFactory<IProductChannelService> _productsFactory = new ChannelFactory<IProductChannelService>("prodHttp");
		public static ChannelFactory<IProductChannelService> ProductsFactory { get { return _productsFactory; } }


	}
}
