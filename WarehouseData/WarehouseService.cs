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

        public List<Hangar> GetAllHangars()
        {
            using (WarehouseContext db = new WarehouseContext())
            {
                return db.Hangars.ToList();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Free(string num, string id)
        {
            using (WarehouseContext db = new WarehouseContext())
            {
                byte n;
                if (!byte.TryParse(num, out n)) throw new FaultException(num + ": введите натуральное число (не превышающее 255)");
                if (n == 0) throw new FaultException("Введите число, отличное от нуля");

                Hangar hangar = db.Hangars.SingleOrDefault(h => h.Id == id);
                if (hangar == null) throw new FaultException(id + ": не найден ангар с данным id");
                if (hangar.PlacedContainers <= n) throw new FaultException(n + ": нельзя выгрузить из ангара " + id + " число контейнеров, превышающее текущее число контейнеров в ангаре");
                hangar.PlacedContainers -= n;
                db.SaveChanges();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Store(string num)
        {
            using (WarehouseContext db = new WarehouseContext())
            {
                int n;
                if (!Int32.TryParse(num, out n) ) throw new FaultException(num + ": введите натуральное число (не превышающее 2 147 483 647)");
                if (n <= 0) throw new FaultException(n + ": введите натуральное число");

                var areas = db.Areas.ToList();
                int[] freePlacesOnAreas = new int[areas.Count];

                int i = 0;
                foreach (var area in areas)
                {
                    var hangars = db.Hangars.Where(h => h.AreaId == area.Id).ToList();
                    freePlacesOnAreas[i] = hangars.Sum(h => h.MaxContainers - h.PlacedContainers);
                    i++;
                }

                int freePlacesOnWarehouse = freePlacesOnAreas.Sum();

                if (freePlacesOnWarehouse == 0) throw new FaultException("Невозможно разместить указанное количество контейнеров на складе! Склад полностью заполнен.");
                if (freePlacesOnWarehouse < n) throw new FaultException("Невозможно разместить указанное количество контейнеров на складе! Превышено максимальное число свободных контейнеров. Количество свободных контейнеров: " + freePlacesOnWarehouse);

                while (n > 0)
                {
                    int minValue = freePlacesOnAreas.Where(x => x > 0).Min();
                    int indexMin = Array.IndexOf(freePlacesOnAreas, minValue);
                    var id = areas.ElementAt(indexMin).Id;
                    var priorityHangars = db.Hangars.Where(h => h.AreaId == id).ToList();

                    while (n > 0 && freePlacesOnAreas[indexMin] != 0)
                    {
                        Hangar hangar = HangarWithMinFreePlaces(priorityHangars);
                        int freePlacesOnHangar = hangar.MaxContainers - hangar.PlacedContainers;
                        int d = Math.Min(freePlacesOnHangar, n);
                        hangar.PlacedContainers += d;
                        db.SaveChanges();
                        n -= d;
                        freePlacesOnAreas[indexMin] -= d;
                    }
                }
            }
        }

        public Hangar HangarWithMinFreePlaces(List<Hangar> hangars)
        {
            int min = 41;
            Hangar hangar = null;
            foreach (var h in hangars)
            {
                int freePlaces = h.MaxContainers - h.PlacedContainers;
                if (freePlaces != 0 && freePlaces < min)
                {
                    min = freePlaces;
                    hangar = h;
                }
            }
            return hangar;
        }

    }
}
