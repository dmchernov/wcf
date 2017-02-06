using System.ServiceModel;
using System.Threading;
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

		public bool Start (HostControl hostControl)
		{
			#region Настройка из кода
			//_ordersHost = ServiceHostHelper.ConfigureServiceHost(_ordersHost, ContractDescription.GetContract(typeof(IOrderService)), String.Empty);
			//_productsHost = ServiceHostHelper.ConfigureServiceHost(_productsHost, ContractDescription.GetContract(typeof(IProductService)), String.Empty);
			//_categoriesHost = ServiceHostHelper.ConfigureServiceHost(_categoriesHost, ContractDescription.GetContract(typeof(ICategoryService)), String.Empty, true);
			#endregion
			var ordersResetEvent = new ManualResetEvent(false);
			var catResetEvent = new ManualResetEvent(false);
			var prodResetEvent = new ManualResetEvent(false);

			_ordersHost.BeginOpen(ar =>
			{
				_ordersHost.EndOpen(ar);
				ordersResetEvent.Set();
			}, null);
			_productsHost.BeginOpen(ar =>
			{
				_productsHost.EndOpen(ar);
				prodResetEvent.Set();
			}, null);
			_categoriesHost.BeginOpen(ar =>
			{
				_categoriesHost.EndOpen(ar);
				catResetEvent.Set();
			}, null);

			WaitHandle.WaitAll(new WaitHandle[] { ordersResetEvent, catResetEvent, prodResetEvent });

			return true;
		}

		public bool Stop (HostControl hostControl)
		{
			var ordersResetEvent = new ManualResetEvent(false);
			var catResetEvent = new ManualResetEvent(false);
			var prodResetEvent = new ManualResetEvent(false);

			_ordersHost.BeginClose(ar =>
			{
				_ordersHost.EndClose(ar);
				ordersResetEvent.Set();
			}, null);
			_productsHost.BeginClose(ar =>
			{
				_productsHost.EndClose(ar);
				prodResetEvent.Set();
			}, null);
			_categoriesHost.BeginClose(ar =>
			{
				_categoriesHost.EndClose(ar);
				catResetEvent.Set();
			}, null);

			WaitHandle.WaitAll(new WaitHandle[] { ordersResetEvent, catResetEvent, prodResetEvent });

			return true;
		}
	}
}
