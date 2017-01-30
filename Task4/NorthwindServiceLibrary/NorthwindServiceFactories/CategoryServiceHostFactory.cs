using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using NorthwindServiceLibrary.Contracts;

namespace NorthwindServiceFactories
{
	public class CategoryServiceHostFactory : ServiceHostFactory
	{
		protected override ServiceHost CreateServiceHost (Type serviceType, Uri[] baseAddresses)
		{
			var host = base.CreateServiceHost(serviceType, baseAddresses);
			return ServiceHostHelper.ConfigureServiceHost(host, ContractDescription.GetContract(typeof(ICategoryService)), "categories.svc", true);
		}
	}
}