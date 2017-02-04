using System;
using System.ServiceModel;
using NorthwindServiceLibrary.Services;
using Topshelf;

namespace NorthwindWindowService
{
	public class NorthwindWinService : ServiceControl
	{
		#region Раскомментировать для настройки из кода
		//private static readonly Uri httpBaseAddress = new Uri("http://localhost:809/winsvc/northwind/");
		//private static readonly Uri netTcpBaseAddress = new Uri("net.tcp://localhost:15432/winsvc/northwind/");

		//private ServiceHost _ordersHost = new ServiceHost(typeof(OrderService), new Uri(httpBaseAddress, "orders"), new Uri(netTcpBaseAddress, "orders"));
		//private ServiceHost _productsHost = new ServiceHost(typeof(ProductService), new Uri(httpBaseAddress, "products"), new Uri(netTcpBaseAddress, "products"));
		//private ServiceHost _categoriesHost = new ServiceHost(typeof(CategoryService), new Uri(httpBaseAddress, "categories"), new Uri(netTcpBaseAddress, "categories"));
		#endregion

		#region Раскомментировать для настройки из файла конфигурации
		private ServiceHost _ordersHost = new ServiceHost(typeof(OrderService));
		private ServiceHost _productsHost = new ServiceHost(typeof(ProductService));
		private ServiceHost _categoriesHost = new ServiceHost(typeof(CategoryService));
		#endregion

		public bool Start(HostControl hostControl)
		{
			#region Настройка из кода
			//_ordersHost = ServiceHostHelper.ConfigureServiceHost(_ordersHost, ContractDescription.GetContract(typeof(IOrderService)), String.Empty);
			//_productsHost = ServiceHostHelper.ConfigureServiceHost(_productsHost, ContractDescription.GetContract(typeof(IProductService)), String.Empty);
			//_categoriesHost = ServiceHostHelper.ConfigureServiceHost(_categoriesHost, ContractDescription.GetContract(typeof(ICategoryService)), String.Empty, true);
			#endregion
			_ordersHost.BeginOpen(OnOpen, _ordersHost);
			_productsHost.BeginOpen(OnOpen, _productsHost);
			_categoriesHost.BeginOpen(OnOpen, _categoriesHost);
			
			/*while (_ordersHost.State != CommunicationState.Opened && _categoriesHost.State != CommunicationState.Opened && _productsHost.State != CommunicationState.Opened)
			{
				Thread.Sleep(100);
			}*/

			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			_ordersHost.BeginClose(OnClose, _ordersHost);
			_productsHost.BeginClose(OnClose, _productsHost);
			_categoriesHost.BeginClose(OnClose, _categoriesHost);

			/*while (_ordersHost.State != CommunicationState.Closed && _categoriesHost.State != CommunicationState.Closed && _productsHost.State != CommunicationState.Closed)
			{
				Thread.Sleep(100);
			}*/

			return true;
		}

		private void OnOpen(IAsyncResult result)
		{
			ServiceHost service = (ServiceHost)result.AsyncState;
			service.EndOpen(result);
		}

		private void OnClose (IAsyncResult result)
		{
			ServiceHost service = (ServiceHost)result.AsyncState;
			service.EndClose(result);
			((IDisposable)service).Dispose();
		}
	}
}
