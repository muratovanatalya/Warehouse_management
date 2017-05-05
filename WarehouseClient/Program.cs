using System;
using System.Collections.Generic;
using System.Linq;
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
        
        static void ShowStatus(WarehouseServiceClient proxi)
        {
            try
            {
                var hangars = proxi.GetAllHangars();
                Console.WriteLine("Площадка   Ангар   Количество размещенных контейнеров   Общее количество мест\n");
                foreach (var h in hangars)
                {
                    Console.WriteLine("   {0}\t    {1}\t\t\t{2}\t\t\t\t{3}", h.AreaId, h.Id, h.PlacedContainers, h.MaxContainers);
                }
            }
            catch (CommunicationException commException)
            {
                Console.WriteLine("CommunicationException: {0}", commException.Message);
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
                proxi.Store(tokens.ElementAt(1));
                Console.WriteLine("{0} контейнеров успешно размещены на складе", tokens.ElementAt(1));
            }
            catch (FaultException exception)
            {
                Console.WriteLine("FaultException: {0}", exception.Message);
            }
            catch (CommunicationException commException)
            {
                Console.WriteLine("CommunicationException: {0}", commException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Free(WarehouseServiceClient proxi, IEnumerable<string> tokens)
        {
            try
            {
                proxi.Free(tokens.ElementAt(1), tokens.ElementAt(2));
                Console.WriteLine("{0} контейнеров успешно выгружены из ангара {1}", tokens.ElementAt(1), tokens.ElementAt(2)); 
            }
            catch (FaultException exception)
            {
                Console.WriteLine("FaultException: {0}", exception.Message);
            }
            catch (CommunicationException commException)
            {
                Console.WriteLine("CommunicationException: {0}", commException.Message);
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
