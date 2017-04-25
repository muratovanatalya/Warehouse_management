using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfService
{
    [ServiceContract]
    public interface IWarehouseService
    {
        [OperationContract]
        List<hangar> GetHangars();

        [OperationContract]
        void UpdateHangar(hangar h);

        [OperationContract]
        int Add(int n1, int n2);
    }
}
