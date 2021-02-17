using System;
using System.Net.Http.Headers;
using System.Threading;

namespace lesson11B_13._02._21
{
    class Program
    {
        public static int IdCount = 0;
        public static object locker = new Object();
        static void Main(string[] args)
        {
            TimerCallback changeInClientBalance = new TimerCallback(CheckBalance);
            Timer checkBalance = new Timer(changeInClientBalance, null, 0, 4000);
            bool run = true;
            while (run)
            {
                ChangeColor(ConsoleColor.Blue);
                Console.WriteLine(
                "Добро пожаловать! Выберите операцию:\n" +
                " 1.Insert\n" +
                " 2.Update\n" +
                " 3.Delete\n" +
                " 4.Select\n" +
                " 5.Select All\n" +
                " 6.Exit");
                ChangeColor(ConsoleColor.White);

                char x = char.Parse(Console.ReadLine());
                switch (x)
                {
                    case '1':
                        Thread InsertThread = new Thread(Insert);
                        InsertThread.Start();
                        InsertThread.Join();
                        break;
                    case '2':
                        Thread UpdateThread = new Thread(Update);
                        UpdateThread.Start();
                        UpdateThread.Join();
                        break;
                    case '3':
                        Thread DeleteThread = new Thread(Delete);
                        DeleteThread.Start();
                        DeleteThread.Join();
                        break;
                    case '4':
                        Thread SelectThread = new Thread(Select);
                        SelectThread.Start();
                        SelectThread.Join();
                        break;
                    case '5':
                        Thread SelectAllThread = new Thread(SelectAll);
                        SelectAllThread.Start();
                        SelectAllThread.Join();
                        break;
                    case '6':
                        run = false;
                        break;
                    default:
                        Console.WriteLine(
                            Client.checkbalancebase[0].Balance + " " + Client.clientbase[0].Balance
                            );
                        ChangeColor(ConsoleColor.Red);
                        Console.WriteLine("Неправильная команда!");
                        ChangeColor(ConsoleColor.White);
                        break;
                }
                Console.WriteLine("Нажмите на любую кнопку, чтобы продолжить.");
                Console.ReadKey();
                Console.Clear();
            }

        }

