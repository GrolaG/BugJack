using System;
using System.Linq;

namespace BlackjackAPP
{
    public class Printer
    {
        static int origRow = 0;
        static int origCol = 0;
        public static void WriteAt(string s, int x, int y)
        {
            Console.SetCursorPosition(origCol + x, origRow + y);
            Console.Write(s);
        }


        public static void Welcome() //приветствие
        {
            Console.WriteLine("Добро пожаловать в виртуальную игру «Blackjack»");
            Console.WriteLine("Введите 0, чтобы выйти из программы или 1, чтобы начать игру");
            while (true)
            {
                var t = Console.ReadKey();
                if (t.KeyChar == 49) break;
                else if (t.KeyChar == 48) Environment.Exit(1);
                else Console.WriteLine("\nВведите корректное значение");
            }
        }
        public static string Introduce() //предложение представиться
        {
            Console.WriteLine("\nПредставьтесь");
            string name = "";
            while (name == "")
            {
                name = Console.ReadLine();
            }
            
            if (name.Length > 20)
            {
                name = new string(name.Take(20).ToArray());
            }
            return name;
        }

        public static void PrintScreenBeforeGameStarts(Player player, int bidsInGame) //Печать только верхней строки состояния
        {
            Console.Clear();
            WriteAt("\t\t\t\tСтавка " + bidsInGame * Account.step + "\t Баланс " + player.account.Value * Account.step, 0, 0);
        }
        public static void PrintOnlyPlayerCardsLineAndPlayerScore(Player player) //Печать информации в режиме игры
        {
            WriteAt("Карты в руке у игрока" + player.VievPlayer(), 0, 2);
            WriteAt("Предварительный итог " + player.Name[0] + ":" + player.Score(), 0, 4);
            Console.WriteLine("\nДальнейшие действия:");
        }
        public static void PrintScreen(Player player, Player dealer, int bidsInGame) //Печать информации в режиме игры
        {
            Console.Clear();
            WriteAt("\t\t\t\tСтавка " + bidsInGame * Account.step + "\t Баланс " + player.account.Value * Account.step, 0, 0);
            WriteAt("Карты в руке у дилера" + dealer.VievDealer(), 0, 1);
            WriteAt("Карты в руке у игрока" + player.VievPlayer(), 0, 2);
            WriteAt("Предварительный итог D:" + dealer.Score(), 0, 3);
            WriteAt("Предварительный итог " + player.Name[0] + ":" + player.Score(), 0, 4);

            Console.WriteLine("\nДальнейшие действия:");
        }

        public static void GameResult(int gameResult)//Вывод результатов 
        {
            Console.WriteLine("\n\t\t\t====================================");
            if (gameResult == 1)
            {
                Console.WriteLine("\n\t\t\tИгрок проиграл");
            }
            else if (gameResult == 2)
            {
                Console.WriteLine("\n\t\t\tИгрок выиграл");
            }
            else
            {
                Console.WriteLine("\n\t\t\tНичья");
            }
            Console.WriteLine("\n\t\t\t====================================");
        }

        public static void ContinueOrExit() //Запрос на продолжение игры
        {
            Console.WriteLine("\n\t\t\t====================================");
            Console.WriteLine("\n\t\t\tКон завершен. \n\t\t\tНачать новый - 1, выйти из игры - 2");
            Console.WriteLine("\n\t\t\t====================================");
            while (true)
            {
                var t = Console.ReadKey();
                if (t.KeyChar == 49)
                {
                    break;
                }
                else if (t.KeyChar == 50)
                {
                    Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine("\nВведите корректное значение - 1 или 2");
                }
            }
        }
    }
}

