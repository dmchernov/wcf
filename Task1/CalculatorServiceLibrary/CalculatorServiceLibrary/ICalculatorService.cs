using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorServiceLibrary
{
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        [WebGet]
        int Add(int a, int b);
        [OperationContract]
        [WebGet]
        int Mul(int a, int b);
		[OperationContract]
        [WebGet]
        int Sub(int a, int b);
		[OperationContract]
        [WebGet]
        int Div(int a, int b);
    }
}
