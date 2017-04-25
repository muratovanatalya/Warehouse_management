using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfService
{
    public class WarehouseService : IWarehouseService, IDisposable
    {
        readonly WarehouseEntities _Entities = new WarehouseEntities();

        public List<hangar> GetHangars()
        {
            return _Entities.hangars.ToList();
        }

        [OperationBehavior(TransactionScopeRequired =true)]
        public void UpdateHangar(hangar h)
        {
            var hangar = _Entities.hangars.Where(han => han.IdHangar == h.IdHangar).First();
            hangar.FreePlaces = h.FreePlaces;
            _Entities.SaveChanges();
        }

        public int Add(int n1, int n2)
        {
            return n1 + n2;
        }

        public void Dispose()
        {
            _Entities.Dispose();
        }
           
    }
}
