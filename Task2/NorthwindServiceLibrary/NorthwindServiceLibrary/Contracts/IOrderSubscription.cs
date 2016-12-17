using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindServiceLibrary.Contracts
{
	public interface IOrderSubscription
	{
		[OperationContract(IsOneWay = true)]
		void SendInformationMessage(string message);
	}
}
