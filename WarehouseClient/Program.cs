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

        static void Main(string[] args)
        {
            Console.Title = "Терминал удалённого управления складом";

            WarehouseServiceClient proxi = new WarehouseServiceClient();

            string input = "";
            while (input.ToUpper() != "EXIT")
            {
                Console.Write("Введите команду ");
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
                               // try
                           //     {
                                    var hangars = proxi.GetHangars();
                                    foreach (var h in hangars)
                                    {
                                        Console.WriteLine("Площадка: {0}", h.Area_IdArea);
                                        Console.WriteLine("Ангар {0}, количество свободных мест - {1}\n", h.IdHangar, h.FreePlaces);
                                    }
                             //   }
                           //     catch (Exception ex)
                            //    {
                            //        Console.WriteLine("Не удалось загрузить данные сервера. {0}", ex.Message);
                            //    }
                            }
                            else
                            {
                                Console.WriteLine("Данная команда не поддерживает задание аргументов");
                            }  
                        }
                        break;
                    case "STORE":
                        break;
                    case "FREE":
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

            proxi.Close();
        }
    }
}
