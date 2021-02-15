using System;
using System.Threading;

namespace lesson11B_13._02._21
{
    class Program
    {
        public static int IdCount = 0;
        public static object locker = new Object();
        static void Main(string[] args)
        {
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
                IdCount += 1;
                tempclient.Id = IdCount;

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

                    foreach (var client in Client.clientbase)
                    {
                        if (client.Id == Id)
                        {
                            if (enterBalance != "")
                            {
                                decimal newBalance = decimal.Parse(enterBalance);
                                Balance balance = new Balance
                                {
                                    Id = Id,
                                    OldBalance = client.Balance,
                                    NewBalance = newBalance
                                };
                                client.Balance = newBalance;
                                TimerCallback changeInClientBalance = new TimerCallback(CheckBalance);
                                Timer checkBalance = new Timer(changeInClientBalance, balance, 0, 0);
                            }
                            if (enterFirst != "")
                                client.FirstName = enterFirst;
                            if (enterBalance != "")
                                client.SecondName = enterSecond;

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
            Balance balance = (Balance)obj;
            string sign;
            decimal oldBalance = balance.OldBalance;
            decimal newBalance = balance.NewBalance;

            if (oldBalance != newBalance)
            {
                if (newBalance - oldBalance > 0)
                    sign = "+";
                else
                    sign = "-";

                ChangeColor(ConsoleColor.Yellow);
                Console.Write(
                    $"Id:{balance.Id},\n" +
                    $"Старый баланс:{oldBalance:C2},\n" +
                    $"Новый баланс:{newBalance:C2},\n" +
                    $"Разница:");

                if (sign == "+")
                    ChangeColor(ConsoleColor.Green);
                else
                    ChangeColor(ConsoleColor.Red);

                Console.WriteLine($"{sign} {Math.Abs(oldBalance - newBalance):C2}");
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
