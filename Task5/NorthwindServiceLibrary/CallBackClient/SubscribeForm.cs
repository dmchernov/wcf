using System;
using System.ServiceModel;
using System.Windows.Forms;
using NorthwindService.Contracts.Subscription;

namespace CallBackClient
{
	public partial class SubscribeForm : Form, IOrderSubscription
	{
		private readonly DuplexChannelFactory<IOrderChannelService> _factory;
		private IOrderChannelService _service;
		public SubscribeForm ()
		{
			InitializeComponent();
			//_service = new OrderServiceClient(new InstanceContext(this));
			_factory = new DuplexChannelFactory<IOrderChannelService>(new InstanceContext(this), "orderHttp");
			_service = _factory.CreateChannel();

			Closed += OnClosed;
		}

		private void OnClosed(object sender, EventArgs eventArgs)
		{
			if (_service?.State == CommunicationState.Faulted)
				_service?.Abort();
			else
				_service?.Close();

			if (_factory?.State == CommunicationState.Faulted)
				_factory?.Abort();
			else
				_factory?.Close();
		}

		private void btnSubscribe_Click (object sender, EventArgs e)
		{
			try
			{
				_service.Subscribe();
			}
			catch
			{
				MessageBox.Show(@"Ошибка взаимодействия с сервисом");
				_service.Abort();
				_service = _factory.CreateChannel();
			}
		}

		private void btnUnSubscribe_Click (object sender, EventArgs e)
		{
			try
			{
				_service.UnSubscribe();
			}
			catch
			{
				MessageBox.Show(@"Ошибка взаимодействия с сервисом");
				_service.Abort();
				_service = _factory.CreateChannel();
			}
		}

		public void SendOrderNotification(OrderNotification notification)
		{
			tbMessages.AppendText($"New status for order №{notification.OrderId} is {notification.NewStatus}" + Environment.NewLine);
		}

		public void SendServiceData(SubscriptionServiceData data)
		{
			tbMessages.AppendText($"Current request result: {data.CurrentOperationResult}; Subscribed: {data.IsSubscribed}" + Environment.NewLine);
		}
	}
}
