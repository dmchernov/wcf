using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using NorthwindModel;
using NorthwindModel.Enums;
using NorthwindModel.Extensions;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;
using NorthwindService.Contracts.ServiceContracts;
using NorthwindService.Contracts.FaultContracts.OrderFaults;
using NorthwindService.Contracts.Subscription;

namespace NorthwindServiceLibrary.Services
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
	public class OrderService : IOrderService
	{
		public IList<BasicOrder> GetOrders()
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				db.Configuration.LazyLoadingEnabled = false;
				var orders = db.Orders.ToBasicOrdersList();

				throw new Exception("Test exception");

				return orders;
			}
		}

		public Order GetOrderEx (int orderId)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				db.Configuration.LazyLoadingEnabled = false;
				var order = db.Orders.FirstOrDefault(o => o.OrderID == orderId);
				if (order == null)
					throw new FaultException<OrderNotFound>(new OrderNotFound {OrderId = orderId});

				var context = (db as IObjectContextAdapter).ObjectContext;
				context.LoadProperty(order, p => p.Order_Details);
				context.LoadProperty(order, p => p.Customer);
				context.LoadProperty(order, p => p.Employee);
				context.LoadProperty(order, p => p.Shipper);

				foreach (var od in order.Order_Details)
				{
					context.LoadProperty(od, p => p.Product);
					context.LoadProperty(od.Product, p => p.Category);
				}
				return order;
			}
		}

		public Order Add(Order newOrder)
		{
			using (var db = new Northwind())
			{
				var result = db.Orders.Add(newOrder);
				db.SaveChanges();
				return GetOrderEx(result.OrderID);
			}
		}

		public Order SendOrderToProcess(int orderId, DateTime orderDate)
		{
			if (orderDate < DateTime.Today)
				throw new FaultException<InvalidOrderDate>(new InvalidOrderDate {OrderId = orderId});

			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				
				var order = db.Orders.First(o => o.OrderID == orderId);
				if (order.Status != OrderStatus.New)
					throw new FaultException<OrderNotInRequiredStatuses>(new OrderNotInRequiredStatuses
					{
						OrderId = orderId,
						RequiredStatuses = new [] {OrderStatus.New}
					});

				order.OrderDate = orderDate;
				db.SaveChanges();

				SendMessage(new OrderNotification {NewStatus = OrderStatus.InProgress, OrderId = order.OrderID});

				return GetOrderEx(order.OrderID);
			}
		}

		public Order SendOrderToCustomer(int orderId, DateTime shippedDate)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				var order = db.Orders.First(o => o.OrderID == orderId);
				if (order.Status != OrderStatus.InProgress)
					throw new FaultException<OrderNotInRequiredStatuses>(new OrderNotInRequiredStatuses
					{
						OrderId = orderId,
						RequiredStatuses = new [] {OrderStatus.InProgress}
					});

				order.ShippedDate = shippedDate;
				db.SaveChanges();

				SendMessage(new OrderNotification { NewStatus = OrderStatus.Complete, OrderId = order.OrderID });

				return GetOrderEx(order.OrderID);
			}
		}

		public Order UpdateOrder(Order orderForUpdate)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;

				var oldOrder = db.Orders.FirstOrDefault(o => o.OrderID == orderForUpdate.OrderID);
				if (oldOrder == null)
					throw new FaultException<OrderNotFound>(new OrderNotFound {OrderId = orderForUpdate.OrderID});

				if (oldOrder.Status != OrderStatus.New)
					throw new FaultException<OrderNotInRequiredStatuses>(new OrderNotInRequiredStatuses {OrderId = orderForUpdate.OrderID, RequiredStatuses = new [] { OrderStatus.New }});

				oldOrder.Customer = db.Customers.Find(orderForUpdate.Customer?.CustomerID) ?? orderForUpdate.Customer;
				oldOrder.Employee = db.Employees.Find(orderForUpdate.Employee?.EmployeeID) ?? orderForUpdate.Employee;
				oldOrder.ShipAddress = orderForUpdate.ShipAddress;

				var oldDetails = db.Order_Details.Where(od => od.OrderID == orderForUpdate.OrderID);

				db.Order_Details.RemoveRange(oldDetails);

				oldOrder.Order_Details = orderForUpdate.Order_Details;
				db.SaveChanges();

				return GetOrderEx(oldOrder.OrderID);
			}
		}

		public void DeleteOrder(int orderId)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;

				var orderForDelete = db.Orders.FirstOrDefault(o => o.OrderID == orderId);
				if (orderForDelete == null)
					throw new FaultException<OrderNotFound>(new OrderNotFound {OrderId = orderId});

				if (orderForDelete.Status == OrderStatus.Complete)
					throw new FaultException<OrderNotInRequiredStatuses>(new OrderNotInRequiredStatuses {RequiredStatuses= new []{OrderStatus.New, OrderStatus.InProgress}, OrderId = orderForDelete.OrderID});

				var detailsForDelete = db.Order_Details.Where(od => od.OrderID == orderId);
				db.Order_Details.RemoveRange(detailsForDelete);
				db.Orders.Remove(orderForDelete);
				db.SaveChanges();
			}
		}

		static List<IOrderSubscription> _callBacks = new List<IOrderSubscription>();

		public void Subscribe()
		{
			var callback = OperationContext.Current.GetCallbackChannel<IOrderSubscription>();
			if (!_callBacks.Contains(callback))
			{
				_callBacks.Add(callback);
				callback.SendServiceData(new SubscriptionServiceData {IsSubscribed = true, CurrentOperationResult = true});
			}
			else
				callback.SendServiceData(new SubscriptionServiceData { IsSubscribed = true, CurrentOperationResult = false });
		}

		public void UnSubscribe()
		{
			var callback = OperationContext.Current.GetCallbackChannel<IOrderSubscription>();
			if (_callBacks.Contains(callback))
			{
				_callBacks.Remove(callback);
				callback.SendServiceData(new SubscriptionServiceData { IsSubscribed = false, CurrentOperationResult = true });
			}
			else
				callback.SendServiceData(new SubscriptionServiceData { IsSubscribed = false, CurrentOperationResult = false });
		}

		private async void SendMessage(OrderNotification notification)
		{
			await Task.Run(() =>
			{
				_callBacks.RemoveAll(c => ((ICommunicationObject) c).State != CommunicationState.Opened);

				List<IOrderSubscription> toRemove = new List<IOrderSubscription>();

				Parallel.ForEach(_callBacks, orderSubscription =>
				{
					try
					{
						orderSubscription.SendOrderNotification(notification);
					}
					catch
					{
						toRemove.Add(orderSubscription);
					}
				});

				_callBacks.RemoveAll(os => toRemove.Contains(os));
			});
		}
	}
}