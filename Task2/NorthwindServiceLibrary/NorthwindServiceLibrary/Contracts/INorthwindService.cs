using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using NorthwindServiceLibrary.Models;

namespace NorthwindServiceLibrary.Contracts
{
	// ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
	[ServiceContract(Namespace = "http://epam.com/dmitrii_chernov/northwind")]
	public interface INorthwindService
	{
		/*[OperationContract]
		string GetData (int value);

		[OperationContract]
		CompositeType GetDataUsingDataContract (CompositeType composite);*/
		// TODO: Добавьте здесь операции служб
		[OperationContract]
		IList<Order> GetOrders();

		[OperationContract]
		Order GetOrderEx(int orderId);

		[OperationContract]
		Order Add(Order newOrder);
		[OperationContract]
		int TestContract();
	}

	// Используйте контракт данных, как показано на следующем примере, чтобы добавить сложные типы к сервисным операциям.
	// В проект можно добавлять XSD-файлы. После построения проекта вы можете напрямую использовать в нем определенные типы данных с пространством имен "NorthwindServiceLibrary.ContractType".
	[DataContract]
	public class CompositeType
	{
		bool boolValue = true;
		string stringValue = "Hello ";

		[DataMember]
		public bool BoolValue
		{
			get { return boolValue; }
			set { boolValue = value; }
		}

		[DataMember]
		public string StringValue
		{
			get { return stringValue; }
			set { stringValue = value; }
		}
	}
}
