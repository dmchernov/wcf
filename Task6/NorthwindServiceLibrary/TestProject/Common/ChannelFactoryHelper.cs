using System;
using System.ServiceModel;
using TestProject.Contracts;
using TestProject.Enums;

namespace TestProject.Common
{
	public class ChannelFactoryHelper
	{
		private static DuplexChannelFactory<IOrderChannelService> _ordersFactory;
		private static ChannelFactory<IProductChannelService> _productsFactory;

		public static ChannelFactory<IProductChannelService> ProductsFactory
		{
			get
			{
				if (_productsFactory == null || _productsFactory.State == CommunicationState.Faulted)
				{
					_productsFactory = new ChannelFactory<IProductChannelService>("prodNetHttpIis");
					_productsFactory.Credentials.UserName.UserName = "Guest";
					_productsFactory.Credentials.UserName.Password = String.Empty;
				}
				return _productsFactory;
			}
		}

		public static DuplexChannelFactory<IOrderChannelService> GetOrdersFactory(InstanceContext context, UserRole role)
		{
			if (_ordersFactory == null || _ordersFactory.State != CommunicationState.Faulted)
			{
				_ordersFactory = new DuplexChannelFactory<IOrderChannelService>(context, "ordersTcpIis");
				SetCredential(_ordersFactory, role);
			}

			return _ordersFactory;
		}

		private static DuplexChannelFactory<IOrderChannelService> _ordersFactoryForSecurityTests;
		public static DuplexChannelFactory<IOrderChannelService> GetOrdersFactoryForSecurityTests(InstanceContext context, string name, string password)
		{
			if (_ordersFactoryForSecurityTests == null || _ordersFactoryForSecurityTests.State != CommunicationState.Faulted)
			{
				_ordersFactoryForSecurityTests = new DuplexChannelFactory<IOrderChannelService>(context, "ordersHttps");
				_ordersFactoryForSecurityTests.Credentials.UserName.UserName = name;
				_ordersFactoryForSecurityTests.Credentials.UserName.Password = password;
			}

			return _ordersFactoryForSecurityTests;
		}

		private static void SetCredential(ChannelFactory factory, UserRole role)
		{
			switch (role)
			{
				case UserRole.Manager:
					factory.Credentials.UserName.UserName = "Manager1";
					factory.Credentials.UserName.Password = "123456";
					break;
				case UserRole.Operator:
					factory.Credentials.UserName.UserName = "Operator1";
					factory.Credentials.UserName.Password = "123456";
					break;
				case UserRole.Operator | UserRole.Manager:
					factory.Credentials.UserName.UserName = "SuperUser";
					factory.Credentials.UserName.Password = "123456";
					break;
				case UserRole.None:
					factory.Credentials.UserName.UserName = "Guest";
					factory.Credentials.UserName.Password = String.Empty;
					break;
			}
		}
	}
}
