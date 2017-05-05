using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData
{
    [ServiceContract]
    public interface IWarehouseService
    {
        [OperationContract]
        List<Hangar> GetAllHangars();

        [OperationContract]
        void Free(string num, string id);

        [OperationContract]
        void Store(string num);

        Hangar HangarWithMinFreePlaces(List<Hangar> hangars);
    }
}
