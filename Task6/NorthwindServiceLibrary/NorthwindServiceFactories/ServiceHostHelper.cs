using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace NorthwindServiceFactories
{
	public static class ServiceHostHelper
	{
		public static ServiceHost ConfigureServiceHost(ServiceHost host, ContractDescription description, string endpointName, bool useStreaming = false)
		{
			var httpsAddresses = host.BaseAddresses.Where(a => a.Scheme == Uri.UriSchemeHttps).ToList();
			httpsAddresses.ForEach(httpsAddress =>
			{
				var netHttpBinding = new NetHttpBinding { MaxReceivedMessageSize = Int32.MaxValue, TransferMode = useStreaming ? TransferMode.Streamed : TransferMode.Buffered };
				netHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
				netHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
				var netHttpEndpoint = new ServiceEndpoint(description, netHttpBinding, new EndpointAddress(new Uri(httpsAddress, endpointName)));
				host.AddServiceEndpoint(netHttpEndpoint);

				var mexBinding = MetadataExchangeBindings.CreateMexHttpsBinding();
				host.AddServiceEndpoint(typeof(IMetadataExchange), mexBinding, "mex");
			});

			var httpAddresses = host.BaseAddresses.Where(a => a.Scheme == Uri.UriSchemeHttp).ToList();
			httpAddresses.ForEach(httpAddress =>
			{
				var netHttpBinding = new WSDualHttpBinding() { MaxReceivedMessageSize = Int32.MaxValue };
				netHttpBinding.Security.Mode = WSDualHttpSecurityMode.Message;
				netHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
				var netHttpEndpoint = new ServiceEndpoint(description, netHttpBinding, new EndpointAddress(new Uri(httpAddress, endpointName)));
				host.AddServiceEndpoint(netHttpEndpoint);

				var mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
				host.AddServiceEndpoint(typeof(IMetadataExchange), mexBinding, "mex");
			});

			var tcpAddresses = host.BaseAddresses.Where(a => a.Scheme == Uri.UriSchemeNetTcp).ToList();
			tcpAddresses.ForEach(tcpAddress =>
			{
				var netTcpBinding = new NetTcpBinding { MaxReceivedMessageSize = Int32.MaxValue, TransferMode = useStreaming ? TransferMode.Streamed : TransferMode.Buffered };
				netTcpBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
				netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
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

			var serviceCredentialsBehavior = host.Description.Behaviors.Find<ServiceCredentials>();
			if (serviceCredentialsBehavior == null)
			{
				serviceCredentialsBehavior = new ServiceCredentials();
				serviceCredentialsBehavior.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, "e8717733267945651161feb83376a7b601dbc810");

				serviceCredentialsBehavior.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
				serviceCredentialsBehavior.UserNameAuthentication.CustomUserNamePasswordValidator = new NorthwindServiceLibrary.Security.NorthwindPasswordValidator();

				host.Description.Behaviors.Add(serviceCredentialsBehavior);
			}

			if (host.Description.Behaviors.Find<ServiceAuthorizationBehavior>() != null)
				host.Description.Behaviors.Remove<ServiceAuthorizationBehavior>();

			var serviceAuthorizationBehavior = new ServiceAuthorizationBehavior
			{
				PrincipalPermissionMode = PrincipalPermissionMode.Custom
			};
			var policies = new List<IAuthorizationPolicy> {new NorthwindServiceLibrary.Security.NorthwindAuthorizationPolicy()};
			serviceAuthorizationBehavior.ExternalAuthorizationPolicies = policies.AsReadOnly();

			host.Description.Behaviors.Add(serviceAuthorizationBehavior);
			
			var debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();
			if (debug == null)
			{
				debug = new ServiceDebugBehavior();
				host.Description.Behaviors.Add(debug);
			}
			debug.IncludeExceptionDetailInFaults = true;

			return host;
		}
	}
}
