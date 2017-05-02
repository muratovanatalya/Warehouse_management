using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WarehouseClient.WarehouseServiceReference;

namespace WarehouseClient
{
    class Program
    {
        static IEnumerable<string> SplitIntoTokens(string s)
        {
            return s.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
        
        static Hangar HangarWithMinFreePlaces(Hangar[] hangars)
        {
            int min = 41;
            Hangar hangar = null;
            foreach(var h in hangars)
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

        static void ShowStatus(WarehouseServiceClient proxi)
        {
            try
            {
                var hangars = proxi.GetHangars();
                Console.WriteLine("Площадка   Ангар   Количество размещенных контейнеров   Общее количество мест\n");
                foreach (var h in hangars)
                {
                    Console.WriteLine("   {0}\t    {1}\t\t\t{2}\t\t\t\t{3}", h.AreaId, h.Id, h.PlacedContainers, h.MaxContainers);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось загрузить данные сервера. {0}", ex.Message);
            }
        }

        static void Store(WarehouseServiceClient proxi, IEnumerable<string> tokens)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    try
                    {
                        int n = Int32.Parse(tokens.ElementAt(1));
                        if (n <= 0)
                        {
                            throw new Exception(n + ": введите натуральное число");
                        }

                        var areas = proxi.GetAreas();

                        int[] freePlacesOnAreas = new int[areas.Length];
                        int i = 0;
                        foreach (var area in areas)
                        {
                            var hangars = proxi.GetHangarsByArea(area.Id);
                            freePlacesOnAreas[i] = hangars.Sum(h => h.MaxContainers - h.PlacedContainers);
                            i++;
                        }

                        int sumFreePlaces = freePlacesOnAreas.Sum();

                        if (sumFreePlaces >= n)
                        {
                            while (n > 0)
                            {
                                int minValue = freePlacesOnAreas.Where(x => x > 0).Min();
                                int indexMin = Array.IndexOf(freePlacesOnAreas, minValue);
                                var priorityHangars = proxi.GetHangarsByArea(areas.ElementAt(indexMin).Id);

                                while (n > 0 && freePlacesOnAreas[indexMin] != 0)
                                {
                                    Hangar hangar = HangarWithMinFreePlaces(priorityHangars);
                                    int freePlaces = hangar.MaxContainers - hangar.PlacedContainers;
                                    int d = Math.Min(freePlaces, n);
                                    hangar.PlacedContainers += d;
                                    if (proxi.UpdateHangar(hangar))
                                    {
                                        n -= d;
                                        freePlacesOnAreas[indexMin] -= d;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Не удалось разместить контейнеры в ангаре {0}", hangar.Id);
                                        throw new TransactionAbortedException();
                                    }

                                }
                            }
                            if (n == 0)
                            {
                                scope.Complete();
                                Console.WriteLine("Введенные контейнеры успешно размещены на складе");
                            }

                        }
                        else if (sumFreePlaces == 0)
                        {
                            Console.WriteLine("Невозможно разместить указанное количество контейнеров на складе! Склад полностью заполнен.");
                        }
                        else
                        {
                            Console.WriteLine("Невозможно разместить указанное количество контейнеров на складе! Превышено максимальное число свободных контейнеров. Количество свободных контейнеров: {0}", sumFreePlaces);
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Ошибка!{0}: неверный формат аргемента. Введите число.", tokens.ElementAt(1));
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("Ошибка!{0}: данное число не входит в диапазон до 2 147 483 647");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Free(WarehouseServiceClient proxi, IEnumerable<string> tokens)
        {
            try
            {
                byte n = byte.Parse(tokens.ElementAt(1));
                if (n <= 0 || n > 40)
                {
                    throw new Exception("Ошибка!\n" + n + ": данное число не входит в диапазон 1 - 40");
                }

                string id = tokens.ElementAt(2);
                Hangar hangar = proxi.GetHangarById(id);
                if (hangar != null)
                {
                    if (hangar.PlacedContainers >= n)
                    {
                        hangar.PlacedContainers -= n;
                        if (proxi.UpdateHangar(hangar))
                        {
                            Console.WriteLine("{0} контейнеров успешно выгружены из ангара {1}", n, id);
                        }
                        else
                        {
                            Console.WriteLine("Не удалось выгрузить контейнеры");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода! Нельзя выгрузить из ангара {0} число контейнеров, превышающее текущее число контейнеров в ангаре", id);
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка! \n{0}: не найден ангар с данным id", id);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка!\n{0}: неверный формат аргемента. Введите число.", tokens.ElementAt(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        static void Main(string[] args)
        {
            Console.Title = "Терминал удалённого управления складом";

            using (WarehouseServiceClient proxi = new WarehouseServiceClient())
            {
                string input = "";
                while (input.ToUpper() != "EXIT")
                {
                    Console.Write("Введите команду: ");
                    input = Console.ReadLine();

                    var tokens = SplitIntoTokens(input);

                    var command = tokens.FirstOrDefault();

                    if (command == null)
                        continue;

                    switch (command.ToUpper())
                    {
                        case "HELP":
                            {
                                if (tokens.Count() == 1)
                                    Console.WriteLine("Поддерживаемые команды: \nSHOWSTATUS – отображение таблицы загруженности всех ангаров на всех площадках склада\nSTORE <N> – разместить N контейнеров на складе (N – натуральное число)\nFREE <N> <H_ID> – выгрузить N контейнеров из ангара H_ID на складе (N – натуральное число)\nHELP – справка по всем поддерживаемым командам\nEXIT – завершение работы\n");
                                else
                                    Console.WriteLine("Данная команда не поддерживает задание аргументов");
                            }
                            break;

                        case "SHOWSTATUS":
                            {
                                if (tokens.Count() == 1)
                                {
                                    ShowStatus(proxi);
                                }
                                else
                                {
                                    Console.WriteLine("Данная команда не поддерживает задание аргументов");
                                }
                            }
                            break;
                        case "STORE":
                            if (tokens.Count() == 2)
                            {
                                Store(proxi, tokens);
                            }
                            else
                            {
                                Console.WriteLine("Неверно введено число аргументов аргументов");
                            }
                            break;
                        case "FREE":
                            if (tokens.Count() == 3)
                            {
                                Free(proxi, tokens);
                            }
                            else
                            {
                                Console.WriteLine("Неверно введено число аргументов аргументов");
                            }
                            break;
                        case "EXIT":
                            if (tokens.Count() == 1)
                                break;
                            else
                                Console.WriteLine("Данная команда не поддерживает задание аргументов");
                            break;
                        default:
                            Console.WriteLine("Неизвестная команда: {0}", command);
                            break;
                    }
                }
            }
            
        }
    }
}
