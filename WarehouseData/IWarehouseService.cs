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
        Hangar GetHangarById(string id);

        [OperationContract]
        List<Hangar> GetHangarsByArea(string areaId);

        [OperationContract]
        List<Hangar> GetHangars();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool UpdateHangar(Hangar hangar);

        [OperationContract]
        List<Area> GetAreas();
    }
}
