using System;
using System.ServiceModel;
using System.Windows.Forms;
using CallBackClient.OrderService;

namespace CallBackClient
{
	public partial class SubscribeForm : Form, IOrderServiceCallback
	{
		private readonly OrderServiceClient _service;
		public SubscribeForm ()
		{
			InitializeComponent();
			_service = new OrderServiceClient(new InstanceContext(this));
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
