using System;
using System.ServiceModel;
using System.Windows.Forms;
using CallBackClient.OrderService;

namespace CallBackClient
{
	public partial class SubscribeForm : Form, IOrderServiceCallback
	{
		private OrderServiceClient _service;
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

		public void SendInformationMessage(string message)
		{
			tbMessages.AppendText(message + Environment.NewLine);
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
	}
}
