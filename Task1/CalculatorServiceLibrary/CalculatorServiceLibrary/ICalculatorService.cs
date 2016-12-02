using System.ServiceModel;
using System.ServiceModel.Web;

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
