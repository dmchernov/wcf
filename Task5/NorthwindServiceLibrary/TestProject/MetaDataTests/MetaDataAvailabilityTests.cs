using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using NorthwindService.Contracts.ServiceContracts;
using NorthwindService.Contracts.Subscription;
using NUnit.Framework;
using TestProject.Common;

namespace TestProject.MetaDataTests
{
	public class MetaDataAvailabilityTests : IOrderSubscription
	{
		private static readonly Uri iisHttpOrdersAddress = new Uri("http://localhost:802/service/Orders.svc/mex");
		private static readonly Uri windowsServiceHttpOrdersAddress = new Uri("http://localhost:809/northwindwindowsservice/orders/mex");

		private static readonly Uri iisTcpOrdersAddress = new Uri("net.tcp://localhost:12345/service/Orders.svc/mex");
		private static readonly Uri windowsServiceTcpOrdersAddress = new Uri("net.tcp://localhost:13579/northwindwindowsservice/orders/mex");

		[Test]
		public void IisHttpMetaDataAvailability()
		{
			PrintMetaDataInfo(iisHttpOrdersAddress);
		}

		[Test]
		public void IisTcpMetaDataAvailability()
		{
			PrintMetaDataInfo(iisTcpOrdersAddress);
		}

		[Test]
		public void WindowsServiceHttpMetaDataAvailability()
		{
			PrintMetaDataInfo(windowsServiceHttpOrdersAddress);
		}

		[Test]
		public void WindowsServiceTcpMetaDataAvailability()
		{
			PrintMetaDataInfo(windowsServiceTcpOrdersAddress);
		}

		private void PrintMetaDataInfo(Uri address)
		{
			MetadataExchangeClient metaClient = new MetadataExchangeClient(address, MetadataExchangeClientMode.MetadataExchange);
			MetadataSet metadataSet = metaClient.GetMetadata();

			WsdlImporter importer = new WsdlImporter(metadataSet);

			foreach (ContractDescription cd in importer.ImportAllContracts())
			{
				Console.WriteLine($"Name: {cd.Name}");

				Console.WriteLine("Operations:");
				foreach (var operation in cd.Operations)
				{
					Console.WriteLine(operation.Name);
				}
			}
		}

		[Test]
		public void EndpointTests()
		{
			//MetadataExchangeClient metaClient = new MetadataExchangeClient(new Uri("http://localhost:802/service/Orders.svc?wsdl"), MetadataExchangeClientMode.HttpGet);
			MetadataExchangeClient metaClient = new MetadataExchangeClient(new Uri("http://localhost:802/service/Orders.svc/mex"), MetadataExchangeClientMode.MetadataExchange);
			MetadataSet metadataSet = metaClient.GetMetadata();

			WsdlImporter importer = new WsdlImporter(metadataSet);
			var endpoints = importer.ImportAllEndpoints();
			foreach (var endpoint in endpoints)
			{
				var binding = endpoint.Binding;
				ConfigureBinding(ref binding);

				var factory = new DuplexChannelFactory<IOrderService>(new InstanceContext(this), endpoint.Binding, endpoint.Address);
				var service = factory.CreateChannel(endpoint.Address);

				var orderId = service?.GetOrders().First().OrderID;
				var order = service?.GetOrderEx(orderId.GetValueOrDefault());
				Assert.NotNull(order);
				OrdersHelper.PrintFullOrderInfo(order);
				
			}
		}

		private void ConfigureBinding(ref Binding binding)
		{
			var httpBinding = binding as NetHttpBinding;
			if (httpBinding != null)
			{
				httpBinding.MaxReceivedMessageSize = Int32.MaxValue;
				httpBinding.MaxBufferSize = Int32.MaxValue;
				binding = httpBinding;
				return;
			}

			var tcpBinding = binding as NetTcpBinding;
			if (tcpBinding != null)
			{
				tcpBinding.MaxReceivedMessageSize = Int32.MaxValue;
				tcpBinding.MaxBufferSize = Int32.MaxValue;
				binding = tcpBinding;
				return;
			}
		}

		public void SendOrderNotification(OrderNotification notification){}

		public void SendServiceData(SubscriptionServiceData data){}
	}
}
