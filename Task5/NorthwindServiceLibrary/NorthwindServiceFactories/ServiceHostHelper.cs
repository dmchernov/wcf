using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace NorthwindServiceFactories
{
	public static class ServiceHostHelper
	{
		public static ServiceHost ConfigureServiceHost(ServiceHost host, ContractDescription description, string endpointName, bool useStreaming = false)
		{
			var httpAddresses = host.BaseAddresses.Where(a => a.Scheme == Uri.UriSchemeHttp).ToList();
			httpAddresses.ForEach(httpAddress =>
			{
				var netHttpBinding = new NetHttpBinding { MaxReceivedMessageSize = Int32.MaxValue, TransferMode = useStreaming ? TransferMode.Streamed : TransferMode.Buffered };
				var netHttpEndpoint = new ServiceEndpoint(description, netHttpBinding, new EndpointAddress(new Uri(httpAddress, endpointName)));
				host.AddServiceEndpoint(netHttpEndpoint);
				//host.AddServiceEndpoint(new ServiceMetadataEndpoint(new EndpointAddress(new Uri(new Uri(httpAddress, endpointName), "mex"))));
				var mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
				host.AddServiceEndpoint(typeof(IMetadataExchange), mexBinding, "mex");
			});

			var tcpAddresses = host.BaseAddresses.Where(a => a.Scheme == Uri.UriSchemeNetTcp).ToList();
			tcpAddresses.ForEach(tcpAddress =>
			{
				var netTcpBinding = new NetTcpBinding { MaxReceivedMessageSize = Int32.MaxValue, TransferMode = useStreaming ? TransferMode.Streamed : TransferMode.Buffered };
				var netTcpEndpoint = new ServiceEndpoint(description, netTcpBinding, new EndpointAddress(new Uri(tcpAddress, endpointName)));
				host.AddServiceEndpoint(netTcpEndpoint);
				var mexBinding = MetadataExchangeBindings.CreateMexTcpBinding();
				host.AddServiceEndpoint(typeof(IMetadataExchange), mexBinding, "mex");
			});

			var serviceMetadataBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (serviceMetadataBehavior == null)
			{
				serviceMetadataBehavior = new ServiceMetadataBehavior();
				host.Description.Behaviors.Add(serviceMetadataBehavior);
			}

			serviceMetadataBehavior.HttpGetEnabled = true;

			/*var debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();
			if (debug == null)
			{
				debug = new ServiceDebugBehavior();
				host.Description.Behaviors.Add(debug);
			}
			debug.IncludeExceptionDetailInFaults = true;*/

			return host;
		}
	}
}