        public static void Insert()
        {
            lock (locker)
            {
                var tempclient = new Client();
                tempclient.Id = IdCount+1;

                Console.Write("Введите Имя:");
                ChangeColor(ConsoleColor.DarkYellow);
                tempclient.FirstName = Console.ReadLine();
                ChangeColor(ConsoleColor.White);

                Console.Write("Введите Фамилию:");
                ChangeColor(ConsoleColor.DarkYellow);
                tempclient.SecondName = Console.ReadLine();
                ChangeColor(ConsoleColor.White);

                Console.Write("Введите Баланс:");
                ChangeColor(ConsoleColor.DarkYellow);
                decimal balance = decimal.Parse(Console.ReadLine());
                tempclient.Balance = balance;
                ChangeColor(ConsoleColor.White);

                Client.clientbase.Add(tempclient);
                Client.checkbalancebase.Add(new Client() { Id = IdCount+1, Balance = balance });
                IdCount += 1;
            }
        }
        public static void Update()
        {
            lock (locker)
            {
                Console.Write("Введите Id:");
                ChangeColor(ConsoleColor.DarkYellow);
                int Id = int.Parse(Console.ReadLine());
                ChangeColor(ConsoleColor.White);

                if (!CheckId(Id))
                {
                    ChangeColor(ConsoleColor.Red);
                    Console.WriteLine($"Клиент с Id {Id} отсутствует");
                }
                else
                {
                    Console.Write("Введите новый Баланс (Нажмите Enter, если не хотите менять):");
                    ChangeColor(ConsoleColor.DarkYellow);
                    string enterBalance = Console.ReadLine();
                    ChangeColor(ConsoleColor.White);

                    Console.Write("Введите новое Имя (Нажмите Enter, если не хотите менять):");
                    ChangeColor(ConsoleColor.DarkYellow);
                    string enterFirst = Console.ReadLine();
                    ChangeColor(ConsoleColor.White);

                    Console.Write("Введите новую Фамилию (Нажмите Enter, если не хотите менять):");
                    ChangeColor(ConsoleColor.DarkYellow);
                    string enterSecond = Console.ReadLine();
                    ChangeColor(ConsoleColor.White);

                    for (int i = 0; i < IdCount; i++)
                    {
                        if (Client.clientbase[i].Id == Id)
                        {
                            if (enterBalance != "")
                            {
                                decimal newBalance = decimal.Parse(enterBalance);
                                Client.clientbase[i].Balance = newBalance;
                            }
                            if (enterFirst != "")
                                Client.clientbase[i].FirstName = enterFirst;
                            if (enterBalance != "")
                                Client.clientbase[i].SecondName = enterSecond;
                        }
                    }
                }
            }
        }
        public static void Delete()
        {
            lock (locker)
            {
                Console.Write("Введите Id:");
                ChangeColor(ConsoleColor.DarkYellow);
                int Id = int.Parse(Console.ReadLine());
                ChangeColor(ConsoleColor.White);

                if (!CheckId(Id))
                {
                    ChangeColor(ConsoleColor.Red);
                    Console.WriteLine($"Клиент с Id {Id} отсутствует");
                }
                else
                {
                    ChangeColor(ConsoleColor.Green);
                    foreach (var client in Client.clientbase)
                    {
                        if (client.Id == Id)
                        {
                            Client.clientbase.Remove(client);
                            ChangeColor(ConsoleColor.Green);
                            Console.WriteLine($"Клиент с Id {Id} успешно удалён!");
                            ChangeColor(ConsoleColor.White);
                            break;
                        }
                    }
                }
            }
        }
        public static void Select()
        {
            lock (locker)
            {
                Console.Write("Введите Id:");
                ChangeColor(ConsoleColor.DarkYellow);
                int Id = int.Parse(Console.ReadLine());
                ChangeColor(ConsoleColor.White);

                if (!CheckId(Id))
                {
                    ChangeColor(ConsoleColor.Red);
                    Console.WriteLine($"Клиент с Id {Id} отсутствует");
                }
                else
                {
                    ChangeColor(ConsoleColor.Green);
                    foreach (var client in Client.clientbase)
                    {
                        if (client.Id == Id)
                        {
                            Console.WriteLine(
                            $"Id:{client.Id}\n" +
                            $"Имя:{client.FirstName}\n" +
                            $"Фамилия:{client.SecondName}\n" +
                            $"Баланс:{client.Balance:C2}"
                            );
                        }
                    }
                }
                ChangeColor(ConsoleColor.White);
            }
        }
        public static void SelectAll()
        {
            lock (locker)
            {
                ChangeColor(ConsoleColor.DarkYellow);
                Console.WriteLine("Все клиенты:");
                ChangeColor(ConsoleColor.White);
                ChangeColor(ConsoleColor.Green);
                foreach (var client in Client.clientbase)
                {
                    Console.WriteLine(
                    $"Id:{client.Id}\n" +
                    $"Имя:{client.FirstName}\n" +
                    $"Фамилия:{client.SecondName}\n" +
                    $"Баланс:{client.Balance:C2}\n"
                    );
                }
                ChangeColor(ConsoleColor.White);
            }
        }
        public static void CheckBalance(object obj)
        {
            string sign;

            if (IdCount > 0)
            {
                for (int i = 0; i <= IdCount-1; i++)
                {
                    if (Client.clientbase[i].Balance != Client.checkbalancebase[i].Balance)
                    {
                        var newBalance = Client.clientbase[i].Balance;
                        var oldBalance = Client.checkbalancebase[i].Balance;
                        
                        if (newBalance - oldBalance > 0)
                            sign = "+";
                        else
                            sign = "-";

                        ChangeColor(ConsoleColor.Yellow);
                        Console.Write(
                            $"Id:{i},\n" +
                            $"Старый баланс:{oldBalance:C2},\n" +
                            $"Новый баланс:{newBalance:C2},\n" +
                            $"Разница:");

                        if (sign == "+")
                            ChangeColor(ConsoleColor.Green);
                        else
                            ChangeColor(ConsoleColor.Red);

                        Console.WriteLine($"{sign} {Math.Abs(oldBalance - newBalance):C2}");
                        Client.checkbalancebase[i].Balance = Client.clientbase[i].Balance;
                        break;
                    }
                }
            }
        }
        public static bool CheckId(int Id)
        {
            foreach (var client in Client.clientbase)
            {
                if (client.Id == Id)
                {
                    return true;
                }
            }
            return false;
        }

        public static void ChangeColor(ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
        }
    }
}
