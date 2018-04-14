using System.Threading.Tasks;
using System.ServiceModel;

namespace CalculatorService
{
    [ServiceContract]
    interface ICalculatorService
    {
        [OperationContract]
        Task<string> Add(int a, int b);
        [OperationContract]
        Task<string> Subtract(int a, int b);
    }
}
