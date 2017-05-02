using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Transactions;

namespace WarehouseData
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, TransactionIsolationLevel = IsolationLevel.Serializable, TransactionTimeout = "00:05:00")]
    public class WarehouseService : IWarehouseService
    {
        public Hangar GetHangarById(string id)
        {
            using (WarehouseContext db = new WarehouseContext())
            {
                return db.Hangars.SingleOrDefault(h => h.Id == id);
            }
        }

        public List<Hangar> GetHangarsByArea(string areaId)
        {
            using (WarehouseContext db = new WarehouseContext())
            {
                return db.Hangars.Where(h => h.AreaId == areaId).ToList();
            }
        }

        public List<Hangar> GetHangars()
        {
            using (WarehouseContext db = new WarehouseContext())
            {
                return db.Hangars.ToList();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public bool UpdateHangar(Hangar hangar)
        {
            using (WarehouseContext db = new WarehouseContext())
            {
                var newHangar = db.Hangars.FirstOrDefault(h => h.Id == hangar.Id);
                if (newHangar != null)
                {
                    newHangar.PlacedContainers = hangar.PlacedContainers;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<Area> GetAreas()
        {
            using (WarehouseContext db = new WarehouseContext())
            {
                return db.Areas.ToList();
            }
        }

    }
}
