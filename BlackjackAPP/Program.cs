using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackAPP
{
    class Program
    {
        protected static int origRow;
        protected static int origCol;
        protected static void WriteAt(string s, int x, int y)
        {
            Console.SetCursorPosition(origCol + x, origRow + y);
            Console.Write(s);
        }

        static Deck Deck = new Deck();
        static Player player = new Player(false);
        static Player dealer = new Player(true);
        static int bidsInGame = 0;

        static void Main()
        {
            Console.Clear();
            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;
            Console.WriteLine("Добро пожаловать в виртуальную игру «Blackjack»");
            Console.WriteLine("Введите 0, чтобы выйти из программы или 1, чтобы начать игру");
            while (true)
            {
                var t = Console.ReadKey();
                if (t.KeyChar == 49) break;
                else if (t.KeyChar == 48) Environment.Exit(1);
                else Console.WriteLine("\nВведите корректное значение");
            }
            InitName();

            //Небольшой костыль
            Console.Clear();
            WriteAt("\t\t\t\tСтавка " + bidsInGame * Account.step + "\t Баланс " + player.account.Value, 0, 0);
            Game();
            Console.WriteLine("\nТы проиграл))))");
            player.account.Value = 10;
            Main();
        }
        static void InitName()
        {
            if (player.Name == null)
            {
                Console.WriteLine("\nПредставьтесь");
                string name = Console.ReadLine();
                if (name.Length > 20)
                {
                    name = new string(name.Take(20).ToArray());
                }
                player.Name = name;
            }
        }

        static void Printer()
        {
            Console.Clear();
            WriteAt("\t\t\t\tСтавка " + bidsInGame * Account.step + "\t Баланс " + player.account.Value * Account.step, 0, 0);
            WriteAt("Карты в руке у дилера" + dealer.VievDealer(), 0, 1);
            WriteAt("Карты в руке у игрока" + player.VievPlayer(), 0, 2);
            WriteAt("Предварительный итог D:" + dealer.Score(), 0, 3);
            WriteAt("Предварительный итог " + player.Name[0] + ":" + player.Score(), 0, 4);

            Console.WriteLine("\nДальнейшие действия:");
        }
        //Игра

        public static int MakeBid()
        {
            int bid = 0;
            while (true)
            {
                WriteAt("\t\t\t\tСтавка " + bidsInGame * Account.step + "\t Баланс " + player.account.Value * Account.step, 0, 0);
                Console.WriteLine("\nВведите желаемое количество минимальных ставок для начала игры. Минимальная ставка – 10 у.е.");
                int.TryParse(Console.ReadLine(), out bid);
                if (player.account.Withdraw(bid) && (bid > 0)) break;
                else Console.WriteLine("Недостаточно средств или ставка не сделана");
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
            }
            return bid;
        }
        public static void Reset()
        {
            bidsInGame = 0;
            Deck.BackToDeck(player.NewRound());
            Deck.BackToDeck(dealer.NewRound());
            dealer.hideCards = true;
            Console.Clear();
        }

        public static void Game()
        {

            while (player.account.Value > 0)
            {
                //Делает ставку
                bidsInGame += MakeBid();
                //Раздача карт и вывод в консоль
                {
                    dealer.AddCard(Deck.GetCard());
                    dealer.AddCard(Deck.GetCard());
                    player.AddCard(Deck.GetCard());
                    player.AddCard(Deck.GetCard());
                    Printer();
                }

                //Игрок набирает карты
                bool take = true;
                while (take == true)
                {
                    if (player.Score() == 21) take = false;
                    else
                    {
                        if (player.Score() > 21) take = false;
                        else
                        {
                            Console.WriteLine("Какое действие совершить? 1 - Взять карту, 2 - Удвоить ставку и взять последнюю карту, 3 - Не брать карту");
                            while (take == true)
                            {
                                var t = Console.ReadKey();
                                if (t.KeyChar == 49)
                                {
                                    player.AddCard(Deck.GetCard());
                                    Printer();
                                    break;
                                }
                                if (t.KeyChar == 50)
                                {
                                    if (player.account.Value >= bidsInGame)
                                    {
                                        player.account.Withdraw(bidsInGame);
                                        bidsInGame *= 2;
                                        player.AddCard(Deck.GetCard());
                                        Printer();
                                        take = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nНедостаточно средств. У вас на счету {0}, а необходимо {1}", player.account.Value, bidsInGame);
                                    }
                                    break;
                                }
                                if (t.KeyChar == 51)
                                {
                                    Printer();
                                    take = false;
                                }
                                else Console.WriteLine("\nВведите корректное значение - 1, 2 или 3");
                            }
                        }
                    }
                }

                //Диллер действует по своему шаблону из табл. 1
                dealer.hideCards = false;
                string msg = "\n\t\t\t";
                //Логика дилера из таблицы
                while (take == false)
                {
                    if (player.Score() > 21)
                    {
                        msg += "Дилер не взял карт, игрок проиграл ставку.";
                        take = true;
                    }
                    else
                    {
                        if (player.Score() == 21)
                        {
                            if (dealer.Score() == 21)
                            {
                                msg += "У дилера тоже BlackJack! Ничья.";
                                player.account.Deposit(bidsInGame);
                                take = true;
                            }
                            else
                            {
                                msg += "Игрок выигрывает этот кон и получает: " + bidsInGame * 2 * Account.step+ " у.е.";
                                player.account.Deposit(bidsInGame * 2);
                                take = true;
                            }

                        }
                        else if (dealer.Score() == player.Score() && player.Score() > 15)
                        {
                            msg += "Дилер не взял карт, ничья. Ставка возвращается.";
                            take = true;
                            player.account.Deposit(bidsInGame);
                        }
                        else if (dealer.Score() > player.Score() && dealer.Score() < 22)
                        {
                            msg += "Дилер не взял карт, игрок проиграл ставку.";
                            take = true;
                        }
                        else if (dealer.Score() > 21)
                        {
                            msg += "Игрок выигрывает этот кон и получает: " + bidsInGame * 2 * Account.step + " у.е.";
                            player.account.Deposit(bidsInGame * 2);
                            Printer();
                            take = true;
                        }
                        else
                        {
                            Console.WriteLine("Дилер берет карту...");
                            System.Threading.Thread.Sleep(1000);
                            dealer.AddCard(Deck.GetCard());
                            Printer();
                        }
                    }
                }

                //Дилер закончил добор карт.
                Printer();
                Console.WriteLine("\n\t\t\t====================================");
                Console.WriteLine("\n\t\t\tКон завершен. "+ msg +"\n\t\t\tНачать новый - 1, выйти из игры - 2");
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

                if (player.account.Value > BestScore.GetBest(player.Name) && (player.account.Value > 10 ))
                {
                    BestScore.SetBest(player.Name, player.account.Value);
                }
                Reset(); //Сброс данных
            }

        }
    }

}
