using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;
using NorthwindServiceFactories;
using NorthwindServiceLibrary.Contracts;
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
		//private ServiceHost _ordersRest = new ServiceHost(typeof(OrdersRestService), new Uri(httpBaseAddress, "ordersrest"));
		//private ServiceHost _categoriesRest = new ServiceHost(typeof(CategoryRestService), new Uri(httpBaseAddress, "catrest"));
		#endregion

		#region Раскомментировать для настройки из файла конфигурации
		private ServiceHost _ordersHost = new ServiceHost(typeof(OrderService));
		private ServiceHost _productsHost = new ServiceHost(typeof(ProductService));
		private ServiceHost _categoriesHost = new ServiceHost(typeof(CategoryService));
		private ServiceHost _ordersRest = new ServiceHost(typeof(OrdersRestService));
		private ServiceHost _categoriesRest = new ServiceHost(typeof(CategoryRestService));
		#endregion

		public bool Start(HostControl hostControl)
		{
			#region Настройка из кода
			//_ordersHost = ServiceHostHelper.ConfigureServiceHost(_ordersHost, ContractDescription.GetContract(typeof(IOrderService)), String.Empty);
			//_productsHost = ServiceHostHelper.ConfigureServiceHost(_productsHost, ContractDescription.GetContract(typeof(IProductService)), String.Empty);
			//_categoriesHost = ServiceHostHelper.ConfigureServiceHost(_categoriesHost, ContractDescription.GetContract(typeof(ICategoryService)), String.Empty, true);
			//_ordersRest = ServiceHostHelper.ConfigureRestServiceHost(_ordersRest, ContractDescription.GetContract(typeof(IOrderRestService)), String.Empty);
			//_categoriesRest = ServiceHostHelper.ConfigureRestServiceHost(_categoriesRest, ContractDescription.GetContract(typeof(ICategoryRestService)), String.Empty);
			#endregion
			var ordersResetEvent = new ManualResetEvent(false);
			var catResetEvent = new ManualResetEvent(false);
			var prodResetEvent = new ManualResetEvent(false);
			var ordersRestResetEvent = new ManualResetEvent(false);
			var catRestResetEvent = new ManualResetEvent(false);

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
			_ordersRest.BeginOpen(ar =>
			{
				_ordersRest.EndOpen(ar);
				ordersRestResetEvent.Set();
			}, null);
			_categoriesRest.BeginOpen(ar =>
			{
				_categoriesRest.EndOpen(ar);
				catRestResetEvent.Set();
			}, null);

			WaitHandle.WaitAll(new WaitHandle[] { ordersResetEvent, catRestResetEvent, catResetEvent, prodResetEvent, ordersRestResetEvent });
			
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			var ordersResetEvent = new ManualResetEvent(false);
			var catResetEvent = new ManualResetEvent(false);
			var prodResetEvent = new ManualResetEvent(false);
			var ordersRestResetEvent = new ManualResetEvent(false);
			var catRestResetEvent = new ManualResetEvent(false);

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
			_ordersRest.BeginClose(ar =>
			{
				_ordersRest.EndClose(ar);
				ordersRestResetEvent.Set();
			}, null);
			_categoriesRest.BeginClose(ar =>
			{
				_categoriesRest.EndClose(ar);
				catRestResetEvent.Set();
			}, null);

			WaitHandle.WaitAll(new WaitHandle[] { ordersResetEvent, catRestResetEvent, catResetEvent, prodResetEvent, ordersRestResetEvent });

			return true;
		}
	}
}
