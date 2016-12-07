using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using NorthwindServiceLibrary.Models;

namespace NorthwindServiceLibrary.Contracts
{
	// ����������. ������� "�������������" � ���� "�����������" ����� ������������ ��� �������������� ��������� ����� ���������� "IService1" � ���� � ����� ������������.
	[ServiceContract(Namespace = "http://epam.com/dmitrii_chernov/northwind")]
	public interface INorthwindService
	{
		/*[OperationContract]
		string GetData (int value);

		[OperationContract]
		CompositeType GetDataUsingDataContract (CompositeType composite);*/
		// TODO: �������� ����� �������� �����
		[OperationContract]
		IList<Order> GetOrders();

		[OperationContract]
		Order GetOrderEx(int orderId);

		[OperationContract]
		Order Add(Order newOrder);
		[OperationContract]
		int TestContract();
	}

	// ����������� �������� ������, ��� �������� �� ��������� �������, ����� �������� ������� ���� � ��������� ���������.
	// � ������ ����� ��������� XSD-�����. ����� ���������� ������� �� ������ �������� ������������ � ��� ������������ ���� ������ � ������������� ���� "NorthwindServiceLibrary.ContractType".
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
